using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataBank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

using iTextSharp.text;

public class pulishedinspjobs : MonoBehaviour
{
    public TextMeshProUGUI SavedInspPrimaryTableID;
    public string folderpath;
    public string inspectionstatus;
    string inspprimarydata;
    List <string> ObservationsList;
    List<string> ObservationsListheader;
    List<string> SireElements;

    public string fileName = "";
    public string filePath;

    string Chapterandsection = "";
    string questionnumber = "";
    string question = "";
    string answer = "";
    string observation = "";
    string Dateandtime = "";// + "-"+Obsrecordreader[10].ToString().Trim();
    string risk = "";
    string rovingcategory = "";
    string title = "";
    string details = "";
    string result = "";
    bool sire;

    public void Createcsv()
    {
        fileName = "Observations.csv";
        sire = false;
        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationDb1.getConnection();
        mlocationDb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

        folderpath = mlocationDb1.folderpath.ToString();
        inspectionstatus = mlocationDb1.status.ToString();
        inspprimarydata = mlocationDb1.outputofsearchresult;

        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection = mlocationDb.getConnection();
        //string query = "SELECT TRIM(Selected_Answer, ' -ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz') AS Answer FROM Inspection_Observations where cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + " GROUP BY Selected_Answer;";
        //string query1 = "select b.Template_Section_Ques as 'ChapterName' ,a.* from Inspection_Observations a left join Inspection_Observations b on a.Cloud_DB_ParentID = b.Cloud_DB_ID where TRIM(a.Inspection_PrimaryDetails_ID)= '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' and TRIM(a.Observation_Text)!= '' and a.Observation_Text is not null and TRIM(a.Obs_Details_8) = 'Question' ";

        string query1 = "select b.Template_Section_Ques as 'ChapterName',a.* from Inspection_Observations a left join Inspection_Observations " +
                "b on a.Cloud_DB_ParentID = b.Cloud_DB_ID and TRIM(a.Inspection_PrimaryDetails_ID) = TRIM(b.Inspection_PrimaryDetails_ID) " +
                "where TRIM(a.Inspection_PrimaryDetails_ID)= '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                "and TRIM(a.Observation_Text)!= '' and a.Observation_Text is not null AND TRIM(a.Selected_Answer) NOT like '%999%' and TRIM(a.Obs_Details_8) = 'Question' and TRIM(b.Obs_Details_8) = 'Chapter';";


        using System.Data.IDataReader Obsrecordreader = mlocationDb.getDatabyQuery(query1);
        ObservationsList = new List<string>();
        //List<String> observations = new List<string>();

        if (((System.Data.Common.DbDataReader)Obsrecordreader).HasRows)
        {
            while (Obsrecordreader.Read())
            {
                if (string.IsNullOrEmpty(Obsrecordreader[8].ToString()) || Obsrecordreader[8].ToString() != "")//CHECK IF THERE IS ANY ANSWER SELECTED.
                {
                    Chapterandsection = Obsrecordreader[0].ToString().Trim() + " | " + "Section: " + Obsrecordreader[19].ToString().Trim();
                    questionnumber = Obsrecordreader[7].ToString().Trim();
                    question = Obsrecordreader[16].ToString().Trim();
                    answer = Obsrecordreader[8].ToString().Trim().Split('-')[1];
                    observation = Obsrecordreader[12].ToString().Trim();
                    Dateandtime = Obsrecordreader[9].ToString().Trim();// + "-"+Obsrecordreader[10].ToString().Trim();
                    risk = Obsrecordreader[13].ToString().Trim();
                    rovingcategory = Obsrecordreader[26].ToString().Trim();

                    //ObservationsList.Add(CreateCsvRow(Chapterandsection, questionnumber, question, answer, observation, Dateandtime, risk, rovingcategory));

                    //string query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "'" + " and TRIM(Attachment_Details_1) = 'sire20' "; //+ " and trim(Attachment_Title) = 'Media'";

                    if (Obsrecordreader[17].ToString().ToLower().Trim().Replace(" ", "").Contains("(sire2.0)"))
                    {
                        sire = true;
                        string query =
                        " SELECT (SELECT Attachment_Title FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 0) AS Attachment_Title1, " +
                        "(SELECT Attachment_Title FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 1) AS Attachment_Title2, " +
                        "(SELECT Attachment_Title FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 2) AS Attachment_Title3, " +
                        "(SELECT Attachment_Name FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 0) AS Attachment_Name1, " +
                        "(SELECT Attachment_Name FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 1) AS Attachment_Name2, " +
                        "(SELECT Attachment_Name FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 2) AS Attachment_Name3," +
                        "(SELECT Attachment_Details_2 FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 0) AS Attachment_Details_2_1," +
                        "(SELECT Attachment_Details_2 FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 1) AS Attachment_Details_2_2," +
                        "(SELECT Attachment_Details_2 FROM Inspection_Attachments WHERE cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(SavedInspPrimaryTableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                        "AND cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "' AND TRIM(Attachment_Details_1) = 'sire20' LIMIT 1 OFFSET 2) AS Attachment_Details_2_3";

                        table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
                        using var connection2 = mlocationdb2.getConnection();
                        using System.Data.IDataReader reader = mlocationdb2.SelectDataByquery1(query);

                        List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();

                        if (((System.Data.Common.DbDataReader)reader).HasRows)
                        {

                            string humanresult = "";
                            string processresult = "";
                            string hardwareresult = "";
                            
                            
                            while (reader.Read())
                            {
                                if (!String.IsNullOrEmpty(reader[6].ToString()) && int.Parse(reader[6].ToString().Trim()) == 0)
                                {
                                    humanresult = "As Expected";
                                }

                                else if (!String.IsNullOrEmpty(reader[6].ToString()) && int.Parse(reader[6].ToString().Trim()) == 1)
                                {
                                    humanresult = "Not As Expected";
                                }

                                if (!String.IsNullOrEmpty(reader[7].ToString()) && int.Parse(reader[7].ToString().Trim()) == 0)
                                {
                                    processresult = "As Expected";
                                }

                                else if (!String.IsNullOrEmpty(reader[7].ToString()) && int.Parse(reader[7].ToString().Trim()) == 1)
                                {
                                    processresult = "Not As Expected";
                                }

                                if (!String.IsNullOrEmpty(reader[8].ToString()) && int.Parse(reader[8].ToString().Trim()) == 0)
                                {
                                    hardwareresult = "As Expected";
                                }

                                else if (!String.IsNullOrEmpty(reader[8].ToString()) && int.Parse(reader[8].ToString().Trim()) == 1)
                                {
                                    hardwareresult = "Not As Expected";
                                }


                                ObservationsList.Add(CreateCsvRow(Chapterandsection, questionnumber, question, answer, observation, Dateandtime, risk, rovingcategory, reader[0].ToString(), reader[3].ToString(), humanresult, reader[1].ToString(), reader[4].ToString(), processresult, reader[2].ToString(), reader[5].ToString(), hardwareresult));
                            }
                            reader.Dispose();
                        }
                    }
                    
                    //if (myList.Count != 0)
                    //{
                        
                    //    foreach (var x in myList)
                    //    {
                    //        if (x._Attachment_Details_1.Trim().ToLower() == "sire20" && (x._Attachment_Title.Trim() == "HumanElement" || x._Attachment_Title.Trim() == "ProcessElement" || x._Attachment_Title.Trim() == "HardwareElement"))
                    //        {
                                
                    //            if (x._Attachment_Details_2.Trim() == "1")
                    //            {
                    //                title = x._Attachment_Title.Trim();
                    //                details = x._Attachment_Name;
                    //                result = "Not As Expected";
                                    
                    //            }

                    //            else if (x._Attachment_Details_2.Trim() == "0")

                    //            {
                    //                title = x._Attachment_Title.Trim();
                    //                details = x._Attachment_Name;
                    //                result = "As Expected";
                    //            }
                    //        }
                    //    }
                        
                    //}
                    else
                    {
                        sire = false;
                        ObservationsList.Add(CreateCsvRow(Chapterandsection, questionnumber, question, answer, observation, Dateandtime, risk, rovingcategory));
                    }

                }
                
            }
            Obsrecordreader.Close();
            Obsrecordreader.Dispose();

        }
        
        else
        {
            Debug.LogWarning("No data to export.");
            ObservationsList.Add($"No Observations recorded !");
            //return;
        }

        //Debug.Log(ObservationsList.ToString());
        ExportListToCsv(ObservationsList, fileName);

    }

