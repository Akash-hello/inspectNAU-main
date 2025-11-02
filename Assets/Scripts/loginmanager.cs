using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class loginmanager : MonoBehaviour
{
    string url;
    string urlforgotpassword;
    string urltermsandconditions;
    public TextMeshProUGUI registerURL;
    public TextMeshProUGUI forgotpasswordURL;
    public TextMeshProUGUI termsandconditionsURL;
    public TMP_InputField password;

   // private string trialdate;
    private DateTime currentdate;
    public GameObject GuestLoginButton;
    public GameObject LoginSignupItems;

    public TextMeshProUGUI PopUpMsg;
    public GameObject PopUp;
    private float time = 0.0f;
    public TextMeshProUGUI freetrialendmsg;
    private int daystogo;
    private DateTime startdate;
    private DateTime enddaate;

    // Start is called before the first frame update
    void Start()
    {
        //GuestLoginButton.SetActive(true); // changed this to false on below line for NAU Inspect, since no guest login option is available.
        GuestLoginButton.SetActive(false);
        LoginSignupItems.SetActive(true);
        //trialdate = "2023/05/01";
        currentdate = DateTime.Now;
        enddaate = new DateTime(2099, 01, 01);

        daystogo = (enddaate - currentdate).Days;

        if (daystogo <= 1)
        {

            freetrialendmsg.text = "Last Day of Trial" + "\n" + "Trial Ends; "+ enddaate.ToString("ddd, dd MMM yyy") + ".*";
        }
        else if (daystogo > 1)
        {
            freetrialendmsg.text = daystogo.ToString() + " - Days to go." + "\n" + "Trial Ends; " + enddaate.ToString("ddd, dd MMM yyy") + ".*"; ;
        }
            
        if (currentdate > enddaate)

        {
            GuestLoginButton.SetActive(false);
            LoginSignupItems.SetActive(true);
            //PopUp.SetActive(true);
            //PopUpMsg.text = "Sorry, free trial's expired, please signup to use the APP.";
            //Debug.Log("TRIAL OVER");
        }
        //registerURL = "";
        registerURL.text = "https://inspectnau.orionmarineconcepts.com/companyregistration.aspx";
        forgotpasswordURL.text = "https://equityanalyze.com/resetpassword";
        termsandconditionsURL.text = "https://equityanalyze.com/terms_conditions";
        urltermsandconditions = termsandconditionsURL.text;
        urlforgotpassword = forgotpasswordURL.text;
        url = registerURL.text;
    }

    // Update is called once per frame
    
    public void OpenRegisterpageOnClick()
    {
        Application.OpenURL(url);
    }

    public void ForgotpasswordpageOnClick()
    {
        Application.OpenURL(urlforgotpassword);
    }

    public void TermsandConditionsOnClick()
    {
        Application.OpenURL(urltermsandconditions);
    }

    public void LoadLoginPage()
    {
        SceneManager.LoadScene("LoginSignup12");
    }
    public void GuestLoad()
    {
        if (currentdate > enddaate)

        {
            GuestLoginButton.SetActive(false);
            LoginSignupItems.SetActive(true);
            PopUp.SetActive(true);
            PopUpMsg.text = "Sorry, free trial's expired, please signup to use the APP.";
            Debug.Log("TRIAL OVER");
        }

        else
        //SceneManager.LoadScene("EquityAnalyzeMainScene");
        SceneManager.LoadScene("loader", LoadSceneMode.Single);
        StartCoroutine(HidePopUp());
    }

    public void ShowHidePassword()
    {
        if (password.contentType == TMP_InputField.ContentType.Standard)
            password.contentType = TMP_InputField.ContentType.Password;
        else
        {
            password.contentType = TMP_InputField.ContentType.Standard;
        }
        password.ForceLabelUpdate();
    }

    IEnumerator HidePopUp()
    {
        time = 2.0f;
        yield return new WaitForSeconds(time);

        PopUp.gameObject.SetActive(false);
    }

}

