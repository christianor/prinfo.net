using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Threading;
using com.monitoring.prinfo;
using System.Diagnostics;

namespace com.monitoring.prinfo.pico
{
    class Pico
    {
        static PrinterManager printerManager = new PrinterManager();
        static Stopwatch stopwatch = new Stopwatch();
      
        /// <summary>
        /// command list, overview of all available commands (help or ? output)
        /// </summary>
        #region commandList
        static string[,] commandList = new string[,]
        {
            {"help", "shows a command overview"},
            {"exit", "quits pico"},
            {"clear", "clears the screen"},
            {"add printer", "adds a printer to the database (e.g. add printer 'hostname or ip of your printer')"},
            {"status", "current prinfo status"},
            {"init-db", "reset the printer database"},
            {"check-all", "check every printer in the database once"},
            {"start / stop service", "starts or stops the service (e.g. 'start service')"},
            {"install / uninstall service", "install or uninstall the Prinfo.Net PrinterList Queue Service (e.g. 'install service')"},
            {"switch language", "switches the current application language (e.g. switch language english OR switch language deutsch)"},
            {"start webserver", @"starts the embedded webserver. directory is ApplicationDirectory\web"}

        };
        #endregion commandList

        static ResourceManager resi = new ResourceManager("com.monitoring.prinfo.pico.Resource.res", Assembly.GetExecutingAssembly());

        /// <summary>
        /// main entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var manager = new PrinterManager();
            var drucker = manager.PrinterDatabase.CreatePrinter("localhost");
            manager.PollPrinter(drucker);

            manager.PrinterDatabase.UpdatePrinter(drucker);


            Console.Title = "PICO - PRINFO INTERACTIVE CONSOLE";

            Out("PICO - PRINFO INTERACTIVE CONSOLE" + Environment.NewLine + "--------------------------------------"  + Environment.NewLine);
            Out(resi.GetString("startup-load"));
            Status();
            Out(resi.GetString("type-help"));

            string command = String.Empty;

            #region pico command loop

            while (true)
            {
                Console.Write("pico> ");
                command = In();

                if (command.Equals("help") || command.Equals("?"))
                    HelpMessage();
                else if (command.Equals("exit"))
                    break;
                else if (command.Equals("clear"))
                    Console.Clear();
                else if (command.Equals("status"))
                    Status();
                else if (command.Contains("add printer"))
                {
                    AddPrinter(command);
                }
                else if (command.Equals("check-all"))
                {
                    CheckPrinterList();
                }
                else if (command.Equals("init-db"))
                {
                    new PrinterDatabase().Initialize();
                    Out("Data wiped successfully");
                }
                else if (command.Contains("service"))
                {
                    ExecuteServiceCommand(command);
                }
                else if (command.Contains("switch"))
                {
                    SwitchLanguage(command);
                }
                else if (command.Contains("import"))
                {
                    Import(command);
                }
                else if (command.Equals("print"))
                {
                    Console.WriteLine();
                    printerManager.LoadPrinterList();
                    foreach (Printer p in printerManager)
                    {
                        Console.WriteLine(p);
                    }
                }
                else
                {
                    Out(resi.GetString("unknown-command"));
                }
            }
            #endregion
        }

        /// <summary>
        /// import function
        /// </summary>
        /// <param name="command">the command</param>
        static void Import(string command)
        {
            Match match = Regex.Match(command, "^import (.*)$");

            if (match.Success)
            {
                try
                {
                    CSVImport import = new CSVImport(match.Groups[1].Value);
                    Out(resi.GetString("importing"));
                    import.LoadData();
                    Out(resi.GetString("saving-data"));
                    import.Save();
                }
                catch (ApplicationException)
                {
                    Out(resi.GetString("file-not-exists"));
                }
                
            }
            else
            {
                Out(resi.GetString("syntax-error"));
            }
        }

