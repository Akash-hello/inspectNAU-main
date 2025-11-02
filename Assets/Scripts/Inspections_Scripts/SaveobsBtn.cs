using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using DataBank;
using UnityEngine.SceneManagement;
using System.Data;
using System;

public class SaveobsBtn : MonoBehaviour

{
   Text selectedanswer;
    public TextMeshProUGUI questiontext;
    public TMP_InputField observationdetail;
    public TextMeshProUGUI ObsDBid;
    int ObsDbID;
    string selectedanswer1;
    public GameObject anydaterelated;
    public GameObject anytimerelated;
   
    public GameObject additionaldetail;
    public GameObject riskcategory;
    public GameObject targetdate;
    public GameObject recommendation;
    public GameObject answertoggles;
    ToggleGroup answertogglegroup;
    Toggle toggle;

    public List<Toggle> m_Toggles = new List<Toggle>();
    int toggleindex; // Toggles on the Questions Prefab -- SIRE 2.0, Insp, Condn and Audits - 0_AsExpected_Toggle | 0_No_Deficiency_Toggle | 0_Good_Toggle , 1_Not_As_Expected_Toggle |1_Deficiency_Toggle , 2_Poor_Toggle, 3_NotSeen_Toggle, 4_NA_Toggle, 5_Yes_Toggle, 6_No_Toggle, 7_NotSeen_Toggle, 9_Satisfactory_Toggle, 10_MajorNC_Toggle,11_MinorNC_Toggle, 12_OBS_Toggle, 13_NotSeen_Toggle

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;
    public GameObject InspectionStatus;
    int Totalquestions;
    int Completedques;
    //public GameObject totalques;
    public TextMeshProUGUI ChapterDBID;
    public GameObject TotalQuesInChap;
    int Totalquestionschap;
    int Completedqueschap;
    public GameObject statusindicator;

    public TextMeshProUGUI primaryinspID;
    public TextMeshProUGUI HeaderTextForROVIQ;

    public TMP_InputField Date;
    public bool ROVIQTrueFalse;
    int AnsweredQuesforROVIQ;
    public GameObject AnswerGroup;
    public Button pendingquestions;// This is on the chapter prefab.
    public bool Photoismandatory;
   // public Image completionstatus; // This is on individual question prefab.REMOVED as unnecessary requires more code on loading questions to verify each
                                   
    public GameObject HumanPnl;
    public GameObject Hanswertoggles;
    ToggleGroup Hanswertogglegroup;
    Toggle Htoggle;
    public Toggle Hdeficiency;
    public Toggle HNodeficiency;
    public TMP_InputField Hobservationdetail;
    public int HumanAnswer;

    public GameObject ProcessPnl;
    public GameObject Panswertoggles;
    ToggleGroup Panswertogglegroup;
    Toggle Ptoggle;
    public Toggle Pdeficiency;
    public Toggle PNodeficiency;
    public TMP_InputField Pobservationdetail;
    public int PAnswer;

    public GameObject HardwarePnl;
    public GameObject Hardanswertoggles;
    ToggleGroup Hardanswertogglegroup;
    Toggle Hardtoggle;
    public Toggle Hardwaredeficiency;
    public Toggle HardwareNodeficiency;
    public TMP_InputField Hardobservationdetail;
    public int HardwareAnswer;

    public GameObject[] Observationpanels;
    public Toggle[] Answergroups;
    string questioncompleted;

    //inspectionstatus
    // Start is called before the first frame update

    void Start()
    {
        time = 3.0f;
        InspectionStatus = GameObject.FindGameObjectWithTag("inspectionstatus");
        
        //InspectionStatus.GetComponent<TextMeshProUGUI>().text = "";
        Totalquestions = 0;
        Completedques = 0;
        Totalquestionschap= 0;
        Completedqueschap=0;
        questioncompleted = "";
        // completionstatus.GetComponent<Image>().color = Color.gray;
    }



