using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;

public class StocksResponse : MonoBehaviour //THIS IS ONLY REFERENCE SCRIPT, IT WAS USED EARLIER DURING TESTING ONLY, NOT IN USE ANYMORE................
{
    private string url;
    public List<string> stocksymbols;
    public string symbols;
    public string DailyState;
    public string WeeklyState;
    public string MonthlyState;
        
    public string FinalStockState;
    public string StocksDeclining;

    public string StockCategory;

    public string VBullStocksResults;
    public List<string> VBullStocks;
    public string RangingStocksResults;
    public List<string> RangingStocks;
    public string BullStocksResults;
    public List<string> BullStocks;
    public string BearishStocksResults;
    public List<string> BearishStocks;

    public string DecliningStocksResults;
    public List<string> DecliningStocks;

    public void GetStocksData()
    {
        //symbols = "INDUSINDBK.NSE";

     VBullStocksResults = "";
     VBullStocks = new List<string>();
    RangingStocksResults = "";
    RangingStocks = new List<string>();
    BullStocksResults = "";
    BullStocks = new List<string>();
    BearishStocksResults = "";
    BearishStocks = new List<string>();

    DecliningStocksResults = "";
   DecliningStocks = new List<string>();


    symbols = "INOXLEISUR.NSE,DNAMEDIA.NSE,ENIL.NSE,EROSMEDIA.NSE,HTMEDIA.NSE";
        url = "https://api.equityanalyze.com/api/chartdatabyquery?symbols=" + symbols + "&periods=D-3,W-3,M-3&columns=date,open,high,low,close,adj_close,rsi,sma_50-D";
        StartCoroutine(StocksResponserequest());
    }

