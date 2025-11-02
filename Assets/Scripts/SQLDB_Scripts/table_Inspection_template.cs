using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;

namespace DataBank
{
    public class table_Inspection_template : SqliteHelper
    {
        //Now, lets create our Inventory table. I will call this class table_Inventory, however, it is just a table in the OrionDB, 
        //this tables communicates with the SqliteHelper. It seems to act like a database and we shall treat it that way too, however its just a table.

        private const String Tag = "Orion: table_Inspection_template:\t";
        private const String TABLE_NAME = "Inspection_template";
        private const String KEY_ID = "ID";
        private const String KEY_Inspection_Template_ID = "InspectionTemplateID";
        private const String KEY_Cloud_DB_ID = "Cloud_DB_ID";
        private const String KEY_Cloud_DB_ParentID = "Cloud_DB_ParentID";
        private const String KEY_Version_Number = "Version_Number";
        private const String KEY_Version_Date = "Version_Date";
        private const String KEY_Template_Section_Ques = "Template_Section_Ques";
        private const String KEY_Details_1 = "Details_1"; 
        private const String KEY_Details_2 = "Details_2";
        private const String KEY_Details_3 = "Details_3";
        private const String KEY_Details_4 = "Details_4";
        private const String KEY_Details_5 = "Details_5";
        private const String KEY_Details_6 = "Details_6";
        private const String KEY_Details_7 = "Details_7";
        private const String KEY_Details_8 = "Details_8";
        
        //Below related specifically to the SIRE 2.0 (Year 2024)
        private const String KEY_ShortQuestionText_Sire = "ShortQuestionText";
        private const String KEY_VesselTypes_Sire = "VesselTypes";
        private const String KEY_ROVIQSequence_Sire = "ROVIQSequence";
        private const String KEY_Publications_Sire = "Publications";
        private const String KEY_Objective_Sire= "Objective";
        private const String KEY_IndustryGuidance= "IndustryGuidance";
        private const String KEY_TMSAKPI = "TMSAKPI";
        private const String KEY_IMOISMCode = "IMOISMCode";
        private const String KEY_InspectionGuidance= "InspectionGuidance";
        private const String KEY_SuggestedInspectorActions = "SuggestedInspectorActions";
        private const String KEY_ExpectedEvidence= "ExpectedEvidence";
        private const String KEY_PotentialGroundsNegativeObservation = "PotentialGroundsObs";
        
        private const String KEY_Active = "Active";
        private const String KEY_Timestamp = "Timestamp";

        private String[] COLUMNS = new String[] { KEY_Cloud_DB_ID, KEY_Cloud_DB_ParentID, KEY_Version_Number, KEY_Version_Date, KEY_Template_Section_Ques, KEY_Details_1, KEY_Details_2, KEY_Details_3, KEY_Details_4, KEY_Details_5, KEY_Details_6, KEY_Details_7, KEY_Details_8,
            KEY_ShortQuestionText_Sire,KEY_VesselTypes_Sire,KEY_ROVIQSequence_Sire,KEY_Publications_Sire,KEY_Objective_Sire,KEY_IndustryGuidance,KEY_TMSAKPI,KEY_IMOISMCode,
KEY_InspectionGuidance,
KEY_SuggestedInspectorActions,
KEY_ExpectedEvidence,
KEY_PotentialGroundsNegativeObservation,KEY_Active, KEY_Timestamp };

        //public MyWatchlistRead watchlistread;
        // public AddItem additem;
        //public string searchresults;
        //public GameObject InventoryListItems;
        //public GameObject InventoryItems;
        public int LatestId;
        //public string MarketSet;
        //public string Session;
        //public long subscriptionvalid;
        //public string useremail;
        //public string usersubsmarkets;
        //public long whatsappnumber;
        public int ThisitemId;
        public String outputofsearchresult;
        //public string password;

        //public Stocks_Accordion_Call_API_For_DB_READ1 stockscallapi;
        //public deletethisitem deleteItemCall;

        public DateTime versiontimestamp;

