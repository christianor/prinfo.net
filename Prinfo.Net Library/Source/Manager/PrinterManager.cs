using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// PrinterManager Klasse. Hauptobjekt dieser Klassenbibliothek, verwaltet und prüft Drucker
    /// </summary>
    public class PrinterManager : IEnumerable<Printer>
    {

        private PrinterDatabase _printerDatabase;
        /// <summary>
        /// Instanz der Schnittstelle zur Datenbank die die Druckerobjekte enthält
        /// </summary>
        public PrinterDatabase PrinterDatabase
        {
            get
            {
                // lazy initializiation
                if (_printerDatabase == null)
                {
                    _printerDatabase = new PrinterDatabase();
                    if (!_printerDatabase.PerformIntegrityCheck())
                    {
                        Logger.Log(GlobalizationHelper.LibraryResource.GetString("printer_db_integrity_failed"), LogType.Error);
                        _printerDatabase.Initialize();

                        if (!_printerDatabase.PerformIntegrityCheck())
                            throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("printer_db_inizialize_failed"));
                    }
                }

                return _printerDatabase;
            }
            set
            {
                _printerDatabase = value;
            }
        }

        private ArchivDatabase _archivDatabase;
        /// <summary>
        /// Instanz der Schnittstelle zur Datenbank die die Archivdaten enthält
        /// </summary>
        public ArchivDatabase ArchivDatabase
        {
            get
            {
                // lazy initializiation
                if (_archivDatabase == null)
                {
                    _archivDatabase = new ArchivDatabase();
                    if (!_archivDatabase.PerformIntegrityCheck())
                    {
                        Logger.Log(GlobalizationHelper.LibraryResource.GetString("archiv_db_integrity_failed"), LogType.Error);
                        _archivDatabase.Initialize();

                        if (!_archivDatabase.PerformIntegrityCheck())
                            throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("archiv_db_initialize_failed"));
                    }
                }

                return _archivDatabase;
            }
            set
            {
                _archivDatabase = value;
            }
        }

        private int _pingTimeout = 1400;
        /// <summary>
        /// Der Timeout der für Pingabfragen toleriert wird
        /// </summary>
        public int PingTimeout
        {
            set
            {
                _pingTimeout = value;
            }
            get
            {
                return _pingTimeout;
            }
        }

        private int _snmpTimeout = 1400;
        /// <summary>
        /// Der Timeout der für SNMP-Abfragen toleriert wird
        /// </summary>
        public int SnmpTimeout
        {
            set
            {
                _snmpTimeout = value;
            }
            get
            {
                return _snmpTimeout;
            }
        }

        private int _snmpRetries = 3;
        /// <summary>
        /// Die Anzahl der Wiederholversuche bei SNMP-Abfragen, kann je nach Netzwerkqualität (z.B. Hub statt Switch)
        /// einen höheren Wert erfordern da es öfter zu Kollisionen kommt und das darunterliegende UDP-Protokoll
        /// keine Sicherheit auf erhalt der Daten bereitstellt.
        /// </summary>
        public int SnmpRetries
        {
            set
            {
                _snmpRetries = value;
            }
            get
            {
                return _snmpRetries;
            }
        }

        /// <summary>
        /// Wird abgefeuert sobald ein Drucker überprüft wurde
        /// </summary>
        public event EventHandler<PrinterEventArgs> OnPrinterChecked;

        /// <summary>
        /// Wird abgefeuert sobald die enthaltene DruckerListe von "PollPrinterList" oder 
        /// "ParallelPollPrinterList" überprüft wurde
        /// </summary>
        public event EventHandler<EventArgs> OnPrinterListChecked;


        /// <summary>
        /// Wird von "PollPrinter" oder von "PollPrinterList" gefeuert falls der 
        /// überprüfte Drucker offline ist
        /// </summary>
        public event EventHandler<PrinterEventArgs> OnPrinterOffline;

        /// <summary>
        /// Wird von "PollPrinter" oder von "PollPrinterList" gefeuert falls für einen spezifischen
        /// verbrauchsgegenstand in der Datenbank ein Schwellwert eingetragen wurde.
        /// <remarks>Wird nicht von einem globalen (in der XML konfigurierten) schwellwerteintrag gefeuert.
        /// Grund dafür ist, dass sonst jedesmal die Konfiugrationsdatei geladen werden sollte, dies allerdings
        /// für zusätzliche Latenz sorgen würde</remarks>
        /// </summary>
        public event EventHandler<SupplyEventArgs> OnSupplyLow;


        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public PrinterManager()
        {
            PrinterList = new List<Printer>();
        }

        /// <summary>
        /// Erstellt ein Druckermanager Objekt mit anderem Daten- und Logvaterverzeichnis
        /// </summary>
        /// <param name="dataAndLogParentDirectory">Der Pfad zum Verzeichnis</param>
        public PrinterManager(string dataAndLogParentDirectory) : this()
        {
            DirectoryController.DataDirectoryLocation = dataAndLogParentDirectory + @"\data";
            DirectoryController.LogDirectoryLocation = dataAndLogParentDirectory + @"\logs";
        }

        /// <summary>
        /// Drucker Enumerator
        /// </summary>
        /// <returns>Den Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (Printer printer in PrinterList)
                yield return printer;
        }

        /// <summary>
        /// Drucker Enumerator
        /// </summary>
        /// <returns>Den typisierten Enumerator</returns>
        IEnumerator<Printer> IEnumerable<Printer>.GetEnumerator()
        {
            foreach (Printer printer in PrinterList)
                yield return printer;
        }

        /// <summary>
        /// Lädt die Druckerdatenbank in die im Druckermanager enthaltene Druckerliste
        /// </summary>
        public List<Printer> LoadPrinterList()
        {
            // lock the printerList, because it shouldn`t be re-loadet while PollPrinterList is running!
            // the lock helps
            lock (PrinterList)
                return PrinterList = PrinterDatabase.GetPrinterList();

        }

        /// <summary>
        /// Holt anhand der übergeben OID den SNMP-Wert vom angegeben Host, gibt leeren String zurück falls
        /// der Wert nicht verfügbar war
        /// </summary>
        /// <param name="ipAddress">Die Adresse des Hosts</param>
        /// <param name="oid">Die SNMP OID</param>
        /// <param name="log">Gibt an ob geloggt werden soll</param>
        /// <returns>Den SNMP Wert der an der angegebenen OID verfügbar war</returns>
        private string GetSNMPValue(IPAddress ipAddress, string oid, bool log = false)
        {
            Manager snmpManager = new Manager();
            snmpManager.Timeout = SnmpTimeout;
            int i = 1;

            try
            {
                while (true)
                {
                    try
                    {
                        List<Variable> snmpResult = (List<Variable>)snmpManager.Get(ipAddress, "public", new List<Variable> { new Variable(new ObjectIdentifier(oid)) });
                        return snmpResult[0].Data.ToString();
                    }
                    catch (Lextm.SharpSnmpLib.Messaging.TimeoutException exce)
                    {
                        if (log)
                            Logger.Log(String.Format("{0}: SNMP Timeout " + i, ipAddress.ToString()), LogType.Printer);

                        if (i >= SnmpRetries)
                            throw exce;
                    }

                    i++;
                }
            }
            catch (OperationException)
            {
                return "";
            }
        }

        /// <summary>
        /// Überprüft den gegeben Drucker über das Ping-Protokoll und SNMP, die bezogenen Werte und Stati werden im
        /// übergebenen Druckerobjekt gespeichert.
        /// Trägt sorge das die Verbrauchsteile mit ihren Id`s beibehalten werden falls sie sich nicht verändert haben
        /// </summary>
        /// <param name="printer">Der Drucker welcher überprüft werden soll</param>
        /// <param name="log">Gibt an ob geloggt werden soll</param>
        public void PollPrinter(Printer printer, bool log = false)
        {
            // save the printers old data (deep copy)
            // muss so gemacht werden da die referenz (printer) nicht während dem durchlauf
            // in einer foreach schleife geändert werden darf
            Printer oldPrinter = new Printer(printer);

            using (Ping p = new Ping())
            {
                printer.LastCheck = DateTime.Now.ToString();
                //
                // try to ping the printer and to get the snmp data
                //
                try
                {
                    PingReply pingReply = p.Send(printer.HostName, PingTimeout);

                    if (pingReply.Status == IPStatus.Success)
                    {
                        if (log)
                            Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("host_reachable"), printer.HostName), LogType.Printer);
                        //
                        // clear all the printer`s dynamic values if the printer is pingable
                        //
                        printer.Clear();

                        printer.Pingable = true;
                        IPAddress resolvedIp = pingReply.Address;

                        //
                        // first step: try to get model, manufacturer
                        // model
                        printer.Model = GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.25.3.2.1.3.1", log);
                        // manufacturer
                        printer.Manufacturer = GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.43.9.2.1.8.1.1", log);
                        
                        
                        // System Description
                        printer.Description = GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.1.1.0");
                        // kyocera fix
                        if (string.IsNullOrWhiteSpace(printer.Manufacturer))
                            printer.Manufacturer = printer.Description;

                        printer.SysContact = GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.1.4.0");
                        printer.SysLocation = GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.1.6.0");
                        printer.UpTime = GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.1.3.0");
                        //
                        // get page count and page count color
                        //
                        try
                        {
                            //
                            // page count
                            //
                            printer.PageCount = int.Parse(GetSNMPValue(resolvedIp, ".1.3.6.1.2.1.43.10.2.1.4.1.1", log));
                        }
                        catch (FormatException) { }


                        try
                        {
                            //
                            // page count color
                            //
                            printer.PageCountColor = int.Parse(GetSNMPValue(resolvedIp, ".1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.7.0", log));
                        }
                        catch (FormatException) { }

                        DiscoverSupplies(printer, resolvedIp, oldPrinter.Supplies, log);

                    }
                    else
                    {
                        printer.Pingable = false;

                        if (log)
                            Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("host_not_reachable"), printer.HostName), LogType.Printer);
                    }
                }
                catch (PingException)
                {
                    printer.Pingable = false;

                    if (log)
                        Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("host_not_reachable"), printer.HostName), LogType.Printer);
                }
                catch (SocketException)
                {
                    if (log)
                        Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("host_not_listening"), printer.HostName), LogType.Printer);
                }
                // after SnmpRetries the timeout is thrown (by getSupply or getSnmpValue) and catched here
                catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
                {
                    //
                    // copy the field values not the whole printer, this could let you run into errors, because if the printer reference comes
                    // from a foreach loop, you can`t change the printer object reference:
                    // Foreach iteration variable: 
                    // The iteration variable corresponds to a read-only local variable with a scope that extends over the embedded statement.
                    //
                    printer.Description = oldPrinter.Description;
                    printer.Manufacturer = oldPrinter.Manufacturer;
                    printer.Model = oldPrinter.Model;
                    printer.PageCount = oldPrinter.PageCount;
                    printer.PageCountColor = oldPrinter.PageCountColor;
                    printer.Supplies = oldPrinter.Supplies;
                    printer.UpTime = oldPrinter.UpTime;
                    printer.SysLocation = oldPrinter.SysLocation;
                    printer.SysContact = oldPrinter.SysContact;

                    if (log)
                        Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("snmp_timeout"), printer.HostName), LogType.Printer);
                }
            }

            if (!printer.Pingable && OnPrinterOffline != null)
                OnPrinterOffline(this, new PrinterEventArgs(printer));

            CheckIfAnySupplyIsLow(printer);

            //
            // trigger the PrinterChecked event
            // muss unbedingt als aller letztes event gefeuert werden, da es ein indikator dafür ist das 
            // andere events bereits abgearbeitet wurden
            //
            if (OnPrinterChecked != null)
                OnPrinterChecked(this, new PrinterEventArgs(printer));
        }

        /// <summary>
        /// Überprüft ob ein Verbrauchsteil den Schwellwert erreicht hat und feuert das Event ab.
        /// <remarks>Achtung, das Supply wird nicht als benachrichtigt markiert,
        /// dies muss der entgegennehmende selbst überprüfen</remarks>
        /// </summary>
        /// <param name="printer"></param>
        private void CheckIfAnySupplyIsLow(Printer printer)
        {
            foreach (var supply in printer)
            {
                string description = supply.Description;
                double value = supply.Value;

                if (supply.NotificationValue >= supply.Value)
                    if (OnSupplyLow != null)
                        OnSupplyLow(this, new SupplyEventArgs(printer, supply));

            }
        }

        /// <summary>
        /// Versucht das angegebe Verbrauchsteil und dessen Werte über die Supplynummer zu beziehen (1-x),
        /// </summary>
        /// <param name="printer">Das Drucker Objekt <remarks>Wird aus Gründen des loggings mitübergeben</remarks></param>
        /// <param name="resolvedIp">Die IP-Adresse des Hosts</param>
        /// <param name="supplyNumber">Die Supplynummer zwischen 1-n</param>
        /// <param name="log">Gibt an ob geloggt werden soll</param>
        /// <returns>Das bezogene Verbrauchsteil</returns>
        private Supply GetSupply(Printer printer, IPAddress resolvedIp , int supplyNumber, bool log = false)
        {
            Manager snmpManager = new Manager();
            snmpManager.Timeout = SnmpTimeout;
            double procentualValue;
            int i = 1;

            while(true)
            {
                try
                {
                    List<Variable> supplyResult = (List<Variable>)snmpManager.Get(resolvedIp, "public",
                        new List<Variable> { 
                                new Variable(new ObjectIdentifier(".1.3.6.1.2.1.43.11.1.1.8.1."+supplyNumber)),     //complete value
                                new Variable(new ObjectIdentifier(".1.3.6.1.2.1.43.11.1.1.9.1."+supplyNumber)),     //actual value
                                new Variable(new ObjectIdentifier(".1.3.6.1.2.1.43.11.1.1.6.1."+supplyNumber))      //supply description
                            });

                    //value smaller than 0 could mean that the cartridge has been recycled or is defect
                    try
                    {
                        if (double.Parse(supplyResult[1].Data.ToString()) <= 0 || double.Parse(supplyResult[0].Data.ToString()) <= 0)
                            procentualValue = 0;
                        else
                        {
                            procentualValue = Math.Round(double.Parse(supplyResult[1].Data.ToString()) / double.Parse(supplyResult[0].Data.ToString()) * 100, 0);
                        }
                    }
                    catch (FormatException)
                    {
                        procentualValue = 0;
                    }

                    if(log)
                        Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("supply_state"), printer.HostName, supplyResult[2].Data.ToString(), procentualValue, supplyResult[1].Data.ToString(), supplyResult[0].Data.ToString(), "actual value: .1.3.6.1.2.1.43.11.1.1.9.1." + supplyNumber + " complete value: .1.3.6.1.2.1.43.11.1.1.8.1." + supplyNumber + " supply description: .1.3.6.1.2.1.43.11.1.1.6.1." + supplyNumber), LogType.Printer);
                    return new Supply() { Description = supplyResult[2].Data.ToString(), Value = procentualValue };
                }
                catch (Lextm.SharpSnmpLib.Messaging.TimeoutException exce)
                {
                    if(log)
                        Logger.Log(String.Format("{0}: SNMP Timeout " + i, printer.HostName), LogType.Printer);
                    if (i >= SnmpRetries)
                        throw exce;
                }
                
                i++;
            }
        }

        /// <summary>
        /// "Entdeckt" die Verbrauchsteile des angegebenen Hosts, achtet darauf auf die Integrität der Verbrauchsteile der bereits 
        /// vorher erfassten Verbrauchsteile.
        /// Dabei wird versucht die Id`s der Verbrauchsteile beizubehalten
        /// </summary>
        /// <param name="printer">Der zu Überprüfende Drucker, wird aus Logging-Gründen Übergeben</param>
        /// <param name="resolvedIp">Die IP des Hosts</param>
        /// <param name="log">Gibt an ob geloggt werden soll</param>
        /// <param name="suppliesBeforeClearing">Die Supplies des Hosts in dem "Ursprungszustand" aus der Datenbank</param>
        private void DiscoverSupplies(Printer printer, IPAddress resolvedIp, List<Supply> suppliesBeforeClearing, bool log = false)
        {
            //
            //try to get up to 14 supplies
            //
            for (int i = 1; i < 15; i++)
            {
                try
                {
                    printer.Supplies.Add(GetSupply(printer, resolvedIp, i, log));
                }
                catch (OperationException oe)
                {
                    if(log)
                        //Supply couldn`t be detected by snmp, break out of the loop because it is the last supply
                        Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("snmp_error") + i + " Exception: " + oe.Message + " ", printer.HostName), LogType.Printer);
                    break;
                }
            }

            if (suppliesBeforeClearing.Count == printer.Supplies.Count)
            {
                //
                //iterate through every supply trying to restore its database values
                //
                int counter = 0;
                bool integrityCheckFailed = false;

                foreach (Supply supply in printer.Supplies)
                {
                    if (suppliesBeforeClearing[counter].Description != supply.Description)
                    {
                        integrityCheckFailed = true;
                        break;
                    }

                    counter++;
                }

                counter = 0;

                if (!integrityCheckFailed)
                    foreach (Supply supply in printer.Supplies)
                    {

                        supply.Id = suppliesBeforeClearing[counter].Id;
                        supply.GlobalAlertTriggered = suppliesBeforeClearing[counter].GlobalAlertTriggered;
                        supply.Notified = suppliesBeforeClearing[counter].Notified;
                        supply.NotificationValue = suppliesBeforeClearing[counter].NotificationValue;
                        supply.NotifyWhenLow = suppliesBeforeClearing[counter].NotifyWhenLow;

                        counter++;
                    }
                else
                    if(log)
                        Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("supply_integrity_failed"), printer.HostName), LogType.Printer);
            }
            else
                if(log)
                    Logger.Log(String.Format(GlobalizationHelper.LibraryResource.GetString("supply_integrity_failed"), printer.HostName), LogType.Printer);
        }

        /// <summary>
        /// Prüft die im Druckermanager vorhanden Druckerliste anhand der "PollPrinter"-Methode
        /// </summary>
        /// <param name="log">Gibt an ob geloggt werden soll</param>
        public void PollPrinterList(bool log = false)
        {
            lock (PrinterList)
            {
                foreach (Printer p in PrinterList)
                    PollPrinter(p, log);

                if (OnPrinterListChecked != null)
                    OnPrinterListChecked(this, null);
            }
        }

        /// <summary>
        /// Überprüft parallel die Drucker
        /// <remarks>
        /// Bei OnPrinterChecked Ereigniss-Handlern sollte bei Verwendung der parallelen Methode die Drucker zu 
        /// überprüfen darauf geachtet werden das der Zugriff auf "shared ressources" serialisiert wird
        /// </remarks>
        /// </summary>
        /// <param name="log">Gibt an ob geloggt werden soll</param>
        public void ParallelPollPrinterList(bool log = false)
        {
            lock(PrinterList)
            {
                ParallelLoopResult result = Parallel.ForEach<Printer>(PrinterList, x => { PollPrinter(x, log); });

                while (!result.IsCompleted)
                    Thread.Sleep(10);

                if (OnPrinterListChecked != null)
                    OnPrinterListChecked(this, null);
            }
        }

        /// <summary>
        /// Prüft ob ein angegebener Host ein Drucker sein könnte, muss nicht wahr sein
        /// </summary>
        /// <param name="ip">Die Ip des Druckers</param>
        /// <returns>Wahr wenn der angegebene Host wirklich ein Drucker ist</returns>
        public bool IsAPrinter(string ip)
        {
            var manager = new Manager();
            manager.Timeout = SnmpTimeout;

            try
            {
                manager.Get(IPAddress.Parse(ip), "public",
                    new List<Variable> { 
                    new Variable(
                        new ObjectIdentifier(".1.3.6.1.2.1.1.1.0")) });
            }
            catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
            {
                return false;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (OperationException)
            {
                return true;
            }

            return true;
        }

        /// <summary>
        /// Gibt die Anzahl der Drucker als globalisierte Zeichenkette zurück
        /// </summary>
        /// <returns>Die Zeichenkette</returns>
        public override string ToString()
        {
            return String.Format(GlobalizationHelper.LibraryResource.GetString("manager_holds"), PrinterList.Count);
        }

        public List<Printer> PrinterList { get; set; }
        
    }

}
