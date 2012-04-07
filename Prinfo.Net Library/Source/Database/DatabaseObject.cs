using System;
using System.Data;
using System.IO;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Abstrakte Klasse die ein allgmeines Datenbankobjekt darstellt
    /// </summary>
    public abstract class DatabaseObject
    {
        /// <summary>
        /// Zeichenkette die die Datenbankspezifische Verbindung bezeichnet
        /// </summary>
        public string ConnectionString { set; get; }

        public SQLiteDatabaseFactory DatabaseInterface
        {
            get;
            set;
        }

        /// <summary>
        /// Erstellt das Schema der Datenbanktabelle
        /// </summary>
        abstract public void Initialize();
        /// <summary>
        /// Führt integrationstest durch
        /// </summary>
        /// <param name="throwException">Gibt an ob Ausnahmen geworfen werden sollen</param>
        /// <returns>True wenn Integritätstest erfolgreich abgeschlossen wurde</returns>
        abstract public bool PerformIntegrityCheck(bool throwException);

    }

}