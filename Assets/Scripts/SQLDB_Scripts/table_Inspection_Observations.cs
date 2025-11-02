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
    public class table_Inspection_Observations : SqliteHelper
    {
        private const String Tag = "Orion: table_Inspection_Observations:\t";
        private const String TABLE_NAME = "Inspection_Observations";
        private const String KEY_ID = "ID";
        private const String KEY_Inspection_PrimaryDetails_ID = "Inspection_PrimaryDetails_ID";
        private const String KEY_Cloud_DB_ID = "Cloud_DB_ID";
        private const String KEY_Cloud_DB_ParentID = "Cloud_DB_ParentID";
        private const String KEY_Version_Number = "Version_Number";
        private const String KEY_Version_Date = "Version_Date";
        private const String KEY_Template_Section_Ques = "Template_Section_Ques";
        private const String KEY_Selected_Answer = "Selected_Answer";
        private const String KEY_Date = "Date";
        private const String KEY_Time = "Time";
        private const String KEY_SomeDetail = "SomeDetail";
        private const String KEY_Observation_Text = "Observation_Text";
        private const String KEY_RiskCategory = "RiskCategory";
        private const String KEY_TargetDate = "TargetDate";
        private const String KEY_Recommendation = "Recommendation";
        private const String KEY_Obs_Details_1 = "Obs_Details_1";
        private const String KEY_Obs_Details_2 = "Obs_Details_2";
        private const String KEY_Obs_Details_3 = "Obs_Details_3";
        private const String KEY_Obs_Details_4 = "Obs_Details_4";
        private const String KEY_Obs_Details_5 = "Obs_Details_5";
        private const String KEY_Obs_Details_6 = "Obs_Details_6";
        private const String KEY_Obs_Details_7 = "Obs_Details_7";
        private const String KEY_Obs_Details_8 = "Obs_Details_8";

        //Below related specifically to the SIRE 2.0 (Year 2024)
        private const String KEY_ShortQuestionText_Sire = "ShortQuestionText";  // USE For CATEGORY - Human - SIRE 2.0
        private const String KEY_VesselTypes_Sire = "VesselTypes";
        private const String KEY_ROVIQSequence_Sire = "ROVIQSequence";          //Bears the Master Categories for grouping for ROVIQ option.
        private const String KEY_Publications_Sire = "Publications";            // USE For CATEGORY - Process
        private const String KEY_Objective_Sire = "Objective";                  //OBJECTIVE
        private const String KEY_IndustryGuidance = "IndustryGuidance";         // USE For CATEGORY - Hardware
        private const String KEY_TMSAKPI = "TMSAKPI";                           //TMSA KPI
        private const String KEY_IMOISMCode = "IMOISMCode";                     //SMS Ref
        private const String KEY_InspectionGuidance = "InspectionGuidance";     //Tagged Rank
        private const String KEY_SuggestedInspectorActions = "SuggestedInspectorActions"; // Question Type; C / R1 / R2
        private const String KEY_ExpectedEvidence = "ExpectedEvidence";                   //USE FOR MANDATORY PHOTOGRAPH
        private const String KEY_PotentialGroundsNegativeObservation = "PotentialGroundsObs";

        private const String KEY_Active = "Active";
        private const String KEY_Timestamp = "Timestamp";

        public List<string> ObservationsList;
        List<Inspection_ObservationsEntity> myList1;

        private String[] COLUMNS = new String[] { KEY_Inspection_PrimaryDetails_ID, KEY_Cloud_DB_ID, KEY_Cloud_DB_ParentID, KEY_Version_Number, KEY_Version_Date, KEY_Template_Section_Ques, KEY_Selected_Answer, KEY_Date,
            KEY_Time, KEY_SomeDetail, KEY_Observation_Text, KEY_RiskCategory, KEY_TargetDate, KEY_Recommendation, KEY_Obs_Details_1, KEY_Obs_Details_2, KEY_Obs_Details_3, KEY_Obs_Details_4, KEY_Obs_Details_5,
            KEY_Obs_Details_6, KEY_Obs_Details_7, KEY_Obs_Details_8,
            KEY_ShortQuestionText_Sire,KEY_VesselTypes_Sire,KEY_ROVIQSequence_Sire,KEY_Publications_Sire,KEY_Objective_Sire,KEY_IndustryGuidance,KEY_TMSAKPI,KEY_IMOISMCode,
KEY_InspectionGuidance,
KEY_SuggestedInspectorActions,
KEY_ExpectedEvidence,
KEY_PotentialGroundsNegativeObservation,
            KEY_Active, KEY_Timestamp};

        public int LatestId;
        public int ThisitemId;
        public String outputofsearchresult;
        public bool photoismandatory;

        public string HumanCategory;
        public string ProcessCategory;
        public string HardwareCategory;
        public string TMSACategory;
        public string ObjectiveCategory;
        public string ISMandComments;

        public table_Inspection_Observations() : base()
        { 
        IDbCommand dbcmd = getDbCommand();
        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, "
                + KEY_Inspection_PrimaryDetails_ID + ","
                + KEY_Cloud_DB_ID + ","
                + KEY_Cloud_DB_ParentID + ","
                + KEY_Version_Number + ","
                + KEY_Version_Date + ","
                + KEY_Template_Section_Ques + ","
                + KEY_Selected_Answer + ","
                + KEY_Date + ","
                + KEY_Time + ","
                + KEY_SomeDetail + ","
                + KEY_Observation_Text + ","
                + KEY_RiskCategory + ","
                + KEY_TargetDate + ","
                + KEY_Recommendation + ","
                + KEY_Obs_Details_1 + ","
                + KEY_Obs_Details_2 + ","
                + KEY_Obs_Details_3 + ","
                + KEY_Obs_Details_4 + ","
                + KEY_Obs_Details_5 + ","
                + KEY_Obs_Details_6 + ","
                + KEY_Obs_Details_7 + ","
                + KEY_Obs_Details_8 + ","

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
                + KEY_Timestamp + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();
        }

    public void addData(Inspection_ObservationsEntity Inspection_ObservationsEntity)
    {
        IDbCommand dbcmd = getDbCommand();
        dbcmd.CommandText =
            "INSERT INTO " + TABLE_NAME
            + " ( " + KEY_Inspection_PrimaryDetails_ID + ","
            + KEY_Cloud_DB_ID + ","
            + KEY_Cloud_DB_ParentID + ","
            + KEY_Version_Number + ","
            + KEY_Version_Date + ","
            + KEY_Template_Section_Ques + ","
            + KEY_Selected_Answer + ","
            + KEY_Date + ","
            + KEY_Time + ","
            + KEY_SomeDetail + ","
            + KEY_Observation_Text + ","
            + KEY_RiskCategory + ","
            + KEY_TargetDate + ","
            + KEY_Recommendation + ","
            + KEY_Obs_Details_1 + ","
            + KEY_Obs_Details_2 + ","
            + KEY_Obs_Details_3 + ","
            + KEY_Obs_Details_4 + ","
            + KEY_Obs_Details_5 + ","
            + KEY_Obs_Details_6 + ","
            + KEY_Obs_Details_7 + ","
            + KEY_Obs_Details_8 + ","
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
            + Inspection_ObservationsEntity._Inspection_PrimaryDetails_ID + " ' , ' "
            + Inspection_ObservationsEntity._Cloud_DB_ID + " ' , ' "
            + Inspection_ObservationsEntity._Cloud_DB_ParentID + " ' , ' "
            + Inspection_ObservationsEntity._Version_Number + " ' , ' "
            + Inspection_ObservationsEntity._Version_Date + " ' , ' "
            + Inspection_ObservationsEntity._Template_Section_Ques + " ' , ' "
            + Inspection_ObservationsEntity._Selected_Answer + " ' , ' "
            + Inspection_ObservationsEntity._Date + " ' , ' "
            + Inspection_ObservationsEntity._Time + " ' , ' "
            + Inspection_ObservationsEntity._SomeDetail + " ' , ' "
            + Inspection_ObservationsEntity._Observation_Text + " ' , ' "
            + Inspection_ObservationsEntity._RiskCategory + " ' , ' "
            + Inspection_ObservationsEntity._TargetDate + " ' , ' "
            + Inspection_ObservationsEntity._Recommendation + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_1 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_2 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_3 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_4 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_5 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_6 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_7 + " ' , ' "
            + Inspection_ObservationsEntity._Obs_Details_8 + " ' , ' "
            + Inspection_ObservationsEntity._ShortQuestionText_Sire + " ' , ' "
            + Inspection_ObservationsEntity._VesselTypes_Sire + " ' , ' " 
            + Inspection_ObservationsEntity._ROVIQSequence_Sire + " ' , ' " 
            + Inspection_ObservationsEntity._Publications_Sire + " ' , ' " 
            + Inspection_ObservationsEntity._Objective_Sire + " ' , ' " 
            + Inspection_ObservationsEntity._IndustryGuidance + " ' , ' " 
            + Inspection_ObservationsEntity._TMSAKPI + " ' , ' " 
            + Inspection_ObservationsEntity._IMOISMCode + " ' , ' " 
            + Inspection_ObservationsEntity._InspectionGuidance + " ' , ' " 
            + Inspection_ObservationsEntity._SuggestedInspectorActions + " ' , ' " 
            + Inspection_ObservationsEntity._ExpectedEvidence + " ' , ' " 
            + Inspection_ObservationsEntity._PotentialGroundsNegativeObservation + " ' , ' " 
            + Inspection_ObservationsEntity._Active + " ')";
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
        List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
        using IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
            reader[29].ToString().Trim(),
            reader[30].ToString().Trim(),
            reader[31].ToString().Trim(),
            reader[32].ToString().Trim(),
            reader[33].ToString().Trim(),
            reader[34].ToString().Trim(),
            reader[35].ToString().Trim(),
            reader[36].ToString().Trim());

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
            //"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_Inspection_Template_ID + " = '" + templateinspid + "'" + KEY_Active + " = 1 " + " ORDER BY " + KEY_Inspection_Template_ID;

            "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = " + templateinspid + " ORDER BY " + "cast(" + KEY_ID + " as int)" ;
            //return dbcmd.ExecuteReader();
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
                reader[29].ToString().Trim(),
                reader[30].ToString().Trim(),
                reader[31].ToString().Trim(),
                reader[32].ToString().Trim(),
                reader[33].ToString().Trim(),
                reader[34].ToString().Trim(),
                reader[35].ToString().Trim(),
                reader[36].ToString().Trim());


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

    public void GetDataObsAndInspID(int primaryid, int obsDBID)
        {

            HumanCategory = "";
            ProcessCategory = "";
            HardwareCategory = "";
            TMSACategory = "";
            ObjectiveCategory = "";
            ISMandComments = "";

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =

           "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = '" + primaryid + "' and " + "cast(" + KEY_Cloud_DB_ID + " as int) = '" + obsDBID +"'";

            
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
                reader[29].ToString().Trim(),
                reader[30].ToString().Trim(),
                reader[31].ToString().Trim(),
                reader[32].ToString().Trim(),
                reader[33].ToString().Trim(),
                reader[34].ToString().Trim(),
                reader[35].ToString().Trim(),
                reader[36].ToString().Trim());


                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;

                HumanCategory = entity._ShortQuestionText_Sire;
                ProcessCategory = entity._Publications_Sire;
                HardwareCategory = entity._IndustryGuidance;
                TMSACategory = entity._TMSAKPI;
                ObjectiveCategory = entity._Objective_Sire;
                ISMandComments = entity._IMOISMCode;
            }
            reader.Close();
            reader.Dispose();
        }

        public void GetDataObservations(int primaryid)
        {

            HumanCategory = "";
            ProcessCategory = "";
            HardwareCategory = "";
            TMSACategory = "";
            ObjectiveCategory = "";
            ISMandComments = "";
            ObservationsList = new List<string>();

        IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =

           "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = '" + primaryid + "' " ;


            myList1 = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
                reader[29].ToString().Trim(),
                reader[30].ToString().Trim(),
                reader[31].ToString().Trim(),
                reader[32].ToString().Trim(),
                reader[33].ToString().Trim(),
                reader[34].ToString().Trim(),
                reader[35].ToString().Trim(),
                reader[36].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList1.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
                ThisitemId = entity._ID;

                HumanCategory = entity._ShortQuestionText_Sire;
                ProcessCategory = entity._Publications_Sire;
                HardwareCategory = entity._IndustryGuidance;
                TMSACategory = entity._TMSAKPI;
                ObjectiveCategory = entity._Objective_Sire;
                ISMandComments = entity._IMOISMCode;

                ObservationsList.Add(output1);


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
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
                reader[29].ToString().Trim(),
                reader[30].ToString().Trim(),
                reader[31].ToString().Trim(),
                reader[32].ToString().Trim(),
                reader[33].ToString().Trim(),
                reader[34].ToString().Trim(),
                reader[35].ToString().Trim(),
                reader[36].ToString().Trim());

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

        photoismandatory = false;

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
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
                reader[29].ToString().Trim(),
                reader[30].ToString().Trim(),
                reader[31].ToString().Trim(),
                reader[32].ToString().Trim(),
                reader[33].ToString().Trim(),
                reader[34].ToString().Trim(),
                reader[35].ToString().Trim(),
                reader[36].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

            var output1 = JsonUtility.ToJson(entity, true);
            Debug.Log(output1);
            outputofsearchresult = output1.ToString();
            ThisitemId = entity._ID;

            if (entity._ExpectedEvidence.Trim().ToLower() == "mandatory")
                {
                    photoismandatory = true;
                }
            else
                {
                    photoismandatory = false;
                }
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
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
            reader[29].ToString().Trim(),
            reader[30].ToString().Trim(),
            reader[31].ToString().Trim(),
            reader[32].ToString().Trim(),
            reader[33].ToString().Trim(),
            reader[34].ToString().Trim(),
            reader[35].ToString().Trim(),
            reader[36].ToString().Trim());

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
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
            reader[29].ToString().Trim(),
            reader[30].ToString().Trim(),
            reader[31].ToString().Trim(),
            reader[32].ToString().Trim(),
            reader[33].ToString().Trim(),
            reader[34].ToString().Trim(),
            reader[35].ToString().Trim(),
            reader[36].ToString().Trim());

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

                List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
                using IDataReader reader = dbcmd.ExecuteReader();
                while (reader.Read())
                {
                    Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
            reader[29].ToString().Trim(),
            reader[30].ToString().Trim(),
            reader[31].ToString().Trim(),
            reader[32].ToString().Trim(),
            reader[33].ToString().Trim(),
            reader[34].ToString().Trim(),
            reader[35].ToString().Trim(),
            reader[36].ToString().Trim());

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
            List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();
            using IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                Inspection_ObservationsEntity entity = new Inspection_ObservationsEntity(
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
            reader[29].ToString().Trim(),
            reader[30].ToString().Trim(),
            reader[31].ToString().Trim(),
            reader[32].ToString().Trim(),
            reader[33].ToString().Trim(),
            reader[34].ToString().Trim(),
            reader[35].ToString().Trim(),
            reader[36].ToString().Trim());

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

        public IDataReader getChapterData(int passingInspPrimaryID)   //FOR Querying Chapters;
        {
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = " + passingInspPrimaryID + " and " + KEY_Obs_Details_8 + " like '%Chapter%'" + " ORDER BY " + "cast( " + KEY_Template_Section_Ques.ToString().Split(' ')[0] + " as int)";

            return dbcmd.ExecuteReader();
        }

        public IDataReader getROVIQData(int passingInspPrimaryID)   //FOR Querying ROVIQMOde;
        {
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = " + passingInspPrimaryID + " and " + KEY_Obs_Details_8 + " like '%Question%'" + " ORDER BY " + "cast( " + KEY_Template_Section_Ques.ToString().Split(' ')[0] + " as int)";

            return dbcmd.ExecuteReader();
        }

        public IDataReader getquestionsData(int passingchapterID, int inspectionprimaryid)    //FOR Querying Questions;
        {
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Cloud_DB_ParentID + " as int) = " + passingchapterID + " and " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = " + inspectionprimaryid + " and " + KEY_Obs_Details_8 + " like '%Question%'" + " ORDER BY " +
    "CAST(SUBSTR(Template_Section_Ques || '.', 1,INSTR(Template_Section_Ques || '.', '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1, INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') + 1) AS INTEGER) ASC;";

            //dbcmd.CommandText = "SELECT * FROM " + TABLE_NAME + " WHERE " + "cast(" + KEY_Inspection_PrimaryDetails_ID + " as int) = " + passingInspPrimaryID + " and " + KEY_Obs_Details_8 + " like '%Question%'" + " ORDER BY " + "cast(" + KEY_Template_Section_Ques.Trim() + " as float)";

            return dbcmd.ExecuteReader();
        }

        public IDataReader pendingquestionsData(string Query)    //FOR Querying Questions;
        {
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "select  * FROM " + Query + " ORDER BY " +
    "CAST(SUBSTR(Template_Section_Ques || '.', 1,INSTR(Template_Section_Ques || '.', '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1, INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') + 1) AS INTEGER) ASC;";
            
            return dbcmd.ExecuteReader();
        }

        public IDataReader SmartSearchQuestions(string Query)    //FOR Querying Questions;
        {
            IDbCommand dbcmd = getDbCommand();

            dbcmd.CommandText = "select  * FROM " + Query + " ORDER BY " +
    "CAST(SUBSTR(Template_Section_Ques || '.', 1,INSTR(Template_Section_Ques || '.', '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1, INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') + 1) AS INTEGER) ASC;";

            return dbcmd.ExecuteReader();
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

    public void deleteDataByquery(string columnquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE FROM " + TABLE_NAME + " WHERE " + columnquery;
            dbcmd.ExecuteNonQuery();

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

        public IDataReader getDatabyQuery(string query)
        {
            using IDbCommand dbcmd = getDbCommand();
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