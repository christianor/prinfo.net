using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace com.monitoring.prinfo.Tests
{

    /// <summary>
    /// Führt Tests durch die den Druckermanager auf Funktionsfähigkeit überprüfen
    /// </summary>
    [TestFixture]
    public class PrinterManagerTests
    {
        private bool notified = false;

        /// <summary>
        /// Prüft ob der PrinterManager sich korrekt verhält, sollte ein Drucker plötzlich weniger
        /// Verbrauchsteile als vorher haben
        /// </summary>
        [Test]
        public void PrinterManager_SupplyIntegrityFail_Test()
        {

            var manager = new PrinterManager();
            var printer = new Printer();
            printer.HostName = "localhost";

            printer.Supplies.Add(new Supply() { Description = "BlackToner" });

            manager.PollPrinter(printer);

            // der drucker muss nach dem pollen 0 supplies haben da localhost (der aktuelle pc) keine supplies
            // haben sollte
            Assert.AreEqual(printer.Supplies.Count, 0);
        }

        /// <summary>
        /// Prüft ob die Autowiederherstellung der Datendateien funktioniert
        /// </summary>
        [Test]
        public void PrinterManager_DeleteDataFiles_AutoRecreate_Test()
        {
            System.IO.File.Delete(DirectoryController.DataDirectoryLocation + "printer.db");

            var manager = new PrinterManager();
            
            var printer = new Printer();
            manager.LoadPrinterList();

            // Datei sollte wieder existieren
            Assert.IsTrue(System.IO.File.Exists(DirectoryController.DataDirectoryLocation + "printer.db"));
        }

        /// <summary>
        /// Prüft ob die OnPrinterOffline - Benachrichtigung funktioniert
        /// </summary>
        [Test]
        public void CheckPrinterOfflineNotification()
        {
            notified = false;
            var manager = new PrinterManager();

            manager.OnPrinterOffline += (o, e) => { notified = true; };

            manager.PollPrinter(new Printer() { HostName = "0.0.0.1" });


            Assert.IsTrue(notified);
        }

        /// <summary>
        /// Prüft ob das laden der Druckerliste funktioniert
        /// </summary>
        [Test]
        public void PrinterSaveAndLoad_Test()
        {
            var manager = new PrinterManager();
            manager.PrinterDatabase.Initialize();
            manager.PrinterDatabase.CreatePrinter("localhost");
            Assert.AreEqual(manager.LoadPrinterList().Count, 1);



            Config.Load();
            Config.Notifications.SendTo = "cortiz@metadok.de";
            Config.Save();
        }
    }
}
