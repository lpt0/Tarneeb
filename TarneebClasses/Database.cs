using System;
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

        /// <summary>
        /// Get this game entry as a string.
        /// </summary>
        /// <returns>The game entry in the format of "Game $GameID at $Start"</returns>
        public override string ToString()
        {
            return $"Game {GameID} at {Start}";
        }
    }

    /// <summary>
    /// Provides database functionality, such as storing logs and statistics.
    /// </summary>
    public static class Database
    {
        #region Queries
        /// <summary>
        /// Used to check if tables exist.
        /// </summary>
        private const string STMT_GET_TABLES = (
            "SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Logs' OR TABLE_NAME = 'Stats' OR TABLE_NAME = 'Games';"
        );

        /// <summary>
        /// Statement to create all required tables.
        /// </summary>
        private const string STMT_CREATE_TABLES = (@"CREATE TABLE Logs (DateTime DATETIME, GameID INT, Action TEXT); 
CREATE TABLE Stats (GameID INT PRIMARY KEY, DateTime DATETIME, Outcome TINYINT);
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
        /// Statement to delete all rows in the logs table.
        /// </summary>
        private const string STMT_DELETE_LOGS = "DELETE FROM Logs;";

        /// <summary>
        /// Statement to delete old game rows from the games table.
        /// </summary>
        private const string STMT_DELETE_GAMES = "DELETE FROM Games WHERE GameID < @GameID;";

        /// <summary>
        /// Statement to add a log to the logs table.
        /// </summary>
        private const string STMT_INSERT_LOG = "INSERT INTO Logs VALUES (@DateTime, @GameID, @Action);";

        /// <summary>
        /// Statement to add a game outcome into the statistics table.
        /// </summary>
        private const string STMT_INSERT_OUTCOME = "INSERT INTO Stats (DateTime, GameID, Outcome) VALUES (@DateTime, @GameID, @Outcome);";

        /// <summary>
        /// Inserts a game into the list of games played.
        /// </summary>
        private const string STMT_INSERT_GAME = "INSERT INTO Games (Start) VALUES (@Start); SELECT @@IDENTITY;";

        /// <summary>
        /// Statement to get logs from the database.
        /// </summary>
        private const string STMT_GET_LOGS = "SELECT DateTime, GameID, Action FROM Logs;";

        /// <summary>
        /// Statement to gets logs for a specified game.
        /// </summary>
        private const string STMT_GET_LOGS_FOR_GAME = "SELECT DateTime, Action FROM Logs WHERE GameID = @GameID;";

        /// <summary>
        /// Statement to get all games played.
        /// </summary>
        private const string STMT_GET_GAMES = "SELECT GameID, Start FROM Games;";

        /// <summary>
        /// Statement to get statistics data from the database, for the 
        /// given outcome.
        /// </summary>
        private const string STMT_GET_STATS = "SELECT COUNT(*) FROM Stats WHERE Outcome = @Outcome";

        /// <summary>
        /// Statement to get the ID for the most recent game, from the logs table
        /// This is ALL games played, not just completed games.
        /// </summary>
        private const string STMT_GET_LATEST_GAME = "SELECT TOP(1) GameID FROM Logs ORDER BY GameID DESC;";
        #endregion

        #region Constants
        /// <summary>
        /// The default connection string, without the path to the database.
        /// </summary>
        private const string CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True";

        /// <summary>
        /// The location of the default database file.
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
            int numberOfTables = (int)new SqlCommand(STMT_GET_TABLES, _connection).ExecuteScalar();
            /* The above query returns the number of tables present, 
             * matching the specified table names; it needs to be 3.
             */
            if (numberOfTables != 3)
            {
                // Issues with the database; drop all tables and try again.
                Database.Drop();
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
            Database.Connect();
            Database.Drop();
            Database.Initialize();
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public static void Close()
        {
            // Only close the connection if it is set, and it is open
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        #endregion

        #region Retrieve methods
        /// <summary>
        /// Get games played from the database.
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
                while (reader.Read() == true)
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
        /// </summary>
        /// <returns>Logs for every game played stored in the database.</returns>
        public static List<Log> GetLogs()
        {
            // Same idea as GetGames, but for the Logs table.
            var logs = new List<Log>();

            var cmdSelect = new SqlCommand(STMT_GET_LOGS, _connection);

            using (var reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read() == true)
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
        /// Get logs for the specified game.
        /// </summary>
        /// <param name="gameId">The game to retrieve logs for.</param>
        /// <returns>The list of game logs.</returns>
        public static List<Log> GetLogs(int gameId)
        {
            // Same idea as GetLogs without gameId
            var logs = new List<Log>();
            var cmdSelect = new SqlCommand(STMT_GET_LOGS_FOR_GAME, _connection);
            cmdSelect.Parameters.AddWithValue("@GameID", gameId);

            using (var reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read() == true)
                {
                    // Creatr the log object and add it to the list of logs
                    logs.Add(new Log(
                        reader.GetDateTime(0),
                        gameId,
                        reader.GetString(1)
                    ));
                }
            }

            return logs;
        }

        /// <summary>
        /// Get the number of outcomes for the specified outcome.
        /// This is used to find the number of wins or losses.
        /// </summary>
        /// <param name="outcome">The outcome type.</param>
        /// <returns>The number of times this outcome happened.</returns>
        public static int GetOutcomeCount(Game.Outcome outcome)
        {
            // Set up the command and parameter
            var cmdSelect = new SqlCommand(STMT_GET_STATS, _connection);
            cmdSelect.Parameters.AddWithValue("@Outcome", (int)outcome);

            // Return the count.
            return (int)cmdSelect.ExecuteScalar();
        }

        /// <summary>
        /// Get the identifier for the most recent game played.
        /// Useful for finding the number of games played.
        /// </summary>
        /// <returns>The most recent game identifier.</returns>
        public static int GetLatestGameID()
        {
            var cmdSelect = new SqlCommand(STMT_GET_LATEST_GAME, _connection);
            var result = cmdSelect.ExecuteScalar();
            return (result == null ? 0 : (int)result);
        }
        #endregion

        #region Insert methods
        /// <summary>
        /// Add a new game to the database, and get it's identifier.
        /// </summary>
        /// <returns>The game ID.</returns>
        public static int InsertGame(DateTime start)
        {
            // Set up the command and add the parameters
            var cmdInsert = new SqlCommand(STMT_INSERT_GAME, _connection);
            cmdInsert.Parameters.AddWithValue("@Start", DateTime.Now);

            /* If there are 3 (or a multiple of 3) games in the database, clear logs
             * to ease ballooning of the DB size. Also, remove previous games.
             * Only one value is returned (one row, one column), so the return value
             * of the query is scalar.
             */
            var gameId = (int)((Decimal)cmdInsert.ExecuteScalar());
            if (gameId % 3 == 0)
            {
                var cmdDelete = new SqlCommand(STMT_DELETE_LOGS, _connection);
                cmdDelete.ExecuteNonQuery();

                var cmdDeleteGames = new SqlCommand(STMT_DELETE_GAMES, _connection);
                cmdDeleteGames.Parameters.AddWithValue("@GameID", gameId);
                cmdDeleteGames.ExecuteNonQuery();
            }

            return gameId;
        }

        /// <summary>
        /// Insert a log into the Logs table.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void InsertLog(Log log)
        {
            // Create the SQL command
            var cmdInsert = new SqlCommand(STMT_INSERT_LOG, _connection);

            // Set up data types for the query params
            cmdInsert.Parameters.Add("@DateTime", SqlDbType.DateTime);
            cmdInsert.Parameters.Add("@Action", SqlDbType.Text);
            cmdInsert.Parameters.Add("@GameID", SqlDbType.Int);

            // Set the values; action is the string rep of the log.
            cmdInsert.Parameters["@DateTime"].Value = log.DateTime;
            cmdInsert.Parameters["@Action"].Value = log.Action;
            cmdInsert.Parameters["@GameID"].Value = log.GameID;

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