    IEnumerator StocksResponserequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");
        }
        else
        {
            var stockdata = JsonConvert.DeserializeObject<StocksmainClass>(request.downloadHandler.text);


            foreach (string sym in symbols.Split(','))
            {
                       
            Debug.Log("These are the Symbols requested; " + symbols.ToString());
                FinalStockState = "";
                StocksDeclining = "";

            List<StocksAPI_Call> DailyData = stockdata.data.Where(l => l.period == "Daily" && l.symbol == sym).ToList();
            List<StocksAPI_Call> WeeklyData = stockdata.data.Where(l => l.period == "W" && l.symbol == sym).ToList();
            List<StocksAPI_Call> MonthlyData = stockdata.data.Where(l => l.period == "M" && l.symbol == sym).ToList();
            int n = DailyData[0].data.Count;

                var SMA_1 = DailyData[0].data[n - 1].sma_50;
                var SMA_2 = DailyData[0].data[n - 2].sma_50;

                if ((DailyData[0].data[n-1].sma_50 - DailyData[0].data[n-2].sma_50) > 0) //COMPARING SMA_50 for latest two days to see if upwards, this needs to further done in slope value.
            {
                    //daily data rsi values
                    double d1 = DailyData[0].data[n-1].rsi;
                    double d2 = DailyData[0].data[n-2].rsi;
                    double d3 = DailyData[0].data[n-3].rsi;

                    //Weekly data rsi values
                    double w1 = WeeklyData[0].data[n-1].rsi;
                    double w2 = WeeklyData[0].data[n-2].rsi;
                    double w3 = WeeklyData[0].data[n-3].rsi;

                    //Monthly data rsi values
                    double m1 = MonthlyData[0].data[n-1].rsi;
                    double m2 = MonthlyData[0].data[n-2].rsi;
                    double m3 = MonthlyData[0].data[n-3].rsi;

                    if (d1 > 60)

                    {
                        if (d1 > 60 && d2 > 60 && d3 > 60)
                        {

                            if (d1 > d2 && d3 > d2)
                            {
                                Debug.Log("RSI Forming Support Above 60 RSI");
                            }
                            //for Parabola function


                           float x1 = 1;
                            float x2 = 2;
                            float x3 = 3;


                            double a = 0;

                            a = (x1 * (d3 - d2) + x2 * (d1 - d3) + x3 * (d2 - d1)) / ((x1 - x2) * (x1 - x3) * (x2 - x3));
                            Debug.Log(sym + "- value of a - " +a);

                            if (d1 > d2 && d2 > d3)
                            {
                                Debug.Log("Daily RSI is Ascending");
                            }

                            if ((d1 < d2 && d2 < d3) || (d1 < d2 && d2>d3))
                            {
                                Debug.Log("Daily RSI is Descending");
                            }

                        }
                        
                        if (d2 < 60)
                        {
                            Debug.Log("Daily Data 1st RSI Breakout from under 60 ");
                        }

                        DailyState = "Bullish";
                        //if ((DailyData[0].data[1].rsi > 60)&& DailyData[0].data[0].rsi > 60));
                        //Debug.Log(sym + DailyState);
                        //("1st RSI is " + DailyData[0].data[2].rsi +" For Date -" + DailyData[0].data[2].date);
                    }

                    else if (d1 >= 40 && d1 <= 60)
                    {

                        //Debug.Log("Daily Data is RANGING");
                        DailyState = "Ranging";
                        //if ((DailyData[0].data[1].rsi > 60)&& DailyData[0].data[0].rsi > 60));
                       // Debug.Log(sym + DailyState);
                    }

                    else if (d1 < 40)
                    {

                      //  Debug.Log("Daily Data is BEARISH");
                        DailyState = "Bearish";
                        //if ((DailyData[0].data[1].rsi > 60)&& DailyData[0].data[0].rsi > 60));
                       /// Debug.Log(sym + DailyState);
                    }

                    //WEEKLY BLOCK

                    if (w1 > 60)

                    {
                        if (w1 > 60 && w2 > 60 && w3 > 60)
                        {

                            if (w1 > w2 && w3 > w2)
                            {
                                Debug.Log("RSI Forming Support Above 60 RSI");
                            }
                           
                            if (w1 > w2 && w2 > w3)
                            {
                                Debug.Log("Weekly RSI is Ascending");
                            }

                            if ((w1 < w2 && w2 < w3) || (w1 < w2 && w2 > w3))
                            {
                                Debug.Log("Weekly RSI is Descending");
                            }

                        }

                        if (w2 < 60)
                        {
                            Debug.Log("Weekly Data 1st RSI Breakout from under 60 ");
                        }

                        WeeklyState = "Bullish";
                        //Debug.Log(sym + WeeklyState);

                        
                    }

                    else if (w1 >= 40 && w1 <= 60)
                    {

                      //  Debug.Log("Weekly Data is RANGING");
                        WeeklyState = "Ranging";
                       // Debug.Log(sym + WeeklyState);
                    }

                    else if (w1 < 40)
                    {

                     //   Debug.Log("Weekly Data is BEARISH");
                        WeeklyState = "Bearish";
                     //   Debug.Log(sym + WeeklyState);
                    }


                    //MONTHLY BLOCK

                    if (m1 > 60)

                    {
                        if (m1 > 60 && m2 > 60 && m3 > 60)
                        {

                            if (m1 > m2 && m3 > m2)
                            {
                                Debug.Log("RSI Forming Support Above 60 RSI");
                            }

                            if (m1 > m2 && m2 > m3)
                            {
                                Debug.Log("Monthly RSI is Ascending");
                            }

                            if ((m1 < m2 && m2 < m3) || (m1 < m2 && m2 > m3))
                            {
                                Debug.Log("Monthly RSI is Descending");
                            }

                        }

                        if (m2 < 60)
                        {
                            Debug.Log("Monthly Data 1st RSI Breakout from under 60 ");
                        }

                        MonthlyState = "Bullish";
                        //Debug.Log(sym + MonthlyState);


                    }

                    else if (m1 >= 40 && m1 <= 60)
                    {

                       // Debug.Log("Monthly Data is RANGING");
                        MonthlyState = "Ranging";
                       // Debug.Log(sym + MonthlyState);
                    }

                    else if (m1 < 40)
                    {

                       // Debug.Log("Monthly Data is BEARISH");
                        MonthlyState = "Bearish";
                       // Debug.Log(sym + MonthlyState);
                    }

                    //FINAL ASSESSMENT OF THE STOCK

                    if (DailyState == "Bullish"  && WeeklyState == "Bullish" && MonthlyState == "Bullish")

                    {
                        StockCategory = "VERY BULLISH";
                        FinalStockState = sym + ": VERY BULLISH"+"\n";
                        //Debug.Log(sym + ": VERY BULLISH");
                    }

                   else if ((DailyState == "Ranging" || DailyState == "Bearish") && WeeklyState == "Bullish" && MonthlyState == "Bullish")

                    {
                        StockCategory = "BULLISH";
                        FinalStockState = sym + ": BULLISH" + "\n";
                        //Debug.Log(sym + ": BULLISH");
                    }

                    else if (WeeklyState == "Ranging" || MonthlyState == "Ranging")

                    {
                        StockCategory = "RANGING";
                        FinalStockState = sym + ": RANGING" + "\n";
                        //Debug.Log(sym + ": RANGING");
                    }

                    else if (WeeklyState == "Bearish" || MonthlyState == "Bearish")

                    {
                        StockCategory = "BEARISH";
                        FinalStockState = sym + ": BEARISH" + "\n";
                        //Debug.Log(sym + ": BEARISH");
                    }
                    stocksresultmethod();
                }

                else if ((DailyData[0].data[n - 1].sma_50 - DailyData[0].data[n - 2].sma_50) <= 0)
                {
                    
                    StocksDeclining = sym + "\n";
                    //Debug.Log(sym + " stock Daily SMA - 50 is on a decline.");
                    stocksresultmethod();
                }
               

            }

           printDebug();

        }

        // Debug.Log(stockrecap.ToString());

        //MAKE ONE BLOCK FOR WATCH CATEGORY ALSO IF REQUIRED LATER

        //if (w1 > 60)
        //{
        //    Debug.Log("Weekly Data 1st RSI is " + WeeklyData[0].data[2].rsi + " For Date -" + WeeklyData[0].data[2].date);
        //}

        //if (m1 > 60)
        //{
        //    Debug.Log("Monthly Data 1st RSI is " + MonthlyData[0].data[2].rsi + " For Date -" + MonthlyData[0].data[2].date);
        //}


        //foreach (StocksAPI_Call Sdata in stockdata.data.Where(l => l.symbol == sym).ToList())
        //{
        //    var period = Sdata.period;
        //    var symbol = Sdata.symbol;
        //    List<StocksmainClass> data = Sdata.data;
        //    Debug.Log("Total noumber of rows" + data.Count.ToString());
        //    int n = data.Count;
        //    if (data[n - 1].rsi > 60)
        //    {
        //        Debug.Log("Stock Symbol: " + symbol + ", Period: " + period + ", Date: " + data[n - 1].date + ", RSI latest: " + data[n - 1].rsi + ", SMA50: " + data[n - 1].sma_50 + "\n"
        //            + ", Date: " + data[n - 2].date + ", RSI 2nd: " + data[n - 2].rsi + ", SMA50: " + data[n - 2].sma_50 + "\n"
        //            + ", Date: " + data[n - 3].date + ", RSI 3rd: " + data[n - 3].rsi + ", SMA50: " + data[n - 3].sma_50 + "\n");
        //        //data[0].rsi > data[1].rsi
        //    }

        //    else
        //    {
        //        Debug.Log("NOOOOOOOOOO");
        //    }
    }

    public void stocksresultmethod()
    {
        if (StockCategory == "VERY BULLISH")
        {
            VBullStocks.Add(FinalStockState);
        }

        if (StockCategory == "BULLISH")
        {
            BullStocks.Add(FinalStockState);
        }

        if (StockCategory == "RANGING")
        {
            RangingStocks.Add(FinalStockState);
        }

        if (StockCategory == "BEARISH")
        {
            BearishStocks.Add(FinalStockState);
        }


        if (StocksDeclining != "")
        {
            DecliningStocks.Add(StocksDeclining);
        }
        

       
    }

    public void printDebug()
    {
        Debug.Log("These are the VERY Bullish Stocks; " + string.Join(",", VBullStocks));
        Debug.Log("These are the Bullish Stocks; " + string.Join(",", BullStocks));
        Debug.Log("These are the RANGING Stocks; " + string.Join(",", RangingStocks));
        Debug.Log("These are the BEARISH Stocks; " + string.Join(",", BearishStocks));
        Debug.Log("These are the DECLINING Stocks; " + string.Join(",", DecliningStocks));
    }

}


