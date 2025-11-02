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
using System.Globalization;

public class Stocks_Accordion_Call_API_For_DB_READ1 : MonoBehaviour
{
    private string url;
    public string StockForSector = "";

    public Text Header;
    public Button refreshbtn;
    public Image[] GainLossArrw;
    public Text AggregateGainloss;
    public double GainorLoss;
    public Text Avggainorloss;
    //public string SectorsinExch = "";
    //public List<string> SectorInExchg;

    public List<string> StockSymbolSector = new List<string>();
    //public List<string> BullishVBullStocks = new List<string>();
    public string StockSymbol = "";
    public int urlfeed;
    public int mystockscount;
    private string url1;

    public RectTransform ParentPanel;
    public GameObject StocknameAndData;
    public GameObject Stockname;

    //public AutoMarketWatchList autowatchlist;
    //RectTransform ParentPanel_1;
    //public GameObject StocknameAndDataforwatch;
    //public GameObject Stocknamewatch;

    public string Stock;
    public string outputstock;

    public string DailyState;
    public string WeeklyState;
    public string MonthlyState;

    public string FinalStockState;
    public string StocksDeclining;
    public string readmoreURL;
    public string StockCategory;
    public AccordionElement accordionelement;
    public TwilioMessaging msgbody;
    table_MyStocks mlocationDb;

    public List<MyStckWatchEntity> searchresultsforhere;
    public List<MyStckWatchEntity> myList1;
    public int totatlrecords;
    public int maxpages;
    public Text TotalPrice;
    public Double totalprice;
    public string outputofsearchresult;
    public string searchresults;
    List<FromDatabase> fromdbList = new List<FromDatabase>();
    int n;
    List<WhatsAppMyWatchResults> forwhatsapp = new List<WhatsAppMyWatchResults>();

    //public GameObject Stockname;
    //public GameObject StockPrefab;
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;

    float time = 0.0f;
    public Color StarColor;

    public TextMeshProUGUI DateforStocks;
    public TextMeshProUGUI Datetouseforstocks;
    public TextMeshProUGUI Datetouseforshowing;
    public TextMeshProUGUI Datetouseforshowingonmywatch;
    private string dateforfetchin;
    public ManageWatchList StockPerfData;

    public Text stock;
    public Text stocksym;

    void Start()
    {
        //msgbody = new TwilioMessaging();
        GainorLoss = 0;
        urlfeed = 0;
        mystockscount = 0;
        //errorpopup.gameObject.SetActive(false);
        time = 1.5f;
        mlocationDb = new table_MyStocks();
        using var connection = mlocationDb.getConnection();
        mlocationDb.stockscallapi = this;
        //mlocationDb.close();
    }

