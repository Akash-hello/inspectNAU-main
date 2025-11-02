using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SectorsMainClass
{
    public int count { get; set; }
    public ExchangeSectors Data { get; set; }
    public List<ExchangeSectors> data { get; set; }
    public Errorss error { get; set; }
    public string result { get; set; }
}

public class ExchangeSectors : MonoBehaviour
{
    //public List<SectorsMainClass> data { get; set; }
    public string _id { get; set; }
     public string name { get; set; }
}

    public class Errorss
    {
        public string message { get; set; }
        public string type { get; set; }
    }

    

