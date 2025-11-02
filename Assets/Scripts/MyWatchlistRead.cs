using DataBank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;


public class MyWatchlistRead : MonoBehaviour
{
    table_MyStocks mlocationDb;

    public List<MyStckWatchEntity> searchresultsforhere;
    public List<MyStckWatchEntity> myList1;
    public int totatlrecords;
    public int maxpages;
    public RectTransform ParentPanel;
    //public GameObject Stockname;
    //public GameObject StockPrefab;

    public void GetMyStocklist()

    {
        table_MyStocks mlocationDb = new table_MyStocks();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getDataByString1("fetchall");
    }

    public void queryresults()
    {
        totatlrecords = searchresultsforhere.Count;
        maxpages = (totatlrecords / 9) + 1;

        foreach (var x in searchresultsforhere)
        {
            Debug.Log(x.ToString());
        }
        //Debug.Log("The list of stocks in the DB are; " + searchresultsforhere.ToString());

        //for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }

        ////BotForNoItemsFound.SetActive(false);
        //GameObject InventoryListItems = Instantiate(Resources.Load("Prefabs/InventoryUpdateImage", typeof(GameObject))) as GameObject;

        GameObject Stockname = Instantiate(Resources.Load("Prefabs/FInal_Prefabs/MyStockDetailsGmObj", typeof(GameObject))) as GameObject;
        Stockname.transform.SetParent(ParentPanel, false);

        //foreach (var result in searchresultsforhere)
        //{

        //    InventoryItems = Instantiate(InventoryListItems) as GameObject;
        //    InventoryItems.transform.SetParent(ParentPanel, false);
        //    InventoryItems.transform.Find("ItemCode").GetComponentInChildren<Text>().text = "Item Code: " + result._itemcode;
        //    InventoryItems.transform.Find("QRCode").GetComponentInChildren<Text>().text = result._itemcode;
        //    InventoryItems.transform.Find("ItemName").GetComponentInChildren<Text>().text = result._itemname;
        //    InventoryItems.transform.Find("MakerName").GetComponentInChildren<Text>().text = "Maker Name: " + result._makername;
        //    InventoryItems.transform.Find("MakerCode").GetComponentInChildren<Text>().text = "Maker Code: " + result._makercode;
        //    InventoryItems.transform.Find("MakerRef").GetComponentInChildren<Text>().text = "Maker's Ref.: " + result._makersref;
        //    InventoryItems.transform.Find("EquipPartNumber").GetComponentInChildren<Text>().text = "Part Number: " + result._equippartnumber;
        //    InventoryItems.transform.Find("DrawingNumber").GetComponentInChildren<Text>().text = "Drawing Number: " + result._equipdrawingnumber;
        //    InventoryItems.transform.Find("Location").GetComponentInChildren<Text>().text = "Location: " + result._defaultlocation;
        //    InventoryItems.transform.Find("TotalStock").GetComponentInChildren<Text>().text = result._currentrob.ToString();
        //    InventoryItems.transform.Find("Unit").GetComponentInChildren<Text>().text = "Unit: " + result._unit;
        //    InventoryItems.transform.Find("ItemID").GetComponentInChildren<Text>().text = "Database ID: " + result._id;

        //    Button plusbutton = InventoryItems.transform.Find("ButtonBackgroundImg/Add").GetComponentInChildren<Button>();
        //    Button minusbutton = InventoryItems.transform.Find("ButtonBackgroundImg/Reduce").GetComponentInChildren<Button>();

        //    confirmbutton = InventoryItems.transform.Find("GreeButtonBckgrnd/ConfirmButton").GetComponentInChildren<Button>();
        //    GreeButtonBckgrnd = InventoryItems.transform.Find("GreeButtonBckgrnd").GetComponentInChildren<UnityEngine.UI.Image>();

        //    GreeButtonBckgrnd.GetComponent<UnityEngine.UI.Image>().color = Color.grey;
        //    cancelbutton = InventoryItems.transform.Find("CancelButton").GetComponentInChildren<Button>();

        //    selecteditem = InventoryItems.gameObject;
        //    selecteditemid = result._id;
        //    selecteditemname = result._itemname.ToString();
        //    selecteditemcode = result._itemcode.ToString();
        //    selecteditemlocation = result._defaultlocation.ToString();
        //    selectedcurrentstock = result._currentrob;
        //    selectedeffectstock = int.Parse(selecteditem.transform.Find("TotalStock").GetComponentInChildren<Text>().text);
        //    //searcheditemdetails = new[] { result._id + ", " + result._itemname + result._itemcode + ", " + quantity + ", " + result._itemcode + ", " };
        //    selecteditemquantity = int.Parse(selecteditem.transform.Find("ButtonBackgroundImg/Quantity").GetComponentInChildren<Text>().text);
        //    Flag = "Update_Qty";
        //    QRCode = InventoryItems.transform.Find("QRCodeRaw").GetComponentInChildren<RawImage>();
        //    listofImage.Add(QRCode);
        //    plusbutton.onClick.AddListener(AddOnClick);
        //    reducebutton = minusbutton;
        //    reducebutton.onClick.AddListener(ReduceOnClick);
        //    confirmbutton.onClick.AddListener(ConfirmClick);
        //    cancelbutton.onClick.AddListener(CancelClick);
        //    PrintButton = InventoryItems.transform.Find("PrintButton").GetComponentInChildren<Button>();
        //    PrintButton.onClick.AddListener(PrintThis);
        //}

        //printlist();
    }


}

//THIS IS THE CLASS SAME AS OTHER Stocks_Accordion_Call_API, except for its is called from table_MyStocks for getting the data.

