using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    private static readonly object _lock = new object();
    private IDbConnection _dbConnection;

    public static DatabaseManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    var dbManager = new GameObject("DatabaseManager").AddComponent<DatabaseManager>();
                    DontDestroyOnLoad(dbManager.gameObject);
                }
                return _instance;
            }
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDatabase()
    {
        string connectionString = "URI=file:" + Application.persistentDataPath + "/Orion_DB_1.db";
        _dbConnection = new SqliteConnection(connectionString);
        _dbConnection.Open();

        // Set busy timeout
        IDbCommand command = _dbConnection.CreateCommand();
        command.CommandText = "PRAGMA busy_timeout = 3000"; // 3000 milliseconds
        command.ExecuteNonQuery();
    }

    public IDataReader ExecuteQuery(string query)
    {
        Log("Executing Query: " + query);
        IDbCommand dbCommand = _dbConnection.CreateCommand();
        dbCommand.CommandText = query;
        using IDataReader reader = dbCommand.ExecuteReader();

        // Ensure reader and command are disposed properly
        var finalizer = new Action(() =>
        {
            reader.Close();
            reader.Dispose();
            dbCommand.Dispose();
        });

        return new DisposableReader(reader, finalizer);
    }

    public void ExecuteNonQuery(string query)
    {
        Log("Executing NonQuery: " + query);
        using (IDbCommand dbCommand = _dbConnection.CreateCommand())
        {
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }
    }

    public void ExecuteTransaction(Action<IDbCommand> operations)
    {
        using (var transaction = _dbConnection.BeginTransaction())
        {
            try
            {
                using (IDbCommand dbCommand = _dbConnection.CreateCommand())
                {
                    dbCommand.Transaction = transaction;

                    operations(dbCommand);
                }

                transaction.Commit();
                Log("Transaction committed.");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log("Transaction rolled back due to an error: " + ex.Message);
            }
        }
    }

    void OnDestroy()
    {
        if (_dbConnection != null)
        {
            _dbConnection.Close();
            _dbConnection = null;
        }
    }

    private void Log(string message)
    {
        Debug.Log("[DatabaseManager] " + message);
    }

    private class DisposableReader : IDataReader
    {
        private readonly IDataReader _reader;
        private readonly Action _finalizer;

        public DisposableReader(IDataReader reader, Action finalizer)
        {
            _reader = reader;
            _finalizer = finalizer;
        }

        public void Dispose()
        {
            _finalizer?.Invoke();
        }

        // Implement all IDataReader methods and properties, delegating to _reader

        public object this[int i] => _reader[i];
        public object this[string name] => _reader[name];
        public int Depth => _reader.Depth;
        public int FieldCount => _reader.FieldCount;
        public bool IsClosed => _reader.IsClosed;
        public int RecordsAffected => _reader.RecordsAffected;

        public void Close() => _reader.Close();
        public bool GetBoolean(int i) => _reader.GetBoolean(i);
        public byte GetByte(int i) => _reader.GetByte(i);
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length) => _reader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
        public char GetChar(int i) => _reader.GetChar(i);
        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length) => _reader.GetChars(i, fieldOffset, buffer, bufferOffset, length);
        public IDataReader GetData(int i) => _reader.GetData(i);
        public string GetDataTypeName(int i) => _reader.GetDataTypeName(i);
        public DateTime GetDateTime(int i) => _reader.GetDateTime(i);
        public decimal GetDecimal(int i) => _reader.GetDecimal(i);
        public double GetDouble(int i) => _reader.GetDouble(i);
        public Type GetFieldType(int i) => _reader.GetFieldType(i);
        public float GetFloat(int i) => _reader.GetFloat(i);
        public Guid GetGuid(int i) => _reader.GetGuid(i);
        public short GetInt16(int i) => _reader.GetInt16(i);
        public int GetInt32(int i) => _reader.GetInt32(i);
        public long GetInt64(int i) => _reader.GetInt64(i);
        public string GetName(int i) => _reader.GetName(i);
        public int GetOrdinal(string name) => _reader.GetOrdinal(name);
        public DataTable GetSchemaTable() => _reader.GetSchemaTable();
        public string GetString(int i) => _reader.GetString(i);
        public object GetValue(int i) => _reader.GetValue(i);
        public int GetValues(object[] values) => _reader.GetValues(values);
        public bool IsDBNull(int i) => _reader.IsDBNull(i);
        public bool NextResult() => _reader.NextResult();
        public bool Read() => _reader.Read();
    }
}
