using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;
//using Unity.XR.CoreUtils;
using UnityEngine.Pool;
using DataBank;


public class CreatingObsTableForInsp : MonoBehaviour
{
    public TextMeshProUGUI InspType;
    public TextMeshProUGUI ChklstID;
    public int Inspection_PrimaryDetails_ID;

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    public string outputofsearchresult;
    public string searchresults;
    public int datacounttemplate;
    public int datacountObsTable;

    //This onwards for creating the observations table after the Primary info has been completed and the START INSP Btn is clicked.
    public int ID;
    public int inspection_primarydetails_ID;
    public int Cloud_DB_ID;
    public string Cloud_DB_ParentID;
    public string Version_Number;
    public string Version_Date;
    public string Template_Section_Ques;
    public string Selected_Answer;
    public string Date;
    public string Time;
    public string SomeDetail;
    public string Observation_Text;
    public string RiskCategory;
    public string TargetDate;
    public string Recommendation;
    public string Obs_Details_1;
    public string Obs_Details_2;
    public string Obs_Details_3;
    public string Obs_Details_4;
    public string Obs_Details_5;
    public string Obs_Details_6;
    public string Obs_Details_7;
    public string Obs_Details_8;

    //Below related specifically to the SIRE 2.0 (Year 2024)
    public string ShortQuestionText_Sire;
    public string VesselTypes_Sire;
    public string ROVIQSequence_Sire;
    public string Publications_Sire;
    public string Objective_Sire;
    public string IndustryGuidance;
    public string TMSAKPI;
    public string IMOISMCode;
    public string InspectionGuidance;
    public string SuggestedInspectorActions;
    public string ExpectedEvidence;
    public string PotentialGroundsNegativeObservation;

    public string Active;
    public string Timestamp;

    public RectTransform ParentPanel;
    public RectTransform QuestionsParentPanel;

    public GameObject chaptersitemprefab; //Used for Chapters here.
    public GameObject chaptersaccordion;

    public GameObject ParentPanel1;
    public GameObject completionstatusprefab; //Used for Chapters here.
    public GameObject completionstatusaccordion;


    public CanvasPositionsMgr canvaspositions;
    public List<int> chapterIDs = new List<int>();
    public string ChapterIds = "";

    public TextMeshProUGUI InspStatus;
    int Totalquestions = 0;
    int Completedques = 0;

    int TotalquesinChap = 0;
    int completionstatus = 0;

    bool LoadROVIQ;
    string HeadertextforROVIQ;
    public GameObject CompletionStatusforROVIQ;
    int AnsweredQuesforROVIQ;
    Button pendingquestionsforroviq;
    GameObject ChapterPrefabtodestroyforROVIQ;
    List<string> standardphotos;
    string vesseltype = "";
    public GameObject InspectionStatus;
    int TotalquestionsMainHd = 0;
    int CompletedquesMainHd = 0;
    public TMP_InputField smartsearchInput;


    public void Start()
    {
        LoadROVIQ = false;
        InspectionStatus = GameObject.FindGameObjectWithTag("inspectionstatus");
    }

    public void GetChecklistContentNEW()

    //TWO JOBS FOR THIS FUNCTION;
    //1. Pulling the Checklist Chapters from the Template table and 
    //2. Saving into the Inspection_ObservationsEntity table and change the STATUS ON THIS AS DRAFT.

    {

        TotalquestionsMainHd = 0;
        CompletedquesMainHd = 0;

        table_Inspection_template mlocationDb1 = new table_Inspection_template();
        using var connection = mlocationDb1.getConnection();
        chapterIDs.Clear();
        //mlocationDb1.getDataByString1("fetchall");
        //Updatedata(string columndataquery)
        using System.Data.IDataReader reader = mlocationDb1.getChapterData(int.Parse(ChklstID.text.ToString()));
        //System.Data.IDataReader reader = mlocationDb.getdataforreader("Chapter");

        List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();

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

            myList.Add(entity);
            chapterIDs.Add(int.Parse(reader[2].ToString()));
            ChapterIds = string.Join(",", chapterIDs);
            //foreach (int chapterId in chapterIDs) //For printing List of ChapterIds in Debug...
            //{
            //    Debug.Log(chapterId);
            //}

            var output1 = JsonUtility.ToJson(entity, true);
            // Debug.Log("WHEN START INSPECTION CALLED FOR A NEW INSPECTION ->"+output1);
            outputofsearchresult = output1.ToString();
        }
        reader.Dispose();
        mlocationDb1.close();
        
        connection.Close();
        searchresults = JsonUtility.ToJson(myList, true);

