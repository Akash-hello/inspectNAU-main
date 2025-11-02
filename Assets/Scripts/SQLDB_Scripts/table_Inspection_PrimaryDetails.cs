using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

namespace DataBank
{

    public class table_Inspection_PrimaryDetails : SqliteHelper
    {
        private const String Tag = "Orion: table_Inspection_PrimaryDetails:\t";
        private const String TABLE_NAME = "Inspection_PrimaryDetails";
        private const String KEY_ID = "ID";
        private const String KEY_Vessel_Name = "Vessel_Name";
        private const String KEY_Vessel_IMO = "Vessel_IMO";
        
        private const String KEY_Type = "Type";
        private const String KEY_Checklist_ID = "Checklist_ID";
        private const String KEY_Vessel_TYPE = "Vessel_TYPE";
        private const String KEY_Vessel_Extra1 = "Vessel_Extra1";
        private const String KEY_Vessel_Extra2 = "Vessel_Extra2";
        private const String KEY_Vessel_Extra3 = "Vessel_Extra3";

        private const String KEY_Inspection_Date = "Inspection_Date";
        private const String KEY_Inspector_Name = "Inspector_Name";
        private const String KEY_Inspector_Company = "Inspector_Company";
        private const String KEY_Location_Port_Sea = "Location_Port_Sea";
        private const String KEY_Port = "Port";
        private const String KEY_Country = "Country";
        private const String KEY_Opening_Meeting_DatetimeFm = "Opening_Meeting_DatetimeFm";
        private const String KEY_Opening_Meeting_DatetimeTo = "Opening_Meeting_DatetimeTo";
        private const String KEY_Closing_Meeting_DatetimeFm = "Closing_Meeting_DatetimeFm";
        private const String KEY_Closing_Meeting_DatetimeTo = "Closing_Meeting_DatetimeTo";
        private const String KEY_Master_Name = "Master_Name";
        private const String KEY_Chief_Engineer_Name = "Chief_Engineer_Name";
        private const String KEY_Chief_Officer_Name = "Chief_Officer_Name";
        private const String KEY_Second_Engr_Name = "Second_Engr_Name";
        
        private const String KEY_Comments = "Comments";
        private const String KEY_Status = "Status";
        private const String KEY_Insp_Details_1 = "Insp_Details_1";
        private const String KEY_Insp_Details_2 = "Insp_Details_2";
        private const String KEY_Insp_Details_3 = "Insp_Details_3";
        private const String KEY_Insp_Details_4 = "Insp_Details_4";

        private const String KEY_Timestamp = "Timestamp";
        //private const String KEY_CreatedDate = "Created";

        private String[] COLUMNS = new String[] { KEY_Vessel_Name, KEY_Vessel_IMO, KEY_Type, KEY_Checklist_ID, KEY_Vessel_TYPE, KEY_Vessel_Extra1, KEY_Vessel_Extra2, KEY_Vessel_Extra3, KEY_Inspection_Date, KEY_Inspector_Name, KEY_Inspector_Company, KEY_Location_Port_Sea, KEY_Port, KEY_Country, KEY_Opening_Meeting_DatetimeFm, KEY_Opening_Meeting_DatetimeTo, KEY_Closing_Meeting_DatetimeFm, KEY_Closing_Meeting_DatetimeTo, KEY_Master_Name, KEY_Chief_Engineer_Name, KEY_Chief_Officer_Name, KEY_Second_Engr_Name,KEY_Comments, KEY_Status, KEY_Insp_Details_1, KEY_Insp_Details_2, KEY_Insp_Details_3, KEY_Insp_Details_4, KEY_Timestamp };//, KEY_CreatedDate };
                                                                                                                                                                                                                             
        public int LatestId;                                                     
        public string status;                                                     
        public string folderpath;
        public string answergroupmodel;
        public string ROVIQMasters;

        public string Vessel_Name;
        public string Vessel_IMO;

        public string Vessel_TYPE;
        public string Vessel_Extra1; // USED FOR InspectorQualifications For the Inspection
        public string Vessel_Extra2;
        public string Vessel_Extra3;

        public string Inspection_Date;
        public string Inspector_Name ;
        public string Inspector_Company ;
        public string Port ;
        public string Country;
        public string Opening_MeetingStart ;
        public string Opening_MeetingEnd ;
        public string Closing_MeetingStart;
        public string Closing_MeetingEnd ;
        public string Master_Name;
        public string Chief_Engineer_Name ;
        public string Chief_Officer_Name;
        public string Second_Engr_Name;
        public string SummaryComments;
        public string reportstatus;
        public string standardphotos;
        public string inspectiontype;
        public int ThisitemId;                                                                                                                                                                                               
        public String outputofsearchresult;

