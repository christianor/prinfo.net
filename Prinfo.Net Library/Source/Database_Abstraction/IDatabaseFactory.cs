using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Schnittstelle für DatenbankFabrik
    /// </summary>
    public interface IDatabaseFactory
    {
        /// <summary>
        /// Erstellen einer Verbindung
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();
        /// <summary>
        /// Erstellen einer Verbindung anhand des ConnectionStrings
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection(string connectionString);
        /// <summary>
        /// Erstelle einen SQL Befehl
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand();
        /// <summary>
        /// Erstelle einen SQL Befehl
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand(string commandString, IDbConnection connection);
        /// <summary>
        /// Erstelle einen SQL Befehl
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand(string commandString, IDbConnection connection, IDbTransaction transaction);
        /// <summary>
        /// Erstellt einen SQL Parameter
        /// </summary>
        /// <returns></returns>
        IDbDataParameter CreateParameter();
        /// <summary>
        /// Erstellt einen SQL Parameter
        /// </summary>
        /// <returns></returns>
        IDbDataParameter CreateParameter(string parameterName);
        /// <summary>
        /// Erstellt einen SQL Parameter
        /// </summary>
        /// <returns></returns>
        IDbDataParameter CreateParameter(string parameterName, string parameterValue);
    }
}
