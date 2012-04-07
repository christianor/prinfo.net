using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Schnittstelle zur Druckerdatenbank
    /// </summary>
    public class PrinterDatabase : DatabaseObject
    {
        /// <summary>
        /// Legt das Data-Verzeichnis an falls dieses nicht existiert und sorgt dafür dass die
        /// Datenbankschnittstelle und der ConnectionString geladen werden
        /// <remarks>
        /// Sollte das Datenbank-Backend geändert werden muss im Konstruktor die Schnittstelle angepasst werden
        /// </remarks>
        /// </summary>
        public PrinterDatabase()
        {
            DirectoryController.CreateDataDirIfNotExists();

            //set a SQLiteDatabase as data interface
            DatabaseInterface = new SQLiteDatabaseFactory();
            //Sqlite connection string
            ConnectionString = "Data Source=" + DirectoryController.DataDirectoryLocation + "printer.db;Version=3;";
            
        }

        /// <summary>
        /// Erstellt das Schema der Datenbanktabelle
        /// </summary>
        public override void Initialize()
        {
            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("DROP TABLE IF EXISTS printers; DROP TABLE IF EXISTS supplies;"
                + "CREATE TABLE printers (id GUID PRIMARY KEY, pingable TEXT DEFAULT False, hostname TEXT, manufacturer TEXT, model TEXT, pageCount INTEGER DEFAULT -1, pageCountColor INTEGER DEFAULT -1, lastCheck TEXT,description TEXT, globalAlertOfflineTriggered TEXT DEFAULT False, upTime TEXT, sysContact TEXT, sysLocation TEXT);"
                + "CREATE TABLE supplies (id INTEGER PRIMARY KEY, printerId GUID, description TEXT, value INTEGER, globalAlertTriggered TEXT DEFAULT False, notifyWhenLow TEXT DEFAULT False, notified TEXT DEFAULT False, notificationValue INTEGER DEFAULT 0);"
                ,
                connection))
            {
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Prüft ob das Datenbankschema vorhanden ist
        /// </summary>
        /// <param name="throwException">Gibt an ob Ausnahmen geworfen werden sollen</param>
        /// <returns>Wahr falls das Schema eingehalten wird</returns>
        public override bool PerformIntegrityCheck(bool throwException = false)
        {
            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT " + 
                "id, pingable, hostname, manufacturer, model, pageCount, pageCountColor, lastCheck, description, globalAlertOfflineTriggered, upTime, sysContact, sysLocation" +
                " FROM printers LIMIT 0;" +
                "SELECT id, printerId, description, value, notifyWhenLow, notified, notificationValue, globalAlertTriggered FROM supplies LIMIT 0", connection))
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

        #region Create, Update, Delete PrinterList

        /// <summary>
        /// Fügt einen Drucker zur Datenbank hinzu
        /// </summary>
        /// <param name="hostname">Der Hostname des Druckers</param>
        /// <returns>Instanz des angelegten Druckers mit der Datenbank Id</returns>
        public Printer CreatePrinter(string hostname)
        {
            Printer printer = new Printer { HostName = hostname, Id = Guid.NewGuid().ToString() };

            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("INSERT INTO printers (id, hostname) VALUES (@Id, @HostName)", connection))
            {
                command.Parameters.Add(DatabaseInterface.CreateParameter("@HostName", printer.HostName));
                command.Parameters.Add(DatabaseInterface.CreateParameter("@Id", printer.Id));

                connection.Open();
                command.ExecuteNonQuery();

                return printer;
            }
        }

        /// <summary>
        /// Löscht den Drucker und seine Supplies aus der Datenbank
        /// </summary>
        /// <param name="id">Die Id des Druckers der gelöscht werden soll</param>
        public void DeletePrinterByID(string id)
        {
            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand
                (
                    //Delete the supplies
                    "DELETE FROM supplies WHERE printerId = @Id;" +
                    //Delete the printer
                    "DELETE FROM printers WHERE id = @Id;"
                            
                , connection))
            {
                command.Parameters.Add(DatabaseInterface.CreateParameter("@Id", id));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update des Druckers, 
        /// Verbrauchsteile werden dynamisch erstellt, aktualisiert und gelöscht falls nötig
        /// </summary>
        /// <param name="printer">Der Drucker der in der Datenbank aktualisiert werden soll</param>
        ///
        public void UpdatePrinter(Printer printer)
        {
            if (string.IsNullOrEmpty(printer.Id))
                throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("printer_update_failed_no_id"));

            try
            {
                using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
                {
                    connection.Open();

                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        using (IDbCommand command = DatabaseInterface.CreateCommand("UPDATE printers SET hostname = @Hostname, manufacturer = @Manufacturer, model = @Model, pingable = @Pingable, pageCount = @PageCount, pageCountColor = @PageCountColor, lastCheck = @LastCheck, description = @Description, globalAlertOfflineTriggered = @GlobalAlertOffline, sysLocation = @SysLocation, sysContact = @SysContact, upTime = @UpTime WHERE id = @Id", connection, transaction))
                        {

                            command.Parameters.Add(DatabaseInterface.CreateParameter("@Hostname", printer.HostName));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@Manufacturer", printer.Manufacturer));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@Model", printer.Model));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@Pingable", printer.Pingable.ToString()));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@Id", printer.Id));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@PageCount", printer.PageCount.ToString()));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@PageCountColor", printer.PageCountColor.ToString()));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@LastCheck", printer.LastCheck));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@Description", printer.Description));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@GlobalAlertOffline", printer.GlobalAlertOfflineTriggered.ToString()));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@UpTime", printer.UpTime));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@SysLocation", printer.SysLocation));
                            command.Parameters.Add(DatabaseInterface.CreateParameter("@SysContact", printer.SysContact));

                            /* if the printer that has been updated exists in the database, 
                             * (may be deleted in the meantime) proceed creating or updating 
                             * the supplies
                             */
                            if (command.ExecuteNonQuery() != 0)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add(DatabaseInterface.CreateParameter("@PrinterId", printer.Id));

                                //supply association
                                if (printer.Supplies.Count > 0)
                                {

                                    /*
                                     * try to update the supply with the given id if the id is not null
                                     * if the supply id is null insert the new supply to the database
                                     */

                                    bool anySupplyNoId = printer.AnySupplyNoId();

                                    if (anySupplyNoId)
                                    {
                                        command.CommandText = "DELETE FROM supplies WHERE printerId = @PrinterId";
                                        command.ExecuteNonQuery();
                                    }


                                    // supply parameters
                                    IDbDataParameter descriptionField = DatabaseInterface.CreateParameter("@Description");
                                    IDbDataParameter valueField = DatabaseInterface.CreateParameter("@Value");
                                    IDbDataParameter idField = DatabaseInterface.CreateParameter("@Id");
                                    IDbDataParameter globalAlertField = DatabaseInterface.CreateParameter("@GlobalAlert");
                                    IDbDataParameter notifyWhenLowField = DatabaseInterface.CreateParameter("@NotifyWhenLow");
                                    IDbDataParameter notifyValueField = DatabaseInterface.CreateParameter("@NotificationValue");
                                    IDbDataParameter notifiedField = DatabaseInterface.CreateParameter("@Notified");

                                    command.Parameters.Add(descriptionField);
                                    command.Parameters.Add(valueField);
                                    command.Parameters.Add(idField);
                                    command.Parameters.Add(globalAlertField);
                                    command.Parameters.Add(notifyWhenLowField);
                                    command.Parameters.Add(notifyValueField);
                                    command.Parameters.Add(notifiedField);

                                    foreach (Supply supply in printer.Supplies)
                                    {
                                        descriptionField.Value = supply.Description;
                                        valueField.Value = supply.Value.ToString();
                                        idField.Value = supply.Id.ToString();
                                        globalAlertField.Value = supply.GlobalAlertTriggered.ToString();
                                        notifyWhenLowField.Value = supply.NotifyWhenLow.ToString();
                                        globalAlertField.Value = supply.GlobalAlertTriggered.ToString();
                                        notifyValueField.Value = supply.NotificationValue.ToString();
                                        notifiedField.Value = supply.Notified.ToString();

                                        if (anySupplyNoId)
                                        {
                                            command.CommandText = "INSERT INTO supplies (id, description, value, printerId, globalAlertTriggered, notifyWhenLow, notificationValue, notified) VALUES (null, @Description, @Value, @PrinterId, @GlobalAlert, @NotifyWhenLow, @NotificationValue, @Notified)";
                                            command.ExecuteNonQuery();

                                            //set the supply id in the object - pretends from reloading the printer list
                                            command.CommandText = "SELECT last_insert_rowid();";
                                            using (IDataReader reader = command.ExecuteReader())
                                            {

                                                if (reader.Read())
                                                {
                                                    try { supply.Id = int.Parse(reader[0].ToString()); }
                                                    catch (FormatException) { }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            command.CommandText = "UPDATE supplies SET value = @Value, globalAlertTriggered = @GlobalAlert, notifyWhenLow = @NotifyWhenLow, notificationValue = @NotificationValue, notified = @Notified WHERE id = @Id";
                                            command.ExecuteNonQuery();
                                        }
                                    }

                                    command.CommandText = "SELECT id FROM printers WHERE id = @PrinterId";

                                    // check if printer still exists if not, rollback
                                    using(IDataReader reader = command.ExecuteReader())
                                    {
                                        if (!reader.Read())
                                        {
                                            transaction.Rollback();
                                            throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("printer_update_failed_wrong_id"));
                                        }
                                    }
                                }
                                else
                                {
                                    command.CommandText = "DELETE FROM supplies WHERE printerId = @PrinterId";
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                                throw new ApplicationException(GlobalizationHelper.LibraryResource.GetString("printer_update_failed_wrong_id"));
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (ApplicationException ae)
            {
                throw ae;
            }
            // e.g. database file locked exception or any other sqlite exception
            catch (Exception e)
            {
                throw new ApplicationException(String.Format(GlobalizationHelper.LibraryResource.GetString("printer_update_aborted"), e.Message), e);
            }
        }

        #endregion

        /// <summary>
        /// Laden der Druckerdaten aus der Datenbank in eine dynamische Liste von Druckerobjekten
        /// </summary>
        /// <returns>Die Liste der Drucker</returns>
        public List<Printer> GetPrinterList()
        {
            List<Printer> printerList = new List<Printer>();

            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT * FROM printers", connection))
            {
                connection.Open();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {                
                        Printer printer = DeserializePrinter(reader, connection);
                        printerList.Add(printer);
                    }
                }
            }

            foreach (Printer p in printerList)
            {
                p.Supplies = GetSupplyList(p);

            }

            return printerList;
        }

        /// <summary>
        /// Weist die Spalten-Werte anhand des übergeben DataReaders einem neuen Druckerobjekt zu
        /// </summary>
        /// <param name="reader">Der Vorbereitete IDataReader</param>
        /// <param name="connection">Die verwendete Verbindung</param>
        /// <returns>Das neu erstellte Druckerobjekt anhand des Datensatzes aus der Datenbank</returns>
        private Printer DeserializePrinter(IDataReader reader, IDbConnection connection)
        {

            Printer printer = new Printer { 
                Id = reader["id"].ToString(), 
                HostName = reader["hostname"].ToString(), 
                Manufacturer = reader["manufacturer"].ToString(), 
                Model = reader["model"].ToString(), 
                LastCheck = reader["lastCheck"].ToString(),
                Description = reader["description"].ToString(),
                Pingable = bool.Parse(reader["pingable"].ToString()),
                GlobalAlertOfflineTriggered = bool.Parse(reader["globalAlertOfflineTriggered"].ToString()),
                UpTime = reader["upTime"].ToString(),
                SysContact = reader["sysContact"].ToString(),
                SysLocation = reader["sysLocation"].ToString()
            };
            

            try
            {
                printer.PageCount = int.Parse(reader["pageCount"].ToString());
            }
            catch (FormatException)
            {
                printer.PageCount = -1;
            }

            try
            {
                printer.PageCountColor = int.Parse(reader["pageCountColor"].ToString());
            }
            catch (FormatException)
            {
                printer.PageCountColor = -1;
            }

            return printer;
        }

        /// <summary>
        /// Lädt den Drucker anhand seiner Id in ein Druckerobjekt
        /// </summary>
        /// <param name="id">Die Id des Druckers (GUID)</param>
        /// <returns>
        /// Eine neue Drucker Instanz mit den Datenbankwerten des gesuchten Druckers
        /// <remarks>Gibt null zurück falls der Drucker nicht gefunden wurde</remarks>
        /// </returns>
        public Printer GetPrinterById(string id)
        {
            Printer printer = null;

            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT * FROM printers WHERE id = @PrinterId", connection))
            {
                command.Parameters.Add(DatabaseInterface.CreateParameter("@PrinterId", id));

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        printer = DeserializePrinter(reader, connection);

                    else printer = null;
                }
            }

            printer.Supplies = GetSupplyList(printer);

            return printer;
        }

        /// <summary>
        /// Lädt die Supplies eines Druckers aus der Datenbank
        /// </summary>
        /// <param name="printer">
        /// Das Drucker Objekt<remarks>Benötigt Datenbank Id</remarks></param>
        /// <returns>Die Liste der Verbrauchsteile</returns>
        public List<Supply> GetSupplyList(Printer printer)
        {
            List<Supply> supplies = new List<Supply>();

            using (IDbConnection connection = DatabaseInterface.CreateConnection(ConnectionString))
            using (IDbCommand command = DatabaseInterface.CreateCommand("SELECT * FROM supplies WHERE printerId = @PrinterId ORDER BY id", connection))
            {
                command.Parameters.Add(DatabaseInterface.CreateParameter("@PrinterId", printer.Id));
                connection.Open();
                //read the supplies
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Supply newSupply = new Supply 
                        { 
                            Id = int.Parse(reader["id"].ToString()), 
                            Description = reader["description"].ToString(), 
                            Value = double.Parse(reader["value"].ToString()), 
                            GlobalAlertTriggered = bool.Parse(reader["globalAlertTriggered"].ToString()), 
                            NotificationValue = int.Parse(reader["notificationValue"].ToString()), 
                            Notified = bool.Parse(reader["notified"].ToString()), 
                            NotifyWhenLow = bool.Parse(reader["notifyWhenLow"].ToString()),
                        };

                        supplies.Add(newSupply);
                    }
                    return supplies;
                }
            }
        }


    }
}
     