        public table_Inspection_template() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " + 
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " 
                + KEY_Inspection_Template_ID + ","
            + KEY_Cloud_DB_ID + ","
            + KEY_Cloud_DB_ParentID + ","
            + KEY_Version_Number + ","
            + KEY_Version_Date + ","
            + KEY_Template_Section_Ques + ","
            + KEY_Details_1 + ","
            + KEY_Details_2 + ","
            + KEY_Details_3 + ","
            + KEY_Details_4 + ","
            + KEY_Details_5 + ","
            + KEY_Details_6 + ","
            + KEY_Details_7 + ","
            + KEY_Details_8 + ","
            + KEY_ShortQuestionText_Sire + ","
            + KEY_VesselTypes_Sire + ","
            + KEY_ROVIQSequence_Sire + ","
            + KEY_Publications_Sire + ","
            + KEY_Objective_Sire + ","
            + KEY_IndustryGuidance + ","
            + KEY_TMSAKPI + ","
            + KEY_IMOISMCode + ","
            + KEY_InspectionGuidance + ","
            + KEY_SuggestedInspectorActions + ","
            + KEY_ExpectedEvidence + ","
            + KEY_PotentialGroundsNegativeObservation + ","
            + KEY_Active + ","
            + KEY_Timestamp +  " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();
        }

        public void addData(InspectionTemplateEntity templates)
        {
            using IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( " + KEY_Inspection_Template_ID + ","
                + KEY_Cloud_DB_ID + ","
                + KEY_Cloud_DB_ParentID + ","
                + KEY_Version_Number + ","
                + KEY_Version_Date + ","
                + KEY_Template_Section_Ques + ","
                + KEY_Details_1 + ","
                + KEY_Details_2 + ","
                + KEY_Details_3 + ","
                + KEY_Details_4 + ","
                + KEY_Details_5 + ","
                + KEY_Details_6 + ","
                + KEY_Details_7 + ","
                + KEY_Details_8 + ","
                + KEY_ShortQuestionText_Sire + ","
                + KEY_VesselTypes_Sire + ","
                + KEY_ROVIQSequence_Sire + ","
                + KEY_Publications_Sire + ","
                + KEY_Objective_Sire + ","
                + KEY_IndustryGuidance + ","
                + KEY_TMSAKPI + ","
                + KEY_IMOISMCode + ","
                + KEY_InspectionGuidance + ","
                + KEY_SuggestedInspectorActions + ","
                + KEY_ExpectedEvidence + ","
                + KEY_PotentialGroundsNegativeObservation + ","
                + KEY_Active + ")"

                + "VALUES (' "
                + templates._Inspection_Template_ID + " ' , ' " +
                templates._Cloud_DB_ID + " ' , ' " +
                templates._Cloud_DB_ParentID + " ' , ' " +
                templates._Version_Number + " ' , ' " +
                templates._Version_Date + " ' , ' " +
                templates._Template_Section_Ques + " ' , ' " +
                templates._Details_1 + " ' , ' " +
                templates._Details_2 + " ' , ' " +
                templates._Details_3 + " ' , ' " +
                templates._Details_4 + " ' , ' " +
                templates._Details_5 + " ' , ' " +
                templates._Details_6 + " ' , ' " +
                templates._Details_7 + " ' , ' " +
                templates._Details_8 + " ' , ' " +
                templates._ShortQuestionText_Sire + " ' , ' " +
                templates._VesselTypes_Sire + " ' , ' " +
                templates._ROVIQSequence_Sire + " ' , ' " +
                templates._Publications_Sire + " ' , ' " +
                templates._Objective_Sire + " ' , ' " +
                templates._IndustryGuidance + " ' , ' " +
                templates._TMSAKPI + " ' , ' " +
                templates._IMOISMCode + " ' , ' " +
                templates._InspectionGuidance + " ' , ' " +
                templates._SuggestedInspectorActions + " ' , ' " +
                templates._ExpectedEvidence + " ' , ' " +
                templates._PotentialGroundsNegativeObservation + " ' , ' " +
                templates._Active  + " ')";
            //+ ","+KEY_Timestamp
            //+" ' , ' " +templates._Timestamp

         int result=  dbcmd.ExecuteNonQuery();
           Debug.Log("Inserted" + result);
        }

        public override IDataReader getDataById(int id)
        {
            return base.getDataById(id);
        }

        public void Getinspectiontemplates()

        {
          
          using  IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = 1 and " + KEY_Active + " = 1 " + " ORDER BY " + KEY_Template_Section_Ques;
            
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;
            }
            reader.Dispose();
        }

        public void GetSectionHeaders (int templateinspid) //NOticed by own self CHANGED after Manish Tyagi release... KEY_Active + " = 'Y' from KEY_Active + " = 1
        {
            versiontimestamp = Convert.ToDateTime("", CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat); ;
           using IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = '" + templateinspid + "'" + KEY_Active + " = 'Y' " + " ORDER BY " + KEY_Inspection_Template_ID;

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;
                versiontimestamp = Convert.ToDateTime(entity._Timestamp, CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            }
            reader.Dispose();
        }