        public table_Inspection_PrimaryDetails() : base()
        {
           // KEY_CreatedDate = DATETIME DEFAULT CURRENT_TIMESTAMP";
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, "
                + KEY_Vessel_Name + ","
                + KEY_Vessel_IMO + ","
               
                + KEY_Type + ","
                + KEY_Checklist_ID + ","
                + KEY_Vessel_TYPE + ","
                + KEY_Vessel_Extra1 + ","
                + KEY_Vessel_Extra2 + ","
                + KEY_Vessel_Extra3 + ","
                + KEY_Inspection_Date + ","
                + KEY_Inspector_Name + ","
                + KEY_Inspector_Company + ","
                + KEY_Location_Port_Sea + ","
                + KEY_Port + ","
                + KEY_Country + ","
                + KEY_Opening_Meeting_DatetimeFm + ","
                + KEY_Opening_Meeting_DatetimeTo + ","
                + KEY_Closing_Meeting_DatetimeFm + ","
                + KEY_Closing_Meeting_DatetimeTo + ","
                + KEY_Master_Name + ","
                + KEY_Chief_Engineer_Name + ","
                + KEY_Chief_Officer_Name + ","
                + KEY_Second_Engr_Name + ","
                + KEY_Comments + ","
                + KEY_Status + ","
                + KEY_Insp_Details_1 + ","
                + KEY_Insp_Details_2 + ","
                + KEY_Insp_Details_3 + ","
                + KEY_Insp_Details_4 + ","
                + KEY_Timestamp +")";
            //+ "DATETIME DEFAULT CURRENT_TIMESTAMP  )";

            dbcmd.ExecuteNonQuery();
        }

        public void addData(Inspection_PrimaryDetailsEntity Inspection_PrimaryDetailsEntity)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( " + KEY_Vessel_Name + ","
                + KEY_Vessel_IMO + ","
                
                + KEY_Type + ","
                + KEY_Checklist_ID + ","
                + KEY_Vessel_TYPE + ","
                + KEY_Vessel_Extra1 + ","
                + KEY_Vessel_Extra2 + ","
                + KEY_Vessel_Extra3 + ","
                + KEY_Inspection_Date + ","
                + KEY_Inspector_Name + ","
                + KEY_Inspector_Company + ","
                + KEY_Location_Port_Sea + ","
                + KEY_Port + ","
                + KEY_Country + ","
                + KEY_Opening_Meeting_DatetimeFm + ","
                + KEY_Opening_Meeting_DatetimeTo + ","
                + KEY_Closing_Meeting_DatetimeFm + ","
                + KEY_Closing_Meeting_DatetimeTo + ","
                + KEY_Master_Name + ","
                + KEY_Chief_Engineer_Name + ","
                + KEY_Chief_Officer_Name + ","
                + KEY_Second_Engr_Name + ","
                + KEY_Comments + ","
                + KEY_Status + ","
                + KEY_Insp_Details_1 + ","
                + KEY_Insp_Details_2 + ","
                + KEY_Insp_Details_3 + ","
                + KEY_Insp_Details_4 + ")"

                + "VALUES (' "
                + Inspection_PrimaryDetailsEntity._Vessel_Name + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Vessel_IMO + " ' , ' "
                
                + Inspection_PrimaryDetailsEntity._Type + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Checklist_ID + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Vessel_TYPE + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Vessel_Extra1 + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Vessel_Extra2 + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Vessel_Extra3 + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Inspection_Date + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Inspector_Name + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Inspector_Company + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Location_Port_Sea + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Port + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Country + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Opening_Meeting_DateTimeFm + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Opening_Meeting_DateTimeTo + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Closing_Meeting_DateTimeFm + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Closing_Meeting_DateTimeTo + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Master_Name + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Chief_Engineer_Name + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Chief_Officer_Name + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Second_Engr_Name + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Comments + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Status + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Insp_Details_1 + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Insp_Details_2 + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Insp_Details_3 + " ' , ' "
                + Inspection_PrimaryDetailsEntity._Insp_Details_4 + " ')";

            dbcmd.ExecuteNonQuery();
        }
        public string getDataBypassedId(int id)
        {
            status = "";
            folderpath = "";
            answergroupmodel = "";
            ROVIQMasters = "";
            Vessel_Name = "";
            Vessel_IMO = "";
            Vessel_TYPE = "";
            Vessel_Extra1 = "";
            Vessel_Extra2 = "";
            Vessel_Extra3 = "";
            Inspection_Date = "";
            Inspector_Name = "";
            Inspector_Company = "";
            Port = "";
            Country = "";
            Opening_MeetingStart = "";
            Opening_MeetingEnd = "";
            Closing_MeetingStart = "";
            Closing_MeetingEnd = "";
            Master_Name = "";
            Chief_Engineer_Name = "";
            Chief_Officer_Name = "";
            SummaryComments = "";
            reportstatus = "";
            standardphotos = "";
            inspectiontype = "";
            IDbCommand dbcmd = getDbCommand();
                dbcmd.CommandText =
                    "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_ID + " as int) = " + id;
            
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
                using IDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                    int.Parse(reader[0].ToString()),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString().Trim(),
                    int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());
                
                    answergroupmodel = entity._Insp_Details_1;
                    folderpath = entity._Insp_Details_4;
                    status = entity._Status;
                    ROVIQMasters = entity._Insp_Details_2;
                standardphotos = entity._Insp_Details_3;
                Vessel_Name = entity._Vessel_Name;
                Vessel_IMO = entity._Vessel_IMO;

