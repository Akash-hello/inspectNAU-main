using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;

namespace DataBank
{
    public class table_LoginConfig : SqliteHelper
    {
        //Now, lets create our Inventory table. I will call this class table_Inventory, however, it is just a table in the OrionDB, 
        //this tables communicates with the SqliteHelper. It seems to act like a database and we shall treat it that way too, however its just a table.

        private const String Tag = "Orion: table_LoginConfig:\t";
        private const String TABLE_NAME = "LoginConfig";
        private const String KEY_ID = "Id";
        private const String KEY_NAME = "Name";
        private const String KEY_COMPANY = "Company";
        private const String KEY_DESIGNATION = "Designation";
        private const String KEY_EMAIL = "Email";
        private const String KEY_PHONE = "Phone";

        private const String KEY_COMPANYCODE = "CompanyCode";
        private const String KEY_COMPANYGUID = "CompanyGuid";
        private const String KEY_TOKENBALANCE = "TokenBalance";
        private const String KEY_USERTYPE = "UserType"; //Ship or Individual
        private const String KEY_VESSELIMO = "VesselIMO"; //Ship or Individual
        private const String KEY_VESSELTYPE = "VesselType"; //Ship or Individual

        private const String KEY_PASSWORD = "Password";
        private const String KEY_APITOKEN = "Apitoken"; //USED FOR Our incoming API to be triggered and kept as hardcoded, earlier user had to input this value. 
        private const String KEY_TOKENVALIDDATE = "Tokendate";//USED FOR Our Tokens requested to the server, if value = 1, means there is a live request, if 0, then no request or tokens already granted.
        private const String KEY_MARKETSET = "Marketsetting";
        private const String KEY_MARKETS = "UserSubscribedmarkets";
        private const String KEY_SESSIONSTATE = "Sessionstate";
        private const String KEY_PHOTOFILENAME = "photofilename";
        private const String KEY_TIMESTAMP = "TimeStamp";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_NAME, KEY_COMPANY, KEY_DESIGNATION, KEY_EMAIL, KEY_PHONE, KEY_COMPANYCODE, KEY_COMPANYGUID, KEY_TOKENBALANCE, KEY_USERTYPE, KEY_VESSELIMO, KEY_VESSELTYPE, KEY_PASSWORD, KEY_APITOKEN, KEY_TOKENVALIDDATE, KEY_MARKETSET, KEY_MARKETS, KEY_SESSIONSTATE, KEY_PHOTOFILENAME, KEY_TIMESTAMP };
                                                                                  
        //public MyWatchlistRead watchlistread;                                  
        // public AddItem additem;                                                   
        //public string searchresults;
        //public GameObject InventoryListItems;
        //public GameObject InventoryItems;
        public int LatestId;
        public string MarketSet;
        public string Session;
        public long subscriptionvalid;
        public string useremail;
        public string name;
        public string designation;
        public string companyname;

        public string usersubsmarkets;
        public long whatsappnumber;
        public string profilephotoname;
        public int ThisitemId;
        public String outputofsearchresult;
        public string password;
        public string ApiAuthKey;
        public int tokenbalance;
        public long tokenrequestedflag;
        public string companyAuthcode;
        public string companyguid;
        public string IncomingAPIdetail;
        

        public Stocks_Accordion_Call_API_For_DB_READ1 stockscallapi;
        public deletethisitem deleteItemCall;
        private const string database_name = "Orion_DB_1";

        public table_LoginConfig() : base()
        {
            
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                //KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                KEY_NAME + " TEXT, " +
                KEY_COMPANY + " TEXT, " +
                KEY_DESIGNATION + " TEXT, " +
                KEY_EMAIL + " TEXT, " +
                KEY_PASSWORD + " TEXT, " +
                KEY_PHONE + " NUMERIC, " +

                KEY_COMPANYCODE + " TEXT, " +
                KEY_COMPANYGUID + " TEXT, " +
                KEY_TOKENBALANCE + " INTEGER, " +
                KEY_USERTYPE + " TEXT, " +
                KEY_VESSELIMO + " INTEGER, " +
                KEY_VESSELTYPE + " TEXT, " +

                KEY_APITOKEN + " TEXT, " +
                KEY_TOKENVALIDDATE + " TEXT, " +
                KEY_MARKETSET + " TEXT, " +
                KEY_MARKETS + " TEXT, " +
                KEY_SESSIONSTATE + " TEXT, " +
                KEY_PHOTOFILENAME + " TEXT, " +
                KEY_TIMESTAMP + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();
        }

