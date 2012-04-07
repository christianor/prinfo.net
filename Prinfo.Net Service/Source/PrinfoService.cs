using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Configuration.Install;
using System.Net.Mail;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace com.monitoring.prinfo
{
    class PrinfoService : ServiceBase
    {
        /// <summary>
        /// Thread für das periodische reporting
        /// </summary>
        private Thread reporting;
       
        /// <summary>
        /// Der Timer wird für das periodische überprüfen der Drucker verwendet
        /// </summary>
        private System.Timers.Timer timer;

        private PrinterManager fetchingManager;
        

        /// <summary>
        /// Startet den Dienst
        /// </summary>
        public static void Main()
        {
            ServiceBase.Run(new PrinfoService());
        }


        /// <summary>
        /// Initialisiert den Timer, initialisiert den "fetchingManager (PrinterManager)" und 
        /// weißt ihm die entsprechenden Ereignisshandler zu
        /// </summary>
        public PrinfoService()
        {
            this.ServiceName = "Prinfo.NET Service";
            this.EventLog.Log = "Application";

            timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(PrinterFetchingAndNotifying);

            fetchingManager = new PrinterManager();
            fetchingManager.OnSupplyLow += new EventHandler<SupplyEventArgs>(fetchingManager_OnSupplyLow);
            fetchingManager.OnPrinterOffline += new EventHandler<PrinterEventArgs>(fetchingManager_OnPrinterOffline);
            fetchingManager.OnPrinterChecked += new EventHandler<PrinterEventArgs>(fetchingManager_OnPrinterChecked);
            fetchingManager.OnPrinterListChecked += new EventHandler<EventArgs>(fetchingManager_OnPrinterListChecked);

            reporting = new Thread(Reporting);
        }

        /// <summary>
        /// Wird abgefeuert sobald ein Verbrauchsteil niedrieger oder gleich Supply.NotificationValue ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fetchingManager_OnSupplyLow(object sender, SupplyEventArgs e)
        {
            var supply = e.Supply;
            var printer = e.Printer;

            if (supply.NotifyWhenLow && !supply.Notified)
            {
                supply.Notified = true;
                // supply.GlobalAlertTriggered = true;

                new Thread(x =>
                {
                    try
                    {
                        Mailer.SendMailOverSmtp(Config.Notifications.SendTo, "Prinfo.NET Notification " + printer.HostName, "@" + DateTime.Now + Environment.NewLine + "PrinterList supply level of " + supply.Description + " printer " + printer.HostName + " is critical. (" + supply.Value + "%)");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(GlobalizationHelper.LibraryResource.GetString("smtp_error") + ex.Message, LogType.Error);
                    }
                }).Start();
            }
        }

        /// <summary>
        /// Wird abgefeuert sobald ein Drucker offline ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fetchingManager_OnPrinterOffline(object sender, PrinterEventArgs e)
        {
            var printer = e.Printer;
            if (Config.Notifications.AlertIfPrinterOffline)
            {
                if (printer.GlobalAlertOfflineTriggered == false && printer.Pingable == false)
                {
                    new Thread(x =>
                    {
                        try
                        {
                            Mailer.SendMailOverSmtp(Config.Notifications.SendTo, "Prinfo.NET Notification " + printer.HostName, "@" + DateTime.Now + Environment.NewLine + "PrinterList " + printer.HostName + " is offline.");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(GlobalizationHelper.LibraryResource.GetString("smtp_error") + ex.Message, LogType.Error);
                        }
                    }).Start();

                    printer.GlobalAlertOfflineTriggered = true;
                }
                else if (printer.Pingable == true && printer.GlobalAlertOfflineTriggered == true)
                    printer.GlobalAlertOfflineTriggered = false;
            }
        }

        /// <summary>
        /// printer and event checking
        /// </summary>
        private void PrinterFetchingAndNotifying(object sender, System.Timers.ElapsedEventArgs e)
        {
            Logger.Log(GlobalizationHelper.LibraryResource.GetString("printer_check_start"), LogType.Service);

            Config.Load();
            fetchingManager.LoadPrinterList();

            fetchingManager.PollPrinterList(true);


        }

        /// <summary>
        /// Tritt ein sobald alle Drucker parallel überprüft wurden, setzt den Timer zurück
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fetchingManager_OnPrinterListChecked(object sender, EventArgs e)
        {
            // Schwellwert um den Timeout zurückzusetzen ist derzeit bei 100 Druckern
            if (fetchingManager.PrinterList.Count < 100)
                timer.Interval = 60000;
            else
                timer.Interval = 1;

            timer.Start();
        }

        /// <summary>
        /// Tritt ein sobald ein Drucker vom "fetchingManager" überprüft wurde.
        /// Updated die Werte des Druckers in der Datenbank und prüft ob Schwellwerte für die
        /// Benachrichtigungen überschritten wurden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fetchingManager_OnPrinterChecked(object sender, PrinterEventArgs e)
        {
            var printer = e.Printer;


            foreach (var supply in printer)
            {
                string description = supply.Description;
                double value = supply.Value;


                //prüfen ob schon benachrichtig wurde und der Wert wieder erhöht ist
                if (supply.NotificationValue < supply.Value && supply.Notified)
                    supply.Notified = false;

                // globalen alert zurücksetzen, wird ausserhalb des AlertIfSupplyLevelCritical Blocks durchgeführt
                // da die Benachrichtigung auch dann zurückgesetzt werden soll, wenn per Konfiguration die Alarmierung deaktiviert ist
                if (Config.Notifications.CriticalSupplyLevel < supply.Value && supply.GlobalAlertTriggered == true)
                {
                    supply.GlobalAlertTriggered = false;
                }

                // prüfen ob globale events (xml konfiguriert) aktiv sind und schon geschmissen wurden
                if (Config.Notifications.AlertIfSupplyLevelCritical)
                {
                    if (supply.GlobalAlertTriggered == false && supply.Value <= Config.Notifications.CriticalSupplyLevel)
                    {
                        supply.GlobalAlertTriggered = true;
                        new Thread(x =>
                        {
                            try
                            {
                                Mailer.SendMailOverSmtp(Config.Notifications.SendTo, "Prinfo.NET Notification " + printer.HostName, "@" + DateTime.Now + Environment.NewLine + "PrinterList supply level of " + description + " printer " + printer.HostName + " is critical. (" + value + "%)");
                            }
                            catch (Exception ex)
                            {
                                Logger.Log(GlobalizationHelper.LibraryResource.GetString("smtp_error") + ex.Message, LogType.Error);
                            }
                        }).Start();
                    }
                }

            }


            try
            {
                fetchingManager.PrinterDatabase.UpdatePrinter(printer);
                
            }
            catch (ApplicationException ae)
            {
                Logger.Log("Failed updating printer database entry. Hostname = " + printer.HostName + ". " + ae.Message, LogType.Error);
            }

            try
            {
                fetchingManager.ArchivDatabase.AddEntry(printer);
            }
            catch (ApplicationException ae)
            {
                Logger.Log("Failed updating archiv database entry. Hostname = " + printer.HostName + ", "  + ae.Message, LogType.Error);
            }
        }


        /// <summary>
        /// Reports the printer status at a specific time
        /// </summary>
        private void Reporting()
        {
            Config.Load();

            if (Config.Reporting.SimpleMailReport || Config.Reporting.ExcelMailReport)
            {
                Logger.Log("Reporting started", LogType.Service);
                PrinterManager manager = new PrinterManager();

                while (true)
                {
                    Config.Load();

                    Logger.Log("Mail report invoked", LogType.Service);

                    DateTime now = DateTime.Now;

                    if (now <= Config.Reporting.TimeToStart)
                    {
                        Logger.Log("Reporting sleeping for " + (Config.Reporting.TimeToStart - now) , LogType.Service);
                        Thread.Sleep(Config.Reporting.TimeToStart - now);
                    }
                    else
                    {
                        Logger.Log("Reporting sleeping for " + (Config.Reporting.TimeToStart.AddHours(24) - now), LogType.Service);
                        Thread.Sleep(Config.Reporting.TimeToStart.AddHours(24) - now);
                    }

                    manager.LoadPrinterList();

                    // no need to write the checked printers back to the database
                    // no database update is performed after
                    manager.ParallelPollPrinterList();

                    MailReportFormatter mailReportFormatter;

                    ReportMessage message = null;

                    if (Config.Reporting.SimpleMailReport)
                    {
                        mailReportFormatter = new TextMailReportFormatter();
                        mailReportFormatter.AddPrinterList(manager.PrinterList);

                        message = mailReportFormatter.GenerateReportMessage();
                        try
                        {
                            Mailer.SendMailOverSmtp(Config.Reporting.SendReportTo, "Prinfo.NET Reporting", message.Message);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(GlobalizationHelper.LibraryResource.GetString("smtp_error") + ex.Message, LogType.Error);
                        }
                    }
                    if (Config.Reporting.ExcelMailReport)
                    {
                        mailReportFormatter = new ExcelMailReportFormatter();
                        mailReportFormatter.AddPrinterList(manager.PrinterList);

                        message = mailReportFormatter.GenerateReportMessage();

                        try
                        {
                            Mailer.SendMailOverSmtp(Config.Reporting.SendReportTo, "Prinfo.NET Reporting", message.Message, message.Attachment);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(GlobalizationHelper.LibraryResource.GetString("smtp_error") + ex.Message, LogType.Error);
                        }
                    }
                   
                
                }
            }
        }



        /// <summary>
        /// Starte Threads
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            timer.Start();
            reporting.Start();

            Config.Load();

            Logger.Log("Service started", LogType.Service);
        }

        /// <summary>
        /// Stoppe Threads
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();

            timer.Stop();
            reporting.Abort();


            Logger.Log("Service stopped", LogType.Service);
        }

        protected override void OnPause()
        {
            base.OnPause();
            timer.Stop();
            reporting.Abort();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
            timer.Stop();
            reporting.Abort();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            timer.Stop();
            reporting.Abort();
        }
    }
}
