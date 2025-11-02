using DataBank;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class APIInputsAndTests : MonoBehaviour
{
    public Text machineIDfromDB;
    public InputField IncomingAPIINput;
    public InputField OutboundAPIINput;
    public Button SaveCommDetails;
    public Text IncomingAPI;
    public Text OutgoingAPI; //API Example; "https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post". DB IS FED FIRST TIME FROM FETCH INSPECTIONS WITH THIS OUTBOUND API..
    public Button TestInbound;
    public Button TestOutbound;
    public Text IncomingAPITest;
    public Text OutgoingAPITest;
    //public Text OutgoingAPICheck;
    //public GameObject inventory;
    //public GameObject synch;

    public bool IncomingResult;
    public bool OutgoingResult;

    public Toggle Out_API_NA;
    public Button Proceed;
    public TextMeshProUGUI proceedwarning;
    //table_Config mLocationDb;
    //public GameObject inventory;
    //public GameObject synch;
    //public GameObject InitialisationPanel;

    ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
    //public GameObject CommsConfigPanel; 

    //public GameObject Mainmenupanel;
    //public GameObject InventoryMainmenu;
    //public GameObject InventorySubmenu;
    //public GameObject BacktoMainMenu;

    ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
    //public GameObject SynchPanel;
    public SynchDataManager Synch;
    
    public string DbMachineID;
    public string DbIncomingAPI;
    public string DbOutboundAPI;
    public string DbOrionConnect;
    public string DbLastLoginDatetime;
    public string DbTimeStamp;
    //public GameObject BotForNoItemsFound;
    //public GameObject BotForConsumption;
    public Text StartStepsCheck;
    //public GameObject LogoutConfirm;
    // Start is called before the first frame update
    void Start()
    {
        //LogoutConfirm.SetActive(false);
        //InvokeRepeating("Logout", 300.0f, 600.0f);
    }
    
        
     public void OnClickOpenConfigPanel()

    {
        //BotForConsumption.SetActive(false);
        //BotForNoItemsFound.SetActive(false);
        //CommsConfigPanel.SetActive(true); ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
        //Mainmenupanel.SetActive(false);
        //InventoryMainmenu.SetActive(false);
        //InventorySubmenu.SetActive(false);

        ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
        //SynchPanel.SetActive(false);

        //BacktoMainMenu.SetActive(true);
        ToggleLabelApi();
        GetConfigData();
        this.gameObject.GetComponent<SystemDataComfig_1>().enabled = false;
        StartStepsCheck.text = "Code line 80 - OnClickOpenConfigPanel from API and INPUTS.";
    }

    public void GetConfigData()
    {
        //Fetch All Data
        table_Config mLocationDb = new table_Config();
        using var connection = mLocationDb.getConnection();
        using  System.Data.IDataReader reader = mLocationDb.getDataByString();

        //int fieldCount = reader.FieldCount;
        List<ConfigEntity> myList = new List<ConfigEntity>();
        while (reader.Read())
        {
            ConfigEntity entity = new ConfigEntity(int.Parse(reader[0].ToString()),
        reader[1].ToString(),
        reader[2].ToString(),
        reader[3].ToString(),
        reader[4].ToString(),
        reader[5].ToString(),
        reader[6].ToString(),
        reader[7].ToString(),
        reader[8].ToString(),
        reader[9].ToString(),
        reader[10].ToString(),
        reader[11].ToString(),
        reader[12].ToString(),
        reader[13].ToString(),
        reader[14].ToString(),
        reader[15].ToString());

            Debug.Log("Machine ID: " + entity._machineid);

            DbMachineID = entity._machineid;
            //Dbhttps = entity._https;
            //Dbserverip = entity._ServerIP;
            //DbServerPort = entity._ServerPort;
            DbIncomingAPI = entity._IncomingAPI;
            DbOutboundAPI = entity._OutboundAPI;
            DbOrionConnect = entity._orionconnect;
            DbLastLoginDatetime = entity._lastlogindatetime;
            DbTimeStamp = entity._timestamp;

            myList.Add(entity);

            var output = JsonUtility.ToJson(entity, true);
            Debug.Log("FROM CONFIG MANAGER: " + output);
        }
        reader.Dispose();
        //mLocationDb.close();
        connection.Close();
        IncomingAPI.text = DbIncomingAPI.ToString().Trim();
        OutgoingAPI.text = DbOutboundAPI.ToString().Trim();
        machineIDfromDB.text = DbMachineID.ToString().Trim();
        Debug.Log("Incoming API = " + IncomingAPI.text.ToString() + "Outgoing API " + OutgoingAPI.text.ToString());

        if(string.IsNullOrEmpty(OutgoingAPI.text))
        {
            Out_API_NA.GetComponent<Toggle>().isOn = true;
        }

        else
        {
            Out_API_NA.GetComponent<Toggle>().isOn = false;
        }


        if (Out_API_NA.GetComponent<Toggle>().isOn == false)
        {
            OutgoingTestConnectionOnClick();
            IncomingTestConnectionOnClick();
        }
        else
        {
            IncomingTestConnectionOnClick();
        }
    }
    public void ToggleLabelApi()
    {
        if (Out_API_NA.GetComponent<Toggle>().isOn == false)
        {
            OutboundAPIINput.enabled = true;
            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Type / Paste Outgoing API link here";
            Out_API_NA.GetComponentInChildren<Text>().text = "Two Way";
        }

        else
        {
            OutboundAPIINput.enabled = false;
            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Not Applicable";
            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.black;
            Out_API_NA.GetComponentInChildren<Text>().text = "Standalone";
        }
    }
    public void SaveButtonOnClick()
    {

        if (Out_API_NA.GetComponent<Toggle>().isOn == false && string.IsNullOrEmpty(OutboundAPIINput.text)) //(string.IsNullOrEmpty(IncomingAPIINput.text) || string.IsNullOrEmpty(OutboundAPIINput.text)))
        { //ABOVE LINE WAS CHANGED TO GET THE FEED FOR INCOMING API AND OUTGOING API FROM THE FETCHINSPECTION COde instead of manual entry..

            IncomingAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
            IncomingAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Required; Please enter Inbound API link here";

            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Required; Please enter Outbound API link here";

            SaveCommDetails.enabled = false;
            //SaveCommDetails.GetComponent<Image>().color = Color.red;

        }

        else if (Out_API_NA.GetComponent<Toggle>().isOn == true)// && string.IsNullOrEmpty(IncomingAPIINput.text))
        {
            SaveCommDetails.enabled = false;
            //SaveCommDetails.GetComponent<Image>().color = Color.red;
            IncomingAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
            IncomingAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Required; Please enter Inbound API link here";

            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.black;
            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Not Applicable";

        }

        else
        {
            SaveCommDetails.enabled = true;
            //SaveCommDetails.GetComponent<Image>().color = Color.white;
            IncomingAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.black;
            IncomingAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Inbound API link here";

            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.black;
            OutboundAPIINput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Outbound API link here";

            UpdateConnectionDetails();
            
        }

        SaveCommDetails.enabled = true;
        IncomingAPIINput.text = "";
        OutboundAPIINput.text = "";

        //mLocationDb.addData(new ConfigEntity(id, machineuniqueid, ServerIP, ServerPort, timestamp));
        //mLocationDb.addData(new ConfigEntity(0, "machineid-2", "https://", API_Ip_Address.ToString(), API_Port.ToString(), "https://nauserver.com:8888", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "Terms and conditions", "Name on Card", "Card Number", "Card CVV", "Card Expiry", "PaymentHistory", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
    }
    public void UpdateConnectionDetails()
    {
        table_Config mLocationDb = new table_Config();
        using var connection = mLocationDb.getConnection();
        mLocationDb.Updatedata(" Https = 'httpdropdown.ToString()HiddenonUI', ServerIP = 'API_Ip_Address.text.ToString()HiddenonUI', ServerPort = 'API_Port.text.ToString()HiddenonUI', IncomingAPI = '" + IncomingAPIINput.text.ToString() + "', OutGoingAPI = '" + OutboundAPIINput.text.ToString() + "'");
        connection.Close();
        GetConfigData();
        Debug.Log("UPDATED CONNECTION DETAILS");
    }
    public void IncomingTestConnectionOnClick()
    {
        StartCoroutine(IncomingTestAPIData());
    }

    IEnumerator IncomingTestAPIData()
    {
        IncomingAPITest.text = "";
        IncomingResult = false;
        if (string.IsNullOrEmpty(IncomingAPI.text))
        {
            IncomingAPITest.text = "There is no Incoming API link entered or saved.";
            IncomingAPITest.GetComponent<Text>().color = Color.red;
        }

        else
        {
            // create the web request and download handlerhttp://59.144.167.27:1020/api/unity/GetInventory
            UnityWebRequest webReq = new UnityWebRequest();
            webReq.downloadHandler = new DownloadHandlerBuffer();

            // build the url and query
            webReq.url = string.Format("{0}", IncomingAPI.text.ToString()); //"Example or can be webaddress http://59.144.167.27:1020/api/unity/GetInventory");IncomingAPI.text

            // send the web request and wait for a returning result
            yield return webReq.SendWebRequest();

            // convert the byte array to a string
            if (webReq.downloadHandler.isDone)
            {
                //string rawJson = Encoding.Default.GetString(webReq.downloadHandler.data);

                Debug.Log("The result of the Incoming API was a Success !!");
                IncomingAPITest.text = "Incoming API -->> connection was successful.";
                IncomingAPITest.GetComponent<Text>().color = Color.green;
                IncomingResult = true;
               Console.WriteLine("The result of the Incoming API was a Success !!");
                
            }
            else
            {
                Debug.Log("UnSuccessful");
                IncomingAPITest.text = "Incoming API -->> connection was un-successful. Please check the API link and try again !! ";
                IncomingAPITest.GetComponent<Text>().color = Color.red;
                IncomingResult = false;
            }

        }

    }



    public void OutgoingTestConnectionOnClick()
    {
        StartCoroutine(OutAPIData());
    }

    IEnumerator OutAPIData()
    {
        OutgoingAPITest.text = "";
        OutgoingResult = false;
        if (Out_API_NA.GetComponent<Toggle>().isOn == false && string.IsNullOrEmpty(OutgoingAPI.text))
        {
            OutgoingAPITest.text = "There is no Outgoing API link entered or saved.";
            OutgoingAPITest.GetComponent<Text>().color = Color.red;
            OutgoingResult = false;
        }

        else if (Out_API_NA.GetComponent<Toggle>().isOn == true)
        {
            OutgoingAPITest.text = "Not Applicable as you have marked system as Standalone.";
            OutgoingAPITest.GetComponent<Text>().color = Color.green;
            OutgoingResult = false;
        }

        else
        {
            
            //OutgoingAPICheck.text = "Api Start 1";
            UnityWebRequest webReq = new UnityWebRequest(OutgoingAPI.text.ToString(), "POST");//Example or can be webaddress"http://59.144.167.27:1035/api/unity/post");OutgoingAPI.text
            //OutgoingAPICheck.text = "Api Start 2";

            webReq.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //OutgoingAPICheck.text = "Api Start 3";

            //string bodyJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new List<Dictionary<string, object>>()); //-- NEWTONSOFT WAS NOT WORKING IN ANDROID.
            string bodyJsonString ="[]";
            //Had to 
            //List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();

            //string bodyJsonString = JsonUtility.ToJson(values.ToString());
            //string bodyJsonString = JsonUtility.ToJson(new SynchEntity(0,"","","",0,"","",""));
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            //byte[] bodyRaw = Encoding.UTF8.GetBytes("[]");
            //OutgoingAPICheck.text = "Api Start 4";
            webReq.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            //OutgoingAPICheck.text = "Api Start 5";
            webReq.SetRequestHeader("Content-Type", "application/json");
            //OutgoingAPICheck.text = "Api Called";
            yield return webReq.SendWebRequest();
            //OutgoingAPICheck.text = "Api Call End";

            if (webReq.downloadHandler.isDone)
            {
                //string rawJson = Encoding.Default.GetString(webReq.downloadHandler.data);
                Debug.Log("The result of the outbpound API was a Success !!");
                OutgoingAPITest.text = "Outgoing API -->> connection was successful.";
                OutgoingAPITest.GetComponent<Text>().color = Color.green;
                OutgoingResult = true;
            }
            else
            {
                Debug.Log("UnSuccessful");
                OutgoingAPITest.text = "Outgoing API -->> connection was un-successful. Please check the API link and try again !! ";
                OutgoingAPITest.GetComponent<Text>().color = Color.red;
                OutgoingResult = false;
            }
            if (webReq.downloadHandler.data!=null)
            {
                string rawJson = Encoding.Default.GetString(webReq.downloadHandler.data);
                Debug.Log("OUTBOUND DATA CHECK " + rawJson);
            }
           
            //OutgoingAPICheck.text = "OUTBOUND DATA CHECK " + rawJson.ToString();

        }

    }

    public void ProceedtoMainMenu()
    {
        if (Out_API_NA.GetComponent<Toggle>().isOn == false)
        {
            OutgoingTestConnectionOnClick();
            IncomingTestConnectionOnClick();
            //Proceed.enabled = false;
            StartCoroutine(Aftertestresults());
        }
        else
        {
            IncomingTestConnectionOnClick();
            StartCoroutine(Aftertestresults());
            //Proceed.enabled = false;
        }

        
        
    }

    IEnumerator Aftertestresults()

    {
        yield return new WaitForSeconds(4.0f);

        if (Out_API_NA.GetComponent<Toggle>().isOn == false && OutgoingResult == true && IncomingResult == true)
        {
            Debug.Log("PROCEED AHEAD WITH BOTH IN / OUT True");
            proceedwarning.text = "";

            ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
            
            //CommsConfigPanel.SetActive(false);
            //Mainmenupanel.SetActive(true);
            //inventory.SetActive(true);
            //synch.SetActive(true);
            //Proceed.enabled = true;
        }
        else if (Out_API_NA.GetComponent<Toggle>().isOn == true && IncomingResult == true)
        {
            Debug.Log("PROCEED AHEAD WITH INCOMING True");
            proceedwarning.text = "";
            ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.

            //CommsConfigPanel.SetActive(false);

            //Mainmenupanel.SetActive(true);
            //inventory.SetActive(true);
            //synch.SetActive(true);
            //Proceed.enabled = true;
        }
        else if (Out_API_NA.GetComponent<Toggle>().isOn == false && OutgoingResult == false && IncomingResult == false)
        {

            proceedwarning.text = "API -->> Detected Un-successful connection/s. Please check and try again !!";
            proceedwarning.GetComponent<TextMeshProUGUI>().color = Color.red;

            ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
            //CommsConfigPanel.SetActive(true);

            //Mainmenupanel.SetActive(false);
           // Proceed.enabled = true;
        }
        else if (Out_API_NA.GetComponent<Toggle>().isOn == true && IncomingResult == false)
        {

            proceedwarning.text = "API -->> Detected Un-successful connection/s. Please check and try again !!";
            proceedwarning.GetComponent<TextMeshProUGUI>().color = Color.red;
            ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
            //CommsConfigPanel.SetActive(true);

            //Mainmenupanel.SetActive(false);
            // Proceed.enabled = true;
        }
    }

    public void OnClickSynch()
    {
        //06th June 2024, Mohit Commented these TWO lines since CANVAS Manager already managing using ALPHA 0 and 1.

        //SynchPanel.SetActive(true); 
        //CommsConfigPanel.SetActive(false);


        //Mainmenupanel.SetActive(false);
        //InventoryMainmenu.SetActive(false);
        //InventorySubmenu.SetActive(false);
        //BacktoMainMenu.SetActive(true);

        Synch.FetchUnprocessed();
    }


    public void Logout()
    {
        //LogoutConfirm.SetActive(true);
        //SceneManager.LoadScene("AIR_TOUCH_WELCOME");
    }


    public void YesLogout()
    {
        //LogoutConfirm.SetActive(false);
        SceneManager.LoadScene("AIR_TOUCH_WELCOME");
    }

    public void NoDontLogout()
    {
        //LogoutConfirm.SetActive(false);
    }


}
