using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Abstrakte Klasse für den Import von Druckerdaten aus einer Datei
    /// </summary>
    public abstract class PrinterImport
    {
        private string _fullPathToFile;
        /// <summary>
        /// Der Pfad zur Datei
        /// </summary>
        public string FullPathToFile 
        {
            set
            {
                if(File.Exists(value))
                    _fullPathToFile = value;
                else
                    throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("file_not_exists"));
            }
            get
            {
                return _fullPathToFile;
            }
        }
        private PrinterDatabase printerDatabase = new PrinterDatabase();

        /// <summary>
        /// Die extrahierten Drucker
        /// </summary>
        public List<Printer> Printers { protected set; get; }

        /// <summary>
        /// Speichert den Pfad zu der Datei
        /// </summary>
        /// <param name="fullPathToFile">Der Dateipfad</param>
        public PrinterImport(string fullPathToFile)
        {
            FullPathToFile = fullPathToFile;
        }

        /// <summary>
        /// Laden der Daten
        /// </summary>
        public abstract void LoadData();

        /// <summary>
        /// Speichern der Daten in der Datenbank
        /// </summary>
        public void Save()
        {
            foreach (Printer _printer in Printers)
            {
                printerDatabase.CreatePrinter(_printer.HostName);
            }
        }
    }
}