        /// <summary>
        /// executes several service controlls
        /// </summary>
        /// <param name="command">the read command</param>
        static void ExecuteServiceCommand(string command)
        {
            Match match = Regex.Match(command, "^(start|stop|install|uninstall) service$");

            #region start the service
            if (match.Groups[1].Value.Equals("start"))
            {
                try
                {
                    ServiceManager.PrinfoService.Start();
                    Out("Service started succesfull");
                }
                catch
                {
                    Out("Couldn`t start the service. Service already running? ");
                }
            }
            #endregion start the service

            #region stop the service
            else if (match.Groups[1].Value.Equals("stop"))
            {
                try
                {
                    ServiceManager.PrinfoService.Stop();
                    Out("Service stopped succesfull");
                }
                catch 
                {
                    Out("There was a problem with the service. Service stopped already?");
                }
            }
            #endregion stop the service

            #region install the service
            else if (match.Groups[1].Value.Equals("install"))
            {
                try
                {
                    ServiceManager.InstallPrinfoService();
                }
                catch
                {
                    Out("Couldn`t install the service. Is the service installed already?");
                }
            }
            #endregion

            #region uninstall the service
            else if (match.Groups[1].Value.Equals("uninstall"))
            {
                try
                {
                    ServiceManager.UninstallPrinfoService();
                }
                catch
                {
                    Out("Couldn`t uninstall the service. Is the service installed?");
                }
            }
            #endregion
            else
            {

                Out(resi.GetString("syntax-error"));

            }
        }

        /// <summary>
        /// switches the language
        /// </summary>
        /// <param name="lang">the command</param>
        static void SwitchLanguage(string command)
        {
            Match match = Regex.Match(command, "^switch language (.*)$");
            if (match.Success)
            {
                if(match.Groups[1].Value.Equals("english"))
                    GlobalizationHelper.SwitchCulture(Language.English);
                else if(match.Groups[1].Value.Equals("deutsch"))
                    GlobalizationHelper.SwitchCulture(Language.German);
                else
                    Out(resi.GetString("language-not-supported"));
            }
            else
            {
                Out(resi.GetString("syntax-error"));
            }
            
        }

        /// <summary>
        /// checks every printer in the database and updates it`s data
        /// </summary>
        static void CheckPrinterList()
        {
            stopwatch.Start();
            Out(resi.GetString("loading-printerlist"));
            printerManager.LoadPrinterList();

            Out(resi.GetString("checking-printer"));
            ProcBar procBar = new ProcBar();
            int counter = 0;

            foreach (Printer p in printerManager)
            { 
                

                printerManager.PollPrinter(p, true);

                printerManager.PrinterDatabase.UpdatePrinter(p);

                counter++;
                procBar.ProcentualValue = (int)((double)counter / printerManager.PrinterList.Count * 100);
                procBar.Draw();
            }

            stopwatch.Stop();
            Out("\n" + resi.GetString("finished-after") + stopwatch.ElapsedMilliseconds / 1000  + " " + resi.GetString("seconds"));
            stopwatch.Reset();
        }

        /// <summary>
        /// adds a printer to the database
        /// </summary>
        /// <param name="command">the read command</param>
        static void AddPrinter(string command)
        {
            Match match = Regex.Match(command, @"add printer ([^\s]+)$");
            if (match.Success)
            {
                new PrinterDatabase().CreatePrinter(match.Groups[1].Value);
                Out(resi.GetString("printer-added"));
            }
            else
                Out(resi.GetString("syntax-error"));                    

        }

        /// <summary>
        /// prints a command overview
        /// </summary>
        static void HelpMessage()
        {
            Out(Environment.NewLine + "COMMAND OVERVIEW" + Environment.NewLine + "---------------------------------");
            Out("command   |   description" + Environment.NewLine + "------------------------------");
            for (int i = 0; i < commandList.GetLength(0); i++)
            {
                Out(commandList[i, 0] + "  |   " + commandList[i, 1]);
            }
            

        }

        /// <summary>
        /// output of a basic status message
        /// </summary>
        static void Status()
        {
            printerManager.LoadPrinterList();

            //supply counter
            int supplies = 0;
            printerManager.PrinterList.ForEach(
                printer =>
                {
                    if (printer.Supplies != null)
                        supplies += printer.Supplies.Count;
                });

            Out("Status" + Environment.NewLine + "--------------------");

            try
            {
                Out(resi.GetString("service-currently") + ServiceManager.PrinfoService.Status);
                
            }
            catch
            {
                Out(resi.GetString("service-error"));
            }
            Out(resi.GetString("there-are-currently") + printerManager.PrinterList.Count + resi.GetString("printer-database-amount") + supplies + resi.GetString("supplies"));

        }

        /// <summary>
        /// Console.WriteLine shortcut
        /// </summary>
        /// <param name="str">the object to print to stdout</param>
        static void Out(object str)
        {
            Console.WriteLine(str);
        }

        /// <summary>
        /// Console.ReadLine shortcut
        /// </summary>
        /// <returns>the input that has been read from the stdin</returns>
        static string In()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// basic "press any key" message
        /// </summary>
        static void PressAnyKey()
        {
            Out(Environment.NewLine + "press any key");
            In();
        }


    }


}
