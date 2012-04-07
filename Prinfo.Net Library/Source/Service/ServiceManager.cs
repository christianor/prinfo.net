using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Klasse ServiceManager, ermöglicht leichten Zugriff auf den Dienst
    /// <remarks>Achten Sie darauf das für den Zugriff auf die Dienste die Anwendung die diese Funktionalität
    /// verwendet ab Windows Vista als Administrator ausgeführt werden muss, da sonst der Zugriff fehlschlägt
    /// </remarks>
    /// </summary>
    public static class ServiceManager
    {

        #region events

        /// <summary>
        /// Wird vor dem Beginn der Installation ausgelöst
        /// </summary>
        public static event InstallEventHandler BeforeInstall;
        /// <summary>
        /// Wird nach der Installation gefeuert
        /// </summary>
        public static event InstallEventHandler AfterInstall;
        /// <summary>
        /// Wird ausgelöst bevor die Deinstallation begonnen wird
        /// </summary>
        public static event InstallEventHandler BeforeUninstall;
        /// <summary>
        /// Wird ausgelöst sobald die Deinstallation abgeschlossen ist
        /// </summary>
        public static event InstallEventHandler AfterUninstall;

        #endregion

        static private ServiceController _prinfoService;
        /// <summary>
        /// Direkter, aktuallisierter Zugriff auf den Dienst
        /// </summary>
        static public ServiceController PrinfoService
        {
            get
            {
                if (_prinfoService == null)
                    _prinfoService = new ServiceController("Prinfo.Net Service");

                _prinfoService.Refresh();
                return _prinfoService;
            }
        }

        /// <summary>
        /// Startet den Dienst neu
        /// </summary>
        static public void RestartService()
        {
            int timeoutMilliseconds = 2000;
            int millisec1 = Environment.TickCount;
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

            try
            {
                PrinfoService.Stop();
                PrinfoService.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch { }

            // count the rest of the timeout
            int millisec2 = Environment.TickCount;
            timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

            PrinfoService.Start();
            PrinfoService.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        /// <summary>
        /// Installiert den Dienst
        /// </summary>
        static public void InstallPrinfoService()
        {
            using (TransactedInstaller ti = new TransactedInstaller())
            {

                if (BeforeInstall != null)
                    ti.BeforeInstall += new InstallEventHandler(BeforeInstall);
                if (AfterInstall != null)
                    ti.AfterInstall += new InstallEventHandler(AfterInstall);

                AssemblyInstaller asmi = new AssemblyInstaller(AppDomain.CurrentDomain.BaseDirectory + "\\Prinfo.Net Service.exe", null);

                ti.Installers.Add(asmi);
                ti.Install(new Hashtable());
            }
        }

        /// <summary>
        /// Deinstalliert den Dienst
        /// </summary>
        static public void UninstallPrinfoService()
        {
            using (TransactedInstaller ti = new TransactedInstaller())
            {

                if (BeforeUninstall != null)
                    ti.BeforeUninstall += new InstallEventHandler(BeforeUninstall);
                if (AfterUninstall != null)
                    ti.AfterUninstall += new InstallEventHandler(AfterUninstall);

                AssemblyInstaller asmi = new AssemblyInstaller(AppDomain.CurrentDomain.BaseDirectory + "\\Prinfo.Net Service.exe", null);

                ti.Installers.Add(asmi);
                ti.Uninstall(null);
            }
        }
    }
}