    private static string CreateCsvRow(params string[] fields)
    {
        // Escape each field as necessary and join with commas
        return string.Join(",", Array.ConvertAll(fields, EscapeCsvField));
    }


    private static string EscapeCsvField(string field)
    {
        // Trim any extra whitespace around the field
        field = field.Trim();

        // If the field contains commas or quotes, enclose it in double quotes
        if (field.Contains(",") || field.Contains("\""))
        {
            // Escape any double quotes by doubling them, and wrap the field in quotes
            field = "\"" + field.Replace("\"", "\"\"") + "\"";
        }

        return field;
    }

    void ExportListToCsv(List<string> observationcontent, string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath + folderpath.ToString()+"/"+ fileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the header row
            ObservationsListheader = new List<string>();

            if (sire == true)
            {
                ObservationsListheader.Add($"Chapter & Section, Question No., Question, Answer/Condition, Observation, Date & Time, Risk Rating, ROVING Seq., Element, Observation, Status, Element, Observation, Status, Element, Observation, Status");

            }

            else if (sire == false)
            {
                ObservationsListheader.Add($"Chapter & Section, Question No., Question, Answer/Condition, Observation, Date & Time, Risk Rating, ROVING Seq.");

            }

            foreach (string item in ObservationsListheader)
            {
               
                writer.WriteLine(item);
            }

            // Write the data rows
            foreach (string item in observationcontent)
            {
                
                writer.WriteLine(item);
            }
        }

