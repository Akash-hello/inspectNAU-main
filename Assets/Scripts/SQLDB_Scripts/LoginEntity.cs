using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class LoginEntity
    {
        //Such ?Entity? classes are used for easy data handling. It?s not like everyone will remember the order of columns in their tables.
        public int _id;
        public string _name;
        public string _company;
        public string _designation;
        public string _email;
        public string _password;
        public string _phone;

        public string   _companycode;
        public string   _companyguid;
        public int      _tokenbalance;
        public string   _usertype;
        public int      _vesselimo;
        public string   _vesseltype;

        public string _apitoken;
        public string _tokenvalidity;
        public string _marketset;
        public string _markets;
        public string _sessionstate;
        public string _photofilename;
        public string _timestamp;

        public LoginEntity(int id, string name, string company, string designation,string email, string password, string phone, string companycode ,string companyguid,int tokenbalance,string usertype,int vesselimo,string vesseltype,string apitoken, string tokenvalidity, string marketset, string markets, string sessionstate, string photofilename, string timestamp)
        {
            _id = id;
            _name = name;
            _company = company;
            _designation = designation;
            _email = email;
            _password = password;
            _phone = phone;

            _companycode = companycode;
            _companyguid = companyguid;
            _tokenbalance = tokenbalance;
            _usertype = usertype;
            _vesselimo = vesselimo;
            _vesseltype = vesseltype;

            _apitoken = apitoken;
            _tokenvalidity = tokenvalidity;
            _marketset = marketset;
            _markets = markets;
            _sessionstate = sessionstate;
            _photofilename = photofilename;
            _timestamp = timestamp;

        }

        public static LoginEntity getFakeInventory()
        {
            return new LoginEntity(0, "name","company","designation","ankush1maurya@gmail.com", "123456", "919810606650","companycodeAUBH" , "companyguid", 2,"Individual",9876544,"Oil Tanker","APITOKENHEREssassasasasasasas", "2022-05-12", "NSE", "NSE-India,BSE-India,XLON-UK,NYSE-USA,HKG-Hongkong", "N" ,"myphoto.jpeg", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }
}