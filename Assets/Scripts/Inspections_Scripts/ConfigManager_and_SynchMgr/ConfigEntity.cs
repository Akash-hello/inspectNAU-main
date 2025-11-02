using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class ConfigEntity
    //Such “Entity” classes are used for easy data handling. It’s not like everyone will remember the order of columns in their tables.

    {
        public int _id;
        public string _machineid;
        public string _https;
        public string _ServerIP;
        public string _ServerPort;
        public string _IncomingAPI;
        public string _OutboundAPI;
        public string _orionconnect;
        public string _lastlogindatetime;
        public string _termsandconditions;
        public string _nameoncard;
        public string _creditcardnumber;
        public string _creditcardcvv;
        public string _creditcardexpiry;
        public string _paymenthistory;
        public string _timestamp;

        public ConfigEntity(int id, string machineid, string https, string ServerIP, string ServerPort,string IncomingAPI, string OutboundAPI, string orionconnect, string lastlogindatetime, string termsandconditions, string nameoncard, string creditcardnumber,string creditcardcvv,string creditcardexpiry,string paymenthistory, string timestamp)
        {
            _id = id;
            _machineid = machineid;
            _https = https;
            _ServerIP = ServerIP;
            _ServerPort = ServerPort;
            _IncomingAPI = IncomingAPI;
            _OutboundAPI = OutboundAPI;
            _orionconnect = orionconnect;
            _termsandconditions = termsandconditions;
            _nameoncard = nameoncard;
            _creditcardnumber = creditcardnumber;
            _creditcardcvv = creditcardcvv;
            _creditcardexpiry = creditcardexpiry;
            _paymenthistory = paymenthistory;
            _lastlogindatetime = lastlogindatetime;
            _timestamp = timestamp;
        }
        public static ConfigEntity getFakeSynch()
        {
            return new ConfigEntity(0, "machineid","https://" ,"192.168.0.1", "110","INcomingAPI","OutBoundAPI","https://nauserver.com:8888","Terms and Conditions", "Smith Davison", "CCard - 4478 5555 8080","CVV - 025","Exp - 05/21", "paymenthistory - Jan21-USD XX,Feb21-USD XX", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }
}