        public void GetQuestions (int sectionHeaderID) //NOticed by own self CHANGED after Manish Tyagi release...KEY_Active + " = 'Y' from KEY_Active + " = 1
        {

          using  IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = '" + sectionHeaderID + "'" + KEY_Active + " = 'Y' " + " ORDER BY " + KEY_Inspection_Template_ID;

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

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
         using   IDbCommand dbcmd = getDbCommand();

            if (str == "fetchall")
            {
                dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID;
            }
            else
            {
                //NEED TO DECIDE WHERE TO USE WHAT FOR NOW ONLY WRITTEN TO AVOID ERRORS
                dbcmd.CommandText =
           "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Details_8 + " LIKE '%" + str.Replace("'", "''") + "%'" + " ORDER BY " + KEY_ID;
                //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Details_8 + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_Template_Section_Ques + " LIKE '%" + str.Replace("'", "''") + "%'";
            }
            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
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
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

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
            "SELECT * FROM " + TABLE_NAME + " WHERE upper(trim(" + KEY_Template_Section_Ques + ")) =upper('" + str + "')"; //-->>> does not work, also match upper or lower cases FOR cHECKING..
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMCODE + " = ' " + str + " '" + " or " + KEY_ITEMCODE + " ='" + str + "'";
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

                //Debug.Log("item code: " + entity._itemcode);
                myList.Add(entity);

                ThisitemId = entity._ID;
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
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
                   int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

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


             using   IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText =
                    "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_ID + " DESC LIMIT 1";

                List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
                using IDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    InspectionTemplateEntity entity = new InspectionTemplateEntity(
               int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

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

        //public void getwhatsappnumber()
        //{
        //    IDbCommand dbcmd = getDbCommand();
        //    dbcmd.CommandText =
        //        "SELECT *FROM " + TABLE_NAME + " WHERE (" + KEY_PHONE + " is not null and " + KEY_PHONE + " !=0) order by " + KEY_ID+" desc ";
        //    Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
        //    //dbcmd.ExecuteNonQuery();
        //    List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
        //    IDataReader reader = dbcmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        InspectionTemplateEntity entity = new InspectionTemplateEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
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
            List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                InspectionTemplateEntity entity = new InspectionTemplateEntity(
               int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString().Trim(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim(),
                reader[12].ToString().Trim(),
                reader[13].ToString().Trim(),
                reader[14].ToString().Trim(),
                reader[15].ToString().Trim(),
                reader[16].ToString().Trim(),
                reader[17].ToString().Trim(),
                reader[18].ToString().Trim(),
                reader[19].ToString().Trim(),
                reader[20].ToString().Trim(),
                reader[21].ToString().Trim(),
                reader[22].ToString().Trim(),
                reader[23].ToString().Trim(),
                reader[24].ToString().Trim(),
                reader[25].ToString().Trim(),
                reader[26].ToString().Trim(),
                reader[27].ToString().Trim(),
                reader[28].ToString().Trim());

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

        public IDataReader getDatabyQuery(string query)
        {
          using  IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = query;
            //dbcmd.CommandText = "SELECT Cloud_DB_ID,Template_Section_Ques,Obs_Details_1,ROVIQSequence FROM " + TABLE_NAME + " where Obs_Details_8='Question'";
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

        public IDataReader deleteItemsQuery(string query)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE  FROM " + TABLE_NAME + " WHERE " + query;
            return dbcmd.ExecuteReader();
        }

        public IDataReader getdataforreader(string str)
        {
          using  IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Details_8 + " LIKE '%" + str.Replace("'", "''") + "%'" + " ORDER BY " + KEY_Template_Section_Ques +" ," + KEY_ID;
            return dbcmd.ExecuteReader();
        }

        public IDataReader getfullchecklistdata(int checklistID)
        {
         using   IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) = " + checklistID + " ORDER BY " + KEY_ID;
            return dbcmd.ExecuteReader();
        }

        public IDataReader getChapterData(int passingcloudparentID)   //FOR Querying Chapters;
        {
          using  IDbCommand dbcmd = getDbCommand();
            
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) = " + passingcloudparentID + " and " + KEY_Details_8 + " like '%Chapter%'" + " ORDER BY " + "cast( " + KEY_Template_Section_Ques.ToString().Split(' ')[0] + " as int)";

            return dbcmd.ExecuteReader();
        }

        public IDataReader getquestionsData(int passingclouddbID)    //FOR Querying Questions;
        {
          using  IDbCommand dbcmd = getDbCommand();
            
          
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) = " + passingclouddbID + " and " + KEY_Details_8 + " like '%Question%'" + " ORDER BY " + "cast(" + KEY_Template_Section_Ques.Trim() + " as float)";

            return dbcmd.ExecuteReader();
        }

