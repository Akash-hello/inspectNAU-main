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

public class loadquestionfmObsTable : MonoBehaviour
{
    public string versiondetails;
    public GameObject CanvasPositionManager;

    public GameObject questionaccordion;

    public GameObject questionsitemprefab;
    public RectTransform ParentPanel_1;

    public GameObject standardphotoheaderaccordion;
    public GameObject standardphotoheaderprefab;

    //public GameObject errorpopup;
    //public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    int id;
    public int Inspection_Template_ID = 0;
    public GameObject InspPrimaryID;
    public int inspPrimaryID = 0;
    public int Cloud_DB_ParentID = 0;
    public GameObject ChapterDBID;
    public int chapterDBID = 0;
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
    public string Active = "";
    public string Timestamp = "";
    public string completionstatus = "";
    public string outputofsearchresult;
    public string searchresults;
    public int datacount;
    public GameObject Completionstatus;

    public TextMeshProUGUI selectedinspectionID;
    public TextMeshProUGUI ROVIQLOADTrueOrFalse;
    public TextMeshProUGUI Headertext;
    int AnsweredQuesforROVIQ;

    public Button PendingQuestions;
    bool pendingquestionsclicked;
    int pendingques;
    public string SuffixForPhotos;

    public string attachmentsfolderpath = "";
    List<FromDatabase_Insptemplates> fromdbList = new List<FromDatabase_Insptemplates>();
    public List<string> standardphotoheaders;

    int obscontainerheight = 0; //This is the height assigned if there are no SIRE Panels (Human, Process or Hardware) under this observation container.

    //public RectTransform QuestionsgroundRect;

    // Minimum height for the background image
    public float minHeight = 200f;

    // Height per line (e.g., 40 means each line adds 40 units)
    public float lineHeight = 60f;

    // Minimum height for the background image
    public float minHeighttex = 180f;

    // Height per line (e.g., 40 means each line adds 40 units)
    public float lineHeighttext = 50f;

    public bool smartsearch;
    public string QuestionIDS;

    Color Blue = new Color(0.0f, 0.447f, 0.6431f);
    Color Grey = new Color(0.8196f, 0.8196f, 0.8196f);

    public void LoadquestionsBtnClicked()

    {
        pendingquestionsclicked = false;
       
        pendingques = 0;
        if (ROVIQLOADTrueOrFalse.text == "True")
        {
            StartCoroutine(FetchROVIQ());
            PendingQuestions.enabled = false;
            //StartCoroutine(Loadquestions());
        }
        else
        {
            PendingQuestions.enabled = true;
            StartCoroutine(Loadquestions());
        }

    }

    public void PendingquestionsBtnClicked()

    {
        pendingquestionsclicked = true;
        pendingques = 0;
        if (ROVIQLOADTrueOrFalse.text == "True")
        {
            StartCoroutine(FetchROVIQ());
            //StartCoroutine(Loadquestions());
        }
        else
        {
            StartCoroutine(Loadquestions());
        }

    }

    public IEnumerator Loadquestions()

    {
        yield return null;
        attachmentsfolderpath = "";
        string Answermodelgroup = "";
        
        System.Data.IDataReader reader;
        inspPrimaryID = int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text);

        if (smartsearch == true)
        {
            chapterDBID = -1;
        }

