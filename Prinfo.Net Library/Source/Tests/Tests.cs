using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace com.monitoring.prinfo.Tests
{
    /// <summary>
    /// Allgemeine Tests
    /// </summary>
    [TestFixture]
    public class Tests
    {

        /// <summary>
        /// Prüft ob das Wechseln der Sprache funktioniert
        /// </summary>
        [Test]
        public void Globalization_Test()
        {
            GlobalizationHelper.SwitchCulture(Language.German);
            string german = GlobalizationHelper.LibraryResource.GetString("no_hostname");

            Assert.AreEqual("Hostname fehlt oder ist null", german);

            GlobalizationHelper.SwitchCulture(Language.English);
            string english = GlobalizationHelper.LibraryResource.GetString("no_hostname");

            Assert.AreEqual("Hostname is missing or null", english);
        }

        /// <summary>
        /// Prüft ob das schreiben von Konfigurationseinträgen funktioniert
        /// </summary>
        [Test]
        public void ConfigReadWrite_Test()
        {
            Config.Load();
            var from = Config.Mail.From;

            Config.Mail.From = "test@test.test";
            Config.Save();

            Config.Load();

            Assert.AreEqual(Config.Mail.From, "test@test.test");

            Config.Mail.From = from;
            Config.Save();

            Config.Load();
            Assert.AreEqual(Config.Mail.From, from);

        }

        /// <summary>
        /// Prüft ob die installation/deinstallation des Dienstes funktioniert
        /// </summary>
        [Test]
        public void ServiceInstall_Start_Stop_Uninstall_Test()
        {
            try
            {

                ServiceManager.UninstallPrinfoService();
            }
            catch (Exception) { }

            ServiceManager.InstallPrinfoService();

            ServiceManager.PrinfoService.Start();

            while (ServiceManager.PrinfoService.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                System.Threading.Thread.Sleep(5);
            ServiceManager.PrinfoService.Stop();

            ServiceManager.UninstallPrinfoService();
            

        }

        /// <summary>
        /// Testet den ExcelMailReportFormatter.
        /// Speichert die generierte Excel Datei auf dem Desktop
        /// </summary>
        [Test]
        public void TestExcelReportMessage()
        {

            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var generator = new ExcelMailReportFormatter();


            var printer = new Printer();
            printer.HostName = "ChristiansHost";
            printer.Model = "MFP";
            printer.Manufacturer = "Hewlett Packard";

            printer.Supplies.Add(new Supply() { Description = "Black Toner", Value = 10, NotificationValue = 11 });
            generator.AddPrinterList(new List<Printer>() { printer });

            var report = generator.GenerateReportMessage();

            // report.Attachment.ContentStream
            using (var zielStream = File.Create(path + "\\test.xls"))
            {
                report.Attachment.ContentStream.CopyTo(zielStream);               
            }
            
            report.Attachment.Dispose();

        }
    }
}
