using DataBank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;

public class ManageWatchList : MonoBehaviour
{
    //table_MyStocks mlocationDb;

    int DBid;
    public Text symbol;
    public Text stckname;
    public Text Stockindustry;
    public Text dateofclosingprice;
    public Text dateinserting; //this is an extra useless field.
    public Text priceonadd;
    public Text perfstatus;
    public GameObject PopUp;
    public Text PopUpMsg;
    public VoiceController texttospeech;
    public string url1;
    public string url2;
    public float time = 0.0f; //Seconds to read the text

    public bool FmStocksAPI;
    public float performanceaggregate;
    public Text performanceTxt;
    public Text RecapTxt;
    Stocks_Accordion_Call_API Stocks_acc_Call_API;
    
    public TextMeshProUGUI Market_Cap_Data;
    public TextMeshProUGUI Revenue_Data;
    public TextMeshProUGUI Avg_10_Days_Data;
    public TextMeshProUGUI DividentDate_Data;
    public TextMeshProUGUI OneWkTxt;
    public TextMeshProUGUI OneMontxt;
    public TextMeshProUGUI threeMontxt;
    public TextMeshProUGUI SixMontxt;
    public TextMeshProUGUI OneYrtxt;
    public TextMeshProUGUI YTDtxt;
    public TextMeshProUGUI NewsData;
    public TextMeshProUGUI AboutData;
    public TextMeshProUGUI ESGScr;
    public TextMeshProUGUI Envscr;
    public TextMeshProUGUI Socialscr;
    public TextMeshProUGUI Govscr;


    string Newstitle;
    string description;

    public Button LoadPerfPanel;
    public RectTransform PerfPanel;
    public GameObject StockRecapParentOBJ;
    public GameObject PerfRecapPrefab;
    public GameObject PerfRecapObj;

    public bool loadbtnclicked;

    void OnEnable() //This automatically gets called as soon it senses this object is active, this was included to use the Performance text.
    {
        texttospeech = new VoiceController();
        SpeakingFunction();

    }

    void Start()
    {
        //systemconfigcode = new SystemDataConfig();
        //mlocationDb = new table_MyStocks();
       time = 1.0f;
        performanceaggregate = 0.0f;
        FmStocksAPI = false;
       
        PopUp.gameObject.SetActive(false);
        performanceTxt.text = "";
        RecapTxt.text = "";
        loadbtnclicked = false;
        url1 = "";
        url2 = "";
}
    
    public void InitiliaseDropTables()
    {
        table_MyStocks mlocationDb = new table_MyStocks();
        using var connection = mlocationDb.getConnection();
        mlocationDb.DropTable();
    }
    
