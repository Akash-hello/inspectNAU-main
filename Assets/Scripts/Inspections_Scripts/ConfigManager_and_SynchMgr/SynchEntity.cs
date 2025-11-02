using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class SynchEntity 
        //Such “Entity” classes are used for easy data handling. It’s not like everyone will remember the order of columns in their tables.
    
    {
        public int _id;
        public string _machineid;
        public string _data;
        public string _itemcode;
        public int _quantity;
        public string _processed;
        public string _sourcetable;
        public string _flag;
        public string _filename;
        public string _timestamp;

        public SynchEntity(int id, string machineid, string data, string itemcode, int quantity, string processed, string sourcetable, string flag,string filename, string timestamp)
        {

            _id = id;
            _machineid = machineid;
            _data = data;
            _itemcode = itemcode;
            _quantity = quantity;
            _processed = processed;
            _sourcetable = sourcetable;
            _flag = flag;
            _filename = filename;
            _timestamp = timestamp;

        }
        public static SynchEntity getFakeSynch()
        {
            return new SynchEntity(0, "XXXXXXXXXXXX","Data: {'Data1',Data2,Data3,Data4,.......}","ItemCode",-1,"N", "Inventory", "NewItem" ,"Synchfile.zip",DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
            
        }
    }
}