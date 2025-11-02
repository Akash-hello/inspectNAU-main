using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using DataBank;
using System;
using System.Globalization;
using SQLite4Unity3d;

public class fetchinspectiontemplates : MonoBehaviour
{
    //Since this is the first class where the story of the APP begins by fetching the data, mentioning the tables here;
    //TABLE - Inspection_template - This is where the data is fetched and kept from server as the inspections templates library for then creating inspections and pulling the data into the next table. Master list of Versions, Chapters and Questions are available here.
    //TABLE - Inspection_PrimaryDetails - From Inspection Template to when a new inspection is started, One row for each inspection as the Master for the inspection details, Folder location for reports and attachments, ROVIQ, etc. governed from here.
    //TABLE - Inspection_Observations - Connected to above table through - Inspection_PrimaryDetails_ID. Chapters and Questions for the Inspection Primary details maintained here with their observations.
    //TABLE - Inspection_Attachments - Connected to above table through - Inspection_Observations_ID and also to Inspection_PrimaryDetails_ID, for maintaining Observation related uploads.
    //DATABASE - Orion maintained at APPLICATION.PERSISTENT DATA PATH or When on development C:\Users\YOURUSER\AppData\LocalLow\Launchfort Technologies\Orion Inspections App
    //TABLE - SYNCH AND CONFIG Are for Synchronisations and API Configurations
    //TABLE - LOGIN Config - for User details.

    private string url;
    public Text mainheader;
    public string Sector = "";
    public string versiondetails;
    public ChangeGamObjectPosition CanvasPositionManager;

    public string Inspectionname;
    public string Inspecteddate;

    public string Chaptername;
    public string Questions;

    public RectTransform ParentPanel;

    public GameObject inspectionsitemprefab;
    public GameObject inspectionaccordion;

    public GameObject publishedinspectionsmprefab;
    public GameObject publishedinspectionaccordion;

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    public Button GetInspections;
    public Button LoadTemplates;
    public GameObject BlueObjects;
    public GameObject WhiteObjects;

    public Button DraftInsp;
    public GameObject DraftBlueObjects;
    public GameObject DraftWhiteObjects;

    public Button Histbtn;
    public GameObject HistBlueObjects;
    public GameObject HistWhiteObjects;
    public Button SynchBtn;

    int id;
    public int Inspection_Template_ID = 0;
    public int Cloud_DB_ID = 0;
    public string Cloud_DB_ParentID = "";
    public string Version_Number = "";
    public string Version_Date = "";
    public string Template_Section_Ques = "";
    public string Details_1 = "";
    public string Details_2 = "";
    public string Details_3 = "";
    public string Details_4 = "";
    public string Details_5 = "";
    public string Details_6 = "";
    public string Details_7 = "";
    public string Details_8 = "";

    public string ShortQuestionText_Sire = "";
    public string VesselTypes_Sire = "";
    public string ROVIQSequence_Sire = "";
    public string Publications_Sire = "";
    public string Objective_Sire = "";
    public string IndustryGuidance = "";
    public string TMSAKPI = "";
    public string IMOISMCode = "";
    public string InspectionGuidance = "";
    public string SuggestedInspectorActions = "";
    public string ExpectedEvidence = "";
    public string PotentialGroundsNegativeObservation = "";

    public string Active = "";
    public string Timestamp = "";
    public string outputofsearchresult;
    public string searchresults;
    public int datacount;

    public TextMeshProUGUI templatesCount;
    public TextMeshProUGUI draftinspCount;
    public TextMeshProUGUI publishedinspCount;

    int templatescount;
    int draftinspcount;
    int publishedinspcount;
    string SelectedhexColor;
    string NormalhexColor;

    public string APIauthkey;
    public string IncomingAPI;

    public APIInputsAndTests communicationAPIs;

    List<FromDatabase_Insptemplates> fromdbList = new List<FromDatabase_Insptemplates>();
    public Image Checklistversionsupdates;
    public GameObject[] flashmesgindicators;

    public void Start()
    {
        NormalhexColor = "#BEBEBE"; // Example hex color code
        Color normalcolor = ColorHelper.HexToColor(NormalhexColor);

        SelectedhexColor = "#2C2B63"; // Example hex color code
        Color selectcolor = ColorHelper.HexToColor(SelectedhexColor);

        Checklistversionsupdates.GetComponent<Image>().color = Color.white;

        foreach (GameObject msg in flashmesgindicators)
        {
            msg.SetActive(false);

        }
    }

    public void DownloadOrNotChecklists () //Basis timestamp of last done checklist synch, checklists download

        {
        table_Inspection_template mlocationdb = new table_Inspection_template();
        using var connection = mlocationdb.getConnection();

    }

