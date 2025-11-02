using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBank;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using TMPro;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

public class SynchDataManager : MonoBehaviour
{
    public int Idincrement;
    public int id;
    Text DBMachineID;
    public Text machineid;
    public GameObject ConfigManager;
    public string data;
    public string selecteditemcode;
    public int InspectionPrimaryID;
    public string processed;
    public string sourcetable;
    public string flag;
    public string filename;
    public TextMeshProUGUI LastSynchtime;
    public DateTime LastSynched;
    public GameObject SynchGreen;
    public GameObject SynchRed;
    public GameObject SynchItems;
    public string outputfetchall;
    public string timestamp;
    //table_Synch mLocationDb;
    Text OutgoingAPIFmConfig;
    Text OutgoingAPI;
    //public bool OutboundToggleONOff;
    public GameObject APITestOutgoing; // THis is one gameobject with below script attached and we are accessing something within this. In this case boolean.
    APIInputsAndTests APITestScript;
    //public Button Inqueue;
    public TextMeshProUGUI InQueCount;
    public TextMeshProUGUI InQueCountBottom;
    public TextMeshProUGUI ShowingHeader;
    public RectTransform ParentPanel;
    public string useremailid;
    public List<string> exportfile;

    //********* IMPORTANT FOR SYNCH, commented by Mohit 21st Feb 2024

    //How to additem to the Synch Table and Synchronise; 
    //  public AddItem additem; // This line was used to associate table_Synch Script with the AddItem Script by writing "synchtable.additem = this;" in the start method of AddItem Script
    //From Additem function named; "public void Synchronise()" was used to send items to Synch table for outbound.


    public GameObject SynchItemPrefab;

    public Scrollbar itemscrollist;
    public bool initiallistload;
    public bool verticalscrollmainlist;
    public bool searchlistload;
    public bool searchscrollmainlist;
    public DateTime lasttriggered;
    int page;
    int maxpages;
    public int totatlrecords;
    List<string> printedids;
    bool synchroniserbusy;

    public TextMeshProUGUI BalanceTokens;
    public GameObject BalanceTokensFreetext;

    string msgtoserver = "";
    public Image Checklistversionsupdates;
    public GameObject[] flashmesgindicators;
    int grantedvalue = 0;
    int originaltokens;
    // Start is called before the first frame update
    public TextMeshProUGUI temp_OutboundText;
    public TextMeshProUGUI temp_OutboundText1;
    List<string> ExportFiles;
    string outbound = "";
    //public Button synchmenu;
   

    void Start()
    {
        outbound = "";
        grantedvalue = 0;
        originaltokens = 0;
        msgtoserver = "";
        synchroniserbusy = false;
        useremailid = "";
        exportfile = new List<string>();
        table_LoginConfig mlocationDB = new table_LoginConfig();
        using var connection1 = mlocationDB.getConnection();
        mlocationDB.getLatestID();
        useremailid = mlocationDB.useremail;
        Checklistversionsupdates.GetComponent<Image>().color = Color.white;

        foreach (GameObject msg in flashmesgindicators)
        {
            msg.SetActive(false);
           
        }

        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();
        Idincrement = 0;
        APITestScript = APITestOutgoing.GetComponent<APIInputsAndTests>();

        checkifmachineidexists();
        InvokeRepeating("SynchroniseCheck", 10.0f, 180.0f);

        ConfigManager.SetActive(true);

        InQueCountBottom.text = "";

        initiallistload = false;
        searchlistload = false;

        string syncPath = "Synchronisation";

        string folderpath = Path.Combine(Application.persistentDataPath, syncPath);

        try
        {
            // Check if the folder does not exist
            if (!Directory.Exists(folderpath))
            {
                // Attempt to create the folder
                Directory.CreateDirectory(folderpath);
                Debug.Log("Folder created successfully at: " + folderpath);

            }
            else
            {
                // If the folder already exists
                Debug.Log("Folder already exists at: " + folderpath);
            }
        }
        catch (Exception ex)
        {
            // Log any errors during folder creation or checks
            Debug.LogError("Error creating folder: " + ex.Message);
        }

        //temp_OutboundText.text = "";
        //temp_OutboundText1.text = "";
    }

    public void checkifmachineidexists()
    {
        //systemconfigcode = new SystemDataConfig();
        if (string.IsNullOrEmpty(APITestScript.OutgoingAPI.text.ToString()))
        {
            OutgoingAPI = APITestScript.OutgoingAPI;
            //Debug.Log("FROM SYNCH CODE - There is no Outgoing API link entered or saved.");
        }

        if (string.IsNullOrEmpty(APITestScript.machineIDfromDB.text.ToString()))
        {
            //Debug.Log("FROM SYNCH CODE Machine ID from DB -- DOES NOT EXIST");
        }

        else
        {
            machineid = APITestScript.machineIDfromDB;
            //Debug.Log("FROM SYNCH CODE Machine ID from DB -- " + machineid.text.ToString());
            OutgoingAPI = APITestScript.OutgoingAPI;
            //Debug.Log("FROM SYNCH CODE" + "Outgoing API = " + OutgoingAPI.text.ToString());
        }
    }
    public void OnVerticalScrollValueChange()
    {

        if (itemscrollist.value < 0.05f && page < maxpages)
        {
            if ((long)(DateTime.Now - lasttriggered).TotalMilliseconds > 1500)
            {
                page += 1;

                if (verticalscrollmainlist == true)
                {
                    initiallistload = false;
                    FetchAll();
                }

                if (searchscrollmainlist == true)
                {
                    searchlistload = false;
                    ////Debug.Log("YOU SCROLLED");
                    FetchUnprocessed_1();

                }

                lasttriggered = DateTime.Now;
            }

            itemscrollist.value = 1.0f;
        }
    }


