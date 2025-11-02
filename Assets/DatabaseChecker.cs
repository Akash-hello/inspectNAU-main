using System;
using UnityEngine;
using Mono.Data.Sqlite; // Assuming you're using Mono.Data.Sqlite for SQLite in Unity

public class DatabaseChecker : MonoBehaviour
{
    private SqliteConnection dbConnection;
    private const string database_name = "Orion_DB_1";
    void Awake()
    {
        Debug.Log("Checking for unfinalized statements or unfinished backups...");

        CheckDatabaseStatus();
    }

    void Start()
    {
        // Optionally, run additional checks if needed
        CheckDatabaseStatus();
    }

    private void CheckDatabaseStatus()
    {
        // Open a test connection to verify the database state
        try
        {
            // Example connection string - modify this to match your actual database path
            string connectionString = "URI=file:" + Application.persistentDataPath + "/" + database_name;
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();

            Debug.Log($"[Database State] Connection state on startup: {dbConnection.State}");

            // Example query to check for open transactions or unfinalized statements
            using (var cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA integrity_check";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string integrityResult = reader.GetString(0);
                        Debug.Log($"[Database Integrity Check] Result: {integrityResult}");
                        if (integrityResult != "ok")
                        {
                            Debug.LogWarning("Potential unfinalized statements or database issues detected.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error checking database state: {ex.Message}");
        }
        finally
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
                Debug.Log("[Database State] Connection closed after startup check.");
                Debug.Log($"[Connection State] After dbcmd disposed: {(dbConnection != null ? dbConnection.State.ToString() : "Connection is null")}");
            }
        }
    }
}