    public void InpsectiontemplatesServerCall () //This function is for fetching the checklists from the cloud basis Orion or Company specific checklists,
                                                 //it uses the incoming API from Login table (APITOKEN field was used for this purpose),
                                                 //and it also update the CONFIG table for setting the incoming API.
    {
        int latestid = 0;
        string authkey = "";
        
        try
        {
            table_LoginConfig mlocationDb1 = new table_LoginConfig();
            using var connection = mlocationDb1.getConnection();
            {
                mlocationDb1.getLatestID();
                IncomingAPI = mlocationDb1.IncomingAPIdetail.Trim(); //GETTING THE API LINK FROM THE LOGIN TABLE...
                latestid = mlocationDb1.LatestId;
                authkey = mlocationDb1.ApiAuthKey.Trim();
                //mlocationDb1.close();
            }

            for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }

            if (latestid > 0 && (string.IsNullOrEmpty(authkey) || authkey == "Awaited"))

            {
                GetInspections.enabled = false;
                //url = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=D3f@u!T#9xY";

                url = IncomingAPI + "D3f@u!T#9xY"; //this is the default code for fetching Orion only checklists, i.e. '0' ID.

                table_Config mLocationDb = new table_Config();
                using var connection1 = mLocationDb.getConnection();
                {
                    mLocationDb.Updatedata("IncomingAPI = '" + url.ToString() + "', OutGoingAPI = 'https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post'");
                }
                    
                //url = "file:///Users/mohit_s_new/Downloads/QA_response_mohit.json"; //SIRE 2.0

            }

            else
            {
                GetInspections.enabled = false;

                //url = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=" + mlocationDb.ApiAuthKey.Trim();

                url = IncomingAPI + authkey; //this is the default code for fetching COMPANY + Orion checklists.

                table_Config mLocationDb = new table_Config();
                using var connection2 = mLocationDb.getConnection();
                {
                    mLocationDb.Updatedata("IncomingAPI = '" + url.ToString() + "', OutGoingAPI = 'https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post'");
                }
                // url = "file:///Users/mohit_s_new/Downloads/QA_response_mohitcopy.json"; //SIRE 2.0

            }
            //communicationAPIs.IncomingAPIINput.text = url; //THIS FEEDS FOR INCOMING API FROM THE FETCHINSPECTION COde instead of manual entry..
            //communicationAPIs.UpdateConnectionDetails();


            StartCoroutine(CLoudtoDB());

        }catch(Exception ex)
        {
            Debug.Log("There was an error on this function "+ ex.StackTrace + "And error message was " + ex.Message);
        }
    }

    public void InitiliaseDropTables()
    {
        table_Inspection_template mlocdb4 = new table_Inspection_template();
        using var connection = mlocdb4.getConnection();
        mlocdb4.DropTable();
        //mlocdb4.close();

        table_Inspection_template mlocationDb = new table_Inspection_template();
        using var connection1 = mlocationDb.getConnection();
        mlocationDb.DropTable();
        //mlocationDb.close();

        table_Inspection_Attachments mlocdb1 = new table_Inspection_Attachments();
        using var connection2 = mlocdb1.getConnection();
        mlocdb1.DropTable();
        //mlocdb1.close();
        
        table_Inspection_Observations mlocdb2 = new table_Inspection_Observations();
        using var connection3 = mlocdb2.getConnection();
        mlocdb2.DropTable();
        //mlocdb2.close();
        
        table_Inspection_PrimaryDetails mlocdb3 = new table_Inspection_PrimaryDetails();
        using var connection4 = mlocdb3.getConnection();
        mlocdb3.DropTable();
        //mlocdb3.close();

        table_Synch mlocdb5 = new table_Synch();
        using var connection5 = mlocdb5.getConnection();

        mlocdb5.DropTable();
        //mlocdb5.close();

        table_Config mlocationDb1 = new table_Config();
        using var connection6 = mlocationDb1.getConnection();
        
        mlocationDb1.DropTable();

        //mlocationDb1.close();

    }

    IEnumerator CLoudtoDB () //This is the Checklist from the Cloud to Mobile APP DB for forming the Inspection Template Table. 
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Inspection_Template_ID = 0;
        Cloud_DB_ID = 0;
        Cloud_DB_ParentID = "";
        Version_Number = "";
        Version_Date = "";
        Template_Section_Ques = "";
        Details_1 = ""; //FOR Inspection level i.e. where Cloud_DB_ParentID = 0 and Type of inspection templates are captured, 
                        //this is used to identify the Answer Group models; Yes/No/NA/US or Good/Fair/Poor, or Satis/Major/MinorNC/Obs, etc.  
        Details_2 = "";
        Details_3 = "";
        Details_4 = "";
        Details_5 = "";
        Details_6 = "";
        Details_7 = "";
        Details_8 = ""; // Identifies if this row is a Version, Chapter or Question ROW
        
        ShortQuestionText_Sire = ""; // USE For CATEGORY - Human
        Publications_Sire = ""; // USE For CATEGORY - Process
        IndustryGuidance = "";  // USE For CATEGORY - Hardware
        VesselTypes_Sire = "";
        ROVIQSequence_Sire = ""; //Bears the Master Categories for grouping for ROVIQ option.
        Objective_Sire = "";    //OBJECTIVE
        TMSAKPI = "";           //TMSA KPI
        IMOISMCode = "";        //SMS Ref
        InspectionGuidance = "";//Tagged Rank
        SuggestedInspectorActions = ""; // Question Type; C / R1 / R2
        ExpectedEvidence = ""; //USE FOR MANDATORY PHOTOGRAPH
        PotentialGroundsNegativeObservation = "";


        Active = "";
        Timestamp = "";

        bool sameversioninDB = false;
        List<string> InspectionTemplates = new List<String>();

        string timestampversion1 = "";
        DateTime timestampversion = DateTime.MinValue; // used here for initialising..

        //DateTime timestampversion = Convert.ToDateTime("", CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
       // table_Inspection_template mlocationDb = new table_Inspection_template();

        // mlocationDb.getLatestID();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");

            GetInspections.enabled = true;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops !! Couldn't fetch checklists, something went wrong, please check your internet connection or try again.";
            StartCoroutine(HidePopUp());
        }

        else if (request.downloadHandler.text.Contains("GUID Does Not Exists"))
        {
            GetInspections.enabled = true;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops !! Couldn't fetch checklists, seems you are not linked to any Company for downloading checklists, please contact Orion APP Support Team At; orionapps@orionmarineconcepts.com.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Please wait fetching while we fetch your inspection questionnaire.";//Please wait connecting with the server to fetch updated inspection templates.
            StartCoroutine(HidePopUp());
            
            var Inspectionstemplatedata = JsonConvert.DeserializeObject<InspectionTemplatesMainClass>(request.downloadHandler.text);

            //int count = mlocationDb.totalRecords("table_Inspection_template");
            //Debug.Log("Count Found" + count);
            
            //Debug.Log("OLD INSPECTION DATA DELETED");
            if (Inspectionstemplatedata != null && Inspectionstemplatedata.Version !=null && Inspectionstemplatedata.Chapters != null && Inspectionstemplatedata.Question != null)
            {
                table_Inspection_template mlocationDb0 = new table_Inspection_template();
                using var connection = mlocationDb0.getConnection();
                mlocationDb0.deleteAllData();

                bool versionsinserted = false;
                int versionscount = 0;
                bool chaptersinserted = false;
                int chapterscount= 0;
                bool questionsinserted = false;
                int questionscount = 0;

                {
                    if (Inspectionstemplatedata.Version != null)
                    {
                        // Define retry parameters for handling locked database errors
                        const int maxRetries = 5;
                        const int retryDelayMs = 200; // 200ms delay between retries

                        int currentRetry = 0;
                        bool dataInserted = false;

                        while (currentRetry < maxRetries && !dataInserted)
                        {
                            try
                            {
                                // Using a single connection for all operations
                                var mlocationDb1 = new table_Inspection_template();
                                {
                                    using (var connection1 = mlocationDb1.getConnection())
                                    {
                                        //connection1.Open();

                                        // Set busy timeout to avoid locking issues
                                        using (var command = connection1.CreateCommand())
                                        {
                                            command.CommandText = "PRAGMA busy_timeout = 3000;"; // wait up to 3 seconds for a lock to clear
                                            command.ExecuteNonQuery();
                                        }

                                        // Start a transaction
                                        using (var transaction = connection1.BeginTransaction())
                                        {
                                            try
                                            {
                                                using (var command = connection1.CreateCommand())
                                                {
                                                    // Loop through questions and insert them
                                                    for (int i = 0; i < Inspectionstemplatedata.Version.Count; i++)
                                                    {
                                                        //Check the timestamp in the Database for the saved version records.
                                                        //using (table_Inspection_template mlocationDb = new table_Inspection_template())

                                                        //{

                                                        //    string query = "select InspectionTemplateID, Details_2, * from Inspection_template where cast(Cloud_DB_ID as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Version'";


                                                        // System.Data.IDataReader Obsrecordreader = mlocationDb.getDatabyQuery(query);

                                                        //    if (((System.Data.Common.DbDataReader)Obsrecordreader).HasRows && Obsrecordreader[1].ToString().Trim() == inspection.Details_2.ToString().Trim().Replace("'", "`"))
                                                        //    {
                                                        //        Debug.Log("Details_2 basically in DB is  =" + Obsrecordreader[1].ToString().Trim() + " this is same as received on the API or different...");
                                                        //        //Convert.ToDateTime(inspection.Timestamp.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                        //        string query1 = "cast(Cloud_DB_ID as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Version'";

                                                        //        string query2 = "cast(Cloud_DB_ParentID as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Chapter'";

                                                        //        string query3 = "cast(Version_Number as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Question'";

                                                        //        mlocationDb.deleteItemsQuery(query1);
                                                        //        mlocationDb.deleteItemsQuery(query2);
                                                        //        mlocationDb.deleteItemsQuery(query3);

                                                        //        //InspectionTemplates.Add("Existing; " + inspection.Template_Section_Ques.ToString() + " Inspection. " + inspection.Version_Number + " Vers. Dated; " + inspection.Version_Date + Environment.NewLine);

                                                        //        //sameversioninDB = true;
                                                        //    }

                                                        //    else if (((System.Data.Common.DbDataReader)Obsrecordreader).HasRows && Obsrecordreader[1].ToString().Trim() != inspection.Details_2.ToString().Trim().Replace("'", "`"))
                                                        //    {
                                                        //        Debug.Log("Details_2 or version is not same in DB =" + Obsrecordreader[1].ToString().Trim() + " as received on the API or different...");
                                                        //        //Convert.ToDateTime(inspection.Timestamp.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                        //        string query1 = "cast(Cloud_DB_ID as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Version'";

                                                        //        string query2 = "cast(Cloud_DB_ParentID as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Chapter'";

                                                        //        string query3 = "cast(Version_Number as int)= '" + int.Parse(inspection.Cloud_DB_ID.ToString()) + "' and TRIM(Details_8) = 'Question'";

                                                        //        mlocationDb.deleteItemsQuery(query1);
                                                        //        mlocationDb.deleteItemsQuery(query2);
                                                        //        mlocationDb.deleteItemsQuery(query3);

                                                        //        InspectionTemplates.Add(inspection.Template_Section_Ques.ToString() + ", Ver.; " + inspection.Version_Number + ", Dated; " + inspection.Version_Date + " imported." + Environment.NewLine);

                                                        //        //sameversioninDB = false; //

                                                        //    }

                                                        //    else
                                                        //    {
                                                        //        Debug.Log("There's no data in the DB....");

                                                        //    }
                                                        //    //timestampversion = Convert.ToDateTime(inspection.Timestamp.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                                                        //}

                                                        Version inspection = Inspectionstemplatedata.Version[i]; //This will run with the "for(int i =0.......

                                                        if (inspection.Template_Section_Ques == null || inspection.Version_Number == null)
                                                        {
                                                            Debug.Log("The API is Empty.. or DO nothing as the Database has the same inspection already...");
                                                            
                                                            continue;
                                                        }

                                                        Inspection_Template_ID = 0;
                                                        Cloud_DB_ID = inspection.Cloud_DB_ID;
                                                        Cloud_DB_ParentID = inspection.Cloud_DB_ParentID;
                                                        Version_Number = inspection.Version_Number;
                                                        Version_Date = inspection.Version_Date;

                                                        Template_Section_Ques = inspection.Template_Section_Ques.ToString().Replace("'", "`");

                                                        Details_1 = inspection.Details_1.ToString().Replace("'", "`");
                                                        Details_2 = inspection.Details_2.ToString().Replace("'", "`");
                                                        Details_3 = inspection.Details_3.ToString().Replace("'", "`");
                                                        Details_4 = inspection.Details_4.ToString().Replace("'", "`");
                                                        Details_5 = inspection.Details_5.ToString().Replace("'", "`");
                                                        Details_6 = inspection.Details_6.ToString().Replace("'", "`");
                                                        Details_7 = inspection.Details_7.ToString().Replace("'", "`");
                                                        Details_8 = "Version";

                                                        ShortQuestionText_Sire = inspection.ShortQuestionText.ToString().Replace("'", "`");
                                                        VesselTypes_Sire = inspection.VesselTypes.ToString().Replace("'", "`");
                                                        ROVIQSequence_Sire = inspection.ROVIQSequence.ToString().Replace("'", "`");
                                                        Publications_Sire = inspection.Publications.ToString().Replace("'", "`");
                                                        Objective_Sire = inspection.Objective.ToString().Replace("'", "`");
                                                        IndustryGuidance = inspection.IndustryGuidance.ToString().Replace("'", "`");
                                                        TMSAKPI = inspection.TMSAKPI.ToString().Replace("'", "`");
                                                        IMOISMCode = inspection.IMOISMCode.ToString().Replace("'", "`");
                                                        InspectionGuidance = inspection.InspectionGuidance.ToString().Replace("'", "`");
                                                        SuggestedInspectorActions = inspection.SuggestedInspectorActions.ToString().Replace("'", "`");
                                                        ExpectedEvidence = inspection.ExpectedEvidence.ToString().Replace("'", "`");
                                                        PotentialGroundsNegativeObservation = inspection.PotentialGroundsNegativeObservation.ToString().Replace("'", "`");

                                                        Active = inspection.Active.ToString();
                                                        Timestamp = inspection.Timestamp.ToString();

                                                        // Insert data using SQLiteCommand
                                                        command.CommandText = $@"
                                        INSERT INTO Inspection_template
                                        (InspectionTemplateID, Cloud_DB_ID, Cloud_DB_ParentID, Version_Number, Version_Date, Template_Section_Ques,
                                        Details_1, Details_2, Details_3, Details_4, Details_5, Details_6, Details_7, Details_8, ShortQuestionText,
                                        VesselTypes, ROVIQSequence, Publications, Objective, IndustryGuidance, TMSAKPI, IMOISMCode,
                                        InspectionGuidance, SuggestedInspectorActions, ExpectedEvidence, PotentialGroundsObs, Active, Timestamp)
                                        VALUES
                                        (@Inspection_Template_ID, @Cloud_DB_ID, @Cloud_DB_ParentID, @Version_Number, @Version_Date, @Template_Section_Ques,
                                        @Details_1, @Details_2, @Details_3, @Details_4, @Details_5, @Details_6, @Details_7, @Details_8, @ShortQuestionText,
                                        @VesselTypes, @ROVIQSequence, @Publications, @Objective, @IndustryGuidance, @TMSAKPI, @IMOISMCode,
                                        @InspectionGuidance, @SuggestedInspectorActions, @ExpectedEvidence, @PotentialGroundsObs, @Active, @Timestamp)";

                                                        command.Parameters.Clear();
                                                        command.Parameters.AddWithValue("@Inspection_Template_ID", Inspection_Template_ID);
                                                        command.Parameters.AddWithValue("@Cloud_DB_ID", Cloud_DB_ID);
                                                        command.Parameters.AddWithValue("@Cloud_DB_ParentID", Cloud_DB_ParentID);
                                                        command.Parameters.AddWithValue("@Version_Number", Version_Number);
                                                        command.Parameters.AddWithValue("@Version_Date", Version_Date);
                                                        command.Parameters.AddWithValue("@Template_Section_Ques", Template_Section_Ques);
                                                        command.Parameters.AddWithValue("@Details_1", Details_1);
                                                        command.Parameters.AddWithValue("@Details_2", Details_2);
                                                        command.Parameters.AddWithValue("@Details_3", Details_3);
                                                        command.Parameters.AddWithValue("@Details_4", Details_4);
                                                        command.Parameters.AddWithValue("@Details_5", Details_5);
                                                        command.Parameters.AddWithValue("@Details_6", Details_6);
                                                        command.Parameters.AddWithValue("@Details_7", Details_7);
                                                        command.Parameters.AddWithValue("@Details_8", Details_8);
                                                        command.Parameters.AddWithValue("@ShortQuestionText", ShortQuestionText_Sire);
                                                        command.Parameters.AddWithValue("@VesselTypes", VesselTypes_Sire);
                                                        command.Parameters.AddWithValue("@ROVIQSequence", ROVIQSequence_Sire);
                                                        command.Parameters.AddWithValue("@Publications", Publications_Sire);
                                                        command.Parameters.AddWithValue("@Objective", Objective_Sire);
                                                        command.Parameters.AddWithValue("@IndustryGuidance", IndustryGuidance);
                                                        command.Parameters.AddWithValue("@TMSAKPI", TMSAKPI);
                                                        command.Parameters.AddWithValue("@IMOISMCode", IMOISMCode);
                                                        command.Parameters.AddWithValue("@InspectionGuidance", InspectionGuidance);
                                                        command.Parameters.AddWithValue("@SuggestedInspectorActions", SuggestedInspectorActions);
                                                        command.Parameters.AddWithValue("@ExpectedEvidence", ExpectedEvidence);
                                                        command.Parameters.AddWithValue("@PotentialGroundsObs", PotentialGroundsNegativeObservation);
                                                        command.Parameters.AddWithValue("@Active", Active);
                                                        command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));

                                                        command.ExecuteNonQuery();

                                                        //if (i == Inspectionstemplatedata.Version.Count - 1)
                                                        //{
                                                        //    versionsinserted = true;
                                                        //    Debug.Log(Inspectionstemplatedata.Version.Count + " versions inserted successfully:"); 
                                                        //}
                                                        //else
                                                        //{
                                                        //    versionsinserted = false;
                                                        //}
                                                    }
                                                    
                                                    transaction.Commit();
                                                    dataInserted = true;
                                                    versionsinserted = true;
                                                    versionscount = Inspectionstemplatedata.Version.Count;
                                                    Debug.Log(Inspectionstemplatedata.Version.Count + " versions inserted successfully:");

                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.LogError($"Error during insert: {ex.Message}");
                                                transaction.Rollback();
                                                versionsinserted = false;
                                                throw;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
                            {
                                currentRetry++;
                                if (currentRetry >= maxRetries)
                                {
                                    Debug.LogError("Max retries reached. Database is still locked.");
                                    throw;
                                }

                                Debug.LogWarning("Database is locked, retrying...");
                                System.Threading.Thread.Sleep(retryDelayMs);
                            }
                        }
                    }
                        
                    if (versionsinserted == true)
                    {
                        // Define retry parameters for handling locked database errors
                        const int maxRetries = 5;
                        const int retryDelayMs = 200; // 200ms delay between retries

                        int currentRetry = 0;
                        bool dataInserted = false;

                        while (currentRetry < maxRetries && !dataInserted)
                        {
                            try
                            {
                                // Using a single connection for all operations
                                var mlocationDb1 = new table_Inspection_template();
                                {
                                    using (var connection1 = mlocationDb1.getConnection())
                                    {
                                        //connection1.Open();

                                        // Set busy timeout to avoid locking issues
                                        using (var command = connection1.CreateCommand())
                                        {
                                            command.CommandText = "PRAGMA busy_timeout = 3000;"; // wait up to 3 seconds for a lock to clear
                                            command.ExecuteNonQuery();
                                        }

                                        // Start a transaction
                                        using (var transaction = connection1.BeginTransaction())
                                        {
                                            try
                                            {
                                                using (var command = connection1.CreateCommand())
                                                {
                                                    // Loop through questions and insert them
                                                    for (int i = 0; i < Inspectionstemplatedata.Chapters.Count; i++)
                                                    {
                                                        Chapter chapter = Inspectionstemplatedata.Chapters[i];

                                                        if (chapter.Template_Section_Ques == null || chapter.Version_Number == null)
                                                        {
                                                            Debug.Log("Empty chapter data..");
                                                            continue;
                                                        }

                                                        // Map chapter fields to variables
                                                        Inspection_Template_ID = 0;
                                                        Cloud_DB_ID = chapter.Cloud_DB_ID;
                                                        Cloud_DB_ParentID = chapter.Cloud_DB_ParentID;
                                                        Version_Number = chapter.Version_Number;
                                                        Version_Date = chapter.Version_Date;

                                                        Template_Section_Ques = chapter.Template_Section_Ques.ToString().Replace("'", "`");

                                                        Details_1 = chapter.Details_1.ToString().Replace("'", "`");
                                                        Details_2 = chapter.Details_2.ToString().Replace("'", "`");
                                                        Details_3 = chapter.Details_3.ToString().Replace("'", "`");
                                                        Details_4 = chapter.Details_4.ToString().Replace("'", "`");
                                                        Details_5 = chapter.Details_5.ToString().Replace("'", "`");
                                                        Details_6 = chapter.Details_6.ToString().Replace("'", "`");
                                                        Details_7 = chapter.Details_7.ToString().Replace("'", "`");
                                                        Details_8 = "Chapter";

                                                        ShortQuestionText_Sire = chapter.ShortQuestionText.ToString().Replace("'", "`");
                                                        VesselTypes_Sire = chapter.VesselTypes.ToString().Replace("'", "`");
                                                        ROVIQSequence_Sire = chapter.ROVIQSequence.ToString().Replace("'", "`");
                                                        Publications_Sire = chapter.Publications.ToString().Replace("'", "`");
                                                        Objective_Sire = chapter.Objective.ToString().Replace("'", "`");
                                                        IndustryGuidance = chapter.IndustryGuidance.ToString().Replace("'", "`");
                                                        TMSAKPI = chapter.TMSAKPI.ToString().Replace("'", "`");
                                                        IMOISMCode = chapter.IMOISMCode.ToString().Replace("'", "`");
                                                        InspectionGuidance = chapter.InspectionGuidance.ToString().Replace("'", "`");
                                                        SuggestedInspectorActions = chapter.SuggestedInspectorActions.ToString().Replace("'", "`");
                                                        ExpectedEvidence = chapter.ExpectedEvidence.ToString().Replace("'", "`");
                                                        PotentialGroundsNegativeObservation = chapter.PotentialGroundsNegativeObservation.ToString().Replace("'", "`");

                                                        Active = chapter.Active.ToString();
                                                        Timestamp = chapter.Timestamp.ToString();

                                                        // Insert data using SQLiteCommand
                                                        command.CommandText = $@"
                                        INSERT INTO Inspection_template
                                        (InspectionTemplateID, Cloud_DB_ID, Cloud_DB_ParentID, Version_Number, Version_Date, Template_Section_Ques,
                                        Details_1, Details_2, Details_3, Details_4, Details_5, Details_6, Details_7, Details_8, ShortQuestionText,
                                        VesselTypes, ROVIQSequence, Publications, Objective, IndustryGuidance, TMSAKPI, IMOISMCode,
                                        InspectionGuidance, SuggestedInspectorActions, ExpectedEvidence, PotentialGroundsObs, Active, Timestamp)
                                        VALUES
                                        (@Inspection_Template_ID, @Cloud_DB_ID, @Cloud_DB_ParentID, @Version_Number, @Version_Date, @Template_Section_Ques,
                                        @Details_1, @Details_2, @Details_3, @Details_4, @Details_5, @Details_6, @Details_7, @Details_8, @ShortQuestionText,
                                        @VesselTypes, @ROVIQSequence, @Publications, @Objective, @IndustryGuidance, @TMSAKPI, @IMOISMCode,
                                        @InspectionGuidance, @SuggestedInspectorActions, @ExpectedEvidence, @PotentialGroundsObs, @Active, @Timestamp)";

                                                        command.Parameters.Clear();
                                                        command.Parameters.AddWithValue("@Inspection_Template_ID", Inspection_Template_ID);
                                                        command.Parameters.AddWithValue("@Cloud_DB_ID", Cloud_DB_ID);
                                                        command.Parameters.AddWithValue("@Cloud_DB_ParentID", Cloud_DB_ParentID);
                                                        command.Parameters.AddWithValue("@Version_Number", Version_Number);
                                                        command.Parameters.AddWithValue("@Version_Date", Version_Date);
                                                        command.Parameters.AddWithValue("@Template_Section_Ques", Template_Section_Ques);
                                                        command.Parameters.AddWithValue("@Details_1", Details_1);
                                                        command.Parameters.AddWithValue("@Details_2", Details_2);
                                                        command.Parameters.AddWithValue("@Details_3", Details_3);
                                                        command.Parameters.AddWithValue("@Details_4", Details_4);
                                                        command.Parameters.AddWithValue("@Details_5", Details_5);
                                                        command.Parameters.AddWithValue("@Details_6", Details_6);
                                                        command.Parameters.AddWithValue("@Details_7", Details_7);
                                                        command.Parameters.AddWithValue("@Details_8", Details_8);
                                                        command.Parameters.AddWithValue("@ShortQuestionText", ShortQuestionText_Sire);
                                                        command.Parameters.AddWithValue("@VesselTypes", VesselTypes_Sire);
                                                        command.Parameters.AddWithValue("@ROVIQSequence", ROVIQSequence_Sire);
                                                        command.Parameters.AddWithValue("@Publications", Publications_Sire);
                                                        command.Parameters.AddWithValue("@Objective", Objective_Sire);
                                                        command.Parameters.AddWithValue("@IndustryGuidance", IndustryGuidance);
                                                        command.Parameters.AddWithValue("@TMSAKPI", TMSAKPI);
                                                        command.Parameters.AddWithValue("@IMOISMCode", IMOISMCode);
                                                        command.Parameters.AddWithValue("@InspectionGuidance", InspectionGuidance);
                                                        command.Parameters.AddWithValue("@SuggestedInspectorActions", SuggestedInspectorActions);
                                                        command.Parameters.AddWithValue("@ExpectedEvidence", ExpectedEvidence);
                                                        command.Parameters.AddWithValue("@PotentialGroundsObs", PotentialGroundsNegativeObservation);
                                                        command.Parameters.AddWithValue("@Active", Active);
                                                        command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));

                                                        command.ExecuteNonQuery();

                                                        //if (i == Inspectionstemplatedata.Chapters.Count - 1)
                                                        //{
                                                        //    chaptersinserted = true;
                                                        //}
                                                        //else
                                                        //{
                                                        //    chaptersinserted = false;
                                                        //}
                                                    }

                                                    transaction.Commit();
                                                    dataInserted = true;

                                                    chaptersinserted = true;
                                                    Debug.Log(Inspectionstemplatedata.Chapters.Count + " chapters inserted successfully:");
                                                    chapterscount = Inspectionstemplatedata.Chapters.Count;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.LogError($"Error during insert: {ex.Message}");
                                                transaction.Rollback();
                                                chaptersinserted = false;
                                                throw;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
                            {
                                currentRetry++;
                                if (currentRetry >= maxRetries)
                                {
                                    Debug.LogError("Max retries reached. Database is still locked.");
                                    throw;
                                }

                                Debug.LogWarning("Database is locked, retrying...");
                                System.Threading.Thread.Sleep(retryDelayMs);
                            }
                        };
                    }

                    else

                    {
                        Debug.Log("Please wait; inserting versions");

                    }
                    if (chaptersinserted == true)
                    {
                        // Define retry parameters for handling locked database errors
                        const int maxRetries = 5;
                        const int retryDelayMs = 200; // 200ms delay between retries

                        int currentRetry = 0;
                        bool dataInserted = false;

                        while (currentRetry < maxRetries && !dataInserted)
                        {
                            try
                            {
                                // Using a single connection for all operations
                                var mlocationDb1 = new table_Inspection_template();
                                {
                                    using (var connection1 = mlocationDb1.getConnection())
                                    {
                                        //connection1.Open();

                                        // Set busy timeout to avoid locking issues
                                        using (var command = connection1.CreateCommand())
                                        {
                                            command.CommandText = "PRAGMA busy_timeout = 3000;"; // wait up to 3 seconds for a lock to clear
                                            command.ExecuteNonQuery();
                                        }

                                        // Start a transaction
                                        using (var transaction = connection1.BeginTransaction())
                                        {
                                            try
                                            {
                                                using (var command = connection1.CreateCommand())
                                                {
                                                    // Loop through questions and insert them
                                                    for (int i = 0; i < Inspectionstemplatedata.Question.Count; i++)
                                                    {
                                                        Question question = Inspectionstemplatedata.Question[i];

                                                        if (question.Template_Section_Ques == null || question.Version_Number == null)
                                                        {
                                                            Debug.Log("Empty question encountered. Skipping...");
                                                            continue;
                                                        }
                                                        
                                                        // Extract and sanitize data
                                                        Inspection_Template_ID = 0;
                                                        Cloud_DB_ID = question.Cloud_DB_ID;
                                                        Cloud_DB_ParentID = question.Cloud_DB_ParentID;
                                                        Version_Number = question.Version_Number;
                                                        Version_Date = question.Version_Date;

                                                        Template_Section_Ques = question.Template_Section_Ques.ToString().Replace("'", "`");
                                                        Details_1 = question.Details_1?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_2 = question.Details_2?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_3 = question.Details_3?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_4 = question.Details_4?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_5 = question.Details_5?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_6 = question.Details_6?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_7 = question.Details_7?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Details_8 = "Question";

                                                        ShortQuestionText_Sire = question.ShortQuestionText?.ToString().Replace("'", "`") ?? string.Empty;
                                                        VesselTypes_Sire = question.VesselTypes?.ToString().Replace("'", "`") ?? string.Empty;
                                                        ROVIQSequence_Sire = question.ROVIQSequence?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Publications_Sire = question.Publications?.ToString().Replace("'", "`") ?? string.Empty;
                                                        Objective_Sire = question.Objective?.ToString().Replace("'", "`") ?? string.Empty;
                                                        IndustryGuidance = question.IndustryGuidance?.ToString().Replace("'", "`") ?? string.Empty;
                                                        TMSAKPI = question.TMSAKPI?.ToString().Replace("'", "`") ?? string.Empty;
                                                        IMOISMCode = question.IMOISMCode?.ToString().Replace("'", "`") ?? string.Empty;
                                                        InspectionGuidance = question.InspectionGuidance?.ToString().Replace("'", "`") ?? string.Empty;
                                                        SuggestedInspectorActions = question.SuggestedInspectorActions?.ToString().Replace("'", "`") ?? string.Empty;
                                                        ExpectedEvidence = question.ExpectedEvidence?.ToString().Replace("'", "`") ?? string.Empty;
                                                        PotentialGroundsNegativeObservation = question.PotentialGroundsNegativeObservation?.ToString().Replace("'", "`") ?? string.Empty;

                                                        Active = question.Active?.ToString() ?? string.Empty;
                                                        Timestamp = question.Timestamp?.ToString() ?? string.Empty;

                                                        // Insert data using SQLiteCommand
                                                        command.CommandText = $@"
                                        INSERT INTO Inspection_template
                                        (InspectionTemplateID, Cloud_DB_ID, Cloud_DB_ParentID, Version_Number, Version_Date, Template_Section_Ques,
                                        Details_1, Details_2, Details_3, Details_4, Details_5, Details_6, Details_7, Details_8, ShortQuestionText,
                                        VesselTypes, ROVIQSequence, Publications, Objective, IndustryGuidance, TMSAKPI, IMOISMCode,
                                        InspectionGuidance, SuggestedInspectorActions, ExpectedEvidence, PotentialGroundsObs, Active, Timestamp)
                                        VALUES
                                        (@Inspection_Template_ID, @Cloud_DB_ID, @Cloud_DB_ParentID, @Version_Number, @Version_Date, @Template_Section_Ques,
                                        @Details_1, @Details_2, @Details_3, @Details_4, @Details_5, @Details_6, @Details_7, @Details_8, @ShortQuestionText,
                                        @VesselTypes, @ROVIQSequence, @Publications, @Objective, @IndustryGuidance, @TMSAKPI, @IMOISMCode,
                                        @InspectionGuidance, @SuggestedInspectorActions, @ExpectedEvidence, @PotentialGroundsObs, @Active, @Timestamp)";

                                                        command.Parameters.Clear();
                                                        command.Parameters.AddWithValue("@Inspection_Template_ID", Inspection_Template_ID);
                                                        command.Parameters.AddWithValue("@Cloud_DB_ID", Cloud_DB_ID);
                                                        command.Parameters.AddWithValue("@Cloud_DB_ParentID", Cloud_DB_ParentID);
                                                        command.Parameters.AddWithValue("@Version_Number", Version_Number);
                                                        command.Parameters.AddWithValue("@Version_Date", Version_Date);
                                                        command.Parameters.AddWithValue("@Template_Section_Ques", Template_Section_Ques);
                                                        command.Parameters.AddWithValue("@Details_1", Details_1);
                                                        command.Parameters.AddWithValue("@Details_2", Details_2);
                                                        command.Parameters.AddWithValue("@Details_3", Details_3);
                                                        command.Parameters.AddWithValue("@Details_4", Details_4);
                                                        command.Parameters.AddWithValue("@Details_5", Details_5);
                                                        command.Parameters.AddWithValue("@Details_6", Details_6);
                                                        command.Parameters.AddWithValue("@Details_7", Details_7);
                                                        command.Parameters.AddWithValue("@Details_8", Details_8);
                                                        command.Parameters.AddWithValue("@ShortQuestionText", ShortQuestionText_Sire);
                                                        command.Parameters.AddWithValue("@VesselTypes", VesselTypes_Sire);
                                                        command.Parameters.AddWithValue("@ROVIQSequence", ROVIQSequence_Sire);
                                                        command.Parameters.AddWithValue("@Publications", Publications_Sire);
                                                        command.Parameters.AddWithValue("@Objective", Objective_Sire);
                                                        command.Parameters.AddWithValue("@IndustryGuidance", IndustryGuidance);
                                                        command.Parameters.AddWithValue("@TMSAKPI", TMSAKPI);
                                                        command.Parameters.AddWithValue("@IMOISMCode", IMOISMCode);
                                                        command.Parameters.AddWithValue("@InspectionGuidance", InspectionGuidance);
                                                        command.Parameters.AddWithValue("@SuggestedInspectorActions", SuggestedInspectorActions);
                                                        command.Parameters.AddWithValue("@ExpectedEvidence", ExpectedEvidence);
                                                        command.Parameters.AddWithValue("@PotentialGroundsObs", PotentialGroundsNegativeObservation);
                                                        command.Parameters.AddWithValue("@Active", Active);
                                                        command.Parameters.AddWithValue("@Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));

                                                        command.ExecuteNonQuery();
                                                    }

                                                    transaction.Commit();
                                                    dataInserted = true;
                                                   
                                                    questionsinserted = true;
                                                    Debug.Log(Inspectionstemplatedata.Question.Count + " questions inserted successfully:");
                                                    questionscount = Inspectionstemplatedata.Question.Count;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.LogError($"Error during insert: {ex.Message}");
                                                transaction.Rollback();
                                                questionsinserted = false;
                                                throw;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
                            {
                                currentRetry++;
                                if (currentRetry >= maxRetries)
                                {
                                    Debug.LogError("Max retries reached. Database is still locked.");
                                    throw;
                                }

                                Debug.LogWarning("Database is locked, retrying...");
                                System.Threading.Thread.Sleep(retryDelayMs);
                            }
                        }
                    }

                    else

                    {
                        Debug.Log("Please wait; inserting chapters");

                    }

                    if (versionsinserted == true && chaptersinserted  == true && questionsinserted == true)
                    {
                        errorpopup.gameObject.SetActive(true);

                        PopUpMsg.text = "Please find your inspection questionnaires as :" + Environment.NewLine + "a. " + versionscount + " Version inserted." + Environment.NewLine + "b. " + chapterscount + " Chapters inserted." + Environment.NewLine + "c. " + questionscount + " Questions inserted.";
                        //PopUpMsg.text = "Success; Inspection templates received and imported from the server:" + Environment.NewLine + "a. " + versionscount + " Version inserted." + Environment.NewLine + "b. " + chapterscount + " Chapters inserted." + Environment.NewLine + "c. " + questionscount + " Questions inserted.";
                        Checklistversionsupdates.GetComponent<Image>().color = Color.white;

                        foreach (GameObject msg in flashmesgindicators)
                        {
                            msg.SetActive(false);

                        }
                    }

                    else

                    {
                        errorpopup.gameObject.SetActive(true);
                        PopUpMsg.text = "Failed; There were no templates recieved. Please retry.";
                        //PopUpMsg.text = "Failed; There were no templates recieved from the server or connection disrupted. Please retry.";

                    }
                    Debug.Log("Data Added");
                }
              //  mlocationDb.close();
            }

            //else //This part was of no use as the POP up message was going ahead to the next If and Else case...
            //{
            //    errorpopup.gameObject.SetActive(true);
            //    PopUpMsg.text = "Your Company key did not return any checklists, please contact your administrator or login to your cloud account and check.";
            //    StartCoroutine(HidePopUp());
            //}

            GetInspections.enabled = true;

            //if (InspectionTemplates.Count>0)
            //{
            //    errorpopup.gameObject.SetActive(true);

            //    string combinedString = string.Join(", ", "New Inspection templates: " + InspectionTemplates);

            //    PopUpMsg.text = combinedString.ToString();
            //    StartCoroutine(HidePopUp());
            //}
            
            //else
                
            //{
            //    errorpopup.gameObject.SetActive(true);
            //    string combinedString = "There were no new templates found.";

            //    PopUpMsg.text = combinedString.ToString();
            //    StartCoroutine(HidePopUp());
            //}

            Loadinspections();
        }
    }

    

    // THIS CODE to be modified for READING DATA FROM LOCAL DB TO Instantiate Inspection Prefabs, no need to call URL or API, direct from DB to Canvas.
 
    public void Loadinspections()
    {
        
        Inspectionname = "";
        Inspecteddate = "";
        versiondetails = "";
        Chaptername = "";
        Questions = "";
        Cloud_DB_ID = 0;
        Cloud_DB_ParentID = "";

        LoadTemplates.GetComponent<Image>().color = new Color (0.172f, 0.168f,0.388f, 0.86f);
        BlueObjects.SetActive(false);
        WhiteObjects.SetActive(true);
        DraftInsp.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f); //239,239,223,255

        DraftBlueObjects.SetActive(true);
        DraftWhiteObjects.SetActive(false);

        Histbtn.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);
        HistBlueObjects.SetActive(true);
        HistWhiteObjects.SetActive(false);

        SynchBtn.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);

        //refreshbtn.enabled = false;
        table_Inspection_template mlocationDb = new table_Inspection_template();
        using var connection = mlocationDb.getConnection();
        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }
        //mlocationDb.getDataByString1("fetchall");
        using System.Data.IDataReader reader = mlocationDb.getdataforreader("Version");

        List<InspectionTemplateEntity> myList = new List<InspectionTemplateEntity>();
        try
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

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);
                Debug.Log(output1);
                outputofsearchresult = output1.ToString();
            }
            reader.Dispose();
            searchresults = JsonUtility.ToJson(myList, true);

            datacount = myList.Count;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during insert: {ex.Message} and at line; {ex.StackTrace}");
            
            throw;
        }

        foreach (var x in myList)
        {
           FromDatabase_Insptemplates DBinspectionDetails = new FromDatabase_Insptemplates();

            DBinspectionDetails.ID = x._ID;
            DBinspectionDetails.Template_ID = x._Inspection_Template_ID;
            DBinspectionDetails.Cloud_DB_ID = x._Cloud_DB_ID;
            //DBinspectionDetails.Cloud_DB_ParentID = x._Cloud_DB_ParentID;

            DBinspectionDetails.Template_Section_Ques = x._Template_Section_Ques;
            DBinspectionDetails.Details_1 = x._Details_1;
            fromdbList.Add(DBinspectionDetails);

            Inspectionname = x._Template_Section_Ques.ToString();// + " Inspection ";
            versiondetails = "Version: " + x._Version_Number.ToString() + " Dated: " + x._Version_Date.ToString();
            Cloud_DB_ID = x._Cloud_DB_ID;
            //Cloud_DB_ParentID = x._Cloud_DB_ParentID;

            //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;
            inspectionaccordion = Instantiate(inspectionsitemprefab);

            inspectionaccordion.transform.SetParent(ParentPanel, false);

            inspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspectionname.ToString();
            inspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();
            inspectionaccordion.transform.Find("AnswerGroup_Details_1").GetComponentInChildren<TextMeshProUGUI>().text = x._Details_1.ToString();
            inspectionaccordion.transform.Find("ROVIQ_List").GetComponentInChildren<TextMeshProUGUI>().text = x._ROVIQSequence_Sire.ToString();
            inspectionaccordion.transform.Find("Cloud_DB").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ID.ToString();
            inspectionaccordion.transform.Find("StartInspection").gameObject.SetActive(true);
            inspectionaccordion.transform.Find("OpenDraftInsp").gameObject.SetActive(false);

            RectTransform rt = inspectionaccordion.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(1060, 250);

            //inspectionaccordion.transform.Find("Cloud_DB_ParentID").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ParentID.ToString();
        }
        Template_Section_Ques = Convert.ToString(Template_Section_Ques);
        //GetStocksData(fromdbList);
        
        countvariousinsp();
    }

    public void loaddraftinspections () //This takes care of draft, new and Published inspections.
    {
        Inspectionname = "";
        Inspecteddate = "";
        versiondetails = "";
        Chaptername = "";
        Questions = "";
        Cloud_DB_ID = 0;
        Cloud_DB_ParentID = "";

        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();
        using System.Data.IDataReader reader = mlocationDb1.getdataforreader("D", "N", "O");

        //refreshbtn.enabled = false;

        LoadTemplates.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);
        BlueObjects.SetActive(true);
        WhiteObjects.SetActive(false);
        DraftInsp.GetComponent<Image>().color = new Color(0.172f, 0.168f, 0.388f, 0.86f);

        DraftBlueObjects.SetActive(false);
        DraftWhiteObjects.SetActive(true);

        
        Histbtn.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);
        HistBlueObjects.SetActive(true);
        HistWhiteObjects.SetActive(false);


        SynchBtn.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);

        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }
                

        List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();

        while (reader.Read())
        {
            Inspection_PrimaryDetailsEntity entity1 = new Inspection_PrimaryDetailsEntity(int.Parse(reader[0].ToString()),
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
            myList.Add(entity1);

            var output2 = JsonUtility.ToJson(entity1, true);
            Debug.Log(output2);
            outputofsearchresult = output2.ToString();
        }
        reader.Dispose();
        searchresults = JsonUtility.ToJson(myList, true);

        datacount = myList.Count;

        foreach (var x in myList)
        {
            FromDatabase_Insptemplates DBinspectionDetails = new FromDatabase_Insptemplates();
            
            Inspectionname =  x._Vessel_Name.Trim() + " - "+ x._Vessel_TYPE.Trim() + Environment.NewLine + "IMO No.: " + x._Vessel_IMO.Trim();
            versiondetails = "Type: " + x._Type.ToString(); //Used here for inspection type
            Inspecteddate = "Dated: " + x._Inspection_Date.ToString() + Environment.NewLine + "By: " + x._Inspector_Name.ToString();
            
            Cloud_DB_ID = int.Parse(x._Checklist_ID.ToString());
            //Cloud_DB_ParentID = x._Cloud_DB_ParentID;

            //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;
            inspectionaccordion = Instantiate(inspectionsitemprefab);

            inspectionaccordion.transform.SetParent(ParentPanel, false);

            inspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspectionname.ToString();
            inspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();
            inspectionaccordion.transform.Find("Thirdheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspecteddate.ToString();
            inspectionaccordion.transform.Find("Cloud_DB").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ID.ToString();
            inspectionaccordion.transform.Find("OpenDraftInsp/PrimaryDetailsID").GetComponentInChildren<TextMeshProUGUI>().text = x._ID.ToString();
            inspectionaccordion.transform.Find("OpenDraftInsp/InspectionType").GetComponentInChildren<TextMeshProUGUI>().text = x._Type.ToString();
            inspectionaccordion.transform.Find("ROVIQ_List").GetComponentInChildren<TextMeshProUGUI>().text = x._Insp_Details_2.ToString();
            inspectionaccordion.transform.Find("StartInspection").gameObject.SetActive(false);
            inspectionaccordion.transform.Find("OpenDraftInsp").gameObject.SetActive(true);
            //inspectionaccordion.transform.Find("Cloud_DB_ParentID").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ParentID.ToString();
        }
        //Template_Section_Ques = Convert.ToString(Template_Section_Ques);
        // GetStocksData(fromdbList);
        //mlocationDb1.close();
        countvariousinsp();
    }

    //IEnumerator Inspectionstemplateresponse() 
    //{
    //    Inspectionname = "";
    //    versiondetails = "";
    //    Chaptername = "";
    //    Questions = "";
    //    Cloud_DB_ID = 0;
    //    Cloud_DB_ParentID = "";

    //UnityWebRequest request = UnityWebRequest.Get(url);
    //    yield return request.SendWebRequest();

    //    if (request.isNetworkError || request.isHttpError)
    //    {
    //        Debug.Log(request.error + "There was a error");

    //        GetInspections.enabled = true;
    //        errorpopup.gameObject.SetActive(true);
    //        PopUpMsg.text = "Oops !! Something went wrong, please check your internet connection or try again.";
    //        StartCoroutine(HidePopUp());
    //    }
    //    else
    //    {
    //        var Inspectionstemplatedata = JsonConvert.DeserializeObject<InspectionTemplatesMainClass>(request.downloadHandler.text);

    //        foreach (Version inspection in Inspectionstemplatedata.Version)
    //        {
    //            if (inspection.Template_Section_Ques == null || inspection.Version_Number == null)
    //            {
    //                Debug.Log("Empty..");
    //            }

    //            else
    //            {
    //                Inspectionname = inspection.Template_Section_Ques.ToString();
    //                versiondetails = "Version No.; " + inspection.Version_Number.ToString() + " Date; " + inspection.Version_Date.ToString();
    //                Cloud_DB_ID = inspection.Cloud_DB_ID;
    //                Cloud_DB_ParentID = inspection.Cloud_DB_ParentID;

    //                //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;
    //                inspectionaccordion = Instantiate(inspectionsitemprefab);

    //                inspectionaccordion.transform.SetParent(ParentPanel, false);

    //                inspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspectionname.ToString();
    //                inspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();
    //                inspectionaccordion.transform.Find("Cloud_DB").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ID.ToString();
    //                inspectionaccordion.transform.Find("Cloud_DB_ParentID").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ParentID.ToString();

    //            }

    //        }

    //        foreach (Chapter chapter in Inspectionstemplatedata.Chapters)
    //        {
    //            if (chapter.Template_Section_Ques == null || chapter.Version_Number == null)
    //            {
    //                Debug.Log("Empty..");
    //            }

    //            else
    //            {
    //                Chaptername = chapter.Template_Section_Ques.ToString();
    //                versiondetails = "Cloud DB ParentID No.; " + chapter.Cloud_DB_ParentID.ToString() + " Date; " + chapter.Details_1.ToString();

    //                //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;
    //                inspectionaccordion = Instantiate(inspectionsitemprefab);

    //                inspectionaccordion.transform.SetParent(ParentPanel, false);

    //                inspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Chaptername.ToString();
    //                inspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();

    //            }

    //        }

    //        foreach (Question question in Inspectionstemplatedata.Question)
    //        {
    //            if (question.Template_Section_Ques == null || question.Version_Number == null)
    //            {
    //                Debug.Log("Empty..");
    //            }

    //            else
    //            {
    //                Questions = question.Template_Section_Ques.ToString() +" " + question.Details_1.ToString();
    //                versiondetails = "Cloud DB ParentID No.; " + question.Cloud_DB_ParentID.ToString();

    //                //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;
    //                inspectionaccordion = Instantiate(inspectionsitemprefab);

    //                inspectionaccordion.transform.SetParent(ParentPanel, false);

    //                inspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Questions.ToString();
    //                inspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();

    //            }

    //        }
    //        GetInspections.enabled = true;
    //        //industriesprefabs = GameObject.FindGameObjectsWithTag("stocksbutton");

    //        //foreach (GameObject go in industriesprefabs)
    //        //{
    //        //    Stocksrefreshbtn = go.GetComponent<Button>();
    //        //    Stocksrefreshbtn.onClick.Invoke();

    //        //}

    //    }
    //}

    public void loadPublishedinspections() //This takes care of draft, new and Published inspections.
    {
        Inspectionname = "";
        Inspecteddate = "";
        versiondetails = "";
        Chaptername = "";
        Questions = "";
        Cloud_DB_ID = 0;
        Cloud_DB_ParentID = "";

        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();
        using System.Data.IDataReader reader = mlocationDb1.getdataforpublished("P");
        //refreshbtn.enabled = false;

        LoadTemplates.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);
        BlueObjects.SetActive(true);
        WhiteObjects.SetActive(false);
        DraftInsp.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);

        DraftBlueObjects.SetActive(true);
        DraftWhiteObjects.SetActive(false);

        Histbtn.GetComponent<Image>().color = new Color(0.172f, 0.168f, 0.388f, 0.86f);
        HistBlueObjects.SetActive(false);
        HistWhiteObjects.SetActive(true);


        SynchBtn.GetComponent<Image>().color = new Color(0.255f, 0.255f, 0.255f, 0.255f);

        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }


        

        List<Inspection_PrimaryDetailsEntity> myList = new List<Inspection_PrimaryDetailsEntity>();

        while (reader.Read())
        {
            Inspection_PrimaryDetailsEntity entity1 = new Inspection_PrimaryDetailsEntity(int.Parse(reader[0].ToString()),
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
            myList.Add(entity1);

            var output2 = JsonUtility.ToJson(entity1, true);
            Debug.Log(output2);
            outputofsearchresult = output2.ToString();
        }
        reader.Dispose();
        searchresults = JsonUtility.ToJson(myList, true);

        datacount = myList.Count;

        foreach (var x in myList)
        {
            FromDatabase_Insptemplates DBinspectionDetails = new FromDatabase_Insptemplates();

            Inspectionname = x._Vessel_Name.Trim() +" - "+x._Vessel_TYPE.Trim()+ Environment.NewLine + "IMO No.: " + x._Vessel_IMO.Trim();
            versiondetails = x._Type.ToString()+ " Inspection"; //Used here for inspection type
            Inspecteddate = "Inspected on: " + x._Inspection_Date.ToString() + Environment.NewLine +"By: " + x._Inspector_Name.ToString();

            Cloud_DB_ID = int.Parse(x._Checklist_ID.ToString());
            //Cloud_DB_ParentID = x._Cloud_DB_ParentID;

            //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;

            
            publishedinspectionaccordion = Instantiate(publishedinspectionsmprefab);

            publishedinspectionaccordion.transform.SetParent(ParentPanel, false);

            publishedinspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspectionname.ToString();
            publishedinspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();
            publishedinspectionaccordion.transform.Find("Thirdheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspecteddate.ToString();
            publishedinspectionaccordion.transform.Find("Cloud_DB").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ID.ToString();
            publishedinspectionaccordion.transform.Find("OpenDraftInsp/PrimaryDetailsID").GetComponentInChildren<TextMeshProUGUI>().text = x._ID.ToString();
            publishedinspectionaccordion.transform.Find("OpenDraftInsp/InspectionType").GetComponentInChildren<TextMeshProUGUI>().text = x._Type.ToString();
            publishedinspectionaccordion.transform.Find("ROVIQ_List").GetComponentInChildren<TextMeshProUGUI>().text = x._Insp_Details_2.ToString();
            publishedinspectionaccordion.transform.Find("StartInspection").gameObject.SetActive(false);
            publishedinspectionaccordion.transform.Find("OpenDraftInsp").gameObject.SetActive(true);
            //inspectionaccordion.transform.Find("Cloud_DB_ParentID").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ParentID.ToString();
        }
        //Template_Section_Ques = Convert.ToString(Template_Section_Ques);
        // GetStocksData(fromdbList);
        connection.Close();
        countvariousinsp();
    }

    public void countvariousinsp()
    {
        
        templatesCount.text = "";
        draftinspCount.text = "";
        publishedinspCount.text = "";

       table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();

        table_Inspection_template mlocationDb2 = new table_Inspection_template();
        using var connection1 = mlocationDb2.getConnection();

        string query1 = "Inspection_template" + " WHERE " + "Details_8 like '%Version%' ";
        templatesCount.text = "("+ mlocationDb2.totalRecords(query1).ToString()+")";


        string query2 = "Inspection_PrimaryDetails" + " WHERE " + "TRIM(Status) = 'D' or TRIM(Status) = 'N' or TRIM(Status) = 'O'";
        draftinspCount.text = "(" + mlocationDb1.totalRecords(query2).ToString() + ")";

        string query3 = "Inspection_PrimaryDetails" + " WHERE " + "TRIM(Status) = 'P' ";
        publishedinspCount.text = "(" + mlocationDb1.totalRecords(query3).ToString() + ")";

        
    }

    IEnumerator HidePopUp()
    {
        time = 5.0f;
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }

}

public static class ColorHelper
{
    public static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        if (hex.Length == 6)
        {
            hex += "FF"; // Add alpha if not present
        }

        if (hex.Length != 8)
        {
            throw new System.ArgumentException("Hexadecimal color must be 6 or 8 characters long.");
        }

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, a);
    }
}