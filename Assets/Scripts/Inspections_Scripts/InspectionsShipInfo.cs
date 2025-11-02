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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public class InspectionsShipInfo : MonoBehaviour
{
    public TMP_InputField Vesselname;
    public TMP_InputField IMOnumber;

    public TextMeshProUGUI InspType;
    public TextMeshProUGUI ChklstID;

    public TMP_Dropdown VesselType;
    public string VslTypeValue;
    public TMP_InputField   InspectorQualifications;
    public TMP_InputField   VesselExtra2;
    public TMP_InputField   VesselExtra3;

    public TextMeshProUGUI SavedInspPrimaryTableID;
    public TextMeshProUGUI AnswerGroup_Model;
    public TextMeshProUGUI ROVIQList;
    
    public TMP_InputField StartDate; //Opening Meeting Date and Time
    public TMP_InputField InspectorName;
    public TMP_InputField InspectingCompany;
    //public TMP_InputField Location_Port_Sea; NOT REQUIRED
    public TMP_InputField Inspcompleteddate; //Closing Meeting Date and Time
    public TMP_InputField Port;
    public TMP_InputField Country;
    public TMP_InputField OpeningDatetimeFm;
    public TMP_InputField OpeningDatetimeTo;
    public TMP_InputField ClosingDatetimeFm;
    public TMP_InputField ClosingDatetimeTo;
    public TMP_InputField Mastername;
    public TMP_InputField ChiefEngName;
    public TMP_InputField ChiefOffName;
    public TMP_InputField SecondEngrName;
    public TMP_InputField Comments;
    public Button SaveinspectionInfo;
    public Button UpdateinspectionInfo;
    public GameObject updatebtntext;
    public TextMeshProUGUI InpspIDtoUpdate;
    public Button StartInspection;
    public GameObject startbtnImg;
    public TextMeshProUGUI startbtntext;

    public int ID;
    string Location_Port_Sea;
    string Status; // N = New, D = Draft, P = Published, Z = Deleted, O = Re-opened....
    string Insp_Details_1; //Used for Answer Group Model (the number identifies which group to activate; Yes/No, or Good/Fair or Minor/Major NC)
    string Insp_Details_2; //USED For ROVIQ divisions for this inspection.
    string Insp_Details_3; //Used for standardphotosinit
    string Insp_Details_4; //Used for Inspection Folder Name.
    string Timestamp;

    public LoadInspBlock loadinspections;
    public CanvasPositionsMgr canvaspositions;

    public GameObject[] mandatoryfields;
    // public List<string> mandatoryinput = new List<string>();
    String mandatoryinputtext = "";
    String inputresult = "";

    public GameObject errorpopup;
    
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    //public string SavedinfoID;

    public string outputofsearchresult;
    public string searchresults;
    public int datacount;
    public bool MandatoryCompleted;

    public CreatingObsTableForInsp Obstable;
    int SaveOrUpdate;

    public string path; //Media and Files attachment path.

    // For Inspection Preview Section
    public string Folderpath;
    public TextMeshProUGUI Shipsname;
    public TextMeshProUGUI ShipImo;
    public TextMeshProUGUI InspectType;
    public TextMeshProUGUI Inspectypeforchecklistarea;
    public TextMeshProUGUI InspectDatFmTo;
    public TextMeshProUGUI Openingmeeting;
    public TextMeshProUGUI InspectorsName;
    public TextMeshProUGUI InspectorsQual;
    public TextMeshProUGUI InspectingCompanysName;
    public TextMeshProUGUI MasterName;
    public TextMeshProUGUI ChiefEng;
    public TextMeshProUGUI ChiefOffr;
    public TextMeshProUGUI SecondEngr;

    public TextMeshProUGUI TotalAnswered;
    public TextMeshProUGUI TotalObs;

    int answergroup;
    public TextMeshProUGUI LabelGeneral;
    public TextMeshProUGUI Label1;
    public TextMeshProUGUI Label2;
    public TextMeshProUGUI Label3;
    public TextMeshProUGUI Label4;
    public TextMeshProUGUI Label5;

    public TextMeshProUGUI LabelGeneralCount;
    public TextMeshProUGUI Label1Count;
    public TextMeshProUGUI Label2Count;
    public TextMeshProUGUI Label3Count;
    public TextMeshProUGUI Label4Count;
    public TextMeshProUGUI Label5Count;
    public GameObject Label5holder;
    public GameObject Yes;
    
    public GameObject Yestodraft;
    public GameObject Cancel;

    public TextMeshProUGUI LowRiskCount;
    public TextMeshProUGUI MedRiskCount;
    public TextMeshProUGUI HighRiskCount;

    public TextMeshProUGUI HumanCount;
    public TextMeshProUGUI ProcessCount;
    public TextMeshProUGUI HardwareCount;

    public TextMeshProUGUI CorequesCount;
    public TextMeshProUGUI Rotation1Count;
    public TextMeshProUGUI Rotation2Count;

    public GameObject ROVIQbtn;
    public Image ROVIQbtnColour;

    public GameObject loadchecklistbtn;

    string ROVIQMasters;
    int questionsDBcount;
    int answeredDBcount;
    int generalDBCount;
    public string synchronisationfilename;
    public string zipfilename;
    public string inspectionrowcreated;
    string generalinspectiondetails;
    string inspectionsprimarydetails;
    string inspectionsObservations;
    string ObservationAttachments;
    public string FlagForSynch;

    public SynchDataManager synchroniserdata;

    public Image[] imagesPieChart;
    public float[] values;

    public Image[] imagesRiskPieChart;
    public float[] valuesrisk;

    public GameObject Sire20AnalysisPanel;
    public string InspectionTypeforSire20;

    public Image[] imagesCategoryPieChart;
    public float[] valuescategories;

    public Image[] imagesQuesPieChart;
    public float[] valuesquestions;

    public Texture NewTexture;
    private RawImage img;

    public RawImage Ship_RawImage;
    public GameObject defaulticon;

    //ANALYSIS PIE CHARTS
    

    float TotalAnsSum = 0;

    public GameObject Publishedinspectionsbuttons;
    public GameObject NewOrDraftinspectionsbuttons;

    public bool existinginsp;
    public TextMeshProUGUI SynchDatabaseID;
    string folderstozip;
    //public List <int> ObservationIds;
    public GameObject ShipImage;
    public GameObject ShipImageDisabledTxt;
    public GameObject DraftMode;
    public GameObject Published;

    public TextMeshProUGUI BalanceTokens;
    public GameObject BalanceTokensFreetext;
    public int tokensused;
    int loginuserid;
    public GameObject Reopened; //TO BE WORKED UPN............Reopened.SetActive(false);
    public bool reopenedinspection;
    public TMP_InputField[] inputFields;

    public UserProfilePage Userprofilefunctions;
    public string msgtoserver;

    public SendGridEmailSender emailsender;
    
    public Button GeneratePDFReport;
    public GameObject pdffilegenerate;
    public GameObject MicBtnForSummary; // THE MIC IS SET ACTIVE / FALSE ON Clicking the page 2 of the information, where it sets it active for other than Android.

    List<int> obsids;

    void Start()
    {
        msgtoserver = "";
        inputFields = new TMP_InputField[] {Vesselname,IMOnumber,InspectorName, InspectorQualifications, InspectingCompany, Port, Country, Mastername, ChiefEngName,ChiefOffName, SecondEngrName,Comments };
        DraftMode.SetActive(false);
        Published.SetActive(false);
        Reopened.SetActive(false);
        loadinspections = new LoadInspBlock();
        //using table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        MandatoryCompleted = false;
        Vesselname.text = "";
        IMOnumber.text = "";
        InspType.text = "";
        VslTypeValue = "";
        //VesselType.ClearOptions();
        VesselType.value = 0;
        InspectorQualifications.text = "";
        VesselExtra2.text = "";
        VesselExtra3.text = "";
        InspectType.text = "";
        InspectionTypeforSire20 = "";
        Inspectypeforchecklistarea.text = "";
        ChklstID.text = "";
        SavedInspPrimaryTableID.text = "";
        AnswerGroup_Model.text = "";
        ROVIQList.text = "";
        
        StartDate.text = "";
        InspectorName.text = "";
        InspectingCompany.text = "";
        Location_Port_Sea = "-";
        Port.text = "";
        Country.text = "";
        Inspcompleteddate.text = "";
        OpeningDatetimeFm.text = "";
        OpeningDatetimeTo.text = "";
        ClosingDatetimeFm.text = "";
        ClosingDatetimeTo.text = "";
        Mastername.text = "";
        ChiefEngName.text = "";
        ChiefOffName.text = "";
        SecondEngrName.text = "";
        Comments.text = "";
        Status = "";
        Insp_Details_1 = "";
        Insp_Details_2 = "";
        Insp_Details_3 = "";
        Insp_Details_4 = "";
        Timestamp = "";
        //mandatoryinput.Clear();
        mandatoryinputtext = "";
        inputresult = "";
        time = 5.0f;
        //path = "";
        SaveOrUpdate = 0;

        answergroup = 0;
        LabelGeneral.text = "";
        Label1.text = "";
        Label2.text = "";
        Label3.text = "";
        Label4.text = "";
        Label5.text = "";
        LabelGeneralCount.text = "";
        Label1Count.text = "";
        Label2Count.text = "";
        Label3Count.text = "";
        Label4Count.text = "";
        Label5Count.text = "";
        LowRiskCount.text = "";
        MedRiskCount.text = "";
        HighRiskCount.text = "";
        
        Sire20AnalysisPanel.SetActive(false);
        InspectionTypeforSire20 = "";
        HumanCount.text = "";
        ProcessCount.text = "";
        HardwareCount.text = "";

        CorequesCount.text = "";
        Rotation1Count.text = "";
        Rotation2Count.text = "";

        Folderpath = "";
        questionsDBcount = 0;
        answeredDBcount = 0;
        generalDBCount = 0;
        inspectionrowcreated = "";
        Yes.SetActive(false);
        Yestodraft.SetActive(false);
        //Cancel.SetActive(false);
        //SavedinfoID = "";
        ROVIQMasters = "";
        ROVIQbtn.SetActive(true);
        //ObservationIds = null;
        existinginsp = false;
        ShipImage = GameObject.FindGameObjectWithTag("shipphotouploadbtn");
        

        if (ID == 0 || String.IsNullOrEmpty(SavedInspPrimaryTableID.text.ToString()))
        {
            StartInspection.enabled = false;
            startbtnImg.GetComponent<Image>().color = Color.grey;
            startbtntext.GetComponent<TextMeshProUGUI>().color = Color.grey;
        }
        else
        {
            StartInspection.enabled = true;
            startbtnImg.GetComponent<Image>().color = Color.white;
            startbtntext.GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        table_LoginConfig mLocationDb = new table_LoginConfig();
        using var connection = mLocationDb.getConnection();
        mLocationDb.getLatestID();

        //string url = "https://inspectnau.orionmarineconcepts.com/loginpage.aspx";
        
        if (mLocationDb.tokenbalance < 0 && !mLocationDb.useremail.Trim().Contains("administrator")) //FREE CREDITS MODE
        {
            BalanceTokensFreetext.SetActive(true);
            BalanceTokensFreetext.GetComponent<TMP_Text>().text= Math.Abs(mLocationDb.tokenbalance).ToString();
            BalanceTokens.text = mLocationDb.tokenbalance.ToString();// + " Credits";
            BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
            int interactDistance = 0;
            
            connection.Close();

            ////SENDING EMAIL CODE FOR WORKING TO SEND THE CREDITS REMAINING ETC....

            //emailsender.toEmail = mLocationDb.useremail.ToString(); // Replace with the recipient's email
            //emailsender.subject = "InspectNAU APP Low Credits Remaining Notification!";


            //emailsender.body = "Hi " + mLocationDb.name.ToString() + "." + Environment.NewLine + Environment.NewLine + "You have only '" + Math.Abs(mLocationDb.tokenbalance).ToString() + "' free credits remaining. " + Environment.NewLine + "We recommend you to always maintain sufficient credits to be able to publish inspections, especially since this requires internet connection, you can manage your credits from; " + $"<a href='{url}'>Click here to login</a>" + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";
            //connection.Close();
            //emailsender.OnSendEmailButtonClicked();
        }

        else if (mLocationDb.tokenbalance >= 0 && mLocationDb.tokenbalance <= 2 && !mLocationDb.useremail.Trim().Contains("administrator"))
        {
            BalanceTokensFreetext.SetActive(true);
            BalanceTokensFreetext.GetComponent<TMP_Text>().text = Math.Abs(mLocationDb.tokenbalance).ToString();
            BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.red;
            BalanceTokens.text = mLocationDb.tokenbalance.ToString();// + " Credits";
            BalanceTokens.GetComponent<TMP_Text>().color = Color.white;

            connection.Close();

            //emailsender.toEmail = mLocationDb.useremail.ToString(); // Replace with the recipient's email
            //emailsender.subject = "InspectNAU APP Low Credits Remaining Notification!";

            //emailsender.body = "Hi " + mLocationDb.name.ToString() + "." + Environment.NewLine + Environment.NewLine + "You have only '" + mLocationDb.tokenbalance.ToString() + "' credits remaining. " + Environment.NewLine + "We recommend you to always maintain sufficient credits to be able to publish inspections, especially since this requires internet connection, you can manage your credits from; " + $"<a href='{url}'>Click here to login</a>" + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";
            //connection.Close();
            //emailsender.OnSendEmailButtonClicked();
        }

        else if (!mLocationDb.useremail.Trim().Contains("administrator"))
        {
            BalanceTokensFreetext.SetActive(true);
            BalanceTokensFreetext.GetComponent<TMP_Text>().text = Math.Abs(mLocationDb.tokenbalance).ToString();
            BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.black;
            BalanceTokens.text = mLocationDb.tokenbalance.ToString();
            BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
            connection.Close();

        }


        tokensused = 0;
        Reopened.SetActive(false);
        Reopened.gameObject.GetComponent<TMP_Text>().text = "";
        reopenedinspection = false;
        
        //mLocationDb.close();
    }

    public void saveinsprimaryinfo()
    {
        SaveOrUpdate = 0;
        MandatoryCompleted = false;
        if (String.IsNullOrEmpty(Vesselname.text.ToString()) || String.IsNullOrEmpty(IMOnumber.text.ToString()) || IMOnumber.text.ToString().Length < 7)
        {

            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Vessel name and IMO number (Seven Digits) are required as a minimum for saving an inspection in draft mode.";
            StartCoroutine(HidePopUp());

        }

        else
        {
            SaveOrUpdate = 1;
            DatetimeChecks();
        }
    }

    public void ToggleMobileInput()
    {

#if UNITY_IOS || UNITY_ANDROID
bool pencilactive = false;
       //  Toggle the shouldHideMobileInput on mobile devices only

        foreach (TMP_InputField input in inputFields)
        {
            if (input.shouldHideMobileInput)
            {
                input.shouldHideMobileInput = false;
                //Debug.Log("MADE IT FALSE");
                pencilactive = true;
            }

            else 
            {
                input.shouldHideMobileInput = true;
                //Debug.Log("MADE IT TRUE");
               pencilactive = false;
            }
        }
        if (pencilactive == true)
        {
         errorpopup.gameObject.SetActive(true);
        PopUpMsg.text = "Pen mode activated.";
        }
        else
        {
         errorpopup.gameObject.SetActive(true);
        PopUpMsg.text = "De-activated pen mode.";
        }

#else

        Debug.Log("This feature is only available on mobile platforms.");
#endif
        StartCoroutine(HidePopUp());
    }

    public void DatetimeChecks()
    {
        bool Go;
        Go = true;

        if (!String.IsNullOrEmpty(OpeningDatetimeFm.text) && !String.IsNullOrEmpty(OpeningDatetimeTo.text))
        {
            DateTime OpeningDateTimeFm = Convert.ToDateTime(OpeningDatetimeFm.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime OpeningDateTimeTo = Convert.ToDateTime(OpeningDatetimeTo.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

            if (OpeningDateTimeFm > OpeningDateTimeTo)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Date and Time Logical error; Please check your entries for opening meeting start cannot be ahead of stop time.";
                Go = false;
            }
        }

        if (!String.IsNullOrEmpty(ClosingDatetimeFm.text) && !String.IsNullOrEmpty(ClosingDatetimeTo.text))
        {
            DateTime ClosingDateTimeFm = Convert.ToDateTime(ClosingDatetimeFm.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime ClosingDateTimeTo = Convert.ToDateTime(ClosingDatetimeTo.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

            if (ClosingDateTimeFm > ClosingDateTimeTo)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Date and Time Logical error; Please check your entries for closing meeting start cannot be ahead of stop time.";
                Go = false;
            }

        }

        if ((!String.IsNullOrEmpty(OpeningDatetimeFm.text) && !String.IsNullOrEmpty(OpeningDatetimeTo.text)) && (!String.IsNullOrEmpty(ClosingDatetimeFm.text) && !String.IsNullOrEmpty(ClosingDatetimeTo.text)))
        {
            DateTime OpeningDateTimeFm = Convert.ToDateTime(OpeningDatetimeFm.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime OpeningDateTimeTo = Convert.ToDateTime(OpeningDatetimeTo.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime ClosingDateTimeFm = Convert.ToDateTime(ClosingDatetimeFm.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            DateTime ClosingDateTimeTo = Convert.ToDateTime(ClosingDatetimeTo.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

            if (OpeningDateTimeTo > ClosingDateTimeFm || OpeningDateTimeTo > ClosingDateTimeTo)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Date and Time Logical error; Please check your entries for opening and closing meetings.";
                Go = false;
            }
        }

        if (!String.IsNullOrEmpty(OpeningDatetimeFm.text))
        {
            DateTime OpeningDateTimeFm = Convert.ToDateTime(OpeningDatetimeFm.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
          
            if (OpeningDateTimeFm > DateTime.Now)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Opening Meeting Start Date and Time cannot be greater than "+ DateTime.Now + " (current date and time).";
                Go = false;
            }
        }

        if (!String.IsNullOrEmpty(OpeningDatetimeTo.text))
        {
            DateTime OpeningDateTimeTo = Convert.ToDateTime(OpeningDatetimeTo.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            
            if (OpeningDateTimeTo > DateTime.Now)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Opening Meeting Closing Date and Time cannot be greater than " + DateTime.Now + " (current date and time).";
                Go = false;
            }
        }

        if (!String.IsNullOrEmpty(ClosingDatetimeFm.text))
        {
            DateTime ClosingDateTimeFm = Convert.ToDateTime(ClosingDatetimeFm.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
            
            if (ClosingDateTimeFm > DateTime.Now)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Closing Meeting start Date and Time cannot be greater than " + DateTime.Now + " (current date and time).";
                Go = false;
            }
        }

        if (!String.IsNullOrEmpty(ClosingDatetimeTo.text))
        {
            DateTime ClosingDateTimeTo = Convert.ToDateTime(ClosingDatetimeTo.text.ToString(), CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);

            if (ClosingDateTimeTo > DateTime.Now)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Closing Meeting end Date and Time cannot be greater than " + DateTime.Now + " (current date and time).";
                Go = false;
            }
        }

        if (Go == true)
        {
            if (SaveOrUpdate == 1)
            {
                InsertData();

            }

            else if (SaveOrUpdate == 2)
            {
                UpdateData();
            }
        }

        StartCoroutine(HidePopUp());
    }

    public void InsertData()
    {
        //SavedinfoID = "";
        ID = 0;

        Status = "N";
        MandatoryCompleted = false;
        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();

        if (VesselType.value != 0)
        {
            VslTypeValue = VesselType.options[VesselType.value].text;
        }

        else
        {
            VslTypeValue = "-";
        }
        ID = mlocationDb.LatestId + 1;

        mlocationDb.addData(new Inspection_PrimaryDetailsEntity
                (ID,
            Vesselname.text.ToString().Replace("'", "''"),
            IMOnumber.text.ToString(),
            InspType.text.ToString(),
            int.Parse(ChklstID.text.ToString()),
            VslTypeValue,
            InspectorQualifications.text.ToString().Replace("'", "''"),
            VesselExtra2.text.ToString(),
            VesselExtra3.text.ToString(),
            StartDate.text.ToString(),
            InspectorName.text.ToString().Replace("'", "''"),
            InspectingCompany.text.ToString().Replace("'", "''"),
            Location_Port_Sea.ToString().Replace("'", "''"),
            Port.text.ToString().Replace("'", "''"),
            Country.text.ToString().Replace("'", "''"),
            OpeningDatetimeFm.text.ToString(),
            OpeningDatetimeTo.text.ToString(),
            ClosingDatetimeFm.text.ToString(),
            ClosingDatetimeTo.text.ToString(),
            Mastername.text.ToString().Replace("'", "''"),
            ChiefEngName.text.ToString().Replace("'", "''"),
            ChiefOffName.text.ToString().Replace("'", "''"),
            SecondEngrName.text.ToString().Replace("'", "''"),
            Comments.text.ToString().Replace("'", "''"),
            Status.ToString(),
            AnswerGroup_Model.text.ToString(),
            ROVIQList.text.ToString(), //ADDED to Insp_Details_2
            Insp_Details_3.ToString(), //ADDED Standard Photos to this
            Insp_Details_4.ToString(),
            Timestamp));

        mlocationDb.getDataByString1("fetchall");
        InpspIDtoUpdate.text = ID.ToString();
        errorpopup.gameObject.SetActive(true);

        SavedInspPrimaryTableID.text = ID.ToString(); //FOR NEW INSPECTIONS THE SAVED ID will be created from Here.
        StartInspection.enabled = true;
        startbtnImg.GetComponent<Image>().color = Color.white;
        startbtntext.GetComponent<TextMeshProUGUI>().color = Color.white;
        SaveinspectionInfo.gameObject.SetActive(false);
        UpdateinspectionInfo.gameObject.SetActive(true);
        updatebtntext.GetComponent<TextMeshProUGUI>().color = Color.white;
        //InspectType.text = InspType.text.ToString();
        //InspectionTypeforSire20 = InspType.text.ToString();
        //Inspectypeforchecklistarea.text = InspType.text.ToString();

        PopUpMsg.text = "New inspection created and information saved successfully. Please continue to complete the other mandatory fields.";
        ShipImage.GetComponent<Button>().interactable = true;
        ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
        StartCoroutine(HidePopUp());
        connection.Close();
        CreateFolders();
    }

    public void CreateFolders()
    {
        string nameforfolder = "";

        if (InspType.text.ToString().Length>=8) //FOLDER NAME FOR THE INSPECTION REDUCED TO FIRST 8 CHARACTERS.
        {
            nameforfolder = InspType.text.ToString().Substring(0, 8);
        }
        else
        {
            nameforfolder = InspType.text.ToString();
        }

        path = "/" + ID.ToString() + "_" + nameforfolder.Trim().Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "") + "_" + DateTime.Now.ToString("s").Replace(":", "");
        Directory.CreateDirectory(Application.persistentDataPath + path);

        Debug.Log("I CAME HERE TO CREATE THE NEW FOLDER FOR THIS INSPECTION");

        //Create sub folders
        //if (!Directory.Exists(Application.persistentDataPath + path + "/" + "MediaFiles"))
        //{
        //    Directory.CreateDirectory(Application.persistentDataPath + path + "/" + "MediaFiles");
        //}

        //if (!Directory.Exists(Application.persistentDataPath + path + "/" + "Filespath"))
        //{
        //    Directory.CreateDirectory(Application.persistentDataPath + path + "/" + "Filespath");
        //}

        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb.getConnection();
        string columndataquery = "Insp_Details_4 = '" + path.ToString() + "' where cast(ID as int)  = " + ID;
       
        mlocationDb.Updatedata(columndataquery);
        connection.Close();
    }

    public void ClearInputNewInsp()
    {
        // THESE three values have been set up here, because this method is called from the AssignCLoudDbiD() method at the start when any inspection template is clicked.
        InspectType.text = InspType.text.ToString();
        InspectionTypeforSire20 = InspType.text.ToString();
        Inspectypeforchecklistarea.text = InspType.text.ToString();

        //BELOW IS TO CLEAR the initial input fields for various details for a inspection...
        Vesselname.text = "";
        IMOnumber.text = "";
        VslTypeValue = "";
        VesselType.value = 0;
        VesselType.interactable = true;
        InspectorQualifications.text = "";
        VesselExtra2.text = "";
        VesselExtra3.text = "";
        StartDate.text = "";
        InspectorName.text = "";
        InspectingCompany.text = "";
        Location_Port_Sea = "-";
        Port.text = "";
        Country.text = "";
        Inspcompleteddate.text = "";
        OpeningDatetimeFm.text = "";
        OpeningDatetimeTo.text = "";
        ClosingDatetimeFm.text = "";
        ClosingDatetimeTo.text = "";
        Mastername.text = "";
        ChiefEngName.text = "";
        ChiefOffName.text = "";
        SecondEngrName.text = "";
        Comments.text = "";
        
        SaveinspectionInfo.gameObject.SetActive(true);
        UpdateinspectionInfo.gameObject.SetActive(false);
        StartInspection.enabled = false;
        startbtnImg.GetComponent<Image>().color = Color.grey;
        startbtntext.GetComponent<TextMeshProUGUI>().color = Color.grey;
        SavedInspPrimaryTableID.text = "";

        Ship_RawImage.GetComponent<RawImage>().texture = null;
        defaulticon.SetActive(true);
    }

    public void startinspection()
    {
        
        UpdateinspectionInfo.enabled = false;
        StartInspection.enabled = false;
        updatebtntext.GetComponent<TextMeshProUGUI>().color = Color.grey;
        startbtntext.GetComponent<TextMeshProUGUI>().color = Color.grey;
        startbtnImg.GetComponent<Image>().color = Color.grey;
       

        inputresult = "";
        PopUpMsg.text = "";
        SaveOrUpdate = 0;
        MandatoryCompleted = false;
        //Check all mandatory tags data completed;
        mandatoryfields = GameObject.FindGameObjectsWithTag("mandatory");
       table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb.getConnection();
        //table_Inspection_Attachments mlocdb1 = new table_Inspection_Attachments();
        //using var connection1 = mlocdb1.getConnection();
        //mlocdb1.close();
        mlocationDb.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

        path = mlocationDb.folderpath.ToString();

        Status = mlocationDb.status.ToString();
        ROVIQMasters = mlocationDb.ROVIQMasters.ToString();
        Insp_Details_3 = mlocationDb.standardphotos.ToString();
        //mlocationDb.close();
        //table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        //mlocationdb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

        //ROVIQMasters = mlocationdb1.ROVIQMasters.ToString();
        //Insp_Details_3 = mlocationdb1.standardphotos.ToString();

        List<string> keys = ROVIQMasters.ToString().Split(',').Select(x => x.Trim().ToLower()).ToList();

        if ((keys == null) || ROVIQMasters.ToString() == "")
        {
            ROVIQbtn.SetActive(false);
        }

        else
        {
            ROVIQbtn.SetActive(true);

        }

        Color selectedColor;
        Button btn = loadchecklistbtn.GetComponent<Button>();
        if (ColorUtility.TryParseHtmlString("#3EA8EB", out selectedColor))
        {
            // Apply color to selected and highlighted states
            ColorBlock cb = btn.colors;
            cb.selectedColor = selectedColor;
            cb.highlightedColor = selectedColor;
            btn.colors = cb;

            // Set the button as selected
            EventSystem.current.SetSelectedGameObject(loadchecklistbtn);
        }
        else
        {
            Debug.LogWarning("Invalid hex color format.");
        }

        foreach (GameObject mandatory in mandatoryfields)
        {
            mandatoryinputtext = mandatory.GetComponent<TMP_InputField>().text;
            if (String.IsNullOrEmpty(mandatoryinputtext) || mandatoryinputtext == "")
            {
                //inputresult += mandatory.name.ToString() + "\n";
                //inputresult += mandatory.transform.parent.GetComponent<TextMeshProUGUI>().text.ToString() + "\n";

                inputresult += mandatory.name.ToString() + "\n";
                var Cb1 = mandatory.GetComponent<TMP_InputField>().image;
                Cb1.color = Color.red;

                var MandTxtClr = mandatory.GetComponent<TMP_InputField>().placeholder;
                MandTxtClr.color = Color.black;
            }

        }
        
        if ((String.IsNullOrEmpty(inputresult) || inputresult == "")&& VesselType.value != 0)
        {
            Debug.Log("All Fields Completed...");
            MandatoryCompleted = true;
            Obstable.Inspection_PrimaryDetails_ID = int.Parse(SavedInspPrimaryTableID.text);
            SaveOrUpdate = 2;
            DatetimeChecks();
            
            //updateinsprimaryinfo();
        }
        else
        {
            MandatoryCompleted = false;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.color = Color.red;

            if (VesselType.value == 0)
            {
                PopUpMsg.text = "Please select the vessel type on section 1 of the vessel information..";
            }
            else
            {
                PopUpMsg.text = "General information section is incomplete. Please check both sections 1 and 2 for mandatory fields and try again.";
            }

            StartCoroutine(HidePopUp());
            Debug.Log("OOPS you have not filled all the required fields" + inputresult);
            //LOAD ERROR MESSAGE POPUP HERE.
            Obstable.Inspection_PrimaryDetails_ID = int.Parse(SavedInspPrimaryTableID.text);
            //Obstable.GetChecklistContentNEW();
            //updateinsprimaryinfo();

        }

        connection.Close();
        StartCoroutine(ChangeMandatoryFieldColor());
    }

    public void updateinsprimaryinfo()
    {
        UpdateinspectionInfo.enabled = false;
        StartInspection.enabled = false;
        updatebtntext.GetComponent<TextMeshProUGUI>().color = Color.grey;
        startbtnImg.GetComponent<Image>().color = Color.grey;
        startbtntext.GetComponent<TextMeshProUGUI>().color = Color.grey;

        PopUpMsg.text = "";
        SaveOrUpdate = 0;
        MandatoryCompleted = false;
        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));
        Insp_Details_3 = mlocationDb.standardphotos.ToString();
        Status = mlocationDb.status.ToString();
        path = mlocationDb.folderpath.ToString();
        mlocationDb.close();
        if (String.IsNullOrEmpty(Vesselname.text.ToString()) || String.IsNullOrEmpty(IMOnumber.text.ToString()) || IMOnumber.text.ToString().Length < 7)
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Vessel name and IMO number (Seven Digits) are required as a minimum for saving an inspection in draft mode.";
            StartCoroutine(HidePopUp());

        }
        else
        {
         SaveOrUpdate = 2;
         DatetimeChecks();
        }
        connection.Close();
    }

    public void UpdateData()
    {
        PopUpMsg.text = "";
        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

        if (VesselType.value != 0)
        {
            VslTypeValue = VesselType.options[VesselType.value].text;
        }

        else
        {
            VslTypeValue = "-";
        }

        mlocationDb.Updatedata
                    (" Vessel_Name = '" + Vesselname.text.ToString().Replace("'","''") + "' , Vessel_IMO = '"
                    + IMOnumber.text.ToString() + "' , Type = '" + InspType.text.ToString()
                    + "', Checklist_ID = '" + int.Parse(ChklstID.text.ToString())

                    + "' , Vessel_TYPE = '" + VslTypeValue
                    + "' , Vessel_Extra1 = '" + InspectorQualifications.text.ToString().Replace("'", "''")
                    + "' , Vessel_Extra2 = '" + VesselExtra2.text.ToString()
                    + "' , Vessel_Extra3 = '" + VesselExtra3.text.ToString()

                    + "' , Inspection_Date = '" + StartDate.text.ToString()
                    + "' , Inspector_Name = '" + InspectorName.text.ToString().Replace("'", "''")
                    + "' , Inspector_Company = '" + InspectingCompany.text.ToString().Replace("'", "''")
                    + "' , Location_Port_Sea = '" + Location_Port_Sea.ToString().Replace("'", "''")
                    + "' , Port = '" + Port.text.ToString().Replace("'", "''")
                    + "' , Country = '" + Country.text.ToString().Replace("'", "''")
                    + "' , Opening_Meeting_DatetimeFm = '" + OpeningDatetimeFm.text.ToString()
                    + "' , Opening_Meeting_DatetimeTo = '" + OpeningDatetimeTo.text.ToString()
                    + "' , Closing_Meeting_DatetimeFm = '" + ClosingDatetimeFm.text.ToString()
                    + "' , Closing_Meeting_DatetimeTo = '" + ClosingDatetimeTo.text.ToString()
                    + "' , Master_Name = '" + Mastername.text.ToString().Replace("'", "''")
                    + "' , Chief_Engineer_Name = '" + ChiefEngName.text.ToString().Replace("'", "''")
                    + "' , Chief_Officer_Name = '" + ChiefOffName.text.ToString().Replace("'", "''")
                    + "' , Second_Engr_Name = '" + SecondEngrName.text.ToString().Replace("'", "''")
                    + "' , Comments = '" + Comments.text.ToString().Replace("'", "''")
                    + "' , Status = '" + Status.ToString()
                    + "' , Insp_Details_1 = '" + AnswerGroup_Model.text.ToString()
                    + "' , Insp_Details_2 = '" + ROVIQList.text.ToString()
                    + "' , Insp_Details_3 = '" + Insp_Details_3.ToString()
                    + "' , Insp_Details_4 = '" + path.ToString()
                    + "' , Timestamp = '" + Timestamp + "' where Id = '" + int.Parse(SavedInspPrimaryTableID.text.ToString()) + "'");
        Debug.Log(Timestamp);
        //FOR NEW INSPECTIONS THE "SavedInspPrimaryTableID" will be created from SAVED FUNCTION, incase of inspections in draft the "SavedInspPrimaryTableID"
        //Will already come from background.
        //mlocationDb.close();
        if (MandatoryCompleted == true)
            {
                time = 5.0f;
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Please wait for few seconds, till we load the inspection checklists.";
            ROVIQbtn.GetComponent<Button>().interactable = false;
            
           ROVIQbtnColour.color = Color.grey;
            StartCoroutine(HidePopUp());

            if (MicBtnForSummary.activeSelf)
            {
                MicBtnForSummary.SetActive(false);
            }

            Loadinspections();
         }

            else if (MandatoryCompleted == false)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Information updated successfully (pending mandatory fields, if any highlighted in red).";

                mandatoryfields = GameObject.FindGameObjectsWithTag("mandatory");

                foreach (GameObject mandatory in mandatoryfields)
                {
                mandatoryinputtext = mandatory.GetComponent<TMP_InputField>().text;
                if (String.IsNullOrEmpty(mandatoryinputtext) || mandatoryinputtext == "")
                {
                    var Cb1 = mandatory.GetComponent<TMP_InputField>().image;
                    Cb1.color = Color.red;

                    var MandTxtClr = mandatory.GetComponent<TMP_InputField>().placeholder;
                    MandTxtClr.color = Color.black;
                }
                    else
                {
                    var Cb1 = mandatory.GetComponent<TMP_InputField>().image;
                    Cb1.color = Color.white;

                    var MandTxtClr = mandatory.GetComponent<TMP_InputField>().placeholder;
                    MandTxtClr.color = Color.red;
                }
                }

                StartCoroutine(ChangeMandatoryFieldColor());
                StartCoroutine(HidePopUp());

            }
        connection.Close();
        //mlocationDb.getDataByString1("fetchall");
        //Debug.Log("This came from Update primary information.");

    }

    public void Loadinspections()
    {
        
        if (Status == "N")
        {
            Debug.Log("Status NEW inspection on ID number; " + int.Parse(SavedInspPrimaryTableID.text) + " Decide where to go on the CreatingObsTableForInsp");
            Obstable.inspection_primarydetails_ID = int.Parse(SavedInspPrimaryTableID.text);
            Obstable.GetChecklistContentNEW();
            //canvaspositions.ProceedtoChecklistBtn();
        }
        else if (Status == "D" || Status == "O") //Draft or Re-opened
        {
            Obstable.inspection_primarydetails_ID = int.Parse(SavedInspPrimaryTableID.text);
            Obstable.Loadchapters();
            Debug.Log("Status Draft or Re-Opened inspection on ID number; " + int.Parse(SavedInspPrimaryTableID.text) + " Decide where to go on the CreatingObsTableForInsp");
            //Obstable.GetChecklistContentDRAFT(); CREATE A NEW PROCEDURE FOR LOADING EXISTING ID FROM OBSERVATION TABLE WHICH IS SAVED TO THIS SavedInspPrimaryTableID.
            //canvaspositions.ProceedtoChecklistBtn();
        }
        //loadinspections.Loadinspections(); This is for main templates not the OBSERVATION TABLE.
        MandatoryCompleted = false;
        Status = "";
    }

    IEnumerator ChangeMandatoryFieldColor()
    {
        yield return new WaitForSeconds(time);
        mandatoryfields = GameObject.FindGameObjectsWithTag("mandatory");

        foreach (GameObject mandatory in mandatoryfields)
        {
            var Cb1 = mandatory.GetComponent<TMP_InputField>().image;
            Cb1.color = Color.white;

            var MandTxtClr = mandatory.GetComponent<TMP_InputField>().placeholder;
            MandTxtClr.color = Color.red;
        }
    }

    public void LoadPrimaryInfo()
    {
        Vesselname.text = "";
        IMOnumber.text = "";
        string InspectionType = ""; //Added on 25th DEC only for checking and enabling the Drop Down for Inspections other than RIGHTSHIP & SIRE.
        //VesselType.ClearOptions();
        VesselType.value = 0;  // Set the selected index based on the text
        VesselType.interactable = true;
        VesselType.RefreshShownValue();
        VslTypeValue = "";
        InspectorQualifications.text = "";
        VesselExtra2.text = "";
        VesselExtra3.text = "";

        StartDate.text = "";
        InspectorName.text = "";
        InspectingCompany.text = "";
        Location_Port_Sea = "-";
        Port.text = "";
        Country.text = "";
        Inspcompleteddate.text = "";
        OpeningDatetimeFm.text = "";
        OpeningDatetimeTo.text = "";
        ClosingDatetimeFm.text = "";
        ClosingDatetimeTo.text = "";
        Mastername.text = "";
        ChiefEngName.text = "";
        ChiefOffName.text = "";
        SecondEngrName.text = "";
        Comments.text = "";
        ChklstID.text = "";
        AnswerGroup_Model.text = "";
        ROVIQList.text = "";
        SaveinspectionInfo.gameObject.SetActive(false);
        UpdateinspectionInfo.gameObject.SetActive(true);
        StartInspection.enabled = true;

        updatebtntext.GetComponent<TextMeshProUGUI>().color = Color.white;

        startbtnImg.GetComponent<Image>().color = Color.white;
        startbtntext.GetComponent<TextMeshProUGUI>().color = Color.white;
        //StartInspection.GetComponent<Image>().color = Color.cyan;
        Shipsname.text = "";
        ShipImo.text = "";
        InspectType.text = "";
        Inspectypeforchecklistarea.text = "";
        InspectDatFmTo.text = "";
        Openingmeeting.text = "";
        InspectorsName.text = "";
        InspectorsQual.text = "";
        InspectingCompanysName.text = "";
        MasterName.text = "";
        ChiefEng.text = "";
        ChiefOffr.text = "";
        SecondEngr.text = "";


        Folderpath = "";
        TotalAnswered.text = "";
        TotalObs.text = "";

        answergroup = 0;
        LabelGeneral.text = "";
        Label1.text = "";
        Label2.text = "";
        Label3.text = "";
        Label4.text = "";
        Label5.text = "";

        LabelGeneralCount.text = "";
        Label1Count.text = "";
        Label2Count.text = "";
        Label3Count.text = "";
        Label4Count.text = "";
        Label5Count.text = "";

        values[0] = 0;
        values[1] = 0;
        values[2] = 0;

        imagesPieChart[0].GetComponent<Image>().color = Color.grey;

        LowRiskCount.text = "";
        MedRiskCount.text = "";
        HighRiskCount.text = "";

        valuesrisk[0] = 0;
        valuesrisk[1] = 0;
        valuesrisk[2] = 0;

        imagesRiskPieChart[0].GetComponent<Image>().color = Color.grey;

        Sire20AnalysisPanel.SetActive(false);
        InspectionTypeforSire20 = ""; 

        HumanCount.text = "";
        ProcessCount.text = "";
        HardwareCount.text = "";

        valuescategories[0] = 0;
        valuescategories[1] = 0;
        valuescategories[2] = 0;

        CorequesCount.text = "";
        Rotation1Count.text = "";
        Rotation2Count.text = "";

        valuesquestions[0] = 0;
        valuesquestions[1] = 0;
        valuesquestions[2] = 0;


       table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();
        using System.Data.IDataReader reader = mlocationDb1.getPrimaryInfoData(int.Parse(SavedInspPrimaryTableID.text));

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
            Vesselname.text = entity1._Vessel_Name;
            IMOnumber.text = entity1._Vessel_IMO;
            ChklstID.text = entity1._Checklist_ID.ToString();
            string inspectionstatus = "";
            inspectionstatus = entity1._Status;
            InspectionType = entity1._Type;

            int index = VesselType.options.FindIndex(option => option.text == entity1._Vessel_TYPE.ToString().Trim());

            if (index != -1)
            {
                VesselType.value = index;  // Set the selected index based on the text
                VesselType.RefreshShownValue();  // Refresh the displayed text
            }
            else
            {
                VesselType.value = 0;  // Set the selected index based on the text
                VesselType.RefreshShownValue();  // Refresh the displayed text
                Debug.LogWarning("Option not found!");
            }

            if (inspectionstatus != "N" &&(InspectionType.ToLower().Contains("sire") || InspectionType.ToLower().Contains("rightship"))) // BASICALLY IF THE INSPECTION HAS BEEN SAVED TO DRAFT MODE, THE Questions are copied from the Template table and copied into Observations table after which Vessel Type change is not allowed.
            {
                VesselType.interactable = false;
            }

            else
            {
                VesselType.interactable = true;
            }
            //THIS AREA FEEDS TWO ASPECTS; 1, Load primary information area and PDF, 2. INput boxes at the Inspection General Information Area.

            InspectorQualifications.text = entity1._Vessel_Extra1.ToString();
            VesselExtra2.text = entity1._Vessel_Extra2.ToString();
            VesselExtra3.text = entity1._Vessel_Extra3.ToString();

            AnswerGroup_Model.text = entity1._Insp_Details_1.ToString();
            ROVIQList.text = entity1._Insp_Details_2.ToString();
            StartDate.text = entity1._Inspection_Date;
            InspectorName.text = entity1._Inspector_Name;
            InspectingCompany.text = entity1._Inspector_Company;
            Location_Port_Sea = entity1._Location_Port_Sea;
            Port.text = entity1._Port;
            Country.text = entity1._Country;
            Inspcompleteddate.text = entity1._Closing_Meeting_DateTimeTo;
            OpeningDatetimeFm.text = entity1._Opening_Meeting_DateTimeFm;
            OpeningDatetimeTo.text = entity1._Opening_Meeting_DateTimeTo;
            ClosingDatetimeFm.text = entity1._Closing_Meeting_DateTimeFm;
            ClosingDatetimeTo.text = entity1._Closing_Meeting_DateTimeTo;
            Mastername.text = entity1._Master_Name;
            ChiefEngName.text = entity1._Chief_Engineer_Name;
            ChiefOffName.text = entity1._Chief_Officer_Name;
            SecondEngrName.text = entity1._Second_Engr_Name;
            Comments.text = entity1._Comments;
            Folderpath = entity1._Insp_Details_4.ToString();
            Shipsname.text = entity1._Vessel_Name.ToUpper();
            ShipImo.text = "IMO No. " + entity1._Vessel_IMO;
            InspectType.text = entity1._Type.ToUpper();
            Inspectypeforchecklistarea.text  = entity1._Type.ToUpper();
            InspectionTypeforSire20 = entity1._Type.ToLower().Trim();
            InspectDatFmTo.text = entity1._Inspection_Date;
            Openingmeeting.text = "Opening: "+entity1._Opening_Meeting_DateTimeFm + "\n" + "Closing: " + entity1._Closing_Meeting_DateTimeFm;
            InspectorsName.text = entity1._Inspector_Name;
            InspectorsQual.text = entity1._Vessel_Extra1.ToString();
            InspectingCompanysName.text = entity1._Inspector_Company;
            MasterName.text = entity1._Master_Name;
            ChiefEng.text = entity1._Chief_Engineer_Name;
            ChiefOffr.text = entity1._Chief_Officer_Name;
            SecondEngr.text = entity1._Second_Engr_Name;

            int checkanswergp = 0; bool intResultTryParse = int.TryParse(entity1._Insp_Details_1, out checkanswergp);

            if (String.IsNullOrEmpty(entity1._Insp_Details_1) || intResultTryParse == false)
            {
                answergroup = 0;
            }
                else
            {
                answergroup = int.Parse(entity1._Insp_Details_1);
            }

        }
        reader.Dispose();
        connection.Close();

      
        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection1 = mlocationDb.getConnection();
        table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
        using var connection2 = mlocationDb2.getConnection();

        if (InspectionTypeforSire20.ToLower().Trim().Replace(" ", "").Contains("(sire2.0)"))
        {

            Sire20AnalysisPanel.SetActive(true);

            #region
            //"SuggestedInspectorActions";  // USE For QUESTIONS - CORE/ROTATION1 or ROTATION2 - SIRE 2.0

            string query3 = "SELECT TRIM(SuggestedInspectorActions) AS Questiontype, COUNT(*) AS Count " +
                "FROM Inspection_Observations where Selected_Answer IS NOT NULL AND TRIM(Selected_Answer) != '' " +
                "and trim(Obs_Details_7) = 'Completed' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " " +
                "and TRIM(Obs_Details_8) = 'Question' " + " GROUP BY SuggestedInspectorActions;";

            using System.Data.IDataReader countbyanswerreader3 = mlocationDb.countbyanswer(query3);

            Dictionary<string, int> questyprows = new Dictionary<string, int>();

            while (countbyanswerreader3.Read())
            {
                if (countbyanswerreader3[0].ToString() != "")
                {
                    questyprows.Add(countbyanswerreader3[0].ToString(), int.Parse(countbyanswerreader3[1].ToString()));
                }
            }

            if (questyprows.Keys.Contains("C"))
            {
                valuesquestions[0] = questyprows["C"];
                CorequesCount.text = valuesquestions[0].ToString();
                imagesQuesPieChart[0].GetComponent<Image>().color = Color.blue;

            }

            else
            {
                valuesquestions[0] = 0;
                CorequesCount.text = "-";
                imagesQuesPieChart[0].GetComponent<Image>().color = Color.grey;
            }

            if (questyprows.Keys.Contains("R1"))
            {
                valuesquestions[1] = questyprows["R1"];
                Rotation1Count.text = valuesquestions[1].ToString();
                imagesQuesPieChart[1].GetComponent<Image>().color = Color.magenta;

            }

            else
            {
                valuesquestions[1] = 0;
                Rotation1Count.text = "-";
                imagesQuesPieChart[1].GetComponent<Image>().color = Color.grey;
            }

            if (questyprows.Keys.Contains("R2"))
            {
                valuesquestions[2] = questyprows["R2"];
                Rotation2Count.text = valuesquestions[2].ToString();
                imagesQuesPieChart[2].GetComponent<Image>().color = Color.cyan;

            }

            else
            {
                valuesquestions[2] = 0;
                Rotation2Count.text = "-";
                imagesQuesPieChart[2].GetComponent<Image>().color = Color.grey;
            }
            SetquestypeValues(valuesquestions);

            #endregion

            #region

            //"ShortQuestionText";  // USE For CATEGORY - Human - SIRE 2.0
            //"Publications";       // USE For CATEGORY - Process
            //"IndustryGuidance";   // USE For CATEGORY - Hardware

            //THis was just to count for temp query
            //string Query4 = "Inspection_Observations where ShortQuestionText IS NOT NULL AND TRIM(ShortQuestionText) != '' and Selected_Answer IS NOT NULL AND TRIM(Selected_Answer) != '' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " ;";
            //string Query5 = "Inspection_Observations where Publications IS NOT NULL AND TRIM(Publications) != '' and Selected_Answer IS NOT NULL AND TRIM(Selected_Answer) != '' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " ;";
            //string Query6 = "Inspection_Observations where IndustryGuidance IS NOT NULL AND TRIM(IndustryGuidance) != '' and Selected_Answer IS NOT NULL AND TRIM(Selected_Answer) != '' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " ;";

            //SIRE CATEGORIES HERE....

            string Query4 = "Inspection_Attachments where TRIM(Attachment_Title) like '%HumanElement%' and cast(TRIM(Attachment_Details_2) as int) = '1' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " ;";
            string Query5 = "Inspection_Attachments where TRIM(Attachment_Title) like '%ProcessElement%' and cast(TRIM(Attachment_Details_2) as int) = '1' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " ;";
            string Query6 = "Inspection_Attachments where TRIM(Attachment_Title) like '%HardwareElement%' and cast(TRIM(Attachment_Details_2) as int) = '1' and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " ;";

            //COMPLETED 3rd Sept 2024-- COUNT OF HUMAN, PROCESS & HARDWARE ANSWERS for SIRE 2.0.......

            valuescategories[0] = mlocationDb2.CountbasisQuery(Query4);
            valuescategories[1] = mlocationDb2.CountbasisQuery(Query5);
            valuescategories[2] = mlocationDb2.CountbasisQuery(Query6);


            if (valuescategories[0]>0)
            {
                HumanCount.text = valuescategories[0].ToString();
                imagesCategoryPieChart[0].GetComponent<Image>().color = Color.blue;

            }

            else
            {
                HumanCount.text = "-";
                imagesCategoryPieChart[0].GetComponent<Image>().color = Color.grey;
            }

            if (valuescategories[1] > 0)
            {
                ProcessCount.text = valuescategories[1].ToString();
                imagesCategoryPieChart[1].GetComponent<Image>().color = Color.magenta;

            }

            else
            {
                ProcessCount.text = "-";
                imagesCategoryPieChart[1].GetComponent<Image>().color = Color.grey;
            }

            if (valuescategories[2] > 0)
            {
                HardwareCount.text = valuescategories[2].ToString();
                imagesCategoryPieChart[2].GetComponent<Image>().color = Color.cyan;

            }

            else
            {
                HardwareCount.text = "-";
                imagesCategoryPieChart[2].GetComponent<Image>().color = Color.grey;
            }

            SetCategoryValues(valuescategories);

            #endregion

        }
        else
        {
            Sire20AnalysisPanel.SetActive(false);
        }
        
        string query = "SELECT TRIM(Selected_Answer, ' -ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz') AS Answer, COUNT(*) AS Count FROM Inspection_Observations " +
            "where cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) +" and TRIM(Obs_Details_8) = 'Question' "+" GROUP BY Selected_Answer;";

        using System.Data.IDataReader countbyanswerreader = mlocationDb.countbyanswer(query);

        string query1 = "SELECT TRIM(RiskCategory) AS Risk, COUNT(*) AS Count FROM Inspection_Observations " +
            "where Selected_Answer IS NOT NULL AND TRIM(Selected_Answer) != '' " +
            "AND TRIM(Selected_Answer) NOT like '%999%' AND substr(Selected_Answer, 1, instr(Selected_Answer, '-') - 1) IN('1', '2', '6','10','11','12')" +
            "and cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " " +
            "and TRIM(Obs_Details_8) = 'Question' "+ " GROUP BY RiskCategory;";

        using System.Data.IDataReader countbyanswerreader1 = mlocationDb.countbyanswer(query1);

        Dictionary<string, int> row = new Dictionary<string, int>();

        while (countbyanswerreader.Read())
        {
            if (countbyanswerreader[0].ToString() != "")
            {
               row.Add(countbyanswerreader[0].ToString(), int.Parse(countbyanswerreader[1].ToString()));
            }
        }

        Dictionary<string, int> riskrows = new Dictionary<string, int>();

        while (countbyanswerreader1.Read())
        {
            if (countbyanswerreader1[0].ToString() != "")
            {
                riskrows.Add(countbyanswerreader1[0].ToString(), int.Parse(countbyanswerreader1[1].ToString()));
            }
        }

        string Query1 = "Inspection_Observations where cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " and Trim(Obs_Details_8) = 'Question'";
        string Query2 = "Inspection_Observations where cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " and Trim(Obs_Details_8) = 'Question' and Trim(Selected_Answer) != '' and trim(Obs_Details_7) = 'Completed'";
        string Query3 = "Inspection_Observations where cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(SavedInspPrimaryTableID.text) + " and Trim(Obs_Details_8) = 'Question' and TRIM(Selected_Answer) like '%999%' and trim(Obs_Details_7) = 'Completed'";

        questionsDBcount = mlocationDb.CountbasisQuery(Query1);
        answeredDBcount = mlocationDb.CountbasisQuery(Query2);
        generalDBCount = mlocationDb.CountbasisQuery(Query2);
        TotalAnswered.text = answeredDBcount + "/" + questionsDBcount;

        LabelGeneral.text = "General";
        LabelGeneralCount.text = "";

        #region
        //valuesrisk[0] = 10;
        //valuesrisk[1] = 20;
        //valuesrisk[2] = 30;

        //LowRiskCount.text = valuesrisk[0].ToString();
        //MedRiskCount.text = valuesrisk[1].ToString();
        //HighRiskCount.text = valuesrisk[2].ToString();

        if (riskrows.Keys.Contains("Low"))
        {
            valuesrisk[0] = riskrows["Low"];
            LowRiskCount.text = valuesrisk[0].ToString();
            imagesRiskPieChart[0].GetComponent<Image>().color = Color.cyan;

        }

        else
        {
            valuesrisk[0] = 0;
            LowRiskCount.text = "-";
            imagesRiskPieChart[0].GetComponent<Image>().color = Color.grey;
        }

        if (riskrows.Keys.Contains("Medium"))
        {
            valuesrisk[1] = riskrows["Medium"];
            MedRiskCount.text = valuesrisk[1].ToString();
            imagesRiskPieChart[1].GetComponent<Image>().color = Color.yellow;

        }

        else
        {
            valuesrisk[1] = 0;
            MedRiskCount.text = "-";
            imagesRiskPieChart[1].GetComponent<Image>().color = Color.grey;
        }

        if (riskrows.Keys.Contains("High"))
        {
            valuesrisk[2] = riskrows["High"];
            HighRiskCount.text = valuesrisk[2].ToString();
            imagesRiskPieChart[2].GetComponent<Image>().color = Color.red;
        }

        else
        {
            valuesrisk[2] = 0;
            HighRiskCount.text = "-";
            imagesRiskPieChart[2].GetComponent<Image>().color = Color.grey;
        }

        if (valuesrisk[0] + valuesrisk[1] +valuesrisk[2] == 0) //THIS MEANS THERE IS NO RISK ALLOCATED TO ANY QUESTIONS...
        {
            
            imagesRiskPieChart[0].GetComponent<Image>().color = Color.white;
            imagesRiskPieChart[1].GetComponent<Image>().color = Color.white;
            imagesRiskPieChart[2].GetComponent<Image>().color = Color.white;
        }

        #endregion

        int countof999 = 0;
        if (row.Keys.Contains("999"))
        {
            countof999 = row["999"];
            LabelGeneralCount.text = countof999.ToString();
        }

        else { LabelGeneralCount.text = "-"; }

        if (answergroup == 1)//5,6,7,8
        {
            Label1.text = "Yes:";
            Label2.text = "No:";
            Label3.text = "Not Seen:";
            Label4.text = "N/A:";
            Label5holder.SetActive(false);

            values[2] = 0; //THERE IS NO NEED FOR A 3rd pie in this case, only Yes and No are analysed.
            imagesPieChart[2].GetComponent<Image>().color = Color.white;

            int countof5 = 0;
            if (row.Keys.Contains("5"))
            {
                values[0] = row["5"];
                countof5 = row["5"];
                Label1Count.text = countof5.ToString();
                imagesPieChart[0].GetComponent<Image>().color = Color.green;
            }

            else { Label1Count.text = "-"; }

            int countof6 = 0;
            if (row.Keys.Contains("6"))
            {
                values[1] = row["6"];
                countof6 = row["6"];
                Label2Count.text = countof6.ToString();
                imagesPieChart[1].GetComponent<Image>().color = Color.red;
                //ObservationIds.Add(int.Parse(countof6.ToString()));
            }
            else { Label2Count.text = "-"; }

            int countof7 = 0;
            if (row.Keys.Contains("7"))
            {
                countof7= row["7"];
                Label3Count.text = countof7.ToString();
            }

            else { Label3Count.text = "-"; }

            int countof8 = 0;
            if (row.Keys.Contains("8"))
            {
                countof8 = row["8"];
                Label4Count.text = countof8.ToString();
            }

            else { Label4Count.text = "-"; }

           
        }

        else if (answergroup == 3)//9,10,11,12,13
        {
            Label1.text = "Satisfactory:";
            Label2.text = "Major NC:";
            Label3.text = "Minor NC:";
            Label4.text = "Observation:";
            Label5holder.SetActive(true);
            Label5.text = "Not Seen or N/A:";


            int countof9 = 0;
            if (row.Keys.Contains("9"))
            {
                countof9 = row["9"];
                Label1Count.text = countof9.ToString();
                
            }

            else { Label1Count.text = "-"; }

            int countof10 = 0;
            if (row.Keys.Contains("10"))
            {
                values[0] = row["10"];
                countof10 = row["10"];
                Label2Count.text = countof10.ToString();
                imagesPieChart[0].GetComponent<Image>().color = Color.red;
            }

            else { Label2Count.text = "-"; }

            int countof11 = 0;
            if (row.Keys.Contains("11"))
            {
                values[1] = row["11"];
                countof11 = row["11"];
                Label3Count.text = countof11.ToString();
                imagesPieChart[1].GetComponent<Image>().color = Color.magenta;
            }

            else { Label3Count.text = "-"; }

            int countof12 = 0;
            if (row.Keys.Contains("12"))
            {
                values[2] = row["12"];
                countof12 = row["12"];
                Label4Count.text = countof12.ToString();
                imagesPieChart[2].GetComponent<Image>().color = Color.yellow;
            }

            else { Label4Count.text = "-"; }

            int countof13 = 0;
            if (row.Keys.Contains("13"))
            {
                countof13 = row["13"];
                Label5Count.text = countof13.ToString();
            }

            else { Label5Count.text = "-"; }
        }

        else 
        {
            Label1.text = "Good:";
            Label2.text = "Fair:";
            Label3.text = "Poor:";
            Label4.text = "Not Seen:";
            Label5holder.SetActive(true);
            Label5.text = "N/A:";

            

            int countof0 = 0;
            if (row.Keys.Contains("0"))
            {
                values[0] = row["0"];
                //values[0] = 35;
                countof0 = row["0"];
                Label1Count.text = countof0.ToString();
                imagesPieChart[0].GetComponent<Image>().color = Color.green;
            }

            else { Label1Count.text = "-"; }

            int countof1 = 0;
            if (row.Keys.Contains("1"))
            {
                //values[1] = row["1"];
                values[1] = 2;
                countof1 = row["1"];
                Label2Count.text = countof1.ToString();
                imagesPieChart[1].GetComponent<Image>().color = Color.yellow;
                //ObservationIds.Add(int.Parse(countof1.ToString()));
            }

            else { Label2Count.text = "-"; }

            int countof2 = 0;
            if (row.Keys.Contains("2"))
            {
                values[2] = row["2"];
                //values[2] = 45;
                countof2 = row["2"];
                Label3Count.text = countof2.ToString();
                imagesPieChart[2].GetComponent<Image>().color = Color.red;
                // ObservationIds.Add(int.Parse(countof2.ToString()));
            }

            else { Label3Count.text = "-"; }

            int countof3 = 0;
            if (row.Keys.Contains("3"))
            {
               
                countof3 = row["3"];
                Label4Count.text = countof3.ToString();
               // ObservationIds.Add(int.Parse(countof3.ToString()));
            }

            else { Label4Count.text = "-";  }

            int countof4 = 0;
            if (row.Keys.Contains("4"))
            {
                countof4 = row["4"];
                Label5Count.text = countof4.ToString();
            }

            else { Label5Count.text = "-"; }


            //TotalAnsSum = values[0]+ values[1]+values[2];

            // Debug.Log("These are the observation IDs -->>"+ObservationIds.ToString());
        }


        countbyanswerreader.Dispose();
        countbyanswerreader1.Dispose();

        //mlocationDb.close();
        //mlocationDb1.close();
        //mlocationDb2.close();

        //connection.Close();
        connection1.Close();
        connection2.Close();

        SetValues(values);
        SetRiskValues(valuesrisk);

        //searchresults = JsonUtility.ToJson(myList, true);

        //datacount = myList.Count;
        //  table_Inspection_Observations mlocationDb = new table_Inspection_Observations();


        //TotalAnswered.text = "";
        //SELECT * FROM 'Inspection_Observations' where cast(Inspection_PrimaryDetails_ID as int)= int.Parse(SavedInspPrimaryTableID.text) and Trim(Obs_Details_8) = 'Question'
        //SELECT * FROM 'Inspection_Observations' where cast(Inspection_PrimaryDetails_ID as int)= int.Parse(SavedInspPrimaryTableID.text) and Trim(Obs_Details_8) = 'Question' and Trim(Selected_Answer) != '';

        // TotalObs.text = "--";


        //Highriskobs.text = "-";
        //SELECT* FROM 'Inspection_Observations' where cast(Inspection_PrimaryDetails_ID as int)= int.Parse(SavedInspPrimaryTableID.text) and Trim(Obs_Details_8) = 'Question' and Trim(Selected_Answer) != '' and Trim(RiskCategory) = 'High';

        //MedRiskObs.text = "-";
        //SELECT* FROM 'Inspection_Observations' where cast(Inspection_PrimaryDetails_ID as int)= int.Parse(SavedInspPrimaryTableID.text) and Trim(Obs_Details_8) = 'Question' and Trim(Selected_Answer) != '' and Trim(RiskCategory) = 'Medium';

        //LowRiskObs.text = "-";
        //SELECT* FROM 'Inspection_Observations' where cast(Inspection_PrimaryDetails_ID as int)= int.Parse(SavedInspPrimaryTableID.text) and Trim(Obs_Details_8) = 'Question' and Trim(Selected_Answer) != '' and Trim(RiskCategory) = 'Low';

    }

    public void SetValues(float[] valuesToSet)
    {
        float totalvalues = 0;
        for (int i = 0; i < imagesPieChart.Length; i++)
        {
            totalvalues += FindPercentage(valuesToSet, i);
            imagesPieChart[i].fillAmount = totalvalues;
        }

    }

    public void SetRiskValues(float[] valuesToSet)
    {
        float totalvalues = 0;
        for (int i = 0; i < imagesRiskPieChart.Length; i++)
        {
            totalvalues += FindPercentage(valuesToSet, i);
            imagesRiskPieChart[i].fillAmount = totalvalues;
        }

    }

    public void SetquestypeValues(float[] valuesToSet)
    {
        float totalvalues = 0;
        for (int i = 0; i < imagesQuesPieChart.Length; i++)
        {
            totalvalues += FindPercentage(valuesToSet, i);
            imagesQuesPieChart[i].fillAmount = totalvalues;
        }

    }

    public void SetCategoryValues(float[] valuesToSet)
    {
        float totalvalues = 0;
        for (int i = 0; i < imagesCategoryPieChart.Length; i++)
        {
            totalvalues += FindPercentage(valuesToSet, i);
            imagesCategoryPieChart[i].fillAmount = totalvalues;
        }

    }

    private float FindPercentage(float[] valuesToSet, int index)
    {
        float totalAmount = 0;
        for (int i = 0; i < valuesToSet.Length; i++)
        {
            totalAmount += valuesToSet[i];
        }

        return valuesToSet[index] / totalAmount;
    }

    void Piechartforprint()
    {
        //img = .GetComponent<RawImage>();

        img.GetComponent<RawImage>().texture = (Texture)NewTexture;
    }

    //public void tempquery()

    //{
    //    table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
    //    string columndataquery1 = "ROVIQSequence = 'Documentation, Bridge, Cargo Control Room, Engine Control Room, Interview - Rating' where cast(Cloud_DB_ID as int) = 839 and Obs_Details_8 like ' %Question%'";
    //    string columndataquery2 = "ROVIQSequence = 'Interview - Deck Officer, Interview - Engineer Officer, Documentation, Bridge, Engine Control Room' where cast(Cloud_DB_ID as int) = 847 and Obs_Details_8 like '%Question%'";
    //    string columndataquery3 = "ROVIQSequence = 'Documentation, Cargo Control Room' where cast(Cloud_DB_ID as int) = 848 and Obs_Details_8 like '%Question%'";
    //    string columndataquery4 = "ROVIQSequence = 'Documentation, Cargo Control Room' where cast(Cloud_DB_ID as int) = 429 and Obs_Details_8 like '%Question%'";
    //    string columndataquery5 = "ROVIQSequence = 'Documentation, Bridge, Cargo Control Room, Engine Control Room, Interview - Rating' where cast(Cloud_DB_ID as int) = 438 and Obs_Details_8 like ' %Question%'";
    //    string columndataquery6 = "ROVIQSequence = 'Interview - Deck Officer, Interview - Engineer Officer, Documentation, Bridge, Engine Control Room' where cast(Cloud_DB_ID as int) = 204 and Obs_Details_8 like '%Question%'";

    //    mlocationDb.Updatedata(columndataquery1);
    //    mlocationDb.Updatedata(columndataquery2);
    //    mlocationDb.Updatedata(columndataquery3);
    //    mlocationDb.Updatedata(columndataquery4);
    //    mlocationDb.Updatedata(columndataquery5);
    //    mlocationDb.Updatedata(columndataquery6);


    //}

    public void PublishBtnClicked()

    {
        Userprofilefunctions = new UserProfilePage();
        loginuserid = 0;
        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();
        mlocationDb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text));

        try
        {
            mlocationDb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text));
        }
        finally
        {
            connection.Close(); // Ensure connection is explicitly closed
        }

        table_LoginConfig mLocationDb = new table_LoginConfig();
        var connection1 = mLocationDb.getConnection(); // Explicitly create the second connection
        try
        {
            mLocationDb.getLatestID();
        }
        finally
        {
            connection1.Close(); // Ensure this connection is explicitly closed as well
        }


        int dbtokensbalance = mLocationDb.tokenbalance;
        BalanceTokens.text = dbtokensbalance.ToString();
        tokensused = 0;
        loginuserid = mLocationDb.LatestId;
        
        if (String.IsNullOrEmpty(mlocationDb1.Vessel_Name) || mlocationDb1.Vessel_Name==""||
            String.IsNullOrEmpty(mlocationDb1.Vessel_IMO) ||mlocationDb1.Vessel_IMO ==""||
            String.IsNullOrEmpty(mlocationDb1.Vessel_TYPE) || mlocationDb1.Vessel_TYPE == "" || mlocationDb1.Vessel_TYPE.Trim().ToLower() == "select"||
            String.IsNullOrEmpty(mlocationDb1.Inspector_Name)|| mlocationDb1.Inspector_Name == "" ||
            String.IsNullOrEmpty(mlocationDb1.Inspector_Company)|| mlocationDb1.Inspector_Company == "" ||
            String.IsNullOrEmpty(mlocationDb1.Port)|| mlocationDb1.Port == "" ||
            String.IsNullOrEmpty(mlocationDb1.Country)|| mlocationDb1.Country == "" ||
            String.IsNullOrEmpty(mlocationDb1.Opening_MeetingStart)|| mlocationDb1.Opening_MeetingStart == "" ||
            String.IsNullOrEmpty(mlocationDb1.Opening_MeetingEnd)|| mlocationDb1.Opening_MeetingEnd == "" ||
            String.IsNullOrEmpty(mlocationDb1.Closing_MeetingStart)|| mlocationDb1.Closing_MeetingStart == "" ||
            String.IsNullOrEmpty(mlocationDb1.Closing_MeetingEnd)|| mlocationDb1.Closing_MeetingEnd == "" ||
            String.IsNullOrEmpty(mlocationDb1.Master_Name)|| mlocationDb1.Master_Name == "" ||
            String.IsNullOrEmpty(mlocationDb1.Chief_Engineer_Name)|| mlocationDb1.Chief_Engineer_Name == "" ||
            String.IsNullOrEmpty(mlocationDb1.Chief_Officer_Name)||mlocationDb1.Chief_Officer_Name == "" ||
            String.IsNullOrEmpty(mlocationDb1.Second_Engr_Name) || mlocationDb1.Second_Engr_Name == "")

        {  
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! Cannot publish, initial information area is incomplete.";
            //StartCoroutine(HidePopUp());
        }

        else if (questionsDBcount != answeredDBcount)
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! Cannot publish as you have '" + (questionsDBcount - answeredDBcount) + "' unanswered questions.";
            //StartCoroutine(HidePopUp());
        }
        
        else if (dbtokensbalance == 0 && reopenedinspection == false) //With token balance this also checks if this a re-opened inspection, i.e. status changed from publish to draft.
        {
            
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! Unable to publish as you do not have <b>0</b> (NO) credits remaining. Manage your account online from; inspectnau.orionmarineconcepts.com/loginpage.aspx (use same login credentials as the mobile APP).'";

            //msgtoserver = "tokenrequested";
            //StartCoroutine(SendRegnDataToCloud());  THIS WAS COMMENTED ON 23rd DEc 2024, SINCE THE TOKENS ARE MANAGED BY THE USER ON THEIR OWN.
            //StartCoroutine(HidePopUp());
        }

        else if (questionsDBcount == answeredDBcount && (dbtokensbalance > 0|| (dbtokensbalance <= -1&& dbtokensbalance > -5) || reopenedinspection == true))
        {
            errorpopup.gameObject.SetActive(true);
           
            pdffilegenerate.GetComponent<PdfReport>().onlyforexport = true;
            GeneratePDFReport.GetComponent<Button>().onClick.Invoke();

            PopUpMsg.text = "Once Published, you should not make any changes to the report. Continue?";
            Yes.SetActive(true);
            //Cancel.SetActive(true);
            //StartCoroutine(HidePopUp());
        }

       

        //mlocationDb1.close();
        //mLocationDb.close();
        //Yes.SetActive(true);
    }

    //public void YesBtnClickedDISCARD() // This method publishes the inspections and also affects the Tokens.
    //{
    //    inspectionsprimarydetails = "";
    //    inspectionsObservations = "";
    //    ObservationAttachments = "";
    //    PopUpMsg.text = "Please wait while we pubish your report, this may take a few seconds...";
    //    folderstozip = "";
    //    Debug.Log("Yes Button Clicked");
    //    Yes.SetActive(false);

    //    table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
    //    var connection1 = mlocationDb.getConnection();
    //    try
    //    {
    //        mlocationDb.UpdateStatusPublished(int.Parse(SavedInspPrimaryTableID.text)); // Change status of the inspection to published.
    //        mlocationDb.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text));
    //    }
    //    finally
    //    {
    //        connection1.Close();
    //    }

    //    table_LoginConfig mLocationDb = new table_LoginConfig();
    //    var connection2 = mLocationDb.getConnection();
    //    try
    //    {
    //        mLocationDb.getLatestID();

    //        if (mlocationDb.status.ToString().Trim() == "P")
    //        {
    //            Publishedinspectionsbuttons.SetActive(true);
    //            NewOrDraftinspectionsbuttons.SetActive(false);
    //            DraftMode.SetActive(false);
    //            Published.SetActive(true);

    //            if (!reopenedinspection)
    //            {
    //                tokensused = mLocationDb.tokenbalance > 0
    //                    ? mLocationDb.tokenbalance - 1
    //                    : mLocationDb.tokenbalance + 1;

    //                string query = "TokenBalance = '" + tokensused + "' where cast(ID as int) = '" + loginuserid + "'";
    //                mLocationDb.Updatedata(query);
    //            }
    //            else
    //            {
    //                Debug.Log("This was a reworked inspection; no tokens will be deducted.");
    //                Reopened.SetActive(false);
    //                Reopened.gameObject.GetComponent<TMP_Text>().text = "";
    //            }
    //            PopUpMsg.text = "Thank you for your patience! Your inspection has been successfully published...";
    //            reopenedinspection = false;
    //        }
    //        else
    //        {
    //            Publishedinspectionsbuttons.SetActive(false);
    //            NewOrDraftinspectionsbuttons.SetActive(true);
    //            DraftMode.SetActive(true);
    //            Published.SetActive(false);
    //        }
    //    }
    //    finally
    //    {
    //        connection2.Close();
    //    }

    //    table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
    //    var connection3 = mlocationDb1.getConnection();
    //    try
    //    {
    //        mlocationDb1.GetSectionHeaders(int.Parse(SavedInspPrimaryTableID.text));
    //        inspectionsObservations = mlocationDb1.outputofsearchresult;
    //    }
    //    finally
    //    {
    //        connection3.Close();
    //    }

    //    table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
    //    var connection4 = mlocationDb2.getConnection();
    //    try
    //    {
    //        mlocationDb2.GetSectionHeaders(int.Parse(SavedInspPrimaryTableID.text));
    //        ObservationAttachments = mlocationDb2.outputofsearchresult;
    //    }
    //    finally
    //    {
    //        connection4.Close();
    //    }

    //    table_Synch mlocationDB = new table_Synch();
    //    var connection5 = mlocationDB.getConnection();
    //    try
    //    {
    //        string query = "SELECT TRIM(SourceTable) AS SynchFileName, TRIM(ID) AS DatabaseID FROM Synch where cast(Quantity as int) = " + int.Parse(SavedInspPrimaryTableID.text);
    //        using var reader1 = mlocationDB.getDatabyQuery(query);

    //        List<int> obsids = new List<int>();

    //        while (reader1.Read())
    //        {
    //            if (!string.IsNullOrEmpty(reader1[0].ToString()))
    //            {
    //                synchronisationfilename = reader1[0].ToString().Trim();
    //                SynchDatabaseID.text = reader1[1].ToString();
    //                existinginsp = true;
    //                obsids.Add(int.Parse(SynchDatabaseID.text));
    //            }
    //            else
    //            {
    //                synchronisationfilename = (ShipImo.text + "_" + Folderpath + "_" + InspectDatFmTo.text + "_" + inspectionrowcreated + ".zip")
    //                    .Replace(" ", String.Empty)
    //                    .Replace("/", "")
    //                    .Replace(":", String.Empty)
    //                    .Replace("-", String.Empty);
    //                existinginsp = false;
    //            }
    //        }

    //        if (!existinginsp)
    //        {
    //            SynchData();
    //        }
    //        else
    //        {
    //            foreach (int obsID in obsids)
    //            {
    //                UpdateSynchData();
    //            }
    //        }
    //    }
    //    finally
    //    {
    //        connection5.Close();
    //    }

    //    createsynchzip();
    //}



    //THIS WAS COMMENTED AND NEW CODE BLOCK CREATED HANDLING CONNECTIONS EXPLICITLY
    public void YesBtnClicked() //This method publishes the inspections and also effects the Tokens...

    {
        inspectionsprimarydetails = "";
        inspectionsObservations = "";
        ObservationAttachments = "";
        PopUpMsg.text = "Please wait while we pubish your report, this may take a few seconds...";
        string inspectionstatus = "";
        folderstozip = "";
        Debug.Log("Yes Button Clicked");
        Yes.SetActive(false);
        //Cancel.SetActive(false);
        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationDb.getConnection();
       
        try
        {
            mlocationDb.UpdateStatusPublished(int.Parse(SavedInspPrimaryTableID.text));//CHANGE STATUS OF THE INSPECTION TO PUBLISHED.
            mlocationDb.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text));

            inspectionsprimarydetails = mlocationDb.outputofsearchresult;
            //System.Data.IDataReader reader = mlocationDb.InspectionCreatedDate("select DATETIME, *FROM 'Inspection_PrimaryDetails' where ID = " + int.Parse(SavedInspPrimaryTableID.text));

            using System.Data.IDataReader reader = mlocationDb.InspectionCreatedDate("select Timestamp, *FROM 'Inspection_PrimaryDetails' where ID = " + int.Parse(SavedInspPrimaryTableID.text));
            inspectionrowcreated = reader[0].ToString();

            inspectionstatus = mlocationDb.status.ToString().Trim();
            reader.Close();
            reader.Dispose();
        }
        finally
        {
            connection1.Close();
        }

        
        table_LoginConfig mLocationDb = new table_LoginConfig();
        using var connection2 = mLocationDb.getConnection();
        mLocationDb.getLatestID();

        mLocationDb.tokenbalance.ToString();
        tokensused = 0;

        if (inspectionstatus == "P")
        {
            Publishedinspectionsbuttons.SetActive(true);
            NewOrDraftinspectionsbuttons.SetActive(false);
            DraftMode.SetActive(false);
            Published.SetActive(true);

            if (reopenedinspection == false) //With token balance this also checks if this a re-opened inspection, i.e. status changed from publish to draft.)
            {
                if (mLocationDb.tokenbalance < 0)
                {
                    tokensused = mLocationDb.tokenbalance + 1;
                }

                else if (mLocationDb.tokenbalance > 0)
                {
                    tokensused = mLocationDb.tokenbalance - 1;
                }

                string query = " TokenBalance = '" + tokensused + "' where cast(ID as int) = '" + loginuserid + "'";

                BalanceTokensFreetext.SetActive(true);
                BalanceTokensFreetext.GetComponent<TMP_Text>().text = Math.Abs(tokensused).ToString() + " Credit(s) remaining.";
                BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.black;
                BalanceTokens.text = tokensused.ToString();
                BalanceTokens.GetComponent<TMP_Text>().color = Color.white;


                mLocationDb.Updatedata(query);
            }

            else if (reopenedinspection == true)
            {
                Debug.Log("This was a reworked inspections therefore no tokens will be deducted.");
                Reopened.SetActive(false);
                Reopened.gameObject.GetComponent<TMP_Text>().text = "";
            }
            PopUpMsg.text = "Thank you for your patience! Your inspection has been successfully published...";
            reopenedinspection = false;
        }
        else
        {
            Publishedinspectionsbuttons.SetActive(false);
            NewOrDraftinspectionsbuttons.SetActive(true);
            DraftMode.SetActive(true);
            Published.SetActive(false);

        }

        connection2.Close();

        //JSON FOR OBSERVATIONS UNDER THE PRIMARY INSPECTION ID

        table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
        using var connection3 = mlocationDb1.getConnection();
        mlocationDb1.GetSectionHeaders(int.Parse(SavedInspPrimaryTableID.text));

        //Observations
        inspectionsObservations = mlocationDb1.outputofsearchresult;
        connection3.Close();

        table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
        using var connection4 = mlocationDb2.getConnection();
        mlocationDb2.GetSectionHeaders(int.Parse(SavedInspPrimaryTableID.text));

        //Observations
        ObservationAttachments = mlocationDb2.outputofsearchresult;

        //ABOVE LINE IS NOT WORKING 02nd May, Check in the database if the DATETIME COlumn exits in the Primary Details Table.

        connection4.Close();

        //while (reader.Read())
        //{
        //    inspectionrowcreated = reader[0].ToString();
        //    //Debug.Log(reader[0].ToString());
        //}

        Debug.Log(Folderpath.ToString());

        //Debug.Log(inspectionsprimarydetails);

        try
        {

            string sourcePath = Path.Combine(Application.persistentDataPath, "Orion_DB_1");

            string path = Application.persistentDataPath + Folderpath.ToString() + "/inspectiondata.txt";
            
            if (File.Exists(sourcePath))
            {
                // Copy the file from source to destination
                File.Copy(sourcePath, Application.persistentDataPath + Folderpath.ToString(), overwrite: true);  // Overwrite true to replace if the file already exists
                Debug.Log("Database copied to: " + path);
            }

            if (File.Exists(path))
                File.Delete(path);
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter(path);

            //Write a line of text with generic inspection details for reference
            generalinspectiondetails = ShipImo.text + "_" + Shipsname.text + "_" + InspectorName.text + "_" + InspectType.text + "_" + InspectDatFmTo.text + "_Created On_" + inspectionrowcreated + "_" + DateTime.Now;
            FlagForSynch = InspectType.text;
            //Write a Heading in the text file
            sw.WriteLine("{"+"\"Inspection_General\":" +"\"" + generalinspectiondetails +"\"," +Environment.NewLine);

            //Write a second line of text
            sw.WriteLine("\"INSPECTION_MAIN_TABLE_DATA\":" + Environment.NewLine);
            sw.WriteLine(inspectionsprimarydetails + "," + Environment.NewLine);

            //Write next line of text
            sw.WriteLine("\"OBSERVATIONS\":" + Environment.NewLine);
            sw.WriteLine(inspectionsObservations + "," + Environment.NewLine);

            //Write next line of text
            if (string.IsNullOrEmpty(ObservationAttachments))
            {
                sw.WriteLine("\"ATTACHMENTS\":" + Environment.NewLine);
                sw.WriteLine(ObservationAttachments + "[{}]}" + Environment.NewLine);
            }

            else
            {
                sw.WriteLine("\"ATTACHMENTS\":" + Environment.NewLine);
                sw.WriteLine(ObservationAttachments + "}" + Environment.NewLine);

            }

            //Close the file
            sw.Close();

            if (!Directory.Exists(Application.persistentDataPath + "/" + "Synchronisation"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/" + "Synchronisation");
            }

            //string synchronisationfilenameraw = ShipImo.text + "_" + InspectType.text + "_" + InspectDatFmTo.text + "_" + inspectionrowcreated + ".zip";

            //string synchronisationfilenameraw = ShipImo.text + "_" + Folderpath + "_" + InspectDatFmTo.text + "_" + inspectionrowcreated + ".zip"; //CHANGED ABOVE LINE TO FOLDERPATH instead of  InspectType.text, since the file name was getting garbage txt like brackets.
            string synchronisationfilenameraw = ShipImo.text + "_" + Folderpath + ".zip"; //CHANGED ABOVE LINE TO FOLDERPATH instead of  InspectType.text, since the file name was getting garbage txt like brackets.

            synchronisationfilename = synchronisationfilenameraw.Replace(" ", String.Empty).Replace("/", "").Replace(":", String.Empty).Replace("-", String.Empty);

            folderstozip = Application.persistentDataPath + Folderpath.ToString();

            table_Synch mlocationDB = new table_Synch();
            using var connection = mlocationDB.getConnection();
            string query = "SELECT TRIM(SourceTable) AS SynchFileName, TRIM(ID) AS DatabaseID FROM Synch where cast(Quantity as int)= " + int.Parse(SavedInspPrimaryTableID.text);

            using System.Data.IDataReader reader1 = mlocationDB.getDatabyQuery(query);

            List<String> observations = new List<string>();
            obsids = new List<int>();

            while (reader1.Read())
            {
                if (!reader1.IsDBNull(0) && !string.IsNullOrEmpty(reader1[0].ToString()))
                {
                    //observations.Add(reader1[4].ToString().Trim());  //InspectionID
                    //observations.Add(reader1[8].ToString().Trim());  //ZIP FILE NAME
                    synchronisationfilename = synchronisationfilenameraw.Replace(" ", String.Empty).Replace("/", "").Replace(":", String.Empty).Replace("-", String.Empty);
                    //synchronisationfilename = reader1[0].ToString().Trim();//ZIP FILE NAME
                    SynchDatabaseID.text = reader1[1].ToString();
                    existinginsp = true;
                    obsids.Add(int.Parse(SynchDatabaseID.text));
                }

                else
                {
                    synchronisationfilename = synchronisationfilenameraw.Replace(" ", String.Empty).Replace("/", "").Replace(":", String.Empty).Replace("-", String.Empty);
                    existinginsp = false;
                }

            }

            reader1.Close();
            reader1.Dispose();
            connection.Close();
            //zipfilename = Application.persistentDataPath + "/Synchronisation/" + synchronisationfilename;

            //if (File.Exists(zipfilename))
            //    File.Delete(zipfilename);

            //System.IO.Compression.ZipFile.CreateFromDirectory(folderstozip, zipfilename);

            //mlocationDB.close();

            createsynchzip();

        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.Message);
        }
        finally
        {
            Debug.Log("Executing finally block.");
        }

    }

    public void ChangetodraftInitiate()
    {
        
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "This action will change the inspection status back to draft mode, ideally you should not make any changes to a published report. Continue?";
            Yestodraft.SetActive(true);
            //Cancel.SetActive(true);
            //StartCoroutine(HidePopUp());
        
    }


    public void changebacktodraft()
    {
        table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        var connection = mlocationDb.getConnection();

        try
        {
            mlocationDb.ReportReopened(int.Parse(SavedInspPrimaryTableID.text)); // Change status of the inspection to reopened.

            DraftMode.SetActive(true);
            Reopened.SetActive(true);
            NewOrDraftinspectionsbuttons.SetActive(true);

            Published.SetActive(false);
            Publishedinspectionsbuttons.SetActive(false);

            Reopened.SetActive(true);
            Reopened.gameObject.GetComponent<TMP_Text>().text = " Re-Working ID: " + SavedInspPrimaryTableID.text;
            reopenedinspection = true;
        }
        finally
        {
            connection.Close();
        }
    }


    public void createsynchzip()
    {
        zipfilename = Application.persistentDataPath + "/Synchronisation/" + synchronisationfilename;

        if (File.Exists(zipfilename))
            File.Delete(zipfilename);

        System.IO.Compression.ZipFile.CreateFromDirectory(folderstozip, zipfilename);

        if (existinginsp == false)
        {

            SynchData();
        }

        else
        {
            foreach (int obsID in obsids)
            {
                UpdateSynchData();

            }

        }
    }

    public void SynchData()
    {
        synchroniserdata.data = inspectionsprimarydetails;
        synchroniserdata.selecteditemcode = Shipsname.text + " ("+ ShipImo.text + ")" + "\n"+ "Inspected On; " + InspectDatFmTo.text;
        synchroniserdata.filename = synchronisationfilename;
        // Ardmore Cherokee (IMO No. 9888878) Inspection On; Fri 01/03/2024 12:27 
        synchroniserdata.InspectionPrimaryID = int.Parse(SavedInspPrimaryTableID.text);
        synchroniserdata.sourcetable = synchronisationfilename;
        synchroniserdata.flag = FlagForSynch;
        //ListAllData();
        //synchroniserdata.LatestID();

       
        synchroniserdata.AddData();
    }

    public void UpdateSynchData()
    {

        table_Synch mLocationDb2 = new table_Synch();
        using var connection = mLocationDb2.getConnection();
        //string query = "Processed  = 'N' WHERE cast(Quantity as int) = '" + int.Parse(SavedInspPrimaryTableID.text) + "'";

        string query = "Processed  = 'N', SourceTable = '" + synchronisationfilename + "', FileName = '" + synchronisationfilename + "' WHERE cast(Quantity as int) = '" + int.Parse(SavedInspPrimaryTableID.text) + "'";

        mLocationDb2.Updatedata(query);
        connection.Close();
        //mLocationDb2.close();
    }

    public void Downloadsynchfile()
    {
        string query = "SELECT TRIM(SourceTable) AS SynchFileName FROM Synch where cast(Quantity as int)= " + int.Parse(SavedInspPrimaryTableID.text);
        table_Synch mlocationDB = new table_Synch();
        using var connection = mlocationDB.getConnection();
        using System.Data.IDataReader reader1 = mlocationDB.getDatabyQuery(query);

        List<String> observations = new List<string>();
        List<int> obsids = new List<int>();
        zipfilename = "";
        while (reader1.Read())
        {
            if (reader1[0].ToString() != "")
            {
                synchronisationfilename = reader1[0].ToString().Trim();//ZIP FILE PATH
                zipfilename = Application.persistentDataPath + "/Synchronisation/" + synchronisationfilename;
                
                //if (File.Exists(zipfilename)) //ZIP FILE PATH
                //{
                //    System.Diagnostics.Process.Start(zipfilename);
                //    Application.OpenURL(zipfilename);
                //}
                //else
                //{
                //    Debug.Log("No Zip File Found!");
                //}
            }

            else
            {
                
                Debug.Log("Not found in the synch table!");
            }

        }


        reader1.Close();
        reader1.Dispose();
        connection.Close();

#if UNITY_EDITOR
       //zipfilename = Application.persistentDataPath + "/Synchronisation/"+ "IMONo.9877665__3_CargoOp_20241130T061004_Sat301120240409_20241130172841.zip";
        if (File.Exists(zipfilename))
        {
            System.Diagnostics.Process.Start(zipfilename);
            Application.OpenURL(zipfilename);
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Please wait, compressing and downloading the entire inspection data, this may take a few seconds. Once downloaded, you can close this window.";
        }
        else
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "There is no Zip file found for this inspection.";
            return;
        }
