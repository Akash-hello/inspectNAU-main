using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewsButtonClicked : MonoBehaviour
{
    public Text CountryCode;
    public NewsRequest newsrequest;
    public Text Countryname;

    public void CountryBtnclckd()
    {
        newsrequest.countrycode = CountryCode.text.ToString();
        newsrequest.headingcountryname.text = Countryname.text.ToString();
        newsrequest.Getnewsdata();
    }
}
