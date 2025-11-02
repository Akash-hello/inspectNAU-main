using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class SectStocksBase : MonoBehaviour
{
        public int count { get; set; }
        public List<StockSymbols> data { get; set; }
        public Errors error { get; set; }
        public string result { get; set; }
    }

    public class Fundamental
    {
        public string sector { get; set; }
    }

    public class StockSymbols
    {
        public Fundamental fundamental { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
    }

    public class Errors
    {
        public string message { get; set; }
        public string type { get; set; }
    }


