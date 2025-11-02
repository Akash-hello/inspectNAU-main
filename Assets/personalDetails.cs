using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI.Extensions;
using System;
using DataBank;

public class personalDetails : MonoBehaviour
{
    public TMP_InputField WhatsAppNumber;
    public TextMeshProUGUI useremail;
    public TextMeshProUGUI subscriptionvalidity;
    public TextMeshProUGUI WhatsAppShow;
    public TMP_InputField whatsappnumber;
    long whatsappnumberforDB;
    public GameObject personalsettingspanel;
    public GameObject PopUp;
    public TextMeshProUGUI PopUpMsg;

    public float time = 0.0f;

    private void Start()
    {

        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        
        DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeSeconds(mlocationDb.subscriptionvalid);
        DateTime datTime = dateTimeOffSet.DateTime;
        
        useremail.text = mlocationDb.useremail.ToString();
        subscriptionvalidity.text = "Subscription: " + Convert.ToDateTime(dateTimeOffSet.ToString()).ToLongDateString();
        if (mlocationDb.whatsappnumber == 0)
        {
            WhatsAppShow.text = "WhatsApp Number: Not available";
        }
        else
        {
            WhatsAppShow.text = "WhatsApp Number: +" + mlocationDb.whatsappnumber.ToString();

        }
        connection.Close();
    }
    public void Whatsappnumberupdate()
    {
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();

        if (whatsappnumber.text != "")
        {
            whatsappnumberforDB = Convert.ToInt64(whatsappnumber.text.ToString());
            mlocationDb.Updatedata(" Phone = '" + whatsappnumberforDB + "' ,TimeStamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "'");

            mlocationDb.getLatestID();
            
            if (mlocationDb.whatsappnumber == 0)
            {
                WhatsAppShow.text = "WhatsApp Number: Not available";
            }
            else
            {
                WhatsAppShow.text = "WhatsApp Number: +" + mlocationDb.whatsappnumber.ToString();

            }

          whatsappnumber.text = "";
        }

        else if (whatsappnumber.text == "")
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Please enter your whatsapp number with country code.";
        }
        StartCoroutine(HidePopUp());
       //Debug.Log(whatsappnumber);
        
    }

    public void SettingsBtnClick ()
    {
        personalsettingspanel.SetActive(!personalsettingspanel.activeInHierarchy);
        //Whatsappnumberupdate();
    }

    IEnumerator HidePopUp()
    {
        time = 2.0f;
        yield return new WaitForSeconds(time);

        PopUp.gameObject.SetActive(false);
    }
}