#elif UNITY_IPHONE || UNITY_ANDROID

         if (File.Exists(zipfilename))
        {
            // Use NativeShare to share the file
            new NativeShare()
                .AddFile(zipfilename) // Attach the PDF file
                .SetSubject("Sharing Zip Fil") // Optional: Set the subject
                .SetText("This the entire inspection data!") // Optional: Add a message
                .SetTitle("Share via") // Optional: Set the title of the share dialog
                .Share(); // Invoke the share dialog
                errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Please wait, compressing and downloading the entire inspection data, this may take a few seconds. Once downloaded, you can close this window.";
        }
        else
        {
        errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "There is no Zip file found for this inspection.";
            Debug.LogError("File not found: " + zipfilename);
        }          
#endif
        //mlocationDB.close();
    }


    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        PopUpMsg.color = Color.black;

        UpdateinspectionInfo.enabled = true;
        StartInspection.enabled = true;
        updatebtntext.GetComponent<TextMeshProUGUI>().color = Color.white;
        startbtnImg.GetComponent<Image>().color = Color.white;
        startbtntext.GetComponent<TextMeshProUGUI>().color = Color.white;

        ROVIQbtn.GetComponent<Button>().interactable = true; //This was added to avoid user clicking the ROVIQ button when the checklist data was being loaded.
        ROVIQbtnColour.color = Color.black;

    }

    IEnumerator SendRegnDataToCloud() //23rd Dec 2024; THIS TOKEN REQUEST AND GRANTED BLOCK WAS DISCONTINUED, SINCE APPLE DID NOT ALLOW THE TOKEN MANAGE BUTTON ON THE APP. THIS WILL BE HANDLED FROM "anymessageforme" within the SYNCHdata FIle.
    {
        
        string result = "";
        string guid = "";
        string columndataquery = "";
        
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();

        //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = "ankush1maurya@gmail.com", password = "123456" });
        //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = emailinputtext, password = passwordinputtext });
        string bodyJsonString = "";

        if (msgtoserver == "tokenrequested")

        {
            bodyJsonString = JsonUtility.ToJson(new AuthData()
            {
                neworupdate = msgtoserver,
                name = mlocationDb.name,
                companyname = mlocationDb.companyname,
                companycode = mlocationDb.companyAuthcode,
                designation = mlocationDb.designation,
                email = mlocationDb.useremail,
                password = mlocationDb.password.Trim(),
                WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),
                tokenremaining = int.Parse(BalanceTokens.text.ToString()),
                tokenrequest = 1,
                granttoken = 0
            });
        }

        else if (msgtoserver == "updatetokenvalues")

        {
            bodyJsonString = JsonUtility.ToJson(new AuthData()
            {
                neworupdate = msgtoserver,
                name = mlocationDb.name,
                companyname = mlocationDb.companyname,
                companycode = mlocationDb.companyAuthcode,
                designation = mlocationDb.designation,
                email = mlocationDb.useremail,
                password = mlocationDb.password.Trim(),
                WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),
                tokenremaining = mlocationDb.tokenbalance,
                tokenrequest = 0,
                granttoken = 0
            });
        }

        var request = new UnityWebRequest("https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        try
        {
            string rawJson = Encoding.Default.GetString(request.downloadHandler.data);

            if (String.IsNullOrEmpty(rawJson))
            {
                PopUpMsg.color = Color.red;
                errorpopup.SetActive(true);
                PopUpMsg.text = "Oops! you do not have any credits remaining to publish this inspection. We are unable to notify the administrator, seems you are offline, please try when online or in good connectivity."; //Oops! did not connect with server, maybe you are offline, please try when online or in good connectivity."
                //StartCoroutine(HidePopUp());
            }

            //message = tokenObj.message.ToString();
            else if (rawJson.Contains("Error"))
            {
                PopUpMsg.color = Color.red;
                Debug.Log("Invalid user.");
                errorpopup.SetActive(true);
                PopUpMsg.text = "Oops! you do not have any credits remaining to publish this inspection. We are unable to notify the administrator, either you are offline, else contact the support team.";
                //StartCoroutine(HidePopUp());
            }

            else if (rawJson.Contains("token requested"))
            {
                PopUpMsg.color = Color.black;
                errorpopup.SetActive(true);
                PopUpMsg.text = "Oops! you do not have any credits remaining to publish this inspection. We have notified the administrator for the credits to be allocated. Internet connectivity required for updating on APP.";
                //StartCoroutine(HidePopUp());

                columndataquery = "Tokendate = '1'";
                
                mlocationDb.Updatedata(columndataquery);
                //mlocationDb.close();


            }

            else if (rawJson.Contains("Values Updated"))
            {
                PopUpMsg.color = Color.black;
                errorpopup.SetActive(true);
                PopUpMsg.text = "The server has been updated with your credits values.";
                //StartCoroutine(HidePopUp());

                columndataquery = "Tokendate = '0'";
                mlocationDb.Updatedata(columndataquery);

                if (mlocationDb.tokenbalance < 0) //FREE CREDITS MODE
                {
                    BalanceTokensFreetext.SetActive(true);
                    BalanceTokensFreetext.GetComponent<TMP_Text>().text = Math.Abs(mlocationDb.tokenbalance).ToString() + " Credit(s) remaining.";

                    BalanceTokens.text = mlocationDb.tokenbalance.ToString();

                    BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
                    
                    
                }

                else if (mlocationDb.tokenbalance <= 2)
                {
                    BalanceTokensFreetext.SetActive(true);
                    BalanceTokensFreetext.GetComponent<TMP_Text>().text = Math.Abs(mlocationDb.tokenbalance).ToString() + " Credit(s) remaining.";
                    BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.red;
                    BalanceTokens.text = mlocationDb.tokenbalance.ToString();
                    BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
                }

                else
                {
                    BalanceTokensFreetext.SetActive(true);
                    BalanceTokensFreetext.GetComponent<TMP_Text>().text = Math.Abs(mlocationDb.tokenbalance).ToString() + " Credit(s) remaining.";
                    BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.black;
                    BalanceTokens.text = mlocationDb.tokenbalance.ToString();
                    BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
                }

                // mlocationDb.close();
                StartCoroutine(HidePopUp());
            }

            else if (rawJson.Contains("flag")) //Tokensstatus checking
            {
                string flagtext = "";
                int flag = 0;

                string granttext = "";
                int grantedvalue = 0;
                string status = "";


                //StartCoroutine(HidePopUp());

                flagtext = rawJson.Split(',')[0];
                flag = int.Parse(flagtext.Split('=')[1]);

                granttext = rawJson.Split(',')[1];
                grantedvalue = int.Parse(granttext.Split('=')[1]);

                status = rawJson.Split(',')[2]; ;

                if (grantedvalue < 0) // Declined when -1 is received...
                {
                    columndataquery = "Tokendate = '0'";

                    PopUpMsg.color = Color.black;
                    errorpopup.SetActive(true);
                    PopUpMsg.text = "Tokens declined by server.";
                    mlocationDb.Updatedata(columndataquery);
                    //mlocationDb.close();
                    msgtoserver = "updatetokenvalues";
                    StartCoroutine(SendRegnDataToCloud());
                }

                else if (grantedvalue > 0) // ALLOCATED
                {
                    int updatetokensinDB = 0;
                    if (int.Parse(BalanceTokens.text.ToString()) <= 0)
                    {
                        updatetokensinDB = grantedvalue;
                    }

                    else
                    {
                        updatetokensinDB = grantedvalue + int.Parse(BalanceTokens.text.ToString());
                    }

                    columndataquery = "Tokendate = '0', TokenBalance = '" + updatetokensinDB + "'";

                    PopUpMsg.color = Color.black;
                    errorpopup.SetActive(true);
                    PopUpMsg.text = "Hooray!! you have been granted " + grantedvalue + " Tokens by the server.";

                    mlocationDb.Updatedata(columndataquery);
                    //mlocationDb.close();
                    msgtoserver = "updatetokenvalues";
                    StartCoroutine(SendRegnDataToCloud());
                }
                else // PENDING
                {
                    PopUpMsg.color = Color.black;
                    errorpopup.SetActive(true);
                    PopUpMsg.text = "Tokens approval pending from server.";

                    columndataquery = "Tokendate = '1'";
                   
                    mlocationDb.Updatedata(columndataquery);
                }

            }

            else
            {
                errorpopup.SetActive(true);
                PopUpMsg.color = Color.black;
                PopUpMsg.text = "Did not receive any response, please try again, if you are the administrator, this data is static and will not be updated.";
            }

        
        }

        catch (Exception ex)
        {

            Debug.Log(request.error + " ex " + ex.Message + "There was a error on this line" + ex.StackTrace);
            
            PopUpMsg.color = Color.red;
            errorpopup.SetActive(true);
            PopUpMsg.text = "Oops !! Something went wrong, please check your internet connection or contact support team for details.";
            //StartCoroutine(HidePopUp());
        }

        connection.Close();
        
    }
}
