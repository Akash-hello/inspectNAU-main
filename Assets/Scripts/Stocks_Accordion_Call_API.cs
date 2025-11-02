using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;
//using Unity.XR.CoreUtils;
using UnityEngine.Pool;

public class Stocks_Accordion_Call_API : MonoBehaviour
{
    private string url;
    public string StockForSector = "";
    string exchangename;
    public Text Header;
    public Button refreshbtn;
    public Text Counter;
    public Text RunningCounter;
   
    public int totalstockscountint;
    //public string SectorsinExch = "";
    //public List<string> SectorInExchg;

    public List<string> StockSymbolSector = new List<string>();
    //public List<string> BullishVBullStocks = new List<string>();
    public string StockSymbol = "";
    public int urlfeed;
    private string url1;

    public Text simulationdateconverted;
    private String dateforstocks;
    
    public RectTransform ParentPanel;
    public GameObject StocknameAndData;
    public GameObject Stockname;

    //public AutoMarketWatchList autowatchlist;
    RectTransform ParentPanel_1;
    public GameObject StocknameAndDataforwatch;
    public GameObject Stocknamewatch;
    public string searchstock;
    public string Stock;
    public string outputstock;

    public string DailyState;
    public string WeeklyState;
    public string MonthlyState;

    public string FinalStockState;
    public string StocksDeclining;
    public string readmoreURL;
    public string StockCategory;
    public string APIStockcategory;
    public  AccordionElement accordionelement;

    public GameObject SearchInput;
    public GameObject Headertextobj;
    InputField Searchinputfield;
    string searchinputtext;
    public Button searchhbtn;

    public GameObject PopUp;
    public Text PopUpMsg;
    public float time = 0.0f;
    bool searchbutton;
    public Color StarColor;
    public int StatusImageNumber;
    public Color GainLossColour;

    public bool addedinstockcount;

    public bool LoadSectorStocks;
    public ManageWatchList StockPerfData;
    public Text stock;
    public Text stocksym;
    public Text PerformanceWMYTD;

    public void Start()
    {
        stock.text = "";
        stocksym.text = "";
        LoadSectorStocks = false;
        
        refreshbtn.enabled = true;
        addedinstockcount = false;
        StockForSector = "";
        exchangename = "";
        Searchinputfield = SearchInput.GetComponent<InputField>();
        PopUp.gameObject.SetActive(false);
        searchbutton = false;
        time = 1.0f;

        dateforstocks = simulationdateconverted.text.ToString();

        //DateTime test = simulationdateentered.AddDays(-10);
        if (Header.text.Split('.')[0] == "Search")
        {
            SearchInput.SetActive(true);
            searchhbtn.gameObject.SetActive(true);

            Headertextobj.SetActive(false);
            refreshbtn.gameObject.SetActive(false);
            RunningCounter.gameObject.SetActive(false);
            Counter.gameObject.SetActive(false);

        }

        else
        {
            SearchInput.SetActive(false);
            searchhbtn.gameObject.SetActive(false);
            Headertextobj.SetActive(true);
            refreshbtn.gameObject.SetActive(true);
            RunningCounter.gameObject.SetActive(true);
            Counter.gameObject.SetActive(true);
        }

        //pooledObjects = new List<GameObject>();
        //GameObject tmp;
        //for (int i = 0; i < amountToPool; i++)
        //{
        //    tmp = Instantiate(StocknameAndData);
        //    tmp.SetActive(false);
        //    pooledObjects.Add(tmp);
        //}
    }

    //public GameObject GetPooledObject()
    //{
    //    for (int i = 0; i < amountToPool; i++)
    //    {
    //        if (!pooledObjects[i].activeInHierarchy)
    //        {
    //            return pooledObjects[i];
    //        }
    //    }
    //    return null;

    //}

    public void SectorBtnClicked()

    {
        LoadSectorStocks = true;
        RefreshData();
    }

    public void RefreshData()
    {
        StatusImageNumber = 2;
        RunningCounter.text = "0" + "  /";
        Counter.text = "0";
        urlfeed = 0;
       
        StockSymbolSector.Clear();
        GetStocksInSector();
    }

