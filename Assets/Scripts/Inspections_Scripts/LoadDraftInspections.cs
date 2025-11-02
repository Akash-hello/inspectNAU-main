using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DataBank;
using System.Linq;
using System;

public class LoadDraftInspections : MonoBehaviour
{
    CanvasPositionsMgr canvaspositionsscripts;
    InspectionsShipInfo InspPrimaryInfoScrn;
    public GameObject PrimaryDetailsID;
    public GameObject InspectionType;

    public GameObject ShipImage;
    public GameObject ShipImageDisabledTxt;

    public void LoadInsp()
    {
        ShipImage = GameObject.FindGameObjectWithTag("shipphotouploadbtn");
        ShipImageDisabledTxt = GameObject.FindGameObjectWithTag("shipphotodisabletext");

        ShipImage.GetComponent<Button>().interactable = true;
        ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;

        canvaspositionsscripts = GameObject.FindGameObjectWithTag("canvaspositions").GetComponent<CanvasPositionsMgr>();
        canvaspositionsscripts.ManualBtnClick();

        InspPrimaryInfoScrn = GameObject.FindGameObjectWithTag("inspectioninfo").GetComponent<InspectionsShipInfo>();
        InspPrimaryInfoScrn.SavedInspPrimaryTableID.text = PrimaryDetailsID.GetComponent<TextMeshProUGUI>().text.ToString();
        InspPrimaryInfoScrn.InspType.text = InspectionType.GetComponent<TextMeshProUGUI>().text.ToString();
        InspPrimaryInfoScrn.InspectType.text = InspectionType.GetComponent<TextMeshProUGUI>().text.ToString();
        InspPrimaryInfoScrn.InspectionTypeforSire20 = InspectionType.GetComponent<TextMeshProUGUI>().text.ToString();
        InspPrimaryInfoScrn.Inspectypeforchecklistarea.text = InspectionType.GetComponent<TextMeshProUGUI>().text.ToString();

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryInfoScrn.SavedInspPrimaryTableID.text));

        if (mlocationdb1.status.ToString().Trim() == "P")
        {
            InspPrimaryInfoScrn.Publishedinspectionsbuttons.SetActive(true);
            InspPrimaryInfoScrn.NewOrDraftinspectionsbuttons.SetActive(false);
            InspPrimaryInfoScrn.DraftMode.SetActive(false);
            InspPrimaryInfoScrn.Published.SetActive(true);
        }
        else
        {
            InspPrimaryInfoScrn.Publishedinspectionsbuttons.SetActive(false);
            InspPrimaryInfoScrn.NewOrDraftinspectionsbuttons.SetActive(true);
            InspPrimaryInfoScrn.DraftMode.SetActive(true);
            InspPrimaryInfoScrn.Published.SetActive(false);

            if (mlocationdb1.status.ToString().Trim() == "O")
            {
                InspPrimaryInfoScrn.Reopened.gameObject.GetComponent<TMP_Text>().text = " Re-Working ID: " + InspPrimaryInfoScrn.SavedInspPrimaryTableID.text;
                InspPrimaryInfoScrn.Reopened.SetActive(true);
                InspPrimaryInfoScrn.reopenedinspection = true;
            }

            else
            {
                InspPrimaryInfoScrn.Reopened.SetActive(false);
            }
        }
        connection.Close();
        InspPrimaryInfoScrn.LoadPrimaryInfo();
    }

}
