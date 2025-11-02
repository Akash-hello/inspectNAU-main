using DataBank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Data;

public class CanvasPositionsMgr : MonoBehaviour
{
    //Use this to change the hierarchy of the GameObject siblings
    public GameObject Main_Menu_Canvas_1;
    public GameObject Profile_Panel_Canvas_2;
    public GameObject Job_Templates_Canvas_3;
    public GameObject Scanner_Canvas_4;
    public GameObject Inspection_Information_Canvas_5;
    public GameObject Inspections_Block_Canvas_6;

    public GameObject Preview_Canvas_8;
    public GameObject APIsetup_Canvas_9;
    public GameObject Synch_Canvas_10;
    public GameObject SirePastInspections;
    public GameObject LivecamCanvas;


    public GameObject Panel_1;
    public GameObject Panel_2;
    //public GameObject Panel_3;

    public Button[] menubuttons;
    int index = 0;

    //public GameObject Homebtn;
    //public GameObject Checklist;
    //public GameObject Reports;
    //public GameObject Profile;
    //public GameObject Synch;
    //public GameObject Settings;

    public string BtnName;
    public GameObject Signaturepanel;
    public GameObject SignatureBtn;
    public GameObject PublishBtn;

    public GameObject PublishedInsHeaderCount;
    public Button reportbutton;
    public GameObject InitialPanel;
    public TextMeshProUGUI publishedinspCount;

    fetchinspectiontemplates loadinspectiontemplatesFmDB;
    string Clickedbtname;
    bool directbtnOrfmCheckMthd;
    string passbtnname;

    public Button TestIncomingAPIBtn;