    public void onclicksavebtn()
    {
        answertogglegroup = answertoggles.GetComponent<ToggleGroup>();
        Hanswertogglegroup = Hanswertoggles.GetComponent<ToggleGroup>();
        Panswertogglegroup = Panswertoggles.GetComponent<ToggleGroup>();
        Hardanswertogglegroup = Hardanswertoggles.GetComponent<ToggleGroup>();

        toggle = answertogglegroup.ActiveToggles().FirstOrDefault();
        Htoggle = Hanswertogglegroup.ActiveToggles().FirstOrDefault();
        Ptoggle = Panswertogglegroup.ActiveToggles().FirstOrDefault();
        Hardtoggle = Hardanswertogglegroup.ActiveToggles().FirstOrDefault();

        Observationpanels = new GameObject[] { AnswerGroup, HumanPnl, ProcessPnl, HardwarePnl };
        Answergroups = new Toggle[] { toggle, Htoggle, Ptoggle, Hardtoggle };

        ObsDbID = int.Parse(ObsDBid.text.ToString());

        if (toggle == null && AnswerGroup.activeSelf)
        {
            Debug.Log("Observation not selected and no comment entered!!");
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Nothing to save, no option(s) selected!!";
            StartCoroutine(HidePopUp());

            if (HumanPnl.activeSelf || ProcessPnl.activeSelf || HardwarePnl.activeSelf)
                                {
                                    forsire20categories();
                                }
        }

        else
        {
            if (AnswerGroup.activeSelf)
            {
                for (var i = 0; i < m_Toggles.Count; i++)
                {

                    if (m_Toggles[i] == toggle)
                    {
                        Debug.Log("The selected toggle name is " + m_Toggles[i].name + " at index; " + i);
                        toggleindex = i;
                        if ((toggleindex == 1 || toggleindex == 2 || toggleindex == 6 || toggleindex == 10 || toggleindex == 11 || toggleindex == 12) && observationdetail.text.Length < 10)
                        // This is IF statement to check if anything marked Poor, NC or as No, etc.
                        // Toggles on the Questions Prefab -- SIRE 2.0, Insp, Condn and Audits - 0_AsExpected_Toggle | 0_No_Deficiency_Toggle | 0_Good_Toggle , 1_Not_As_Expected_Toggle |1_Deficiency_Toggle , 2_Poor_Toggle, 3_NotSeen_Toggle, 4_NA_Toggle, 5_Yes_Toggle, 6_No_Toggle, 7_NotSeen_Toggle, 9_Satisfactory_Toggle, 10_MajorNC_Toggle,11_MinorNC_Toggle, 12_OBS_Toggle, 13_NotSeen_Toggle
                        
                        {
                            errorpopup.gameObject.SetActive(true);
                            PopUpMsg.text = "Not Saved, Insufficient observation / comment text (min 10 characters required with the selected options)!!";
                            PopUpMsg.color = Color.red;
                            StartCoroutine(HidePopUp());

                        }

                        else // Else marked Good, NA, Unseen, etc.
                        {
                            table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
                            using var connection = mlocationDb.getConnection();
                            string columndataquery = "";
                            if (questiontext.text.ToLower().Contains("date") && string.IsNullOrEmpty(Date.text))
                            {
                                if (!HumanPnl.activeSelf && !ProcessPnl.activeSelf && !HardwarePnl.activeSelf) //FIXED ON 22nd FEB 2025 as the answer was being saved irrespective Sire or No Sire
                                {
                                    selectedanswer1 = toggleindex + "-" + toggle.GetComponentInChildren<TextMeshProUGUI>().text;
                                    columndataquery = "Selected_Answer = '" + selectedanswer1.ToString() + "', Observation_Text = '" + observationdetail.text.ToString().Replace("'", "''") + "', Date = '" + Date.text.ToString() + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                                    mlocationDb.Updatedata(columndataquery);

                                    questioncompleted = "Completed";
                                    statusindicator.GetComponent<Image>().color = Color.green;

                                    errorpopup.gameObject.SetActive(true);
                                    PopUpMsg.text = "Saved without any 'Date', hope you haven't missed entering a date."; // General Observation
                                    PopUpMsg.color = Color.red;
                                    StartCoroutine(HidePopUp());
                                }

                                if (HumanPnl.activeSelf || ProcessPnl.activeSelf || HardwarePnl.activeSelf)
                                {
                                    forsire20categories();
                                }

                                else
                                {
                                    if (ROVIQTrueFalse == true)
                                    {
                                        FetchROVIQ();
                                    }

                                    else
                                    {
                                        updatestatusnew();
                                    }
                                }

                            }
                            else // The question does not have a date reference, so it takes the selected option and obs data to the DB.
                            {
                                if (!HumanPnl.activeSelf && !ProcessPnl.activeSelf && !HardwarePnl.activeSelf) //FIXED ON 22nd FEB 2025 as the answer was being saved irrespective Sire or No Sire
                                {
                                    selectedanswer1 = toggleindex + "-" + toggle.GetComponentInChildren<TextMeshProUGUI>().text;
                                    Debug.Log(toggle.name + " - " + toggle.GetComponentInChildren<TextMeshProUGUI>().text + "--" + selectedanswer1.ToString() + " Observation ID -->" + ObsDbID + " Observation Text -->" + observationdetail.text);

                                    questioncompleted = "Completed";
                                    statusindicator.GetComponent<Image>().color = Color.green;

                                    columndataquery = "Selected_Answer = '" + selectedanswer1.ToString() + "', Observation_Text = '" + observationdetail.text.ToString().Replace("'", "''") + "', Date = '" + Date.text.ToString() + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                                    errorpopup.gameObject.SetActive(true);
                                    PopUpMsg.text = "Saved successfully!!"; // General Observation Saved successfully!!
                                    PopUpMsg.color = Color.black;
                                    StartCoroutine(HidePopUp());
                                    mlocationDb.Updatedata(columndataquery);
                                }
                                if (HumanPnl.activeSelf || ProcessPnl.activeSelf || HardwarePnl.activeSelf)
                                {
                                    forsire20categories();
                                }

                                else
                                {
                                    if (ROVIQTrueFalse == true)
                                    {
                                        FetchROVIQ();
                                    }

                                    else
                                    {
                                        updatestatusnew();
                                    }
                                }
                            }
                            //mlocationDb.close();
                        }
                    }


                }
            }
            
            else

            {
                if (observationdetail.text.Length < 5 && !(AnswerGroup.activeSelf)) //The Obs text can't be empty for the General Categories where there are no Good/Fair Yes/No options, these will entail some details pertaining to the question.
                {
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = "Please enter sufficient comment text (min 5 characters)!!";
                    PopUpMsg.color = Color.red;
                    StartCoroutine(HidePopUp());

                }

                else
                {
                    table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
                    string columndataquery = "";

                    if (questiontext.text.ToLower().Contains("date") && string.IsNullOrEmpty(Date.text))
                    {
                        columndataquery = "Selected_Answer = '999-Free', Observation_Text = '" + observationdetail.text.ToString().Replace("'", "''") + "', Date = '" + Date.text.ToString() + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                        errorpopup.gameObject.SetActive(true);
                        PopUpMsg.text = "Saved without any 'Date', hope you haven't missed entering.";// General Observation
                        PopUpMsg.color = Color.red;
                        StartCoroutine(HidePopUp());
                        mlocationDb.Updatedata(columndataquery);

                        questioncompleted = "Completed";
                        statusindicator.GetComponent<Image>().color = Color.green;

                        //mlocationDb.close();

                    }

                    else
                    {
                        columndataquery = "Selected_Answer = '999-Free', Observation_Text = '" + observationdetail.text.ToString().Replace("'", "''") + "', Date = '" + Date.text.ToString() + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                        errorpopup.gameObject.SetActive(true);
                        PopUpMsg.text = "Saved successfully!!"; // General Observation saved
                        PopUpMsg.color = Color.black;
                        StartCoroutine(HidePopUp());
                        mlocationDb.Updatedata(columndataquery);
                        mlocationDb.close();

                        questioncompleted = "Completed";
                        statusindicator.GetComponent<Image>().color = Color.green;
                    }

                    if (HumanPnl.activeSelf || ProcessPnl.activeSelf || HardwarePnl.activeSelf)
                    {
                        forsire20categories();
                    }

                    else
                    {
                        if (ROVIQTrueFalse == true)
                        {
                            FetchROVIQ();
                        }

                        else
                        {
                            updatestatusnew();
                        }
                    }
                    
                }

            }

        }

    }
    
