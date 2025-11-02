using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace DataBank
{
    public class table_Inspection_Attachments : SqliteHelper
    {
        private const String Tag = "Orion: table_Inspection_Attachments:\t";
        private const String TABLE_NAME = "Inspection_Attachments";
        private const String KEY_ID = "ID";
        private const String KEY_Inspection_PrimaryDetails_ID = "Inspection_PrimaryDetails_ID";
        private const String KEY_Inspection_Observations_ID = "Inspection_Observations_ID";
        private const String KEY_Attachment_Title = "Attachment_Title";
        private const String KEY_Attachment_Name = "Attachment_Name";
        private const String KEY_Attachment_Path = "Attachment_path";
        private const String KEY_Attachment_Details_1 = "Attachment_Details_1";
        private const String KEY_Attachment_Details_2 = "Attachment_Details_2";
        private const String KEY_Attachment_Details_3 = "Attachment_Details_3";
        private const String KEY_Attachment_Details_4 = "Attachment_Details_4";
        private const String KEY_Active = "Active";
        private const String KEY_Timestamp = "Timestamp";

        private String[] COLUMNS = new String[] {KEY_ID,
            KEY_Inspection_PrimaryDetails_ID,
            KEY_Inspection_Observations_ID,
            KEY_Attachment_Title,
            KEY_Attachment_Name,
            KEY_Attachment_Path,
            KEY_Attachment_Details_1,
            KEY_Attachment_Details_2,
            KEY_Attachment_Details_3,
            KEY_Attachment_Details_4,
            KEY_Active,
            KEY_Timestamp };

        public int LatestId;
        public int ThisitemId;
        public String outputofsearchresult;
        public int CountRecords;

        public table_Inspection_Attachments() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                    KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, "
                    + KEY_Inspection_PrimaryDetails_ID + ","
                    + KEY_Inspection_Observations_ID + ","
                    + KEY_Attachment_Title + ","
                    + KEY_Attachment_Name + ","
                    + KEY_Attachment_Path + ","
                    + KEY_Attachment_Details_1 + ","
                    + KEY_Attachment_Details_2 + ","
                    + KEY_Attachment_Details_3 + ","
                    + KEY_Attachment_Details_4 + ","
                    + KEY_Active + ","
                    + KEY_Timestamp + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();

           
        }

        public void addData(Inspection_AttachmentsEntity Inspection_AttachmentsEntity)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =

                "INSERT INTO " + TABLE_NAME
                + " ( " + KEY_Inspection_PrimaryDetails_ID + ","
                        + KEY_Inspection_Observations_ID + ","
                        + KEY_Attachment_Title + ","
                        + KEY_Attachment_Name + ","
                        + KEY_Attachment_Path + ","
                        + KEY_Attachment_Details_1 + ","
                        + KEY_Attachment_Details_2 + ","
                        + KEY_Attachment_Details_3 + ","
                        + KEY_Attachment_Details_4 + ","
                        + KEY_Active + ")"

                + "VALUES (' "
                + Inspection_AttachmentsEntity._Inspection_PrimaryDetails_ID + " ' , ' "
                + Inspection_AttachmentsEntity._Inspection_Observations_ID + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Title + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Name + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Path + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Details_1 + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Details_2 + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Details_3 + " ' , ' "
                + Inspection_AttachmentsEntity._Attachment_Details_4 + " ' , ' "
                + Inspection_AttachmentsEntity._Active + " ')";

            dbcmd.ExecuteNonQuery();
        }
        public override IDataReader getDataById(int id)
        {
            return base.getDataById(id);
        }

        public void Getinspectiontemplates()

        {
            IDbCommand dbcmd = getDbCommand();
            //dbcmd.CommandText =
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = 1 and " + KEY_Active + " = 1 " + " ORDER BY " + KEY_Template_Section_Ques;

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;
            }
            reader.Dispose();
        }

        public void GetSectionHeaders(int templateinspid)
        {

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = " + templateinspid + " ORDER BY " + "cast(" + KEY_ID + " as int)";

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());
                               
                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                //Debug.Log(output1);
                //outputofsearchresult = output1.ToString();
                outputofsearchresult = JsonConvert.SerializeObject(myList);
                ThisitemId = entity._ID;
            }
            reader.Dispose();
        }

        public void GetQuestions(int sectionHeaderID)
        {

            IDbCommand dbcmd = getDbCommand();
            //dbcmd.CommandText =
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = '" + sectionHeaderID + "'" + KEY_Active + " = 1 " + " ORDER BY " + KEY_Inspection_Template_ID;

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;
            }
            reader.Dispose();
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
                //NEED TO DECIDE WHERE TO USE WHAT FOR NOW ONLY WRITTEN TO AVOID ERRORS
                //     dbcmd.CommandText =
                //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Details_8 + " LIKE '%" + str.Replace("'", "''") + "%'" + " ORDER BY " + KEY_ID;
                //     //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Details_8 + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_Template_Section_Ques + " LIKE '%" + str.Replace("'", "''") + "%'";
            }
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                //Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;
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
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

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
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

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

        public int getLatestID()
        {
            LatestId = 0;
            try
            {


                IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText =
                    "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID + " DESC LIMIT 1";

                List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
                using IDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                    int.Parse(reader[0].ToString()),
                    int.Parse(reader[1].ToString()),
                    int.Parse(reader[2].ToString()),
                    reader[3].ToString().Trim(),
                    reader[4].ToString(),
                    reader[5].ToString().Trim(),
                    reader[6].ToString().Trim(),
                    reader[7].ToString().Trim(),
                    reader[8].ToString().Trim(),
                    reader[9].ToString().Trim(),
                    reader[10].ToString().Trim(),
                    reader[11].ToString().Trim());

                    LatestId = entity._ID;
                    //MarketSet = entity._marketset.ToString();
                    //Session = entity._sessionstate.ToString();
                    //useremail = entity._email.ToString();
                    //usersubsmarkets = entity._markets.ToString();
                    //whatsappnumber = Convert.ToInt64(entity._phone.ToString());
                    //password = entity._password.ToString();
                    //subscriptionvalid = Convert.ToInt64(entity._tokenvalidity.ToString());
                    myList.Add(entity);

                    var output1 = JsonUtility.ToJson(entity, true);
                    //CLOSED Debug.Log(output1);
                    outputofsearchresult = output1.ToString();

                }
                reader.Dispose();
                return LatestId;
                
            }
            catch (Exception ex)
            {
                return LatestId;
            }
            //Debug.Log(outputofsearchresult);
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
            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

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
        public void deleteDataByquery(string columnquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE FROM " + TABLE_NAME + " WHERE " + columnquery;
            dbcmd.ExecuteNonQuery();

        }

        public IDataReader SelectDataByquery(string columnquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "Select *FROM " + TABLE_NAME + " WHERE " + columnquery;

            return dbcmd.ExecuteReader();

        }


        public IDataReader SelectDataByquery1(string columnquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = columnquery;

            //dbcmd.CommandText = "Select *FROM " + TABLE_NAME + " WHERE " + columnquery;

            return dbcmd.ExecuteReader();

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

        public IDataReader getdataforreader(string str)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_PrimaryDetails_ID + " LIKE '%" + str.Replace("'", "''") + "%'" + " ORDER BY " + KEY_ID;
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
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_Timestamp + " DESC LIMIT 1";
            return dbcmd.ExecuteReader();

        }

        public IDataReader IfRecordExists(string query)
        {
            CountRecords = 0;
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT COUNT(*) FROM " + query;
            CountRecords = int.Parse(dbcmd.ExecuteScalar().ToString());
            return dbcmd.ExecuteReader();

        }

        public void Updatedata(string columndataquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + columndataquery;

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

        public void Deletedata(string columndataquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "DELETE  FROM " + TABLE_NAME + columndataquery;

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

    }
}