        public IDataReader getquestionsDataForChapters(String chapterids, string vesseltype)    //FOR Querying Questions from Template to Observation table, added Vessel type on 05th Oct 2024 for filtering questions for a specific type of vessel/s;
        {
          using  IDbCommand dbcmd = getDbCommand();

            string multiplevesseltypes = "";

            if (vesseltype.Contains ("oil") || vesseltype.Contains("tanker"))

            {
                multiplevesseltypes = "%Oil%";

            }

            if (vesseltype.Contains("shuttle"))

            {
                multiplevesseltypes = "%Shuttle%%' or VesselTypes like '%Oil%";

            }

            if (vesseltype.Contains("chemical"))

            {
                multiplevesseltypes = "%Oil / Chemical Tanker%' or VesselTypes " +
                    "like '%Chemical Carrier%' or VesselTypes " + 
                    "like '%Shuttle Tanker%' or VesselTypes " +
                    "like  '%Chemical Carrier,Oil / Chemical Tanker,Oil Tanker (Product and Crude),Shuttle Tanker%";
               
            }

            if (vesseltype.Contains("lpg") )

            {
                multiplevesseltypes = "%LPG (Semi and Fully Refrigerated)%";
              
            }

            if (vesseltype.Contains("lngmembrane"))

            {
                multiplevesseltypes = "%LNG (Membrane Tanks)%' or VesselTypes " +
                    " like '%LNG (Membrane Tanks),LNG (Moss-type Tanks)%";
               
            }
            if (vesseltype.Contains("lngmoss"))

            {
                multiplevesseltypes = "%LNG (Moss-type Tanks)%' or VesselTypes " +
                    " like '%LNG (Membrane Tanks),LNG (Moss-type Tanks)%";
               
            }

            if (vesseltype.Contains("combination") || vesseltype.Contains("obo"))

            {
                multiplevesseltypes = "%OBO / Combination Carriers%' or VesselTypes " +
                "like '%oil tanker%' and VesselTypes not like '%chemical%";
               
            }

            //if (vesseltype.Contains("combination")|| vesseltype.Contains("general") || vesseltype.Contains("container")|| vesseltype.Contains("transhipper"))

            //{
            //    multiplevesseltypes = "%Bulk Carrier,Chemical Carrier,Container Vessel,General Cargo,Oil Tanker (Product and Crude),Passenger Vessel%";
            //    dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " + KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE(" + KEY_VesselTypes_Sire + ", ' ', '')))" + " like '" + multiplevesseltypes + "' OR " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " + KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE(" + KEY_VesselTypes_Sire + ", ' ', ''))) = '' ORDER BY " + "cast(" + KEY_Template_Section_Ques.Trim() + " as float)";

            //}

            else
            {
                multiplevesseltypes = vesseltype;
                //dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") " +
                //    "and " + KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE(" + KEY_VesselTypes_Sire + ", ' ', '')))" + " like '" + vesseltype
                //    + "' OR " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " + KEY_Details_8 + " like '%Question%' and "
                //    + "LOWER(TRIM(REPLACE(" + KEY_VesselTypes_Sire + ", ' ', ''))) = '' ORDER BY " + "cast(" + KEY_Template_Section_Ques.Trim() + " as float)";
                //Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            }


            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " +
                    KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE(" + KEY_VesselTypes_Sire + ", ' ', '')))" + " like '" +
                    multiplevesseltypes.ToLower().Trim().Replace(" ", "") + "' OR " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " +
                    KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE(" + KEY_VesselTypes_Sire + ", ' ', ''))) = '' ORDER BY " +
    "CAST(SUBSTR(Template_Section_Ques || '.', 1,INSTR(Template_Section_Ques || '.', '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1, INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') + 1) AS INTEGER) ASC;";

            //dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " + KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE("+KEY_VesselTypes_Sire+", ' ', '')))" + " like '"+ vesseltype  +"' OR " + "cast(" + KEY_Cloud_DB_ParentID + " as int) in (" + chapterids + ") and " + KEY_Details_8 + " like '%Question%' and " + "LOWER(TRIM(REPLACE("+KEY_VesselTypes_Sire+", ' ', ''))) = '' ORDER BY " + "cast(" + KEY_Template_Section_Ques.Trim() + " as float)";
            Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
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
          using  IDbCommand dbcmd = getDbCommand();
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

        public void Updatedata(string columndataquery)
        {
          using  IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + columndataquery;

             Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();
        }

    }
}