    private void Start()
    {
        loadinspectiontemplatesFmDB = new fetchinspectiontemplates();
        BtnName = "";
        directbtnOrfmCheckMthd = false;
        passbtnname = "";
        CheckForPublishedInspections();
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, bool isActive)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }
    }

    public void CheckForPublishedInspections()
    {
        IDataReader reader;
        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();

        publishedinspCount.text = "";

        reader = mlocationDb1.getdataforpublished("P");

        string query3 = "Inspection_PrimaryDetails" + " WHERE " + "Status = 'P' ";
        
       if (int.Parse(mlocationDb1.totalRecords(query3).ToString()) > 0)
        {
            Debug.Log("Load the History section with new header !!");
            publishedinspCount.text = mlocationDb1.totalRecords(query3).ToString();
            
            InitialPanel.SetActive(false);

            directbtnOrfmCheckMthd = true;
            passbtnname = "Home";

            reportbutton.GetComponent<Button>().onClick.Invoke();
            PublishedInsHeaderCount.SetActive(true);
        }
        
        else

            {
                Debug.Log("No published inspections found as yet..");
            PublishedInsHeaderCount.SetActive(false);
            InitialPanel.SetActive(true);
            }

        reader.Dispose();
        connection.Close();
        //mlocationDb1.close();
    }

    

    public void MainMenuBtnClick()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), true);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(8);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(6);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(5);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(4);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }
    public void MyProfileBtnClicks()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), true);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(8);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(6);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(5);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(4);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }

    public void JobsBtnClicks()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(8);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), true);

        Scanner_Canvas_4.transform.SetSiblingIndex(6);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(5);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(4);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }

    public void StartBtnClick()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), true);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }

    public void ManualBtnClick()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), true);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);

        Signaturepanel.gameObject.SetActive(false);
        SignatureBtn.gameObject.SetActive(true);
        PublishBtn.gameObject.SetActive(false);

        InspectionInfoBtn_1();

        Debug.Log("MANUAL BTN CLICKED");
    }

    public void LoadGeneralInfo()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), true);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);

        InspectionInfoBtn_1();
    }

    public void HaveQRcodeBtnClick()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);
        
        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), true);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }

    public void ProceedtoChecklistBtn()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);

        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), true);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }

    public void GotoAPIConfig()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);

        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(12);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), true);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);

        TestIncomingAPIBtn.GetComponent<Button>().onClick.Invoke();
    }

    public void GotoSynch()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);

        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(12);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), true);

        SirePastInspections.transform.SetSiblingIndex(1);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), false);
    }

    public void GotoFetchHistoricSires()
    {
        Main_Menu_Canvas_1.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Main_Menu_Canvas_1.GetComponentInChildren<CanvasGroup>(), false);

        Profile_Panel_Canvas_2.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Profile_Panel_Canvas_2.GetComponentInChildren<CanvasGroup>(), false);

        Job_Templates_Canvas_3.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Job_Templates_Canvas_3.GetComponentInChildren<CanvasGroup>(), false);

        Scanner_Canvas_4.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Scanner_Canvas_4.GetComponentInChildren<CanvasGroup>(), false);

        Inspection_Information_Canvas_5.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspection_Information_Canvas_5.GetComponentInChildren<CanvasGroup>(), false);

        Inspections_Block_Canvas_6.transform.SetSiblingIndex(11);
        SetCanvasGroupState(Inspections_Block_Canvas_6.GetComponentInChildren<CanvasGroup>(), false);

        Preview_Canvas_8.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Preview_Canvas_8.GetComponentInChildren<CanvasGroup>(), false);

        APIsetup_Canvas_9.transform.SetSiblingIndex(1);
        SetCanvasGroupState(APIsetup_Canvas_9.GetComponentInChildren<CanvasGroup>(), false);

        Synch_Canvas_10.transform.SetSiblingIndex(1);
        SetCanvasGroupState(Synch_Canvas_10.GetComponentInChildren<CanvasGroup>(), false);

        SirePastInspections.transform.SetSiblingIndex(12);
        SetCanvasGroupState(SirePastInspections.GetComponentInChildren<CanvasGroup>(), true);
    }

    public void InspectionInfoBtn_1()
    {
        Panel_1.transform.SetSiblingIndex(5);
        Panel_1.GetComponentInChildren<CanvasGroup>().alpha = 1;

        Panel_2.transform.SetSiblingIndex(4);
        Panel_2.GetComponentInChildren<CanvasGroup>().alpha = 0;

        //Panel_3.transform.SetSiblingIndex(3);
        //Panel_3.GetComponentInChildren<CanvasGroup>().alpha = 0;
    }

    public void InspectionInfoBtn_2()
    {
        Panel_1.transform.SetSiblingIndex(4);
        Panel_1.GetComponentInChildren<CanvasGroup>().alpha = 0;

        Panel_2.transform.SetSiblingIndex(5);
        Panel_2.GetComponentInChildren<CanvasGroup>().alpha = 1;

        //Panel_3.transform.SetSiblingIndex(3);
        //Panel_3.GetComponentInChildren<CanvasGroup>().alpha = 0;
    }

    public void InspectionInfoBtn_3()
    {
        Panel_1.transform.SetSiblingIndex(3);
        Panel_1.GetComponentInChildren<CanvasGroup>().alpha = 0;

        Panel_2.transform.SetSiblingIndex(4);
        Panel_2.GetComponentInChildren<CanvasGroup>().alpha = 0;

        //Panel_3.transform.SetSiblingIndex(5);
        //Panel_3.GetComponentInChildren<CanvasGroup>().alpha = 1;
    }

    public void BottomMenuBtnColors()
    {
        // Remove color changing logic - let ButtonGroupController handle button appearance
        // This method can still be used for other logic if needed, but won't interfere with button colors
        
        if (directbtnOrfmCheckMthd == true)
        {
            Clickedbtname = passbtnname;
        }
        else
        {
            Clickedbtname = EventSystem.current.currentSelectedGameObject.name;
        }
        
        // Optional: Add debug logging to see when this method is called
        Debug.Log($"BottomMenuBtnColors called for button: {Clickedbtname}");

        directbtnOrfmCheckMthd = false;
    }

    public void ActivateLiveCam()
    {
        LivecamCanvas.transform.SetSiblingIndex(50);
        SetCanvasGroupState(LivecamCanvas.GetComponentInChildren<CanvasGroup>(), true);
    }

    public void DeActivateLiveCam()
    {
        LivecamCanvas.transform.SetSiblingIndex(1);
        SetCanvasGroupState(LivecamCanvas.GetComponentInChildren<CanvasGroup>(), false);
    }
}