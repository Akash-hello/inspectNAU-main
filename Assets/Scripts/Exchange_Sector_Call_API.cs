using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using DataBank;
using UnityEngine.SceneManagement;
using System.Data;
using System;

public class Exchange_Sector_Call_API : MonoBehaviour
{
    private string url;
    public string Sector = "";
    public string SectorsinExch = "";
    public Text mainheader;

    Stocks_Accordion_Call_API stockscalling;
    public ChangeGamObjectPosition CanvasPositionManager;
    public InputField SearchSym; //SEARCH FIELD INPUT TEXT
    public Button Searchbutton; //TAG = "stocksearchbtn"

    public List<string> Sectors = new List<string>();
    public string IndustryName;
    public List<string> ExchangeStocks;
    public Sector_Stocks_Call_API SectorAPI;
    public Button NSE;
    public Button BSE;
    //Calling Stocks in the sector
    int buttonid;
    private string url1;
    public int urlfeed;
    public string StockForSector = "";

    //public string SectorsinExchs = "";
    //public List<string> SectorInExchg;
    public RectTransform ParentPanel;
    RectTransform ParentPanel_1;
    public GameObject industryprefab;
    public GameObject industryaccordion;

    public List<string> StockSymbolSector = new List<string>();
    public string StockSymbol = "";

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;
    public TextMeshProUGUI fromdate;
    public TextMeshProUGUI dateforshowing;
    private DateTime simulationdateentered;

    GameObject[] industriesprefabs;
    Button Stocksrefreshbtn;

    public string outputofsearchresult;
    public string searchresults;

    string[] Stockexchanges;
    public RectTransform ExchangeParentPanel;
    public Button exchangeprefab;
    public Button exchange;
    public Button ExchgButton;
    public string exchangename;
    public string exhchangesplit = "";
    public string countrysplit = "";

    public GameObject MarketMenu;

    public GameObject simulatepopup;
    public TextMeshProUGUI simulatepopupfromdate;
    
    public TextMeshProUGUI simulatepopupresultdate;
    public TMP_InputField Subtractsdaysinput;
    private int subtractdays = 0;
    //private string dateforfetchin;
    public TextMeshProUGUI counterforttl;
        

    public void Awake()
    {
        Application.runInBackground = true;
        simulationdateentered = DateTime.Now;
        fromdate.text = simulationdateentered.ToString("yyyy-MM-dd");
        dateforshowing.text = simulationdateentered.ToString("ddd, dd MMM yyy");
        
        exhchangesplit = "";
        countrysplit = "";
        //CanvasPositionManager = new ChangeGamObjectPosition();
        stockscalling = new Stocks_Accordion_Call_API();
        ParentPanel_1 = GameObject.FindGameObjectWithTag("AutowatchList").GetComponent<RectTransform>();
        GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";
        //GameObject.FindGameObjectWithTag("counterttlstocksexchg").GetComponent<TextMeshProUGUI>().text = counterforttl.text;
        //counterforttl.text = "0";
        mainheader.text = "";
        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        time = 2.0f;
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        
        if (mlocationDb.usersubsmarkets == "" || String.IsNullOrEmpty(mlocationDb.usersubsmarkets))
        {
            Stockexchanges = new string[] { "NSE", "NYSE", "NASDAQ", "LSE" };
        }
        else
        {
            //Stockexchanges = new string[] { "NSE", "BSE","NASDAQ", "NYSE", "LSE" };
            Stockexchanges = mlocationDb.usersubsmarkets.Split(new string[] { "," }, StringSplitOptions.None);
        }

       

        if (String.IsNullOrEmpty(mlocationDb.MarketSet) || mlocationDb.MarketSet == "")
        {
            Sector = "";
            mainheader.text = "";
            CanvasPositionManager.mainheader.text = "";
            errorpopup.SetActive(true);
            PopUpMsg.text = "Welcome To Equity Analyze, please load the desired Market index.";
        }

        else
        {
            Sector = mlocationDb.MarketSet.ToString();
            mainheader.text = mlocationDb.MarketSet.ToString();
            
            if (Sector != "")
            {
                GetExchange();

                
                CanvasPositionManager.loadingfromexchangescrpt();
                //errorpopup.SetActive(true);
                //PopUpMsg.text = "Welcome To Equity Analyze, loading stocks for; " + Sector.ToString();
            }
           
        }
        //mlocationDb.close();
        connection.Close();
        LoadExchanges();
        StartCoroutine(HidePopUp());
    }


