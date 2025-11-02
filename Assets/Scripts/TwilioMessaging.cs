using UnityEngine;
using System.Collections;
using TMPro;
using DataBank;
using SQLite4Unity3d;
using System.Threading;
using System;

public class TwilioMessaging : MonoBehaviour
{
    string url = "api.twilio.com/2010-04-01/Accounts/";
    string service = "/Messages.json";
    public string from;
    public string to;
    public string account_sid;
    public string auth;
    public string Content_Sid;
    public string body;
    public string contentvariables;
    long whatsappfmDB;
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;
    public GameObject[] Mobilenumbers;
    //public VoiceController voicecontroller;
    public TMP_Text MEssageFromTwilio;

    public object MessageResource { get; private set; }
    public WWWForm form;

    private void Start()
    {
        whatsappfmDB = 0;
        time = 15f;
        from = "whatsapp:+15592490222"; // format for Whatsapp
        //from = "+15592490222"; format for SMS
        account_sid = "ACf1de5a64b09e6bd9543e9d1e174df2de";
        auth = "9326a0ba0c4a18b80c972234d551acdf";

    }

    public void ExecuteWithRetry(Action action, int maxRetries = 3, int delayMilliseconds = 1000)
    {
        int retries = 0;
        while (true)
        {
            try
            {
                action();
                break; // Exit the loop if the action succeeded
            }
            catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
            {
                if (++retries == maxRetries)
                    throw; // Rethrow the exception if maximum retries reached
                Thread.Sleep(delayMilliseconds); // Wait before retrying
            }
        }
    }
    public void SendSMS()
    {
        ExecuteWithRetry(() =>
        {
            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();

            mlocationDb.getLatestID();
            whatsappfmDB = mlocationDb.whatsappnumber;
            
            if (whatsappfmDB == 0)
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "If you are a subscribed member, please update your number in settings and try again." + "\n" + "\n" + "Note; This feature is not available during 'TRIAL' or 'GUEST LOGINS'.";
                StartCoroutine(HidePopUp());
            }
            else
            {
                to = "whatsapp:+" + whatsappfmDB;
                //to = "whatsapp:+919810606650";
                form = new WWWForm();
                form.AddField("ContentSid", Content_Sid);
                form.AddField("From", from);
                form.AddField("ContentVariables", contentvariables);
                form.AddField("To", to);
                form.AddField("MessagingServiceSid", "MG3de4aec36d77c6121b005f38baf2b8df");

                int contentlength = contentvariables.Length;
                int bodylength = body.Length;
                string completeurl = "https://" + account_sid + ":" + auth + "@" + url + account_sid + service;
                Debug.Log(completeurl);
                WWW www = new WWW(completeurl, form);
                StartCoroutine(WaitForRequest(www));
            }
            
        });
    }

    //public void SendSMS()
    //{
    //    table_LoginConfig mlocationDb = new table_LoginConfig();
    //    mlocationDb.getLatestID();
    //    whatsappfmDB = mlocationDb.whatsappnumber;

    //    if (whatsappfmDB == 0)
    //    {
    //        errorpopup.gameObject.SetActive(true);
    //        PopUpMsg.text = "If you are a subscribed member, please update your number in settings and try again." + "\n" + "\n" + "Note; This feature is not available during 'TRIAL' or 'GUEST LOGINS'.";

    //        StartCoroutine(HidePopUp());
    //    }

    //    else

    //    {

    //        //content_variables = json.dumps({'1': 'Name'}),

    //        to = "whatsapp:+" + whatsappfmDB;
    //        //to = "whatsapp:+919810606650";
    //        //to = "+919810606650"; //format for SMS
    //        form = new WWWForm();

    //        form.AddField("ContentSid", Content_Sid);
    //        form.AddField("From", from);
    //        form.AddField("ContentVariables", contentvariables);
    //        form.AddField("To", to);
    //        form.AddField("MessagingServiceSid", "MG3de4aec36d77c6121b005f38baf2b8df");
    //        //string bodyWithoutSpace = body.Replace (" ", "%20");//Twilio doesn't need this conversion

    //        //form.AddField("Body", body);


    //        //string[] arr = body.Split(body, 2, StringSplitOptions.None);
    //        int contentlength = contentvariables.Length;
    //        int bodylength = body.Length;
    //        //Debug.Log("Number of characters -- "+bodylength);
    //        string completeurl = "https://" + account_sid + ":" + auth + "@" + url + account_sid + service;
    //        Debug.Log(completeurl);
    //        WWW www = new WWW(completeurl, form);
    //        StartCoroutine(WaitForRequest(www));
    //    }
    //    //foreach (GameObject Mobilenumber in Mobilenumbers)
    //    //{

    //    //    WWWForm form = new WWWForm();
    //    //    form.AddField("To", "+" + Mobilenumber.GetComponent<TMP_InputField>().text);
    //    //    Debug.Log("+" + Mobilenumber.GetComponent<TMP_InputField>().text);
    //    //    form.AddField("From", from);
    //    //    //string bodyWithoutSpace = body.Replace (" ", "%20");//Twilio doesn't need this conversion
    //    //    form.AddField("Body", body);
    //    //    string completeurl = "https://" + account_sid + ":" + auth + "@" + url + account_sid + service;
    //    //    Debug.Log(completeurl);
    //    //    WWW www = new WWW(completeurl, form);
    //    //    StartCoroutine(WaitForRequest(www));

    //    //    if (Mobilenumber == Mobilenumbers.Last())

    //    //    {
    //    //        //voicecontroller.OnFinalSpeechResult("sent");
    //    //        Debug.Log("I AM THE LAST Mobile Number & OnFinalSpeechResult. ");
    //    //    }

    //    //}
    //    mlocationDb.close();
    //}

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok! SMS/WhatsApp sent through Web API: " + www.text);
            MEssageFromTwilio.text = "Success! message sent through Web API: " + www.text;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Please check your Email (Inbox/Spam) or WhatsApp for details: +" + whatsappfmDB;

            StartCoroutine(HidePopUp());
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
            MEssageFromTwilio.text = "Error! sending message through Web API: " + www.error;
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Failed to send WhatsApp message! to: +" + whatsappfmDB;

            StartCoroutine(HidePopUp());
        }
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);
        PopUpMsg.text = "";
        errorpopup.gameObject.SetActive(false);

    }
}