    public void GetStocksInSector()
    {
        refreshbtn.enabled = false;
         //StockForSector = "Real Estate";
         StockForSector = Header.text.Split('.')[0];// REMOVE the ".NSE" from the Header;
        exchangename = Header.text.Split('.')[1];// KEEP the "NSE" from the Header;
        
        for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }
        url = "https://api.equityanalyze.com/api/symbol/" + exchangename + "/" + StockForSector; //https://50.28.67.53:1015/api/symbol/
        //url = "https://api.equityanalyze.com/api/symbol/BSE/Other";
        StartCoroutine(SectorResponserequest());
    }

    public IEnumerator SectorResponserequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
          //CLOSED Debug.Log(request.error + "There was a error"); 
            PopUpMsg.text = "Oops!! please try again !!";
            PopUp.gameObject.SetActive(true);
            StartCoroutine(HidePopUp());
            refreshbtn.enabled = true;
        }
        else
        {
            var SectorsStocksData = JsonConvert.DeserializeObject<SectStocksBase>(request.downloadHandler.text);

            foreach (StockSymbols stocks in SectorsStocksData.data)
            {
                if (stocks.symbol == null || stocks.symbol == null || stocks.symbol == "")
                {
                    //CLOSEDDebug.Log("Empty..");
                    refreshbtn.enabled = true;
                }

                else
                {
                    StockSymbol = stocks.symbol.ToString();

                    
                    //Debug.Log("These are the Stocks in the; " + Sector.ToString() + ": " + industry._id.ToString());
                    StocksinSector();
                }

            }

        }
        //simulationdateconverted = GameObject.FindGameObjectWithTag("simulatedate").GetComponent<Text>();
        //Debug.Log("This the date for stocks" + simulationdateconverted.text.ToString());
        printDebug();
    }

    public void StocksinSector()
    {
        StockSymbolSector.Add(StockSymbol);
    }

    public void printDebug()
    {
        //Debug.Log("These are the Stock Symbols in; " + StockForSector + " - " + string.Join(",", StockSymbolSector));
        urlfeed = 0;
        GetStocksData();
    }

    public void GetStocksData()
    {
       
        if (StockSymbolSector.Count > urlfeed)
        {
            url1 = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=" + StockSymbolSector[urlfeed].Replace("&", "%26") + "&date="+ dateforstocks + "&periods=D-6,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_10-D,sma_20-D,sma_50-D,macd,macdsignal,macdhist,ema_5-D,ema_9-D,ema_26-D,volume,sma_100,sma_200,category";

           // url1 = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=JUBLFOOD.NSE&periods=D-6,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_10-D,sma_20-D,sma_50-D,macd,macdsignal,macdhist,ema_5-D,ema_9-D,ema_26-D,volume,sma_100,sma_200,category";

            //J&KBANK.NSE, L&TFH.NSE,M&MFIN.NSE  Error in  Symbol:J&KBANK.NSE
            //UnityEngine.Debug:Log(object)
            //Stocks_Accordion_Call_API /< SectorResponserequest1 > d__31:MoveNext()(at Assets / Scripts / Stocks_Accordion_Call_API.cs:512)
            //UnityEngine.SetupCoroutine:InvokeMoveNext(System.Collections.IEnumerator, intptr)

            outputstock = StockSymbolSector[urlfeed];
            
            StartCoroutine(SectorResponserequest1());
        }
       
    }

    public void SearchThisStockSymbol()
    {
        StockSymbolSector.Clear();
        if (Searchinputfield.text == "")
        {
            //CLOSED Debug.Log("Empty Stock Symbol");
            PopUp.gameObject.SetActive(true);
            PopUpMsg.text = "Please enter a Stock symbol !!";
            StartCoroutine(HidePopUp());
        }

        else
        {
            searchinputtext = Searchinputfield.text.ToString().ToUpper();
            for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }
            //url1 = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=" + ".NSE" + "&date=" + dateforstocks + "&periods=D-6,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_10-D,sma_20-D,sma_50-D,macd,macdsignal,macdhist,ema_5-D,ema_9-D,ema_26-D";
            url1 = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=" + searchinputtext.Replace("&", "%26") + "." + Header.text.Split('.')[1] + "&date=" + dateforstocks + "&periods=D-6,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_10-D,sma_20-D,sma_50-D,macd,macdsignal,macdhist,ema_5-D,ema_9-D,ema_26-D,volume,sma_100,sma_200,category";

            //url1 = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=" + "AAFRF.US" + "&periods=D-3,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_50-D,ema_5-D,ema_9-D,ema_26-D";
            
            outputstock = searchinputtext.ToString() + "." + Header.text.Split('.')[1];
            
            //CLOSEDDebug.Log("Search Button Clicked and URL is ---->>>>" + url1.ToString());
            searchbutton = true;
            LoadSectorStocks = true;
            StartCoroutine(SectorResponserequest1());
        }
       
    }


    public IEnumerator SectorResponserequest1()
    {
        Counter.text = StockSymbolSector.Count.ToString();

        try
        {
            if (addedinstockcount == false)
            {
                GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = (int.Parse(GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text) + StockSymbolSector.Count).ToString();
                addedinstockcount = true;
            }
        }
        catch (Exception err)
        {
            //Debug.Log("ERROR IN COUNTER --->" + StockForSector);
            RefreshData();
        }

        UnityWebRequest request = UnityWebRequest.Get(url1);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            //CLOSED Debug.Log(request.error + "There was a error");
            PopUpMsg.text = "Oops!! please try again !!";
            PopUp.gameObject.SetActive(true);
            StartCoroutine(HidePopUp());
            searchbutton = false;
            refreshbtn.enabled = true;
        }
        else
        {
            //DateTime before = DateTime.Now;

            //Debug.Log("Before -"+request.downloadHandler.text);
            var stockdata = JsonConvert.DeserializeObject<StocksmainClass>(request.downloadHandler.text);

            List<StocksAPI_Call> DailyDatafordate = stockdata.data.Where(l => l.period == "Daily").ToList();
            // string date = DailyDatafordate[0].data[0].date;

            //Debug.Log(outputstock.Split('.')[0].ToString() + "--"+ outputstock.Split('.')[1].ToString() +"--" +date + "--"+request.downloadHandler.text);

            //Debug.Log("After -" + stockdata.data[0]);
            //var result = JsonConvert.DeserializeObject<Result>(request.downloadHandler.text);
            if (stockdata.data[0].data == null || stockdata.data.Count == 0 || stockdata.data[0].data.Count == 0)//||result.result == "error") //INCASE THE SEARCH YIELDED NO RESULTS OR THERE WAS AN INVALID SYMBOL, this will take care of that..
            {
                //CLOSED Debug.Log("No Data or Invalid Stock Symbol !!");
                PopUpMsg.text = "No data for this symbol !!";
                PopUp.gameObject.SetActive(true);
                searchbutton = false;
                refreshbtn.enabled = true;
                RunningCounter.text = (urlfeed + 1).ToString() + "  /";
                StartCoroutine(HidePopUp());
                printDebug1();
            }

            else
            {

                string date = DailyDatafordate[0].data[0].date;
                foreach (string sym in outputstock.Split(','))
                {

                    // the code that you want to measure comes here


                    //try
                    //{
                    //CLOSED Debug.Log("These are the Symbols requested; " + outputstock.ToString());
                    FinalStockState = "";
                    StocksDeclining = "";
                    StockCategory = "";
                    readmoreURL = "";
                    string urlexchange = sym.Split('.')[1];// Keep the ".NSE" from sym
                    string urlsym = sym.Split('.')[0];// REMOVE the ".NSE" from the sym
                    //readmoreURL = "https://www.tradingview.com/chart/?symbol=" + urlexchange + ":" + "ABMINTLLTD";
                    readmoreURL = "https://www.tradingview.com/chart/?symbol=" + urlexchange + ":" + urlsym.Replace("&", "_");
                    double gainloss = 0;
                    List<StocksAPI_Call> DailyData = stockdata.data.Where(l => l.period == "Daily" && l.symbol == sym).ToList();
                    List<StocksAPI_Call> WeeklyData = stockdata.data.Where(l => l.period == "W" && l.symbol == sym).ToList();
                    List<StocksAPI_Call> MonthlyData = stockdata.data.Where(l => l.period == "M" && l.symbol == sym).ToList();
                    int n = DailyData[0].data.Count; // TO CHECK WITH ANKUSH For ARGUMENT EXCEPTION ON THIS
                    int w = WeeklyData[0].data.Count;
                    int m = MonthlyData[0].data.Count;

                    DateTime recorddate = Convert.ToDateTime(DailyData[0].data[n - 1].date);

                    //IMPORTANT, THIS DATA CAN BE SAVED IN THE DB  Debug.Log(outputstock.Split('.')[0].ToString() + "--" + outputstock.Split('.')[1].ToString() + "--" + recorddate + "--" + request.downloadHandler.text);

                    var markercap = DailyData[0].marketcap;
                    var stockname = "";

                    if (String.IsNullOrEmpty(DailyData[0].name) || DailyData[0].name == "")
                    {
                        stockname = "-";
                    }
                    else
                    {
                        stockname = DailyData[0].name;
                    }

                    APIStockcategory = DailyData[0].data[n - 1].category.ToString();
                    
                    ////CHECK THE SMAs 10,50,100 & 200 trends

                    double sma_10_diff = DailyData[0].data[n - 1].sma_10 - DailyData[0].data[n - 2].sma_10;
                    double sma_50_diff = DailyData[0].data[n - 1].sma_50 - DailyData[0].data[n - 2].sma_50;
                    double sma_100_diff = DailyData[0].data[n - 1].sma_100 - DailyData[0].data[n - 2].sma_100;
                    double sma_200_diff = DailyData[0].data[n - 1].sma_200 - DailyData[0].data[n - 2].sma_200;
                    int result = 0;

                    if (sma_10_diff > 0)
                    {
                        result += 1;
                    }

                    if (sma_50_diff > 0)
                    {
                        result += 1;
                    }
                    if (sma_100_diff > 0)
                    {
                        result += 1;
                    }
                    if (sma_200_diff > 0)
                    {
                        result += 1;
                    }

                    //1 Time Check


                    //Debug.Log("Final result Score; " + sym +"----"+result);
                    var candleimagecolor = Color.grey;
                    float rotate = 0;

                    if (DailyData[0].data[n - 1].open > DailyData[0].data[n - 1].adj_close)
                    {
                        candleimagecolor = Color.red;
                        rotate = 180;
                    }

                    else if (DailyData[0].data[n - 1].open < DailyData[0].data[n - 1].adj_close)
                    {
                        candleimagecolor = Color.green;
                        rotate = 0;
                    }

                    gainloss = DailyData[0].data[n - 1].adj_close - DailyData[0].data[n - 2].adj_close;

                    StockCategory = APIStockcategory;

                    if (StockCategory == "VERY BULLISH") { StarColor = Color.green; StatusImageNumber = 0; }
                    else if (StockCategory == "SUPER TREND") { ColorUtility.TryParseHtmlString("#D4AF37", out StarColor); StatusImageNumber = 0; }
                    else if (StockCategory == "BULLISH") { StarColor = Color.green; StatusImageNumber = 1; }
                    else if (StockCategory == "RANGING") { StarColor = Color.grey; StatusImageNumber = 2; }
                    else if (StockCategory == "WATCH OUT") { StarColor = Color.yellow; StatusImageNumber = 1; }
                    else if (StockCategory == "DECLINING") { StarColor = Color.red; StatusImageNumber = 1; }
                    else { StarColor = Color.grey; StatusImageNumber = 3; } //THESE ARE THE STOCKS WITH WEEK AND MONTH > 60, BUT DAILY ARE NOT YET MEETING THE 40 RSI CRITERIA AND NOT YET WORTH LOOKING AT.....

                    if (gainloss >= 0) { GainLossColour = Color.green; } else if (gainloss < 0) { GainLossColour = Color.red; }

                    if (LoadSectorStocks == true || LoadSectorStocks == true)
                    {
                        Stockname = Instantiate(StocknameAndData);

                        //Stockname = Stocks_Accordion_Call_API.SharedInstance.GetPooledObject();
                        //Stockname.SetActive(true);
                        Stockname.transform.SetParent(ParentPanel, false);
                        Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
                        Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();
                        Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
                        Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
                        Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
                        Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = StockCategory.ToString();
                        Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                        Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
                        Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
                        Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[StatusImageNumber];
                        Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = StarColor;
                        Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].adj_close.ToString();
                        Stockname.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = GainLossColour;
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)

                        Stockname.transform.Find("NewsRoomBTNSpeak/Text").GetComponentInChildren<Text>().text = APIStockcategory.ToString();

                    }


                    if ((StockCategory == "VERY BULLISH" || StockCategory == "SUPER TREND" || StockCategory == "BULLISH") && LoadSectorStocks == false && searchbutton == false) //If the Stock is Very Bullish, it instantiates a prefab into the Auto Watch List from here and also Next Block for BULLISh. 
                    {

                        //This block generates the Prefab inside the "MARKETWATCH LIST"
                        Stocknamewatch = Instantiate(StocknameAndDataforwatch);
                         ParentPanel_1 = GameObject.FindGameObjectWithTag("AutowatchList").GetComponent<RectTransform>();
                        Stocknamewatch.transform.SetParent(ParentPanel_1, false);
                        Stocknamewatch.transform.GetComponentInChildren<Text>().text = sym.ToString();
                        Stocknamewatch.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();
                        Stocknamewatch.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
                        Stocknamewatch.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
                        Stocknamewatch.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
                        Stocknamewatch.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                        Stocknamewatch.transform.Find("Status").GetComponentInChildren<Text>().text = StockCategory.ToString();
                        Stocknamewatch.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].adj_close.ToString();
                        Stocknamewatch.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
                        Stocknamewatch.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
                        Stocknamewatch.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
                        Stocknamewatch.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stocknamewatch.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[StatusImageNumber];
                        Stocknamewatch.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = StarColor;
                        Stocknamewatch.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = GainLossColour;
                        Stocknamewatch.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        Stocknamewatch.transform.Find("NewsRoomBTNSpeak/Text").GetComponentInChildren<Text>().text = APIStockcategory.ToString();

                        GameObject.FindWithTag("marketwatchcounter").GetComponent<TextMeshProUGUI>().text = (int.Parse(GameObject.FindWithTag("marketwatchcounter").GetComponent<TextMeshProUGUI>().text) + 1).ToString();

                        StockPerfData = new ManageWatchList();
                        
                        //stock.text = sym;

                        StockPerfData.stckname = stock.GetComponentInChildren<Text>();
                        StockPerfData.symbol = stocksym.GetComponentInChildren<Text>();
                        //StockPerfData.performanceTxt = PerformanceWMYTD.GetComponentInChildren<Text>();
                        StockPerfData.stckname.text = sym.ToString();
                        StockPerfData.symbol.text = sym.ToString();
                        StockPerfData.FmStocksAPI = true;
                        StockPerfData.url2 = "https://api.equityanalyze.com/api/yahoo/" + sym.ToString();
                        StockPerfData.url1 = "https://news-headlines.tradingview.com/v2/headlines?client=chart&lang=en&symbol=NSE%3A" + sym.ToString();
                        //StockPerfData.stckname.text = stock.text.ToString();
                        //StartCoroutine(StockPerfData.FetchPerformancedata());

                        //Debug.Log("Details for StocksAPI stock name " + stockname.ToString() + " Stock Sym " + sym.ToString());
                    }


                    if (n <= 4 && w <= 2 && m <= 2 && LoadSectorStocks == true)// if (n < 3 || w < 2 || m < 3)
                    {
                        //CLOSED Debug.Log("The RSI Data is unavaible for one of the periods: " + sym);
                        gainloss = DailyData[0].data[n - 1].adj_close - DailyData[0].data[n - 2].adj_close;
                        Stockname = Instantiate(StocknameAndData);
                        Stockname.transform.SetParent(ParentPanel, false);

                        Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
                        Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();// "Insufficient Data";
                        Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
                        Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
                        Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
                        Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = "INSUFFICIENT DATA";
                        Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].adj_close.ToString();
                        Stockname.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)";
                        Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[3];
                        Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
                        Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
                        Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                        Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = Color.grey;
                        Stocknamewatch.transform.Find("NewsRoomBTNSpeak/Text").GetComponentInChildren<Text>().text = APIStockcategory.ToString();

                        if (gainloss >= 0)
                        {
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.green;
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        }
                        else if (gainloss < 0)
                        {
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.red;
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        }
                    }
                    //}
                    //catch (Exception err)
                    //{
                    //    // 3333333333 
                    //    Debug.Log("Error in  Symbol:" + sym);
                    //    PopUpMsg.text = "No data for " + sym;
                    //    PopUp.gameObject.SetActive(true);
                    //    RunningCounter.text = (urlfeed + 1).ToString() + "  /";
                    //    searchbutton = false;
                    //    refreshbtn.enabled = true;
                    //    StartCoroutine(HidePopUp());
                    //    printDebug1();

                    //}
                    // Debug.Log(LoadSectorStocks);
                }
                if (Counter.text == "0") //Line added on 13th Aug 2023 for ensuring Running counter does not trigger before Counter.
                {
                    GetStocksData();
                }
                else
                {
                    RunningCounter.text = (urlfeed + 1).ToString() + "  /";
                }
                searchbutton = false;
                printDebug1();

            }

            //DateTime after = DateTime.Now;
            //TimeSpan duration = after.Subtract(before);
            //Debug.Log("Duration in milliseconds: " + outputstock.ToString() + "--" +duration.Milliseconds);

            //Debug.Log(outputstock.ToString() + " Mohit Category -- " + StockCategory.ToString() + " , Ankush Category -- " + APIStockcategory.ToString());
        }
    }

    //OLD BEFORE TRENDS FROM API ----->>> public IEnumerator SectorResponserequest1()
    //{
    //    Counter.text = StockSymbolSector.Count.ToString();

    //    try
    //    {
    //        if (addedinstockcount == false)
    //        {
    //            GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text = (int.Parse(GameObject.FindWithTag("stockscount").GetComponent<TextMeshProUGUI>().text) + StockSymbolSector.Count).ToString();
    //            addedinstockcount = true;
    //        }
    //    }
    //    catch (Exception err)
    //    {
    //        //Debug.Log("ERROR IN COUNTER --->" + StockForSector);
    //        RefreshData();
    //    }

    //    UnityWebRequest request = UnityWebRequest.Get(url1);
    //        yield return request.SendWebRequest();

    //    if (request.isNetworkError || request.isHttpError)
    //    {
    //        //CLOSED Debug.Log(request.error + "There was a error");
    //        PopUpMsg.text = "Oops!! please try again !!";
    //        PopUp.gameObject.SetActive(true);
    //        StartCoroutine(HidePopUp());
    //        searchbutton = false;
    //        refreshbtn.enabled = true;
    //    }
    //    else
    //    {
    //        //DateTime before = DateTime.Now;

    //        //Debug.Log("Before -"+request.downloadHandler.text);
    //        var stockdata = JsonConvert.DeserializeObject<StocksmainClass>(request.downloadHandler.text);

    //        List<StocksAPI_Call> DailyDatafordate = stockdata.data.Where(l => l.period == "Daily").ToList();
    //       // string date = DailyDatafordate[0].data[0].date;

    //        //Debug.Log(outputstock.Split('.')[0].ToString() + "--"+ outputstock.Split('.')[1].ToString() +"--" +date + "--"+request.downloadHandler.text);
            
    //        //Debug.Log("After -" + stockdata.data[0]);
    //        //var result = JsonConvert.DeserializeObject<Result>(request.downloadHandler.text);
    //        if (stockdata.data[0].data == null || stockdata.data.Count == 0|| stockdata.data[0].data.Count == 0)//||result.result == "error") //INCASE THE SEARCH YIELDED NO RESULTS OR THERE WAS AN INVALID SYMBOL, this will take care of that..
    //        {
    //            //CLOSED Debug.Log("No Data or Invalid Stock Symbol !!");
    //            PopUpMsg.text = "No data for this symbol !!";
    //            PopUp.gameObject.SetActive(true);
    //            searchbutton = false;
    //            refreshbtn.enabled = true;
    //            RunningCounter.text = (urlfeed + 1).ToString() + "  /";
    //            StartCoroutine(HidePopUp());
    //            printDebug1();
    //        }

    //        else
    //        {
                                
    //            string date = DailyDatafordate[0].data[0].date;
    //            foreach (string sym in outputstock.Split(','))
    //            {
                    
    //                // the code that you want to measure comes here
                   

    //                //try
    //                //{
    //                //CLOSED Debug.Log("These are the Symbols requested; " + outputstock.ToString());
    //                FinalStockState = "";
    //                    StocksDeclining = "";
    //                    StockCategory = "";
    //                    readmoreURL = "";
    //                    string urlexchange = sym.Split('.')[1];// Keep the ".NSE" from sym
    //                    string urlsym = sym.Split('.')[0];// REMOVE the ".NSE" from the sym
    //                //readmoreURL = "https://www.tradingview.com/chart/?symbol=" + urlexchange + ":" + "ABMINTLLTD";
    //                readmoreURL = "https://www.tradingview.com/chart/?symbol=" + urlexchange + ":" + urlsym.Replace("&", "_");
    //                    double gainloss = 0;
    //                    List<StocksAPI_Call> DailyData = stockdata.data.Where(l => l.period == "Daily" && l.symbol == sym).ToList();
    //                    List<StocksAPI_Call> WeeklyData = stockdata.data.Where(l => l.period == "W" && l.symbol == sym).ToList();
    //                    List<StocksAPI_Call> MonthlyData = stockdata.data.Where(l => l.period == "M" && l.symbol == sym).ToList();
    //                    int n = DailyData[0].data.Count;
    //                int w = WeeklyData[0].data.Count;
    //                int m = MonthlyData[0].data.Count;

    //                DateTime recorddate = Convert.ToDateTime(DailyData[0].data[n - 1].date);
                  
    //                //IMPORTANT, THIS DATA CAN BE SAVED IN THE DB  Debug.Log(outputstock.Split('.')[0].ToString() + "--" + outputstock.Split('.')[1].ToString() + "--" + recorddate + "--" + request.downloadHandler.text);

    //                var markercap = DailyData[0].marketcap;
    //                var stockname = "";

    //                if (String.IsNullOrEmpty(DailyData[0].name) || DailyData[0].name =="")
    //                {
    //                    stockname = "-";
    //                }
    //                else
    //                {
    //                    stockname = DailyData[0].name;
    //                }
    //                APIStockcategory = DailyData[0].data[n - 1].category.ToString();
    //                var SMA_1 = DailyData[0].data[n - 1].sma_50;
    //                    var SMA_2 = DailyData[0].data[n - 2].sma_50;
                         
    //                    double sma_20_1 = DailyData[0].data[n - 1].sma_20;
    //                    double sma_20_2 = DailyData[0].data[n - 2].sma_20;

    //                    double sma_10_1 = DailyData[0].data[n - 1].sma_10;
    //                    double sma_10_2 = DailyData[0].data[n - 2].sma_10;
    //                    double sma_10_3 = DailyData[0].data[n - 3].sma_10;

    //                double sma_50_1 = DailyData[0].data[n - 1].sma_50;
    //                double sma_50_2 = DailyData[0].data[n - 2].sma_50;

    //                double sma_100_1 = DailyData[0].data[n - 1].sma_100;
    //                double sma_100_2 = DailyData[0].data[n - 2].sma_100;
    //                double sma_100_3 = DailyData[0].data[n - 3].sma_100;

    //                double sma_200_1 = DailyData[0].data[n - 1].sma_200;
    //                double sma_200_2 = DailyData[0].data[n - 2].sma_200;
    //                double sma_200_3 = DailyData[0].data[n - 3].sma_200;

    //                double e5_1 = DailyData[0].data[n - 1].ema_5;
    //                    double e5_2 = DailyData[0].data[n - 2].ema_5;
    //                    double e5_3 = DailyData[0].data[n - 3].ema_5;

    //                    double e9_1 = DailyData[0].data[n - 1].ema_9;
    //                    double e9_2 = DailyData[0].data[n - 2].ema_9;
    //                    double e9_3 = DailyData[0].data[n - 3].ema_9;

    //                    double e26_1 = DailyData[0].data[n - 1].ema_26;
    //                    double e26_2 = DailyData[0].data[n - 2].ema_26;

    //                    double open = DailyData[0].data[n - 1].open;
    //                    double close = DailyData[0].data[n - 1].adj_close;

    //                    double macd_1 = DailyData[0].data[n - 1].macd;
    //                    double macdsignal_1 = DailyData[0].data[n - 1].macdsignal;
    //                    double macdhist_1 = DailyData[0].data[n - 1].macdhist;

    //                    double macd_2 = DailyData[0].data[n - 2].macd;
    //                    double macdsignal_2 = DailyData[0].data[n - 2].macdsignal;
    //                    double macdhist_2 = DailyData[0].data[n - 2].macdhist;

    //                    double macd_3 = DailyData[0].data[n - 3].macd;
    //                    double macdsignal_3 = DailyData[0].data[n - 3].macdsignal;
    //                    double macdhist_3 = DailyData[0].data[n - 3].macdhist;

    //                    double macd_4 = DailyData[0].data[n - 4].macd;
    //                    double macdsignal_4 = DailyData[0].data[n - 4].macdsignal;
    //                    double macdhist_4 = DailyData[0].data[n - 4].macdhist;

    //                    double V_1 = DailyData[0].data[n - 1].volume;
    //                    double V_2 = DailyData[0].data[n - 2].volume;
    //                    double V_3 = DailyData[0].data[n - 3].volume;

    //                //Daily data values
    //                double d1 = DailyData[0].data[n - 1].rsi;
    //                double d2 = DailyData[0].data[n - 2].rsi;
    //                double d3 = DailyData[0].data[n - 3].rsi;

    //                //Weekly data values
    //                double w1 = WeeklyData[0].data[w - 1].rsi;
    //                double w2 = WeeklyData[0].data[w - 2].rsi;
    //                double w3 = WeeklyData[0].data[w - 3].rsi;


    //                //Monthly data values
    //                double m1 = MonthlyData[0].data[m - 1].rsi;
    //                double m2 = MonthlyData[0].data[m - 2].rsi;
    //                double m3 = MonthlyData[0].data[m - 3].rsi;

    //                //CHECK THE SMAs 10,50,100 & 200 trends

    //                double sma_10_diff = sma_10_1 - sma_10_2;
    //                double sma_50_diff = sma_50_1 - sma_50_2;
    //                double sma_100_diff = sma_100_1 - sma_100_2;
    //                double sma_200_diff = sma_200_1 - sma_200_2;
    //                int result = 0;

    //                if (sma_10_diff > 0)
    //                {
    //                    result += 1;
    //                }

    //                if (sma_50_diff > 0)
    //                {
    //                    result += 1;
    //                }
    //                if (sma_100_diff > 0)
    //                {
    //                    result += 1;
    //                }
    //                if (sma_200_diff > 0)
    //                {
    //                    result += 1;
    //                }

    //                //1 Time Check

                   
    //                //Debug.Log("Final result Score; " + sym +"----"+result);
    //                var candleimagecolor = Color.grey;
    //                float rotate = 0;

    //                if (open > close)
    //                {
    //                    candleimagecolor = Color.red;
    //                    rotate = 180;
    //                }

    //                    else if (open < close)
    //                {
    //                    candleimagecolor = Color.green;
    //                    rotate = 0;
    //                }

    //                    if (n>4 && w >2 && m >2) //(WeeklyData[0].data[n - 3].rsi != 0) && (MonthlyData[0].data[n - 3].rsi != 0)&&This is to state if the RSI data is insufficient or COunt of Daily, Weekly, Monthly is Less than 3 then the data is of no use.
    //                {

    //                    //2 Time Check
                        

    //                    if (((DailyData[0].data[n - 1].sma_100 - DailyData[0].data[n - 2].sma_100) > 0)) //COMPARING SMA_100 for latest two days to see if upwards, this needs to further done in slope value.
    //                        {
                            

    //                        if (w1 > 45 && m1 > 55) // This is important for 5 star Strategy & then check the status of the Daily RSI to see forming support and then take the call.
                                                            
    //                        {

    //                            if(((d1 > d2) && ((((e9_1 - e9_2) / e9_2) * 100) > 0.4) &&
    //                                ((((sma_20_1 - sma_20_2) / sma_20_2) * 100) > 0.1) &&
    //                                V_1 > 100000 && open < close &&
    //                                ((((e5_1 - e5_2) / e5_2) * 100) > 0.35))
    //                                && 
    //                                ((((macd_1 > macdsignal_1) && macdhist_1 > macdhist_2) && ((macd_2 > macdsignal_2) &&
    //                                    macdhist_2 > macdhist_3))
    //                                    ||
    //                                    (((macd_2 > macd_3 && macd_2 > macdsignal_2 && macd_3 < macdsignal_3 && macdsignal_2 > macdsignal_3) && macdhist_2 > macdhist_3
    //                                    && ((sma_10_1 - sma_10_2) > sma_10_2 - sma_10_3)))))

    //                                {
    //                                    if (macd_3 < 0) // Basically if the crossover happened under the ZERO Line..
    //                                    {
    //                                        //CLOSED Debug.Log(sym + "-> VERY BULLISH ABOVE CONDITION WITH HEAVY RISING TREND");
    //                                        StockCategory = "SUPER TREND"; //GOLD STAR

    //                                    //Debug.Log(sym + "- Good Volume; " + V_1 + "---" + V_2 + "---" + V_2 + "---" + V_3);
    //                                    }

    //                                    else
    //                                    {
    //                                        //CLOSED Debug.Log(sym + "Support, Daily RSI");
    //                                        StockCategory = "VERY BULLISH"; //GREEN STAR
    //                                        FinalStockState = sym + ": VERY BULLISH" + "\n";
    //                                        // Debug.Log(sym + ": VERY BULLISH with RSI = " + d1);
    //                                        //Debug.Log(sym + "- Good Volume; " + V_1 + "---" + V_2 + "---" + V_2 + "---" + V_3);
    //                                    }
    //                                }

    //                                else
    //                                    {
    //                                    StockCategory = "WATCH OUT"; //YELLOW DOT
    //                                    FinalStockState = sym + ": WATCH OUT" + "\n";
                                        
    //                                }


    //                        }
                            
    //                        else if (V_1 > 100000 && V_1 > V_2*1.75 && open < close && macdhist_1 > 0 && (macdhist_1 > macdhist_2) && (macd_1 > macdsignal_1)&&(sma_10_2<sma_10_3 && sma_10_1>sma_10_2))
    //                            {
    //                                //CLOSED Debug.Log(sym + "Support, Daily RSI");
    //                                StockCategory = "BULLISH"; //GREEN DOT
    //                                FinalStockState = sym + ": BULLISH" + "\n";
    //                                //Debug.Log(sym + ": BULLISH with RSI = " + d1);
    //                                //Debug.Log(sym + "- Volume DOUBLED OR MORE; " + V_1 + "---" + V_2 + "---" + V_2 + "---" + V_3);
    //                            }
    //                        else if (w1 <= 45 || m1 <= 55)
    //                            {

    //                                StocksDeclining = sym + "\n";
    //                                StockCategory = "RANGING";

    //                            }

    //                        if ((sma_10_1 > sma_10_2 && sma_50_1 > sma_50_2 && sma_100_1 > sma_100_2 && sma_200_1 > sma_200_2) && (sma_10_1 > sma_50_1 && sma_50_1 > sma_100_1 && sma_100_1 > sma_200_1)
    //                        && V_1 > 100000 && V_1 > V_2 * 1.75 && open < close && macdhist_1 > 0 && (macdhist_1 > macdhist_2) && (macd_1 > macdsignal_1)
    //                         )// 
    //                        {
    //                            StockCategory = "SUPER TREND"; //GOLD STAR
    //                            //Debug.Log(sym + ": BULLISH with RSI = " + d1);
    //                            //Debug.Log("THIS ONE IS ON SUPER TREND ----> " + sym);
    //                        }

    //                        // COMMENTED ON 06th June 2022 FOR CORRECTING THE STRATEGY AND REPLACED WITH ABOVE MODIFICATIONS

    //                        //CODE FROM HERE is backup at location; D:\Documents_1\Stocks\C_Scharp_Script_Formula - File name; All_Above_60_old_Backup.txt

    //                        // COMMENTED ON 06th June 2022 FOR CORRECTING THE STRATEGY

    //                        //Stock = sym.ToString();

    //                        gainloss = DailyData[0].data[n - 1].adj_close - DailyData[0].data[n - 2].adj_close;

    //                        if (StockCategory == "VERY BULLISH") {StarColor = Color.green; StatusImageNumber = 0; }
    //                        else if (StockCategory == "SUPER TREND") { ColorUtility.TryParseHtmlString("#D4AF37", out StarColor); StatusImageNumber = 0; }
    //                        else if (StockCategory == "BULLISH") { StarColor = Color.green; StatusImageNumber = 1; }
    //                        else if (StockCategory == "RANGING") { StarColor = Color.grey; StatusImageNumber = 2; }
    //                        else if (StockCategory == "WATCH OUT") { StarColor = Color.yellow; StatusImageNumber = 1; }
    //                        else if (StockCategory == "DECLINING") { StarColor = Color.red; StatusImageNumber = 1; }
    //                        else { StarColor = Color.grey; StatusImageNumber = 3; } //THESE ARE THE STOCKS WITH WEEK AND MONTH > 60, BUT DAILY ARE NOT YET MEETING THE 40 RSI CRITERIA AND NOT YET WORTH LOOKING AT.....

    //                        if (gainloss >= 0) {GainLossColour = Color.green;} else if (gainloss < 0) {GainLossColour = Color.red;}

    //                        if (LoadSectorStocks == true||LoadSectorStocks == true)
    //                        {
    //                            Stockname = Instantiate(StocknameAndData);

    //                            //Stockname = Stocks_Accordion_Call_API.SharedInstance.GetPooledObject();
    //                            //Stockname.SetActive(true);
    //                            Stockname.transform.SetParent(ParentPanel, false);
    //                            Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
    //                            Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();
    //                            Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
    //                            Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
    //                            Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
    //                            Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = StockCategory.ToString();
    //                            Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
    //                            Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
    //                            Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
    //                            Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[StatusImageNumber];
    //                            Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = StarColor;
    //                            Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].adj_close.ToString();
    //                            Stockname.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
    //                            Stockname.transform.Find("NewsRoom/Text").GetComponentInChildren<Text>().text = APIStockcategory.ToString();

    //                        }


    //                        if ((StockCategory == "VERY BULLISH" || StockCategory == "SUPER TREND" || StockCategory == "BULLISH")  && LoadSectorStocks == false && searchbutton == false) //If the Stock is Very Bullish, it instantiates a prefab into the Auto Watch List from here and also Next Block for BULLISh. 
    //                        {

    //                                //This block generates the Prefab inside the "MARKETWATCH LIST"
    //                                Stocknamewatch = Instantiate(StocknameAndDataforwatch);
    //                                ParentPanel_1 = GameObject.FindGameObjectWithTag("AutowatchList").GetComponent<RectTransform>();
    //                                Stocknamewatch.transform.SetParent(ParentPanel_1, false);
    //                                Stocknamewatch.transform.GetComponentInChildren<Text>().text = sym.ToString();
    //                                Stocknamewatch.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();
    //                                Stocknamewatch.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
    //                                Stocknamewatch.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
    //                                Stocknamewatch.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
    //                                Stocknamewatch.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
    //                                Stocknamewatch.transform.Find("Status").GetComponentInChildren<Text>().text = StockCategory.ToString();
    //                                Stocknamewatch.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].adj_close.ToString();
    //                                Stocknamewatch.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
    //                                Stocknamewatch.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
    //                                Stocknamewatch.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
    //                                Stocknamewatch.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stocknamewatch.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[StatusImageNumber];
    //                                Stocknamewatch.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = StarColor;
    //                                Stocknamewatch.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = GainLossColour;
    //                                Stocknamewatch.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
    //                            Stocknamewatch.transform.Find("NewsRoom/Text").GetComponentInChildren<Text>().text = APIStockcategory.ToString();

    //                            GameObject.FindWithTag("marketwatchcounter").GetComponent<TextMeshProUGUI>().text = (int.Parse(GameObject.FindWithTag("marketwatchcounter").GetComponent<TextMeshProUGUI>().text) + 1).ToString();

    //                        }
                           
    //                        }


    //                        else if (((DailyData[0].data[n - 1].sma_100 - DailyData[0].data[n - 2].sma_100) <= 0) && LoadSectorStocks == true)
    //                        {
    //                        //CLOSED Debug.Log("Declining Averages: " + sym);
    //                        gainloss = DailyData[0].data[n - 1].adj_close - DailyData[0].data[n - 2].adj_close;
    //                            Stockname = Instantiate(StocknameAndData);
    //                            Stockname.transform.SetParent(ParentPanel, false);

    //                            Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
    //                            Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();  // "Declining Averages";
    //                            Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
    //                            Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
    //                            Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
    //                            Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = "DECLINING";
    //                            Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text =  DailyData[0].data[n - 1].adj_close.ToString();
    //                            Stockname.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
    //                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)";
    //                            Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[1];
    //                            Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
    //                        Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
    //                        Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
    //                            Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = Color.red;
    //                        Stockname.transform.Find("NewsRoom/Text").GetComponentInChildren<Text>().text = APIStockcategory.ToString();
    //                        if (gainloss >= 0)
    //                            {
    //                                Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.green;
    //                                Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
    //                            }
    //                            else if (gainloss < 0)
    //                            {
    //                                Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.red;
    //                                Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
    //                            }
    //                        }
    //                    }

    //                    else if (LoadSectorStocks == true)// if (n < 3 || w < 2 || m < 3)
    //                    {
    //                    //CLOSED Debug.Log("The RSI Data is unavaible for one of the periods: " + sym);
    //                    gainloss = DailyData[0].data[n - 1].adj_close - DailyData[0].data[n - 2].adj_close;
    //                        Stockname = Instantiate(StocknameAndData);
    //                        Stockname.transform.SetParent(ParentPanel, false);

    //                        Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
    //                        Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();// "Insufficient Data";
    //                        Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
    //                        Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
    //                        Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
    //                        Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = "INSUFFICIENT DATA";
    //                        Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text =  DailyData[0].data[n - 1].adj_close.ToString();
    //                        Stockname.transform.Find("LastOpen").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].open.ToString();
    //                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)";
    //                        Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[3];
    //                        Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
    //                    Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
    //                    Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
    //                        Stockname.transform.Find("AddtoMyWtchDummy/AddtoMyWtchBtn").GetComponent<Image>().color = Color.grey;
    //                        if (gainloss >= 0)
    //                        {
    //                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.green;
    //                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
    //                        }
    //                        else if (gainloss < 0)
    //                        {
    //                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.red;
    //                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / DailyData[0].data[n - 2].adj_close) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
    //                        }
    //                    }
    //                //}
    //                //catch (Exception err)
    //                //{
    //                //    // 3333333333 
    //                //    Debug.Log("Error in  Symbol:" + sym);
    //                //    PopUpMsg.text = "No data for " + sym;
    //                //    PopUp.gameObject.SetActive(true);
    //                //    RunningCounter.text = (urlfeed + 1).ToString() + "  /";
    //                //    searchbutton = false;
    //                //    refreshbtn.enabled = true;
    //                //    StartCoroutine(HidePopUp());
    //                //    printDebug1();

    //                //}
    //               // Debug.Log(LoadSectorStocks);
    //            }
    //            if (Counter.text == "0") //Line added on 13th Aug 2023 for ensuring Running counter does not trigger before Counter.
    //            {
    //                GetStocksData(); 
    //            }
    //            else
    //            {
    //                RunningCounter.text = (urlfeed + 1).ToString() + "  /";
    //            }
    //            searchbutton = false;
    //            printDebug1();
                
    //        }

    //        //DateTime after = DateTime.Now;
    //        //TimeSpan duration = after.Subtract(before);
    //        //Debug.Log("Duration in milliseconds: " + outputstock.ToString() + "--" +duration.Milliseconds);

    //        //Debug.Log(outputstock.ToString() + " Mohit Category -- " + StockCategory.ToString() + " , Ankush Category -- " + APIStockcategory.ToString());
    //    }
    //}

    public void StocksData()
        {
            StockSymbolSector.Add(StockSymbol);
        }

        public void printDebug1()
        {
        // Debug.Log("These are the Stock Symbols in; " + StockSymbolSector[urlfeed].ToString() + " - " + string.Join(",", StockSymbolSector));
        
        if (StockSymbolSector.Count - urlfeed == 1)
            {
            refreshbtn.enabled = true;
            return;
            }
            else
            {
                urlfeed += 1;
            
            GetStocksData();
        }
        
        }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);
        
        PopUp.gameObject.SetActive(false);
    }
}