        datacounttemplate = myList.Count;
        Debug.Log(InspType.text.ToString() + "-" + ChklstID.text.ToString() + "-" + Inspection_PrimaryDetails_ID.ToString() + "-" + datacounttemplate.ToString());

        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection1 = mlocationDb2.getConnection();
        foreach (var x in myList) //PICKED 16 (except ID) VALUES FROM Template table and inserting as new inspection into observation table with TEMPLATE ID FROM THE INSP TEMPLATE TABLE.
        {
            //ID;
            //inspection_primarydetails_ID = x._Inspection_Template_ID;
            Cloud_DB_ID = x._Cloud_DB_ID;
            Cloud_DB_ParentID = x._Cloud_DB_ParentID.ToString();
            Version_Number = x._Version_Number.ToString();
            Version_Date = x._Version_Date.ToString();
            Template_Section_Ques = x._Template_Section_Ques.ToString();
            Selected_Answer = "";
            Date = "";
            Time = "";
            SomeDetail = "";
            Observation_Text = "";
            RiskCategory = x._Details_3.ToString();
            TargetDate = "";
            Recommendation = "";

            Obs_Details_1 = x._Details_1;
            Obs_Details_2 = x._Details_2; //DETAILS_2 bear the inspection TYPE; like SIRE, CDI, etc.
            Obs_Details_3 = ""; // Will be blank as this field is essentially the RISK CATEGORY, WHICH is already catered to above, in the templates table "Details_3" is the risk.
            Obs_Details_4 = x._Details_4;
            Obs_Details_5 = x._Details_5;
            Obs_Details_6 = x._Details_6;
            Obs_Details_7 = x._Details_7;
            Obs_Details_8 = x._Details_8;

            ShortQuestionText_Sire = x._ShortQuestionText_Sire;
            VesselTypes_Sire = x._VesselTypes_Sire;
            ROVIQSequence_Sire = x._ROVIQSequence_Sire;
            Publications_Sire = x._Publications_Sire;
            Objective_Sire = x._Objective_Sire;
            IndustryGuidance = x._IndustryGuidance;
            TMSAKPI = x._TMSAKPI;
            IMOISMCode = x._IMOISMCode;
            InspectionGuidance = x._InspectionGuidance;
            SuggestedInspectorActions = x._SuggestedInspectorActions;
            ExpectedEvidence = x._ExpectedEvidence;
            PotentialGroundsNegativeObservation = x._PotentialGroundsNegativeObservation;

            Active = "Y";
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            mlocationDb2.addData(new Inspection_ObservationsEntity(ID, inspection_primarydetails_ID, Cloud_DB_ID, Cloud_DB_ParentID, Version_Number,
                Version_Date, Template_Section_Ques,
                Selected_Answer, Date, Time, SomeDetail, Observation_Text, RiskCategory, TargetDate, Recommendation, Obs_Details_1,
                Obs_Details_2,
                Obs_Details_3,
                Obs_Details_4,
                Obs_Details_5,
                Obs_Details_6,
                Obs_Details_7,
                Obs_Details_8, ShortQuestionText_Sire, VesselTypes_Sire, ROVIQSequence_Sire, Publications_Sire, Objective_Sire, IndustryGuidance, TMSAKPI, IMOISMCode,
                InspectionGuidance, SuggestedInspectorActions, ExpectedEvidence, PotentialGroundsNegativeObservation, Active, Timestamp));

            //LOAD PREFABS FOR THE QUESTIONS AND OBSERVATION BODY...                                   
        }
        //mlocationDb2.getDataByString1("fetchall");

        //Once inspections data is pulled from template and inserted into the Observations table for using as a checklist for this inspection, 
        //change the Status of the PrimaryDetails into Draft mode.

