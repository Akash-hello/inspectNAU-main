using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DataBank;
using System;
using System.IO;
using System.Globalization;

public class DeleteInspection : MonoBehaviour
{
    public GameObject DeleteBtn;
    string inspectiondate;
    public TextMeshProUGUI SavedInspPrimaryTableID;
    int inspectionID;

    public GameObject errorpopup;

    public TextMeshProUGUI PopUpMsg;
    public GameObject YestoDelete;
    public Button loaddraftinsp;

    // Start is called before the first frame update
    void Start()
    {
        DeleteBtn.SetActive(false);
        loaddraftinsp = GameObject.FindWithTag("loaddraftbtn").GetComponent<Button>();
        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();

        if (!String.IsNullOrEmpty(SavedInspPrimaryTableID.text.ToString()))
        {
            DateTime currentDate = DateTime.Now;
            DateTime date365DaysAgo = DateTime.Now.AddDays(-365);
            DateTime date7DaysAgo = DateTime.Now.AddDays(-7);
            //string format = "ddd dd-MM-yyyy HH:mm";

            DateTime Inspectiondate = DateTime.MinValue;

            mlocationDb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

            string[] formats = { "ddd dd-MM-yyyy HH:mm", "ddd dd/MM/yyyy HH:mm", "dd-MM-yyyy HH:mm", "dd/MM/yyyy HH:mm",
             "ddd-dd-MM-yyyy HH:mm",
                "MM/dd/yyyy HH:mm",
                "yyyy-MM-dd HH:mm",
                "ddd, dd MMM yyyy HH:mm" };

            DateTime parsedDate;
            bool success = false;

            if (!String.IsNullOrEmpty(mlocationDb1.Inspection_Date.ToString().Trim()))
            {
                success = DateTime.TryParseExact(mlocationDb1.Inspection_Date.ToString().Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

                if (success)
                {
                    Inspectiondate = parsedDate;
                }
            }

            // Parse the string into a DateTime object

            //if (!String.IsNullOrEmpty(mlocationDb1.Inspection_Date.ToString().Trim()))
            //{
            //    Inspectiondate = DateTime.ParseExact(mlocationDb1.Inspection_Date.ToString().Trim(), format, null);
            //}

            if (mlocationDb1.status == "D" && Inspectiondate < date7DaysAgo)
            {
                DeleteBtn.SetActive(true);
            }
            else if (mlocationDb1.status == "P" && Inspectiondate < date365DaysAgo)
            {
                DeleteBtn.SetActive(true);
            }

            else if (mlocationDb1.status == "N" && Inspectiondate < currentDate)
            {
                DeleteBtn.SetActive(true);
            }

            else
            {
                DeleteBtn.SetActive(false);
            }
        }
       
        else

        {
            Debug.Log("This would be a inspection template !!");
        }
        connection.Close();
        //mlocationDb1.close();
    }

    public void DeleteBtnClick()

    {
        errorpopup.gameObject.SetActive(true);
        PopUpMsg.text = "Are you sure you want to delete everything for this report including data, files uploaded, photographs, etc. Continue ?";
        YestoDelete.SetActive(true);
    }

    public void DeleteYes()
    {
        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationDb1.getConnection();
        string foldername = "";

        if (!String.IsNullOrEmpty(SavedInspPrimaryTableID.text.ToString()))
        {
            mlocationDb1.ReportDeletedStatus(int.Parse(SavedInspPrimaryTableID.text.ToString())); //Change inspection report status to KEY_Status + " = 'Z' for this report and delete all associated records and folders...
            mlocationDb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

            if (!String.IsNullOrEmpty(mlocationDb1.folderpath.ToString().Trim()))
            {
               foldername = Application.persistentDataPath + mlocationDb1.folderpath.ToString();
                if (Directory.Exists(foldername))
                {
                    DeleteDirectoryContents(foldername);
                    Directory.Delete(foldername);
                    Debug.Log("foldername for the report to be deleted is; " + foldername);
                }

                else
                {
                    Debug.Log("There's no directory with this name");
                }
            }

            loaddraftinsp.GetComponent<Button>().onClick.Invoke();

        }
        connection.Close();
        //mlocationDb1.close();
    }

    void DeleteDirectoryContents(string path)
    {
        // Delete all files in the directory
        foreach (var file in Directory.GetFiles(path))
        {
            File.Delete(file);
        }

        // Recursively delete all subdirectories
        foreach (var subDirectory in Directory.GetDirectories(path))
        {
            DeleteDirectoryContents(subDirectory); // Recursive call
            Directory.Delete(subDirectory);
        }
    }
}
