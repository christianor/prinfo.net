using System;
using System.Collections.Generic;
using System.Text;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Repräsentiert das Verbrauchsteil eines Druckers
    /// </summary>
    public class Supply
    {
        /// <summary>
        /// Datenbank Id des Verbrauchsteils
        /// </summary>
        public int? Id { set; get; }
        /// <summary>
        /// Bezeichnung des Verbrauchsteils, z.B. "Black Cartridge"
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// Prozentualer Wert/Füllstand des Verbrauchsteils
        /// </summary>
        public double Value { set; get; }
        /// <summary>
        /// Gibt an dass das globale Ereigniss (XML Konfigurierter Wert) für dieses Verbrauchsteil eingetroffen ist,
        /// welches besagt, dass das Verbrauchsteil den kritischen Schwellwert unterschritten oder erreicht hat
        /// </summary>
        public bool GlobalAlertTriggered { set; get; }

        /// <summary>
        /// Gibt an das dieses Verbrauchsteil dediziert gemeldet werden soll, sollte der durch "NotificationValue"
        /// angegebene Wert unterschritten oder erreicht werden
        /// </summary>
        public bool NotifyWhenLow { set; get; }
        /// <summary>
        /// Der dedizierte Schwellwert
        /// </summary>
        public int NotificationValue { set; get; }
        /// <summary>
        /// Gibt an das eine Benachrichtung für den erreichten dedizierten Schwellwert ausgelöst wurde
        /// </summary>
        public bool Notified { set; get; }

    }

}