                Vessel_TYPE = entity._Vessel_TYPE;
                Vessel_Extra1 = entity._Vessel_Extra1;
                Vessel_Extra2 = entity._Vessel_Extra2;
                Vessel_Extra3 = entity._Vessel_Extra3;

                Inspection_Date = entity._Inspection_Date;
                Inspector_Name = entity._Inspector_Name;
                Inspector_Company = entity._Inspector_Company;
                Port = entity._Port;
                Country = entity._Country;
                Opening_MeetingStart = entity._Opening_Meeting_DateTimeFm;
                Opening_MeetingEnd = entity._Opening_Meeting_DateTimeTo;
                Closing_MeetingStart = entity._Closing_Meeting_DateTimeFm;
                Closing_MeetingEnd = entity._Closing_Meeting_DateTimeTo;
                Master_Name = entity._Master_Name;
                Chief_Engineer_Name = entity._Chief_Engineer_Name;
                Chief_Officer_Name = entity._Chief_Officer_Name;
                Second_Engr_Name = entity._Second_Engr_Name;
                SummaryComments = entity._Comments;
                reportstatus = entity._Status;
                inspectiontype = entity._Type.ToLower();


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
            return status;
            
        }

        public void Getinspectiontemplates()

        {

            IDbCommand dbcmd = getDbCommand();
            //dbcmd.CommandText =
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = 1 and " + KEY_Active + " = 1 " + " ORDER BY " + KEY_Template_Section_Ques;

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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
            //dbcmd.CommandText =
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = '" + templateinspid + "'" + KEY_Active + " = 1 " + " ORDER BY " + KEY_Inspection_Template_ID;

            //  "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ITEMNAME + " LIKE '%" + str.Replace("'", "''") + "%' or " + KEY_ITEMCODE + " ='" + str + "'";
            //return dbcmd.ExecuteReader();
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());


                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
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
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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

                List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
                using IDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                    int.Parse(reader[0].ToString()),
                    reader[1].ToString(),
                    reader[2].ToString(),
                    reader[3].ToString().Trim(),
                    int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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
            List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_PrimaryDetailsEntity entity = new Inspection_PrimaryDetailsEntity(
                int.Parse(reader[0].ToString()),
                reader[1].ToString(),
                reader[2].ToString(),
                reader[3].ToString().Trim(),
                int.Parse(reader[4].ToString()),
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
                    reader[28].ToString().Trim(),
                    reader[29].ToString().Trim());

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

       
        public IDataReader getdataforreader(string str, string str2, string str3)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Status + " LIKE '%" + str.Replace("'", "''") + "%'" + " OR " + KEY_Status + " LIKE '%" + str2.Replace("'", "''") + "%'" + " OR " + KEY_Status + " LIKE '%" + str3.Replace("'", "''") + "%'" + " ORDER BY " + KEY_ID + " Desc ";
            return dbcmd.ExecuteReader();
        }

        public IDataReader getdataforpublished(string str)
        {
            using IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Status + " LIKE '%" + str.Replace("'", "''") + "%'" + " ORDER BY " + KEY_ID + " Desc ";
            return dbcmd.ExecuteReader();
        }

        public IDataReader getPrimaryInfoData(int id)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_ID + " as int) = " + id;
            return dbcmd.ExecuteReader();
        }

            public void UpdateStatus(int inspection_primarydetails_ID)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_Status + " = 'D' WHERE " + KEY_ID + " = '" + inspection_primarydetails_ID + "'";

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

        public void UpdateStatusPublished(int inspection_primarydetails_ID)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_Status + " = 'P' WHERE " + KEY_ID + " = '" + inspection_primarydetails_ID + "'";

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

        public void ChangeStatusDraft(int inspection_primarydetails_ID)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_Status + " = 'D' WHERE " + KEY_ID + " = '" + inspection_primarydetails_ID + "'";

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

        public void ReportDeletedStatus(int inspection_primarydetails_ID)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_Status + " = 'Z' WHERE " + KEY_ID + " = '" + inspection_primarydetails_ID + "'";

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

        public void ReportReopened(int inspection_primarydetails_ID)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + KEY_Status + " = 'O' WHERE " + KEY_ID + " = '" + inspection_primarydetails_ID + "'";

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }

        public void CountInpspections(string status)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Status + " = " + status;

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

        }


        //CREATED DURING DEVELOPMENT FOR THE PDF REPORT TO Insert Sample Summary/Comment

        public void UpdateComment(string comment, int inspection_primarydetails_ID,string fieldtoupdate)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + fieldtoupdate + " = '"+ comment + "' WHERE " + KEY_ID + " = '" + inspection_primarydetails_ID + "'";

            //CLOSED Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();

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

        public IDataReader InspectionCreatedDate(string Query)    //FOR Querying Questions;
        {
            return base.InspectionCreatedDate(Query);
            
        }

        public IDataReader getDatabyQuery(string query)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = query;
            //dbcmd.CommandText = "SELECT Cloud_DB_ID,Template_Section_Ques,Obs_Details_1,ROVIQSequence FROM " + TABLE_NAME + " where Obs_Details_8='Question'";
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