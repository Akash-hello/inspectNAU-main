using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DataBank;
using System;

//public class AddItem : MonoBehaviour
//{
//    public GameObject[] ItemDetail;
//    string input;
//    public string inputFieldContent;
//    public int newitemid;
//    public int existingitemId;
//    public string itemcode;
//    public string itemname;
//    public string partnumber;
//    public string DrawingNumber;
//    public string makername;
//    public string makercode;
//    public string makerref;
//    public int currentstock;
//    public string unit;
//    table_Inventory mLocationDb;
//    table_Synch synchtable;
//    table_Config tblconfig;
//    InventoryDataAndQuery inventorymanager;
//    public List<InventoryEntity> searchresultsforhere;
//    public String outputofsearchresult;
//    //public SynchDataManager synchroniserdata;
//    private const String TABLE_NAME = "Inventory";
//    public string Flag;
//    public GameObject PopUpMsg;

//    public GameObject UpdatedMsg;
//    public int SearchItemID;
//    public int Idincrement;
//    public int id;
//    public String machineid;
    
//    void Start()
//    {
//        mLocationDb = new table_Inventory();
//        mLocationDb.additem = this;
//        synchtable = new table_Synch();
//        synchtable.additem = this;
//        inventorymanager = new InventoryDataAndQuery();
//        //synchroniserdata = new SynchDataManager();
//        //synchroniserdata.additem = this;
//        PopUpMsg.SetActive(false);
//        UpdatedMsg.SetActive(false);
//        GetMachineId();
       
//    }

//    public void GetMachineId()
//    {
//            tblconfig = new table_Config();
//            System.Data.IDataReader reader = tblconfig.getDataByString();

//            //int fieldCount = reader.FieldCount;
//            List<ConfigEntity> myList = new List<ConfigEntity>();
//            while (reader.Read())
//            {
//                ConfigEntity entity = new ConfigEntity(int.Parse(reader[0].ToString()),
//            reader[1].ToString(),
//            reader[2].ToString(),
//            reader[3].ToString(),
//            reader[4].ToString(),
//            reader[5].ToString(),
//            reader[6].ToString(),
//            reader[7].ToString(),
//            reader[8].ToString(),
//            reader[9].ToString(),
//            reader[10].ToString(),
//            reader[11].ToString(),
//            reader[12].ToString(),
//            reader[13].ToString(),
//            reader[14].ToString(),
//            reader[15].ToString());

//                Debug.Log("Machine ID: " + entity._machineid);

//                machineid = entity._machineid;
//                //Dbhttps = entity._https;
//                //Dbserverip = entity._ServerIP;
//                //DbServerPort = entity._ServerPort;
               
//                myList.Add(entity);

//                var output = JsonUtility.ToJson(entity, true);
//                Debug.Log("FROM CONFIG MANAGER: " + output);
//            }
//        tblconfig.close();
//        }
   
//    public void ConfirmClick()
//    {
       
//        bool validForm = true;
//        foreach (GameObject detail in ItemDetail)
//        {
//            inputFieldContent = detail.GetComponent<TMP_InputField>().text;
//            if (string.IsNullOrEmpty(inputFieldContent))
//            {
//                detail.GetComponent<TMP_InputField>().placeholder.color = Color.red;
//                detail.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Required Field !";
//                //detail.GetComponent<TMP_InputField>().text = "Required Field.";
//                validForm = false;
//            }
            
//        }
//        if (validForm)
//        {
//            DataFmInputs();
//        }
//    }


//    public void DataFmInputs()
//    {
//        //mLocationDb.getLatestID();
//        mLocationDb.CheckifItemExists(ItemDetail[0].GetComponent<TMP_InputField>().text.ToString().Trim(), "AddingNew");
//    }

//    public void Queryresults()
//    {
//        mLocationDb.additem = this;
//        if (searchresultsforhere.Count == 0)
//        {
            
//            Debug.Log("REACHED BACK TO ADD ITEMS..");
//            itemcode = ItemDetail[0].GetComponent<TMP_InputField>().text.ToString().Trim();
//            itemname = ItemDetail[1].GetComponent<TMP_InputField>().text.ToString().Trim();
//            partnumber = ItemDetail[2].GetComponent<TMP_InputField>().text.ToString().Trim();
//            DrawingNumber = ItemDetail[3].GetComponent<TMP_InputField>().text.ToString().Trim();
//            makername = ItemDetail[4].GetComponent<TMP_InputField>().text.ToString().Trim();
//            makercode = ItemDetail[5].GetComponent<TMP_InputField>().text.ToString().Trim();
//            makerref = ItemDetail[6].GetComponent<TMP_InputField>().text.ToString().Trim();
//            currentstock = int.Parse(ItemDetail[7].GetComponent<TMP_InputField>().text.ToString().Trim());
//            unit = ItemDetail[8].GetComponent<TMP_InputField>().text.ToString().Trim();