    public void AddData()

    {
        table_Synch mLocationDb1 = new table_Synch();
        using var connection = mLocationDb1.getConnection();
        mLocationDb1.addData(new SynchEntity(id, machineid.text.ToString(), data, selecteditemcode.Trim(), InspectionPrimaryID, "N", sourcetable.ToString(), flag, filename, DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
        //mLocationDb1.addData(new SynchEntity(id, machineid.text.ToString(), data, selecteditemcode, 80, "N", "SASAS", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

        ////Debug.Log("SYNCH DATA TO BE ADDED is" + data + "  -- LATEST ID IN TO INSERT IS : " + id.ToString());
        //mLocationDb1.close();

        //mLocationDb.deleteAllData();
        //mLocationDb.vacuumAllData();

        //timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
        //table_Synch mLocationDb1 = new table_Synch();
        ////mLocationDb.addData(new SynchEntity(id, machineid, data, processed, sourcetable, timestamp));
        // mLocationDb1.addData(new SynchEntity(1, "machineid-2", "data-1","" ,-5,"N", "sourcetable-1", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
        //mLocationDb1.close();
    }



    public void CountUnProcessed()
    {
        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();

        using System.Data.IDataReader reader = mLocationDb.CountNotProcessed();
        InQueCount.text = mLocationDb.CountRecords.ToString();
        InQueCountBottom.text = "Number of records in queue: " + mLocationDb.CountRecords.ToString();


    }

    public void FetchUnprocessed()
    {
        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();
        searchscrollmainlist = true;
        verticalscrollmainlist = false;
        initiallistload = false;
        searchlistload = true;
        using System.Data.IDataReader reader = mLocationDb.CountNotProcessed();
        totatlrecords = mLocationDb.CountRecords;
        maxpages = (mLocationDb.CountRecords / 5) + 1;
        searchlistload = true;
        page = 0;
        printedids = new List<string>();

        //mLocationDb.close();
        FetchUnprocessed_1();
    }

    public void FetchUnprocessed_1()

    {
        if (searchlistload == true)
        {
            for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }

        }
        table_Synch mLocationDb2 = new table_Synch();
        using var connection = mLocationDb2.getConnection();
        using System.Data.IDataReader reader = mLocationDb2.Inqueue(10, page);

        int fieldCount = reader.FieldCount;
        List<SynchEntity> myList = new List<SynchEntity>();

        int taken = 1;
        while (reader.Read())
        {
            if (printedids.Contains(reader[0].ToString()))
            {
                continue;
            }
            if (taken > 10)
            {
                continue;
            }
            printedids.Add(reader[0].ToString());

            taken++;
            SynchEntity entity = new SynchEntity(int.Parse(reader[0].ToString()),
                                    reader[1].ToString(),
                                    reader[2].ToString(),
                                    reader[3].ToString(),
                                    int.Parse(reader[4].ToString()),
                                    reader[5].ToString(),
                                    reader[6].ToString(),
                                    reader[7].ToString(),
                                    reader[8].ToString(),
                                    reader[9].ToString());

            SynchItems = Instantiate(SynchItemPrefab) as GameObject;
            SynchItemPrefab SynchItemscontent = SynchItems.GetComponent<SynchItemPrefab>();
            SynchItems.transform.SetParent(ParentPanel, false);
            //barcode.ChangeLastResult(result._qrcode.ToString());

            SynchItemscontent.DatabaseID.text = "#" + reader[0].ToString().Trim(); //"App Database ID; " + 

            SynchItemscontent.MachineID.text = reader[1].ToString().Trim();
            SynchItemscontent.Data.text = reader[2].ToString().Trim(); // "Item: " + 
            SynchItemscontent.ItemCode.text = reader[3].ToString().Trim(); // Generalinspectiondetails, Ships_IMO+Inspection_Date, etc.

            SynchItemscontent.Instruction.text = reader[7].ToString().Trim(); // "Type, this is mapped to "FLAG" column in the database.
            if (int.Parse(reader[4].ToString()) < 0)
            {
                SynchItemscontent.Quantity.text = "'" + reader[4].ToString().Trim() + "' Reduced";
            }
            if (int.Parse(reader[4].ToString()) > 0)
            {
                SynchItemscontent.Quantity.text = "'" + reader[4].ToString().Trim() + "' Added";
            }
            //if (int.Parse(result._quantity.ToString()) == 0)
            //{
            //     SynchItems.transform.Find("Quantity").GetComponentInChildren<Text>().text = result._flag.ToString();
            //}
            if (reader[7].ToString().Trim() == "Delete_Item" || reader[7].ToString().Trim() == "New_Item" || reader[7].ToString().Trim() == "Update_Item" || reader[7].ToString().Trim() == "Reset_all")
            {
                SynchItemscontent.Quantity.text = reader[7].ToString().Trim();
            }


            SynchItemscontent.Processed.text = reader[5].ToString().Trim(); //Synch To Server status.

            if (SynchItemscontent.Processed.text == "N")
            {
                SynchItemscontent.Status.GetComponent<Image>().color = Color.red;
            }
            else
                SynchItemscontent.Status.GetComponent<Image>().color = Color.green;
            SynchItemscontent.Source.text = reader[6].ToString().Trim(); //ZIP FILE REFERENCE
            SynchItemscontent.TimeStamp.text = reader[9].ToString().Trim();


            ////Debug.Log("SYNCH ADD DATA : " + entity._id.ToString());


            myList.Add(entity);

            var output = JsonUtility.ToJson(entity, true);
            ////Debug.Log("FROM SYNCH MANAGER: " + output);

            outputfetchall = output;
        }
        //reader.Dispose();
        ////Debug.Log("FROM SYNCH MANAGER: " + outputfetchall.ToString());
        ////Debug.Log("This is the Outbound Data Count " + myList.Count);
        //InQueCount.text = myList.Count.ToString();
        //InQueCountBottom.text = "'"+ myList.Count.ToString() + "'"+" Records in Queue";

        InQueCount.text = totatlrecords.ToString();
        InQueCountBottom.text = "'" + totatlrecords.ToString() + "'" + " Records in Queue";

        ShowingHeader.text = "Total number of records in queue: " + totatlrecords.ToString();

        //reader.Close();
        //mLocationDb2.close();

    }

    public void Fetchall1()

    {
        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();
        searchscrollmainlist = false;
        verticalscrollmainlist = true;
        initiallistload = true;
        searchlistload = false;
        totatlrecords = mLocationDb.totalRecords();
        maxpages = (totatlrecords / 5) + 1;
        page = 0;

        printedids = new List<string>();
        //mLocationDb.close();
        FetchAll();
    }

    public void FetchAll()

    {
        if (initiallistload == true)
        {
            for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }
        }


        table_Synch mLocationDb2 = new table_Synch();
        using var connection = mLocationDb2.getConnection();
        using System.Data.IDataReader reader = mLocationDb2.getAllData1(5, page);

        //System.Data.IDataReader reader = mLocationDb2.getDataByString();

        int fieldCount = reader.FieldCount;
        List<SynchEntity> myList = new List<SynchEntity>();
        int taken = 1;
        while (reader.Read())
        {
            if (printedids.Contains(reader[0].ToString()))
            {
                continue;
            }
            if (taken > 10)
            {
                continue;
            }
            printedids.Add(reader[0].ToString());

            taken++;
            SynchEntity entity = new SynchEntity(int.Parse(reader[0].ToString()),
                                    reader[1].ToString(),
                                    reader[2].ToString(),
                                    reader[3].ToString(),
                                    int.Parse(reader[4].ToString()),
                                    reader[5].ToString(),
                                    reader[6].ToString(),
                                    reader[7].ToString(),
                                    reader[8].ToString(),
                                    reader[9].ToString());


            SynchItems = Instantiate(SynchItemPrefab) as GameObject;
            SynchItemPrefab SynchItemscontent = SynchItems.GetComponent<SynchItemPrefab>();
            SynchItems.transform.SetParent(ParentPanel, false);
            //barcode.ChangeLastResult(result._qrcode.ToString());


            SynchItemscontent.DatabaseID.text = "#" + reader[0].ToString().Trim();//"App Database ID; " + 

            SynchItemscontent.MachineID.text = reader[1].ToString().Trim();
            SynchItemscontent.Data.text = reader[2].ToString().Trim();// "Item: " + 

            SynchItemscontent.ItemCode.text = reader[3].ToString().Trim(); //generalinspectiondetails, Ships_IMO+Inspection_Date, etc.


            SynchItemscontent.Instruction.text = reader[7].ToString().Trim();// "Type

            if (int.Parse(reader[4].ToString()) < 0)
            {
                SynchItemscontent.Quantity.text = "'" + reader[4].ToString().Trim() + "' Reduced";
            }
            if (int.Parse(reader[4].ToString()) > 0)
            {
                SynchItemscontent.Quantity.text = "'" + reader[4].ToString().Trim() + "' Added";
            }
            //if (int.Parse(result._quantity.ToString()) == 0)
            //{
            //     SynchItems.transform.Find("Quantity").GetComponentInChildren<Text>().text = result._flag.ToString();
            //}
            if (reader[7].ToString().Trim() == "Delete_Item" || reader[7].ToString().Trim() == "New_Item" || reader[7].ToString().Trim() == "Update_Item" || reader[7].ToString().Trim() == "Reset_all")
            {
                SynchItemscontent.Quantity.text = reader[7].ToString().Trim();
            }


            SynchItemscontent.Processed.text = reader[5].ToString().Trim();

            if (SynchItemscontent.Processed.text == "N")
            {
                SynchItemscontent.Status.GetComponent<Image>().color = Color.red;
            }
            else
                SynchItemscontent.Status.GetComponent<Image>().color = Color.green;

            SynchItemscontent.Source.text = reader[6].ToString().Trim();
            SynchItemscontent.TimeStamp.text = reader[9].ToString().Trim();

            ////Debug.Log("SYNCH ADD DATA : " + entity._id.ToString());
            myList.Add(entity);

            var output = JsonUtility.ToJson(entity, true);
            ////Debug.Log("FROM SYNCH MANAGER: " + output);

            outputfetchall = output;
        }

        ShowingHeader.text = "Total number of records: " + totatlrecords.ToString();

    }


