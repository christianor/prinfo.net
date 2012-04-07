using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace com.monitoring.prinfo
{
    /// <summary>
    ///  service installer
    /// </summary>
    [RunInstaller(true)]
    public class PrinfoServiceInstaller : Installer
    {

        public PrinfoServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //service Information

            serviceInstaller.DisplayName = "Prinfo.NET Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = "Prinfo.NET Service";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}