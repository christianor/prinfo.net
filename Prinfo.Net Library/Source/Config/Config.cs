using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Kapselt die Konfigurationsdaten in den Unterliegenden statischen Klassen
    /// Ermöglicht speichern, laden und initialisieren (Ursprungszustand) der Konfiguration
    /// <remarks>
    /// Die Konfiguration sollte zunächst über Config.Load() geladen werden bevor Werte
    /// modifiziert und gespeichert werden
    /// <example>
    /// Config.Load();
    /// //... Modifikationen der Konfigurationsdaten
    /// Config.Save();
    /// </example>
    /// </remarks>
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Serialisiert Zugriff auf die Konfigurations-XML innerhalb der Bibliothek
        /// </summary>
        private static Object configLock = new Object();

        /// <summary>
        /// Initialisert die Konfigurationsdatei.
        /// Beinhaltet:
        ///     - Erstellen der Datei im Data-Verzeichnis
        ///     - Schreiben der Konfigurationsstruktur mit Beispielwerten
        /// </summary>
        public static void Initialize()
        {
            DirectoryController.CreateDataDirIfNotExists();
            using (XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(DirectoryController.DataDirectoryLocation + "config.xml", Encoding.UTF8))
            {
                xmlTextWriter.WriteStartDocument(true);
                xmlTextWriter.Formatting = System.Xml.Formatting.Indented;

                // write root element config
                xmlTextWriter.WriteStartElement("config");

                // write mail config
                xmlTextWriter.WriteStartElement("mail");
                xmlTextWriter.WriteElementString("from", "prinfo@noreply.com");
                xmlTextWriter.WriteElementString("smtp_server", "smtp.nosmtp.de");
                xmlTextWriter.WriteElementString("username", "konradzuse");
                xmlTextWriter.WriteElementString("password", "passswd");
                xmlTextWriter.WriteElementString("use_ssl", "false");
                xmlTextWriter.WriteElementString("port", "25");
                xmlTextWriter.WriteEndElement();

                // global alerts
                xmlTextWriter.WriteStartElement("notifications");
                xmlTextWriter.WriteElementString("alertIfPrinterOffline", "false");
                xmlTextWriter.WriteElementString("alertIfSupplyLevelCritical", "false");
                xmlTextWriter.WriteElementString("criticalSupplyLevel", "0");
                xmlTextWriter.WriteElementString("sendNotificationTo", "test@test.org");
                xmlTextWriter.WriteEndElement();

                // reporting
                xmlTextWriter.WriteStartElement("reporting");
                xmlTextWriter.WriteElementString("excelMailReport", "false");
                xmlTextWriter.WriteElementString("simpleMailReport", "false");
                xmlTextWriter.WriteElementString("sendReportTo", "test@test.org");
                xmlTextWriter.WriteElementString("timeToStart", "9:03");
                xmlTextWriter.WriteEndElement();

                // general settings
                xmlTextWriter.WriteStartElement("general");
                xmlTextWriter.WriteElementString("directSearch", "true");
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteEndDocument();
            }
        }

        /// <summary>
        /// Lädt die Konfigurationsdaten in die von Config gekapsalten statischen Klassen.
        /// Sollte das öffnen und laden der Datei fehlschlagen wird die Konfiguration durch Config.Initialize()
        /// initialisert
        /// </summary>
        public static void Load()
        {
            lock (configLock)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    XmlNode bufferNode;

                    try
                    {
                        doc.Load(DirectoryController.DataDirectoryLocation + "config.xml");
                    }
                    catch (FileNotFoundException)
                    {
                        Initialize();
                        doc.Load(DirectoryController.DataDirectoryLocation + "config.xml");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        DirectoryController.CreateDataDirIfNotExists();

                        Initialize();
                        doc.Load(DirectoryController.DataDirectoryLocation + "config.xml");
                    }

                    // load the mail configuration
                    bufferNode = doc.SelectSingleNode("config/mail/from");
                    CheckIfBufferNodeNull("config/mail/from", bufferNode);
                    Mail.From = bufferNode.InnerText;
                    bufferNode = doc.SelectSingleNode("config/mail/smtp_server");
                    CheckIfBufferNodeNull("config/mail/smtp_server", bufferNode);
                    Mail.SmtpServer = bufferNode.InnerText;
                    bufferNode = doc.SelectSingleNode("config/mail/username");
                    CheckIfBufferNodeNull("config/mail/username", bufferNode);
                    Mail.Username = bufferNode.InnerText;
                    bufferNode = doc.SelectSingleNode("config/mail/password");
                    CheckIfBufferNodeNull("config/mail/password", bufferNode);
                    Mail.Password = bufferNode.InnerText;
                    bufferNode = doc.SelectSingleNode("config/mail/use_ssl");
                    CheckIfBufferNodeNull("config/mail/use_ssl", bufferNode);
                    Mail.UseSsl = bool.Parse(bufferNode.InnerText);
                    bufferNode = doc.SelectSingleNode("config/mail/port");
                    CheckIfBufferNodeNull("config/mail/port", bufferNode);
                    Mail.Port = int.Parse(bufferNode.InnerText);

                    // load the global alert configuration
                    bufferNode = doc.SelectSingleNode("config/notifications/alertIfPrinterOffline");
                    CheckIfBufferNodeNull("config/notifications/alertIfPrinterOffline", bufferNode);
                    Notifications.AlertIfPrinterOffline = bool.Parse(bufferNode.InnerText);
                    bufferNode = doc.SelectSingleNode("config/notifications/alertIfSupplyLevelCritical");
                    CheckIfBufferNodeNull("config/notifications/alertIfSupplyLevelCritical", bufferNode);
                    Notifications.AlertIfSupplyLevelCritical = bool.Parse(bufferNode.InnerText);
                    bufferNode = doc.SelectSingleNode("config/notifications/criticalSupplyLevel");
                    CheckIfBufferNodeNull("config/notifications/criticalSupplyLevel", bufferNode);
                    Notifications.CriticalSupplyLevel = int.Parse(bufferNode.InnerText);
                    bufferNode = doc.SelectSingleNode("config/notifications/sendNotificationTo");
                    CheckIfBufferNodeNull("config/notifications/sendNotificationTo", bufferNode);
                    Notifications.SendTo = bufferNode.InnerText;

                    // load reporting
                    bufferNode = doc.SelectSingleNode("config/reporting/simpleMailReport");
                    CheckIfBufferNodeNull("config/reporting/simpleMailReport", bufferNode);
                    Reporting.SimpleMailReport = bool.Parse(bufferNode.InnerText);
                    bufferNode = doc.SelectSingleNode("config/reporting/excelMailReport");
                    CheckIfBufferNodeNull("config/reporting/excelMailReport", bufferNode);
                    Reporting.ExcelMailReport = bool.Parse(bufferNode.InnerText);
                    bufferNode = doc.SelectSingleNode("config/reporting/sendReportTo");
                    CheckIfBufferNodeNull("config/reporting/sendReportTo", bufferNode);
                    Reporting.SendReportTo = bufferNode.InnerText;
                    bufferNode = doc.SelectSingleNode("config/reporting/timeToStart");
                    CheckIfBufferNodeNull("config/reporting/timeToStart", bufferNode);
                    Reporting.TimeToStart = DateTime.Parse(bufferNode.InnerText);
                  

                    // load general settings
                    bufferNode = doc.SelectSingleNode("config/general/directSearch");
                    CheckIfBufferNodeNull("config/general/directSearch", bufferNode);
                    Config.General.DirectSearch = bool.Parse(bufferNode.InnerText);
                }
                catch (Exception e)
                {
                    throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("config_error_load"), e.Message), e);
                }
            }
        }

        /// <summary>
        /// Exception Throwing helper
        /// </summary>
        /// <param name="name">Der Name des XML Elements</param>
        /// <param name="node">Der XML-Knoten der zu prüfen ist</param>
        private static void CheckIfBufferNodeNull(string name, XmlNode node)
        {
            if (!(node is XmlNode))
                throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("config_error_location"), name));

        }

        /// <summary>
        /// Speichert die Konfigurationsdaten
        /// </summary>
        public static void Save()
        {
            lock (configLock)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = false;

                    try
                    {
                        doc.Load(DirectoryController.DataDirectoryLocation + "config.xml");
                    }
                    catch (FileNotFoundException)
                    {
                        Initialize();
                        doc.Load(DirectoryController.DataDirectoryLocation + "config.xml");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        DirectoryController.CreateDataDirIfNotExists();

                        Initialize();
                        doc.Load(DirectoryController.DataDirectoryLocation + "config.xml");
                    }

                    XmlNodeList config = doc.GetElementsByTagName("config");

                    config[0].ChildNodes[0].ChildNodes[0].InnerText = Mail.From.ToString();
                    config[0].ChildNodes[0].ChildNodes[1].InnerText = Mail.SmtpServer.ToString();
                    config[0].ChildNodes[0].ChildNodes[2].InnerText = Mail.Username.ToString();
                    config[0].ChildNodes[0].ChildNodes[3].InnerText = Mail.Password.ToString();
                    config[0].ChildNodes[0].ChildNodes[4].InnerText = Mail.UseSsl.ToString();
                    config[0].ChildNodes[0].ChildNodes[5].InnerText = Mail.Port.ToString();

                    config[0].ChildNodes[1].ChildNodes[0].InnerText = Notifications.AlertIfPrinterOffline.ToString();
                    config[0].ChildNodes[1].ChildNodes[1].InnerText = Notifications.AlertIfSupplyLevelCritical.ToString();
                    config[0].ChildNodes[1].ChildNodes[2].InnerText = Notifications.CriticalSupplyLevel.ToString();
                    config[0].ChildNodes[1].ChildNodes[3].InnerText = Notifications.SendTo.ToString();

                    config[0].ChildNodes[2].ChildNodes[0].InnerText = Reporting.ExcelMailReport.ToString();
                    config[0].ChildNodes[2].ChildNodes[1].InnerText = Reporting.SimpleMailReport.ToString();
                    config[0].ChildNodes[2].ChildNodes[2].InnerText = Reporting.SendReportTo.ToString();
                    config[0].ChildNodes[2].ChildNodes[3].InnerText = Reporting.TimeToStart.TimeOfDay.ToString();


                    config[0].ChildNodes[3].ChildNodes[0].InnerText = Config.General.DirectSearch.ToString();

                    using (FileStream writer = new FileStream(DirectoryController.DataDirectoryLocation + "config.xml", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        doc.Save(writer);

                }
                catch (Exception e)
                {
                    throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("config_error_save"), e.Message), e);
                }
            }
        }

        /// <summary>
        /// Werte zur Mailkonfiguration
        /// </summary>
        public static class Mail
        {
            /// <summary>
            /// Versender
            /// </summary>
            public static string From { set; get; }
            /// <summary>
            /// Smtp-Server
            /// </summary>
            public static string SmtpServer { set; get; }
            /// <summary>
            /// Benutzername
            /// </summary>
            public static string Username { set; get; }
            /// <summary>
            /// Passwort
            /// </summary>
            public static string Password { set; get; }
            /// <summary>
            /// Gibt an ob SSL verwendet werden soll (TLS)
            /// </summary>
            public static bool UseSsl { set; get; }
            /// <summary>
            /// Port
            /// </summary>
            public static int Port { set; get; }
        }

        /// <summary>
        /// Versionsinformationen
        /// </summary>
        public static class Version
        {
            /// <summary>
            /// Beschreibung der Bibliothek, statisch und nicht in der Config-Datei vorhanden
            /// </summary>
            public static string Description = "v0.5 stable";
        }

        /// <summary>
        /// Werte zur Benachrichtigungskonfiguration
        /// </summary>
        public static class Notifications
        {
            /// <summary>
            /// Benachrichtigen sobald ein Drucker offline ist
            /// </summary>
            public static bool AlertIfPrinterOffline { set; get; }
            /// <summary>
            /// Benachrichtigen sobald SupplyLevel kritisches Level erreicht
            /// </summary>
            public static bool AlertIfSupplyLevelCritical { set; get; }
            /// <summary>
            /// Globaler kritischer Füllstand
            /// </summary>
            public static int CriticalSupplyLevel { set; get; }
            /// <summary>
            /// Gibt an wer der Empfänger der Benachrichtigung ist, Komma oder Punktstrich separiert
            /// </summary>
            public static string SendTo { set; get; }
        }

        /// <summary>
        /// Werte zur Report-Konfiguration
        /// </summary>
        public static class Reporting
        {
            /// <summary>
            /// Gibt an ob ein einfacher TextReport versendet werden soll
            /// </summary>
            public static bool SimpleMailReport { set; get; }
            /// <summary>
            /// Gibt and ob ein ExcelReport versendet werden soll
            /// </summary>
            public static bool ExcelMailReport { set; get; }
            /// <summary>
            /// Gibt an wer der Empfänger des Reports ist, Komma oder Punktstrich separiert
            /// </summary>
            public static string SendReportTo { set; get; }
            /// <summary>
            /// Gibt an zu welcher Zeit der Report versendet werden soll
            /// </summary>
            public static DateTime TimeToStart { set; get; }
        }

        /// <summary>
        /// Allgemeine Einstellungen für GUI und Konsolenanwendungen
        /// </summary>
        public static class General
        {
            /// <summary>
            /// Gibt an ob Direktsuche im Grafischen Manager aktiviert sein soll
            /// sollte in eine eigene Konfigurationsdatei extrahiert werden
            /// </summary>
            public static bool DirectSearch { set; get; }
        }

        
    }
}
