﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

/**
 * @author  Haran
 * @date    2021-03-27
 */
namespace TarneebClasses
{
    /// <summary>
    /// A row in the Games table.
    /// </summary>
    public struct DatabaseGameEntry
    {
        /// <summary>
        /// The game identifier.
        /// </summary>
        public readonly int GameID;

        /// <summary>
        /// The date and time that the game started.
        /// </summary>
        public readonly DateTime Start;

        /// <summary>
        /// Create a new DatabaseGameEntry.
        /// </summary>
        /// <param name="gameId">The identifier for the game.</param>
        /// <param name="start">The date and time that the game started.</param>
        public DatabaseGameEntry(int gameId, DateTime start)
        {
            this.GameID = gameId;
            this.Start = start;
        }
    }

    /// <summary>
    /// Provides database functionality, such as storing logs and statistics.
    /// </summary>
    public static class Database
    {
        #region Queries
        /// <summary>
        /// TODO
        /// Used to check if tables exist.
        /// </summary>
        private const string STMT_GET_TABLES = (
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Logs' OR TABLE_NAME = 'Statistics' OR TABLE_NAME = 'Games';"
        );

        /// <summary>
        /// Statement to create all required tables.
        /// </summary>
        private const string STMT_CREATE_TABLES = (@"CREATE TABLE Logs (DateTime DATETIME, GameID INT, Action TEXT); 
CREATE TABLE Stats (DateTime DATETIME, GameID INT, Outcome TINYINT);
CREATE TABLE Games (GameID INT PRIMARY KEY IDENTITY(1, 1), Start DATETIME);");

        /// <summary>
        /// Statement to create any required indexes, to optimize database lookups.
        /// </summary>
        private const string STMT_CREATE_INDEXES = "CREATE INDEX StatsOutcomeIdx ON Stats (Outcome);";

        /// <summary>
        /// Statement to drop all tables.
        /// </summary>
        private const string STMT_DROP_TABLES = "DROP TABLE IF EXISTS Logs; DROP TABLE IF EXISTS Stats; DROP TABLE IF EXISTS Games;";

        /// <summary>
        /// Statement to add a log to the logs table.
        /// </summary>
        private const string STMT_INSERT_LOG = "INSERT INTO Logs VALUES (@DateTime, @GameID, @Action);";

        /// <summary>
        /// Statement to add a game outcome into the statistics table.
        /// </summary>
        private const string STMT_INSERT_OUTCOME = "INSERT INTO Stats VALUES (@DateTime, @GameID, @Outcome);";

        /// <summary>
        /// TODO
        /// </summary>
        private const string STMT_INSERT_GAME = "INSERT INTO Games (Start) VALUES (@Start); SELECT @@IDENTITY;";

        /// <summary>
        /// Statement to get logs from the database.
        /// </summary>
        private const string STMT_GET_LOGS = "SELECT DateTime, GameID, Action FROM Logs;";

        /// <summary>
        /// TODO
        /// </summary>
        private const string STMT_GET_LOGS_FOR_GAME = "SELECT DateTime AS \"Date and Time\",  Action FROM Logs WHERE GameID = @GameID;";

        /// <summary>
        /// TODO
        /// </summary>
        private const string STMT_GET_GAMES = "SELECT GameID, Start FROM Games;";

        /// <summary>
        /// Statement to get statistics data from the database, for the 
        /// given outcome.
        /// </summary>
        private const string STMT_GET_STATS = "SELECT COUNT(*) FROM Statistics WHERE Outcome = @Outcome";
        #endregion

        #region Constants
        /// <summary>
        /// The default connection string, without the path to the database.
        /// </summary>
        private const string CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True";

        /// <summary>
        /// The location of the default database file, to be copied.
        /// TODO
        /// </summary>
        private const string DB_PATH = "./TarneebData.mdf";
        #endregion

        #region Variables
        /// <summary>
        /// The database connection.
        /// </summary>
        private static SqlConnection _connection = null;
        #endregion

        #region Database methods
        /// <summary>
        /// Create a connection to the database.
        /// </summary>
        public static void Connect()
        {
            // Check if the database does not exist, and copy if not in %LOCALAPPDATA%.
            /*if (!File.Exists(dbPath))
            {
                File.Copy(DEFAULT_DB_PATH, dbPath);
            }*/ //TODO

            // Connect to the database using the full path appended to the default connection string.
            _connection = new SqlConnection($"{CONN_STRING};AttachDbFilename={Path.GetFullPath(DB_PATH)}");
            _connection.Open();

            // Initialize the database with the tables, if they do not already exist
            Database.Initialize();
        }

        /// <summary>
        /// Attempt to initialize the database by creating the required tables.
        /// </summary>
        public static void Initialize()
        {
            // TODO: Explain how this is used to make sure there are two tables
            if (new SqlCommand(STMT_GET_TABLES, _connection).ExecuteNonQuery() != 3)
            {
                Database.Drop(); // TODO: Explain that this is done to clear the tables if there are issues
                // Create and execute the command to create the tables, and ignore the return value.
                new SqlCommand(STMT_CREATE_TABLES, _connection).ExecuteNonQuery();

                // Create and execute the command to create database indexes
                new SqlCommand(STMT_CREATE_INDEXES, _connection).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Drop the database tables.
        /// </summary>
        public static void Drop()
        {
            // Setup and execute the command to drop the tables
            new SqlCommand(STMT_DROP_TABLES, _connection).ExecuteNonQuery();
        }

        /// <summary>
        /// Drop and create the required database tables.
        /// </summary>
        public static void Reset()
        {
            Database.Drop();
            Database.Initialize();
        }
        #endregion

        #region Retrieve methods
        /// <summary>
        /// TODO
        /// </summary>
        public static List<DatabaseGameEntry> GetGames()
        {
            var games = new List<DatabaseGameEntry>();

            // Create the command for the query
            var cmdSelect = new SqlCommand(STMT_GET_GAMES, _connection);

            // Start reading rows
            using (var reader = cmdSelect.ExecuteReader())
            {
                // Read records until there are no more records
                while (reader.Read() != false)
                {
                    /* Read the latest row, and create an object to represent the row.
                     * The order of the columns is defined in the SELECT query at the top of the file.
                     */
                    games.Add(new DatabaseGameEntry(
                        reader.GetInt32(0),
                        reader.GetDateTime(1)
                    ));
                }
            }

            return games;
        }

        /// <summary>
        /// Get all of the logs in the database.
        /// TODO
        /// </summary>
        /// <returns></returns>
        public static List<Log> GetLogs()
        {
            // Same idea as GetGames, but for the Logs table.
            var logs = new List<Log>();

            var cmdSelect = new SqlCommand(STMT_GET_LOGS, _connection);

            using (var reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read() != false)
                {
                    logs.Add(new Log(
                        reader.GetDateTime(0),
                        reader.GetInt32(1),
                        reader.GetString(2)
                    ));
                }
            }

            return logs;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static SqlCommand GetLogsForDataTable(int gameId)
        {
            var cmdSelect = new SqlCommand(STMT_GET_LOGS_FOR_GAME, _connection);
            cmdSelect.Parameters.AddWithValue("@GameID", gameId); // TODO
            return cmdSelect;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="outcome"></param>
        /// <returns></returns>
        public static int GetOutcomeCount(Game.Outcome outcome)
        {
            // Set up the command and parameter
            var cmdSelect = new SqlCommand(STMT_GET_STATS, _connection);
            cmdSelect.Parameters.AddWithValue("@Outcome", (int)outcome);

            // Return the count.
            return (int)cmdSelect.ExecuteScalar();
        }
        #endregion

        #region Insert methods
        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>The game ID.</returns>
        public static int InsertGame(DateTime start)
        {
            // Set up the command and add the parameters
            var cmdInsert = new SqlCommand(STMT_INSERT_GAME, _connection);
            cmdInsert.Parameters.AddWithValue("@Start", DateTime.Now);

            /* Since the query returns the most recent identity value, and only that
             * value, execute the query as scalar and return whatever comes back.
             */
            return (int)cmdInsert.ExecuteScalar();
        }

        /// <summary>
        /// Insert a log into the Logs table.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void InsertLog(Logging.ILog log)
        {
            // Create the SQL command
            var cmdInsert = new SqlCommand(STMT_INSERT_LOG, _connection);

            // Set up data types for the query params
            cmdInsert.Parameters.Add("@DateTime", SqlDbType.DateTime);
            cmdInsert.Parameters.Add("@Action", SqlDbType.Text);
            cmdInsert.Parameters.AddWithValue("@GameID", 1); //TODO

            // Set the values; action is the string rep of the log.
            cmdInsert.Parameters["@DateTime"].Value = log.DateTime;
            cmdInsert.Parameters["@Action"].Value = log.ToString();

            // Execute the command; return value must be 1.
            // If it is not 1, there has been an error TODO.
            if (cmdInsert.ExecuteNonQuery() != 1)
            {

            }

            // TODO: Catch errors
        }

        /// <summary>
        /// Insert a game outcome into the stats table.
        /// </summary>
        /// <param name="dateTime">The date and time the outcome occurred.</param>
        /// <param name="outcome">The outcome (win/loss/tie)</param>
        public static void InsertOutcome(DateTime dateTime, int gameId, Game.Outcome outcome)
        {
            // Create the SQL command, set up the parameter data types
            var cmdInsert = new SqlCommand(STMT_INSERT_OUTCOME, _connection);
            cmdInsert.Parameters.Add("@DateTime", SqlDbType.DateTime);
            cmdInsert.Parameters.Add("@GameID", SqlDbType.Int);
            cmdInsert.Parameters.Add("@Outcome", SqlDbType.TinyInt);

            // Set the values for the parameters
            cmdInsert.Parameters["@DateTime"].Value = dateTime;
            cmdInsert.Parameters["@GameID"].Value = gameId;
            cmdInsert.Parameters["@Outcome"].Value = (int)outcome;

            // Execute the command; return value must be 1.
            // If it is not 1, there has been an error TODO.
            if (cmdInsert.ExecuteNonQuery() != 1)
            {

            }

            // TODO: Catch errors
        }
        #endregion
    }
}