using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Generiert ReportMessages für den Mailversand
    /// </summary>
    public class TextMailReportFormatter : MailReportFormatter
    {
        /// <summary>
        /// generiert eine einfache textnachricht ohne anhang
        /// </summary>
        /// <returns>Die ReportMessage</returns>
        public override ReportMessage GenerateReportMessage()
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("@" + DateTime.Now + Environment.NewLine + " Reporting " + printerList.Count + " printer(s)" + Environment.NewLine + Environment.NewLine);

            List<Printer> printerWithLowSupplies = new List<Printer>();

            foreach (Printer p in printerList)
            {
                foreach (Supply s in p)
                    if (s.Value < Config.Notifications.CriticalSupplyLevel)
                    {
                        printerWithLowSupplies.Add(p);
                        break;
                    }
            }

            mailBody.Append("PrinterList with low supplies" + Environment.NewLine);
            mailBody.Append("=========================" + Environment.NewLine + Environment.NewLine);
            foreach (Printer p in printerWithLowSupplies)
            {
                mailBody.Append(p.HostName + Environment.NewLine);
                foreach (Supply s in p)
                {
                    if (s.Value <= Config.Notifications.CriticalSupplyLevel || (s.NotifyWhenLow == true && s.Value <= s.NotificationValue))
                        mailBody.Append(s.Description + " " + s.Value + Environment.NewLine);
                }

                mailBody.Append("#" + Environment.NewLine);
            }

            if (printerWithLowSupplies.Count == 0)
                mailBody.Append(" none" + Environment.NewLine + Environment.NewLine);

            mailBody.Append(Environment.NewLine + "Overview all printers" + Environment.NewLine);
            mailBody.Append("=====================" + Environment.NewLine + Environment.NewLine);

            if (printerList.Count > 0)
            {
                mailBody.Append("Hostname | Pingable" + Environment.NewLine);

                foreach (Printer p in printerList)
                {
                    mailBody.Append(p.HostName + "|" + p.Pingable.ToString() + Environment.NewLine + "------------------------------------" + Environment.NewLine);
                }
            }
            else
                mailBody.Append("none");


            return new ReportMessage() { Message = mailBody.ToString() };
        }
    }
}