    public void AddData()
    {
        table_MyStocks mlocationDb = new table_MyStocks();
        using var connection = mlocationDb.getConnection();
        //mlocationDb.deleteAllData();
        //mlocationDb.vacuumAllData();
        //mlocationDb.sqlite_sequence();
        mlocationDb.getDataByString1(symbol.text.ToString());
        if (mlocationDb.outputofsearchresult != null)
            
        {

          PopUp.gameObject.SetActive(true);
         
          PopUpMsg.color = Color.yellow;
          PopUpMsg.text = "Already in your watch list !!";
          StartCoroutine(HidePopUp());
            Debug.Log("This symbol " + symbol.text.ToString() + "Already added to your watch list.");
        }

        else
        {
            //Add Data
            mlocationDb.addData(new MyStckWatchEntity(DBid, symbol.text.ToString(), stckname.text.ToString(), Stockindustry.text.ToString(), dateofclosingprice.text.ToString(), dateofclosingprice.text.ToString(), Convert.ToDouble(priceonadd.text), perfstatus.text.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
            //mlocationDb.addData(new MyStckWatchEntity(1,"HDFC.NSE", "HDFC Bank Limited", "Real Estate", "2022-05-11","2022-05-12", 3230.456, "Very Bullish", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
            //mlocationDb.addData(new MyStckWatchEntity(2,"IRCTC.NSE", "IRCTC LTD", "Railways", "2022-05-11", "2022-05-05", 452.456, "Bullish", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

            PopUp.gameObject.SetActive(true);
           
            PopUpMsg.color = Color.white;
            PopUpMsg.text = "Added to your watch list.";
            StartCoroutine(HidePopUp());
            
            //mlocationDb.getLatestID();

        }


    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);
       
        PopUp.gameObject.SetActive(false);
    }

    public void loadperfbtnclick()
    {
        loadbtnclicked = true;

        PopUp.gameObject.SetActive(true);

        PopUpMsg.color = Color.white;
        PopUpMsg.text = "Fetching performance data.";
        StartCoroutine(HidePopUp());
        SpeakingFunction();
    }

    public void SpeakingFunction()
    {
        //Text symblcorct = Regex.Replace(symbol, "[\$\|\&]", " "); mahindra & mahindra replace ""&"" with "_"
        url1 = "https://news-headlines.tradingview.com/v2/headlines?client=chart&lang=en&symbol=NSE%3A" + symbol.text.Split('.')[0].Replace("-", "_").Replace("&", "_");
        FmStocksAPI = false;
        StartCoroutine(FetchRecapdata());
        
    }

    public IEnumerator FetchRecapdata()
    {
        
        UnityWebRequest request = UnityWebRequest.Get(url1);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            //CLOSED Debug.Log(request.error + "There was a error");
            PopUpMsg.text = "Oops!! please try again !!";
            PopUp.gameObject.SetActive(true);
            StartCoroutine(HidePopUp());
        }

        else
        {
            var Stocksrecapdata = JsonConvert.DeserializeObject<StockItem>(request.downloadHandler.text);
            Newstitle = "";
            if (Stocksrecapdata.items.Count > 0)
            {
                Newstitle = Stocksrecapdata.items[0].title.ToString();
            }
            try
            {
                if (Stocksrecapdata.items.Count == 0 || Newstitle == null || Newstitle == "")
                {
                //CLOSEDDebug.Log("Empty..");
                texttospeech.StartSpeaking("No news item found for " + stckname.text.ToString() + ". Performance overview. ");
                //Debug.Log("No news item found for " + stckname.text.ToString());
                NewsData.text = "No news item found for " + stckname.text.ToString();
                StockperformanceAPI();
            }
           
                else
            {
                    texttospeech.StartSpeaking(Newstitle + ". Performance overview. ");//GETTING RANDOM NULL ERROR ON THESE SOMETIME HERE; DNAMEDIA.NSE, KPITTECH.NSE, MAXVIL.NSE, PHOENIXLTD.NSE
                    NewsData.text = Newstitle.ToString();
                    //Debug.Log("Speak name;" + stckname.text.ToString()+ ""+Newstitle + ". Further let us review the performance of this Stock.");

                    StockperformanceAPI();

            }
            }
            catch (Exception err)
            {
                Debug.Log("ERROR IN NEWS TITLE --->" + Newstitle.ToString());

            }
    //StocksRecapData();

            }
        }

    public void StocksRecapData()
    {
        texttospeech.StartSpeaking(" The stock ; " + stckname.text.ToString() + ", belongs to ;" + Stockindustry.text.ToString());
        Debug.Log("Speak name;" + stckname.text.ToString());
        StockperformanceAPI();
    }

    public void StockperformanceAPI()
    {
        url2 = "https://api.equityanalyze.com/api/yahoo/" + symbol.text.Replace("-", "_").Replace("&", "_");//"MAXVIL.NSE";
        FmStocksAPI = false;
        StartCoroutine(FetchPerformancedata());
    }

