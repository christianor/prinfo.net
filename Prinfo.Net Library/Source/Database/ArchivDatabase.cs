using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Schnittstelle zur Archivdatenbank
    /// </summary>
    public class ArchivDatabase : DatabaseObject
    {
        /// <summary>
        /// Instanziert ein ArchivDatabase Objekt. 
        /// Legt das Data-Verzeichnis an falls dieses nicht existiert und sorgt dafür dass die
        /// Datenbankschnittstelle und der ConnectionString geladen werden
        /// <remarks>
        /// Sollte das Datenbank-Backend geändert werden muss im Konstruktor die Schnittstelle angepasst werden
        /// </remarks>
        /// </summary>
        public ArchivDatabase()
        {
            DirectoryController.CreateDataDirIfNotExists();

            //set a SQLiteDatabase as data interface
            DatabaseInterface = new SQLiteDatabaseFactory();
            //Sqlite connection string
            ConnectionString = "Data Source=" + DirectoryController.DataDirectoryLocation + "archiv.db;Version=3;";

        }

        /// <summary>
        /// Initialisiert das Datenbankschema
        /// </summary>
        public override void Initialize()
        {
            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("DROP TABLE IF EXISTS archiv_printer_entries;DROP TABLE IF EXISTS archiv_supply_entries;"
                + "CREATE TABLE archiv_printer_entries " +
                "(id INTEGER PRIMARY KEY, printerId GUID, pageCount INTEGER DEFAULT -1, pageCountColor INTEGER DEFAULT -1, lastCheck TEXT);" +
                "CREATE TABLE archiv_supply_entries " +
                "(id INTEGER PRIMARY KEY, supplyId INTEGER, printerId GUID, value INTEGER);",
                connection))
            {
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Führt einen Schematest durch
        /// </summary>
        /// <param name="throwException">Gibt an ob Ausnahmen geworfen werden sollen</param>
        /// <returns>Wahr wenn der Integritätstest erfolgreich abgeschlossen wurde</returns>
        public override bool PerformIntegrityCheck(bool throwException = false)
        {
            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT " +
                "id, printerId, pageCount, pageCountColor, lastCheck" +
                " FROM archiv_printer_entries LIMIT 0; SELECT id, supplyId, printerId, value FROM archiv_supply_entries LIMIT 0;", connection))
            {

                //connect to the datafile
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (throwException)
                        throw e;

                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Fügt ein Drucker Archiveintrag in das Archiv hinzu, versucht dabei den Eintrag des aktuellen
        /// Tages stehts zu überschreiben. Je Tag steht also ein Archiveintrag zur verfügung
        /// </summary>
        /// <param name="printer">Das Druckerobjekt</param>
        public void AddEntry(Printer printer)
        {
            var printerIdParam = DatabaseInterface.CreateParameter("@PrinterId", printer.Id);
            var printerPageCountParam = DatabaseInterface.CreateParameter("@PageCount", printer.PageCount.ToString());
            var printerPageCountColorParam = DatabaseInterface.CreateParameter("@PageCountColor", printer.PageCountColor.ToString());
            var printerLastCheckParam = DatabaseInterface.CreateParameter("@LastCheck", printer.LastCheck);

            try
            {
                // prüfen ob für diesen tag bereits ein eintrag besteht
                using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
                using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT * FROM archiv_printer_entries WHERE lastCheck LIKE ('" + DateTime.Parse(printer.LastCheck).ToString("dd.MM.yyyy") + "%') AND printerId = '" + printer.Id + "'", connection))
                {
                    command.Parameters.Add(printerIdParam);
                    command.Parameters.Add(printerPageCountParam);
                    command.Parameters.Add(printerPageCountColorParam);
                    command.Parameters.Add(printerLastCheckParam);

                    //connect to the datafile
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            var id = reader["id"].ToString();
                            reader.Close();

                            command.CommandText = "UPDATE archiv_printer_entries set pageCount = @PageCount, pageCountColor = @PageCountColor, lastCheck = @LastCheck WHERE id = " + id;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            reader.Close();

                            command.CommandText = "INSERT INTO archiv_printer_entries (id, printerId, pageCount, pageCountColor, lastCheck) VALUES " +
                                "(null, @PrinterId, @PageCount, @PageCountColor, @LastCheck)";


                            command.ExecuteNonQuery();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("printer_update_aborted"), e.Message), e);

            }
        }

        /// <summary>
        /// Holt Archiveinträge anhand der Übergebenen Drucker Id
        /// </summary>
        /// <param name="printerId">Die Id des Druckers</param>
        /// <returns>Liste aller Drucker die zur übergebenen Id gefunden wurden</returns>
        public List<Printer> GetEntriesById(string printerId)
        {
            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT " +
                "pageCount, pageCountColor, lastCheck FROM archiv_printer_entries WHERE printerId = '"+printerId+"'", connection))
            {

                //connect to the datafile
                connection.Open();

                var reader = command.ExecuteReader();

                List<Printer> printerList = new List<Printer>();

                while (reader.Read())
                {
                    printerList.Add(new Printer
                    {
                        PageCount = int.Parse(reader["pageCount"].ToString()),
                        PageCountColor = int.Parse(reader["pageCountColor"].ToString()),
                        LastCheck = reader["lastCheck"].ToString()
                    });
                }

                return printerList;

            }
        }

    }
}
     
