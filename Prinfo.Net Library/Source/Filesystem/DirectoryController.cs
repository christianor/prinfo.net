using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Verzeichnis "Controller", Erstellt und legt Verzeichnisse fest
    /// </summary>
    public static class DirectoryController
    {
        private static string _dataDirectoryLocation = AppDomain.CurrentDomain.BaseDirectory + @"\data\";
        /// <summary>
        /// Der Ort des Data Verzeichnisses
        /// </summary>
        public static string DataDirectoryLocation 
        {
            set
            {
                if (Directory.Exists(value))
                {
                    _dataDirectoryLocation = AddFinalBackslashIfNotThere(value);
                }
                else throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("directory_not_exist"), value));
            }
            get
            {
                return _dataDirectoryLocation;
            }
        }

        private static string _logDirectoryLocation = AppDomain.CurrentDomain.BaseDirectory + @"\logs\";
        /// <summary>
        /// Der Ort des Logverzeichnisses
        /// </summary>
        public static string LogDirectoryLocation
        {
            set
            {
                if (Directory.Exists(value))
                {
                    _logDirectoryLocation = AddFinalBackslashIfNotThere(value);
                }
                else throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("directory_not_exist"), value));
            }
            get
            {
                return _logDirectoryLocation;
            }
        }

        /// <summary>
        /// Erstellt das Logverzeichnis falls es nicht existiert
        /// </summary>
        /// <remarks>Meldet Exceptions nicht</remarks>
        public static void CreateLogDirIfNotExists()
        {
            if (!Directory.Exists(DirectoryController.LogDirectoryLocation))
            {
                try
                {
                    Directory.CreateDirectory(DirectoryController.LogDirectoryLocation);
                }
                catch { }
            }
        }

        /// <summary>
        /// Erstellt das Data-Verzeichniss falls es nicht existiert.
        /// In diesem befinden sich die Datenbankdateien und die Konfiguration
        /// </summary>
        public static void CreateDataDirIfNotExists()
        {
            if (!Directory.Exists(DirectoryController.DataDirectoryLocation))
            {
                Directory.CreateDirectory(DirectoryController.DataDirectoryLocation);
            }
        }

        /// <summary>
        /// Fügt am Ende einer Zeichenkette ein Backslash hinzu falls dieser noch nicht vorhanden ist
        /// </summary>
        /// <param name="directoryPath">Zeichenkette die den Pfad repräsentiert</param>
        /// <returns>Die Zeichenkette mit Backslash am Ende</returns>
        public static string AddFinalBackslashIfNotThere(string directoryPath)
        {
            if (!directoryPath[directoryPath.Length - 1].Equals('\\') && !directoryPath[directoryPath.Length - 1].Equals('/'))
                return directoryPath += "\\";
            else return directoryPath;
        }
    }
}
