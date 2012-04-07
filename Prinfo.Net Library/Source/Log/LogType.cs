
namespace com.monitoring.prinfo
{
    /// <summary>
    /// Die Art der Logdatei die verwendet werden soll
    /// Wird verwendet in <see cref="Logger"/>
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// logs into error file
        /// </summary>
        Error,
        /// <summary>
        /// logs into printer file
        /// </summary>
        Printer,
        /// <summary>
        /// logs into service file
        /// </summary>
        Service
    }
}
