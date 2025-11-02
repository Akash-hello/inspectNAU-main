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


public class loadquestions : MonoBehaviour //THIS SCRIPT WAS FOR LOADING BLANK CHAPTERS AND OBSERVATIONS FROM THE INSP TEMPLATE TABLE, ONCE THE INSPECTION IS INITIATED THE CHAP AND OBSERVATIONS ARE LOADED FROM THE OBSERVATION TABLE.
                                           //THIS SCRIPT WAS INITIATED THRU OLD BUTTON KNOWN AS "PROCEED TO CHECKLIST" and FURTHER FROM THE RIGHT DOWN ARROW OF THE CHAPTERS.
{
    public string versiondetails;
    public GameObject CanvasPositionManager;
    
    public string Questionnumber;
    public string Questions;

    public GameObject questionaccordion;

    public GameObject questionsitemprefab;
    public RectTransform ParentPanel_1;

    //public GameObject errorpopup;
    //public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    int id;
    public int Inspection_Template_ID = 0;
    public GameObject CloudDBiD;
    public int Cloud_DB_ID = 0;
    public int Cloud_DB_ParentID = 0;
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

    public TextMeshProUGUI selectedinspectionID;

    List<FromDatabase_Insptemplates> fromdbList = new List<FromDatabase_Insptemplates>();
    public void LoadquestionsBtnClicked()

    {
        StartCoroutine(Loadquestions());
    }

    public IEnumerator Loadquestions()
    
    {
        yield return null;
        Cloud_DB_ID = int.Parse(CloudDBiD.GetComponent<TextMeshProUGUI>().text);
        Questionnumber = "";
        Questions = "";

        table_Inspection_template mlocationDb = new table_Inspection_template();
        using var connection = mlocationDb.getConnection();
        for (int i = 0; i < ParentPanel_1.transform.childCount; ++i)
        {
            Destroy(ParentPanel_1.transform.GetChild(i).gameObject);
        }
        using System.Data.IDataReader reader = mlocationDb.getquestionsData(Cloud_DB_ID); //CLOUD DB ID IS the ID passed from above method, basically the CHAPTER ID

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

            //Debug.Log("Stock Code: " + entity._stocksym);
            myList.Add(entity);

            var output1 = JsonUtility.ToJson(entity, true);
            Debug.Log(output1);
            outputofsearchresult = output1.ToString();
        }
        reader.Dispose();
        searchresults = JsonUtility.ToJson(myList, true);
        Debug.Log("Questions searched output.");
        datacount = myList.Count;
        foreach (var x in myList)
        {
            FromDatabase_Insptemplates DBinspectionDetails = new FromDatabase_Insptemplates();

            DBinspectionDetails.ID = x._ID;
            DBinspectionDetails.Template_ID = x._Inspection_Template_ID;
            DBinspectionDetails.Cloud_DB_ID = x._Cloud_DB_ID;

            DBinspectionDetails.Template_Section_Ques = x._Template_Section_Ques;
            DBinspectionDetails.Details_1 = x._Details_1;
            fromdbList.Add(DBinspectionDetails);

            Questionnumber = x._Template_Section_Ques.ToString();
            Questions = x._Details_1.ToString();

            completionstatus = "CREATE COMPLETION STATUS FOR HERE";
            //versiondetails = "Version No.; " + x._Version_Number.ToString() + " Date; " + x._Version_Date.ToString();
            Cloud_DB_ID = x._Cloud_DB_ID;
            //Cloud_DB_ParentID = x._Cloud_DB_ParentID;

            questionaccordion = Instantiate(questionsitemprefab);
            questionsprefab Question = questionaccordion.GetComponent<questionsprefab>();
            questionaccordion.transform.SetParent(ParentPanel_1, false);

            Question.Questions_Text.text = Questionnumber.ToString() + ": " + Questions.ToString();
            //questionaccordion.transform.Find("Questions_(List_Item)/Questions_Content_Holder/Questions_Text_Area/Questions_Text").GetComponentInChildren<TextMeshProUGUI>().text = Questionnumber.ToString() + ": " + Questions.ToString();

            //inspectionaccordion.transform.Find("Cloud_DB_ParentID").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ParentID.ToString();
        }

        //mlocationDb.close();
    }

    //IEnumerator HidePopUp()
    //{
    //    yield return new WaitForSeconds(time);

    //    errorpopup.gameObject.SetActive(false);
    //    PopUpMsg.text = "";
    //}

}


