using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;


namespace com.monitoring.prinfo
{
    /// <summary>
    /// Generierte eine ReportMessage mit einer Excel-Datei im Anhang die die Druckerstände auflistet
    /// </summary>
    public class ExcelMailReportFormatter : MailReportFormatter
    {
        /// <summary>
        /// generiert eine excel tabelle aus den daten
        /// </summary>
        /// <returns>Die ReportMessage</returns>
        public override ReportMessage GenerateReportMessage()
        {
            // lade die config um globalen schwellwert zu prüfen
            Config.Load();

            StringBuilder markup = new StringBuilder();
            markup.Append("<html><head><title>Prinfo Report</title></head><body><h2>Druckerübersicht</h2><table>");

            markup.Append("<tr><th>Hostname</th><th>Hersteller</th><th>Modell</th><th>Letzte Überprüfung</th><th>Erreichbar?</th></tr>");

            foreach (var printer in printerList)
                markup.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", printer.HostName, printer.Manufacturer, printer.Model, printer.LastCheck, printer.Pingable == true ? "Ja" : "Nein"));

            markup.Append("</table><h2>Verbrauchsteilübersicht</h2><table><tr><th>Hostname</th><th>Hersteller</th><th>Modell</th><th>Verbrauchsteil</th><th>Füllstand %</th></tr>");

            foreach (var printer in printerList)
                foreach (var supply in printer)
                {
                    bool supplyLow = false;
                    if (supply.Value <= supply.NotificationValue)
                        supplyLow = true;
                    if (supply.Value <= Config.Notifications.CriticalSupplyLevel)
                        supplyLow = true;

                    markup.Append(String.Format("<tr " + (supplyLow ? "bgcolor=\"red\"":"") + "><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", printer.HostName, printer.Manufacturer, printer.Model, supply.Description, supply.Value));
                }

            markup.Append("</table></body></html>");

            /* Stream sollte später von MailMessage.Dispose() bereinigt werden (using block in Mailer Klasse)
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(markup.ToString());*/

            return new ReportMessage
            {
                Message = "Excel Report im Anhang",
                Attachment = Attachment.CreateAttachmentFromString(markup.ToString(), "DruckerReport_" + DateTime.Now + ".xls", Encoding.Default, "application/excel")
            };
        }
    }
}
