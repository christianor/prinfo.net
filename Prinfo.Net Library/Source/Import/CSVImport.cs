using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;


namespace com.monitoring.prinfo
{
    /// <summary>
    /// Einfacher und unsicherer CSV-Import
    /// </summary>
    public class CSVImport : PrinterImport
    {
        private char _separator = ';';
        
        /// <summary>
        /// Trennzeichen der Werte
        /// </summary>
        public char Separator
        {
            set
            {
                _separator = value;
            }
            get
            {
                return _separator;
            }
        }

        /// <summary>
        /// Legt den Pfad zur CSV-Datei fest
        /// </summary>
        /// <param name="fqfn">Dateipfad</param>
        public CSVImport(string fqfn) : base(fqfn)
        {}

        /// <summary>
        /// Laden der Druckernamen aus der CSV Datei
        /// </summary>
        public override void LoadData()
        {
            using (StreamReader reader = new StreamReader(FullPathToFile))
            {
                List<Printer> printerList = new List<Printer>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] str = line.Split(Separator);
                    printerList.Add(new Printer { HostName = str[0] });
                }

                Printers = printerList;
            }
        }

    }
}
