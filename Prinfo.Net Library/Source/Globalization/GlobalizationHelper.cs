using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Hilfsklasse die den Umgang mit der Ressourcendatei vereichnfacht
    /// </summary>
    public static class GlobalizationHelper
    {
        /// <summary>
        /// Die Ressourcendatei der Klassenbibliothek
        /// </summary>
        public static ResourceManager LibraryResource = new ResourceManager("com.monitoring.prinfo.Resource.res", Assembly.GetExecutingAssembly());
        
        /// <summary>
        /// Ändern der CultureInfo des Threads
        /// </summary>
        /// <param name="lang">Die Zielsprache</param>
        public static void SwitchCulture(Language lang)
        {
            switch (lang)
            {
                case Language.German:
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("de-DE");
                    break;
                case Language.English:
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
                    break;
            }
        }
    }
}
