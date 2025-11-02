using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank

{
    public class table_Synch : SqliteHelper

    //Now, lets create our Synch table. I will call this class table_Synch, however, it is just a table in the OrionDB, 
    //this tables communicates with the SqliteHelper. It seems to act like a database and we shall treat it that way too, however its just a table.
    {

        private const String Tag = "Orion: table_Synch:\t";
        private const String TABLE_NAME = "Synch";
        private const String KEY_ID = "Id";
        private const String KEY_MACHINEID = "MachineId";
        
        private const String KEY_DATA = "Data";
        private const String KEY_ITEMCODE = "ItemCode";
        private const String KEY_QUANTITY = "Quantity";

        private const String KEY_PROCESSED = "Processed";
        private const String KEY_SOURCETABLE = "SourceTable";
        private const String KEY_FLAG = "Flag";
        private const String KEY_OUTBOUNDFILENAME = "FileName";
        private const String KEY_TIMESTAMP = "TimeStamp";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_MACHINEID, KEY_DATA, KEY_ITEMCODE ,KEY_QUANTITY, KEY_PROCESSED, KEY_SOURCETABLE, KEY_FLAG,KEY_OUTBOUNDFILENAME,KEY_TIMESTAMP };
        public int CountRecords;

        //********* IMPORTANT FOR SYNCH, commented by Mohit 21st Feb 2024

        //How to additem to the Synch Table and Synchronise; 
      //  public AddItem additem; // This line was used to associate table_Synch Script with the AddItem Script by writing "synchtable.additem = this;" in the start method of AddItem Script
        //From Additem function named; "public void Synchronise()" was used to send items to Synch table for outbound.


        public table_Synch() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                //KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                KEY_MACHINEID + " TEXT, " +
                KEY_DATA + " TEXT, " +
                KEY_ITEMCODE + " TEXT, " +
                KEY_QUANTITY + " TEXT, " +
                KEY_PROCESSED + " TEXT, " +
                KEY_SOURCETABLE + " TEXT, " +
                KEY_FLAG + " TEXT, " +
                KEY_OUTBOUNDFILENAME + " TEXT, " +
                KEY_TIMESTAMP + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();

            //Debug.Log("TABLE SYNCH CREATED");
        }
        
        public void addData(SynchEntity synchdata)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ("+ KEY_MACHINEID + ", "+ KEY_DATA + ", " + KEY_ITEMCODE + ", " + KEY_QUANTITY + ", " + KEY_PROCESSED + ", " + KEY_SOURCETABLE + ", " + KEY_OUTBOUNDFILENAME + ", " + KEY_FLAG + ")"
                //"+  KEY_ID + ",  next line; ' " + synchdata._id + " ' ,
                + "VALUES ( ' " + synchdata._machineid + " ' , ' "+ synchdata._data + " ' , ' " + synchdata._itemcode + " ' , ' " + synchdata._quantity + " ' , ' " + synchdata._processed + " ', ' " + synchdata._sourcetable + " ', ' " + synchdata._filename + " ', ' " + synchdata._flag + " ')";

            dbcmd.ExecuteNonQuery();

        }

        public override IDataReader getDataByString()
        {
            return base.getAllData(TABLE_NAME);
        }

        public IDataReader NotyetProcessed()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_PROCESSED + " LIKE '%N%' order by id DESC";
            return dbcmd.ExecuteReader();
            
        }
        public IDataReader Inqueue(int size, int page)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_PROCESSED + " LIKE '%N%' order by id DESC";
            return dbcmd.ExecuteReader();

        }
        //    public override IDataReader getDataById(int id)
        //    {
        //        return base.getDataById(id);
        //    }

        //    public  void getDataByString1(string str)
        //    {
        //        inventorymanager = new InventoryDataAndQuery();
        //        //InventoryListItems = GameObject.Instantiate(Resources.Load("Prefabs/InventoryItemListImage")) as GameObject;

        //        Debug.Log(Tag + "Getting Search string: " + str);
        //        //str = Inputs.searchinventory.text.ToString();
        //        IDbCommand dbcmd = getDbCommand();
        //        dbcmd.CommandText =
        //            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%"+str.Replace("'","''")+ "%' or " + KEY_ITEMCODE + " LIKE '%" + str.Replace("'", "''") + "%'";
        //        //return dbcmd.ExecuteReader();
        //        List<InventoryEntity> myList = new List<InventoryEntity>();
        //        IDataReader reader = dbcmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            InventoryEntity entity = new InventoryEntity(reader[0].ToString(),
        //            reader[1].ToString(),
        //            reader[2].ToString(),
        //            reader[3].ToString(),
        //            reader[4].ToString(),
        //            reader[5].ToString(),
        //            reader[6].ToString(),
        //            reader[7].ToString(),
        //            reader[8].ToString(),
        //            reader[9].ToString(),
        //            reader[10].ToString(),
        //            reader[11].ToString(),
        //            int.Parse(reader[12].ToString()),
        //            reader[13].ToString(),
        //            reader[14].ToString());
        //            Debug.Log("item code: " + entity._itemcode);
        //            myList.Add(entity);

        //        }

        //        //searchresults = JsonUtility.ToJson(myList, true);
        //        inventorymanager.searchresultsforhere = myList;
        //        inventorymanager.queryresults();

        //    }

        //    public override void deleteDataByString(string id)
        //    {
        //        Debug.Log(Tag + "Deleting Location: " + id);

        //        IDbCommand dbcmd = getDbCommand();
        //        dbcmd.CommandText =
        //            KEY_PROCESSED  "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
        //        dbcmd.ExecuteNonQuery();
        //    }

        //    public override void deleteDataById(int id)
        //    {
        //        base.deleteDataById(id);
        //    }

        public override void deleteAllData()
        {
            Debug.Log(Tag + "Deleting Table");

            base.deleteAllData(TABLE_NAME);

        }

        public override void vacuumAllData()
        {
            Debug.Log(Tag + "Table Vacuum");

            base.vacuumAllData(TABLE_NAME);

        }
        public override void DropTable()
        {
            Debug.Log(Tag + "Dropping Table");

            base.DropTable(TABLE_NAME);
        }

        public IDataReader getLatestID()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID + " DESC LIMIT 1";
            return dbcmd.ExecuteReader();
        }

        public int getMaxID()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT count(" + KEY_ID + ") FROM " + TABLE_NAME + "";
            return Convert.ToInt32(dbcmd.ExecuteScalar());
        }

        //public string getLatestID1()
        //{
        //    IDbCommand dbcmd = getDbCommand();
        //    dbcmd.CommandText =
        //        "SELECT id FROM " + TABLE_NAME + " ORDER BY " + KEY_ID + " DESC LIMIT 1";
        //    return Convert.ToString(dbcmd.ExecuteScalar());
        //}

        //    public override IDataReader getDataByString()
        //    {
        //        return base.getAllData(TABLE_NAME);
        //    }

        //    //public IDataReader getNearestLocation(LocationInfo loc)
        //    //{
        //    //    Debug.Log(Tag + "Getting nearest centoid from: "
        //    //        + loc.latitude + ", " + loc.longitude);
        //    //    IDbCommand dbcmd = getDbCommand();

        //    //    string query =
        //    //        "SELECT * FROM "
        //    //        + TABLE_NAME
        //    //        + " ORDER BY ABS(" + KEY_LAT + " - " + loc.latitude
        //    //        + ") + ABS(" + KEY_LNG + " - " + loc.longitude + ") ASC LIMIT 1";

        //    //    dbcmd.CommandText = query;
        //    //    return dbcmd.ExecuteReader();
        //    //}

        public IDataReader getLatestTimeStamp()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_TIMESTAMP + " DESC LIMIT 1";
            return dbcmd.ExecuteReader();
        }

        public IDataReader getDatabyQuery(string query)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = query;
            //dbcmd.CommandText = "SELECT Cloud_DB_ID,Template_Section_Ques,Obs_Details_1,ROVIQSequence FROM " + TABLE_NAME + " where Obs_Details_8='Question'";
            return dbcmd.ExecuteReader();
        }

        public IDataReader CountNotProcessed()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT COUNT(*) FROM " + TABLE_NAME + " WHERE " + KEY_PROCESSED + " LIKE '%N%'";
            CountRecords = int.Parse(dbcmd.ExecuteScalar().ToString());
            return dbcmd.ExecuteReader();

        }
        public void Updatedata(string columndataquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + columndataquery;

            Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();
        }
        public int totalRecords()
        {
            return base.totalRecords(TABLE_NAME);
        }
        public IDataReader getAllData1(int size, int page)
        {
            return base.getAllData1(TABLE_NAME, size, page);
        }

    }
}