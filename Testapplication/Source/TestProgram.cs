using System;
using com.monitoring.prinfo;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Threading;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using NUnit.Framework;


namespace com.monitoring.prinfo
{
    [TestFixture]
    class Program
    {

        private static PrinterManager printerManager = new PrinterManager();

        public static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.AddDays(1).ToString());
            PrinterManager pm = new PrinterManager();
            pm.PrinterDatabase.Initialize();
         
            var printer = pm.PrinterDatabase.CreatePrinter("fufu");
            printer.PageCount = 10;
            printer.PageCountColor = 20;

            var now = DateTime.Now;

            printer.LastCheck = now.ToString();

            pm.PrinterDatabase.UpdatePrinter(printer);
            pm.ArchivDatabase.AddEntry(printer);

            Console.WriteLine("starting action");

            for (int i = 0; i < 10000; i++)
            {
                printer.PageCount += 10;
                printer.PageCountColor += 5;

                printer.LastCheck = now.AddDays(i+1).ToString();

                pm.PrinterDatabase.UpdatePrinter(printer);
                pm.ArchivDatabase.AddEntry(printer);

                Console.WriteLine("added " + i+1);
            }
            
            Console.WriteLine("done");
            Console.ReadLine();
        }

        [Test]
        public static void AddAndCheckPrinter()
        {
            EventHandler<PrinterEventArgs> updatePrinter = (object sender, PrinterEventArgs e) =>
            {
                printerManager.PrinterDatabase.UpdatePrinter(e.Printer);
            };

            printerManager.OnPrinterChecked += updatePrinter;

            printerManager.PrinterDatabase.Initialize();
            printerManager.PrinterDatabase.CreatePrinter("localhost");

            Assert.IsTrue(printerManager.LoadPrinterList().Count == 1, "Adding printer failed");

            printerManager.ConnectAndCheckPrinterList();

            Assert.IsTrue(printerManager.PrinterList[0].Pingable, "localhost is not pingable");

            printerManager.OnPrinterChecked -= updatePrinter;

            
        }

        [Test]
        public static void ConfigTest()
        {
            Config.Load();
            Config.Reporting.TimeToStart = DateTime.Parse("00:00");
            Config.Save();

            Config.Load();

            Assert.AreEqual(Config.Reporting.TimeToStart, DateTime.Parse("00:00"));
        }

    }

}