        else
        {
            chapterDBID = int.Parse(ChapterDBID.GetComponent<TextMeshProUGUI>().text);
        }

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text));
        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        Answermodelgroup = mlocationdb1.answergroupmodel.ToString();
        
        Debug.Log(InspPrimaryID.GetComponent<TextMeshProUGUI>().text.ToString() + " Comes with Standard Photos" + mlocationdb1.standardphotos.ToString());
        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection1 = mlocationDb.getConnection();
        pendingques = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) !='Completed' and  cast(Inspection_PrimaryDetails_ID as int) = '" + inspPrimaryID + "' and  cast(Cloud_DB_ParentID as int) = '" + chapterDBID+"'");
        //pendingques = mlocationDb.totalRecords("Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) ='' and  cast(Inspection_PrimaryDetails_ID as int) = " + inspPrimaryID + " and  cast(Cloud_DB_ParentID as int) = " + chapterDBID);


        for (int i = 0; i < ParentPanel_1.transform.childCount; ++i)
        {
            Destroy(ParentPanel_1.transform.GetChild(i).gameObject);
        }

        if (pendingquestionsclicked == true)

        {
            //reader = mlocationDb.getquestionsData(chapterDBID, inspPrimaryID); //CLOUD DB ID IS the ID passed from above method, basically the CHAPTER ID
            string query = "Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Obs_Details_7) !='Completed' and  cast(Inspection_PrimaryDetails_ID as int) = " + inspPrimaryID + " and  cast(Cloud_DB_ParentID as int) = " + chapterDBID;
            //string query = "Inspection_Observations where trim(Obs_Details_8) = 'Question' and trim(Selected_Answer) ='' and  cast(Inspection_PrimaryDetails_ID as int) = " + inspPrimaryID + " and  cast(Cloud_DB_ParentID as int) = " + chapterDBID;

            reader = mlocationDb.pendingquestionsData(query);

        }

        else if (smartsearch == true)
        {
            string query1 = "Inspection_Observations where trim(Obs_Details_8) = 'Question' and cast(ID as int) in ("+QuestionIDS.Trim()+") and  cast(Inspection_PrimaryDetails_ID as int) = " + inspPrimaryID;
            reader = mlocationDb.SmartSearchQuestions(query1);
        }

        else
        {
            reader = mlocationDb.getquestionsData(chapterDBID, inspPrimaryID); //CLOUD DB ID IS the ID passed from above method, basically the CHAPTER ID

        }

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
            if (pendingquestionsclicked == true && myList.Count >0)
            {
                PendingQuestions.GetComponent<Image>().color = Color.yellow;
                //PendingQuestions.GetComponent<Button>().interactable = true;
            }

            else if (pendingques == 0)
            {
                PendingQuestions.GetComponent<Image>().color = Color.green;
                //PendingQuestions.GetComponent<Button>().interactable = false;
            }

            var output1 = JsonUtility.ToJson(entity, true);
            Debug.Log(output1);
            outputofsearchresult = output1.ToString();


            completionstatus = "CREATE COMPLETION STATUS FOR HERE";
            //versiondetails = "Version No.; " + x._Version_Number.ToString() + " Date; " + x._Version_Date.ToString();

            questionaccordion = Instantiate(questionsitemprefab);
            questionsprefab Question = questionaccordion.GetComponent<questionsprefab>();
            questionaccordion.transform.SetParent(ParentPanel_1, false);

            Question.InspectionPrimaryID.text = entity._Inspection_PrimaryDetails_ID.ToString();
            Question.QuestionDBid.text = entity._Cloud_DB_ID.ToString();
            Question.ChapterDBID.text = entity._Cloud_DB_ParentID.ToString();
            Question.AttachmentFolderpath.text = attachmentsfolderpath.ToString();
            Question.ShortQues.text = "Short Ques.: " + entity._ShortQuestionText_Sire;
            Question.ROVIQ.text = "ROVIQ: " + entity._ROVIQSequence_Sire;
            Question.VesselTypes.text = entity._VesselTypes_Sire;
            Question.Questions_Text.text = entity._Template_Section_Ques.ToString() + ": " + entity._Obs_Details_1.ToString();
            Question.Observation_Input_Area.text = entity._Observation_Text.ToString();
            Question.Save.GetComponent<SaveobsBtn>().TotalQuesInChap = Completionstatus;
            Question.Save.GetComponent<SaveobsBtn>().pendingquestions = PendingQuestions;

            Question.Questions_Text.ForceMeshUpdate(); //07th FEB 2025, ADJUSTED THE QUESTIONS PREFAB in HEIGHT AS PER THE NUMBER OF LINES

            // Get the number of lines in the text
            int lineCount = Question.Questions_Text.textInfo.lineCount;

            // Calculate the new height (minimum height or number of lines * lineHeight, whichever is greater)
            float newHeight = Mathf.Max(minHeight, lineCount * lineHeight +110.0f);
            float newHeight1 = Mathf.Max(minHeighttex, lineCount * lineHeighttext);

            Question.QuesbackgroundRect.GetComponent<LayoutElement>().preferredHeight = newHeight;
           
            Vector2 newSize = Question.QuesTextRect.sizeDelta;
            newSize.y = newHeight1;
            Question.QuesTextRect.sizeDelta = newSize;

            if (entity._Obs_Details_7.Trim() == "Completed")
            {
                Question.Save.GetComponent<SaveobsBtn>().statusindicator.GetComponent<Image>().color = Color.green;
            }

            

            if (!String.IsNullOrEmpty(entity._RiskCategory.ToString().Trim()) || entity._RiskCategory.ToString().Trim() != "")
            {
                Question.RiskCategory.text = entity._RiskCategory.ToString() + " Risk";
            }
            else
            {
                Color color = Question.RiskCategoryBackground.GetComponent<Image>().color;

                color.a = 0; // Set alpha (transparency)
                Question.RiskCategoryBackground.GetComponent<Image>().color = color;
                Question.RiskCategory.text = entity._RiskCategory.ToString();
            }


            Question.HeaaderTextForROVIQ.text = Headertext.text.ToString();
            Question.Date.text = entity._Date.ToString();
            Question.InspType.text = entity._Obs_Details_2.ToString().ToLower().Trim();

            if (entity._ExpectedEvidence.Trim().ToLower() == "mandatory")
            {
                Question.isphotomandatory = true;
                Question.Save.GetComponent<SaveobsBtn>().Photoismandatory = true;
            }

            else
            {
                Question.isphotomandatory = false;
                Question.Save.GetComponent<SaveobsBtn>().Photoismandatory = false;
            }
           
            if (entity._Obs_Details_2.ToString().ToLower().Trim().Replace(" ", "").Contains("(sire2.0)"))

            {
                Question.ObjectiveBtnNonSire.gameObject.SetActive(false);
               
                obscontainerheight = 0; //This is the height assigned if there are no SIRE Panels (Human, Process or Hardware) under this observation container.
                table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
                using var connection2 = mlocationDb2.getConnection();
                string humancheckifexists = "";
                humancheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(entity._Cloud_DB_ID.ToString()) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(entity._Inspection_PrimaryDetails_ID.ToString()) + " and trim(Attachment_Title) like '%HumanElement%';";
                using System.Data.IDataReader Obsrecordreader = mlocationDb2.SelectDataByquery(humancheckifexists);

                string Processcheckifexists = "";
                Processcheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(entity._Cloud_DB_ID.ToString()) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(entity._Inspection_PrimaryDetails_ID.ToString()) + " and trim(Attachment_Title) like '%ProcessElement%';";
                using System.Data.IDataReader Obsrecordreader1 = mlocationDb2.SelectDataByquery(Processcheckifexists);

                string hardwarecheckifexists = "";
                hardwarecheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(entity._Cloud_DB_ID.ToString()) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(entity._Inspection_PrimaryDetails_ID.ToString()) + " and trim(Attachment_Title) like '%HardwareElement%';";
                using System.Data.IDataReader Obsrecordreader2 = mlocationDb2.SelectDataByquery(hardwarecheckifexists);

                if (!String.IsNullOrEmpty(entity._ShortQuestionText_Sire.ToString().Trim()) || entity._ShortQuestionText_Sire.ToString().Trim() != "")
                {
                    Question.HumanBtn.enabled = true;
                    Question.HumanIcn.color = Blue;
                    Question.HumanPnl.SetActive(true);
                    Question.Hobservationdetail.text = "";
                    obscontainerheight +=1;

                    if (Obsrecordreader[3].ToString().Trim() == "HumanElement" && int.Parse(Obsrecordreader[7].ToString().Trim()) == 1)
                    {
                        Question.Hobservationdetail.text = Obsrecordreader[4].ToString().Trim();
                        Question.Humandeficiency.isOn = true;
                        Question.Humanasexpected.isOn = false;
                    }

                    else if (Obsrecordreader[3].ToString().Trim() == "HumanElement" && int.Parse(Obsrecordreader[7].ToString().Trim()) == 0)
                    {
                        Question.Hobservationdetail.text = Obsrecordreader[4].ToString().Trim();
                        Question.Humandeficiency.isOn = false;
                        Question.Humanasexpected.isOn = true;
                    }

                }
                else
                {
                    Question.HumanBtn.enabled = false;
                    Question.HumanIcn.color = Grey;
                    //Question.HumanPnl.SetActive(false);
                }

                if (!String.IsNullOrEmpty(entity._Publications_Sire.ToString().Trim()) || entity._Publications_Sire.ToString().Trim() != "")
                {
                    Question.ProcessBtn.enabled = true;
                    Question.ProcessIcn.color = Blue;
                    Question.ProcessPnl.SetActive(true);
                    Question.Pobservationdetail.text = "";
                    obscontainerheight += 1;


                    if (Obsrecordreader1[3].ToString().Trim() == "ProcessElement" && int.Parse(Obsrecordreader1[7].ToString().Trim()) == 1)
                    {
                        Question.Pobservationdetail.text = Obsrecordreader1[4].ToString().Trim();
                        Question.Processdeficiency.isOn = true;
                        Question.Processasexpected.isOn = false;
                    }

                    else if (Obsrecordreader1[3].ToString().Trim() == "ProcessElement" && int.Parse(Obsrecordreader1[7].ToString().Trim()) == 0)
                    {
                        Question.Pobservationdetail.text = Obsrecordreader1[4].ToString().Trim();
                        Question.Processdeficiency.isOn =  false;
                        Question.Processasexpected.isOn =  true;
                    }

                }
                else
                {
                    Question.ProcessBtn.enabled = false;
                    Question.ProcessIcn.color = Grey;
                    Question.ProcessPnl.SetActive(false);
                }


                if (!String.IsNullOrEmpty(entity._IndustryGuidance.ToString().Trim()) || entity._IndustryGuidance.ToString().Trim() != "")
                {
                    Question.HardwareBtn.enabled = true;
                    Question.HardwareIcn.color = Blue;
                    Question.HardwarePnl.SetActive(true);
                    Question.Hardobservationdetail.text = "";
                     obscontainerheight += 1;

                    if (Obsrecordreader2[3].ToString().Trim() == "HardwareElement" && int.Parse(Obsrecordreader2[7].ToString().Trim()) == 1)
                    {
                        Question.Hardobservationdetail.text = Obsrecordreader2[4].ToString().Trim();
                        Question.Hardwaredeficiency.isOn = true;
                        Question.Hardwareasexpected.isOn = false;
                    }

                    else if (Obsrecordreader2[3].ToString().Trim() == "HardwareElement" && int.Parse(Obsrecordreader2[7].ToString().Trim()) == 0)
                    {
                        Question.Hardobservationdetail.text = Obsrecordreader2[4].ToString().Trim();
                        Question.Hardwaredeficiency.isOn = false;
                        Question.Hardwareasexpected.isOn = true;
                    }

                }
                else
                {
                    Question.HardwareBtn.enabled = false;
                    Question.HardwareIcn.color = Grey;
                    Question.HardwarePnl.SetActive(false);
                }

                Question.ObservationContainer.GetComponent<LayoutElement>().preferredHeight = obscontainerheight * 300f;

                //if (Question.HumanPnl.activeSelf || Question.ProcessPnl.activeSelf|| Question.HardwarePnl.activeSelf)
                //{
                //    Question.ObservationContainer.SetActive(true);
                //    Question.ObservationContainer.GetComponent<LayoutElement>().preferredHeight = obscontainerheight * 300f;

                //}
                //else

                //{
                //    Question.ObservationContainer.SetActive(false);
                //    Question.ObservationContainer.GetComponent<LayoutElement>().preferredHeight = obscontainerheight * 300f;
                //}


                if ((!String.IsNullOrEmpty(entity._Objective_Sire.ToString().Trim()) || entity._Objective_Sire.ToString().Trim() != "") ||
                    (!String.IsNullOrEmpty(entity._IMOISMCode.ToString().Trim()) || entity._IMOISMCode.ToString().Trim() != ""))
                {
                    Question.ObjectiveBtn.enabled = true;
                    Question.ObjectiveIcn.color = Blue;
                }
                else
                {
                    Question.ObjectiveBtn.enabled = false;
                    Question.ObjectiveIcn.color = Grey;
                }

                if (!String.IsNullOrEmpty(entity._TMSAKPI.ToString().Trim()) || entity._TMSAKPI.ToString().Trim() != "")
                {
                    Question.TMSABTN.enabled = true;
                    Question.TMSAIcn.color = Blue;
                }
                else
                {
                    Question.TMSABTN.enabled = false;
                    Question.TMSAIcn.color = Grey;
                }

                if (!String.IsNullOrEmpty(entity._InspectionGuidance.ToString().Trim()) || entity._InspectionGuidance.ToString().Trim() != "")
                {
                    Question.TaggedRanks.text = entity._InspectionGuidance.ToString().Trim();

                }
                else
                {
                    Question.TaggedRanks.text = "--";
                }

                if (!String.IsNullOrEmpty(entity._SuggestedInspectorActions.ToString().Trim()) || entity._InspectionGuidance.ToString().Trim() != "")
                {

                    if (entity._SuggestedInspectorActions.ToString().Trim() == "C")
                    {
                        Question.Corequestion.SetActive(true);
                    }

                    if (entity._SuggestedInspectorActions.ToString().Trim() == "R1")
                    {
                        Question.Rotation1.SetActive(true);
                    }

                    if (entity._SuggestedInspectorActions.ToString().Trim() == "R2")
                    {
                        Question.Rotation2.SetActive(true);
                    }

                    else

                    {
                        Question.Defaulticon.SetActive(true);
                    }
                }
                else
                {
                    Question.Defaulticon.SetActive(true);
                }
                //mlocationDb2.close();
            }
            else
            {
               
                if ((!String.IsNullOrEmpty(entity._Objective_Sire.ToString().Trim()) || entity._Objective_Sire.ToString().Trim() != "") ||
                    (!String.IsNullOrEmpty(entity._IMOISMCode.ToString().Trim()) || entity._IMOISMCode.ToString().Trim() != ""))
                {
                    Question.ObjectiveBtnNonSire.gameObject.SetActive(true);

                    Question.ObjectiveBtnNonSire.enabled = true;
                    Question.ObjectiveIcnNonSire.color = Blue;
                }
                else
                {
                    Question.ObjectiveBtnNonSire.gameObject.SetActive(false) ;
                    Question.ObjectiveBtnNonSire.enabled = false;
                    Question.ObjectiveIcnNonSire.color = Grey;
                }
                Debug.Log("This is not a Sire20 inspection.");
            }

            //23 ShortQuestionText_Sire = ""; // USE For CATEGORY - Human
            //26  Publications_Sire = ""; // USE For CATEGORY - Process
            //28 IndustryGuidance = "";  // USE For CATEGORY - Hardware


            //27 Objective_Sire = "";    //OBJECTIVE
            //29 TMSAKPI = "";           //TMSA KPI
            //30 IMOISMCode = "";        //SMS Ref
            //31 InspectionGuidance = "";//Tagged Rank
            //32 SuggestedInspectorActions = ""; // Question Type; C / R1 / R2
            //33 ExpectedEvidence = ""; //USE FOR MANDATORY PHOTOGRAPH


            var i1 = 0;
                var j1 = 0;

                if (Answermodelgroup.Trim() == "2")
                {
                    i1 = 0;
                    j1 = 5;
                }
                else if (Answermodelgroup.Trim() == "1")
                {
                    i1 = 5;
                    j1 = 9;
                }
                else if (Answermodelgroup.Trim() == "3")
                {
                    i1 = 9;
                    j1 = 14;
                }

                else
                {
                    i1 = 0;
                    j1 = 5;
                }
                for (var i = i1; i < j1; i++)
                {
                    Question.answergroupmodels[i].SetActive(true);
                }
            
            
            if (!String.IsNullOrEmpty(entity._Selected_Answer) || entity._Selected_Answer != "")
            {
                string selectedanswer = entity._Selected_Answer;
                int indexforselectedanswer = int.Parse(selectedanswer.Split('-')[0]);// KEEP the index number only, remove text after the "-".

                if (indexforselectedanswer !=999)
                {
                    Question.Save.GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
                }
                Question.Save.GetComponent<SaveobsBtn>().ROVIQTrueFalse = false;
                //questionaccordion.transform.Find("Questions_(List_Item)/Observation_Content_Holder/Observation_Container/SaveBtnBck/SaveBtn").GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
            }

        }
        reader.Close();
        reader.Dispose();
        searchresults = JsonUtility.ToJson(myList, true);

        datacount = myList.Count;
        Debug.Log("Total questions " + datacount);

        
        pendingquestionsclicked = false;
        //mlocationDb.close();
        //mlocationdb1.close();

        if (chapterDBID == -1)
        {
            smartsearch = true;
        }
        else
        {
            smartsearch = false;
        }
    }


    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> LoadDataByROVIQSequence() //Used for Getting ROVIQ.
    {
        
        table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        using var connection2 = mlocationDb2.getConnection();
        //string columndataquery = "SELECT Cloud_DB_ID,Template_Section_Ques,Obs_Details_1,ROVIQSequence FROM Inspection_Observations where TRIM(Obs_Details_8) ='Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text);
        string columndataquery = "SELECT *FROM Inspection_Observations where TRIM(Obs_Details_8) ='Question' and cast(Inspection_PrimaryDetails_ID as int) = " + int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text) + " ORDER BY " +
    "CAST(SUBSTR(Template_Section_Ques || '.', 1,INSTR(Template_Section_Ques || '.', '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1, INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(Template_Section_Ques || '.',INSTR(SUBSTR(Template_Section_Ques || '.',INSTR(Template_Section_Ques || '.', '.') + 1), '.') + 1) AS INTEGER) ASC;";

        using System.Data.IDataReader reader = mlocationDb2.getDatabyQuery(columndataquery);
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> myList = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        while (reader.Read())
        {  
            List<string> keys = reader[25].ToString().Split(',').Select(x => x.Trim().ToLower()).ToList();
            foreach (string key in keys)
            {
                if (myList.Keys.Contains(key))
                {
                    Dictionary<string, Dictionary<string, string>> questions = myList[key];
                    if (!questions.Keys.Contains(reader[2].ToString().Trim()))
                    {
                        Dictionary<string, string> props = new Dictionary<string, string>();

                        props.Add("Inspection_PrimaryDetails_ID", reader[1].ToString().Trim());
                        props.Add("Cloud_DB_ID", reader[2].ToString().Trim());
                        props.Add("Cloud_DB_ParentID", reader[3].ToString().Trim());
                        props.Add("Template_Section_Ques", reader[6].ToString().Trim());
                        props.Add("Selected_Answer", reader[7].ToString().Trim());
                        props.Add("Date", reader[8].ToString().Trim());
                        props.Add("Observation_Text", reader[11].ToString().Trim());
                        props.Add("RiskCategory", reader[12].ToString().Trim());
                        props.Add("Obs_Details_1", reader[15].ToString().Trim());
                        props.Add("Obs_Details_2", reader[16].ToString().Trim());
                        props.Add("Obs_Details_7", reader[21].ToString().Trim());

                        props.Add("ShortQuestionText", reader[23].ToString().Trim());
                        props.Add("VesselTypes", reader[24].ToString().Trim());
                        props.Add("ROVIQSequence", reader[25].ToString().Trim());

                        props.Add("Human", reader[23].ToString().Trim());
                        props.Add("Process", reader[26].ToString().Trim());
                        props.Add("Hardware", reader[28].ToString().Trim());
                        props.Add("Objective_Sire", reader[27].ToString().Trim());
                        props.Add("TMSAKPI", reader[29].ToString().Trim());
                        props.Add("IMOISMCode", reader[30].ToString().Trim());
                        props.Add("TaggedRank", reader[31].ToString().Trim());
                        props.Add("C1R1R2", reader[32].ToString().Trim());
                        props.Add("Photomandatory", reader[33].ToString().Trim());
                      

                        //props.Add("Cloud_DB_ID", reader[0].ToString().Trim());
                        //props.Add("Template_Section_Ques", reader[1].ToString().Trim());
                        //props.Add("Obs_Details_1", reader[2].ToString().Trim());
                        questions.Add(reader[2].ToString().Trim(), props);


                    }
                }
                else
                {
                    Dictionary<string, string> props = new Dictionary<string, string>();

                    props.Add("Inspection_PrimaryDetails_ID", reader[1].ToString().Trim());
                    props.Add("Cloud_DB_ID", reader[2].ToString().Trim());
                    props.Add("Cloud_DB_ParentID", reader[3].ToString().Trim());
                    props.Add("Template_Section_Ques", reader[6].ToString().Trim());
                    props.Add("Selected_Answer", reader[7].ToString().Trim());
                    props.Add("Date", reader[8].ToString().Trim());
                    props.Add("Observation_Text", reader[11].ToString().Trim());
                    props.Add("RiskCategory", reader[12].ToString().Trim());
                    props.Add("Obs_Details_1", reader[15].ToString().Trim());
                    props.Add("Obs_Details_2", reader[16].ToString().Trim());
                    props.Add("Obs_Details_7", reader[21].ToString().Trim());
                    props.Add("ShortQuestionText", reader[23].ToString().Trim());
                    props.Add("VesselTypes", reader[24].ToString().Trim());
                    props.Add("ROVIQSequence", reader[25].ToString().Trim());

                    props.Add("Human", reader[23].ToString().Trim());
                    props.Add("Process", reader[26].ToString().Trim());
                    props.Add("Hardware", reader[28].ToString().Trim());
                    props.Add("Objective_Sire", reader[27].ToString().Trim());
                    props.Add("TMSAKPI", reader[29].ToString().Trim());
                    props.Add("IMOISMCode", reader[30].ToString().Trim());
                    props.Add("TaggedRank", reader[31].ToString().Trim());
                    props.Add("C1R1R2", reader[32].ToString().Trim());
                    props.Add("Photomandatory", reader[33].ToString().Trim());

                    //props.Add("Cloud_DB_ID", reader[0].ToString().Trim());
                    //props.Add("Template_Section_Ques", reader[1].ToString().Trim());
                    //props.Add("Obs_Details_1", reader[2].ToString().Trim());

                    Dictionary<string, Dictionary<string, string>> questions = new Dictionary<string, Dictionary<string, string>>();
                    questions.Add(reader[2].ToString().Trim(), props);
                    myList.Add(key, questions);
                }
            }
        }
        reader.Dispose();
        //mlocationDb2.close();
        return myList;
       
    }

    public IEnumerator FetchROVIQ()
    {
        yield return null;

        attachmentsfolderpath = "";
        string Answermodelgroup = "";
        //AnsweredQuesforROVIQ = 0;
        

        for (int i = 0; i < ParentPanel_1.transform.childCount; ++i)
        {
            Destroy(ParentPanel_1.transform.GetChild(i).gameObject);
        }

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text));
        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        Answermodelgroup = mlocationdb1.answergroupmodel.ToString();

        // To Get all ROVIQSequence list        
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> groupedData = LoadDataByROVIQSequence();
        List<string> keys = groupedData.Keys.ToList();
        //To get all questions by groupName

        Dictionary<string, Dictionary<string, string>> finalids = new Dictionary<string, Dictionary<string, string>>();
        //List<string> finalSection_Ques = new List<string>();
        //List<string> finalObs_Details = new List<string>();
        foreach (string key in keys)
        {
            if (key.ToLower().Contains(Headertext.text.ToString())) //PASSING THE ROVIQ HEADER TEXT HERE...
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
        Debug.Log("Questiin Ids --" + string.Join(",", finalids.Keys));
        foreach (string id in finalids.Keys)
        {
            Dictionary<string, string> prop = finalids[id];


            string Template_Section_Ques = prop["Template_Section_Ques"].Trim();
            string Obs_Details_1 = prop["Obs_Details_1"].Trim();
            string Obs_Details_2 = prop["Obs_Details_2"].Trim();
            string completionstatus = prop["Obs_Details_7"].Trim();

            questionaccordion = Instantiate(questionsitemprefab);
            questionsprefab Question = questionaccordion.GetComponent<questionsprefab>();
            questionaccordion.transform.SetParent(ParentPanel_1, false);

            Question.Questions_Text.text = Template_Section_Ques;
            Question.Observation_Input_Area.text = Obs_Details_1;

            Question.InspectionPrimaryID.text = prop["Inspection_PrimaryDetails_ID"];
            Question.QuestionDBid.text = prop["Cloud_DB_ID"];
            Question.ChapterDBID.text = prop["Cloud_DB_ParentID"];
            Question.AttachmentFolderpath.text = attachmentsfolderpath.ToString();
            Question.ShortQues.text = "Short Ques.: " + prop["ShortQuestionText"];
            Question.ROVIQ.text = "ROVIQ: " + prop["ROVIQSequence"];
            Question.VesselTypes.text = prop["VesselTypes"];
            Question.Questions_Text.text = prop["Template_Section_Ques"] + ": " + prop["Obs_Details_1"];
            Question.Observation_Input_Area.text = prop["Observation_Text"];
            
            Question.Save.GetComponent<SaveobsBtn>().TotalQuesInChap = Completionstatus;
            Question.Save.GetComponent<SaveobsBtn>().ROVIQTrueFalse = true;

            Question.Questions_Text.ForceMeshUpdate(); //07th FEB 2025, ADJUSTED THE QUESTIONS PREFAB in HEIGHT AS PER THE NUMBER OF LINES

            // Get the number of lines in the text
            int lineCount = Question.Questions_Text.textInfo.lineCount;

            // Calculate the new height (minimum height or number of lines * lineHeight, whichever is greater)
            float newHeight = Mathf.Max(minHeight, lineCount * lineHeight+ 110.0f);

            Question.QuesbackgroundRect.GetComponent<LayoutElement>().preferredHeight = newHeight;

            Vector2 newSize = Question.QuesTextRect.sizeDelta;
            newSize.y = newHeight;
            Question.QuesTextRect.sizeDelta = newSize;

            if (completionstatus.Trim() == "Completed")
            {
                Question.Save.GetComponent<SaveobsBtn>().statusindicator.GetComponent<Image>().color = Color.green;
            }

            if (!String.IsNullOrEmpty(prop["RiskCategory"])|| prop["RiskCategory"] != "")
            {
                Question.RiskCategory.text = prop["RiskCategory"] + " Risk";
            }
            else
            {
                Color color = Question.RiskCategoryBackground.GetComponent<Image>().color;

                color.a = 0; // Set alpha (transparency)
                Question.RiskCategoryBackground.GetComponent<Image>().color = color;
                Question.RiskCategory.text = prop["RiskCategory"];
            }

            Question.HeaaderTextForROVIQ.text = Headertext.text.ToString();
            Question.Date.text = prop["Date"];
            Question.InspType.text = Obs_Details_2.ToLower().Trim();

            if (Obs_Details_2.ToString().ToLower().Trim().Replace(" ", "").Contains("(sire2.0)"))
            {
                Question.ObjectiveBtnNonSire.gameObject.SetActive(false);

                obscontainerheight = 0;
                table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
                using var connection2 = mlocationDb2.getConnection();
                string humancheckifexists = "";
                humancheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(prop["Cloud_DB_ID"]) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(prop["Inspection_PrimaryDetails_ID"]) + " and trim(Attachment_Title) like '%HumanElement%';";
                using System.Data.IDataReader Obsrecordreader = mlocationDb2.SelectDataByquery(humancheckifexists);

                string Processcheckifexists = "";
                Processcheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(prop["Cloud_DB_ID"]) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(prop["Inspection_PrimaryDetails_ID"]) + " and trim(Attachment_Title) like '%ProcessElement%';";
                using System.Data.IDataReader Obsrecordreader1 = mlocationDb2.SelectDataByquery(Processcheckifexists);

                string hardwarecheckifexists = "";
                hardwarecheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(prop["Cloud_DB_ID"]) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(prop["Inspection_PrimaryDetails_ID"]) + " and trim(Attachment_Title) like '%HardwareElement%';";
                using System.Data.IDataReader Obsrecordreader2 = mlocationDb2.SelectDataByquery(hardwarecheckifexists);

                if (!String.IsNullOrEmpty(prop["Human"]) || prop["Human"] != "")
                {
                    Question.HumanBtn.enabled = true;
                    Question.HumanIcn.color = Blue;
                    Question.HumanPnl.SetActive(true);
                    Question.Hobservationdetail.text = "";
                    obscontainerheight +=1;

                    if (Obsrecordreader[3].ToString().Trim() == "HumanElement" && int.Parse(Obsrecordreader[7].ToString().Trim()) == 1)
                    {
                        Question.Hobservationdetail.text = Obsrecordreader[4].ToString().Trim();
                        Question.Humandeficiency.isOn = true;
                        Question.Humanasexpected.isOn = false;
                    }

                    else if (Obsrecordreader[3].ToString().Trim() == "HumanElement" && int.Parse(Obsrecordreader[7].ToString().Trim()) == 0)
                    {
                        Question.Hobservationdetail.text = Obsrecordreader[4].ToString().Trim();
                        Question.Humandeficiency.isOn = false;
                        Question.Humanasexpected.isOn = true;
                    }

                }
                else
                {
                    Question.HumanBtn.enabled = false;
                    Question.HumanIcn.color = Grey;
                    Question.HumanPnl.SetActive(false);
                }

                if (!String.IsNullOrEmpty(prop["Process"]) || prop["Process"] != "")
                {
                    Question.ProcessBtn.enabled = true;
                    Question.ProcessIcn.color = Blue;
                    Question.ProcessPnl.SetActive(true);
                    Question.Pobservationdetail.text = "";
                    obscontainerheight += 1;

                    if (Obsrecordreader1[3].ToString().Trim() == "ProcessElement" && int.Parse(Obsrecordreader1[7].ToString().Trim()) == 1)
                    {
                        Question.Pobservationdetail.text = Obsrecordreader1[4].ToString().Trim();
                        Question.Processdeficiency.isOn = true;
                        Question.Processasexpected.isOn = false;
                    }

                    else if (Obsrecordreader1[3].ToString().Trim() == "ProcessElement" && int.Parse(Obsrecordreader1[7].ToString().Trim()) == 0)
                    {
                        Question.Pobservationdetail.text = Obsrecordreader1[4].ToString().Trim();
                        Question.Processdeficiency.isOn = false;
                        Question.Processasexpected.isOn = true;
                    }
                }
                else
                {
                    Question.ProcessBtn.enabled = false;
                    Question.ProcessIcn.color = Grey;
                    Question.ProcessPnl.SetActive(false);
                }

                if (!String.IsNullOrEmpty(prop["Hardware"]) || prop["Hardware"] != "")
                {
                    Question.HardwareBtn.enabled = true;
                    Question.HardwareIcn.color = Blue;
                    Question.HardwarePnl.SetActive(true);
                    Question.Hardobservationdetail.text = "";
                    obscontainerheight += 1;

                    if (Obsrecordreader2[3].ToString().Trim() == "HardwareElement" && int.Parse(Obsrecordreader2[7].ToString().Trim()) == 1)
                    {
                        Question.Hardobservationdetail.text = Obsrecordreader2[4].ToString().Trim();
                        Question.Hardwaredeficiency.isOn = true;
                        Question.Hardwareasexpected.isOn = false;
                    }

                    else if (Obsrecordreader2[3].ToString().Trim() == "HardwareElement" && int.Parse(Obsrecordreader2[7].ToString().Trim()) == 0)
                    {
                        Question.Hardobservationdetail.text = Obsrecordreader2[4].ToString().Trim();
                        Question.Hardwaredeficiency.isOn = false;
                        Question.Hardwareasexpected.isOn = true;
                    }
                }
                else
                {
                    Question.HardwareBtn.enabled = false;
                    Question.HardwareIcn.color = Grey;
                    Question.HardwarePnl.SetActive(false);
                }

                Question.ObservationContainer.GetComponent<LayoutElement>().preferredHeight = obscontainerheight * 300f;


                if ((!String.IsNullOrEmpty(prop["Objective_Sire"]) || prop["Objective_Sire"] != "")
                    ||
                    (!String.IsNullOrEmpty(prop["IMOISMCode"]) || prop["IMOISMCode"] != ""))
                {
                    Question.ObjectiveBtn.enabled = true;
                    Question.ObjectiveIcn.color = Blue;
                }
                else
                {
                    Question.ObjectiveBtn.enabled = false;
                    Question.ObjectiveIcn.color = Grey;
                }

                if (!String.IsNullOrEmpty(prop["TMSAKPI"]) || prop["TMSAKPI"] != "")
                {
                    Question.TMSABTN.enabled = true;
                    Question.TMSAIcn.color = Blue;
                }
                else
                {
                    Question.TMSABTN.enabled = false;
                    Question.TMSAIcn.color = Grey;
                }

                if (!String.IsNullOrEmpty(prop["TaggedRank"]) || prop["TaggedRank"] != "")
                {
                    Question.TaggedRanks.text = prop["TaggedRank"];

                }
                else
                {
                    Question.TaggedRanks.text = "--";
                }

                if (prop["Photomandatory"].Trim().ToLower() == "mandatory")
                {
                    Question.isphotomandatory = true;
                    Question.Save.GetComponent<SaveobsBtn>().Photoismandatory = true;
                }

                else
                {
                    Question.isphotomandatory = false;
                    Question.Save.GetComponent<SaveobsBtn>().Photoismandatory = false;
                }

                if (!String.IsNullOrEmpty(prop["C1R1R2"]) || prop["C1R1R2"] != "")
                {

                    if (prop["C1R1R2"].ToString().Trim() == "C")
                    {
                        Question.Corequestion.SetActive(true);
                    }

                    if (prop["C1R1R2"].ToString().Trim() == "R1")
                    {
                        Question.Rotation1.SetActive(true);
                    }

                    if (prop["C1R1R2"].ToString().Trim() == "R2")
                    {
                        Question.Rotation2.SetActive(true);
                    }

                    else

                    {
                        Question.Defaulticon.SetActive(true);
                    }

                }
                else
                {
                    Question.Defaulticon.SetActive(true);
                }
                mlocationDb2.close();
            }
            else
            {

                if ((!String.IsNullOrEmpty(prop["Objective_Sire"]) || prop["Objective_Sire"] != "")
                    ||
                    (!String.IsNullOrEmpty(prop["IMOISMCode"]) || prop["IMOISMCode"] != ""))
                {
                    Question.ObjectiveBtnNonSire.gameObject.SetActive(true);

                    Question.ObjectiveBtnNonSire.enabled = true;
                    Question.ObjectiveIcnNonSire.color = Blue;
                }
                else
                {
                    Question.ObjectiveBtnNonSire.gameObject.SetActive(false);
                    Question.ObjectiveBtnNonSire.enabled = false;
                    Question.ObjectiveIcnNonSire.color = Grey;
                }


                Debug.Log("This is not a Sire20 inspection.");
            }

            var i1 = 0;
            var j1 = 0;

            if (Answermodelgroup.Trim() == "2")
            {
                i1 = 0;
                j1 = 5;
            }
            else if (Answermodelgroup.Trim() == "1")
            {
                i1 = 5;
                j1 = 9;
            }
            else if (Answermodelgroup.Trim() == "3")
            {
                i1 = 9;
                j1 = 14;
            }

            else
            {
                i1 = 0;
                j1 = 5;
            }
            for (var i = i1; i < j1; i++)
            {
                Question.answergroupmodels[i].SetActive(true);
            }

            if (!String.IsNullOrEmpty(prop["Selected_Answer"]) || prop["Selected_Answer"] != "") //24th Aug check this method for its property and sort out.
            {
                string selectedanswer = prop["Selected_Answer"];
                int indexforselectedanswer = int.Parse(selectedanswer.Split('-')[0]);// KEEP the index number only, remove text after the "-".
                //AnsweredQuesforROVIQ++;

                if (indexforselectedanswer != 999)
                {
                    Question.Save.GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
                }

                //Question.Save.GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
                //questionaccordion.transform.Find("Questions_(List_Item)/Observation_Content_Holder/Observation_Container/SaveBtnBck/SaveBtn").GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
            }

            //Completionstatus.GetComponent<TextMeshProUGUI>().text = "Answered; " + AnsweredQuesforROVIQ.ToString() + "/" + finalids.Count.ToString();
            //24th FEB commented above line and 'AnsweredQuesforROVIQ' as this was calculating total answered basis number of answered questions instead of actual completed status.
        }
        //finalids contains all questionids
        //To get other properties of question
        //string Template_Section_Ques = questions["839"]["Template_Section_Ques"];//questionids[0]
        //string Obs_Details_1 = questions["839"]["Obs_Details_1"];
        //mlocationdb1.close();

    }





    public IEnumerator SmartSearchQuestions()
    {
        yield return null;

        attachmentsfolderpath = "";
        string Answermodelgroup = "";
        //AnsweredQuesforROVIQ = 0;


        for (int i = 0; i < ParentPanel_1.transform.childCount; ++i)
        {
            Destroy(ParentPanel_1.transform.GetChild(i).gameObject);
        }

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text));
        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        Answermodelgroup = mlocationdb1.answergroupmodel.ToString();

        // To Get all ROVIQSequence list        
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> groupedData = LoadDataByROVIQSequence();
        List<string> keys = groupedData.Keys.ToList();
        //To get all questions by groupName

        Dictionary<string, Dictionary<string, string>> finalids = new Dictionary<string, Dictionary<string, string>>();
        //List<string> finalSection_Ques = new List<string>();
        //List<string> finalObs_Details = new List<string>();
        foreach (string key in keys)
        {
            if (key.ToLower().Contains(Headertext.text.ToString())) //PASSING THE ROVIQ HEADER TEXT HERE...
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
        Debug.Log("Questiin Ids --" + string.Join(",", finalids.Keys));
        foreach (string id in finalids.Keys)
        {
            Dictionary<string, string> prop = finalids[id];


            string Template_Section_Ques = prop["Template_Section_Ques"].Trim();
            string Obs_Details_1 = prop["Obs_Details_1"].Trim();
            string Obs_Details_2 = prop["Obs_Details_2"].Trim();
            string completionstatus = prop["Obs_Details_7"].Trim();

            questionaccordion = Instantiate(questionsitemprefab);
            questionsprefab Question = questionaccordion.GetComponent<questionsprefab>();
            questionaccordion.transform.SetParent(ParentPanel_1, false);

            Question.Questions_Text.text = Template_Section_Ques;
            Question.Observation_Input_Area.text = Obs_Details_1;

            Question.InspectionPrimaryID.text = prop["Inspection_PrimaryDetails_ID"];
            Question.QuestionDBid.text = prop["Cloud_DB_ID"];
            Question.ChapterDBID.text = prop["Cloud_DB_ParentID"];
            Question.AttachmentFolderpath.text = attachmentsfolderpath.ToString();
            Question.ShortQues.text = "Short Ques.: " + prop["ShortQuestionText"];
            Question.ROVIQ.text = "ROVIQ: " + prop["ROVIQSequence"];
            Question.VesselTypes.text = prop["VesselTypes"];
            Question.Questions_Text.text = prop["Template_Section_Ques"] + ": " + prop["Obs_Details_1"];
            Question.Observation_Input_Area.text = prop["Observation_Text"];

            Question.Save.GetComponent<SaveobsBtn>().TotalQuesInChap = Completionstatus;
            Question.Save.GetComponent<SaveobsBtn>().ROVIQTrueFalse = true;

            Question.Questions_Text.ForceMeshUpdate(); //07th FEB 2025, ADJUSTED THE QUESTIONS PREFAB in HEIGHT AS PER THE NUMBER OF LINES

            // Get the number of lines in the text
            int lineCount = Question.Questions_Text.textInfo.lineCount;

            // Calculate the new height (minimum height or number of lines * lineHeight, whichever is greater)
            float newHeight = Mathf.Max(minHeight, lineCount * lineHeight + 110.0f);

            Question.QuesbackgroundRect.GetComponent<LayoutElement>().preferredHeight = newHeight;

            Vector2 newSize = Question.QuesTextRect.sizeDelta;
            newSize.y = newHeight;
            Question.QuesTextRect.sizeDelta = newSize;

            if (completionstatus.Trim() == "Completed")
            {
                Question.Save.GetComponent<SaveobsBtn>().statusindicator.GetComponent<Image>().color = Color.green;
            }

            if (!String.IsNullOrEmpty(prop["RiskCategory"]) || prop["RiskCategory"] != "")
            {
                Question.RiskCategory.text = prop["RiskCategory"] + " Risk";
            }
            else
            {
                Color color = Question.RiskCategoryBackground.GetComponent<Image>().color;

                color.a = 0; // Set alpha (transparency)
                Question.RiskCategoryBackground.GetComponent<Image>().color = color;
                Question.RiskCategory.text = prop["RiskCategory"];
            }

            Question.HeaaderTextForROVIQ.text = Headertext.text.ToString();
            Question.Date.text = prop["Date"];
            Question.InspType.text = Obs_Details_2.ToLower().Trim();

            if (Obs_Details_2.ToString().ToLower().Trim().Replace(" ", "").Contains("(sire2.0)"))
            {
                Question.ObjectiveBtnNonSire.gameObject.SetActive(false);

                obscontainerheight = 0;
                table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
                using var connection2 = mlocationDb2.getConnection();
                string humancheckifexists = "";
                humancheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(prop["Cloud_DB_ID"]) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(prop["Inspection_PrimaryDetails_ID"]) + " and trim(Attachment_Title) like '%HumanElement%';";
                using System.Data.IDataReader Obsrecordreader = mlocationDb2.SelectDataByquery(humancheckifexists);

                string Processcheckifexists = "";
                Processcheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(prop["Cloud_DB_ID"]) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(prop["Inspection_PrimaryDetails_ID"]) + " and trim(Attachment_Title) like '%ProcessElement%';";
                using System.Data.IDataReader Obsrecordreader1 = mlocationDb2.SelectDataByquery(Processcheckifexists);

                string hardwarecheckifexists = "";
                hardwarecheckifexists = " cast(Inspection_Observations_ID as int)  = " + int.Parse(prop["Cloud_DB_ID"]) + " and cast(Inspection_PrimaryDetails_ID as int)  = " + int.Parse(prop["Inspection_PrimaryDetails_ID"]) + " and trim(Attachment_Title) like '%HardwareElement%';";
                using System.Data.IDataReader Obsrecordreader2 = mlocationDb2.SelectDataByquery(hardwarecheckifexists);

                if (!String.IsNullOrEmpty(prop["Human"]) || prop["Human"] != "")
                {
                    Question.HumanBtn.enabled = true;
                    Question.HumanIcn.color = Blue;
                    Question.HumanPnl.SetActive(true);
                    Question.Hobservationdetail.text = "";
                    obscontainerheight += 1;

                    if (Obsrecordreader[3].ToString().Trim() == "HumanElement" && int.Parse(Obsrecordreader[7].ToString().Trim()) == 1)
                    {
                        Question.Hobservationdetail.text = Obsrecordreader[4].ToString().Trim();
                        Question.Humandeficiency.isOn = true;
                        Question.Humanasexpected.isOn = false;
                    }

                    else if (Obsrecordreader[3].ToString().Trim() == "HumanElement" && int.Parse(Obsrecordreader[7].ToString().Trim()) == 0)
                    {
                        Question.Hobservationdetail.text = Obsrecordreader[4].ToString().Trim();
                        Question.Humandeficiency.isOn = false;
                        Question.Humanasexpected.isOn = true;
                    }

                }
                else
                {
                    Question.HumanBtn.enabled = false;
                    Question.HumanIcn.color = Grey;
                    Question.HumanPnl.SetActive(false);
                }

                if (!String.IsNullOrEmpty(prop["Process"]) || prop["Process"] != "")
                {
                    Question.ProcessBtn.enabled = true;
                    Question.ProcessIcn.color = Blue;
                    Question.ProcessPnl.SetActive(true);
                    Question.Pobservationdetail.text = "";
                    obscontainerheight += 1;

                    if (Obsrecordreader1[3].ToString().Trim() == "ProcessElement" && int.Parse(Obsrecordreader1[7].ToString().Trim()) == 1)
                    {
                        Question.Pobservationdetail.text = Obsrecordreader1[4].ToString().Trim();
                        Question.Processdeficiency.isOn = true;
                        Question.Processasexpected.isOn = false;
                    }

                    else if (Obsrecordreader1[3].ToString().Trim() == "ProcessElement" && int.Parse(Obsrecordreader1[7].ToString().Trim()) == 0)
                    {
                        Question.Pobservationdetail.text = Obsrecordreader1[4].ToString().Trim();
                        Question.Processdeficiency.isOn = false;
                        Question.Processasexpected.isOn = true;
                    }
                }
                else
                {
                    Question.ProcessBtn.enabled = false;
                    Question.ProcessIcn.color = Grey;
                    Question.ProcessPnl.SetActive(false);
                }

                if (!String.IsNullOrEmpty(prop["Hardware"]) || prop["Hardware"] != "")
                {
                    Question.HardwareBtn.enabled = true;
                    Question.HardwareIcn.color = Blue;
                    Question.HardwarePnl.SetActive(true);
                    Question.Hardobservationdetail.text = "";
                    obscontainerheight += 1;

                    if (Obsrecordreader2[3].ToString().Trim() == "HardwareElement" && int.Parse(Obsrecordreader2[7].ToString().Trim()) == 1)
                    {
                        Question.Hardobservationdetail.text = Obsrecordreader2[4].ToString().Trim();
                        Question.Hardwaredeficiency.isOn = true;
                        Question.Hardwareasexpected.isOn = false;
                    }

                    else if (Obsrecordreader2[3].ToString().Trim() == "HardwareElement" && int.Parse(Obsrecordreader2[7].ToString().Trim()) == 0)
                    {
                        Question.Hardobservationdetail.text = Obsrecordreader2[4].ToString().Trim();
                        Question.Hardwaredeficiency.isOn = false;
                        Question.Hardwareasexpected.isOn = true;
                    }
                }
                else
                {
                    Question.HardwareBtn.enabled = false;
                    Question.HardwareIcn.color = Grey;
                    Question.HardwarePnl.SetActive(false);
                }

                Question.ObservationContainer.GetComponent<LayoutElement>().preferredHeight = obscontainerheight * 300f;


                if ((!String.IsNullOrEmpty(prop["Objective_Sire"]) || prop["Objective_Sire"] != "")
                    ||
                    (!String.IsNullOrEmpty(prop["IMOISMCode"]) || prop["IMOISMCode"] != ""))
                {
                    Question.ObjectiveBtn.enabled = true;
                    Question.ObjectiveIcn.color = Blue;
                }
                else
                {
                    Question.ObjectiveBtn.enabled = false;
                    Question.ObjectiveIcn.color = Grey;
                }

                if (!String.IsNullOrEmpty(prop["TMSAKPI"]) || prop["TMSAKPI"] != "")
                {
                    Question.TMSABTN.enabled = true;
                    Question.TMSAIcn.color = Blue;
                }
                else
                {
                    Question.TMSABTN.enabled = false;
                    Question.TMSAIcn.color = Grey;
                }

                if (!String.IsNullOrEmpty(prop["TaggedRank"]) || prop["TaggedRank"] != "")
                {
                    Question.TaggedRanks.text = prop["TaggedRank"];

                }
                else
                {
                    Question.TaggedRanks.text = "--";
                }

                if (prop["Photomandatory"].Trim().ToLower() == "mandatory")
                {
                    Question.isphotomandatory = true;
                    Question.Save.GetComponent<SaveobsBtn>().Photoismandatory = true;
                }

                else
                {
                    Question.isphotomandatory = false;
                    Question.Save.GetComponent<SaveobsBtn>().Photoismandatory = false;
                }

                if (!String.IsNullOrEmpty(prop["C1R1R2"]) || prop["C1R1R2"] != "")
                {

                    if (prop["C1R1R2"].ToString().Trim() == "C")
                    {
                        Question.Corequestion.SetActive(true);
                    }

                    if (prop["C1R1R2"].ToString().Trim() == "R1")
                    {
                        Question.Rotation1.SetActive(true);
                    }

                    if (prop["C1R1R2"].ToString().Trim() == "R2")
                    {
                        Question.Rotation2.SetActive(true);
                    }

                    else

                    {
                        Question.Defaulticon.SetActive(true);
                    }

                }
                else
                {
                    Question.Defaulticon.SetActive(true);
                }
                mlocationDb2.close();
            }
            else
            {

                if ((!String.IsNullOrEmpty(prop["Objective_Sire"]) || prop["Objective_Sire"] != "")
                    ||
                    (!String.IsNullOrEmpty(prop["IMOISMCode"]) || prop["IMOISMCode"] != ""))
                {
                    Question.ObjectiveBtnNonSire.gameObject.SetActive(true);

                    Question.ObjectiveBtnNonSire.enabled = true;
                    Question.ObjectiveIcnNonSire.color = Blue;
                }
                else
                {
                    Question.ObjectiveBtnNonSire.gameObject.SetActive(false);
                    Question.ObjectiveBtnNonSire.enabled = false;
                    Question.ObjectiveIcnNonSire.color = Grey;
                }


                Debug.Log("This is not a Sire20 inspection.");
            }

            var i1 = 0;
            var j1 = 0;

            if (Answermodelgroup.Trim() == "2")
            {
                i1 = 0;
                j1 = 5;
            }
            else if (Answermodelgroup.Trim() == "1")
            {
                i1 = 5;
                j1 = 9;
            }
            else if (Answermodelgroup.Trim() == "3")
            {
                i1 = 9;
                j1 = 14;
            }

            else
            {
                i1 = 0;
                j1 = 5;
            }
            for (var i = i1; i < j1; i++)
            {
                Question.answergroupmodels[i].SetActive(true);
            }

            if (!String.IsNullOrEmpty(prop["Selected_Answer"]) || prop["Selected_Answer"] != "") //24th Aug check this method for its property and sort out.
            {
                string selectedanswer = prop["Selected_Answer"];
                int indexforselectedanswer = int.Parse(selectedanswer.Split('-')[0]);// KEEP the index number only, remove text after the "-".
                //AnsweredQuesforROVIQ++;

                if (indexforselectedanswer != 999)
                {
                    Question.Save.GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
                }

                //Question.Save.GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
                //questionaccordion.transform.Find("Questions_(List_Item)/Observation_Content_Holder/Observation_Container/SaveBtnBck/SaveBtn").GetComponent<SaveobsBtn>().m_Toggles[indexforselectedanswer].isOn = true;
            }

            //Completionstatus.GetComponent<TextMeshProUGUI>().text = "Answered; " + AnsweredQuesforROVIQ.ToString() + "/" + finalids.Count.ToString();
            //24th FEB commented above line and 'AnsweredQuesforROVIQ' as this was calculating total answered basis number of answered questions instead of actual completed status.
        }
        //finalids contains all questionids
        //To get other properties of question
        //string Template_Section_Ques = questions["839"]["Template_Section_Ques"];//questionids[0]
        //string Obs_Details_1 = questions["839"]["Obs_Details_1"];
        //mlocationdb1.close();

    }


    public void ForStandardPhotos()
    {
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryID.GetComponent<TextMeshProUGUI>().text));
        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        int photosectionID = 0;
        int ObsDBid = 0;//For standard photos the OBS ID has been kept as 0.
        int mediacount = 0;
        string query = "";
        table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
        using var connection2 = mlocationdb2.getConnection();

        if (standardphotoheaders.Count > 1 && standardphotoheaders != null) // For standard as well other photos if there are no photos required under standard.
        {
            foreach (string heading in standardphotoheaders)
            {
                photosectionID += 1;
                standardphotoheaderaccordion = Instantiate(standardphotoheaderprefab);
                standardphotoprefab standardphotoheaders = standardphotoheaderaccordion.GetComponent<standardphotoprefab>();
                standardphotoheaderaccordion.transform.SetParent(ParentPanel_1, false);

                standardphotoheaders.InspectionPrimaryID.text = inspPrimaryID.ToString();
                standardphotoheaders.Questions_Text.text = SuffixForPhotos + photosectionID +"_"+ heading.ToString();
                standardphotoheaders.AttachmentFolderpath.text = attachmentsfolderpath.ToString();

                query = " Inspection_Attachments where cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(inspPrimaryID.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + ObsDBid + "'" + " and Attachment_Name like '%_" + SuffixForPhotos + photosectionID+"_%';";
                mediacount = mlocationdb2.CountbasisQuery(query);

                if (mediacount >0)
                {
                    standardphotoheaders.completionstatusindicator.GetComponent<Image>().color = Color.green;
                    standardphotoheaders.mediacount.text = mediacount.ToString();

                }
            }
        }
        else

        {
            for (int i = 0; i < ParentPanel_1.transform.childCount; ++i)
            {
                Destroy(ParentPanel_1.transform.GetChild(i).gameObject);
            }

            standardphotoheaderaccordion = Instantiate(standardphotoheaderprefab);
            standardphotoprefab standardphotoheaders = standardphotoheaderaccordion.GetComponent<standardphotoprefab>();
            standardphotoheaderaccordion.transform.SetParent(ParentPanel_1, false);
            standardphotoheaders.Questions_Text.text = SuffixForPhotos + "1_" + "Upload Additional Photographs"; // Basically the photographs from here are save with suffix O1 (Other1) since there is only one child element.
            standardphotoheaders.InspectionPrimaryID.text = inspPrimaryID.ToString();
            standardphotoheaders.AttachmentFolderpath.text = attachmentsfolderpath.ToString();
            query = " Inspection_Attachments where cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(inspPrimaryID.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + ObsDBid + "'" + " and Attachment_Name like '%_" + SuffixForPhotos + "1" + "_%';";
            mediacount = mlocationdb2.CountbasisQuery(query);

            if (mediacount > 0)
            {
                standardphotoheaders.completionstatusindicator.GetComponent<Image>().color = Color.green;
                standardphotoheaders.mediacount.text = mediacount.ToString();

            }
        }
        //mlocationdb1.close();
        //mlocationdb2.close();
    }

   
}