        Debug.Log("Data exported to " + filePath);
        //OpenDocument(filePath);
        OpenDocumentNew(filePath);
        //DownloadCsv();
    }

    private void OpenDocument(string path)
    {
        if (File.Exists(path))
        {
            System.Diagnostics.Process.Start(path);
        }
        else
        {
            return;
        }
    }


    public void OpenDocumentNew(string reportpath)
    {

#if UNITY_EDITOR

        if (File.Exists(reportpath))
        {
            System.Diagnostics.Process.Start(reportpath);
        }
        else
        {
            return;
        }
//#elif UNITY_IPHONE

////File.Copy(OriginPath, destination, true);

//  //string reportpath1 = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf";
//  string reportpath1 = folderpath.ToString()+"/"+ fileName;
  
//  iOSOpenURL opener = new iOSOpenURL();
//  opener.OpenPDF(reportpath1);
        
#elif UNITY_ANDROID ||UNITY_IPHONE
         if (File.Exists(reportpath))
        {
            // Open the PDF file using the default PDF viewer
           
        //AndroidContentOpenerWrapper.OpenContent(Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf");
     // Use NativeShare to share the file
            new NativeShare()
                .AddFile(reportpath) // Attach the PDF file
                .SetSubject("Sharing Observations CSV") // Optional: Set the subject
                .SetText("Please find Observations CSV!") // Optional: Add a message
                .SetTitle("Share via") // Optional: Set the title of the share dialog
                .Share(); // Invoke the share dialog
        }
        else
        {
            Debug.LogError("File not found: " + reportpath);
        }          
#endif

    }

    public void OpenInspectionPDF()
    {
        table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationDb1.getConnection();
        mlocationDb1.getDataBypassedId(int.Parse(SavedInspPrimaryTableID.text.ToString()));

        folderpath = mlocationDb1.folderpath.ToString();

        string filepath = Application.persistentDataPath + folderpath.ToString() + "/InspectionReport.pdf";
#if UNITY_EDITOR

        if (File.Exists(filepath))
        {
            System.Diagnostics.Process.Start(filepath);
        }
        else
        {
            return;
        }

//#elif UNITY_IPHONE //THIS ALSO WORKS FOR IOS, JUST CALLED NATIVE FOR ANDROID & IOS as per below...

//        //File.Copy(OriginPath, destination, true);

//        //string reportpath1 = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf";
//        string reportpath1 = folderpath.ToString() + "/InspectionReport.pdf";

//        iOSOpenURL opener = new iOSOpenURL();
//        opener.OpenPDF(reportpath1);

#elif UNITY_ANDROID || UNITY_IPHONE
         if (File.Exists(filepath))
        {
            // Open the PDF file using the default PDF viewer
           
        //AndroidContentOpenerWrapper.OpenContent(Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf");
     // Use NativeShare to share the file
            new NativeShare()
                .AddFile(filepath) // Attach the PDF file
                .SetSubject("Sharing PDF") // Optional: Set the subject
                .SetText("Please check out this PDF!") // Optional: Add a message
                .SetTitle("Share via") // Optional: Set the title of the share dialog
                .Share(); // Invoke the share dialog
        }
        else
        {
            Debug.LogError("File not found: " + filepath);
        }       
#endif
    }



}