    //public void LastSynchTime()
    //{

    //    System.Data.IDataReader reader = mLocationDb.getLatestTimeStamp();

    //    int fieldCount = reader.FieldCount;
    //    List<SynchEntity> myList = new List<SynchEntity>();
    //    while (reader.Read())
    //    {
    //        SynchEntity entity = new SynchEntity(int.Parse(reader[0].ToString()),
    //                                reader[1].ToString(),
    //                                reader[2].ToString(),
    //                                reader[3].ToString(),
    //                                int.Parse(reader[4].ToString()),
    //                                reader[5].ToString(),
    //                                reader[6].ToString(),
    //                                reader[7].ToString());


    //        LastSynched = Convert.ToDateTime(entity._timestamp);
    //        myList.Add(entity);

    //        var output = JsonUtility.ToJson(entity, true);
    //        //Debug.Log("FROM SYNCH LAST SYNCH TIME: " + output);
    //        LastSynchtime.text = "Last Synchronised: " + LastSynched.ToString("dd MMM yyyy HH:mm") + " hrs UTC";
    //    }
    //}

    void SynchroniseCheck()
    {
        if (synchroniserbusy == true)
        {
            Debug.Log("Synchronisation is already in progress. Skipping this cycle.");
            return;
        }

        Synchronise();
    }

