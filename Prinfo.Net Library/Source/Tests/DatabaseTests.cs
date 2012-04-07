using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace com.monitoring.prinfo.Tests
{
    /// <summary>
    /// Tests die die Datenbankschnittstellen betreffen
    /// </summary>
    [TestFixture]
    public class DatabaseTests
    {
        /// <summary>
        /// Initialisiert die Datenbank (legt Schema an) und prüft anschließend 
        /// die integrität des Schemas. Schlegt fehl sollte die Initialisierung kein 
        /// sauberes Schema angelegt haben
        /// </summary>
        [Test]
        public void InitializePrinterDatabase_And_IntegrityTest()
        {
            var printerDatabase = new PrinterDatabase();
            printerDatabase.Initialize();

            Assert.IsTrue(printerDatabase.PerformIntegrityCheck(true));
        }

        /// <summary>
        /// Initialisiert die Datenbank (legt Schema an) und prüft anschließend 
        /// die integrität des Schemas. Schlegt fehl sollte die Initialisierung kein 
        /// sauberes Schema angelegt haben
        /// </summary>
        [Test]
        public void InitializeArchivDatabase_And_IntegrityTest()
        {
            var archivDatabase = new ArchivDatabase();
            archivDatabase.Initialize();

            Assert.IsTrue(archivDatabase.PerformIntegrityCheck(true));
        }

        /// <summary>
        /// Fügt ein Modifiziertes Druckerobjekt der Datenbank hinzu und prüft ob das 
        /// gespeicherte Druckerobjekt dem aktuellen entspricht.
        /// </summary>
        [Test]
        public void Add_And_GetPrinterFromDatabase_IntegrityTest()
        {
            var printerDatabase = new PrinterDatabase();
            var printer = printerDatabase.CreatePrinter("localhost");

            // fill with random values
            printer.Pingable = true;
            printer.Manufacturer = "Kyocera";

            printer.PageCount = 2000;

            printer.Supplies.Add(
                new Supply() { Description = "Black Toner", Value = 89, NotifyWhenLow = true, NotificationValue = 30 });

            printerDatabase.UpdatePrinter(printer);

            var retrievedPrinter = printerDatabase.GetPrinterById(printer.Id);

            // check if the retrieved values equal the saved ones

            Assert.AreEqual(printer.Manufacturer, retrievedPrinter.Manufacturer);
            Assert.AreEqual(printer.PageCount, retrievedPrinter.PageCount);
            Assert.AreEqual(printer.Supplies[0].Value, retrievedPrinter.Supplies[0].Value);
            Assert.AreEqual(printer.Supplies[0].NotifyWhenLow, retrievedPrinter.Supplies[0].NotifyWhenLow);
            Assert.AreEqual(printer.Supplies[0].NotificationValue, retrievedPrinter.Supplies[0].NotificationValue);

        }

        /// <summary>
        /// Prüft ob die Archivdatenbank Einträge die einen Tag auseinanderliegen separat speichert
        /// </summary>
        [Test]
        public void ArchivDb_2Day_Test()
        {
            var printerDatabase = new PrinterDatabase();
            var archivDatabase = new ArchivDatabase();

            var printer = printerDatabase.CreatePrinter("localhost");
            printer.LastCheck = DateTime.Now.ToString();


            archivDatabase.Initialize();
            archivDatabase.AddEntry(printer);
            // modifizieren des datums um einen tag damit ein weiterer archiveintrag erzeugt wird
            printer.LastCheck = DateTime.Now.AddDays(1).ToString();
            archivDatabase.AddEntry(printer);

            Assert.AreEqual(archivDatabase.GetEntriesById(printer.Id).Count, 2);



        }

        /// <summary>
        /// Prüft ob die Archivdatenbank Einträge die am selben Tag erzeugt werden Aktualisiert
        /// </summary>
        [Test]
        public void ArchivDb_SingleDay_Test()
        {
            var printerDatabase = new PrinterDatabase();
            var archivDatabase = new ArchivDatabase();

            var printer = printerDatabase.CreatePrinter("localhost");
            printer.LastCheck = DateTime.Now.ToString();


            archivDatabase.Initialize();
            archivDatabase.AddEntry(printer);
            archivDatabase.AddEntry(printer);

            Assert.AreEqual(archivDatabase.GetEntriesById(printer.Id).Count, 1);



        }

        /// <summary>
        /// Prüft ob das löschen eines Druckers funktioniert
        /// </summary>
        [Test]
        public void DeletePrinter_Test()
        {
            var printerDatabase = new PrinterDatabase();
            printerDatabase.Initialize();
            var printer = printerDatabase.CreatePrinter("localhost");


            printerDatabase.DeletePrinterByID(printer.Id);
            Assert.AreEqual(printerDatabase.GetPrinterList().Count, 0);

        }
    }
}