    public void forsire20categories()
    {

        //if (HumanPnl.activeSelf && Htoggle == null || ProcessPnl.activeSelf && Ptoggle == null || HardwarePnl.activeSelf && Hardtoggle == null)
        //{
        //    Debug.Log("You need to select some option for SIRE 2.0!!");
        //    errorpopup.gameObject.SetActive(true);
        //    PopUpMsg.color = Color.red;
        //    PopUpMsg.text = "Not saved; You have not selected any answer to the SIRE 2.0 elements.";
        //    StartCoroutine(HidePopUp());
        //}

        if ((HumanPnl.activeSelf && Hdeficiency.isOn && Hobservationdetail.text.Length < 10) || (ProcessPnl.activeSelf && Pdeficiency.isOn && Pobservationdetail.text.Length < 10) || (HardwarePnl.activeSelf && Hardwaredeficiency.isOn && Hardobservationdetail.text.Length < 10))
        {
            Debug.Log("Insufficient observation comment SIRE 2.0!!");
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.color = Color.red;
            PopUpMsg.text = "You need to input sufficient (min 10 characters) observation text for 'NOT As Expected or with Defeciency'!!";
            StartCoroutine(HidePopUp());
        }

        else
        {
            table_Inspection_Attachments mlocationDb = new table_Inspection_Attachments();
            using var connection = mlocationDb.getConnection();
            string columndataquery = "";
            string checkifexists = "";
            int countifexists = 0;

            int ID = 0;
            string Attachment_Title = "";
            string Attachment_Name = "";

            string Attachment_Path = "";
            string Attachment_Details_1 = "";
            string Attachment_Details_2 = "";
            string Attachment_Details_3 = "";
            string Attachment_Details_4 = "";
            string Active = "";

            if (HumanPnl.activeSelf)// && Htoggle != null) 
            {
                Attachment_Path = "";
                Attachment_Details_1 = "";
                Attachment_Details_2 = "";
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "";
                checkifexists = "";
                countifexists = 0;

                if (Hdeficiency.isOn)
                {
                    HumanAnswer = 1;//NOT AS EXPECTED
                }
                else if (HNodeficiency.isOn)
                {
                    HumanAnswer = 0; //FOUND AS EXPECTED
                }

                checkifexists = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%HumanElement%' ;";

                using System.Data.IDataReader reader = mlocationDb.IfRecordExists(checkifexists);
                countifexists = mlocationDb.CountRecords;

                Attachment_Title = "HumanElement";
                Attachment_Name = Hobservationdetail.text.ToString().Replace("'", "''");

                Attachment_Path = "Not_Applicable";
                Attachment_Details_1 = "sire20";
                Attachment_Details_2 = HumanAnswer.ToString().Trim();
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "Y";

                if (countifexists > 0) // THAT MEANS this element was already created and should now be Updated.
                {

                    if (!Hdeficiency.isOn && !HNodeficiency.isOn) // Basically user has deselected the HUMAN options, then if it exists in database, delete it...
                    {
                        columndataquery = " where cast(Inspection_Observations_ID as int)  = "
                   + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = "
                   + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%HumanElement%' ;";

                        mlocationDb.Deletedata(columndataquery);
                    }

                    else
                    {
                        columndataquery = " Attachment_Name = '" + Hobservationdetail.text.ToString().Replace("'", "''")
                   + "', Attachment_Details_2 = '" + HumanAnswer
                   + "', Timestamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
                   + "' where cast(Inspection_Observations_ID as int)  = "
                   + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = "
                   + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%HumanElement%' ;";

                        mlocationDb.Updatedata(columndataquery);

                    }

                }

                else if (Htoggle != null)
                { 
                    mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                     int.Parse(primaryinspID.text),
                     ObsDbID,
                     Attachment_Title,
                     Attachment_Name,
                     Attachment_Path,
                     Attachment_Details_1,
                     Attachment_Details_2,
                     Attachment_Details_3,
                     Attachment_Details_4,
                     Active,
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

                }
                reader.Dispose();
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Saved successfully!!"; //"Human Element Data saved.";

            }

            if (ProcessPnl.activeSelf)// && Ptoggle != null)
            {
                Attachment_Path = "";
                Attachment_Details_1 = "";
                Attachment_Details_2 = "";
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "";
                checkifexists = "";
                countifexists = 0;

                if (Pdeficiency.isOn)
                {
                    PAnswer = 1;//NOT AS EXPECTED
                }
                else if (PNodeficiency.isOn)
                {
                    PAnswer = 0; //FOUND AS EXPECTED
                }

                checkifexists = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%ProcessElement%' ;";

                using System.Data.IDataReader reader = mlocationDb.IfRecordExists(checkifexists);
                countifexists = mlocationDb.CountRecords;

                Attachment_Title = "ProcessElement";
                Attachment_Name = Pobservationdetail.text.ToString().Replace("'", "''");

                Attachment_Path = "Not_Applicable";
                Attachment_Details_1 = "sire20";
                Attachment_Details_2 = PAnswer.ToString().Trim();
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "Y";

                if (countifexists > 0) // THAT MEANS this element was already created and should now be Updated.
                {
                    if (!Pdeficiency.isOn && !PNodeficiency.isOn) // Basically user has deselected the HUMAN options, then if it exists in database, delete it...
                    {
                        columndataquery = " where cast(Inspection_Observations_ID as int)  = "
                   + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = "
                   + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%ProcessElement%' ;";

                        mlocationDb.Deletedata(columndataquery);
                    }

                    else
                    {
                        columndataquery = " Attachment_Name = '" + Pobservationdetail.text.ToString().Replace("'", "''")
                    + "', Attachment_Details_2 = '" + PAnswer
                    + "', Timestamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
                    + "' where cast(Inspection_Observations_ID as int)  = "
                    + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = "
                    + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%ProcessElement%' ;";

                        mlocationDb.Updatedata(columndataquery);
                    }
                    
                   
                }

                else if (Ptoggle != null)
                {
                    mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                     int.Parse(primaryinspID.text),
                     ObsDbID,
                     Attachment_Title,
                     Attachment_Name,
                     Attachment_Path,
                     Attachment_Details_1,
                     Attachment_Details_2,
                     Attachment_Details_3,
                     Attachment_Details_4,
                     Active,
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

                }
                reader.Dispose();
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Saved successfully!!"; //"Process Element Data saved.";
            }

            if (HardwarePnl.activeSelf)// && Hardtoggle != null)
            {
                Attachment_Path = "";
                Attachment_Details_1 = "";
                Attachment_Details_2 = "";
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "";
                checkifexists = "";
                countifexists = 0;

                if (Hardwaredeficiency.isOn)
                {
                    HardwareAnswer = 1;//NOT AS EXPECTED
                }
                else if (HardwareNodeficiency.isOn)
                {
                    HardwareAnswer = 0; //FOUND AS EXPECTED
                }

                checkifexists = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%HardwareElement%' ;";

                using System.Data.IDataReader reader = mlocationDb.IfRecordExists(checkifexists);
                countifexists = mlocationDb.CountRecords;

                Attachment_Title = "HardwareElement";
                Attachment_Name = Hardobservationdetail.text.ToString().Replace("'", "''");

                Attachment_Path = "Not_Applicable";
                Attachment_Details_1 = "sire20";
                Attachment_Details_2 = HardwareAnswer.ToString().Trim();
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "Y";

                if (countifexists > 0) // THAT MEANS this element was already created and should now be Updated.
                {
                    if (!Hardwaredeficiency.isOn && !HardwareNodeficiency.isOn) // Basically user has deselected the HARDWARE options, then if it exists in database, delete it...
                    {
                        columndataquery =  " where cast(Inspection_Observations_ID as int)  = "
                   + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = "
                   + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%HardwareElement%' ;";

                        mlocationDb.Deletedata(columndataquery);
                    }

                    else
                    {
                        columndataquery = " Attachment_Name = '" + Hardobservationdetail.text.ToString().Replace("'", "''")
                   + "', Attachment_Details_2 = '" + HardwareAnswer
                   + "', Timestamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
                   + "' where cast(Inspection_Observations_ID as int)  = "
                   + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = "
                   + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%HardwareElement%' ;";

                        mlocationDb.Updatedata(columndataquery);
                    }
                    
                }

                else if (Hardtoggle != null)
                {
                    mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                     int.Parse(primaryinspID.text),
                     ObsDbID,
                     Attachment_Title,
                     Attachment_Name,
                     Attachment_Path,
                     Attachment_Details_1,
                     Attachment_Details_2,
                     Attachment_Details_3,
                     Attachment_Details_4,
                     Active,
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

                }
                reader.Dispose();
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Saved successfully!!"; //"Hardware Element Data saved.";

            }

            // 23rd Feb 2025, THIS "IF" CONDITION WOULDN't arise since Yes and No were now dependent on the SIRE Elements automatically... NOW NOT BELOW IS WHEN SAVING WITH OBSERVATION IS SELECTED WITH A GOOD, SATIS, or YES, when SIRE answers contain a Deficiency...
            if ((HumanAnswer == 1 ||PAnswer == 1 ||HardwareAnswer ==1)&& (toggleindex == 0 || toggleindex == 5|| toggleindex == 9 || toggleindex == 3 || toggleindex == 4 || toggleindex == 7 || toggleindex == 8 || toggleindex == 13))
            {
                //Set specific indices to OFF
                int[] indicesToNull = { 0, 5, 9, 3, 4 };

                foreach (int index in indicesToNull)
                {
                    if (index < m_Toggles.Count) // Ensure index is within bounds
                    {
                        m_Toggles[index].isOn = false;
                    }
                }

                //table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
                //using var connection1 = mlocationDb1.getConnection();
                //string columndataquery1 = "";
                //columndataquery1 = "Selected_Answer = '', Observation_Text = '', Date = '' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                //mlocationDb1.Updatedata(columndataquery);

                Debug.Log("You are not allowed to select this option with a Not expected or a deficiency...!!");
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.color = Color.red;
                PopUpMsg.text = "You cannot select this answer on the General observation text with 'NOT As Expected or Defeciency marked.'!!";
                StartCoroutine(HidePopUp());
            }

            else if (toggleindex == 3 || toggleindex == 4 || toggleindex == 7 || toggleindex == 8 || toggleindex == 13) //ONLY Toggle 7 and 8 are handled here others are Not applicable for SIRE
            {
                table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
                using var connection1 = mlocationDb1.getConnection();
                string columndataquery1 = "";

                selectedanswer1 = toggleindex + "-" + toggle.GetComponentInChildren<TextMeshProUGUI>().text;
                Debug.Log(toggle.name + " - " + toggle.GetComponentInChildren<TextMeshProUGUI>().text + "--" + selectedanswer1.ToString() + " Observation ID -->" + ObsDbID + " Observation Text -->" + observationdetail.text);
                columndataquery1 = "Selected_Answer = '" + selectedanswer1.ToString() + "', Observation_Text = '" + observationdetail.text.ToString().Replace("'", "''") + "', Date = '" + Date.text.ToString() + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                mlocationDb1.Updatedata(columndataquery1);

                Debug.Log("NA and NOT Seen automatically deselect the other options from 'Toggle Manager' script and save the data...!!");
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Saved successfully!!"; // General Observation Saved successfully!!
                PopUpMsg.color = Color.black;
                StartCoroutine(HidePopUp());
            }
            //THIS WILL BE MOSTLY THE CASE SINCE YES/NO is now auto selected...
            else if ((HumanPnl.activeSelf && Htoggle != null)||(ProcessPnl.activeSelf && Ptoggle != null)||(HardwarePnl.activeSelf && Hardtoggle != null))
            {
                table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
                using var connection1 = mlocationDb1.getConnection();
                string columndataquery1 = "";

                if (toggle != null && AnswerGroup.activeSelf)
                {
                    selectedanswer1 = toggleindex + "-" + toggle.GetComponentInChildren<TextMeshProUGUI>().text;
                    Debug.Log(toggle.name + " - " + toggle.GetComponentInChildren<TextMeshProUGUI>().text + "--" + selectedanswer1.ToString() + " Observation ID -->" + ObsDbID + " Observation Text -->" + observationdetail.text);
                    columndataquery1 = "Selected_Answer = '" + selectedanswer1.ToString() + "', Observation_Text = '" + observationdetail.text.ToString().Replace("'", "''") + "', Date = '" + Date.text.ToString() + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and TRIM(Obs_Details_8) = 'Question' ";
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = "Saved successfully!!"; // General Observation Saved successfully!!
                    PopUpMsg.color = Color.black;
                    StartCoroutine(HidePopUp());
                    mlocationDb1.Updatedata(columndataquery1);
                }

                else
                {
                    PopUpMsg.text = "Saved with pending final answer!!"; // General Observation Saved successfully!!
                    PopUpMsg.color = Color.black;
                    StartCoroutine(HidePopUp());
                }
            }

            else
            {
                Debug.Log("Complete the SIRE questions first...!!");
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "You cannot select this unless all Human, Process and/or Hardware elements as applicable, are answered.'!!"; // General Observation Saved successfully!!
                PopUpMsg.color = Color.black;
                StartCoroutine(HidePopUp());
            }
            // mlocationDb.close();
            Optionsactiveandsaved();
        }
       
        
    }


