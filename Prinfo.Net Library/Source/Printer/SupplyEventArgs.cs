using System;
using System.Collections.Generic;
using System.Text;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Drucker Ereignisargument
    /// </summary>
    public class SupplyEventArgs : EventArgs
    {
        /// <summary>
        /// Das Druckerobjekt für welches das Ereignis eingetroffen ist
        /// </summary>
        public Printer Printer { set; get; }

        /// <summary>
        /// Der Verbrauchsgegenstand der als Leer erkannt wurde
        /// </summary>
        public Supply Supply { set; get; }

        /// <summary>
        /// Legt das Druckerobjekt und den Verbrauchsgegenstand fest
        /// </summary>
        /// <param name="supply">Der Verbrauchsgegenstand für den niedriger Füllstand auftrat</param>
        /// <param name="printer"></param>
        public SupplyEventArgs(Printer printer, Supply supply)
        {
            Printer = printer;
            Supply = supply;
        }
    }
}
