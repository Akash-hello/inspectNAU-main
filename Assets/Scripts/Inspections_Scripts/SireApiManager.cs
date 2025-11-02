using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Sockets;
using System;
using System.IO;
using DataBank;
using System.Linq;

public class SireApiManager : MonoBehaviour
{
    public TMP_InputField imoInputField;  // Input field for IMO number
    public TextMeshProUGUI Dateperiod; // Input field for start date
    
    public Button fetchButton;            // Button to fetch inspections
    public Transform vesseldetailspanel;      // Parent object for vesseldetails
    public GameObject vesseldetailsPrefab;          // Prefab for table rows

    public Transform inspectionsdetailsContainer;      // Parent object for rows (Grid Layout Group)
    public GameObject inspectionsdetailsPrefab;          // Prefab for table rows
   
    private string BaseUrl = "https://sire.ocimf.org/api/";
    private string ApiKey = "";//"YOUR_ACCESS_TOKEN";
    public TMP_InputField months;   // Input field for monthsfrom now.
    string[,] inspections;
    public GameObject errorpopup;

    public TextMeshProUGUI PopUpMsg;
    private static readonly HttpClient client = new HttpClient(); // Persistent client
    public int countofinsp;
    public TextMeshProUGUI inspcount;
    public GameObject dateperiodpanel;
    string selectedInspectionID;
    string inspectiongeneraldetails;
    string inspectiongeneraldetails1;
    List<string> InspectionsGeneralDetails;
    List<string> ObservationsList;
    List<string> ObservationsListheader;
    public string fileName = "";
    public string filePath;

    public GameObject historicdataBtn;
    public GameObject historicdataBtnBottom;
    public GameObject historicdataBtnInfopnl;

    public TMP_InputField Imonumber; //From inspection Info panel if clicked from there.
    public TextMeshProUGUI Insptype; //From inspection Info panel if clicked from there.

    public TMP_Dropdown historicdatafor;
    public string historicdataval;
    public List<string> APIkey = new List<string>();

    void Start()
    {
        fetchButton.onClick.AddListener(FetchInspections);
        dateperiodpanel.SetActive(false);
        imoInputField.text = "";
        months.text = "";
        selectedInspectionID = "";
        inspectiongeneraldetails = "";
        inspectiongeneraldetails1 = "";
        ApiKey = "";
        table_LoginConfig mLocationDb = new table_LoginConfig();
        using var connection = mLocationDb.getConnection();
        mLocationDb.getLatestID();
        connection.Close();

        if (String.IsNullOrEmpty(mLocationDb.MarketSet)) //USED FOR API KEYS for SIRE / RIGHTSHIP, etc, historic data ; 27th Feb 2025
        {
            historicdataBtn.SetActive(false);
            historicdataBtnBottom.SetActive(false);
            historicdataBtnInfopnl.SetActive(false);
        }

        else if (mLocationDb.MarketSet.ToUpper().Contains("SIRE") || mLocationDb.MarketSet.ToUpper().Contains("RIGHTSHIP") || mLocationDb.MarketSet.ToUpper().Contains("OTHER"))
        {
            historicdataBtn.SetActive(true);
            historicdataBtnBottom.SetActive(true);
            historicdataBtnInfopnl.SetActive(true);

            string inspectionkeysinDB = mLocationDb.MarketSet;
            List<string> extractedOptions = ExtractMarketSets(inspectionkeysinDB);

            historicdatafor.ClearOptions();

            if (extractedOptions.Count > 0)
            {
                List<string> finalOptions = new List<string> { "Select" };
                historicdatafor.AddOptions(finalOptions);
                historicdatafor.AddOptions(extractedOptions);
                historicdatafor.value = 0;
            }
            else
            {
                historicdatafor.AddOptions(new List<string> { "None" }); // Default option
            }
        }

    }

    private List<string> ExtractMarketSets(string dbValue) //USED FOR API KEYS for SIRE And RIGHTSHIP Only at this time; 27th Feb 2025
    {
        List<string> options = new List<string>();

        // Ensure there's at least one entry
        if (string.IsNullOrWhiteSpace(dbValue))
            return options;

        // Split by " | " only if multiple exist; otherwise, treat as a single entry
        string[] entries = dbValue.Contains("|") ? dbValue.Split('|') : new string[] { dbValue };

        foreach (string entry in entries)
        {
            string trimmedEntry = entry.Trim(); // Remove whitespace
            string[] parts = trimmedEntry.Split(':'); // Split by ':'

            if (parts.Length > 1) // Ensure valid format
            {
                string key = parts[0]; // Extract "SIRE_ApiKey"
                APIkey.Add(parts[1]);
                // Extracting market name (e.g., "SIRE" -> "SIRE")
                if (key.ToUpper().Contains("SIRE") || key.ToUpper().Contains("RIGHTSHIP"))
                {
                    options.Add(key.Split('_')[0]); // Take only "SIRE" or "RIGHTSHIP"
                }
            }
        }

        return options.Distinct().ToList(); // Remove duplicates
    }