    public void Optionsactiveandsaved()
    {
        int totalanswergroupoptions = 0;
        int notansweredtillnow = 0;

        if ( toggleindex != 7 || toggleindex != 8)
        {
            for (int i = 0; i < Observationpanels.Length; i++)
            {
                Debug.Log("Index: " + i + ", GameObject: " + Observationpanels[i].name);

                if (Observationpanels[i].activeSelf && Answergroups[i] == null) //The Answergroup is on with no option selected.
                {
                    Debug.Log("The Answergroup is on with no option selected!!");
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = "Saved but answer still incomplete as selection is pending for the; " + Observationpanels[i].name + ".";
                    StartCoroutine(HidePopUp());
                    totalanswergroupoptions += 1;
                    notansweredtillnow += 1;
                }
                else if (Observationpanels[i].activeSelf && Answergroups[i] != null) //The Answergroup is on && option selected.
                {
                    Debug.Log("One option is selected for this element; " + Observationpanels[i].name);
                    totalanswergroupoptions += 1;
                }

            }

        }

        Debug.Log("Number of answer groups active for this questions are; " + totalanswergroupoptions);

        if (notansweredtillnow == totalanswergroupoptions)// Old if statement which was only with one obs (toggle == null && AnswerGroup.activeSelf)
        {
            Debug.Log("Observation not selected and no comment entered!!");
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Nothing to save, no option(s) selected!!";
            StartCoroutine(HidePopUp());
        }

        else if (notansweredtillnow == 0 || toggleindex == 7 || toggleindex == 8) // Indicates no Answer Groups left unanswered, all completed or NA, NOT SEEN ARE selected
        {
            questioncompleted = "Completed";
            statusindicator.GetComponent<Image>().color = Color.green;
            // completionstatus.GetComponent<Image>().color = Color.yellow;
            if (ROVIQTrueFalse == true)
                {
                    FetchROVIQ();
                }

                else
                {
                updatestatusnew();
                }
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Saved successfully.";
            StartCoroutine(HidePopUp());
        }

        else if (notansweredtillnow != 0 && (toggleindex != 7 || toggleindex != 8)) // Indicates some unanswered, not all completed and NA, NOT SEEN ARE NOT selected
        {
            questioncompleted = "";
            // completionstatus.GetComponent<Image>().color = Color.yellow;
            if (ROVIQTrueFalse == true)
            {
                FetchROVIQ();
            }

            else
            {
                updatestatusnew();
            }

        }

    }