//            mLocationDb.addData(new InventoryEntity(newitemid, itemcode,itemname,makername,makercode,makerref,partnumber,DrawingNumber,"","","",currentstock,unit, DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
//            Flag = "New_Item";
//            StartCoroutine(GetIdsForSynch());
//            Debug.Log("New Item Going in " + itemcode + "-" + itemname + partnumber + "-" + DrawingNumber + "-" + makername + "-" + makercode + "-" + makerref + "-" + currentstock + unit);
//        }
//        else if (searchresultsforhere.Count > 0)
//        {
//            itemcode = ItemDetail[0].GetComponent<TMP_InputField>().text.ToString().Trim();
//            itemname = ItemDetail[1].GetComponent<TMP_InputField>().text.ToString().Trim();
//            partnumber = ItemDetail[2].GetComponent<TMP_InputField>().text.ToString().Trim();
//            DrawingNumber = ItemDetail[3].GetComponent<TMP_InputField>().text.ToString().Trim();
//            makername = ItemDetail[4].GetComponent<TMP_InputField>().text.ToString().Trim();
//            makercode = ItemDetail[5].GetComponent<TMP_InputField>().text.ToString().Trim();
//            makerref = ItemDetail[6].GetComponent<TMP_InputField>().text.ToString().Trim();
//            currentstock = int.Parse(ItemDetail[7].GetComponent<TMP_InputField>().text.ToString().Trim());
//            unit = ItemDetail[8].GetComponent<TMP_InputField>().text.ToString().Trim();
//            //Debug.Log("This item already exists in the Database - " + itemcode + ". Do you want me to overwrite the existing item.");
            
//            PopUpMsg.SetActive(true);
//        }
//    }

//    public void YesClicked()
//    {
//        mLocationDb.Updatedata(" ItemCode = '" + itemcode + "', ItemName = '" + itemname + "', EquipPartNum = '" + partnumber + "', EqupDrawNum = '" + DrawingNumber + "', MakerName = '" + makername
//         + "', MakerCode = '" + makercode + "', MakerRef = '" + makerref + "', CurrentROB = '" + currentstock + "', Unit = '" + unit + "', EquipIntNum = '-" + "', PrefVendCode = '-" + "', DefaultLocation = '-" + "' where ItemCode = ' " + itemcode.ToString() + " ' and Id = '" + existingitemId+ "'");// + " and Id = '" + SearchItemID+"'");
//        Debug.Log("Yes Clicked");
//        Flag = "Update_Item";
//        PopUpMsg.SetActive(false);
//        mLocationDb.getNewOrUpdatedItemId(existingitemId);

//    }

//    public void NoClicked()
//    {
//        PopUpMsg.SetActive(false);
//        ItemDetail[0].GetComponent<TMP_InputField>().text= "";
//        Debug.Log("NO Clicked");
//    }

//    IEnumerator GetIdsForSynch()
//    {
//        yield return new WaitForSeconds(1.0f);
//        mLocationDb.getNewOrUpdatedItemId(newitemid);
//    }


//    //********* IMPORTANT FOR SYNCH, commented by Mohit 21st Feb 2024, procedure for Synchronisation data sending.
//    public void Synchronise()
//    {
//        System.Data.IDataReader reader = synchtable.getLatestID();

//        int fieldCount = reader.FieldCount;
//        List<SynchEntity> myList = new List<SynchEntity>();
//        while (reader.Read())
//        {
//            SynchEntity entity = new SynchEntity(int.Parse(reader[0].ToString()),
//                                    reader[1].ToString(),
//                                    reader[2].ToString(),
//                                    reader[3].ToString(),
//                                    int.Parse(reader[4].ToString()),
//                                    reader[5].ToString(),
//                                    reader[6].ToString(),
//                                    reader[7].ToString(),
//                                    reader[8].ToString());


//            Idincrement = entity._id + 1;
//            myList.Add(entity);
//            var output = JsonUtility.ToJson(entity, true);
//            Debug.Log(output);
//        }

//        if (Idincrement == 0)
//        {
//            id = 1;
//        }
//        else
//        {
//            id = Idincrement;
//        }
        
//        Debug.Log("LATEST ID IN TO INSERT IS : " + id);
       
//        synchtable.addData(new SynchEntity(id,machineid, mLocationDb.outputofsearchresult, itemcode.Trim(), currentstock, "N", TABLE_NAME.ToString(), Flag, DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
//        synchtable.close();
//        UpdatedMsg.SetActive(true);
//        StartCoroutine(UpdatedMsgClose());
//    }

//    IEnumerator UpdatedMsgClose()
//    {

//        yield return new WaitForSeconds(0.5f);

//        UpdatedMsg.SetActive(false);
//        CloseButtonClick();
//    }
//    public void CloseButtonClick()
//    {
//        //inventorymanager.listalldata();
//        StartCoroutine(inventorymanager.listalldata());
//    }
//}
                  
                  
                  
                  
                  
                  
                  