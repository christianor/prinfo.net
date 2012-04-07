using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Sqlite Datenbankfabrik
    /// </summary>
    public class SQLiteDatabaseFactory : IDatabaseFactory
    {
        /// <summary>
        /// Erstelle einen SQL Connection
        /// </summary>
        /// <returns>Die Connection</returns>
        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection();
        }
        /// <summary>
        /// Erstelle eine SQL Connection
        /// </summary>
        /// <param name="connectionString">Der ConnectionString</param>
        /// <returns>Die Connection</returns>
        public IDbConnection CreateConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// Erstelle einen SQL Befehl
        /// </summary>
        /// <returns>Den Befehl</returns>
        public IDbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }
        /// <summary>
        /// Erstelle einen SQL Befehl
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand(string commandString, IDbConnection connection)
        {
            return new SQLiteCommand(commandString, (SQLiteConnection)connection);
        }

        /// <summary>
        /// Erstelle einen SQL Befehl
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand(string commandString, IDbConnection connection, IDbTransaction transaction)
        {
            return new SQLiteCommand(commandString, (SQLiteConnection)connection, (SQLiteTransaction)transaction);
        }

        /// <summary>
        /// Generiert einen Paramater
        /// </summary>
        /// <returns>Den Parameter</returns>
        public IDbDataParameter CreateParameter()
        {
            return new SQLiteParameter();
        }

        /// <summary>
        /// Erstellen eines Parameters
        /// </summary>
        /// <param name="parameterName">Der Name des Parameters</param>
        /// <returns>Den erstellten Parameter</returns>
        public IDbDataParameter CreateParameter(string parameterName)
        {
            return new SQLiteParameter(parameterName);
        }

        /// <summary>
        /// Erstellen eines Parameters
        /// </summary>
        /// <param name="parameterName">Parametername</param>
        /// <param name="parameterValue">Parameterwert</param>
        /// <returns></returns>
        public IDbDataParameter CreateParameter(string parameterName, string parameterValue)
        {
            return new SQLiteParameter(parameterName, parameterValue);
        }
    }
}