    public void updatestatusnew ()
    {
        Completedques = 0;
        int mediacount = 0;
        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection = mlocationDb.getConnection();
        Totalquestions = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));
        //Completedques = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_7) = 'Completed' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

        string query1 = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%Media%' ;";
        mediacount = mlocationDb.CountbasisQuery(query1);
        Debug.Log(mediacount.ToString());

        if (Photoismandatory == true && mediacount == 0)
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Incomplete! Photographic evidence required!";
            PopUpMsg.color = Color.red;
            StartCoroutine(HidePopUp());
        }

        else
        {
            table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
            using var connection1 = mlocationDb1.getConnection();
            string columndataquery = "";
            //questioncompleted = "Completed";
            //statusindicator.GetComponent<Image>().color = Color.green;
            columndataquery = "Obs_Details_7 = '"+ questioncompleted + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text);
            mlocationDb1.Updatedata(columndataquery);

            Debug.Log("REACHED HERE CLOSING The question has been completed -- " + questioncompleted);
            Completedques = mlocationDb1.totalRecords("Inspection_Observations where trim(Obs_Details_7) = 'Completed' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));
            InspectionStatus.GetComponent<TextMeshProUGUI>().text = Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";
            
            Debug.Log(int.Parse(ChapterDBID.text.ToString()));
            Completedqueschap = mlocationDb1.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) = 'Completed' " +
                    "and  cast(Cloud_DB_ParentID as int) = " + int.Parse(ChapterDBID.text.ToString()) +
                    " and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

            Totalquestionschap = mlocationDb1.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question'" +
                " and  cast(Cloud_DB_ParentID as int) = " + int.Parse(ChapterDBID.text.ToString()) +
                " and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

            TotalQuesInChap.GetComponent<TextMeshProUGUI>().text = "Answered; " + Completedqueschap.ToString() + "/" + Totalquestionschap.ToString();

            if (Completedqueschap == Totalquestionschap)
            {
                //  completionstatus.GetComponent<Image>().color = Color.green; // This is on individual question prefab.
                pendingquestions.GetComponent<Image>().color = Color.green;// This is on the chapter prefab.
                pendingquestions.GetComponent<Button>().interactable = false;
                statusindicator.GetComponent<Image>().color = Color.green; 
            }

            else
            {
                // completionstatus.GetComponent<Image>().color = Color.yellow; // This is on individual question prefab.
                pendingquestions.GetComponent<Image>().color = Color.yellow; // This is on the chapter prefab.
                pendingquestions.GetComponent<Button>().interactable = true;
                //statusindicator.GetComponent<Image>().color = Color.grey;
            }
        }
    }
    
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> LoadDataByROVIQSequence() //Used for Getting ROVIQ.
    {
        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection2 = mlocationDb2.getConnection();
        string columndataquery = "SELECT Cloud_DB_ID,Template_Section_Ques,Obs_Details_1,ROVIQSequence,Selected_Answer,Obs_Details_7 FROM Inspection_Observations where TRIM(Obs_Details_8) ='Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString());
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
        //mlocationDb2.close();
        return myList;
    }

    public void FetchROVIQ()
    {

        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection = mlocationDb.getConnection();
        string query1 = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%Media%' ;";
        int mediacount = mlocationDb.CountbasisQuery(query1);
        Totalquestions = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

        AnsweredQuesforROVIQ = 0;

        Debug.Log(mediacount.ToString());

        if (Photoismandatory == true && mediacount == 0)
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Incomplete! Photographic evidence required!";
            PopUpMsg.color = Color.red;
            StartCoroutine(HidePopUp());
        }

        else

        {

            table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
            using var connection1 = mlocationDb1.getConnection();
            string columndataquery = "";
            //questioncompleted = "Completed";
            //statusindicator.GetComponent<Image>().color = Color.green;
            columndataquery = "Obs_Details_7 = '" + questioncompleted + "' where cast(Cloud_DB_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text);
            mlocationDb1.Updatedata(columndataquery);

            // To Get all ROVIQSequence list        
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> groupedData = LoadDataByROVIQSequence();
            List<string> keys = groupedData.Keys.ToList();
            //To get all questions by groupName

            Dictionary<string, Dictionary<string, string>> finalids = new Dictionary<string, Dictionary<string, string>>();
            //List<string> finalSection_Ques = new List<string>();
            //List<string> finalObs_Details = new List<string>();
            foreach (string key in keys)
            {
                if (key.ToLower().Contains(HeaderTextForROVIQ.text.ToString()))
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

            TotalQuesInChap.GetComponent<TextMeshProUGUI>().text = "Answered; " + AnsweredQuesforROVIQ.ToString() + "/" + finalids.Count.ToString();
            Completedques = mlocationDb1.totalRecords("Inspection_Observations where trim(Obs_Details_7) = 'Completed' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));
            InspectionStatus.GetComponent<TextMeshProUGUI>().text = Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";
        }
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        PopUpMsg.color = Color.black;
    }



    //public void updatestatus() // OLD STATUS UPDATE WITHOUT COMPLETION STATUS.s
    //{

    //    Completedques = 0;
    //    int mediacount = 0;
    //    table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
    //    Totalquestions = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));
    //    Completedques = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) !='' and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

    //    string query1 = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%Media%' ;";
    //    mediacount = mlocationDb.CountbasisQuery(query1);
    //    Debug.Log(mediacount.ToString());

    //    if (Photoismandatory == true && mediacount == 0)
    //    {
    //        errorpopup.gameObject.SetActive(true);
    //        PopUpMsg.text = "Incomplete! Photographic evidence required!";
    //        PopUpMsg.color = Color.red;
    //        StartCoroutine(HidePopUp());
    //    }

    //    else
    //    {
    //        questioncompleted = "All answers completed";
    //        Debug.Log("REACHED HERE CLOSING The question has been completed -- " + questioncompleted);
    //        InspectionStatus.GetComponent<TextMeshProUGUI>().text = Completedques.ToString() + " / " + Totalquestions.ToString() + " Completed.";

    //        Debug.Log(int.Parse(ChapterDBID.text.ToString()));
    //        Completedqueschap = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) !='' " +
    //                "and  cast(Cloud_DB_ParentID as int) = " + int.Parse(ChapterDBID.text.ToString()) +
    //                " and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

    //        Totalquestionschap = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question'" +
    //            " and  cast(Cloud_DB_ParentID as int) = " + int.Parse(ChapterDBID.text.ToString()) +
    //            " and  cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(primaryinspID.text.ToString()));

    //        TotalQuesInChap.GetComponent<TextMeshProUGUI>().text = "Answered; " + Completedqueschap.ToString() + "/" + Totalquestionschap.ToString();

    //        if (Completedqueschap == Totalquestionschap)
    //        {
    //            //  completionstatus.GetComponent<Image>().color = Color.green; // This is on individual question prefab.
    //            pendingquestions.GetComponent<Image>().color = Color.green;// This is on the chapter prefab.
    //            pendingquestions.GetComponent<Button>().interactable = false;

    //        }

    //        else
    //        {
    //            // completionstatus.GetComponent<Image>().color = Color.yellow; // This is on individual question prefab.
    //            pendingquestions.GetComponent<Image>().color = Color.yellow; // This is on the chapter prefab.
    //            pendingquestions.GetComponent<Button>().interactable = true;
    //        }
    //    }

    //    mlocationDb.close();
    //}

    //public void FetchROVIQ()
    //{

    //    table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
    //    string query1 = " Inspection_Attachments" + " where cast(Inspection_Observations_ID as int)  = " + ObsDbID + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(primaryinspID.text) + " and trim(Attachment_Title) like '%Media%' ;";
    //    int mediacount = mlocationDb.CountbasisQuery(query1);
    //    Debug.Log(mediacount.ToString());

    //    if (Photoismandatory == true && mediacount == 0)
    //    {
    //        errorpopup.gameObject.SetActive(true);
    //        PopUpMsg.text = "Incomplete! Photographic evidence required!";
    //        PopUpMsg.color = Color.red;
    //        StartCoroutine(HidePopUp());
    //    }

    //    else

    //    {
    //        AnsweredQuesforROVIQ = 0;
    //        // To Get all ROVIQSequence list        
    //        Dictionary<string, Dictionary<string, Dictionary<string, string>>> groupedData = LoadDataByROVIQSequence();
    //        List<string> keys = groupedData.Keys.ToList();
    //        //To get all questions by groupName

    //        Dictionary<string, Dictionary<string, string>> finalids = new Dictionary<string, Dictionary<string, string>>();
    //        //List<string> finalSection_Ques = new List<string>();
    //        //List<string> finalObs_Details = new List<string>();
    //        foreach (string key in keys)
    //        {
    //            if (key.ToLower().Contains(HeaderTextForROVIQ.text.ToString()))
    //            {
    //                Dictionary<string, Dictionary<string, string>> questions = groupedData[key];
    //                foreach (string qid in questions.Keys)
    //                {
    //                    if (!finalids.Keys.Contains(qid))
    //                    {
    //                        finalids.Add(qid, questions[qid]);
    //                    }
    //                }

    //            }
    //        }
    //        foreach (string id in finalids.Keys)
    //        {
    //            Dictionary<string, string> prop = finalids[id];

    //            if (!String.IsNullOrEmpty(prop["Selected_Answer"]) || prop["Selected_Answer"] != "")
    //            {
    //                string selectedanswer = prop["Selected_Answer"];
    //                int indexforselectedanswer = int.Parse(selectedanswer.Split('-')[0]);// KEEP the index number only, remove text after the "-".
    //                AnsweredQuesforROVIQ++;
    //            }
    //        }
    //        Debug.Log("Questiin Ids --" + string.Join(",", finalids.Keys));

    //        TotalQuesInChap.GetComponent<TextMeshProUGUI>().text = "Answered; " + AnsweredQuesforROVIQ.ToString() + "/" + finalids.Count.ToString();
    //    }
    //}
}