    public IEnumerator FetchPerformancedata()
    {

        yield return new WaitForSeconds(2);
        UnityWebRequest request = UnityWebRequest.Get(url2);
        yield return request.SendWebRequest();

        //RecapTxt.text = "";
  if (request.isNetworkError || request.isHttpError)
        {
            //CLOSED Debug.Log(request.error + "There was a error");
            PopUpMsg.text = "Oops!! please try again !!";
            PopUp.gameObject.SetActive(true);
            StartCoroutine(HidePopUp());
        }
     
        else
        {
            var Stocksperformancedata = JsonConvert.DeserializeObject<StockPerformanceData>(request.downloadHandler.text);
            performanceaggregate = 0.0f;
            description = "";
            string dividentdate = "";
            string environrisk = "";
            string socialrisk = "";
            string governancerisk = "";
            string esgscore = "";

            string market_cap = "";
            string revenue = "";
            float volume = 0;
            
            float price_52_week_high = 0;
            float price_52_week_low = 0;
            string sector = "";
            string country = "";
            float Low1M = 0;
            float High1M = 0;
            float Perf1W = 0;
            float Perf1M = 0;
            float Perf3M = 0;
            float Perf6M = 0;
            float PerfY = 0;
            float PerfYTD = 0;
            string RecommendAll = "";
            float average_volume_10d_calc = 0;
            
            string lastweekperf = "";
            string lastOneMonthperf = "";
            string lastThreeMonthsperf = "";
            string lastSixMonthperf = "";
            string lastOneYearperf = "";
            string YTDperf = "";

            //string script = "";
            string peformancescript1 = "";
            string ESGScores = "";
            string Dividentdate = "";
            string Revenue = "";
            string Avg10DaysVolume = "";
            string MarketCap = "";
            
           if (FmStocksAPI == true)
            {
                Stocks_acc_Call_API = new Stocks_Accordion_Call_API();

                // **** IMPORTANT -- This line can create the listed Peformances in a loop for using further.
                performanceTxt = GameObject.Find("NewsRoomBTNSpeak/Text").GetComponentInChildren<Text>(); 

            }
            if (Stocksperformancedata.data.performace != null && Stocksperformancedata.data.performace.Count > 0)
            {
                //Data from API

                market_cap = Stocksperformancedata.data.market_cap.ToString();

                description = Stocksperformancedata.data.description.ToString();
                dividentdate = Stocksperformancedata.data.divident_date.ToString();
                environrisk = Stocksperformancedata.data.erisk.ToString();
                socialrisk = Stocksperformancedata.data.srisk.ToString();

                governancerisk = Stocksperformancedata.data.grisk.ToString();

                esgscore = Stocksperformancedata.data.esg.ToString();

                revenue = Stocksperformancedata.data.revenue.ToString();
                if (String.IsNullOrEmpty(Stocksperformancedata.data.volumn) || Stocksperformancedata.data.volumn == "")
                {
                    volume = 0.0f;
                }
                else
                {
                    volume = float.Parse(Stocksperformancedata.data.volumn.ToString());
                }

                price_52_week_high = float.Parse(Stocksperformancedata.data.performace[0].ToString());
                price_52_week_low = float.Parse(Stocksperformancedata.data.performace[1].ToString());

                sector = Stocksperformancedata.data.performace[2].ToString();
                country = Stocksperformancedata.data.performace[3].ToString();

                if (Stocksperformancedata.data.performace[4] == null || Stocksperformancedata.data.performace[5] == null)
                {
                    High1M = 0.0f; Low1M = 0.0f;
                    
                }

                else
                {
                    Low1M = float.Parse(Stocksperformancedata.data.performace[4].ToString());
                    High1M = float.Parse(Stocksperformancedata.data.performace[5].ToString());
                   
                }
                if (Stocksperformancedata.data.performace[6] == null || Stocksperformancedata.data.performace[7] == null ||
                   Stocksperformancedata.data.performace[8] == null || Stocksperformancedata.data.performace[9] == null || Stocksperformancedata.data.performace[10] == null || Stocksperformancedata.data.performace[11] == null)
                {
                   
                    Perf1W = 0.0f;
                    Perf1M = 0.0f;
                    Perf3M = 0.0f;
                    Perf6M = 0.0f;
                    PerfY = 0.0f;
                    PerfYTD = 0.0f;
                }

                else
                {
                    
                    Perf1W = float.Parse(Stocksperformancedata.data.performace[6].ToString());
                    Perf1M = float.Parse(Stocksperformancedata.data.performace[7].ToString());
                    Perf3M = float.Parse(Stocksperformancedata.data.performace[8].ToString());
                    Perf6M = float.Parse(Stocksperformancedata.data.performace[9].ToString());
                    PerfY = float.Parse(Stocksperformancedata.data.performace[10].ToString());
                    PerfYTD = float.Parse(Stocksperformancedata.data.performace[11].ToString());
                }



                RecommendAll = Stocksperformancedata.data.performace[12].ToString();
                average_volume_10d_calc = float.Parse(Stocksperformancedata.data.performace[13].ToString());

                //Performance Assessment

                if (Stocksperformancedata.data.esg.ToString() != "" || Stocksperformancedata.data.erisk.ToString() != "" || Stocksperformancedata.data.grisk.ToString() != "" || Stocksperformancedata.data.srisk.ToString() != "")
                {
                    ESGScores = "The Total ESG scores for " + stckname.text.ToString() + " is: " + esgscore + " with environment risk at: " + environrisk + " social score of: " + socialrisk + " and governance risk score at: " + governancerisk + ". ";
                    ESGScr.text = esgscore.ToString();
                    Envscr.text = environrisk.ToString();
                    Socialscr.text = socialrisk.ToString();
                    Govscr.text = governancerisk.ToString();
                }
                else if (Stocksperformancedata.data.esg.ToString() == "" || Stocksperformancedata.data.erisk.ToString() == "" || Stocksperformancedata.data.grisk.ToString() == "" || Stocksperformancedata.data.srisk.ToString() == "")
                {
                    ESGScores = "There are no ESG scores available for " + stckname.text.ToString() + ". ";
                    ESGScr.text = "There are no ESG scores available for " + stckname.text.ToString() + ". ";
                    Envscr.text = "";
                    Socialscr.text = "";
                    Govscr.text = "";
                }

                if (market_cap != "") { MarketCap = "has a Market Capital of : " + market_cap + ". "; Market_Cap_Data.text = market_cap.ToString(); } else { MarketCap = ", no market capital data was found for this stock. "; Market_Cap_Data.text = "-"; }

                if (revenue != "") { Revenue = " The last reported revenue is: " + revenue + ". "; Revenue_Data.text = revenue.ToString(); } else { Revenue = " No revenue data was found. "; Revenue_Data.text = "-"; }
                if (average_volume_10d_calc != 0) { Avg10DaysVolume = " The last 10 days average traded volume is: " + average_volume_10d_calc + ". "; Avg_10_Days_Data.text = average_volume_10d_calc.ToString(); } else { Avg10DaysVolume = " No average volume data was found. "; Avg_10_Days_Data.text = "-"; }
                if (dividentdate != "" || dividentdate != null || dividentdate != "N/A") { Dividentdate = " The divident date is: " + dividentdate + ". "; DividentDate_Data.text = dividentdate.ToString(); } else { Dividentdate = " No dividend date found for this item or dividend is not applicable. "; DividentDate_Data.text = "-"; }



                if (Perf1W > 0) { lastweekperf = " Profit with "; OneWkTxt.text = Perf1W.ToString(); OneWkTxt.color = Color.green; } else if (Perf1W < 0) { lastweekperf = " Loss with "; OneWkTxt.text = Perf1W.ToString(); OneWkTxt.color = Color.red; } else if (Perf1W == 0) { lastweekperf = " a change of "; OneWkTxt.text = Perf1W.ToString(); OneWkTxt.color = Color.green; }

                if (Perf1M > 0) { lastOneMonthperf = " Profit with "; OneMontxt.text = Perf1M.ToString(); OneMontxt.color = Color.green; } else if (Perf1M < 0) { lastOneMonthperf = " Loss with "; OneMontxt.text = Perf1M.ToString(); OneMontxt.color = Color.red; } else if (Perf1M == 0) { lastOneMonthperf = " a change of "; OneMontxt.text = Perf1M.ToString(); OneMontxt.color = Color.green; }

                if (Perf3M > 0) { lastThreeMonthsperf = " Profit with "; threeMontxt.text = Perf3M.ToString(); threeMontxt.color = Color.green; } else if (Perf3M < 0) { lastThreeMonthsperf = " Loss with "; threeMontxt.text = Perf3M.ToString(); threeMontxt.color = Color.red; } else if (Perf3M == 0) { lastThreeMonthsperf = " a change of "; threeMontxt.text = Perf3M.ToString(); threeMontxt.color = Color.green; }

                if (Perf6M > 0) { lastSixMonthperf = " Profit with "; SixMontxt.text = Perf6M.ToString(); SixMontxt.color = Color.green; } else if (Perf6M < 0) { lastSixMonthperf = " Loss with "; SixMontxt.text = Perf6M.ToString(); SixMontxt.color = Color.red; } else if (Perf6M == 0) { lastSixMonthperf = " a change of "; SixMontxt.text = Perf6M.ToString(); SixMontxt.color = Color.green; }

                if (PerfY > 0) { lastOneYearperf = " Profit with "; OneYrtxt.text = PerfY.ToString(); OneYrtxt.color = Color.green; } else if (PerfY < 0) { lastOneYearperf = " Loss with "; OneYrtxt.text = PerfY.ToString(); OneYrtxt.color = Color.red; } else if (PerfY == 0) { lastOneYearperf = " a change of "; OneYrtxt.text = PerfY.ToString(); OneYrtxt.color = Color.green; }

                if (PerfYTD > 0) { YTDperf = " Profit with "; YTDtxt.text = PerfYTD.ToString(); YTDtxt.color = Color.green; } else if (PerfYTD < 0) { YTDperf = " Loss with "; YTDtxt.text = PerfYTD.ToString(); YTDtxt.color = Color.red; } else if (PerfYTD == 0) { YTDperf = " a change of "; YTDtxt.text = PerfYTD.ToString(); YTDtxt.color = Color.green; }

                if ((Perf1W < 0) || (Perf1M < 0)||(Perf3M<0)||(Perf6M<0)||(PerfY<0)||(PerfYTD<0)) { performanceaggregate = 0; performanceTxt.text = "IGNORE"; LoadPerfPanel.GetComponent<Image>().color = Color.red; } //RecapTxt.text = peformancescript1; }

                else { performanceaggregate = Perf1W + Perf1M + Perf3M + Perf6M + PerfY + PerfYTD; performanceTxt.text = "1W " + Perf1W + " 1M "+ Perf1M + " 3M " + Perf3M + " 6M " + Perf6M + " 1Y " + PerfY + " YTD-" + PerfYTD; LoadPerfPanel.GetComponent<Image>().color = Color.green; }

                    //Scripts for speaking and analysis

                    //script = " This stock belongs to the " + sector + " Industry. " + "Its last week's performance has been "+ Perf1W + " %";
                    peformancescript1 = stckname.text.ToString()
                                      + " "
                                      + MarketCap
                                      + Revenue
                                      + Avg10DaysVolume
                                      + Dividentdate
                                      + "In the last one week this stock rendered " + lastweekperf + Perf1W + "%. "
                                      + " Last one month this stock rendered " + lastOneMonthperf + Perf1M + "%. "
                                      + " Last three months this stock rendered " + lastThreeMonthsperf + Perf3M + "%. "
                                      + " Last six months this stock rendered " + lastSixMonthperf + Perf6M + "%. "
                                      + " In the last one year this stock rendered " + lastOneYearperf + PerfY + "%. "
                                      + " Year to Date this stock rendered " + YTDperf + PerfYTD + "% ! "
                                      + ESGScores;

                if (description == null || description == "")
                {
                    AboutData.text = "No information available for " + stckname.text.ToString();
                }
                else
                {
                    AboutData.text = description.ToString();
                }
                

                //Debug.Log(stckname.text.ToString()
                //                      + " "
                //                      + MarketCap
                //                      + Revenue
                //                      + Avg10DaysVolume
                //                      + Dividentdate
                //                      + "In the last one week this stock rendered " + lastweekperf + Perf1W + "%. "
                //                      + " Last one month this stock rendered " + lastOneMonthperf + Perf1M + "%. "
                //                      + " Last three months this stock rendered " + lastThreeMonthsperf + Perf3M + "%. "
                //                      + " Last six months this stock rendered " + lastSixMonthperf + Perf6M + "%. "
                //                      + " In the last one year this stock rendered " + lastOneYearperf + PerfY + "%. "
                //                      + " Year to Date this stock rendered " + YTDperf + PerfYTD + "% ! "
                //                      + ESGScores + "PERFORMANCE TEXT = " + performanceTxt.ToString());
                }

            if ((Stocksperformancedata.data.performace == null || Stocksperformancedata.data.performace.Count <= 0 )&& FmStocksAPI == false)
            {
                //CLOSEDDebug.Log("Empty..");
                texttospeech.StartSpeaking("No performance data is available for " + stckname.text.ToString());
                //Debug.Log("NEWS FOR THIS STOCK " + NewsData.text.ToString());
            }

            else if ((Stocksperformancedata.data.performace != null || Stocksperformancedata.data.performace.Count > 0) && FmStocksAPI == false)
            {
                texttospeech.StartSpeaking(peformancescript1);
                //Debug.Log("NEWS FOR THIS STOCK " + NewsData.text.ToString());
                //Debug.Log(peformancescript1);
            }

            if (loadbtnclicked == true)
            {
                loadperfbtnclickPrefabON();
            }

            FmStocksAPI = false;
        }

    }

