using System;
using System.Collections.Generic;
using System.Text;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Drucker Ereignisargument
    /// </summary>
    public class PrinterEventArgs : EventArgs
    {
        /// <summary>
        /// Das Druckerobjekt für welches das Ereignis eingetroffen ist
        /// </summary>
        public Printer Printer { set; get; }

        /// <summary>
        /// Legt das Druckerobjekt fest
        /// </summary>
        /// <param name="printer"></param>
        public PrinterEventArgs(Printer printer)
        {
            Printer = printer;
        }
    }
}