        TotalquestionsMainHd = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(Inspection_PrimaryDetails_ID.ToString()));

        CompletedquesMainHd = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_7) = 'Completed' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(Inspection_PrimaryDetails_ID.ToString()));
        InspectionStatus.GetComponent<TextMeshProUGUI>().text = CompletedquesMainHd.ToString() + " / " + TotalquestionsMainHd.ToString() + " Completed.";
       
        mlocationDb2.close();
        connection1.Close();
        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection2 = mlocationDb.getConnection();

        if (InspType.text.ToString().Contains("Sample"))

        {
            mlocationDb.ReportReopened(inspection_primarydetails_ID);  //change the Status of the PrimaryDetails into "O", Reworking mode. This is done in the case of TEST inspection, so that there is no Credit Removed..
        }
        else
        {
            mlocationDb.UpdateStatus(inspection_primarydetails_ID); //change the Status of the PrimaryDetails into "D", draft mode.
        }

       
        //mlocationDb.getDataByString1("fetchall");
        mlocationDb.getDataBypassedId(inspection_primarydetails_ID);
        vesseltype = mlocationDb.Vessel_TYPE.ToLower().Trim().Replace(" ", "");
        mlocationDb.close();

        connection2.Close();
        GetQuestionsforChapters();
       
        //Loadchapters();

    }


    public void GetQuestionsforChapters()
    {
        
        List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
        bool fetchcomplete = false;

        //1. Fetch Questions for Chapters from Template

        try
        {
            var mlocationDb1 = new table_Inspection_template();
            using (var connection3 = mlocationDb1.getConnection())
            using (var reader = mlocationDb1.getquestionsDataForChapters(ChapterIds, vesseltype))
            {
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

                    myList.Add(entity);
                }
            }

            searchresults = JsonUtility.ToJson(myList, true);
            datacounttemplate = myList.Count;
            Debug.Log($"Data Count Template: {datacounttemplate}");
            fetchcomplete = true;
            
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching questions: {ex.Message}");
            return;
        }
        
        // 3. Insert Questions into Observations Table
        try
        {
            if (fetchcomplete == true)
            {
                table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
                using var connection4 = mlocationDb2.getConnection();
                foreach (var x in myList) //PICKED 16 (except ID) VALUES FROM Template table and inserting as new inspection into observation table with TEMPLATE ID FROM THE INSP TEMPLATE TABLE.
                {
                    //ID;
                    //inspection_primarydetails_ID = x._Inspection_Template_ID;
                    Cloud_DB_ID = x._Cloud_DB_ID;
                    Cloud_DB_ParentID = x._Cloud_DB_ParentID.ToString();
                    Version_Number = x._Version_Number.ToString();
                    Version_Date = x._Version_Date.ToString();
                    Template_Section_Ques = x._Template_Section_Ques.ToString();
                    Selected_Answer = "";
                    Date = "";
                    Time = "";
                    SomeDetail = "";
                    Observation_Text = "";
                    RiskCategory = x._Details_3.ToString();
                    TargetDate = "";
                    Recommendation = "";

                    Obs_Details_1 = x._Details_1;
                    Obs_Details_2 = x._Details_2; //DETAILS_2 bear the inspection TYPE; like SIRE, CDI, etc.
                    Obs_Details_3 = ""; // Will be blank as this field is essentially the RISK CATEGORY, WHICH is already catered to above, in the templates table "Details_3" is the risk.
                    Obs_Details_4 = x._Details_4;
                    Obs_Details_5 = x._Details_5;
                    Obs_Details_6 = x._Details_6;
                    Obs_Details_7 = x._Details_7;
                    Obs_Details_8 = x._Details_8;

                    ShortQuestionText_Sire = x._ShortQuestionText_Sire;
                    VesselTypes_Sire = x._VesselTypes_Sire;
                    ROVIQSequence_Sire = x._ROVIQSequence_Sire;
                    Publications_Sire = x._Publications_Sire;
                    Objective_Sire = x._Objective_Sire;
                    IndustryGuidance = x._IndustryGuidance;
                    TMSAKPI = x._TMSAKPI;
                    IMOISMCode = x._IMOISMCode;
                    InspectionGuidance = x._InspectionGuidance;
                    SuggestedInspectorActions = x._SuggestedInspectorActions;
                    ExpectedEvidence = x._ExpectedEvidence;
                    PotentialGroundsNegativeObservation = x._PotentialGroundsNegativeObservation;

                    Active = "Y";
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");

                    mlocationDb2.addData(new Inspection_ObservationsEntity(ID, inspection_primarydetails_ID, Cloud_DB_ID, Cloud_DB_ParentID, Version_Number,
                        Version_Date, Template_Section_Ques,
                        Selected_Answer, Date, Time, SomeDetail, Observation_Text, RiskCategory, TargetDate, Recommendation, Obs_Details_1,
                        Obs_Details_2,
                        Obs_Details_3,
                        Obs_Details_4,
                        Obs_Details_5,
                        Obs_Details_6,
                        Obs_Details_7,
                        Obs_Details_8, ShortQuestionText_Sire, VesselTypes_Sire, ROVIQSequence_Sire, Publications_Sire, Objective_Sire, IndustryGuidance, TMSAKPI, IMOISMCode,
                        InspectionGuidance, SuggestedInspectorActions, ExpectedEvidence, PotentialGroundsNegativeObservation, Active, Timestamp));

                    connection4.Close();
                    //LOAD PREFABS FOR THE QUESTIONS AND OBSERVATION BODY...                                   
                }
            }

            else
            {
                Debug.LogError($"Waiting to fetch data from templates");
            }
            
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error inserting observations: {ex.Message}");
        }

        // 4. Update Inspection Primary Details Status to Draft
        try
        {
            var mlocationDb = new table_Inspection_PrimaryDetails();
            using (var connection5 = mlocationDb.getConnection())
            {

                if (InspType.text.ToString().Contains("Sample"))

                {
                    mlocationDb.ReportReopened(inspection_primarydetails_ID);  //change the Status of the PrimaryDetails into "O", Reworking mode. This is done in the case of TEST inspection, so that there is no Credit Removed..
                }
                else
                {
                    mlocationDb.UpdateStatus(inspection_primarydetails_ID); //change the Status of the PrimaryDetails into "D", draft mode.
                }

                //mlocationDb.UpdateStatus(inspection_primarydetails_ID);
            }
            
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error updating status: {ex.Message}");
        }
        
        // 5. Load Chapters
        Loadchapters();
    }


    //public void GetQuestionsforChapters_Before_Otimizing_ABOVE() //Pulling the Checklist Chapters - Questions from the Template table and This also takes into account the appliable questions for the selected vessel type.

    //{
    //    string vesseltype = "";
    //    table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
    //    using var connection2 = mlocationDb.getConnection();
    //    mlocationDb.getDataBypassedId(inspection_primarydetails_ID);

    //    vesseltype = mlocationDb.Vessel_TYPE.ToLower().Trim().Replace(" ", ""); //To Fetch the questions as per the vessel type including those where vesseltype is blank, i.e. applicable to all.

    //    table_Inspection_template mlocationDb1 = new table_Inspection_template();
    //    using var connection3 = mlocationDb1.getConnection();
    //    //mlocationDb1.getDataByString1("fetchall");

    //    using System.Data.IDataReader reader = mlocationDb1.getquestionsDataForChapters(ChapterIds, vesseltype);

    //    List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();

    //    while (reader.Read())
    //    {
    //        InspectionTemplateEntity entity = new InspectionTemplateEntity(
    //            int.Parse(reader[0].ToString()),
    //            int.Parse(reader[1].ToString()),
    //            int.Parse(reader[2].ToString()),
    //            reader[3].ToString().Trim(),
    //            reader[4].ToString().Trim(),
    //            reader[5].ToString().Trim(),
    //            reader[6].ToString().Trim(),
    //            reader[7].ToString().Trim(),
    //            reader[8].ToString().Trim(),
    //            reader[9].ToString().Trim(),
    //            reader[10].ToString().Trim(),
    //            reader[11].ToString().Trim(),
    //            reader[12].ToString().Trim(),
    //            reader[13].ToString().Trim(),
    //            reader[14].ToString().Trim(),
    //            reader[15].ToString().Trim(),
    //            reader[16].ToString().Trim(),
    //            reader[17].ToString().Trim(),
    //            reader[18].ToString().Trim(),
    //            reader[19].ToString().Trim(),
    //            reader[20].ToString().Trim(),
    //            reader[21].ToString().Trim(),
    //            reader[22].ToString().Trim(),
    //            reader[23].ToString().Trim(),
    //            reader[24].ToString().Trim(),
    //            reader[25].ToString().Trim(),
    //            reader[26].ToString().Trim(),
    //            reader[27].ToString().Trim(),
    //            reader[28].ToString().Trim());

    //        myList.Add(entity);

    //        var output1 = JsonUtility.ToJson(entity, true);
    //        Debug.Log("QUestions in this template ->" + output1);
    //        outputofsearchresult = output1.ToString();
    //    }
    //    reader.Dispose();
    //    searchresults = JsonUtility.ToJson(myList, true);

    //    datacounttemplate = myList.Count;
    //    Debug.Log(InspType.text.ToString() + "-" + ChklstID.text.ToString() + "-" + Inspection_PrimaryDetails_ID.ToString() + "-" + datacounttemplate.ToString());

    //    table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
    //    using var connection4 = mlocationDb2.getConnection();
    //    foreach (var x in myList) //PICKED 16 (except ID) VALUES FROM Template table and inserting as new inspection into observation table with TEMPLATE ID FROM THE INSP TEMPLATE TABLE.
    //    {
    //        //ID;
    //        //inspection_primarydetails_ID = x._Inspection_Template_ID;
    //        Cloud_DB_ID = x._Cloud_DB_ID;
    //        Cloud_DB_ParentID = x._Cloud_DB_ParentID.ToString();
    //        Version_Number = x._Version_Number.ToString();
    //        Version_Date = x._Version_Date.ToString();
    //        Template_Section_Ques = x._Template_Section_Ques.ToString();
    //        Selected_Answer = "";
    //        Date = "";
    //        Time = "";
    //        SomeDetail = "";
    //        Observation_Text = "";
    //        RiskCategory = x._Details_3.ToString();
    //        TargetDate = "";
    //        Recommendation = "";

    //        Obs_Details_1 = x._Details_1;
    //        Obs_Details_2 = x._Details_2; //DETAILS_2 bear the inspection TYPE; like SIRE, CDI, etc.
    //        Obs_Details_3 = ""; // Will be blank as this field is essentially the RISK CATEGORY, WHICH is already catered to above, in the templates table "Details_3" is the risk.
    //        Obs_Details_4 = x._Details_4;
    //        Obs_Details_5 = x._Details_5;
    //        Obs_Details_6 = x._Details_6;
    //        Obs_Details_7 = x._Details_7;
    //        Obs_Details_8 = x._Details_8;

    //        ShortQuestionText_Sire = x._ShortQuestionText_Sire;
    //        VesselTypes_Sire = x._VesselTypes_Sire;
    //        ROVIQSequence_Sire = x._ROVIQSequence_Sire;
    //        Publications_Sire = x._Publications_Sire;
    //        Objective_Sire = x._Objective_Sire;
    //        IndustryGuidance = x._IndustryGuidance;
    //        TMSAKPI = x._TMSAKPI;
    //        IMOISMCode = x._IMOISMCode;
    //        InspectionGuidance = x._InspectionGuidance;
    //        SuggestedInspectorActions = x._SuggestedInspectorActions;
    //        ExpectedEvidence = x._ExpectedEvidence;
    //        PotentialGroundsNegativeObservation = x._PotentialGroundsNegativeObservation;

    //        Active = "Y";
    //        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");

    //        mlocationDb2.addData(new Inspection_ObservationsEntity(ID, inspection_primarydetails_ID, Cloud_DB_ID, Cloud_DB_ParentID, Version_Number,
    //            Version_Date, Template_Section_Ques,
    //            Selected_Answer, Date, Time, SomeDetail, Observation_Text, RiskCategory, TargetDate, Recommendation, Obs_Details_1,
    //            Obs_Details_2,
    //            Obs_Details_3,
    //            Obs_Details_4,
    //            Obs_Details_5,
    //            Obs_Details_6,
    //            Obs_Details_7,
    //            Obs_Details_8, ShortQuestionText_Sire, VesselTypes_Sire, ROVIQSequence_Sire, Publications_Sire, Objective_Sire, IndustryGuidance, TMSAKPI, IMOISMCode,
    //            InspectionGuidance, SuggestedInspectorActions, ExpectedEvidence, PotentialGroundsNegativeObservation, Active, Timestamp));

    //        //LOAD PREFABS FOR THE QUESTIONS AND OBSERVATION BODY...                                   
    //    }
    //    //mlocationDb2.getDataByString1("fetchall");

    //    //Once inspections data is pulled from template and inserted into the Observations table for using as a checklist for this inspection, 
    //    //change the Status of the PrimaryDetails into Draft mode.

    //    //table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();

    //    mlocationDb.UpdateStatus(inspection_primarydetails_ID); //change the Status of the PrimaryDetails into "D", draft mode.
    //    mlocationDb.getDataByString1("fetchall");
    //    //mlocationDb.close();
    //    //mlocationDb1.close();
    //    //mlocationDb2.close();
    //    Loadchapters();


    //    //if (LoadROVIQ == false)
    //    //{
    //    //    Loadchapters();

    //    //}

    //    //else if (LoadROVIQ == true)
    //    //{
    //    //    LoadROVIQMthd();

    //    //}
    //}

    public void DestroyAnyActiveChecklist()
    {
        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }
    }

    public void Loadchapters() //Used for Chapters here, on LoadInspBlock.cs this is called Loadinspections().
    {

        InspStatus.text = "";
        Totalquestions = 0;
        Completedques = 0;
        TotalquestionsMainHd = 0;
        CompletedquesMainHd = 0;

        TotalquesinChap = 0;
        completionstatus = 0;
        LoadROVIQ = false;
        
        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }

        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection = mlocationDb2.getConnection();
        using System.Data.IDataReader reader = mlocationDb2.getChapterData(inspection_primarydetails_ID);

        Totalquestions = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));
        Completedques = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) ='Completed' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));

        //Completedques = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));
        InspStatus.text = Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";
        //System.Data.IDataReader reader = mlocationDb.getdataforreader("Chapter");

        TotalquestionsMainHd = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(Inspection_PrimaryDetails_ID.ToString()));

        CompletedquesMainHd = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_7) = 'Completed' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(Inspection_PrimaryDetails_ID.ToString()));
        InspectionStatus.GetComponent<TextMeshProUGUI>().text = CompletedquesMainHd.ToString() + " / " + TotalquestionsMainHd.ToString() + " Completed.";

        List<Inspection_ObservationsEntity> myList = new List<Inspection_ObservationsEntity>();

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
        
        searchresults = JsonUtility.ToJson(myList, true);

        datacountObsTable = myList.Count;


        foreach (var x in myList)
        {
            completionstatus = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) ='Completed' " +
                "and  cast(Cloud_DB_ParentID as int) = " + int.Parse(x._Cloud_DB_ID.ToString()) +
                " and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));

            TotalquesinChap = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question'" +
                " and  cast(Cloud_DB_ParentID as int) = " + int.Parse(x._Cloud_DB_ID.ToString()) +
                " and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));

            if (TotalquesinChap != 0)

            {
                chaptersaccordion = Instantiate(chaptersitemprefab);
                chapterprefabitems Chapter = chaptersaccordion.GetComponent<chapterprefabitems>();
                chaptersaccordion.transform.SetParent(ParentPanel, false);
                
                if (Chapter.GetComponent<loadquestionfmObsTable>() != null)
                {
                    Chapter.GetComponent<loadquestionfmObsTable>().ParentPanel_1 = QuestionsParentPanel;
                }

                //completionstatus = "CREATE COMPLETION STATUS FOR HERE";

                Chapter.Header_Text.text = x._Template_Section_Ques.ToString() + ": " + x._Obs_Details_1.ToString();

                Chapter.InspPrimaryID.text = inspection_primarydetails_ID.ToString();
                Chapter.ChapterDBID.text = x._Cloud_DB_ID.ToString();
                Chapter.CloudDBParentID.text = x._Cloud_DB_ParentID.ToString();
                Chapter.ROVIQLOADTrueOrFalse.text = LoadROVIQ.ToString();

                Chapter.Completionstatus.text = "Answered; " + completionstatus.ToString() + "/" + TotalquesinChap.ToString();
                Chapter.RightclickArrow.GetComponent<Button>().onClick.AddListener(Chapter.GetComponent<loadquestionfmObsTable>().LoadquestionsBtnClicked);

                if (completionstatus == TotalquesinChap)
                {
                    Chapter.pendingquestions.GetComponent<Image>().color = Color.green;
                    Chapter.pendingquestions.GetComponent<Button>().interactable = false;
                }

                else
                {
                    Chapter.pendingquestions.GetComponent<Image>().color = Color.yellow;
                    Chapter.pendingquestions.GetComponent<Button>().interactable = true;
                }

            }

            else
            {
                Debug.Log(x._Template_Section_Ques.ToString() + ": " + x._Obs_Details_1.ToString() + " has no questions attached..");
            }
            //Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";
        }


        //table_Inspection_Observations mlocationDb = new table_Inspection_Observations();

        connection.Close();
        CheckStandardPhotos();
        canvaspositions.ProceedtoChecklistBtn();
        
        //Template_Section_Ques = Convert.ToString(Template_Section_Ques);
        // GetStocksData(fromdbList);
    }

    public void LoadROVIQMthd() //This was sorted out; NOT WORKING SOME ISSUE FETCHING ROVIQ LIST CORRECTYLY BECOZ OF Inspection_PrimaryDetails table some value getting updated; 09th March 2024
    {

        InspStatus.text = "";
        string ROVIQMasters = "";
        Totalquestions = 0;
        Completedques = 0;
        TotalquesinChap = 0;
        completionstatus = 0;
        LoadROVIQ = true;

        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }

        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection = mlocationDb2.getConnection();
        using System.Data.IDataReader reader = mlocationDb2.getROVIQData(inspection_primarydetails_ID);
        
        Totalquestions = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));
        Completedques = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) ='Completed' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));

        //Completedques = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));

        InspStatus.text = Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";
        //System.Data.IDataReader reader = mlocationDb.getdataforreader("Chapter");

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(inspection_primarydetails_ID.ToString()));

        ROVIQMasters = mlocationdb1.ROVIQMasters.ToString();

        List<string> keys = ROVIQMasters.ToString().Split(',').Select(x => x.Trim().ToLower()).ToList();

        //if ((keys != null) && (!keys.Any()))
        //{

            foreach (string key in keys)
            {
                chaptersaccordion = Instantiate(chaptersitemprefab);
                chapterprefabitems Chapter = chaptersaccordion.GetComponent<chapterprefabitems>();
                chaptersaccordion.transform.SetParent(ParentPanel, false);

                Chapter.Header_Text.text = key.ToString();
                HeadertextforROVIQ = key.ToString();
                Chapter.InspPrimaryID.text = inspection_primarydetails_ID.ToString();
                Chapter.ROVIQLOADTrueOrFalse.text = LoadROVIQ.ToString();
                CompletionStatusforROVIQ = Chapter.Completionstatus.gameObject;
                pendingquestionsforroviq = Chapter.pendingquestions;
                ChapterPrefabtodestroyforROVIQ = Chapter.gameObject;
                Chapter.RightclickArrow.GetComponent<Button>().onClick.AddListener(Chapter.GetComponent<loadquestionfmObsTable>().LoadquestionsBtnClicked);

            FetchROVIQ();
            }
        connection.Close();
        connection1.Close();
        CheckStandardPhotos();
        //mlocationdb1.close();
        //mlocationDb2.close();
    }

    //SHIFTED BELOW TO LoadquestionsfmObsTable.cs where it actually belongs for loading questions, here it is only used for gathering total questions inside and how many completed.

    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> LoadDataByROVIQSequence() //Used for Getting ROVIQ.
    {
        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection1 = mlocationDb2.getConnection();
        string columndataquery = "SELECT Cloud_DB_ID,Template_Section_Ques,Obs_Details_1,ROVIQSequence,Selected_Answer,Obs_Details_7 FROM Inspection_Observations where TRIM(Obs_Details_8) ='Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString());
        using System.Data.IDataReader reader = mlocationDb2.getDatabyQuery(columndataquery);
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> myList = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        while (reader.Read())
        {
            List<string> keys = reader[3].ToString().Split(',').Select(x => x.Trim().ToLower()).ToList();
            foreach (string key in keys)
            {
                if (myList.Keys.Contains(key))
                {
                    Dictionary<string, Dictionary<string, string>> questions = myList[key];
                    if (!questions.Keys.Contains(reader[0].ToString().Trim()))
                    {
                        Dictionary<string, string> props = new Dictionary<string, string>();
                        props.Add("Selected_Answer", reader[4].ToString().Trim());
                        props.Add("Obs_Details_7", reader[5].ToString().Trim());
                        //props.Add("Cloud_DB_ID", reader[0].ToString().Trim());
                        //props.Add("Template_Section_Ques", reader[1].ToString().Trim());
                        //props.Add("Obs_Details_1", reader[2].ToString().Trim());
                        questions.Add(reader[0].ToString().Trim(), props);
                    }
                }
                else
                {
                    Dictionary<string, string> props = new Dictionary<string, string>();
                    props.Add("Selected_Answer", reader[4].ToString().Trim());
                    props.Add("Obs_Details_7", reader[5].ToString().Trim());
                    //props.Add("Cloud_DB_ID", reader[0].ToString().Trim());
                    //props.Add("Template_Section_Ques", reader[1].ToString().Trim());
                    //props.Add("Obs_Details_1", reader[2].ToString().Trim());

                    Dictionary<string, Dictionary<string, string>> questions = new Dictionary<string, Dictionary<string, string>>();
                    questions.Add(reader[0].ToString().Trim(), props);
                    myList.Add(key, questions);
                }
            }
        }
        reader.Dispose();
        connection1.Close();
        //mlocationDb2.close();
        return myList;
        
    }

    public void FetchROVIQ()
    {

        AnsweredQuesforROVIQ = 0;
        // To Get all ROVIQSequence list        
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> groupedData = LoadDataByROVIQSequence();
        List<string> keys = groupedData.Keys.ToList();
        //To get all questions by groupName

        Dictionary<string, Dictionary<string, string>> finalids = new Dictionary<string, Dictionary<string, string>>();
        //List<string> finalSection_Ques = new List<string>();
        //List<string> finalObs_Details = new List<string>();
        foreach (string key in keys)
        {
            if (key.ToLower().Contains(HeadertextforROVIQ)) //WORKING HERE 26th MAR 2024
            {
                Dictionary<string, Dictionary<string, string>> questions = groupedData[key];
                foreach (string qid in questions.Keys)
                {
                    if (!finalids.Keys.Contains(qid))
                    {
                        finalids.Add(qid, questions[qid]);
                    }
                }

            }
        }
        foreach (string id in finalids.Keys)
        {
            Dictionary<string, string> prop = finalids[id];

            if (!String.IsNullOrEmpty(prop["Obs_Details_7"]) || prop["Obs_Details_7"].Trim() == "Completed")
            {
                string completionstatus = prop["Obs_Details_7"];
                //int indexforselectedanswer = int.Parse(selectedanswer.Split('-')[0]);// KEEP the index number only, remove text after the "-".
                AnsweredQuesforROVIQ++;
            }
        }
        Debug.Log("Questiin Ids --" + string.Join(",", finalids.Keys));

        CompletionStatusforROVIQ.GetComponent<TextMeshProUGUI>().text = "Answered; " + AnsweredQuesforROVIQ.ToString() + "/" + finalids.Count.ToString();

        if (finalids.Count == 0)
        {
            Destroy(ChapterPrefabtodestroyforROVIQ.gameObject);
        }

        else if (AnsweredQuesforROVIQ == finalids.Count)
        {
            pendingquestionsforroviq.GetComponent<Image>().color = Color.green;
            pendingquestionsforroviq.GetComponent<Button>().interactable = false;
        }

        else 
        {
            pendingquestionsforroviq.GetComponent<Image>().color = Color.yellow;
            pendingquestionsforroviq.GetComponent<Button>().interactable = true;
        }

        Debug.Log("ROVIQ --> "+ finalids.Count.ToString());

    }

    public void CheckStandardPhotos()
    {
        standardphotos = new List<string>();
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(inspection_primarydetails_ID.ToString()));
        standardphotos = mlocationdb1.standardphotos.ToString().Split(',').Select(x => x.Trim().ToLower()).ToList();

        if ((standardphotos != null) && (standardphotos.Count > 1))
        {
            chaptersaccordion = Instantiate(chaptersitemprefab);
            chapterprefabitems Chapter = chaptersaccordion.GetComponent<chapterprefabitems>();
            chaptersaccordion.transform.SetParent(ParentPanel, false);
           
            Chapter.RightclickArrow.GetComponent<Button>().onClick.AddListener(Chapter.GetComponent<loadquestionfmObsTable>().ForStandardPhotos);
            Chapter.GetComponent<loadquestionfmObsTable>().inspPrimaryID = inspection_primarydetails_ID;
            Chapter.GetComponent<loadquestionfmObsTable>().standardphotoheaders = standardphotos;
            Chapter.GetComponent<loadquestionfmObsTable>().SuffixForPhotos = "S";
            Chapter.Header_Text.text = "Standard Additional Photos";
            Chapter.InspPrimaryID.text = inspection_primarydetails_ID.ToString();
            Chapter.pendingquestions.gameObject.SetActive(false);
        }

        else
        {
            chaptersaccordion = Instantiate(chaptersitemprefab);
            chapterprefabitems Chapter = chaptersaccordion.GetComponent<chapterprefabitems>();
            chaptersaccordion.transform.SetParent(ParentPanel, false);
           
            Chapter.RightclickArrow.GetComponent<Button>().onClick.AddListener(Chapter.GetComponent<loadquestionfmObsTable>().ForStandardPhotos);
            Chapter.GetComponent<loadquestionfmObsTable>().inspPrimaryID = inspection_primarydetails_ID;
            Chapter.GetComponent<loadquestionfmObsTable>().SuffixForPhotos = "O";
            Chapter.Header_Text.text = "Upload Additional Photographs";
            Chapter.pendingquestions.gameObject.SetActive(false);
            Chapter.InspPrimaryID.text = inspection_primarydetails_ID.ToString();
        }
        connection1.Close();
        //mlocationdb1.close();
    }

    
    IEnumerator HidePopUp()
    {
        
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }

    public void OnSearchClicked()
    {
        string input = smartsearchInput.text;
        List<QAItem> results = SearchByGroupedKeywords(input);
        List<string> prefabNames = GetKeywordGroupNames(input);
        Dictionary<string, List<int>> rowIdMap = GetRowIdsByGroup(input);

        Debug.Log("Results Found: " + results.Count);
        foreach (var item in results)
        {
            Debug.Log($"{item.RowID}:{item.QuestionNumber}: {item.QuestionText}");
        }

        Debug.Log("Prefab Group Names:");
        foreach (var name in prefabNames)
        {
            Debug.Log(name);
        }

        foreach (var kvp in rowIdMap)
        {
            Debug.Log($"Group: {kvp.Key} → RowIDs: {string.Join(", ", kvp.Value)}");
        }
        
        Debug.Log("Results Found: " + results.Count);
        if (results.Count <= 0)
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! No result for this search, please try another";
        }
        LoadSmartSearchHeaders(input);
    }

    public List<List<string>> ParseKeywordGroups(string input)
    {
        return input.Split(',')
            .Select(group => group
                .Split('&')
                .Select(k => k.Trim().ToLower())
                .Where(k => !string.IsNullOrEmpty(k))
                .Distinct()
                .ToList()
            )
            .Where(g => g.Count > 0)
            .ToList();
    }

    public List<QAItem> SearchByGroupedKeywords(string input)
    {
        List<List<string>> keywordGroups = ParseKeywordGroups(input);
        List<QAItem> totalResults = new List<QAItem>();

        foreach (var group in keywordGroups)
        {
            if (group.Count == 0) continue;

            List<QAItem> groupResults = new List<QAItem>();

            table_Inspection_Observations mLocationDb = new table_Inspection_Observations();
            using (var connection = mLocationDb.getConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    string kw = group[0];
                    cmd.CommandText = @"
                        SELECT * FROM Inspection_Observations 
                        WHERE Obs_Details_8 LIKE '%Question%' AND cast(Inspection_PrimaryDetails_ID as int) = @inspID AND (
                            LOWER(Obs_Details_1) LIKE @kw OR 
                            LOWER(InspectionGuidance) LIKE @kw OR 
                            LOWER(ROVIQSequence) LIKE @kw
                        )";
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    cmd.Parameters.AddWithValue("@inspID", int.Parse(inspection_primarydetails_ID.ToString()));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            groupResults.Add(new QAItem
                            {
                                RowID = int.Parse(reader["ID"].ToString()),
                                QuestionNumber = reader["Template_Section_Ques"].ToString(),
                                QuestionText = reader["Obs_Details_1"].ToString(),
                                ROVIQSequence = reader["ROVIQSequence"].ToString(),
                                HumanElement = reader["ShortQuestionText"].ToString(),
                                ProcessElement = reader["Publications"].ToString(),
                                HardwareElement = reader["IndustryGuidance"].ToString(),
                                Objective = reader["Objective"].ToString(),
                                Rank = reader["InspectionGuidance"].ToString(),
                                SelectedAnswer = reader["Selected_Answer"].ToString()
                            });
                        }
                    }
                }

                connection.Close();
            }

            for (int i = 1; i < group.Count; i++)
            {
                string kw = group[i];
                groupResults = groupResults
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.QuestionText) && q.QuestionText.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(q.Rank) && q.Rank.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(q.ROVIQSequence) && q.ROVIQSequence.ToLower().Contains(kw)))
                    .ToList();
            }

            totalResults.AddRange(groupResults);
        }

        return totalResults.Distinct().ToList();
    }

    public List<string> GetKeywordGroupNames(string input)
    {
        var groupNames = input.Split(',')
            .Select(group => group
                .Split('&')
                .Select(k => k.Trim().ToLower().Replace(" ", "_"))
                .Where(k => !string.IsNullOrEmpty(k))
                .ToList()
            )
            .Where(g => g.Count > 0)
            .Select(g => string.Join("__", g))
            .ToList();

        return groupNames;
    }

    public Dictionary<string, List<int>> GetRowIdsByGroup(string input)
    {
        List<List<string>> keywordGroups = ParseKeywordGroups(input);
        Dictionary<string, List<int>> groupRowIdMap = new Dictionary<string, List<int>>();

        foreach (var group in keywordGroups)
        {
            if (group.Count == 0) continue;

            string groupName = string.Join("__", group.Select(k => k.Trim().ToLower().Replace(" ", "_")));

            List<QAItem> groupResults = new List<QAItem>();
            table_Inspection_Observations mLocationDb = new table_Inspection_Observations();

            using (var connection = mLocationDb.getConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    string kw = group[0];
                    cmd.CommandText = @"
                        SELECT * FROM Inspection_Observations 
                        WHERE Obs_Details_8 LIKE '%Question%' AND cast(Inspection_PrimaryDetails_ID as int) = @inspID
                        AND (
                            LOWER(Obs_Details_1) LIKE @kw OR 
                            LOWER(InspectionGuidance) LIKE @kw OR 
                            LOWER(ROVIQSequence) LIKE @kw
                        )";
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    cmd.Parameters.AddWithValue("@inspID", int.Parse(inspection_primarydetails_ID.ToString()));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            groupResults.Add(new QAItem
                            {
                                RowID = int.Parse(reader["ID"].ToString()),
                                QuestionText = reader["Obs_Details_1"].ToString(),
                                Rank = reader["InspectionGuidance"].ToString(),
                                ROVIQSequence = reader["ROVIQSequence"].ToString()
                            });
                        }
                    }
                }

                connection.Close();
            }

            for (int i = 1; i < group.Count; i++)
            {
                string kw = group[i];
                groupResults = groupResults
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.QuestionText) && q.QuestionText.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(q.Rank) && q.Rank.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(q.ROVIQSequence) && q.ROVIQSequence.ToLower().Contains(kw)))
                    .ToList();
            }

            groupRowIdMap[groupName] = groupResults.Select(r => r.RowID).Distinct().ToList();
        }

        foreach (var kvp in groupRowIdMap)
        {
            Debug.Log($"Group: {kvp.Key} → RowIDs: {string.Join(", ", kvp.Value)}");
        }

        return groupRowIdMap;
    }

    public class QAItem
    {
        public int RowID;
        public string QuestionNumber;
        public string QuestionText;
        public string ROVIQSequence;
        public string HumanElement;
        public string ProcessElement;
        public string HardwareElement;
        public string Objective;
        public string Rank;
        public string SelectedAnswer;
    }

    public void LoadSmartSearchHeaders(string input) //Used for Chapters here, on LoadInspBlock.cs this is called Loadinspections().
    {
        Dictionary<string, List<int>> rowMap = GetRowIdsByGroup(input);
        List<string> groupNames = rowMap.Keys.ToList();

        InspStatus.text = "";
        Totalquestions = 0;
        Completedques = 0;
        TotalquestionsMainHd = 0;
        CompletedquesMainHd = 0;

        TotalquesinChap = 0;
        completionstatus = 0;
        LoadROVIQ = false;

        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }

        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection = mlocationDb2.getConnection();
        //using System.Data.IDataReader reader = mlocationDb2.getChapterData(inspection_primarydetails_ID);

        Totalquestions = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));
        Completedques = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) ='Completed' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));

        //Completedques = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(inspection_primarydetails_ID.ToString()));
        InspStatus.text = Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";
        //System.Data.IDataReader reader = mlocationDb.getdataforreader("Chapter");

        TotalquestionsMainHd = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(Inspection_PrimaryDetails_ID.ToString()));

        CompletedquesMainHd = mlocationDb2.totalRecords("Inspection_Observations where trim(Obs_Details_7) = 'Completed' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(Inspection_PrimaryDetails_ID.ToString()));
        InspectionStatus.GetComponent<TextMeshProUGUI>().text = CompletedquesMainHd.ToString() + " / " + TotalquestionsMainHd.ToString() + " Completed.";


        foreach (string groupKey in groupNames)
        {
            string displayTitle = groupKey.Replace("__", " → ").Replace("_", " ");
            List<int> rowIds = rowMap[groupKey];

            Debug.Log("Section Header: " + displayTitle);
            Debug.Log("Row IDs: " + string.Join(", ", rowIds));

            chaptersaccordion = Instantiate(chaptersitemprefab);
            chapterprefabitems Chapter = chaptersaccordion.GetComponent<chapterprefabitems>();
            chaptersaccordion.transform.SetParent(ParentPanel, false);
            
            if (Chapter.GetComponent<loadquestionfmObsTable>() != null)
            {
                Chapter.GetComponent<loadquestionfmObsTable>().ParentPanel_1 = QuestionsParentPanel;
            }

            //completionstatus = "CREATE COMPLETION STATUS FOR HERE";

            Chapter.Header_Text.text = displayTitle.ToString();

            Chapter.InspPrimaryID.text = inspection_primarydetails_ID.ToString();

            Chapter.ROVIQLOADTrueOrFalse.text = LoadROVIQ.ToString();

            if (Chapter.QuestionsIDListText != null)
            {
                Chapter.QuestionsIDListText.text = "Row IDs: " + string.Join(", ", rowIds);
                Chapter.Completionstatus.text = "Total Questions; " + rowIds.Count.ToString();
                Chapter.GetComponent<loadquestionfmObsTable>().QuestionIDS = string.Join(",", rowIds);
            }
            Chapter.RightclickArrow.GetComponent<Button>().onClick.AddListener(Chapter.GetComponent<loadquestionfmObsTable>().LoadquestionsBtnClicked);
            Chapter.GetComponent<loadquestionfmObsTable>().smartsearch = true;
        }


        //table_Inspection_Observations mlocationDb = new table_Inspection_Observations();

        connection.Close();
        CheckStandardPhotos();
        canvaspositions.ProceedtoChecklistBtn();

        //Template_Section_Ques = Convert.ToString(Template_Section_Ques);
        // GetStocksData(fromdbList);
    }

}
