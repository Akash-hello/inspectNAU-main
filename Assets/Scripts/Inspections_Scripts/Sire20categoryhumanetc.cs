using System.Collections;
using System.Collections.Generic;
using DataBank;
using TMPro;
using UnityEngine;

public class Sire20categoryhumanetc : MonoBehaviour
{
    public TextMeshProUGUI QuestionsDbID;
    public TextMeshProUGUI InspectionPrimaryID;
    public GameObject Human_Process_Hardware_Scroll;

    public TextMeshProUGUI Heading;
    public TextMeshProUGUI Content;

    // Start is called before the first frame update

    public void Humandataview()

    {
        Heading.text = "";
        Content.text = "";

            Human_Process_Hardware_Scroll.SetActive(true);
        table_Inspection_Observations mlocationdb1 = new table_Inspection_Observations();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.GetDataObsAndInspID(int.Parse(InspectionPrimaryID.GetComponent<TextMeshProUGUI>().text), int.Parse(QuestionsDbID.GetComponent<TextMeshProUGUI>().text));

            Heading.text = "Human Element";

            Content.text = mlocationdb1.HumanCategory.ToString();
     }

    public void Processtaview()

    {
        Heading.text = "";
        Content.text = "";

        Human_Process_Hardware_Scroll.SetActive(true);
        table_Inspection_Observations mlocationdb1 = new table_Inspection_Observations();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.GetDataObsAndInspID(int.Parse(InspectionPrimaryID.GetComponent<TextMeshProUGUI>().text), int.Parse(QuestionsDbID.GetComponent<TextMeshProUGUI>().text));

        Heading.text = "Process Element";

        Content.text = mlocationdb1.ProcessCategory.ToString();
    }

    public void Hardware()

    {
        Heading.text = "";
        Content.text = "";

        Human_Process_Hardware_Scroll.SetActive(true);
        table_Inspection_Observations mlocationdb1 = new table_Inspection_Observations();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.GetDataObsAndInspID(int.Parse(InspectionPrimaryID.GetComponent<TextMeshProUGUI>().text), int.Parse(QuestionsDbID.GetComponent<TextMeshProUGUI>().text));

        Heading.text = "Hardware Element";

        Content.text = mlocationdb1.HardwareCategory.ToString();
    }

    public void TMSACategory()

    {
        Heading.text = "";
        Content.text = "";

        Human_Process_Hardware_Scroll.SetActive(true);
        table_Inspection_Observations mlocationdb1 = new table_Inspection_Observations();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.GetDataObsAndInspID(int.Parse(InspectionPrimaryID.GetComponent<TextMeshProUGUI>().text), int.Parse(QuestionsDbID.GetComponent<TextMeshProUGUI>().text));

        Heading.text = "TMSA";

        Content.text = mlocationdb1.TMSACategory.ToString();
    }

    public void Objective()

    {
        Heading.text = "";
        Content.text = "";

        Human_Process_Hardware_Scroll.SetActive(true);
        table_Inspection_Observations mlocationdb1 = new table_Inspection_Observations();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.GetDataObsAndInspID(int.Parse(InspectionPrimaryID.GetComponent<TextMeshProUGUI>().text), int.Parse(QuestionsDbID.GetComponent<TextMeshProUGUI>().text));

        Heading.text = "Objective and ISM Reference/Guidance";

        Content.text = mlocationdb1.ObjectiveCategory.ToString() + "\n" + "\n" + "ISM and other comments:"+"\n"+mlocationdb1.ISMandComments.ToString();
    }

   
}
