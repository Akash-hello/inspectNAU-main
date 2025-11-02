using DataBank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeGamObjectPosition : MonoBehaviour
{
    //Use this to change the hierarchy of the GameObject siblings
    public GameObject maincanvas;
    public GameObject marketwatchcanvas;
    public GameObject mywatchlistcanvas;
    public GameObject BusinessNewsscrollview;
    public GameObject MenuPanel;
    public GameObject NewsPanel;
    public Text mainheader;
    public Text exchangename;
    public GameObject ScrollUp;
    public bool loadmarketwatch;
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    public GameObject marketpanel;
    public GameObject simulatepopup;

    public GameObject totalstocks;
    public TextMeshProUGUI Totalstocks;
    public GameObject totalstockslabel;

    public NewsRequest newsrequest; // THIS IS FITTED FOR USER TO SEE NEWS AS SOON AS the APP starts, so he doesn't need to wait for stocks to load and get bored.
    

    float time;

    public void MarketWatchBtnClick()
    {
        exchangename.enabled = false;
        mainheader.text = "Market Watch List";
        BusinessNewsscrollview.transform.SetSiblingIndex(5);
        BusinessNewsscrollview.GetComponentInChildren<CanvasGroup>().alpha = 0;
        marketwatchcanvas.transform.SetSiblingIndex(8);
        marketwatchcanvas.GetComponentInChildren<CanvasGroup>().alpha = 1;
        maincanvas.transform.SetSiblingIndex(7);
        maincanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        mywatchlistcanvas.transform.SetSiblingIndex(6);
        mywatchlistcanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        Totalstocks.fontSize = 1;
        loadmarketwatch = false;
    }

    public void loadingfromexchangescrpt()
    {
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        if (String.IsNullOrEmpty(mlocationDb.MarketSet) || mlocationDb.MarketSet == "")
        {
        loadmarketwatch = false;
        totalstocks.SetActive(true);
            Totalstocks.fontSize = 42;
        totalstockslabel.SetActive(true);
            ScrollUp.SetActive(true);
            MarketBtnClick();
        }
         else
        {
        loadmarketwatch = true;
            //totalstocks.SetActive(false);
            Totalstocks.fontSize = 1;
            totalstockslabel.SetActive(false);
            ScrollUp.SetActive(false);
            exchangename.enabled = false;
            mainheader.text = "Market Watch List";
            MarketBtnClick();
        }
        connection.Close();
    }

    public void MarketBtnClick()
    {
        //exchangesectorscrpt = new Exchange_Sector_Call_API();
        
        BusinessNewsscrollview.transform.SetSiblingIndex(5);
        BusinessNewsscrollview.GetComponentInChildren<CanvasGroup>().alpha = 0;
        MenuPanel.SetActive(!MenuPanel.activeSelf);

        if (loadmarketwatch == false)
        {
            mainheader.text = "";
            exchangename.enabled = true;
            Totalstocks.fontSize = 42;
            marketwatchcanvas.transform.SetSiblingIndex(7);
            marketwatchcanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
            maincanvas.transform.SetSiblingIndex(8);
            maincanvas.GetComponentInChildren<CanvasGroup>().alpha = 1;
        }
        else if (loadmarketwatch == true)
        {
            marketwatchcanvas.transform.SetSiblingIndex(8);
            marketwatchcanvas.GetComponentInChildren<CanvasGroup>().alpha = 1;
            maincanvas.transform.SetSiblingIndex(7);
            maincanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
            Totalstocks.fontSize = 1;
            errorpopup.SetActive(true);
            PopUpMsg.text = "Thank you for your patience while we load our favorites for the day.";
            
            marketpanel.SetActive(false);
            //BusinessNewsBtnClick(); // LOAD NEWS BY DEFAULT.
        }
        StartCoroutine(HidePopUp());
        mywatchlistcanvas.transform.SetSiblingIndex(6);
        mywatchlistcanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        loadmarketwatch = false;
    }

    public void MyWatchBtnClick()
    {
        exchangename.enabled = false;
        mainheader.text = "My Watch List";
        BusinessNewsscrollview.transform.SetSiblingIndex(5);
        BusinessNewsscrollview.GetComponentInChildren<CanvasGroup>().alpha = 0;
        marketwatchcanvas.transform.SetSiblingIndex(7);
        marketwatchcanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        maincanvas.transform.SetSiblingIndex(6);
        maincanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        mywatchlistcanvas.transform.SetSiblingIndex(8);
        mywatchlistcanvas.GetComponentInChildren<CanvasGroup>().alpha = 1;
        Totalstocks.fontSize = 1;
    }
    public void BusinessNewsBtnClick()
    {
        exchangename.enabled = false;
        mainheader.text = "Business News";
        NewsPanel.SetActive(!NewsPanel.activeSelf);
        BusinessNewsscrollview.transform.SetSiblingIndex(8);
        BusinessNewsscrollview.GetComponentInChildren<CanvasGroup>().alpha = 1;
        marketwatchcanvas.transform.SetSiblingIndex(7);
        marketwatchcanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        maincanvas.transform.SetSiblingIndex(6);
        maincanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        mywatchlistcanvas.transform.SetSiblingIndex(5);
        mywatchlistcanvas.GetComponentInChildren<CanvasGroup>().alpha = 0;
        Totalstocks.fontSize = 1;
    }

    IEnumerator HidePopUp()
    {
        time = 5.0f;
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        simulatepopup.SetActive(false);
        PopUpMsg.text = "";
    }
}