    public void clickedfrominspinfo() //This is only attached to the button on the Information Panel area...
    {
        imoInputField.text = Imonumber.text.ToString();
    }

    public async void FetchInspections()
    {

        if (string.IsNullOrEmpty(imoInputField.text) || string.IsNullOrEmpty(months.text)|| historicdatafor.value == 0)
        {
            if (historicdatafor.value == 0)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Please select the desired inspecion type, enter IMO number and period to fetch the inspections historic data.";
            }
            else
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "Please enter the IMO Number and period to fetch the inspection details for a vessel.";
            }

        }

        else
        {

            string imoNumber = imoInputField.text;
            string startDate = System.DateTime.Now.Date.AddMonths(-(int.Parse(months.text))).ToString("dd/MM/yyyy");
            string endDate = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
            dateperiodpanel.SetActive(true);
            Dateperiod.text = "Inspections between: " + startDate + " & " + endDate;
            //string url = $"{BaseUrl}inspections?imo={imoNumber}&start_date={startDate}&end_date={endDate}";

            string url = "https://mocki.io/v1/550a021c-de44-479b-91fe-30c5b49040fc"; //SIRE 2.0

            Debug.Log("Fetching inspections from: " + url);

            try
            {
                if (historicdatafor.options[historicdatafor.value].text.ToUpper().Contains("SIRE"))
                {
                    ApiKey = APIkey[0].ToString();
                }

                else if (historicdatafor.options[historicdatafor.value].text.ToUpper().Contains("RIGHTSHIP"))
                {
                    ApiKey = APIkey[1].ToString();
                }

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKey);
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    ParseInspectionResponse(jsonResponse);
                }
                else
                {
                    ShowError("Oops! Seems you are offline or there may be some issue with the API. Please try again later. Error: " + response.StatusCode);
                }
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.InnerException is SocketException socketEx)
                {
                    ShowError("Network error! You might be offline. " + socketEx.Message);
                }
                else
                {
                    ShowError("Oops! Seems there is a network issue: " + httpEx.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("An unexpected error occurred: " + ex.Message);
            }
        }
    }

    void ShowError(string message)
    {
        errorpopup.SetActive(true);
        PopUpMsg.text = message;
        //Debug.LogError(message);
    }


    private void ParseInspectionResponse(string json)
    {
        ClearTable();
        countofinsp = 0;
        var data = JsonConvert.DeserializeObject<SireInspectionResponse>(json);

        //if (data != null)
        //{
        //    DisplayVesselDetails(data.vessel);
        //    DisplayInspections(data.inspections);
        //}
        if (data.vessel != null)
        {
            countofinsp = data.inspections.Count;
            GameObject row = Instantiate(vesseldetailsPrefab, vesseldetailspanel);
            row.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = countofinsp.ToString();
            row.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.vessel.imo_number;
            row.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = data.vessel.name;
            row.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = data.vessel.flag;
            row.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = data.vessel.@operator;
            row.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = data.vessel.ship_type;
            row.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = data.vessel.built_year;
        }
        
        Debug.Log("The number of inspections carried out in the selected period; " + countofinsp.ToString());

        foreach (var inspection in data.inspections)
        {
            GameObject row = Instantiate(inspectionsdetailsPrefab, inspectionsdetailsContainer);
            row.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =  inspection.inspection_id;
            row.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =  inspection.inspector;

            DateTime parsedDate = DateTime.Parse(inspection.date);
            
            row.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = parsedDate.ToString("dd/MMM/yyyy");
            row.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text =  inspection.port;
            row.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text =  inspection.status;

            Button rowButton = row.transform.GetChild(6).GetComponent<Button>();
            //selectedInspectionID = inspection.inspection_id;
            
            //inspectiongeneraldetails = "Vessel: " + data.vessel.name + ",IMO Number: " + data.vessel.imo_number+",Flag: " + data.vessel.flag + ",Operator: " + data.vessel.@operator + ",Type: " + data.vessel.ship_type + ",Year Built: " + data.vessel.built_year;
            //inspectiongeneraldetails1 = "Inspection ID: " + inspection.inspection_id + ",Inspected by: " + inspection.inspector + ",Date of Inspection: " + parsedDate.ToString("dd/MMM/yyyy") + ",Port of Inspection: " + inspection.port + ",Status: " + inspection.status;

            rowButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Vessel: " + data.vessel.name + ",IMO Number: " + data.vessel.imo_number + ",Flag: " + data.vessel.flag + ",Operator: " + data.vessel.@operator + ",Type: " + data.vessel.ship_type + ",Year Built: " + data.vessel.built_year;
            rowButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Inspection ID: " + inspection.inspection_id + ",Inspected by: " + inspection.inspector + ",Date of Inspection: " + parsedDate.ToString("dd/MMM/yyyy") + ",Port of Inspection: " + inspection.port + ",Status: " + inspection.status;

            rowButton.onClick.AddListener(() => FetchInspectionDetails(inspection.inspection_id, rowButton));
            //rowButton.onClick.AddListener(() => FetchInspectionDetails(inspection.inspection_id));
        }
    }                                                                        

    private void ClearTable()
    {
        foreach (Transform child in inspectionsdetailsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in vesseldetailspanel)
        {
            Destroy(child.gameObject);
        }

    }

    public void forcombineddownload()
    {
       GameObject[] downloadObsbuttons = GameObject.FindGameObjectsWithTag("historicsireinsp");

        foreach (var inspectioninfo in downloadObsbuttons)
        {
            inspectioninfo.GetComponent<Button>().onClick.Invoke();
        }
    }

    public async void FetchInspectionDetails(string selectedInspectionpassed, Button fetchinsp)
    {
        //string url = $"{BaseUrl}inspections/{selectedInspectionpassed}";
        string url = "https://mocki.io/v1/af09120b-8118-480a-ad0a-eb00b0e6e3db"; //SIRE 2.0 Observations data
        Debug.Log("Fetching details for: " + url);

        // Get the first and second child of the clicked button
        Transform firstChild = fetchinsp.transform.GetChild(0);
        Transform secondChild = fetchinsp.transform.GetChild(1);

        // Get the TextMeshProUGUI components
        TextMeshProUGUI InspGenInfo = firstChild.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI InspGenInfo1 = secondChild.GetComponent<TextMeshProUGUI>();

        using (HttpClient client = new HttpClient())
        {                                    
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKey);

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                ParseInspectionDetails(jsonResponse, selectedInspectionpassed, InspGenInfo.text, InspGenInfo1.text);
            }
            else
            {
                Debug.LogError("API Error: " + response.StatusCode);
            }
        }

    }

    private void ParseInspectionDetails(string json, string selectedInspectionpassed, string inspgendetail, string inspgendetails1)
    {
        var details = JsonConvert.DeserializeObject<InspectionDetailResponse>(json);
        ObservationsList = new List<string>();
       
        fileName = selectedInspectionpassed + "-SireHistoric.csv";

        foreach (var obs in details.observations)
        {
            Debug.Log(obs.id + ""+ obs.category + "" + obs.description + "" + obs.severity + "" + obs.status);

            ObservationsList.Add(CreateCsvRow(obs.id, obs.category, obs.description, obs.severity, obs.status));

        }
        ExportListToCsv(ObservationsList, fileName, inspgendetail, inspgendetails1);
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

    void ExportListToCsv(List<string> observationcontent, string fileName, string inspgeninfo, string inspgeninfo1)
    {
        
        filePath = Path.Combine(Application.persistentDataPath + "/" + fileName);
       
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write the Inspections Details row

            writer.WriteLine(inspgeninfo);
            writer.WriteLine(inspgeninfo1);

            writer.WriteLine();
            writer.WriteLine();

            // Write the header row
            ObservationsListheader = new List<string>();

            ObservationsListheader.Add($"Observation ID, Category, Description, Severity, Status");

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
       
#elif UNITY_ANDROID || UNITY_IPHONE
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


}

// API Response Classes
[System.Serializable]
public class SireInspectionResponse
{
    public VesselDetails vessel;
    public List<Inspection> inspections;
}

[System.Serializable]
public class VesselDetails
{
    public string imo_number;
    public string name;
    public string flag;
    public string @operator;
    public string ship_type;
    public string built_year;
}


[System.Serializable]
public class Inspection
{
    public string inspection_id;
    public string inspector;
    public string date;
    public string port;
    public string status;
}

[System.Serializable]
public class InspectionDetailResponse
{
    public List<Observation> observations;
}

[System.Serializable]
public class Observation
{
    public string id;
    public string category;
    public string description;
    public string severity;
    public string status;
}