    public void loadperfbtnclickPrefabON()

    {
            StockRecapParentOBJ = GameObject.FindGameObjectWithTag("StockRecapParentOBJ");
            StockRecapParentOBJ.transform.SetSiblingIndex(11);
            StockRecapParentOBJ.GetComponentInChildren<CanvasGroup>().alpha = 1;
            PerfPanel = GameObject.FindGameObjectWithTag("StockRecap").GetComponent<RectTransform>();
            for (int i = 0; i < PerfPanel.transform.childCount; ++i) { Destroy(PerfPanel.transform.GetChild(i).gameObject); }

            //GameObject.FindGameObjectWithTag("StockRecap").SetActive(true);
            PerfRecapObj = Instantiate(PerfRecapPrefab);

            PerfRecapObj.transform.SetParent(PerfPanel, false);

            PerfRecapObj.transform.Find("Stock_Symbol").GetComponentInChildren<TextMeshProUGUI>().text = stckname.text.ToString();
            PerfRecapObj.transform.Find("RightSideItems/Market_Cap_Hdg/Market_Cap_Data").GetComponentInChildren<TextMeshProUGUI>().text = Market_Cap_Data.text.ToString();
            PerfRecapObj.transform.Find("RightSideItems/Revenue/Revenue_Data").GetComponentInChildren<TextMeshProUGUI>().text = Revenue_Data.text.ToString();

            PerfRecapObj.transform.Find("RightSideItems/Avg_10_Days_Vol/Avg_10_Days_Data").GetComponentInChildren<TextMeshProUGUI>().text = Avg_10_Days_Data.text.ToString();

            PerfRecapObj.transform.Find("RightSideItems/Divident_Date/DividentDate_Data").GetComponentInChildren<TextMeshProUGUI>().text = DividentDate_Data.text.ToString();

            PerfRecapObj.transform.Find("About/AboutData").GetComponentInChildren<TextMeshProUGUI>().text = AboutData.text.ToString();
            PerfRecapObj.transform.Find("News/NewsData").GetComponentInChildren<TextMeshProUGUI>().text = NewsData.text.ToString()+".";


            PerfRecapObj.transform.Find("Perf_1W/1W_Text").GetComponentInChildren<TextMeshProUGUI>().text = OneWkTxt.text.ToString() +" %";
            PerfRecapObj.transform.Find("Perf_1W/1W_Text").GetComponentInChildren<TextMeshProUGUI>().color = OneWkTxt.color;

            PerfRecapObj.transform.Find("Perf_1M/1M_Text").GetComponentInChildren<TextMeshProUGUI>().text = OneMontxt.text.ToString() + " %";
            PerfRecapObj.transform.Find("Perf_1M/1M_Text").GetComponentInChildren<TextMeshProUGUI>().color = OneMontxt.color;

            PerfRecapObj.transform.Find("Perf_3M/3M_Text").GetComponentInChildren<TextMeshProUGUI>().text = threeMontxt.text.ToString() + " %";
            PerfRecapObj.transform.Find("Perf_3M/3M_Text").GetComponentInChildren<TextMeshProUGUI>().color = threeMontxt.color;

            PerfRecapObj.transform.Find("Perf_6M/6M_Text").GetComponentInChildren<TextMeshProUGUI>().text = SixMontxt.text.ToString() + " %";
            PerfRecapObj.transform.Find("Perf_6M/6M_Text").GetComponentInChildren<TextMeshProUGUI>().color = SixMontxt.color;

            PerfRecapObj.transform.Find("Perf_1Y/1Y_Text").GetComponentInChildren<TextMeshProUGUI>().text = OneYrtxt.text.ToString() + " %";
            PerfRecapObj.transform.Find("Perf_1Y/1Y_Text").GetComponentInChildren<TextMeshProUGUI>().color = OneYrtxt.color;

            PerfRecapObj.transform.Find("Perf_YTD/YTD_Text").GetComponentInChildren<TextMeshProUGUI>().text = YTDtxt.text.ToString() + " %";
            PerfRecapObj.transform.Find("Perf_YTD/YTD_Text").GetComponentInChildren<TextMeshProUGUI>().color = YTDtxt.color;
        
        //COMMENTED ESG SCORES as most companies don't have this yet.
        
            //PerfRecapObj.transform.Find("ESGScore/TotlESGScrData").GetComponentInChildren<TextMeshProUGUI>().text = ESGScr.text.ToString();
            //PerfRecapObj.transform.Find("ESGScore/EnvScrData").GetComponentInChildren<TextMeshProUGUI>().text = Envscr.text.ToString();
            //PerfRecapObj.transform.Find("ESGScore/SocialScrData").GetComponentInChildren<TextMeshProUGUI>().text = Socialscr.text.ToString();
            //PerfRecapObj.transform.Find("ESGScore/GovScrData").GetComponentInChildren<TextMeshProUGUI>().text = Govscr.text.ToString();

            loadbtnclicked = false;
    }
    }
   



// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class StockItemNews
{
    public List<StockItem> data { get; set; }
    public string id { get; set; }
    public string title { get; set; }
    public string provider { get; set; }
    public string sourceLogoId { get; set; }
    public int published { get; set; }
    public string source { get; set; }
    public int urgency { get; set; }
    public string permission { get; set; }
    public List<RelatedSymbol> relatedSymbols { get; set; }
    public string storyPath { get; set; }
}

public class RelatedSymbol
{
    public string symbol { get; set; }
    public string logoid { get; set; }
}

public class StockItem
{
    public List<StockItem> items { get; set; }
    public string title { get; set; }
}

public class StockPerformance
{
    public List<StockPerformanceData> data { get; set; }
    public string description { get; set; }
    public string divident_date { get; set; }
    public string erisk { get; set; }
    public string esg { get; set; }
    public string grisk { get; set; }
    public string market_cap { get; set; }
    public List<object> performace { get; set; }
    public string revenue { get; set; }
    public string srisk { get; set; }
    public string volumn { get; set; }
}

public class Error1
{
    public string message { get; set; }
    public string type { get; set; }
}

public class StockPerformanceData
{
    public StockPerformanceData data { get; set; }
    public List<object> performace { get; set; }
    public int count { get; set; }
    public string description { get; set; }
    public string divident_date { get; set; }
    public string erisk { get; set; }
    public string esg { get; set; }
    public string grisk { get; set; }
    public string market_cap { get; set; }
    public string revenue { get; set; }
    public string srisk { get; set; }
    public string volumn { get; set; }
    public Error1 error { get; set; }
    public string result { get; set; }
}