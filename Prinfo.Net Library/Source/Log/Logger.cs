using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Statische Klasse die einfache Loggingfunktionalität bereitstellt
    /// </summary>
    public static class Logger
    {
        //locks for the threadsafety
        private static Object writeLockErrors = new Object();
        private static Object writeLockPrinter = new Object();
        private static Object writeLockService = new Object();

        /// <summary>
        /// Schreibt Daten + Uhrzeit in Logfiles
        /// <remarks>Die Methode ist Threadsicher</remarks>
        /// </summary>
        /// <param name="output">Die Zeichenkette</param>
        /// <param name="logType">Die Art der Logdatei</param>
        public static void Log(string output, LogType logType)
        {
            Object writeLock = null;
            int maxRetry = 30;
            String fileName = "";

            //select the type of lock, prevents from writing in multi threaded applications at the same time to files
            if (logType == LogType.Error)
            {
                writeLock = writeLockErrors;
                fileName = DirectoryController.LogDirectoryLocation + "error.log";
            }
            else if (logType == LogType.Printer)
            {
                writeLock = writeLockPrinter;
                fileName = DirectoryController.LogDirectoryLocation + "printer.log";
            }
            else if (logType == LogType.Service)
            {
                writeLock = writeLockService;
                fileName = DirectoryController.LogDirectoryLocation + "service.log";
            }

            //lock for the file access
            lock (writeLock)
            {
                new Thread(() =>
                    {
                        //check for a file size smaller then 5 mb, else delete the file
                        FileInfo fnfo = new FileInfo(fileName);
                        if (fnfo.Exists)
                            if (fnfo.Length >= 5242880)
                                fnfo.Delete();

                        //retry loggin "maxRetry"-times
                        for (int i = 0; i < maxRetry; i++)
                        {
                            try
                            {
                                File.AppendAllText(fileName, DateTime.Now + "| " + output + Environment.NewLine);
                                return;
                            }
                            catch (DirectoryNotFoundException)
                            {
                                DirectoryController.CreateLogDirIfNotExists();
                            }
                            //probably another process locks the file
                            //this is a work-around
                            catch (IOException)
                            {
                                //wait a random amount of time (max 100 ms) before relogging
                                Thread.Sleep(new Random(DateTime.Now.Millisecond).Next(100));
                            }
                        }
                    }).Start();
            }
        }
    }
}
