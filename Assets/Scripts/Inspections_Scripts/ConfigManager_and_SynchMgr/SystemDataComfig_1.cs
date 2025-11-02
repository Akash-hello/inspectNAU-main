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
using System.Threading;
using SQLite4Unity3d;
using Mono.Data.Sqlite;

public class SystemDataComfig_1 : MonoBehaviour
{
    public Text deviceID;
    public Text machineIDfromDB;
    public Text IncomingAPI;
    public Text OutgoingAPI;
    public Toggle Out_API_NA;
    public TextMeshProUGUI proceedwarning;
    //table_Config mLocationDb;
    //public GameObject inventory;
    //public GameObject synch;

    ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
    //public GameObject CommsConfigPanel;

    ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
    //public GameObject Mainmenupanel;

    public APIInputsAndTests ConfigurationsSettings;
    public string DbMachineID;
    public string DbIncomingAPI;
    public string DbOutboundAPI;
    public string DbOrionConnect;
    public string DbLastLoginDatetime;
    public string DbTimeStamp;
    public Text StartStepsCheck;

   

    // Start is called before the first frame update
    void Start()
    {
        //inventorymanager.GetComponent<InventoryDataAndQuery>().AddData();
        GetConfigData();
        StartStepsCheck.text = "Code line 42 - Start of Config.";
       
    }
    public void ExecuteQueryOutsideHelper(string query)
    {
        // Declare and open the SQLite connection
        string db_connection_string = "URI=file:" + Application.persistentDataPath + "/Orion_DB_1";
        using (var connection = new SqliteConnection(db_connection_string))
        {
            connection.Open();

            // Perform operations within a using block to ensure resources are properly disposed
            using (var command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery(); // Execute the query
                    Debug.Log("Query executed successfully outside helper.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error executing query: " + ex.Message);
                }
                // No need to explicitly close the connection here, it will be disposed of by the using block
            }
        } // The connection and command objects are disposed of here
    }

    public void GetConfigData()
    {
        
        //string db_connection_string = "URI=file:" + Application.persistentDataPath + "/Orion_DB_1";

        //using (var connection = new SqliteConnection(db_connection_string))
        //{
        //    connection.Open();
            //Fetch All Data
            table_Config mLocationDb = new table_Config();
        using var connection = mLocationDb.getConnection();
        using System.Data.IDataReader reader = mLocationDb.getDataByString();

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
                StartStepsCheck.text = "Code line 89 - Get Config Data" + output;

            
            //reader.Close();
            //mLocationDb.close();
            
            //connection.Close();
        }

        //reader.Dispose();

        IncomingAPI.text = DbIncomingAPI.ToString().Trim();
        OutgoingAPI.text = DbOutboundAPI.ToString().Trim();
        machineIDfromDB.text = DbMachineID.ToString().Trim();


        Debug.Log("Incoming API = " + IncomingAPI.text.ToString() + "Outgoing API " + OutgoingAPI.text.ToString());

        if (string.IsNullOrEmpty(machineIDfromDB.text))//IF There is no Machine ID, that means first time or for someother reason.
        {
            InitiliaseDropTables();
            StartStepsCheck.text = "Code line 101 - Calling InitiliaseDropTables as there is no Machine ID, that means first time or for someother reason.";
        }
       
        if (Out_API_NA.isOn == false && (string.IsNullOrEmpty(IncomingAPI.text) || string.IsNullOrEmpty(OutgoingAPI.text)))
        {
            proceedwarning.text = "API -->> Detected Un-successful connection/s. Please check and try again !!";
            ConfigurationsSettings.OnClickOpenConfigPanel();
            Debug.Log("ACTIVATE CONFIG PANEL & KEEP MAIN MENU OFF AS BOTH ARE EMPTY");
            StartStepsCheck.text = "Code line 109 - ACTIVATE CONFIG PANEL & KEEP MAIN MENU OFF AS BOTH ARE EMPTY";
        }

        else if (Out_API_NA.isOn == true && string.IsNullOrEmpty(IncomingAPI.text))
        {
            Debug.Log("ACTIVATE CONFIG PANEL & KEEP MAIN MENU OFF");
            ConfigurationsSettings.OnClickOpenConfigPanel();
            StartStepsCheck.text = "Code line 116 - ACTIVATE CONFIG PANEL & KEEP MAIN MENU OFF";
        }
        else
        {
            Debug.Log("ACTIVATE MAIN MENU and stop config panel");
            //inventory.SetActive(true);
            //synch.SetActive(true);
            ConfigurationsSettings.OnClickOpenConfigPanel();
            StartCoroutine(ActivateMainMenu());
            StartStepsCheck.text = "Code line 125 - ACTIVATE MAIN MENU and stop config panel";
        }

    }

    IEnumerator ActivateMainMenu()

    {
        yield return new WaitForSeconds(10f);
        
        this.enabled = false;

        ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
        //Mainmenupanel.SetActive(true);

        ////06th June 2024, Mohit Commented this line below since CANVAS Manager already managing using ALPHA 0 and 1.
        //CommsConfigPanel.SetActive(false);
    }

        public void InitiliaseDropTables()
    {
        //table_Inventory mLocationDb2 = new table_Inventory();
        //mLocationDb2.DropTable();

        table_Synch mLocationDb3 = new table_Synch();
        using var connection = mLocationDb3.getConnection();
        mLocationDb3.DropTable();
        //mLocationDb3.close();

        table_Config mLocationDb = new table_Config();
        using var connection1 = mLocationDb.getConnection();
        mLocationDb.DropTable();
        //mLocationDb.close();

        IncomingAPI.text = "";
        OutgoingAPI.text = "";

   InitiliaseCreateTables();
    }

    public void ExecuteWithRetry(Action action, int maxRetries = 3, int delayMilliseconds = 1000)
    {
        int retries = 0;
        while (true)
        {
            try
            {
                action();
                break; // Exit the loop if the action succeeded
            }
            catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
            {
                if (++retries == maxRetries)
                    throw; // Rethrow the exception if maximum retries reached
                Thread.Sleep(delayMilliseconds); // Wait before retrying
            }
        }
    }


    public void InitiliaseCreateTables()
    {

        ExecuteWithRetry(() =>
        {
            //Get the system ID and input into the configuration table
        deviceID.text = SystemInfo.deviceUniqueIdentifier.ToString();

            //deviceID.text = "TestSTringforDeviceID";
            table_Config mLocationDb = new table_Config();
            using var connection = mLocationDb.getConnection();

            mLocationDb.addData(new ConfigEntity(1, deviceID.text.ToString(), "", "", "", "", "", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "", "", "", "", "", "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
        //mLocationDb.close();

        table_Synch mlocationDbsynch = new table_Synch();
            using var connection1 = mlocationDbsynch.getConnection();

            mlocationDbsynch.addData(new SynchEntity(1, deviceID.text.ToString(), "ResetEntry", "ResetEntry", 0, "N", "ResetEntry", "Reset_all","ResetEntryNo.zip", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
        //table_Inventory mLocationDb3 = new table_Inventory();
        IncomingAPI.text = "";
        OutgoingAPI.text = "";
        //inventory.SetActive(true);
        //synch.SetActive(true);
        //InitialisationPanel.SetActive(false);
        //Mainmenupanel.SetActive(false);
        //CommsConfigPanel.SetActive(true);
        ConfigurationsSettings.OnClickOpenConfigPanel();
        
        //mlocationDbsynch.close();
        });
    }
    
}
