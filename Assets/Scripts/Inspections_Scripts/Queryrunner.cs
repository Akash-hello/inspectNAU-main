using DataBank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Queryrunner : MonoBehaviour
{
    
    public TMP_InputField Query;
    
    string[] myVariable;
    float time = 5.0f;
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;

    void Start()
    {
        Query.text = "";
    }

    public void RunaQuery()
    {

        try
        {
            //Example statement; "INSPECTNAU SAYS:", "Inspection_Observations", "RiskCategory = 'Medium' where Obs_Details_8 like '%Question%';"
            if (Query.text.Trim().Contains("INSPECTNAU SAYS:") && !string.IsNullOrEmpty(Query.text.Trim()))
            {
                //string query = Query.text.Trim().Split(':')[1];

                string[] myVariable = Query.text.Trim().Split(',');

                if (myVariable[1].Contains("Config"))
                {
                    table_Config mlocationDb = new table_Config();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);
                    mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());

                }
                if (myVariable[1].Contains("Inspection_Attachments"))
                {
                    table_Inspection_Attachments mlocationDb = new table_Inspection_Attachments();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);
                    mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());
                }

                if (myVariable[1].Contains("Inspection_Observations"))
                {
                    table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);
                    //mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());
                }

                if (myVariable[1].Contains("Inspection_PrimaryDetails"))
                {
                    table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);
                    mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());
                }
                if (myVariable[1].Contains("Inspection_template"))
                {
                    table_Inspection_template mlocationDb = new table_Inspection_template();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);
                    mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());
                }

                if (myVariable[1].Contains("LoginConfig"))
                {
                    table_LoginConfig mlocationDb = new table_LoginConfig();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);
                    mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());
                }

                if (myVariable[1].Contains("Synch"))
                {
                    table_Synch mlocationDb = new table_Synch();
                    using var connection = mlocationDb.getConnection();
                    string columnquery = myVariable[2].Trim('"').Replace("\"", "");
                    mlocationDb.Updatedata(columnquery);

                    mlocationDb.close();

                    Query.gameObject.SetActive(false);
                    errorpopup.gameObject.SetActive(true);
                    PopUpMsg.text = Query.text.Trim() + " - Query Executed...";
                    Query.text = "";
                    StartCoroutine(HidePopUp());
                }

            }

            else
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = Query.text.Trim() + " Is not a valid Query...";
                Debug.Log("The variable is NOT a string array.");
                Query.gameObject.SetActive(false);
                StartCoroutine(HidePopUp());
            }
        }

        catch (System.Exception ex)
        {
            // Catch any exception that occurs and log it
            Debug.LogError($"Error retrieving -->>:  {ex.Message} & {ex.StackTrace}");
        }
        
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        Query.gameObject.SetActive(true);
    }
}
