using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class MyStckWatchEntity
    {
        //Such “Entity” classes are used for easy data handling. It’s not like everyone will remember the order of columns in their tables.
        public int _id;
        public string _stocksym;
        public string _stockname; 
        public string _industry;
        public string _closingpricedate;
        public string _dateadd;
        public double _closingprice;
        public string _status;
        public string _timestamp;

        public MyStckWatchEntity(int id, string stocksym, string stockname, string industry, string closingpricedate, string dateadd, double closingprice, string status, string timestamp)
        {
            _id = id;
            _stocksym = stocksym;
            _stockname = stockname;
            _industry = industry;
            _closingpricedate = closingpricedate;
            _dateadd = dateadd;
            _closingprice = closingprice;
            _status = status;
            _timestamp = timestamp;

        }

        public static MyStckWatchEntity getFakeInventory()
        {
            return new MyStckWatchEntity(0, "HDFC.NSE", "HDFC Bank Limited", "Bank", "2022-05-11", "2022-05-12", 3230.456, "Very Bullish", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }
}