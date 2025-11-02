using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DataBank;
using UnityEngine.UI;

public class GetDbID : MonoBehaviour
{
    public TextMeshProUGUI clouddbid;
    public TextMeshProUGUI inpsectionname;
    public TextMeshProUGUI CloudDBid;
    public TextMeshProUGUI AnswerGroup_Details_1;
    public TextMeshProUGUI ROVIQ_list;

    public GameObject selectedinspectionTxt;
    public GameObject selectedinspectionNameTxt;
    public GameObject InpspectionBlock;
    public GameObject AnswerGroupModel;
    public GameObject ROVIQList;

    public RectTransform ParentPanel;
    public string ChapterSectionName;
    public string CompletionStatus;
    public string Cloud_DB_ParentID;
    public string Cloud_DB_ID;
    CanvasPositionsMgr canvaspositionsscripts;
    InspectionsShipInfo InspPrimaryInfoScrn;
    public GameObject  ShipImage;
    public GameObject ShipImageDisabledTxt;

    //private void Start()
    //{

    //    //InpspectionBlock.SetActive(false);
    //}

    public void AssignCLoudDbiD()
    {
        ShipImage = GameObject.FindGameObjectWithTag("shipphotouploadbtn");
        ShipImageDisabledTxt = GameObject.FindGameObjectWithTag("shipphotodisabletext");

        ShipImage.GetComponent<Button>().interactable = false;
        ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = true;

        CloudDBid.text = clouddbid.text.ToString();
        selectedinspectionTxt = GameObject.FindGameObjectWithTag("selectedinspid");
        selectedinspectionTxt.GetComponent<TextMeshProUGUI>().text = clouddbid.text.ToString();

        selectedinspectionNameTxt = GameObject.FindGameObjectWithTag("selectedinspname");
        selectedinspectionNameTxt.GetComponent<TextMeshProUGUI>().text = inpsectionname.text.ToString();

        Debug.Log("Cloud DB ID is; " + CloudDBid.text.ToString());
        InpspectionBlock = GameObject.FindGameObjectWithTag("inspectionsblock");
        canvaspositionsscripts = GameObject.FindGameObjectWithTag("canvaspositions").GetComponent<CanvasPositionsMgr>();

        AnswerGroupModel = GameObject.FindGameObjectWithTag("AnswerGroup");
        AnswerGroupModel.GetComponent<TextMeshProUGUI>().text = AnswerGroup_Details_1.text.ToString();

        ROVIQList = GameObject.FindGameObjectWithTag("ROVIQ");
        ROVIQList.GetComponent<TextMeshProUGUI>().text = ROVIQ_list.text.ToString();

        ClearPrimaryInfoFields();
        //canvaspositionsscripts.StartBtnClick(); FOR OPENING SCAN CANVAS CHANGED TO DIRECTLY LOAD INFORMATION SECTIONS; 30th May 2024
        canvaspositionsscripts.LoadGeneralInfo();
    }

    public void ClearPrimaryInfoFields()
    {
        InspPrimaryInfoScrn = GameObject.FindGameObjectWithTag("inspectioninfo").GetComponent<InspectionsShipInfo>();
        InspPrimaryInfoScrn.ClearInputNewInsp();
    }

    //public void Loadchapters()
    //{
    //    ChapterSectionName = "";
    //    CompletionStatus = "";
    //    Cloud_DB_ID = "0";
    //    Cloud_DB_ParentID = "";

    //    //refreshbtn.enabled = false;
    //    table_Inspection_template mlocationDb = new table_Inspection_template();
    //    for (int i = 0; i < ParentPanel.transform.childCount; ++i)
    //    {
    //        Destroy(ParentPanel.transform.GetChild(i).gameObject);
    //    }
    //    System.Data.IDataReader reader = mlocationDb.getdataforreader("Version");

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
    //             reader[15].ToString().Trim(),
    //            reader[16].ToString().Trim());

    //        //Debug.Log("Stock Code: " + entity._stocksym);
    //        myList.Add(entity);

    //        var output1 = JsonUtility.ToJson(entity, true);
    //        Debug.Log(output1);
    //        outputofsearchresult = output1.ToString();
    //    }

    //    searchresults = JsonUtility.ToJson(myList, true);

    //    datacount = myList.Count;

    //    foreach (var x in myList)
    //    {
    //        FromDatabase_Insptemplates DBinspectionDetails = new FromDatabase_Insptemplates();

    //        DBinspectionDetails.ID = x._ID;
    //        DBinspectionDetails.Template_ID = x._Inspection_Template_ID;
    //        DBinspectionDetails.Cloud_DB_ID = x._Cloud_DB_ID;
    //        //DBinspectionDetails.Cloud_DB_ParentID = x._Cloud_DB_ParentID;

    //        DBinspectionDetails.Template_Section_Ques = x._Template_Section_Ques;
    //        DBinspectionDetails.Details_1 = x._Details_1;
    //        fromdbList.Add(DBinspectionDetails);

    //        Inspectionname = x._Template_Section_Ques.ToString();
    //        versiondetails = "Version No.; " + x._Version_Number.ToString() + " Date; " + x._Version_Date.ToString();
    //        Cloud_DB_ID = x._Cloud_DB_ID;
    //        //Cloud_DB_ParentID = x._Cloud_DB_ParentID;

    //        //url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + Inspectionname;
    //        inspectionaccordion = Instantiate(inspectionsitemprefab);

    //        inspectionaccordion.transform.SetParent(ParentPanel, false);

    //        inspectionaccordion.transform.Find("mainheader").GetComponentInChildren<TextMeshProUGUI>().text = Inspectionname.ToString();
    //        inspectionaccordion.transform.Find("secondheader").GetComponentInChildren<TextMeshProUGUI>().text = versiondetails.ToString();
    //        inspectionaccordion.transform.Find("Cloud_DB").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ID.ToString();
    //        //inspectionaccordion.transform.Find("Cloud_DB_ParentID").GetComponentInChildren<TextMeshProUGUI>().text = Cloud_DB_ParentID.ToString();
    //    }
    //    Template_Section_Ques = Convert.ToString(Template_Section_Ques);
    //    // GetStocksData(fromdbList);


    //}

}
