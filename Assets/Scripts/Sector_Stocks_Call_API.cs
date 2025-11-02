using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;

//This CODE WAS MERGED WITHIN THE Exchange_Sector_Call_API.cs

public class Sector_Stocks_Call_API : MonoBehaviour
{
    private string url;
    public string StockForSector = "";

    //public string SectorsinExch = "";
    //public List<string> SectorInExchg;

    public List<string> StockSymbolSector = new List<string>();
    public string StockSymbol = "";
    
    public void GetStocksInSector()
    {
        StockForSector = "Real Estate";
        url = "https://api.equityanalyze.com/api/symbol/NSE/" + StockForSector;
        StartCoroutine(SectorResponserequest());
    }
    
  public  IEnumerator SectorResponserequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");
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

        printDebug();
    }

    public void StocksinSector()
    {
        StockSymbolSector.Add(StockSymbol);
    }

    public void printDebug()
    {
        Debug.Log("These are the Stock Symbols in; " + StockForSector + " - " + string.Join(",", StockSymbolSector));
    }
}
