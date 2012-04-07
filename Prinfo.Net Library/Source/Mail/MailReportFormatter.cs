using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Abstrakte Klasse die die Standartfunktionen eines MailReportFormatters
    /// festlegt
    /// </summary>
    public abstract class MailReportFormatter
    {
        /// <summary>
        /// Die Druckerliste für die eine ReportMessage generiert werden sol
        /// </summary>
        protected List<Printer> printerList = new List<Printer>();

        /// <summary>
        /// hinzufügen einer Druckerliste
        /// </summary>
        /// <param name="printerList"></param>
        public void AddPrinterList(List<Printer> printerList)
        {
            this.printerList.AddRange(printerList);
        }

        /// <summary>
        /// Generiert eine ReportMessage mit entsprechendem Inhalt (Bezogen auf die Druckerliste)
        /// </summary>
        /// <returns>Die ReportMessage</returns>
        public abstract ReportMessage GenerateReportMessage();
    }
}
