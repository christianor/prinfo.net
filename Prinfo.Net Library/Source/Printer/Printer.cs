using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;


namespace com.monitoring.prinfo
{
    /// <summary>
    /// Drucker Klasse
    /// </summary>
    public class Printer : IEnumerable<Supply>
    {
        /// <summary>
        /// id als guid
        /// </summary>
        public string Id { set; get; }

        private string _hostName;
        /// <summary>
        /// Der Hostname oder die Ip des Druckers
        /// </summary>
        public string HostName
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("no_hostname"));

                _hostName = value;
            }
            get
            {
                return _hostName;
            }
        }

        /// <summary>
        /// Zeitpunkt der letzten Überprüfung
        /// </summary>
        public string LastCheck { set; get; }

        /// <summary>
        /// Herstellerbezeichnung
        /// </summary>
        public string Manufacturer { set; get; }
        /// <summary>
        /// Gibt an ob der Drucker über das Ping-Protokoll erreichbar ist
        /// </summary>
        public bool Pingable { set; get; }
        /// <summary>
        /// Das Model des Druckers
        /// </summary>
        public string Model { set; get; }
        /// <summary>
        /// Die genaue Werksbeschreibung des Druckers
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// Die Uptime in Ticks des Druckers
        /// </summary>
        public string UpTime { set; get; }
        /// <summary>
        /// Der eingetragene Standort des Druckers
        /// </summary>
        public string SysLocation { set; get; }
        /// <summary>
        /// Die eingetragene Kontaktperson des Druckers
        /// </summary>
        public string SysContact { set; get; }

        
        private int _pageCount = -1;
        /// <summary>
        /// Mono Seitenzähler
        /// <remarks>PageCount ist -1 wenn der Zählerstand nicht verfügbar ist</remarks>
        /// </summary>
        public int PageCount 
        {
            set
            {
                _pageCount = value;
            }
            get
            {
                return _pageCount;
            }
        }
       
        private int _pageCountColor = -1;

        /// <summary>
        /// PageCountColor ist -1 wenn der Zählerstand nicht verfügbar ist
        /// </summary>
        public int PageCountColor 
        {
            set
            {
                _pageCountColor = value;
            }
            get
            {
                return _pageCountColor;
            }
        }


        private List<Supply> _supplies =  new List<Supply>();
        /// <summary>
        /// Eine Liste der Verbrauchsteile des Druckers, werden in der Regel über 
        /// den DruckerManager zugewiesen
        /// </summary>
        public List<Supply> Supplies 
        {
            set
            {
                _supplies = value;
            }
            get
            {
                return _supplies;
            }
        }

        /// <summary>
        /// Leerer Standardkonstruktor
        /// </summary>
        public Printer() { }


        /// <summary>
        /// Kopierkonstruktor, kopiert die Werte des Übergebenen Drucker-Objekts in das neu erstellte
        /// </summary>
        /// <param name="printer">Der Drucker dessen Werte Kopiert werden sollen</param>
        public Printer(Printer printer)
        {

            this.HostName = printer.HostName;
            this.Id = printer.Id;
            this.GlobalAlertOfflineTriggered = printer.GlobalAlertOfflineTriggered;
            this.LastCheck = printer.LastCheck;
            this.Description = printer.Description;
            this.Manufacturer = printer.Manufacturer;
            this.Model = printer.Model;
            this.PageCount = printer.PageCount;
            this.PageCountColor = printer.PageCountColor;
            this.Pingable = printer.Pingable;
            this.SysContact = printer.SysContact;
            this.SysLocation = printer.SysLocation;
            this.UpTime = printer.UpTime;

            foreach (Supply supply in printer)
                Supplies.Add(new Supply{ 
                    Id = supply.Id, 
                    Description = supply.Description, 
                    Value = supply.Value, 
                    GlobalAlertTriggered = supply.GlobalAlertTriggered, 
                    NotificationValue = supply.NotificationValue, 
                    NotifyWhenLow = supply.NotifyWhenLow, 
                    Notified = supply.Notified,
                });

        }

        /// <summary>
        /// Löscht die dynamischen Werte die über SNMP eingeholt wurden
        /// </summary>
        public void Clear()
        {
            Manufacturer = String.Empty;
            Model = String.Empty;
            Pingable = false;
            PageCount = -1;
            PageCountColor = -1;
            Description = String.Empty;
            SysContact = string.Empty;
            SysLocation = string.Empty;
            UpTime = string.Empty;

            Supplies = new List<Supply>();
        }

        /// <summary>
        /// Gibt das Druckerobjekt als Zeichenkette wieder 
        /// </summary>
        /// <returns>In String Formatiertes Drucker Objekt</returns>
        public override string ToString()
        {
            string supplies = String.Empty;
            string events = String.Empty;

            if (this.Supplies != null)
                foreach (Supply supply in this.Supplies)
                {
                    supplies += "Description: " + supply.Description + ", Value: " + supply.Value + " %\n";
                }

            return String.Format("Id: {3}\nHostname: {0}\nManufacturer: {4}\nModel: {5}\nPingable: {1}\nCounter: {6}\nCounter color: {7}\nSupplies=>\n{2}Last Check: {8}\nDescription: {9}\n", 
                HostName, 
                Pingable, 
                supplies, 
                Id, 
                Manufacturer, 
                Model, 
                PageCount,
                PageCountColor,
                LastCheck,
                Description);
        }

        /// <summary>
        /// Prüfen ob eines der enthaltenen Verbrauchsteile über keine Datenbank-Id verfügt
        /// </summary>
        /// <returns>true falls alle Verbrauchsteile in der Datenbank verfügbar sind</returns>
        public bool AnySupplyNoId()
        {
            foreach (Supply s in this)
                if (s.Id == null)
                    return true;
            return false;
        }

        /// <summary>
        /// Gibt an ob der Globale (in der XML-Konfiguration konfigurierte) Drucker-Offline Event
        /// für diesen Drucker ausgelöst wurde
        /// </summary>
        public bool GlobalAlertOfflineTriggered { set; get; }

        IEnumerator<Supply> IEnumerable<Supply>.GetEnumerator()
        {
            foreach (Supply supply in Supplies)
                yield return supply;
        }

        /// <summary>
        /// Verbrauchsteile Enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (Supply supply in Supplies)
                yield return supply;
        }
    }

}
