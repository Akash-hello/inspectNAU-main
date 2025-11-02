using System;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Data;
using System.Threading;

namespace DataBank
{
    public class SqliteHelper : IDisposable
    {
        private const string Tag = "Orion: SqliteHelper:\t";

        private const string database_name = "Orion_DB_1";

        private static readonly object _lock = new object();
        private bool _disposed = false;


        public string db_connection_string;
        public IDbConnection db_connection;
        public int RowCount = 0;

        // ThreadLocal to ensure each thread has its own connection
        private readonly ThreadLocal<IDbConnection> _threadConnection;

        //public SqliteHelper()
        //{
        //    db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + database_name;
        //    //Debug.Log("db_connection_string" + db_connection_string);
        //    db_connection = new SqliteConnection(db_connection_string);
        //    db_connection.Open();
        //}

        public SqliteHelper()
        {
            // Construct the connection string
            db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + database_name;

            // Initialize ThreadLocal connection

            _threadConnection = new ThreadLocal<IDbConnection>(() =>
            {
                var connection = new SqliteConnection(db_connection_string);
                connection.Open();
                return connection;
            }, trackAllValues: true);
        }


        private IDbConnection Connection
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(SqliteHelper));

                return _threadConnection.Value;
            }
        }

        ~SqliteHelper()
        {
            
            Dispose(false);
        }



        // virtual functions
        public virtual IDataReader getDataById(int id)
        {
            Debug.Log(Tag + "This function is not implemnted");
            throw null;
        }

        public virtual IDataReader getDataByString(string str)
        {
            Debug.Log(Tag + "This function is not implemnted");
            throw null;
        }

        public virtual void deleteDataById(int id)
        {
            Debug.Log(Tag + "This function is not implemented");
            throw null;
        }

        public virtual void deleteDataByString(string id)
        {
            Debug.Log(Tag + "This function is not implemented");
            throw null;
        }

        public virtual IDataReader getDataByString()
        {
            Debug.Log(Tag + "This function is not implemented");
            throw null;
        }

        public virtual IDataReader UpdateData()
        {
            Debug.Log(Tag + "This function is not implemented");
            throw null;
        }

        public virtual void deleteAllData()
        {
            Debug.Log(Tag + "This function is not implemnted");
            throw null;
        }
        public virtual void DropTable()
        {
            Debug.Log(Tag + "This function is not implemnted");
            throw null;
        }
        public virtual void vacuumAllData()
        {
            Debug.Log(Tag + "This function is not implemnted");
            throw null;
        }

        public virtual IDataReader getNumOfRows()
        {
            Debug.Log(Tag + "This function is not implemnted");
            throw null;
        }

        //helper functions
        public IDbCommand getDbCommand()
        {
           
            //if (db_connection.State == ConnectionState.Closed)
            //{
            //    db_connection.Open();
            //}
            //return db_connection.CreateCommand();
            return Connection.CreateCommand();
               // return dbCmd;
        }

        public SqliteConnection getConnection()
        {
            var connection = new SqliteConnection(db_connection_string); // replace with actual connection string
            connection.Open();

            return connection;
           
        }


        public IDataReader getAllData(string table_name)
        {
            //var connection = new SqliteConnection(db_connection_string);
            //connection.Open();

            using (IDbCommand dbCmd = Connection.CreateCommand())
                {
                try
                {
                    dbCmd.CommandText = $"SELECT * FROM `{table_name}`"; // Use backticks to handle special characters
                    return dbCmd.ExecuteReader();
                }
            catch (Exception ex)
            {
                Debug.LogError($"{Tag} GetAllData Error: {ex.Message}");
                throw;
            }

                //finally
                //{
                //    dbCmd.Dispose();
                //    //connection.Close();
                //}
            }
            

        }
        public IDataReader getAllData1(string table_name, int size, int page)
        {
            //int skips = size * page;
            //if (skips < 0)
            //{
            //    skips = 0;
            //}

            using IDbCommand dbcmd = Connection.CreateCommand();
            if (table_name == "Synch")
            {
                dbcmd.CommandText =
               "SELECT * FROM " + table_name + " order by id desc";// where id >" + skips + " order by id LIMIT " + size + "
            }
            else
            {
                dbcmd.CommandText =
               "SELECT * FROM " + table_name + " order by id";// where id >" + skips + " order by id LIMIT " + size + "
            }

             IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public IDataReader searchbystring(string table_name, string SearchQuery, int size, int page)
        {
            //Debug.Log("calling page " + page+" with searc text"+ SearchQuery);
            //int skips = size * page;
            //if (skips < 0)
            //{
            //    skips = 0;
            //}

            using IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "SELECT * FROM " + table_name + "  WHERE ItemName LIKE '%" + SearchQuery.Replace("'", "''") + "%' or ItemCode LIKE '%" + SearchQuery.Replace("'", "''") + "%' order by id";
            // "select * from (SELECT distinct *,(select count(DISTINCT id) from " + table_name + " b  where a.id >= b.id) as cnt FROM " + table_name + " a WHERE ItemName LIKE '%" + SearchQuery.Replace("'", "''") + "%' or ItemCode LIKE '%" + SearchQuery.Replace("'", "''") + "%' order by a.id) c where c.cnt >" + skips + " order by id LIMIT " + size + "";

            // RowCount = Convert.ToInt32(dbcmd.ExecuteScalar());
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }


        public IDataReader InspectionCreatedDate(string Query)
        {

            using IDbCommand dbcmd = Connection.CreateCommand();

            dbcmd.CommandText = Query;

            IDataReader reader = dbcmd.ExecuteReader();
            return reader;

        }

        public IDataReader countbyanswer(string query)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = query;
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public int totalRecords(string table_name)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText = "select count(*) FROM " + table_name;
            return Convert.ToInt32(dbcmd.ExecuteScalar());
        }

        public int CountbasisQuery(string Query)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText = "select count(*) FROM " + Query;
            return Convert.ToInt32(dbcmd.ExecuteScalar());
        }


        public void deleteAllData(string table_name)
        {
            using var connection = new SqliteConnection(db_connection_string); // replace with actual connection string
            connection.Open();

            using IDbCommand dbcmd = Connection.CreateCommand();
            try
            {
                dbcmd.CommandText = "DELETE FROM " + table_name;
                dbcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error retrieving -->>:  {ex.Message} & {ex.StackTrace}");
               
            }

            //finally
            //{
            //    if (dbcmd != null)
            //    {
            //        dbcmd.Dispose();
            //    }
            //    connection.Close();   // Explicitly close connection after operations
            //    Debug.Log($"[Connection State] Closed: {connection.State}");
            //    Debug.Log("Database resources disposed.");
            //}

        }

        public void DropTable(string table_name)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText = "DROP TABLE IF EXISTS " + table_name;
            dbcmd.ExecuteNonQuery();
        }

        public void vacuumAllData(string table_name)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText = "vacuum " + table_name;
            dbcmd.ExecuteNonQuery();
        }
        public IDataReader UpdateData(string table_name, string columndataquery)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText = "UPDATE " + table_name + " SET " + columndataquery;
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }
        public IDataReader getNumOfRows(string table_name)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText =
                "SELECT COALESCE(MAX(id)+1, 0) FROM " + table_name;
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public IDataReader getColumnheaders(string table_name)
        {
            using IDbCommand dbcmd = Connection.CreateCommand();
            dbcmd.CommandText =
                "SELECT sql FROM sqlite_master WHERE tbl_name =" + table_name;
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public void close()
        {
            if (db_connection != null && db_connection.State != ConnectionState.Closed)
            {
                db_connection.Close();
                db_connection.Dispose();
                db_connection = null;
            }
        }


        public void close1()
        {
            if (_threadConnection.IsValueCreated)
            {
                try
                {
                    Connection.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{Tag} CloseConnection Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Implements the Dispose pattern to clean up resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose method.
        /// </summary>
        /// <param name="disposing">Indicates if called from Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_threadConnection != null)
                    {
                        foreach (var connection in _threadConnection.Values)
                        {
                            if (connection != null)
                            {
                                try
                                {
                                    connection.Close();
                                    connection.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError($"{Tag} Dispose Connection Error: {ex.Message}");
                                }
                            }
                        }
                        _threadConnection.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}

//public class SqliteHelper : IDisposable
//{
//    private const string Tag = "Orion: SqliteHelper:\t";

//    private const string database_name = "Orion_DB_1";

//    public string db_connection_string;
//    public IDbConnection db_connection;
//    public int RowCount = 0;

//    private bool _disposed = false; // Track whether Dispose has been called

//    public SqliteHelper()
//    {
//        db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + database_name;
//        //Debug.Log("db_connection_string" + db_connection_string);
//        db_connection = new SqliteConnection(db_connection_string);
//        db_connection.Open();
//    }

//    ~SqliteHelper()
//    {
//        db_connection.Close();
//        Dispose(false);
//    }

//    // virtual functions
//    public virtual IDataReader getDataById(int id)
//    {
//        Debug.Log(Tag + "This function is not implemnted");
//        throw null;
//    }

//    public virtual IDataReader getDataByString(string str)
//    {
//        Debug.Log(Tag + "This function is not implemnted");
//        throw null;
//    }

//    public virtual void deleteDataById(int id)
//    {
//        Debug.Log(Tag + "This function is not implemented");
//        throw null;
//    }

//    public virtual void deleteDataByString(string id)
//    {
//        Debug.Log(Tag + "This function is not implemented");
//        throw null;
//    }

//    public virtual IDataReader getDataByString()
//    {
//        Debug.Log(Tag + "This function is not implemented");
//        throw null;
//    }

//    public virtual IDataReader UpdateData()
//    {
//        Debug.Log(Tag + "This function is not implemented");
//        throw null;
//    }

//    public virtual void deleteAllData()
//    {
//        Debug.Log(Tag + "This function is not implemnted");
//        throw null;
//    }
//    public virtual void DropTable()
//    {
//        Debug.Log(Tag + "This function is not implemnted");
//        throw null;
//    }
//    public virtual void vacuumAllData()
//    {
//        Debug.Log(Tag + "This function is not implemnted");
//        throw null;
//    }

//    public virtual IDataReader getNumOfRows()
//    {
//        Debug.Log(Tag + "This function is not implemnted");
//        throw null;
//    }

//    //helper functions
//    public IDbCommand getDbCommand()
//    {
//        if (db_connection.State == ConnectionState.Closed)
//        {
//            db_connection.Open();
//        }
//        return db_connection.CreateCommand();
//    }

//    public IDataReader getAllData(string table_name)
//    {

//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText =
//            "SELECT * FROM " + table_name;
//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }
//    public IDataReader getAllData1(string table_name, int size, int page)
//    {
//        //int skips = size * page;
//        //if (skips < 0)
//        //{
//        //    skips = 0;
//        //}

//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        if (table_name == "Synch")
//        {
//            dbcmd.CommandText =
//           "SELECT * FROM " + table_name + " order by id desc";// where id >" + skips + " order by id LIMIT " + size + "
//        }
//        else
//        {
//            dbcmd.CommandText =
//           "SELECT * FROM " + table_name + " order by id";// where id >" + skips + " order by id LIMIT " + size + "
//        }

//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }

//    public IDataReader searchbystring(string table_name, string SearchQuery, int size, int page)
//    {
//        //Debug.Log("calling page " + page+" with searc text"+ SearchQuery);
//        //int skips = size * page;
//        //if (skips < 0)
//        //{
//        //    skips = 0;
//        //}

//        using IDbCommand dbcmd = getDbCommand();

//        dbcmd.CommandText = "SELECT * FROM " + table_name + "  WHERE ItemName LIKE '%" + SearchQuery.Replace("'", "''") + "%' or ItemCode LIKE '%" + SearchQuery.Replace("'", "''") + "%' order by id";
//        // "select * from (SELECT distinct *,(select count(DISTINCT id) from " + table_name + " b  where a.id >= b.id) as cnt FROM " + table_name + " a WHERE ItemName LIKE '%" + SearchQuery.Replace("'", "''") + "%' or ItemCode LIKE '%" + SearchQuery.Replace("'", "''") + "%' order by a.id) c where c.cnt >" + skips + " order by id LIMIT " + size + "";

//        // RowCount = Convert.ToInt32(dbcmd.ExecuteScalar());
//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }


//    public IDataReader InspectionCreatedDate(string Query)
//    {

//        using IDbCommand dbcmd = db_connection.CreateCommand();

//        dbcmd.CommandText = Query;

//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;

//    }

//    public IDataReader countbyanswer(string query)
//    {
//        IDbCommand dbcmd = getDbCommand();
//        dbcmd.CommandText = query;
//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }

//    public int totalRecords(string table_name)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText = "select count(*) FROM " + table_name;
//        return Convert.ToInt32(dbcmd.ExecuteScalar());
//    }

//    public int CountbasisQuery(string Query)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText = "select count(*) FROM " + Query;
//        return Convert.ToInt32(dbcmd.ExecuteScalar());
//    }


//    public void deleteAllData(string table_name)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText = "DELETE FROM " + table_name;
//        dbcmd.ExecuteNonQuery();
//    }

//    public void DropTable(string table_name)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText = "DROP TABLE IF EXISTS " + table_name;
//        dbcmd.ExecuteNonQuery();
//    }

//    public void vacuumAllData(string table_name)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText = "vacuum " + table_name;
//        dbcmd.ExecuteNonQuery();
//    }
//    public IDataReader UpdateData(string table_name, string columndataquery)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText = "UPDATE " + table_name + " SET " + columndataquery;
//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }
//    public IDataReader getNumOfRows(string table_name)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText =
//            "SELECT COALESCE(MAX(id)+1, 0) FROM " + table_name;
//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }

//    public IDataReader getColumnheaders(string table_name)
//    {
//        using IDbCommand dbcmd = db_connection.CreateCommand();
//        dbcmd.CommandText =
//            "SELECT sql FROM sqlite_master WHERE tbl_name =" + table_name;
//        IDataReader reader = dbcmd.ExecuteReader();
//        return reader;
//    }


//    public virtual void Dispose()
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }

//    protected virtual void Dispose(bool disposing)
//    {
//        if (!_disposed)
//        {
//            if (disposing)
//            {
//                // Dispose managed resources
//                if (db_connection != null)
//                {
//                    db_connection.Close();
//                    db_connection.Dispose();
//                    db_connection = null;
//                }
//            }
//            _disposed = true;
//        }
//    }



//    public void close()
//    {
//        db_connection.Close();
//    }
//}