        public void addData(LoginEntity logincreds)
        {
            using var connection = new SqliteConnection(db_connection_string); // replace with actual connection string
            connection.Open();

            using IDbCommand dbcmd = getDbCommand();

            try
            {

                dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ("
                + KEY_NAME + ", "
                + KEY_COMPANY + ", "
                + KEY_DESIGNATION + ", "
                + KEY_EMAIL + ", "
                + KEY_PASSWORD + ", "
                + KEY_PHONE + ", "

                + KEY_COMPANYCODE + ", "
                + KEY_COMPANYGUID + ", "
                + KEY_TOKENBALANCE + ", "
                + KEY_USERTYPE + ", "
                + KEY_VESSELIMO + ", "
                + KEY_VESSELTYPE + ", "

                + KEY_APITOKEN + ", "
                + KEY_TOKENVALIDDATE + ", "
                + KEY_MARKETSET + ", "
                + KEY_MARKETS + ", "
                + KEY_SESSIONSTATE + ", "
                + KEY_PHOTOFILENAME + ")"

                + "VALUES (' "
                + logincreds._name + " ' , ' "
                + logincreds._company + " ' , ' "
                + logincreds._designation + " ' , ' "
                + logincreds._email + " ' , ' "
                + logincreds._password + " ' , ' "
                + logincreds._phone + " ' , ' "

                + logincreds._companycode + " ' , ' "
                + logincreds._companyguid + " ' , ' "
                + logincreds._tokenbalance + " ' , ' "
                + logincreds._usertype + " ' , ' "
                + logincreds._vesselimo + " ' , ' "
                + logincreds._vesseltype + " ' , ' "

                + logincreds._apitoken + " ' , ' "
                + logincreds._tokenvalidity + " ' , ' "
                + logincreds._marketset + " ' , ' "
                + logincreds._markets + " ' , ' "
                + logincreds._sessionstate + " ' , ' "
                + logincreds._photofilename + " ')";

                dbcmd.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                Debug.LogError($"Error retrieving -->>:  {ex.Message} & {ex.StackTrace}");

            }

            finally
            {
                if (dbcmd != null)
                {
                    dbcmd.Dispose();
                }
                connection.Close();   // Explicitly close connection after operations
                Debug.Log($"[Connection State] Closed: {connection.State}");
                Debug.Log("Database resources disposed.");
            }
        }

        public override IDataReader getDataById(int id)
        {
            return base.getDataById(id);
        }

        public void getDataByString1(string str)

        {
            Debug.Log(Tag + "Getting Search string: " + str);
            //str = Inputs.searchinventory.text.ToString();
            IDbCommand dbcmd = getDbCommand();

            if (str == "fetchall")
            {
                dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID;
            }
            else
            {
                dbcmd.CommandText =
           "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_EMAIL + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_PHONE + " LIKE '%" + str.Replace("'", "''") + "%'";
            }
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<LoginEntity> myList = new List<LoginEntity>();
           using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()), 
                reader[1].ToString().Trim(), 
                reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                int.Parse(reader[9].ToString().Trim()),
                reader[10].ToString().Trim(),
                int.Parse(reader[11].ToString().Trim()),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._id;
            }
            reader.Dispose();
        }

        public void updateConsbyID(int ID)
        {
            Debug.Log(Tag + "Getting Search ID: " + ID);
            //str = Inputs.searchinventory.text.ToString();
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE  " + TABLE_NAME + " trim('   SQLite trim   ')";
            dbcmd.CommandText =
           "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + ID + "'";
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<LoginEntity> myList = new List<LoginEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()),
                reader[1].ToString().Trim(),
                reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                int.Parse(reader[9].ToString().Trim()),
                reader[10].ToString().Trim(),
                int.Parse(reader[11].ToString().Trim()),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
            }
            reader.Dispose();
            //searchresults = JsonUtility.ToJson(myList, true);
            //inventorymanager.searchresultsforhere = myList;
            //inventorymanager.queryresults();
        }
        public void CheckifItemExists(string str, string methodname)
        {
            Debug.Log(Tag + "Getting Search string: " + str);
            //str = Inputs.searchinventory.text.ToString();
            IDbCommand dbcmd = getDbCommand();
            //dbcmd.CommandText = "select MAX(id) from " + TABLE_NAME;
            dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE upper(trim(" + KEY_EMAIL + ")) =upper('" + str + "')"; //-->>> does not work, also match upper or lower cases FOR cHECKING..
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMCODE + " = ' " + str + " '" + " or " + KEY_ITEMCODE + " ='" + str + "'";
            List<LoginEntity> myList = new List<LoginEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()),
                reader[1].ToString().Trim(),
                reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                int.Parse(reader[9].ToString().Trim()),
                reader[10].ToString().Trim(),
                int.Parse(reader[11].ToString().Trim()),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim());

                //Debug.Log("item code: " + entity._itemcode);
                myList.Add(entity);

                ThisitemId = entity._id;
                var output1 = JsonUtility.ToJson(entity, true);
                // Debug.Log(output1);
                outputofsearchresult = output1.ToString();
            }
            reader.Dispose();
            //searchresults = JsonUtility.ToJson(myList, true);


            if (methodname == "AddingNew")
            {
                getLatestID();
            }

           
        }
        public void getNewOrUpdatedItemId(int ID)
        {
            Debug.Log(Tag + "Getting Search ID: " + ID);
            //str = Inputs.searchinventory.text.ToString();
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE  " + TABLE_NAME + " trim('   SQLite trim   ')";
            dbcmd.CommandText =
           "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + ID + "'";
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<LoginEntity> myList = new List<LoginEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()),
                reader[1].ToString().Trim(),
                reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                int.Parse(reader[9].ToString().Trim()),
                reader[10].ToString().Trim(),
                int.Parse(reader[11].ToString().Trim()),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
            }
            reader.Dispose();
            //additem.outputofsearchresult = outputofsearchresult;
            //additem.Synchronise
            //  inventorymanager.SynchEditedData();

        }

        private void StartCoroutine(IEnumerator enumerator)
        {
            throw new NotImplementedException();
        }

        public int getLatestID ()
        {
            //db_connection_string = "URI=file:" + Application.persistentDataPath + "/" + database_name;
            //using var connection = new SqliteConnection(db_connection_string); // replace with actual connection string

            //using var connection = getConnection();
            //connection.Open();

            //var connection = getConnection();

            //int llatestId = 0;
            Session = "";
            LatestId = 0;
            ApiAuthKey = "";
            companyAuthcode = "";
            IncomingAPIdetail = "";
            tokenbalance = 0;
            companyguid = "";
            tokenrequestedflag = 0;
            MarketSet = "";
            //using IDbCommand dbcmd = connection.CreateCommand();
            IDbCommand dbcmd = getDbCommand();
            IDataReader reader = null;

            // Explicitly opening and closing the connection here
            
           // Debug.Log($"[Connection State] Opened: {connection.State}");

            try {

                dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID + " DESC LIMIT 1";

            List<LoginEntity> myList = new List<LoginEntity>();
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()),
                reader[1].ToString().Trim(),
                reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                int.Parse(reader[9].ToString().Trim()),
                reader[10].ToString().Trim(),
                int.Parse(reader[11].ToString().Trim()),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim());

                    //llatestId = entity._id;
                    LatestId = entity._id;
                MarketSet = entity._marketset.ToString();
                    Session = entity._sessionstate.ToString();
                name = entity._name.ToString();
                designation = entity._designation.ToString();
                companyname = entity._company.ToString();

                useremail = entity._email.ToString();
                usersubsmarkets = entity._markets.ToString();
                whatsappnumber = Convert.ToInt64(entity._phone.ToString());
                    password = entity._password.ToString();
                    profilephotoname = entity._photofilename.ToString();
                    subscriptionvalid = Convert.ToInt64(entity._tokenvalidity.ToString());

                    ApiAuthKey = entity._companyguid;
                    tokenbalance = entity._tokenbalance;
                    companyAuthcode = entity._companycode;
                    companyguid = entity._companyguid;
                    IncomingAPIdetail = entity._apitoken;
                    tokenrequestedflag = Convert.ToInt64(entity._tokenvalidity.ToString());
                    myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                   // Debug.Log(output1);
                    outputofsearchresult = output1.ToString();

            }
                return LatestId;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error retrieving latest ID is -->>: {LatestId} && {ex.Message} & {ex.StackTrace}");
                return LatestId;

            }

            //finally
            //{
            //    //Ensure resources are cleaned up
            //    if (reader != null)
            //    {
            //        reader.Close();
            //        reader.Dispose();
            //    }

            //    if (dbcmd != null)
            //    {
            //        dbcmd.Dispose();
            //    }
            //    //connection.Close();   // Explicitly close connection after operations
            //    //Debug.Log($"[Connection State] Closed: {connection.State}"+ "From the LOGIN CREDENTIALS AWAKE FUNCTION");
            //    //Debug.Log("Database resources disposed.");
            //}

           
        }

        //public void getwhatsappnumber()
        //{
        //    IDbCommand dbcmd = getDbCommand();
        //    dbcmd.CommandText =
        //        "SELECT *FROM " + TABLE_NAME + " WHERE (" + KEY_PHONE + " is not null and " + KEY_PHONE + " !=0) order by " + KEY_ID+" desc ";
        //    Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
        //    //dbcmd.ExecuteNonQuery();
        //    List<LoginEntity> myList = new List<LoginEntity>();
        //    IDataReader reader = dbcmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
        //         reader[3].ToString().Trim(),
        //          reader[4].ToString().Trim(),
        //            reader[5].ToString().Trim(),
        //        reader[6].ToString().Trim(),
        //       reader[7].ToString().Trim(),
        //        reader[8].ToString().Trim(),
        //        reader[9].ToString().Trim());


        //        whatsappnumber = Convert.ToInt64(entity._phone.ToString());
        //        Debug.Log("THIS WAS whatsappnumber -- " + whatsappnumber);
        //    }
        //}

        public void ForDeleteItemId1(int ID, string methodname)
        {
            Debug.Log(Tag + "Getting Search ID: " + ID);
            //str = Inputs.searchinventory.text.ToString();
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE  " + TABLE_NAME + " trim('   SQLite trim   ')";
            dbcmd.CommandText =
           "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + ID + "'";
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<LoginEntity> myList = new List<LoginEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                LoginEntity entity = new LoginEntity(int.Parse(reader[0].ToString()),
                reader[1].ToString().Trim(),
                reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                int.Parse(reader[9].ToString().Trim()),
                reader[10].ToString().Trim(),
                int.Parse(reader[11].ToString().Trim()),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim());

                // Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
            }
            reader.Dispose();
            if (methodname == "DeletethisCheck")
            {
                //inventorymanager.ItemDataStoring = outputofsearchresult;
                //inventorymanager.DeletethisCheck_Step_2();
            }
        }
        public override void deleteDataByString(string id)
        {
            Debug.Log(Tag + "Deleting Location: " + id);

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
            dbcmd.ExecuteNonQuery();
            deletethisitem deleted = new deletethisitem();
            deleted.ItemDeleted();
        }
        public void deleteDataByID1(int id)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " > '" + id + "'";
            dbcmd.ExecuteNonQuery();

        }
        public override void deleteDataById(int id)
        {
            base.deleteDataById(id);
        }

        public IDataReader deleteItemById(int id)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE  FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
            return dbcmd.ExecuteReader();
        }

        public override void deleteAllData()
        {
            Debug.Log(Tag + "Deleting AllData");

            base.deleteAllData(TABLE_NAME);

        }
        public override void DropTable()
        {
            Debug.Log(Tag + "Dropping Table");

            base.DropTable(TABLE_NAME);
        }

        public override void vacuumAllData()
        {
            Debug.Log(Tag + "Table Vacuum");

            base.vacuumAllData(TABLE_NAME);

        }
        public void sqlite_sequence()
        {
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText =
                "DELETE  FROM sqlite_sequence where name = '" + TABLE_NAME + "'";
            dbcmd.ExecuteNonQuery();
        }
        public override IDataReader getDataByString()
        {
            return base.getAllData(TABLE_NAME);
        }
        public IDataReader getAllData1(int size, int page)
        {
            return base.getAllData1(TABLE_NAME, size, page);
        }

        public IDataReader searchbystring(string userinput, int size, int page)
        {
            return base.searchbystring(TABLE_NAME, userinput, size, page);
        }

        public int totalRecords()
        {
            return base.totalRecords(TABLE_NAME);
        }

        public IDataReader featchalldata()

        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID;
            return dbcmd.ExecuteReader();

        }
        public IDataReader getLatestTimeStamp()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_TIMESTAMP + " DESC LIMIT 1";
            return dbcmd.ExecuteReader();

        }

        public void Updatedata(string columndataquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + columndataquery;

           //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();
        }
    }
}