    public void DateSimulate() //THIS WAS ADDED on 29th Nov 2022 for simulating date to fetch the data
    {
      
        if (mainheader.text==""|| String.IsNullOrEmpty(mainheader.text))
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Please select an exchange first to do the back test.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            simulatepopupfromdate.text = dateforshowing.text;
            simulatepopupresultdate.text = dateforshowing.text;
            simulatepopup.SetActive(true);
        }
        
    }
    public void DateSimulateYES() //THIS WAS ADDED on 29th Nov 2022 for simulating date to fetch the data
    {
        
        if (Subtractsdaysinput.text =="")
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Please enter some value and try again.";
            StartCoroutine(HidePopUp());
        }
        else
        {
        subtractdays = int.Parse(Subtractsdaysinput.text.ToString());
        simulationdateentered = DateTime.Parse(fromdate.text).AddDays(-subtractdays);
        //simulationdateentered = DateTime.Now.AddDays(-subtractdays);
        simulatepopupresultdate.text = simulationdateentered.ToString("ddd, dd MMM yyy");
        fromdate.text = simulationdateentered.ToString("yyyy-MM-dd");
        dateforshowing.text = simulationdateentered.ToString("ddd, dd MMM yyy");
        errorpopup.gameObject.SetActive(true);
        PopUpMsg.text = "Fetching stocks data for; " + fromdate.text.ToString() + " or market closing before this date.";

        Subtractsdaysinput.text = "";
            GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";
            GetExchange();
        StartCoroutine(HidePopUp());
        }
        
        //SceneManager.LoadScene("EquityAnalyzeMainScene");
    }

    public void DateSimulateNO() //THIS WAS ADDED on 29th Nov 2022 for simulating date to fetch the data
    {
        simulatepopup.SetActive(false);
    }

    public void DateSimulateADDDays() //THIS WAS ADDED on 29th Nov 2022 for simulating date to fetch the data
    {
        if (fromdate.text == DateTime.Now.ToString("yyyy-MM-dd"))
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! Already at the latest date.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";
            DateTime date = DateTime.Parse(fromdate.text).AddDays(1);
            fromdate.text = date.ToString("yyyy-MM-dd");
            dateforshowing.text = date.ToString("ddd, dd MMM yyy");
            GetExchange();
        }

        //SceneManager.LoadScene("EquityAnalyzeMainScene");
    }

    public void ResetDate() //THIS WAS ADDED on 29th Nov 2022 for simulating date to reset the data
    {
        GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";
        DateTime date = DateTime.Now;
        fromdate.text = date.ToString("yyyy-MM-dd");
        dateforshowing.text = date.ToString("ddd, dd MMM yyy");
        GetExchange();

    }

    public void LoadExchanges()
    {
        for (int i = 0; i < ExchangeParentPanel.transform.childCount; ++i) { Destroy(ExchangeParentPanel.transform.GetChild(i).gameObject); }

        foreach (string exchanges in Stockexchanges)
        {
            if (exchanges == null || exchanges == "")
            {
                Debug.Log("Empty..");
            }

            else
            {

                //CLOSED Debug.Log("These are the Exchanges in the; " + exchanges.ToString());

                //exhchangesplit = exchanges.Split('-')[0];// REMOVE the "-Country" from the Header;
                // countrysplit = exchanges.Split('-')[1];// KEEP the "COUNTRY" from the Header;

                exchange = Instantiate(exchangeprefab);
                exchange.transform.SetParent(ExchangeParentPanel, false);

                exchange.transform.Find("Text").GetComponentInChildren<Text>().text = exchanges.ToString();// + "." + mainheader.text.ToString();
                exchangename = exchanges.ToString();

                ExchgButton = exchange.GetComponent<Button>();
                ExchgButton.name = exchanges.ToString();
                exchange.onClick.AddListener(() => Exchangenamebtnclicked(exchanges.ToString()));

            }
            
        }
        //}
    }

    //public void Start() MOVED TO AWAKE IN ORDER TO CALL THE MARKET SETTING FROM DB on SCENE LOAD

    //{
    //    stockscalling = new Stocks_Accordion_Call_API();
    //    ParentPanel_1 = GameObject.FindGameObjectWithTag("AutowatchList").GetComponent<RectTransform>();
    //    mainheader.text = "";
    //    errorpopup.gameObject.SetActive(false);
    //    time = 1.5f;
    //}

    public void Exchangenamebtnclicked(string Exchangename)
    {

        Sector = Exchangename.ToString();
        mainheader.text = Exchangename.ToString();
        MarketMenu.SetActive(!MarketMenu.activeInHierarchy);
        GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";

        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        mlocationDb.Updatedata(" Marketsetting = '" + Sector + "' where Id = '" + mlocationDb.LatestId + "'");
        connection.Close();
        GetExchange();
    }

    public void NSEbtnclicked() //NOT USED AUTOMATED THE MARKETS FROM USER DATA AND USING ABOVE FUNCTION NAMED - Exchangenamebtnclicked(string Exchangename)
    {
        Sector = "NSE";
        mainheader.text = "NSE";
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        mlocationDb.Updatedata(" Marketsetting = '" + Sector + "' where Id = '" + mlocationDb.LatestId + "'");
        // Debug.Log("LATEST ID DETAILS IN DB ARE " + mlocationDb.LatestId.ToString() + " Market set is; " + mlocationDb.MarketSet.ToString());
        GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";
        connection.Close();
        GetExchange();
    }

    public void BSEbtnclicked() //NOT USED AUTOMATED THE MARKETS FROM USER DATA AND USING ABOVE FUNCTION NAMED - Exchangenamebtnclicked(string Exchangename)
    {
        Sector = "BSE";
        mainheader.text = "BSE";
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        mlocationDb.Updatedata(" Marketsetting = '" + Sector + "'where Id = '" + mlocationDb.LatestId + "'");
        //Debug.Log("LATEST ID DETAILS IN DB ARE " + mlocationDb.LatestId.ToString() +" Market set is; " + mlocationDb.MarketSet.ToString());
        GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = "0";
        connection.Close();
        GetExchange();
    }

    public void GetExchange()
    {
        for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }

        for (int i = 0; i < ParentPanel_1.transform.childCount; ++i) { Destroy(ParentPanel_1.transform.GetChild(i).gameObject); }
        //NSE.enabled = false;
        //BSE.enabled = false;
        ExchgButton.enabled = false;
        SectorAPI = new Sector_Stocks_Call_API();
        SectorAPI.StockSymbolSector = new List<string>();
        Sectors.Clear();
        GameObject.FindWithTag("marketwatchcounter").GetComponent<TextMeshProUGUI>().text = "0";

        //Sector = "NSE";
        url = "https://api.equityanalyze.com/api/sector/" + Sector;
       StartCoroutine(SectorResponserequest());
        
    }

    IEnumerator SectorResponserequest()
    {
        IndustryName = "";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");
            //NSE.enabled = true;
            //BSE.enabled = true;
            ExchgButton.enabled = true;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops !! Something went wrong, please check your internet connection or try again.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            var SectorsData = JsonConvert.DeserializeObject<SectorsMainClass>(request.downloadHandler.text);
            // ADD an extra Header named; Search For Stocks "{"_id":"Search","name":"Search"}" +","+ ;
            //for (int i=0;i< SectorsData.data.Count;i++)
            //{
            //    SectorsData.data[i].search = "Search";
            //    SectorsData.data[i]._id = "Search";
            //}
            //SectorsData.data.Add(new ExchangeSectors() { _id="Search",name="Search",search=""});
            
            industryaccordion = Instantiate(industryprefab);
            industryaccordion.transform.SetParent(ParentPanel, false);
            industryaccordion.transform.Find("Header").GetComponentInChildren<Text>().text = "Search" + "." + mainheader.text.ToString();
            //industryaccordion.transform.Find("Text/DateForStocks").GetComponentInChildren<Text>().text = fromdate.text.ToString();

            foreach (ExchangeSectors industry in SectorsData.data)
            {
                if (industry._id == null || industry.name == null)
                {
                    Debug.Log("Empty..");
                }

                else
                {
                    IndustryName = industry.name.ToString();
                    //CLOSED Debug.Log("These are the Sectors in the; " + Sector.ToString() + ": " + industry._id.ToString());
                    url = "https://api.equityanalyze.com/api/symbol/" + Sector + "/" + IndustryName;
                    industryaccordion = Instantiate(industryprefab);
                    
                    industryaccordion.transform.SetParent(ParentPanel, false);

                    industryaccordion.transform.Find("Header").GetComponentInChildren<Text>().text = IndustryName.ToString() + "."+ mainheader.text.ToString();
                    industryaccordion.transform.Find("Header/Text/DateForStocks").GetComponentInChildren<Text>().text = fromdate.text.ToString();
                    
                    //industryaccordion.transform.Find("Industry_Sectors/Header").GetComponentInChildren<Text>().text = IndustryName.ToString();
                    //industryaccordion.transform.GetComponentInChildren<AccordionElement>().enabled = true;
                    //industryaccordion.transform.GetComponentInChildren<AccordionElement>().isOn = false;
                    //industryaccordion.transform.GetComponentInChildren<AccordionElement>().interactable = true;
                    InudstriesinExch();
                }

            }

            industriesprefabs = GameObject.FindGameObjectsWithTag("stocksbutton");

            //if (industryaccordion.transform.Find("Header").GetComponentInChildren<Text>().text != "Search")
            //{
               foreach (GameObject go in industriesprefabs)
            {
                Stocksrefreshbtn = go.GetComponent<Button>();
                Stocksrefreshbtn.onClick.Invoke();
               
            }
               
            //}

        }

        printDebug();
    }

    public void InudstriesinExch()
    {
        Sectors.Add(IndustryName);
        
    }

    public void printDebug()
    {
        //Debug.Log("These are the Industries in; " + Sector + " Exchange -" + string.Join(",", Sectors).TrimEnd(','));
        urlfeed = 0;
        //NSE.enabled = true;
        //BSE.enabled = true;
        ExchgButton.enabled = true;
        //MOHIT COMMENTED THIS AS THIS PART AND ONWARDS MOVED TO THE ACCORDION ELEMENT CLICK EVENT Using Secto_Stocks_Call_API Script.

        //  GetStocksInSector();
    }

    //This segment is called from Each Stocks Refresh Button to get the latest information of the stock 
    public void GetStocksInSector()
    {
        StockSymbolSector.Clear();
        url1 = "https://api.equityanalyze.com/api/symbol/NSE/" + Sectors[urlfeed];
        StartCoroutine(SectorResponserequest1());
    }

    public IEnumerator SectorResponserequest1()
    {
        UnityWebRequest request = UnityWebRequest.Get(url1);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops !! Something went wrong, please check your internet connection or try again.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            var SectorsStocksData = JsonConvert.DeserializeObject<SectStocksBase>(request.downloadHandler.text);

            foreach (StockSymbols stocks in SectorsStocksData.data)
            {
                if (stocks.symbol == null || stocks.symbol == null || stocks.symbol == "")
                {
                    Debug.Log("Empty..");
                }

                else
                {
                    StockSymbol = stocks.symbol.ToString() + "\n";
                    //Debug.Log("These are the Sectors in the; " + Sector.ToString() + ": " + industry._id.ToString());
                    StocksinSector();
                }

            }

        }
        
        printDebug1();
        
    }

    public void StocksinSector()
    {
        StockSymbolSector.Add(StockSymbol);
    }

    public void printDebug1()
    {
       // Debug.Log("These are the Stock Symbols in; " + Sectors[urlfeed].ToString() + " - " + string.Join(",", StockSymbolSector));
        
        
        if (Sectors.Count - urlfeed ==1)
        {
            //NSE.enabled = true;
            //BSE.enabled = true;
            ExchgButton.enabled = true;
            return;
        }
        else
        {
            urlfeed += 1;
            GetStocksInSector();
        }
        
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        simulatepopup.SetActive(false);
        PopUpMsg.text = "";
    }
}
