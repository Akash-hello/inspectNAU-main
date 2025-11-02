using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StocksAPI_Call : MonoBehaviour
    {
        public List<StocksmainClass> data { get; set; }
        public int days { get; set; }
        public string period { get; set; }
        public string symbol { get; set; }
        public double marketcap { get; set; }
        public string name { get; set; }
}

    public class Error
    {
        public string message { get; set; }
        public string type { get; set; }
    }
public class Result
{
    public string result { get; set; }
   
}
public class StocksmainClass
    {
   
    public double adj_close { get; set; }
    public double close { get; set; }
    public string date { get; set; }
    public double high { get; set; }
    public double low { get; set; }
    public string category { get; set; }
    public double open { get; set; }
    public double rsi { get; set; }
    public double sma_50 { get; set; }
    public double sma_100 { get; set; }
    public double sma_200 { get; set; }
    public double sma_10 { get; set; }
    public double sma_20 { get; set; }
    public double ema_5 { get; set; }
    public double ema_9 { get; set; }
    public double ema_26 { get; set; }
    public double macd { get; set; }
    public double macdsignal { get; set; }
    public double macdhist { get; set; }
    public double marketcap { get; set; }
    public string symbol { get; set; }
    public string name { get; set; }
    public double volume { get; set; }

    public StocksAPI_Call Data { get; set; }
    public List<StocksAPI_Call> data { get; set; }
    //public List<object> data { get; set; }
    public object error { get; set; }
    public object result { get; set; }
}

public class FromDatabase
{
    public int id { get; set; }
    public string stocksym { get; set; }
    public string price { get; set; }
    public string date { get; set; }
}

public class WhatsAppMyWatchResults
{
    public string stocksym { get; set; }
    public string date { get; set; }
    public string watchsince { get; set; }
    public string open { get; set; }
    public string close { get; set; }
    public string category { get; set; }
    public string high { get; set; }
    public string lossgain { get; set; }
}
