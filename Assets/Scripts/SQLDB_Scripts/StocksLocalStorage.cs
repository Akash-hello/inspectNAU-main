using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{ 
public class StocksLocalStorage

{
    public int _id;
    public string _exchgname;
    public string _stocksym;
    public string _data;
    public string _dateadd;
    public string _timestamp;

    public StocksLocalStorage(int id, string exchgname, string stocksym, string data, string dateadd, string timestamp)
    {
        _id = id;
        _exchgname = exchgname;
        _stocksym = stocksym;
        _data = data;
        _dateadd = dateadd;
        _timestamp = timestamp;
    }

    public static StocksLocalStorage getFakeInventory()
    {
        return new StocksLocalStorage(0, "NSE", "HDFCBANK", "Bank", "2/1/2023 11:30:00 AM", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
    }
}

}

