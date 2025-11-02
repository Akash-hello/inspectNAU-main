using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FundamentalsfromEOD
{
    //Source API example https://eodhistoricaldata.com/api/fundamentals/AAPL.US?api_token=OeAFFmMliFG5orCUuwAKQ8l4WWFQ67YX
   
    //General

    public bool IsDelisted { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Exchange { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencySymbol { get; set; }
    public string CountryName { get; set; }
    public string CountryISO { get; set; }
    public string FiscalYearEnd { get; set; }
    public string IPODate { get; set; }
    public string Sector { get; set; }
    public string Industry { get; set; }
    
    //Highlights
   
    public double MarketCapitalizationMln { get; set; }
    public long EBITDA { get; set; }
    public double PERatio { get; set; }
    public double PEGRatio { get; set; }
    public double EarningsShare { get; set; }
    public double ReturnOnAssetsTTM { get; set; }
    public double ReturnOnEquityTTM { get; set; }
   
    //Valuation
    public double PriceBookMRQ { get; set; }
    
    //Technicals
    
    public double Beta { get; set; }
    public double _52WeekHigh { get; set; }
    public double _52WeekLow { get; set; }
    public double _200DayMA { get; set; }
    
    //AnalystRatings
    
    public double Rating { get; set; }
    public double TargetPrice { get; set; }
    public int StrongBuy { get; set; }
    public int Buy { get; set; }
    public int Hold { get; set; }
    public int Sell { get; set; }
    public int StrongSell { get; set; }

    //SharesStats
    
    public long SharesOutstanding { get; set; }
}

public class FundasMainClass
{
    public FundamentalsfromEOD Highlights { get; set; }
    public List<FundamentalsfromEOD> fundamentaldata { get; set; }

}