    public void AddDays()
    {
        if (dateforfetchin == DateTime.Now.ToString("yyyy-MM-dd"))
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops! Already at the latest date.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            DateTime date = DateTime.Parse(dateforfetchin).AddDays(1);
            Datetouseforshowingonmywatch.text = date.ToString("ddd, dd MMM yyy");
            dateforfetchin = date.ToString("yyyy-MM-dd");
            RefreshData();
        }

    }

    public void SubtractDays()
    {
        DateTime date = DateTime.Parse(dateforfetchin).AddDays(-1);
        Datetouseforshowingonmywatch.text = date.ToString("ddd, dd MMM yyy");
        dateforfetchin = date.ToString("yyyy-MM-dd");
        RefreshData();
    }

    public void Loadmystocks()

    {
        dateforfetchin = DateforStocks.text.ToString();
        Datetouseforshowingonmywatch.text = Datetouseforshowing.text;
        RefreshData();
    }

    public void RefreshData()
    {
        
        urlfeed = 0;
        mystockscount = 0;
        GainorLoss = 0;
        totalprice = 0;
        AggregateGainloss.text = "0";
        Avggainorloss.text = "0";
        TotalPrice.text = "";
        AggregateGainloss.color = Color.white;
        Avggainorloss.color = Color.white;
        StockSymbolSector.Clear();
        fromdbList.Clear();
        forwhatsapp.Clear();
        //DateforStocks.text = GameObject.FindGameObjectWithTag("simulatedate").GetComponent<TextMeshProUGUI>().text;
        //DateforStocks = GameObject.FindGameObjectWithTag("simulatedate").GetComponent<TextMeshProUGUI>().text.ToString();

        //mlocationDb.deleteAllData();

        GetMyStocklist();
    }

     public void GetMyStocklist()
    {
        refreshbtn.enabled = false;

        for (int i = 0; i < ParentPanel.transform.childCount; ++i) {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }
        mlocationDb.stockscallapi = this;
        
        using System.Data.IDataReader reader = mlocationDb.featchalldata();
        List<MyStckWatchEntity> myList = new List<MyStckWatchEntity>();

        while (reader.Read())
        {
            MyStckWatchEntity entity = new MyStckWatchEntity(int.Parse(reader[0].ToString()), reader[1].ToString().Trim(), reader[2].ToString().Trim(),
            reader[3].ToString().Trim(),
            reader[4].ToString().Trim(),
            reader[5].ToString().Trim(),
            double.Parse(reader[6].ToString()),
            reader[7].ToString().Trim(),
            reader[8].ToString().Trim());

            //Debug.Log("Stock Code: " + entity._stocksym);
            myList.Add(entity);

            var output1 = JsonUtility.ToJson(entity, true);
            //Debug.Log(output1);
            outputofsearchresult = output1.ToString();
        }
        reader.Dispose();
        mlocationDb.close();
        searchresults = JsonUtility.ToJson(myList, true);

        Stocks_Accordion_Call_API_For_DB_READ1 stockscallapi = new Stocks_Accordion_Call_API_For_DB_READ1();
        stockscallapi.StockSymbolSector = new List<string>();
        mystockscount = myList.Count;
        foreach (var x in myList)
        {
            FromDatabase DBstockDetails = new FromDatabase();

            DBstockDetails.id = x._id;
            DBstockDetails.stocksym = x._stocksym.ToString().Trim();
            DBstockDetails.price = x._closingprice.ToString().Trim();
            DBstockDetails.date = x._dateadd.ToString().Trim();
            totalprice += (x._closingprice);
            StockSymbolSector.Add(DBstockDetails.stocksym);
            fromdbList.Add(DBstockDetails);
        }
        TotalPrice.text = Convert.ToString(totalprice);
        GetStocksData(fromdbList);
        //watchlistread = new MyWatchlistRead();
        //watchlistread.searchresultsforhere = myList;
        //watchlistread.queryresults();

    }

    
    public void GetStocksData(List<FromDatabase> fromdbList)
    {
        if (StockSymbolSector.Count > urlfeed)
        {
            // StockSymbolSector.Clear();
            url1 = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=" + StockSymbolSector[urlfeed].Replace("&", "%26") + "&date="+ dateforfetchin + "&periods=D-6,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_10-D,sma_20-D,sma_50-D,macd,macdsignal,macdhist,ema_5-D,ema_9-D,ema_26-D,volume,sma_100,sma_200,category"; //http://50.28.67.53:1015/api/chartdatabyquery?symbols=APARINDS.NSE&periods=D-3,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_50-D"
            outputstock = StockSymbolSector[urlfeed];
            StartCoroutine(SectorResponserequest1(fromdbList));
        }
    }

    public IEnumerator SectorResponserequest1(List<FromDatabase> fromdbList)
    {
        UnityWebRequest request = UnityWebRequest.Get(url1);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            //CLOSED Debug.Log(request.error + "There was a error");
            refreshbtn.enabled = true;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops!! Please check your internet connection or try after sometime.";
            StartCoroutine(HidePopUp());
        }
        else
        {
            var stockdata = JsonConvert.DeserializeObject<StocksmainClass>(request.downloadHandler.text);
            //Debug.Log(stockdata.ToString());

            
            
            foreach (string sym in outputstock.Split(','))
            {
                try
                {
                    // Debug.Log("These are the Symbols requested; " + outputstock.ToString());
                    FinalStockState = "";
                StocksDeclining = "";
                StockCategory = "";
                readmoreURL = "";
                string urlexchange = sym.Split('.')[1];// Keep the ".NSE" from sym
                string urlsym = sym.Split('.')[0];// REMOVE the ".NSE" from the sym
                //readmoreURL = "https://www.tradingview.com/chart/?symbol=" + urlexchange + ":" + "ABMINTLLTD";
                readmoreURL = "https://www.tradingview.com/chart/?symbol="+ urlexchange + ":"+ urlsym.Replace("&", "_");
                double gainloss = 0;
                double oldPrice = 0;
                List<StocksAPI_Call> DailyData = stockdata.data.Where(l => l.period == "Daily" && l.symbol == sym).ToList();
                List<StocksAPI_Call> WeeklyData = stockdata.data.Where(l => l.period == "W" && l.symbol == sym).ToList();
                List<StocksAPI_Call> MonthlyData = stockdata.data.Where(l => l.period == "M" && l.symbol == sym).ToList();
                n = DailyData[0].data.Count;
                int w = WeeklyData[0].data.Count;
                int m = MonthlyData[0].data.Count;

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


                var SMA_1 = DailyData[0].data[n - 1].sma_50;
                var SMA_2 = DailyData[0].data[n - 2].sma_50;

                double sma_20_1 = DailyData[0].data[n - 1].sma_20;
                double sma_20_2 = DailyData[0].data[n - 2].sma_20;

                double sma_10_1 = DailyData[0].data[n - 1].sma_10;
                double sma_10_2 = DailyData[0].data[n - 2].sma_10;
                double sma_10_3 = DailyData[0].data[n - 3].sma_10;

                    double sma_50_1 = DailyData[0].data[n - 1].sma_50;
                    double sma_50_2 = DailyData[0].data[n - 2].sma_50;

                    double sma_100_1 = DailyData[0].data[n - 1].sma_100;
                    double sma_100_2 = DailyData[0].data[n - 2].sma_100;
                    double sma_100_3 = DailyData[0].data[n - 3].sma_100;

                    double sma_200_1 = DailyData[0].data[n - 1].sma_200;
                    double sma_200_2 = DailyData[0].data[n - 2].sma_200;
                    double sma_200_3 = DailyData[0].data[n - 3].sma_200;
                    
                    double e5_1 = DailyData[0].data[n - 1].ema_5;
                double e5_2 = DailyData[0].data[n - 2].ema_5;
                double e5_3 = DailyData[0].data[n - 3].ema_5;

                double e9_1 = DailyData[0].data[n - 1].ema_9;
                double e9_2 = DailyData[0].data[n - 2].ema_9;
                double e9_3 = DailyData[0].data[n - 3].ema_9;

                double e26_1 = DailyData[0].data[n - 1].ema_26;
                double e26_2 = DailyData[0].data[n - 2].ema_26;

                double open = DailyData[0].data[n - 1].open;
                double close = DailyData[0].data[n - 1].close;//adj_close changed to HIGH to subtract old DB price from the latest HIGH to see the gain or loss based on the highest value achieved.

                double macd_1 = DailyData[0].data[n - 1].macd;
                double macdsignal_1 = DailyData[0].data[n - 1].macdsignal;
                double macdhist_1 = DailyData[0].data[n - 1].macdhist;

                double macd_2 = DailyData[0].data[n - 2].macd;
                double macdsignal_2 = DailyData[0].data[n - 2].macdsignal;
                double macdhist_2 = DailyData[0].data[n - 2].macdhist;

                double macd_3 = DailyData[0].data[n - 3].macd;
                double macdsignal_3 = DailyData[0].data[n - 3].macdsignal;
                double macdhist_3 = DailyData[0].data[n - 3].macdhist;

                double macd_4 = DailyData[0].data[n - 4].macd;
                double macdsignal_4 = DailyData[0].data[n - 4].macdsignal;
                double macdhist_4 = DailyData[0].data[n - 4].macdhist;

                double V_1 = DailyData[0].data[n - 1].volume;
                double V_2 = DailyData[0].data[n - 2].volume;
                double V_3 = DailyData[0].data[n - 3].volume;

                    //daily data rsi values
                    double d1 = DailyData[0].data[n - 1].rsi;
                    double d2 = DailyData[0].data[n - 2].rsi;
                    double d3 = DailyData[0].data[n - 3].rsi;

                    //Weekly data rsi values
                    double w1 = WeeklyData[0].data[w - 1].rsi;
                    double w2 = WeeklyData[0].data[w - 2].rsi;
                    double w3 = WeeklyData[0].data[w - 3].rsi;

                    //Monthly data rsi values
                    double m1 = MonthlyData[0].data[m - 1].rsi;
                    double m2 = MonthlyData[0].data[m - 2].rsi;
                    double m3 = MonthlyData[0].data[m - 3].rsi;

                    var candleimagecolor = Color.grey;
                float rotate = 0;

                if (open > close)
                {
                    candleimagecolor = Color.red;
                    rotate = 180;
                }

                else if (open < close)
                {
                    candleimagecolor = Color.green;
                    rotate = 0;
                }

                    if (n > 2 && w > 2 && m > 2) // ((WeeklyData[0].data[n - 3].rsi != 0) && (MonthlyData[0].data[n - 3].rsi != 0)) This is to state if the RSI data is insufficient or COunt of Daily, Weekly, Monthly is Less than 3 then the data is of no use
                    {
                        if (((DailyData[0].data[n - 1].sma_100 - DailyData[0].data[n - 2].sma_100) > 0)) //COMPARING SMA_100 for latest two days to see if upwards, this needs to further done in slope value.
                        {
                            if (w1 > 45 && m1 > 55) // This is important for 5 star Strategy & then check the status of the Daily RSI to see forming support and then take the call.

                            {
                                if ((((d1 > d2) && ((((e9_1 - e9_2) / e9_2) * 100) > 0.4) && //REMOVED d1 > 40 on 26th FEB 2023 to check if better results, found this had no use
                                ((macd_1 > macdsignal_1) && macdhist_1 > macdhist_2) && ((macd_2 > macdsignal_2) &&
                                macdhist_2 > macdhist_3) && ((((e5_1 - e5_2) / e5_2) * 100) > 0.35) &&
                                ((((sma_20_1 - sma_20_2) / sma_20_2) * 100) > 0.1) && V_1 > 100000 && open < close) ||

                                ((d1 > d2) && ((((e9_1 - e9_2) / e9_2) * 100) > 0.4) && //REMOVED d1 > 40 on 26th FEB 2023 to check if better results, found this had no use
                                ((macd_2 > macd_3 && macd_2 > macdsignal_2 && macd_3 < macdsignal_3 && macdsignal_2 > macdsignal_3) && macdhist_2 > macdhist_3 && ((sma_10_1 - sma_10_2) > sma_10_2 - sma_10_3))
                                && ((((e5_1 - e5_2) / e5_2) * 100) > 0.35) &&
                                ((((sma_20_1 - sma_20_2) / sma_20_2) * 100) > 0.1))) && V_1 > 100000 && open < close)

                                //&& ((((sma_10_1 - sma_10_2) / sma_10_2) * 100) > 0.5)

                                {
                                    if (macd_3 < 0) // Basically if the crossover happened under the ZERO Line..
                                    {
                                        //CLOSED Debug.Log(sym + "-> VERY BULLISH ABOVE CONDITION WITH HEAVY RISING TREND");
                                        StockCategory = "SUPER TREND"; //GOLD STAR

                                        //Debug.Log(sym + "- Good Volume; " + V_1 + "---" + V_2 + "---" + V_2 + "---" + V_3);
                                    }

                                    else
                                    {
                                        //CLOSED Debug.Log(sym + "Support, Daily RSI");
                                        StockCategory = "VERY BULLISH"; //GREEN STAR
                                        FinalStockState = sym + ": VERY BULLISH" + "\n";
                                        // Debug.Log(sym + ": VERY BULLISH with RSI = " + d1);
                                        //Debug.Log(sym + "- Good Volume; " + V_1 + "---" + V_2 + "---" + V_2 + "---" + V_3);
                                    }
                                }

                                else
                                {
                                    StockCategory = "WATCH OUT"; //YELLOW DOT
                                    FinalStockState = sym + ": WATCH OUT" + "\n";

                                }

                                //OLD FORMULAE; COMMENTED on 05th DEC to change to RSI, EMA 9 & SMA 10 & 20 and do the BACK testing to check which one works better.

                                //=============================================================================

                                //    if ((d1 > 60 && d1 <= 78) && ((d2 >= 59 && d3 > d2 && d1 > d2) || (d1 > 60 && d1 > d2 && d2 > d3) || (d1 > 60 && d2 <= 60)) && ((((e5_1 - e5_2) / e5_2) * 100) > 0.5) && sma_20_1 > sma_20_2) //Support at 60's or rising RSI or or 60 crossover & EMA 5 change GREATER 0.5% in a day & SMA 20 Moving Upwards.
                                //    {
                                //        //CLOSED Debug.Log(sym + "Support, Daily RSI");
                                //        StockCategory = "VERY BULLISH";
                                //        FinalStockState = sym + ": VERY BULLISH" + "\n";
                                //        //Debug.Log(sym + ": VERY BULLISH");

                                //        if ((e9_1 > e9_2 && e9_1 > sma_20_1 && e9_2 < sma_20_2 && sma_20_1 > sma_20_2) || (((((e9_1 - e9_2) / e9_2) * 100) > 1) && ((((sma_20_1 - sma_20_2) / sma_20_2) * 100) > 1)))
                                //        {
                                //            //CLOSED Debug.Log(sym + "-> VERY BULLISH EMA & SMA CROSSOVER OR  SUPER STAR TREND");
                                //            StockCategory = "EMA CROSS";
                                //        }

                                //    }

                                //    else if (((d1 > 78) && ((d2 >= 59 && d3 > d2 && d1 > d2) || (d1 > 60 && d1 > d2 && d2 > d3) || (d1 > 60 && d2 <= 60)) && ((((e5_1 - e5_2) / e5_2) * 100) > 0.5) && sma_20_1 > sma_20_2) ||
                                //            ((d1 > 60) && ((d2 >= 59 && d3 > d2 && d1 > d2) || (d1 > 60 && d1 > d2 && d2 > d3) || (d1 > 60 && d2 <= 60)) && (((((e5_1 - e5_2) / e5_2) * 100) < 0.5) || sma_20_1 < sma_20_2))) //Even if SMA & EMA are indicating but the RSI is > 78 or Support at 60's or rising RSI or or 60 crossover BUT EMA 5 change Less Than 0.5% in a day & SMA 20 Moving Downwards.
                                //    {
                                //        //CLOSED Debug.Log(sym + "Support, Daily RSI");
                                //        StockCategory = "WATCH OUT";
                                //        FinalStockState = sym + ": WATCH OUT" + "\n";
                                //        //if(((((e5_1 - e5_2) / e5_2) * 100) < 0.5) || sma_20_1 < sma_20_2)
                                //        //{
                                //        //    //CLOSED Debug.Log(sym + "-> WATCH OUT VERY BULLISH BUT becoz of EMA 5 or SMA 20");

                                //        //}
                                //    }

                                //    else if ((((d2 >= 40 && d2 <= 59) && d3 > d2 && d1 > d2) || (d3 > 40 && d1 > d2 && d2 > d3) || (d3 > 30 && d2 > 40 && d1 > d2)) && ((((e5_1 - e5_2) / e5_2) * 100) > 0.5) && sma_20_1 > sma_20_2) //RISING DAILY RSI & EMA 5 change GREATER Than 0.5% in a day & SMA 20 Moving Upwards.
                                //    {
                                //        StockCategory = "BULLISH";
                                //        FinalStockState = sym + ": BULLISH" + "\n";
                                //        if ((e9_1 > e9_2 && e9_1 > sma_20_1 && e9_2 < sma_20_2 && sma_20_1 > sma_20_2) || (((((e9_1 - e9_2) / e9_2) * 100) > 1) && ((((sma_20_1 - sma_20_2) / sma_20_2) * 100) > 1)))
                                //        {
                                //            //CLOSED Debug.Log(sym + "-> BULLISH EMA & SMA CROSSOVER OR  SUPER STAR TREND BECOZ of DAILY RISE of more than 1 % on EMA 9 & SMA 20");
                                //            StockCategory = "EMA CROSS";
                                //        }
                                //    }
                                //    else if ((((d2 >= 40 && d2 <= 59) && d3 > d2 && d1 > d2) || (d3 > 40 && d1 > d2 && d2 > d3) || (d3 > 30 && d2 > 40 && d1 > d2)) && (((((e5_1 - e5_2) / e5_2) * 100) < 0.5) || sma_20_1 < sma_20_2)) //RISING DAILY RSI BUT EMA 5 change Less Than 0.5% in a day & SMA 20 Moving Downwards.
                                //    {
                                //        //CLOSED Debug.Log(sym + "Support, Daily RSI");
                                //        StockCategory = "WATCH OUT";
                                //        FinalStockState = sym + ": WATCH OUT" + "\n";

                                //        //if (((((e5_1 - e5_2) / e5_2) * 100) < 0.5) || sma_20_1 < sma_20_2)
                                //        //{
                                //        //    //CLOSED Debug.Log(sym + "-> WATCH OUT BULLISH BUT becoz of EMA 5 or SMA 20");

                                //        //}
                                //    }
                                //    else if (((d1 < 60 && d1 < d2) || d1 < d2) || ((((e5_1 - e5_2) / e5_2) * 100) < 0.5))
                                //    {
                                //        // 222222 
                                //        //CLOSED Debug.Log(sym + "Declining, Daily RSI");
                                //        StockCategory = "WATCH OUT";
                                //        FinalStockState = sym + ": WATCH OUT" + "\n";

                                //    }

                                ////Checking for CHARU's ROCKSTARS
                                ////        if ((e5_1 > e9_1 && e5_1 > e26_1) && (e5_2 < e9_2 && e5_2 < e26_2))

                                ////    {
                                ////        CLOSED Debug.Log("I am Charu's RockStar" + sym);
                                ////    }

                                //=============================================================================

                            }

                            else if (V_1 > 100000 && V_1 > V_2 * 1.75 && open < close && macdhist_1 > 0 && (macdhist_1 > macdhist_2) && (macd_1 > macdsignal_1) && (sma_10_2 < sma_10_3 && sma_10_1 > sma_10_2))
                            {
                                //CLOSED Debug.Log(sym + "Support, Daily RSI");
                                StockCategory = "BULLISH"; //GREEN DOT
                                FinalStockState = sym + ": BULLISH" + "\n";
                                //Debug.Log(sym + ": BULLISH with RSI = " + d1);
                                //Debug.Log(sym + "- Volume DOUBLED OR MORE; " + V_1 + "---" + V_2 + "---" + V_2 + "---" + V_3);
                            }
                            else if (w1 <= 45 || m1 <= 55)
                            {

                                StocksDeclining = sym + "\n";
                                StockCategory = "RANGING";

                            }

                            if ((sma_10_1 > sma_10_2 && sma_50_1 > sma_50_2 && sma_100_1 > sma_100_2 && sma_200_1 > sma_200_2) && (sma_10_1 > sma_50_1 && sma_50_1 > sma_100_1 && sma_100_1 > sma_200_1)
                            && V_1 > 100000 && V_1 > V_2 * 1.75 && open < close && macdhist_1 > 0 && (macdhist_1 > macdhist_2) && (macd_1 > macdsignal_1)
                             )// 
                            {
                                StockCategory = "SUPER TREND"; //GOLD STAR
                                //Debug.Log(sym + ": BULLISH with RSI = " + d1);
                                //Debug.Log("THIS ONE IS ON SUPER TREND ----> " + sym);
                            }
                            // COMMENTED ON 06th June 2022 FOR CORRECTING THE STRATEGY AND REPLACED WITH ABOVE MODIFICATIONS

                            //CODE FROM HERE is backup at location; D:\Documents_1\Stocks\C_Scharp_Script_Formula - File name; All_Above_60_old_Backup.txt

                            // COMMENTED ON 06th June 2022 FOR CORRECTING THE STRATEGY

                            //Stock = sym.ToString();

                            Stockname = Instantiate(StocknameAndData);
                        Stockname.transform.SetParent(ParentPanel, false);

                        Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
                        Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();
                        Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
                        //Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
                        Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
                        Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = StockCategory.ToString();
                        Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
                        Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
                        //double gainloss = DailyData[0].data[n - 1].adj_close - DailyData[0].data[n - 2].adj_close;
                        Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                        WhatsAppMyWatchResults forwhatsapp1 = new WhatsAppMyWatchResults();

                        forwhatsapp1.date = DailyData[0].data[n - 1].date.ToString();
                        forwhatsapp1.stocksym = sym.ToString();
                        forwhatsapp1.open = DailyData[0].data[n - 1].open.ToString().Trim();
                        forwhatsapp1.close = DailyData[0].data[n - 1].close.ToString().Trim();
                        forwhatsapp1.high = DailyData[0].data[n - 1].high.ToString().Trim();
                        forwhatsapp.Add(forwhatsapp1);
                        

                        if (StockCategory == "VERY BULLISH" || StockCategory == "SUPER TREND") //If the Stock is Very Bullish, it instantiates a prefab into the Auto Watch List from here and also Next Block for BULLISh. 
                        {
                            if (StockCategory == "VERY BULLISH")
                            {
                                StarColor = Color.green;
                            }

                            else if (StockCategory == "SUPER TREND")
                            {
                                ColorUtility.TryParseHtmlString("#D4AF37", out StarColor);
                            }
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[0];
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = StarColor;


                        }
                        else if (StockCategory == "BULLISH")
                        {
                                if (StockCategory == "BULLISH")
                                {
                                    StarColor = Color.green;
                                }

                                else if (StockCategory == "SUPER TREND")
                                {
                                    ColorUtility.TryParseHtmlString("#D4AF37", out StarColor);
                                }

                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[1];
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = StarColor;

                        }
                        else if (StockCategory == "RANGING")
                        {
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[2];
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = Color.grey;
                        }
                        else if (StockCategory == "WATCH OUT")
                        {
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[1];
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = Color.yellow;
                        }

                        else if (StockCategory == "DECLINING")
                        {
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[1];
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = Color.red;
                        }

                        else //THESE ARE THE STOCKS WITH WEEK AND MONTH > 60, BUT DAILY ARE NOT YET MEETING THE 40 RSI CRITERIA AND NOT YET WORTH LOOKING AT..... 
                        {
                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[3];

                            Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = Color.grey;
                        }

                        List<string> oldReccord = fromdbList.Where(l => l.stocksym == sym).Select(l => l.price).ToList();

                        if (oldReccord.Count > 0)
                        {
                            oldPrice = Convert.ToDouble(oldReccord[0]);
                            Stockname.transform.Find("StockAllocation").GetComponentInChildren<Text>().text = Convert.ToString(((oldPrice / totalprice) * 100).ToString("0.00") + " %");
                        }

                        List<int> oldReccord1 = fromdbList.Where(l => l.stocksym == sym).Select(l => l.id).ToList();
                        int symbolid = 0;
                        if (oldReccord1.Count > 0)
                        {
                            symbolid = oldReccord1[0];
                            Stockname.transform.Find("ID").GetComponentInChildren<Text>().text = symbolid.ToString();
                        }

                        List<string> oldReccord2 = fromdbList.Where(l => l.stocksym == sym).Select(l => l.date).ToList();
                        string olddate = "";
                        if (oldReccord2.Count > 0)
                        {
                            olddate = oldReccord2[0].ToString();

                        }

                        gainloss = DailyData[0].data[n - 1].high - oldPrice;
                        if (gainloss ==0)
                        {
                            mystockscount -=1;
                        }
                        //else
                        //{
                        //    mystockscount += 1;
                        //}

                        GainorLoss += (gainloss / oldPrice) * 100;
                        //MOVED AGGREGATES FROM HERE TO END OUT OF ALL BLOCKS..
                        // Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = (oldPrice- DailyData[0].data[n - 1].adj_close).ToString();
                        Stockname.transform.Find("DateAddedWatch").GetComponentInChildren<Text>().text = olddate.ToString();
                        Stockname.transform.Find("WatchAddAdjClose").GetComponentInChildren<Text>().text = oldPrice.ToString();
                        Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = (DailyData[0].data[n - 1].high).ToString();
                        if (gainloss >= 0)
                        {
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.green;
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        }
                        else if (gainloss < 0)
                        {
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.red;
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        }

                        //catch (Exception ex)
                        //{
                        //    Debug.Log("There was a error in catch value of n was "  + n.ToString());
                        //}

                    }
                    else if ((DailyData[0].data[n - 1].sma_50 - DailyData[0].data[n - 2].sma_50) <= 0)
                    {

                        StocksDeclining = sym + "\n";
                        StockCategory = "DECLINING";
                        List<string> oldReccord = fromdbList.Where(l => l.stocksym == sym).Select(l => l.price).ToList();
                        List<int> oldReccord1 = fromdbList.Where(l => l.stocksym == sym).Select(l => l.id).ToList();
                        List<string> oldReccord2 = fromdbList.Where(l => l.stocksym == sym).Select(l => l.date).ToList();

                        if (oldReccord.Count > 0)
                        {
                            oldPrice = Convert.ToDouble(oldReccord[0]);
                            
                        }
                                               
                        string olddate = "";
                        if (oldReccord2.Count > 0)
                        {
                            olddate = oldReccord2[0].ToString();

                        }
                        int symbolid = 0;

                        //Debug.Log(sym + " stock Daily SMA - 50 is on a decline.");
                        // stocksresultmethod();
                        //CLOSED Debug.Log("Declining Averages: " + sym);
                        gainloss = DailyData[0].data[n - 1].high - oldPrice;

                        if (gainloss == 0)
                        {
                            mystockscount -= 1;
                        }
                        //else
                        //{
                        //    mystockscount += 1;
                        //}

                        GainorLoss += (gainloss / oldPrice) * 100;
                        Stockname = Instantiate(StocknameAndData);
                        Stockname.transform.SetParent(ParentPanel, false);

                        Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
                        Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString();// "Declining Averages";
                        Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
                        //Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
                        Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
                        Stockname.transform.Find("DateAddedWatch").GetComponentInChildren<Text>().text = olddate.ToString();
                        Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = "DECLINING";
                        Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].high.ToString();
                        Stockname.transform.Find("WatchAddAdjClose").GetComponentInChildren<Text>().text = oldPrice.ToString();
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)";
                        Stockname.transform.Find("StockAllocation").GetComponentInChildren<Text>().text = Convert.ToString(((oldPrice / totalprice) * 100).ToString("0.00") + " %");
                        Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[1];
                        Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
                        Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
                        Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                        Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = Color.red;
                        
                        if (oldReccord1.Count > 0)
                        {
                            symbolid = oldReccord1[0];
                            Stockname.transform.Find("ID").GetComponentInChildren<Text>().text = symbolid.ToString();
                        }
                        if (gainloss >= 0)
                        {
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.green;
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        }
                        else if (gainloss < 0)
                        {
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.red;
                            Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                        }

                        WhatsAppMyWatchResults forwhatsapp2 = new WhatsAppMyWatchResults();

                        forwhatsapp2.date = DailyData[0].data[n - 1].date.ToString();
                        forwhatsapp2.stocksym = sym.ToString();
                        forwhatsapp2.open = DailyData[0].data[n - 1].open.ToString().Trim();
                        forwhatsapp2.close = DailyData[0].data[n - 1].close.ToString().Trim();
                        forwhatsapp2.high = DailyData[0].data[n - 1].high.ToString().Trim();
                        forwhatsapp.Add(forwhatsapp2);

                    }                   
                }
                else
                {
                    //CLOSED Debug.Log("The RSI Data is unavaible for one of the periods: " + sym);

                    List<string> oldReccord = fromdbList.Where(l => l.stocksym == sym).Select(l => l.price).ToList();
                    List<int> oldReccord1 = fromdbList.Where(l => l.stocksym == sym).Select(l => l.id).ToList();
                    List<string> oldReccord2 = fromdbList.Where(l => l.stocksym == sym).Select(l => l.date).ToList();

                    if (oldReccord.Count > 0)
                    {
                        oldPrice = Convert.ToDouble(oldReccord[0]);

                    }
                    string olddate = "";
                    if (oldReccord2.Count > 0)
                    {
                        olddate = oldReccord2[0].ToString();

                    }


                    int symbolid = 0;
                    gainloss = DailyData[0].data[n - 1].high - oldPrice;
                    if (gainloss == 0)
                    {
                        mystockscount -= 1;
                    }
                    //else
                    //{
                    //    mystockscount += 1;
                    //}

                    GainorLoss += (gainloss / oldPrice) * 100;
                    Stockname = Instantiate(StocknameAndData);
                    Stockname.transform.SetParent(ParentPanel, false);

                    Stockname.transform.GetComponentInChildren<Text>().text = sym.ToString();
                    Stockname.transform.Find("StockSymbl").GetComponentInChildren<Text>().text = sym.ToString(); //"Insufficient Data"
                    Stockname.transform.Find("StockName").GetComponentInChildren<Text>().text = stockname.ToString();
                    //Stockname.transform.Find("Industry").GetComponentInChildren<Text>().text = Header.text.ToString();
                    Stockname.transform.Find("Date").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].date.ToString();
                    Stockname.transform.Find("DateAddedWatch").GetComponentInChildren<Text>().text = olddate.ToString();
                    Stockname.transform.Find("Status").GetComponentInChildren<Text>().text = "INSUFFICIENT DATA";
                    Stockname.transform.Find("LastAdjClose").GetComponentInChildren<Text>().text = DailyData[0].data[n - 1].high.ToString();
                    Stockname.transform.Find("WatchAddAdjClose").GetComponentInChildren<Text>().text = oldPrice.ToString();
                    Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)";
                    Stockname.transform.Find("StockAllocation").GetComponentInChildren<Text>().text = Convert.ToString(((oldPrice / totalprice) * 100).ToString("0.00") + " %");
                    Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().sprite = Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Status_Images>().statusimage[3];
                    Stockname.transform.Find("Date/CandleImg").GetComponentInChildren<Image>().color = candleimagecolor;
                    Stockname.transform.Find("Date/CandleImg").Rotate(0, 0, rotate);
                    Stockname.transform.Find("StockChartBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                    Stockname.transform.Find("AddtoMyWtchBtn").GetComponent<Image>().color = Color.grey;

                    if (oldReccord1.Count > 0)
                    {
                        symbolid = oldReccord1[0];
                        Stockname.transform.Find("ID").GetComponentInChildren<Text>().text = symbolid.ToString();
                    }

                    if (gainloss >= 0)
                    {
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.green;
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                    }
                    else if (gainloss < 0)
                    {
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().color = Color.red;
                        Stockname.transform.Find("ChangeFmPreviousCls").GetComponentInChildren<Text>().text = gainloss.ToString("0.00") + " (" + ((gainloss / oldPrice) * 100).ToString("0.00") + "%)"; //+0.04(+0.11%)
                    }
                    WhatsAppMyWatchResults forwhatsapp3 = new WhatsAppMyWatchResults();

                    forwhatsapp3.date = DailyData[0].data[n - 1].date.ToString();
                    forwhatsapp3.stocksym = sym.ToString();
                    forwhatsapp3.open = DailyData[0].data[n - 1].open.ToString().Trim();
                    forwhatsapp3.close = DailyData[0].data[n - 1].close.ToString().Trim();
                    forwhatsapp3.high = DailyData[0].data[n - 1].high.ToString().Trim();
                    forwhatsapp.Add(forwhatsapp3);
                }
                printDebug1();
                AggregateGainloss.text = Math.Round(GainorLoss, 1).ToString() + " %";

                if (mystockscount>0)
                {
                    Avggainorloss.text = Math.Round((Math.Round(GainorLoss, 1) / (mystockscount)), 1).ToString() + " %";

                    //CLOSED Debug.Log("Average Gain/Loss" + Avggainorloss.text);
                }
                else
                {
                    Avggainorloss.text = "--";

                    //CLOSED Debug.Log("Average Gain/Loss" + Avggainorloss.text);
                }

                if (GainorLoss > 0)
                {
                    AggregateGainloss.GetComponent<Text>().color = Color.green;
                    Avggainorloss.GetComponent<Text>().color = Color.green;
                    GainLossArrw[0].enabled = true;
                    GainLossArrw[1].enabled = false;
                    GainLossArrw[2].enabled = false;
                }

                else if (GainorLoss < 0)
                {
                    AggregateGainloss.GetComponent<Text>().color = Color.red;
                    Avggainorloss.GetComponent<Text>().color = Color.red;
                    GainLossArrw[0].enabled = false;
                    GainLossArrw[1].enabled = true;
                    GainLossArrw[2].enabled = false;
                }

                else if (GainorLoss == 0)
                {
                    AggregateGainloss.GetComponent<Text>().color = Color.grey;
                    Avggainorloss.GetComponent<Text>().color = Color.grey;
                    GainLossArrw[0].enabled = false;
                    GainLossArrw[1].enabled = false;
                    GainLossArrw[2].enabled = true;
                }
                }
                catch (Exception err)
                {
                    // 3333333333 
                    Debug.Log("Error in  Symbol:" + sym);
                    
                }
                StockPerfData = new ManageWatchList();
                StockPerfData.stckname = stock.GetComponentInChildren<Text>();
                StockPerfData.symbol = stocksym.GetComponentInChildren<Text>();
                //StockPerfData.performanceTxt = PerformanceWMYTD.GetComponentInChildren<Text>();
                StockPerfData.stckname.text = sym.ToString();
                StockPerfData.symbol.text = sym.ToString();
                StockPerfData.FmStocksAPI = true;
                StockPerfData.url2 = "https://api.equityanalyze.com/api/yahoo/" + sym.ToString();
                StockPerfData.url1 = "https://news-headlines.tradingview.com/v2/headlines?client=chart&lang=en&symbol=NSE%3A" + sym.ToString();
            }
        }
    }
    public void StocksData()
    {
        StockSymbolSector.Add(StockSymbol);
    }

    public void printDebug1()
    {
        //Debug.Log("These are the Stock Symbols in; " + StockSymbolSector[urlfeed].ToString() + " - " + string.Join(",", StockSymbolSector));

        if (StockSymbolSector.Count - urlfeed == 1)
        {
            //urlfeed =0;
            refreshbtn.enabled = true;
            return;
        }
        else
        {
            urlfeed += 1;
            GetStocksData(fromdbList);
        }

    }

    public void MsgforTwilio()
    {
        string whatsappsmsbody;
        whatsappsmsbody = "";
        msgbody.body = "";

       if (forwhatsapp.Count != 0)
        {
            int watchstockcount = 0;
            foreach (var x in forwhatsapp)
            {
                //watchstockcount +=1;
                //whatsappsmsbody += x.stocksym.ToString() + ", " + Convert.ToDateTime(x.date.ToString()).ToLongDateString() + ", (O): " + x.open.ToString() + ", (C): " + x.close.ToString() + ", " + "\n" + "\n";
                whatsappsmsbody += "*" + x.stocksym.ToString()+ "*." + Convert.ToDateTime(x.date).ToString("ddd, dd MMM yyy")+ ",(O):" + x.open.ToString() + ",(C):" + x.close.ToString() + ",(H):" + x.high.ToString() + ".";
                //whatsappsmsbody += "TEST MESSAGE FOR *BOLD*.";

                //whatsappsmsbody += x.stocksym.ToString()+", " + "\n";
            }
            msgbody.body = "Your personal watchlist of " + StockSymbolSector.Count+ " stocks are " + whatsappsmsbody + ". Thank you.";

            //msgbody.body = "Dear Sahana Sabharwal welcome to EquityAnalyze, your login username; 9650006650 and password; $Uperm4n123_!as.";

            //msgbody.body = "Your personal watchlist of " + StockSymbolSector.Count + " stocks are " + whatsappsmsbody + ". Thank you.";
            //Your personal watchlist of {{1}} stocks are {{2}}. Thank you.
            //"Here's your personal watchlist of '" + StockSymbolSector.Count + "' items. " + whatsappsmsbody + ".";//"+whatsappsmsbody +"
            msgbody.SendSMS();
        }
       
        else
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Oops!! There are no stocks showing in your watchlist to send.";
            StartCoroutine(HidePopUp());
        }
        

    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);
        PopUpMsg.text = "";
        errorpopup.gameObject.SetActive(false);
       
    }
}