    public void Synchronise()
    {
        synchroniserbusy = true;

        if (APITestScript.Out_API_NA.GetComponent<Toggle>().isOn == false)
        {
            APITestScript.OutgoingTestConnectionOnClick();
            StartCoroutine(PassOutgoingtestResult());
            //Debug.Log("TWO WAY!!");
        }
        else
        {
            //Debug.Log("STANDALONE MODE IS SWITCHED ON !!");
            LastSynchtime.text = "No Synch setup, system on standalone mode.";
        }

    }

    IEnumerator PassOutgoingtestResult()

    {
        yield return new WaitForSeconds(3.0f);
        if (APITestScript.OutgoingResult == true)
        {
            //Debug.Log("Ready to SYNCH");
            Outbounddata();
            LastSynchtime.text = "Online: Last Connected: " + DateTime.Now.ToString("dd MMM yyyy HH:mm") + " hrs";
            SynchGreen.SetActive(true);
            SynchRed.SetActive(false);
        }
        else
        {
            //Debug.Log("Failed to SYNCH");
            SynchGreen.SetActive(false);
            SynchRed.SetActive(true);
            LastSynchtime.text = "Offline.";
            synchroniserbusy = false;
            Debug.Log("Synchroniser released for retrying, as API was unable to connect.");
        }
    }


    public void Outbounddata()
    {
        string zipFilePath = "";
        string base64String = "";
        string jsonOutput = "";
        ExportFiles = new List<string>();
        outbound = "";

        List <Dictionary<string, string>> exportData = new List<Dictionary<string, string>>();

        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();
        using System.Data.IDataReader reader = mLocationDb.NotyetProcessed();

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        List<string> myList = new List<string>();
        

        while (reader.Read())
        {
            SynchEntity entity = new SynchEntity(int.Parse(reader[0].ToString()),
                                    reader[1].ToString(),
                                    reader[2].ToString(),
                                    reader[3].ToString(),
                                    int.Parse(reader[4].ToString()),
                                    reader[5].ToString(),
                                    reader[6].ToString(),
                                    reader[7].ToString(),
                                    reader[8].ToString(),
                                    ((DateTime)reader[9]).ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture)); // Format timestamp
                                    //reader[9].ToString()) ;

            myList.Add(JsonUtility.ToJson(entity));


            zipFilePath = Application.persistentDataPath + "/Synchronisation/" + entity._filename.Trim();

           // Debug.Log("From Kanishka First AA zipFilePath;" + zipFilePath);
            
            if (!File.Exists(zipFilePath))
            {
                Debug.Log("Zip file not found at path: " + zipFilePath);
                base64String = "";

                Dictionary<string, string> fileEntry = new Dictionary<string, string>
            {
                { "filename", "" },
                { "base64String", base64String } //DISCONTINUED USE OF base64String  THIS WAS CAUSING INTENSE LOAD ON THE MOBILE DEVICE AND ALSO CREATING ISSUES IN FILE TRANSFER SO THE FILE TRANSFER WAS SHIFTED TO DIRECT BUFFER METHOD.
            };
                exportData.Add(fileEntry);
            }

            else
            {
                ExportFiles.Add(entity._filename.Trim());

                //byte[] fileBytes = File.ReadAllBytes(zipFilePath);
                // Convert byte array to Base64 string
                //base64String = Convert.ToBase64String(fileBytes); //DISCONTINUED USE OF base64String  THIS WAS CAUSING INTENSE LOAD ON THE MOBILE DEVICE AND ALSO CREATING ISSUES IN FILE TRANSFER SO THE FILE TRANSFER WAS SHIFTED TO DIRECT BUFFER METHOD.

                base64String = "";

                // Create a dictionary for this file
                Dictionary<string, string> fileEntry = new Dictionary<string, string>
            {
                { "filename", entity._filename.Trim() },
                { "base64String", base64String } //DISCONTINUED USE OF base64String  THIS WAS CAUSING INTENSE LOAD ON THE MOBILE DEVICE AND ALSO CREATING ISSUES IN FILE TRANSFER SO THE FILE TRANSFER WAS SHIFTED TO DIRECT BUFFER METHOD.
            };
                exportData.Add(fileEntry);
            }

            // Convert to JSON using Newtonsoft.Json
            jsonOutput = JsonConvert.SerializeObject(exportData, Formatting.Indented);

        }

        //exportfilejsonList = JsonUtility.ToJson(new Base64Wrapper { Base64Strings = exportfile });

        //string output = "";

        if (myList.Count > 0 && ExportFiles.Count>0)
        {
            outbound = "[" + String.Join(",", myList) + "," + String.Join(",", jsonOutput.Replace("[", "").Replace("]", ""));

            StartCoroutine(FileTransmission());

            //StartCoroutine(UploadInventory(outbound)); SHIFTED TO ABOVE COROUTINE;

            InQueCount.text = myList.Count.ToString();
            InQueCountBottom.text = "Records in Queue - " + "'" + myList.Count.ToString() + "'";

        }

        else if (myList.Count > 0)
            {
                outbound = "[" + String.Join(",", myList) + "," + String.Join(",", jsonOutput.Replace("[", "").Replace("]", ""));

                StartCoroutine(UploadInventory(outbound));
                InQueCount.text = myList.Count.ToString();
                InQueCountBottom.text = "Records in Queue - " + "'" + myList.Count.ToString() + "'";

            }

