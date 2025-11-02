using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank
{
    public class table_MyStocks : SqliteHelper
    {
        //Now, lets create our Inventory table. I will call this class table_Inventory, however, it is just a table in the OrionDB, 
    //this tables communicates with the SqliteHelper. It seems to act like a database and we shall treat it that way too, however its just a table.
    
        private const String Tag = "Orion: table_MyStocks:\t";
        private const String TABLE_NAME = "MyStocks";
        private const String KEY_ID = "Id";
        private const String KEY_STCKSYM = "StockSym";
        private const String KEY_STCKNAME = "StockName";
        private const String KEY_INDUSTRY = "Industry";
        private const String KEY_CLOSINGPRICEDATE = "ClosePriceDate";
        private const String KEY_DATEADD = "DateAdded";
        private const String KEY_CLOSINGPRICE = "ClosingPrice";
        private const String KEY_STATUS = "Status";

        private const String KEY_CURRENTROB = "CurrentROB";
        private const String KEY_UNIT = "Unit";
        private const String KEY_TIMESTAMP = "TimeStamp";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_STCKSYM, KEY_STCKNAME, KEY_INDUSTRY, KEY_CLOSINGPRICEDATE, KEY_DATEADD, KEY_CLOSINGPRICE, KEY_STATUS, KEY_TIMESTAMP };

       public MyWatchlistRead watchlistread;
       // public AddItem additem;
        public string searchresults;
        //public GameObject InventoryListItems;
        //public GameObject InventoryItems;
        public int LatestId;
        public int ThisitemId;
        public String outputofsearchresult;

       public Stocks_Accordion_Call_API_For_DB_READ1 stockscallapi;
        public deletethisitem deleteItemCall;

        public table_MyStocks() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                //KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                KEY_STCKSYM + " TEXT, " +
                KEY_STCKNAME + " TEXT, " +
                KEY_INDUSTRY + " TEXT, " + 
                KEY_CLOSINGPRICEDATE + " TEXT, " +
                KEY_DATEADD + " TEXT, " +
                KEY_CLOSINGPRICE + " NUMERIC, " +
                KEY_STATUS + " TEXT, " +
                KEY_TIMESTAMP + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();
        }

        public void addData(MyStckWatchEntity watchlist)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " (" 
                + KEY_STCKSYM + ", " 
                + KEY_STCKNAME + ", " 
                + KEY_INDUSTRY + ", " 
                + KEY_CLOSINGPRICEDATE + ", " 
                + KEY_DATEADD + ", " 
                + KEY_CLOSINGPRICE + ", " 
                + KEY_STATUS  +")"

                + "VALUES (' " 
                + watchlist._stocksym + " ' , ' " 
                + watchlist._stockname + " ' , ' " 
                + watchlist._industry + " ' , ' "
                + watchlist._closingpricedate + " ' , ' " 
                + watchlist._dateadd + " ' , ' " 
                + watchlist._closingprice + " ' , ' " 
                + watchlist._status + " ')";

            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader getDataById(int id)
        {
            return base.getDataById(id);
        }

        public void getDataByString1(string str)

        {
            //CLOSED Debug.Log(Tag + "Getting Search string: " + str);
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
           "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_STCKSYM + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_STCKNAME + " LIKE '%" + str.Replace("'", "''") + "%'";
            }
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                double.Parse(reader[6].ToString()),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim());

                Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._id;
            }
            reader.Dispose();
            searchresults = JsonUtility.ToJson(myList, true);

            //List<FromDatabase> BullishVBullStocks = new List<FromDatabase>();
            Stocks_Accordion_Call_API_For_DB_READ1 stockscallapi = new Stocks_Accordion_Call_API_For_DB_READ1();
            stockscallapi.StockSymbolSector = new List<string>();


            //foreach (var x in myList)
            //{
            //    FromDatabase DBstockDetails = new FromDatabase();

            //    DBstockDetails.id = x._id;
            //    DBstockDetails.stocksym = x._stocksym.ToString().Trim();

            //    //Debug.Log(x._id.ToString());
            //    //BullishVBullStocks.Add(DBstockDetails);
            //    stockscallapi.urlfeed = 0;
            //    stockscallapi.StockSymbolSector.Add(DBstockDetails.stocksym);
            //    stockscallapi.GetStocksData();

            //}

            //watchlistread = new MyWatchlistRead();
            //watchlistread.searchresultsforhere = myList;
            //watchlistread.queryresults();

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
            List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
                 reader[3].ToString().Trim(),
                  reader[4].ToString().Trim(),
                    reader[5].ToString().Trim(),
                double.Parse(reader[6].ToString()),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim());

                Debug.Log("Stock Code: " + entity._stocksym);
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
            "SELECT * FROM " + TABLE_NAME + " WHERE upper(trim(" + KEY_STCKSYM + ")) =upper('" + str + "')"; //-->>> does not work, also match upper or lower cases FOR cHECKING..
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMCODE + " = ' " + str + " '" + " or " + KEY_ITEMCODE + " ='" + str + "'";
            List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
                 reader[3].ToString().Trim(),
                  reader[4].ToString().Trim(),
                    reader[5].ToString().Trim(),
                double.Parse(reader[6].ToString()),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim());

                //Debug.Log("item code: " + entity._itemcode);
                myList.Add(entity);

                ThisitemId = entity._id;
                var output1 = JsonUtility.ToJson(entity, true);
                // Debug.Log(output1);
                outputofsearchresult = output1.ToString();
            }
            reader.Dispose();
            searchresults = JsonUtility.ToJson(myList, true);

           // inventorymanager.searchresultsforhere = myList;
            //additem.searchresultsforhere = myList;
            //additem.existingitemId = ThisitemId;


            if (methodname == "AddingNew")
            {
                getLatestID();
            }

            //else if (methodname == "ScannedQR")

            //{
            //    inventorymanager.queryresults();
            //}

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
            List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
                 reader[3].ToString().Trim(),
                  reader[4].ToString().Trim(),
                    reader[5].ToString().Trim(),
                double.Parse(reader[6].ToString()),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim());

                Debug.Log("Stock Code: " + entity._stocksym);
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

        public void getLatestID()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID + " DESC LIMIT 1";

            List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
                 reader[3].ToString().Trim(),
                  reader[4].ToString().Trim(),
                    reader[5].ToString().Trim(),
                double.Parse(reader[6].ToString()),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim());

                LatestId = entity._id;
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();

            }
            reader.Dispose();
            //inventorymanager.newitemid = LatestId + 1;
            //inventorymanager.Queryresults();
            //additem.newitemid = LatestId + 1;
            //additem.Queryresults();
        }
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
            List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
                 reader[3].ToString().Trim(),
                  reader[4].ToString().Trim(),
                    reader[5].ToString().Trim(),
                double.Parse(reader[6].ToString()),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim());

                Debug.Log("Stock Code: " + entity._stocksym);
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

            Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();
        }
    }
}