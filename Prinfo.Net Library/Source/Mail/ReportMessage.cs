using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Klasse ReportMessage, kapselt die Daten die für eine Report-Email benötigt werden
    /// </summary>
    public class ReportMessage
    {
        /// <summary>
        /// Die zu versendende Nachricht
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// Der zu versendende Anhang
        /// </summary>
        public Attachment Attachment { set; get; }
    }
}