        else //THERE ARE NO FILES AND NOTHING IN QUEUE
        {
            msgtoserver = "anymessageforme";
            StartCoroutine(SendRegnDataToCloud()); //CHECK IF SERVER HAS ANY MESSAGE FOR THE APP...
        }
        //Debug.Log("This is the Outbound Data in JSON UTILITY " + output);
        //Debug.Log(output);
        ////Debug.Log("This is the Outbound Data Count " + data.Count);

        //reader.Dispose();
        //mLocationDb.close();
    }

    //public class Base64Wrapper
    //{
    //    public List<string> Base64Strings;
    //}

    IEnumerator UploadInventoryOLD(string data) //Without Zip File and useremail details.
    {
        string msgtoserver = "";
        msgtoserver = "appsyncdata";
        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();
        string bodyJsonString = data + "|MessageToServer: '" + msgtoserver + "' Useremailid: '" + useremailid + "'";//Newtonsoft.Json.JsonConvert.SerializeObject(data);
        var request = new UnityWebRequest(OutgoingAPI.text.ToString(), "POST");//API Example; "https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post". here API brought from DB AND DB IS FED FIRST TIME FROM FETCH INSPECTIONS..

        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        // convert the byte array to a string
        string rawJson = Encoding.Default.GetString(request.downloadHandler.data);

        // parse the raw string into a json result we can easily read
        JSONNode jsonResult = JSON.Parse(rawJson);

        //menuarray = new string[jsonResult.Count, 4];
        for (var i = 0; i < jsonResult.Count; i++)
        {
            mLocationDb.Updatedata(" Processed = '" + jsonResult[i]["_processed"] + "' where Ltrim(rtrim(ItemCode)) = Ltrim(rtrim('" + jsonResult[i]["_itemcode"] + "')) and Ltrim(rtrim(Id)) = Ltrim(rtrim('" + jsonResult[i]["_id"] + "')) and Ltrim(rtrim(MachineId)) = Ltrim(rtrim('" + jsonResult[i]["_machineid"] + "'))");
        }

    }


    IEnumerator FileTransmission()
    {
        string Url = "";
        WWWForm form = new WWWForm();
        List<string> resultforfiles = new List<string>();
        List<string> fileUploaded = new List<string>();

        foreach (string file in ExportFiles)
        {

            byte[] fileBytes = File.ReadAllBytes(Application.persistentDataPath + "/Synchronisation/" + file);

            // Create a form and add the binary data
           
            form.AddBinaryData("ExportFile", fileBytes, useremailid + ";" + file, "application/octet-stream");

            // Set the URL
            Url = "https://hsseqapi.orionmarineconcepts.com/api/Inspectionuploads/Post";

            // Create the UnityWebRequest
            UnityWebRequest request = UnityWebRequest.Post(Url, form);

            
            // Send the request
            yield return request.SendWebRequest();

            // Handle the response
            if (request.result == UnityWebRequest.Result.Success && request.downloadHandler.text.Contains("DataSynched"))// ("DataSynched")
            {
                Debug.Log("File uploaded successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("File upload failed: " + request.error);
                resultforfiles.Add("Error: " + request.error);
                
            }

            resultforfiles.Add(request.result.ToString());
            fileUploaded.Add(request.downloadHandler.text);
        }

        bool hasIssues = resultforfiles.Any(result => result.Contains("Error") || result.Contains("protocol error") || fileUploaded.Contains("NOFileUploaded") || fileUploaded.Contains("FileNotPresent"));

        if (!hasIssues) //All files transmitted successfully.
        {
            StartCoroutine(UploadInventory(outbound));
            LastSynchtime.text += "<color=#006400> Result: Files transmitted successfully to the cloud.</color>";
        }

        else
        {
            LastSynchtime.text += " <color=red> Result: Files still pending to be sent, will try again in sometime.</color>";
            msgtoserver = "anymessageforme";
            StartCoroutine(SendRegnDataToCloud()); //CHECK IF SERVER HAS ANY MESSAGE FOR THE APP...
        }
    }

    IEnumerator UploadInventory(string data)
    {
        string msgtoserver = "";
        msgtoserver = "appsyncdata";
        string bodyJsonString = "";
        // Prepare the database connection
        table_Synch mLocationDb = new table_Synch();
        using var connection = mLocationDb.getConnection();


        // Prepare the file path for the .zip file

        bodyJsonString = data + "," + "{ " + "\"" + "messagetoserver" + "\"" + ":" + "\"" + msgtoserver + "\"" + "," + "\"" + "useremailid" + "\"" + ":" + "\"" + useremailid + "\"" + "}]";

        //Debug.Log("From Kanishka 2 BodyJson;"+ bodyJsonString);  

        //// Check if the zip file exists
        //if (!File.Exists(zipFilePath))
        //{
        //    Debug.Log("Zip file not found at path: " + zipFilePath);

        //    bodyJsonString = data + ", " + "{ " + "\"" + "messagetoserver" + "\"" + ":" + "\"" + msgtoserver + "\"" + "," + "\"" + "useremailid" + "\"" + ":" + "\"" + useremailid + "\"" + "}]";

        //}

        //else
        //{
        //    byte[] fileBytes = File.ReadAllBytes(zipFilePath);
        //    // Convert byte array to Base64 string
        //    string base64String = Convert.ToBase64String(fileBytes);
        //    bodyJsonString = data + ", " + "{ " + "\"" + "messagetoserver" + "\"" + ":" + "\"" + msgtoserver + "\"" + "," + "\"" + "useremailid" + "\"" + ":" + "\"" + useremailid + "\"" +"," + "\""+"file" + "\"" + ":" + "\"" + base64String + "\""+"}]";

        //}

        var request = new UnityWebRequest("https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // Create the UnityWebRequest using the form
        //UnityWebRequest www = UnityWebRequest.Post(OutgoingAPI.text.ToString(), form);

        // Send the request and wait for the response
        yield return request.SendWebRequest();
        try
        {
            string rawJson = Encoding.Default.GetString(request.downloadHandler.data);
            //Debug.Log("From Kanishka 3 Raw Json;" + rawJson);

            //if (bodyJsonString.Contains("Reset"))
            //{
            //    temp_OutboundText1.text =  temp_OutboundText.text + " RAWJSON; " + rawJson;
            //}
           
            if (request.result == UnityWebRequest.Result.Success && !request.downloadHandler.text.Contains("Error"))
            {
                Debug.Log("Upload successful: " + request.downloadHandler.text);

                // Parse the raw response into JSON
                JSONNode jsonResult = JSON.Parse(request.downloadHandler.text);

                // Process the response data
                for (var i = 0; i < jsonResult.Count; i++)
                {
                    mLocationDb.Updatedata("Processed = '" + jsonResult[i]["_processed"] + "' where Ltrim(rtrim(ItemCode)) = Ltrim(rtrim('" + jsonResult[i]["_itemcode"] + "')) " +
                        "and Ltrim(rtrim(Id)) = Ltrim(rtrim('" + jsonResult[i]["_id"] + "')) and Ltrim(rtrim(MachineId)) = Ltrim(rtrim('" + jsonResult[i]["_machineid"] + "'))");

                }
                if (jsonResult.Count > 0)
                {
                    LastSynchtime.text += "<color=#006400> Result: File successfully processed on the cloud.</color>";
                    //temp_OutboundText.text = "success - RAWJSON; " + rawJson;
                }

                //synchmenu.GetComponent<Button>().onClick.Invoke(); THIS WAS CAUSING SYNCH TAB TO OPEN, when working elsewhere Everytime some file was in queue and sent out.
                ConfigManager.GetComponent<APIInputsAndTests>().OnClickSynch();
            }
            else
            {
                //temp_OutboundText1.text = "failed - RAWJSON; " + rawJson;
                Debug.Log("Upload failed: " + request.error);
                LastSynchtime.text += " <color=red> Result: Unable to synchronize data on cloud, will try again in few  minutes.</color>";

            }

        }
        catch (Exception ex)
        {
            Debug.Log("Try Catch Error caught upload failed: " + ex.StackTrace);
        }
        // Check for errors in the response
        //synchroniserbusy = false;
        //Debug.Log("Synchroniser released from the UploadInventory method.");
        msgtoserver = "anymessageforme";
        StartCoroutine(SendRegnDataToCloud()); //CHECK IF SERVER HAS ANY MESSAGE FOR THE APP...
    }

    IEnumerator SendRegnDataToCloud()
    {
        string columndataquery = "";
        originaltokens = int.Parse(BalanceTokens.text.ToString());
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        bool somenewupdate = false;
        //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = "ankush1maurya@gmail.com", password = "123456" });
        //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = emailinputtext, password = passwordinputtext });
        string bodyJsonString = "";

        if (msgtoserver == "anymessageforme")

        {
            bodyJsonString = JsonUtility.ToJson(new AuthData()
            {
                neworupdate = msgtoserver,
                email = mlocationDb.useremail,

                //name = mlocationDb.name,
                //companyname = mlocationDb.companyname,
                //companycode = mlocationDb.companyAuthcode,
                //designation = mlocationDb.designation,
                //password = mlocationDb.password.Trim(),
                //WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),
                //tokenremaining = mlocationDb.tokenbalance,
                //tokenrequest = 0,
                //granttoken = 0
            });
        }

        else if (msgtoserver == "updatetokenvalues")

        {
            bodyJsonString = JsonUtility.ToJson(new AuthData()
            {
                neworupdate = msgtoserver,
                email = mlocationDb.useremail,
                tokenremaining = Math.Abs(mlocationDb.tokenbalance) + grantedvalue

                //name = mlocationDb.name,
                //companyname = mlocationDb.companyname,
                //companycode = mlocationDb.companyAuthcode,
                //designation = mlocationDb.designation,

                //password = mlocationDb.password.Trim(),
                //WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),

                //tokenrequest = 0,
                //granttoken = 0
            });
        }
        connection.Close();

        var request = new UnityWebRequest("https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        try
        {
            string rawJson = Encoding.Default.GetString(request.downloadHandler.data);
            string[] versionUpdates = new string[0];
            string[] cloudDBID = new string[0];


            if (rawJson.Contains("tokensissued")) //Tokensstatus checking
            {
                // The JSON string
                //rawJson example = "{\"tokensissued\":\"2\",\"version_update\":\"1,1,1\",\"cloud_db_id\":\"1,2,6\",\"email_id\":\"sabhi@orion.com\"}";

                JObject jsonObject = JObject.Parse(rawJson);

                // Access fields directly
                //string tokensIssued = jsonObject["tokensissued"]?.ToString();
                int.TryParse(jsonObject["tokensissued"]?.ToString(), out grantedvalue);
                string emailId = jsonObject["email_id"]?.ToString();


                string versionUpdate = jsonObject["version_update"]?.ToString();
                List<(string CloudDBID, string VersionNumber)> databasePairs = FetchDatabasePairs();

                if (!string.IsNullOrEmpty(versionUpdate))
                {
                    versionUpdates = versionUpdate.Split(',');

                }

                string CloudDBID = jsonObject["cloud_db_id"]?.ToString();
                if (!string.IsNullOrEmpty(CloudDBID))
                {
                    cloudDBID = CloudDBID.Split(',');
                }

                // Ensure both arrays have the same length
                if (versionUpdates.Length != cloudDBID.Length)
                {
                    Debug.Log("Error: The lengths of version_update and cloud_db_id do not match.");

                }

                // Check if the count of pairs differs
                else if (databasePairs.Count != versionUpdates.Length)
                {
                    Debug.Log("New update received: Count of pairs differs.");
                    Checklistversionsupdates.GetComponent<Image>().color = Color.green;
                    foreach (GameObject msg in flashmesgindicators)
                    {
                        msg.SetActive(true);

                    }
                }

                else
                {
                    // Compare each pair
                    for (int i = 0; i < versionUpdates.Length; i++)
                    {
                        string currentCloudDBID = cloudDBID[i];
                        string currentVersionNumber = versionUpdates[i];

                        // Check if the database contains this pair
                        var matchingPair = databasePairs.FirstOrDefault(pair =>
                            pair.CloudDBID == currentCloudDBID && pair.VersionNumber == currentVersionNumber);

                        if (matchingPair == default)
                        {
                            somenewupdate = true;
                        }
                    }

                    if (somenewupdate == true)
                    {
                        Checklistversionsupdates.GetComponent<Image>().color = Color.green;
                        somenewupdate = true;
                        foreach (GameObject msg in flashmesgindicators)
                        {
                            msg.SetActive(true);

                        }
                    }

                    else
                    {
                        Checklistversionsupdates.GetComponent<Image>().color = Color.white;
                        foreach (GameObject msg in flashmesgindicators)
                        {
                            msg.SetActive(false);

                        }
                    }

                    //    if (matchingPair == default)
                    //    {
                    //        Debug.Log($"New version detected for CloudDBID {currentCloudDBID}: {currentVersionNumber}");
                    //        Checklistversionsupdates.GetComponent<Image>().color = Color.green;
                    //        foreach (GameObject msg in flashmesgindicators)
                    //        {
                    //            msg.SetActive(true);

                    //        }
                    //    }

                    //    else
                    //    {
                    //        Checklistversionsupdates.GetComponent<Image>().color = Color.white;
                    //        foreach (GameObject msg in flashmesgindicators)
                    //        {
                    //            msg.SetActive(false);

                    //        }
                    //    }
                    //}

                }


                //string flagtext = "";
                //int flag = 0;

                //string granttext = "";
                //int grantedvalue = 0;
                //string status = "";


                ////StartCoroutine(HidePopUp());

                //flagtext = rawJson.Split(',')[0];
                //flag = int.Parse(flagtext.Split('=')[1]);

                //granttext = rawJson.Split(',')[1];
                //grantedvalue = int.Parse(granttext.Split('=')[1]);

                //status = rawJson.Split(',')[2]; ;


                msgtoserver = "updatetokenvalues";
                connection.Close();
                StartCoroutine(SendRegnDataToCloud());
            }

            else if (rawJson.Contains("messagereceiveddbupdated"))
            {
                //table_LoginConfig mlocationDb1 = new table_LoginConfig();
                //using var connection1 = mlocationDb1.getConnection();

                if (grantedvalue > 0) // ALLOCATED
                {
                    int updatetokensinDB = 0;
                    if (int.Parse(BalanceTokens.text.ToString()) < 0)
                    {
                        updatetokensinDB = grantedvalue + Math.Abs(int.Parse(BalanceTokens.text.ToString()));
                    }

                    else
                    {
                        updatetokensinDB = grantedvalue + int.Parse(BalanceTokens.text.ToString());
                    }

                    columndataquery = "Tokendate = '0', TokenBalance = '" + updatetokensinDB + "'";

                    mlocationDb.Updatedata(columndataquery);


                    if (updatetokensinDB >= 0 && updatetokensinDB <= 2)
                    {
                        BalanceTokensFreetext.SetActive(true);
                        BalanceTokensFreetext.GetComponent<TMP_Text>().text = updatetokensinDB + " Credit(s) remaining.";
                        BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.red;
                        BalanceTokens.text = updatetokensinDB.ToString();// + " Credits";
                        BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
                    }

                    else if (updatetokensinDB > 2)

                    {
                        BalanceTokensFreetext.SetActive(true);
                        BalanceTokensFreetext.GetComponent<TMP_Text>().text = updatetokensinDB + " Credit(s) remaining.";
                        BalanceTokensFreetext.GetComponent<TMP_Text>().color = Color.black;
                        BalanceTokens.text = updatetokensinDB.ToString();
                        BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
                    }
                }

                else
                {
                    BalanceTokensFreetext.SetActive(true);
                    BalanceTokensFreetext.GetComponent<TMP_Text>().text = int.Parse(BalanceTokens.text.ToString()) + " Credit(s) remaining.";
                    BalanceTokens.text = BalanceTokens.text.ToString();// + " Credits";
                    BalanceTokens.GetComponent<TMP_Text>().color = Color.white;
                }

                //if (mlocationDb.tokenbalance < 0) //FREE CREDITS MODE
                //{
                //    BalanceTokensFreetext.SetActive(true);
                //    BalanceTokens.text = (Math.Abs(mlocationDb.tokenbalance) + grantedvalue).ToString();
                //    BalanceTokens.GetComponent<TMP_Text>().color = Color.white;

                //}

                //else if (mlocationDb.tokenbalance <= 2)
                //{
                //    BalanceTokensFreetext.SetActive(false);
                //    BalanceTokens.text = (mlocationDb.tokenbalance + grantedvalue).ToString();
                //    BalanceTokens.GetComponent<TMP_Text>().color = Color.red;
                //}

                //else
                //{
                //    BalanceTokensFreetext.SetActive(false);
                //    BalanceTokens.text = (mlocationDb.tokenbalance + grantedvalue).ToString();
                //    BalanceTokens.GetComponent<TMP_Text>().color = Color.black;
                //}

                synchroniserbusy = false;
                connection.Close();
            }

            else if (rawJson.Contains("historicdataapi")) //I.e. incase the server does not respond back with an "ValuesUpdated" message...
            {
                JObject jsonObject = JObject.Parse(rawJson);

                // Access fields directly
                //string tokensIssued = jsonObject["tokensissued"]?.ToString();
                
                string SireRightShipAPI = jsonObject["historicdataapi"]?.ToString();
                string CompanyGuid = jsonObject["guid"]?.ToString();

                columndataquery = "Marketsetting = '"+SireRightShipAPI+ " where CompanyGuid = '" + CompanyGuid + "'"; //27th Feb 2025, MARKET SETTINGS FIELD OF THE LOGIN_CONFIG table is used for SIRE and RIGHTSHIP Data API Keys.

                mlocationDb.Updatedata(columndataquery);
                synchroniserbusy = false;
                connection.Close();
            }

            else if (rawJson.Contains("nomessage"))
            {
                // The JSON string
                //rawJson example = "{\"nomessage\":\"2\",\"version_update\":\"1,1,1\",\"cloud_db_id\":\"1,2,6\",\"email_id\":\"sabhi@orion.com\"}";

                JObject jsonObject = JObject.Parse(rawJson);

                // Access fields directly
                //string tokensIssued = jsonObject["tokensissued"]?.ToString();
                //int.TryParse(jsonObject["tokensissued"]?.ToString(), out grantedvalue);
                string emailId = jsonObject["email_id"]?.ToString();


                string versionUpdate = jsonObject["version_update"]?.ToString();
                List<(string CloudDBID, string VersionNumber)> databasePairs = FetchDatabasePairs();

                if (!string.IsNullOrEmpty(versionUpdate))
                {
                    versionUpdates = versionUpdate.Split(',');

                }

                string CloudDBID = jsonObject["cloud_db_id"]?.ToString();
                if (!string.IsNullOrEmpty(CloudDBID))
                {
                    cloudDBID = CloudDBID.Split(',');
                }

                // Ensure both arrays have the same length
                if (versionUpdates.Length != cloudDBID.Length)
                {
                    Debug.Log("Error: The lengths of version_update and cloud_db_id do not match.");

                }

                // Check if the count of pairs differs
                else if (databasePairs.Count != versionUpdates.Length)
                {
                    Debug.Log("New update received: Count of pairs differs.");
                    Checklistversionsupdates.GetComponent<Image>().color = Color.green;
                    foreach (GameObject msg in flashmesgindicators)
                    {
                        msg.SetActive(true);

                    }
                }

                else
                {
                    
                    // Compare each pair
                    for (int i = 0; i < versionUpdates.Length; i++)
                    {
                        string currentCloudDBID = cloudDBID[i];
                        string currentVersionNumber = versionUpdates[i];

                        // Check if the database contains this pair
                        var matchingPair = databasePairs.FirstOrDefault(pair =>
                            pair.CloudDBID == currentCloudDBID && pair.VersionNumber == currentVersionNumber);

                        if (matchingPair == default)
                        {
                            somenewupdate = true;
                        }
                    }

                    if (somenewupdate == true)
                    {
                        Checklistversionsupdates.GetComponent<Image>().color = Color.green;
                        somenewupdate = true;
                        foreach (GameObject msg in flashmesgindicators)
                        {
                            msg.SetActive(true);

                        }
                    }

                    else
                    {
                        Checklistversionsupdates.GetComponent<Image>().color = Color.white;
                        foreach (GameObject msg in flashmesgindicators)
                        {
                            msg.SetActive(false);

                        }
                    }
                }


                synchroniserbusy = false;
                connection.Close();
                Debug.Log("Synchroniser released Since no files in queue.");
            }

            else //I.e. incase the server does not respond back with an "ValuesUpdated" message...
            {
                columndataquery = "Tokendate = '0', TokenBalance = '" + originaltokens + "'";

                mlocationDb.Updatedata(columndataquery);
                synchroniserbusy = false;
                connection.Close();
            }

            synchroniserbusy = false;
            //connection.Close();
        }

        catch (Exception ex)
        {
            Debug.Log(request.error + " ex " + ex.Message + "There was a error on this line" + ex.StackTrace);

            columndataquery = "Tokendate = '0', TokenBalance = '" + originaltokens + "'";

            mlocationDb.Updatedata(columndataquery);
            synchroniserbusy = false;
            connection.Close();
        }

    }

    List<(string CloudDBID, string VersionNumber)> FetchDatabasePairs()
    {
        table_Inspection_template mlocationDb1 = new table_Inspection_template();
        using var connection1 = mlocationDb1.getConnection();
        string query = "SELECT Cloud_DB_ID, Details_2 from Inspection_template where Details_8 like '%Version%';";

        using System.Data.IDataReader reader = mlocationDb1.getDatabyQuery(query);

        List<(string CloudDBID, string VersionNumber)> databasePairs = new List<(string CloudDBID, string VersionNumber)>();
        try
        {
            while (reader.Read())
            {
                // Get values from the result
                string cloudDBID = reader["Cloud_DB_ID"].ToString();
                string versionNumber = reader["Details_2"].ToString();

                // Add to the list
                databasePairs.Add((cloudDBID, versionNumber));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching database pairs: {ex.Message}");
        }

        reader.Dispose();
        connection1.Close();

        return databasePairs;
    }
}