using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class SQLiteConnectionManager
{
    // List to keep track of open SQLite connections
    private List<SqliteConnection> openConnections = new List<SqliteConnection>();

    //SQLiteConnectionManager closeallconns = new SQLiteConnectionManager();
    //closeallconns.CloseAndDisposeAllConnections();

    // Method to open a new SQLite connection and add it to the list
    public SqliteConnection OpenConnection(string connectionString)
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        openConnections.Add(connection); // Track the connection
        return connection;
    }

    // Method to close and dispose all open connections
    public void CloseAndDisposeAllConnections()
    {
        foreach (var connection in openConnections)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    Debug.Log("This connection was found open; " + connection.ToString());
                    connection.Close();
                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error closing connection: " + ex.Message);
                }
            }
        }

        // Clear the list after disposing all connections
        openConnections.Clear();
    }

    // Optionally, close a specific connection
    public void CloseConnection(SqliteConnection connection)
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            try
            {
                connection.Close();
                connection.Dispose();
                openConnections.Remove(connection); // Remove from the list
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing connection: " + ex.Message);
            }
        }
    }
}
