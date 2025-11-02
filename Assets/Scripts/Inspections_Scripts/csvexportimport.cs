using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DataBank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class csvexportimport : MonoBehaviour
{
    public string OriginPath;
    
    string filename;
    public string filePath;
    string FileExt;
    Double Filesize;
    int ObsDBid;
    public TextMeshProUGUI InspPrimaryId;

    public string folderpath;
    List<string> Infolist;
    List<string> Infolistheader;
    List<string> SireElements;

    string FieldName = "";
    string Value = "";
    
    //public TextMeshProUGUI attachmentfilename;

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    public void downloadfile()

    {
        if (!String.IsNullOrEmpty(InspPrimaryId.text.ToString()))

        {
            filename = "ImportTemplate.csv";
            table_Inspection_PrimaryDetails mlocationDb1 = new table_Inspection_PrimaryDetails();
            using var connection = mlocationDb1.getConnection();

            mlocationDb1.getDataBypassedId(int.Parse(InspPrimaryId.text.ToString()));

            folderpath = mlocationDb1.folderpath.ToString();

            Infolist = new List<string>();

            string query = "SELECT ID, Vessel_Name, Vessel_IMO FROM Inspection_PrimaryDetails where cast(ID as int) = " + int.Parse(InspPrimaryId.text.ToString());

            using System.Data.IDataReader reader = mlocationDb1.getDatabyQuery(query);

            if (((System.Data.Common.DbDataReader)reader).HasRows)
            {
                while (reader.Read())
                {

                    if (reader[0].ToString() != "")
                    {
                        //Infolist.Add(reader[0].ToString().Trim());  //PrimaryId
                        //Infolist.Add(reader[1].ToString().Trim());  //VesselName
                        //Infolist.Add(reader[2].ToString().Trim());  //VesselIMO

                        Infolist.Add(CreateCsvRow("Inspection Number; " + reader[0].ToString().Trim() + " | Vessel Name; " + reader[1].ToString().Trim() + " | IMO Number; " + reader[2].ToString().Trim()));
                    }
                }
                reader.Dispose();
            }

            //mlocationDb1.close();
            ExportListToCsv(Infolist, filename);
        }

        else
        {
            time = 2.0f;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! This feature is available only after saving Vessel name and IMO number.";
            StartCoroutine(HidePopUp());
        }
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

    void ExportListToCsv(List<string> list, string fileName)
    {
        filePath = Path.Combine(Application.persistentDataPath + folderpath.ToString() + "/" + fileName);

        string header = "Field Name,Values";
        string[] StandardPrimaryFields = { "Inspector Name", "Inspecting Company", "Port of Inspection", "Country", "Master", "Chief Engineer", "Chief Officer", "Second Engineer" };

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the VesselName/IMO Number row

            foreach (string item in list)
            {
                writer.WriteLine(item);
            }

            // Insert a blank row (just write an empty line)
            writer.WriteLine("");

            // Insert a Header row
            writer.WriteLine(header);
           
            // Write the data rows

            foreach (string item in StandardPrimaryFields)
            {
                writer.WriteLine($"{item},");
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
#elif UNITY_IPHONE

//File.Copy(OriginPath, destination, true);

  //string reportpath1 = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf";
  string reportpath1 = folderpath.ToString()+"/"+ filename;
  
  iOSOpenURL opener = new iOSOpenURL();
  opener.OpenPDF(reportpath1);
        
#elif UNITY_ANDROID
         if (File.Exists(filePath))
        {
            // Open the PDF file using the default PDF viewer
            Application.OpenURL(reportpath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }          
#endif

    }

    // Start is called before the first frame update
    public void uploadfile()
    {
        time = 2.0f;
        filename = "";
        //attachmentfilename.text = "";
        FileExt = "";
        Filesize = 0;
       
        string FileType = NativeFilePicker.ConvertExtensionToFileType("csv");
        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection = mlocationDb.getConnection();
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation Cancelled");
            else

            {
                OriginPath = path;
                filename = Path.GetFileName(path);
                
                long filesize = new System.IO.FileInfo(OriginPath).Length;
                Filesize = (filesize / 1024 ^ 2) / 1024;

               
              using (StreamReader reader = new StreamReader(OriginPath))
            {
                string line;
                int rowCount = 0;
                    int inspectionprimaryId = 0;
                while ((line = reader.ReadLine()) != null)
                {
                        string[] fieldID = line.Split(',');
                        rowCount++;
                        if (rowCount == 1)
                        {
                            inspectionprimaryId = int.Parse(fieldID[1].Trim());
                        }

                    // Skip the first two rows (header and second row)
                    
                    if (rowCount <= 2)
                    {
                        continue;
                    }
                        string[] fields = line.Split(',');
                        // Parse the CSV line into fields (assuming simple CSV with no complex cases)

                        if (fields.Length == 4) // Ensure we have exactly 4 fields first field being the Observation Cloud_DB_ID ID
                        {
                            //Debug.Log("Recommendation = " + fields[2].Trim().ToString() + "Observation_Text = " + fields[1].Trim().ToString() + " Inspection ID =" + inspectionprimaryId + " Cloud_DB_ID = " + int.Parse(fields[0].Trim()));
                            // Insert data into the SQLite database
                            
                            string columndataquery = " Observation_Text= '" + "Test Observation Text" + "', Recommendation = '" + "Value entered for recommendation" + "' where cast(Inspection_PrimaryDetails_ID as int) = '" + inspectionprimaryId + "' and cast(Cloud_DB_ID as int) = '" + int.Parse(fields[0].Trim().ToString()) + "' and TRIM(Obs_Details_8) = 'Question' ";

                            mlocationDb.Updatedata(columndataquery);

                            //string columndataquery = @"
                            //Observation_Text= " + fields[1].Trim().ToString()+ ", Recommendation = " + fields[2].Trim().ToString() + " where cast(Inspection_PrimaryDetails_ID as int) = "+ inspectionprimaryId + " and cast(Cloud_DB_ID as int) = " + int.Parse(fields[0].Trim() + " and TRIM(Obs_Details_8) = 'Question' ");
                            //mlocationDb.Updatedata(columndataquery);
                            Debug.Log("Testing CSV Import");
                    }
                }
            }
            }
        
        }, //new string[] { FileType }) // Commented on 22nd May as Android was giving message; "No Apps can perofrm this action" as per https://forum.unity.com/threads/native-file-picker-for-android-ios-open-source.912890/page-3
           NativeFilePicker.ConvertExtensionToFileType("csv"));
        //mlocationDb.close();
    }


    // Custom CSV parser for handling commas inside quoted fields
    private static string[] ParseCsvLine(string csvLine)
    {
        // Regular expression to match CSV fields enclosed in quotes or without quotes
        var csvPattern = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");

        var fields = new List<string>();
        foreach (Match match in csvPattern.Matches(csvLine))
        {
            // Remove leading commas and trim quotes from the field
            string field = match.Value.TrimStart(',');
            if (field.StartsWith("\"") && field.EndsWith("\""))
            {
                // Remove surrounding quotes and handle escaped double quotes
                field = field.Substring(1, field.Length - 2).Replace("\"\"", "\"");
            }

            fields.Add(field);
        }

        return fields.ToArray();
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        PopUpMsg.color = Color.black;
    }

}
