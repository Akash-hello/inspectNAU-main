using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using DataBank;
using System.IO;
//using UnityEngine.iOS;

public class registrationmobile : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TMP_InputField Company;
    public TMP_InputField CompanyCode;
    public Toggle CompanyCodeToggle;
    public Toggle UserType;
    public TMP_Dropdown VesselType;
    public TMP_InputField VesselIMONumber;

    public TMP_InputField Designation;
    public TMP_InputField EmailInputhidden;
    public TMP_InputField EmailInput;
    public TMP_InputField CountryCodeInput;
    public TMP_InputField WhatsappnbrInput;

    public TMP_InputField PasswordInput;
    public TMP_InputField ReenterPasswordInput;
    
    public string nameinputtext;
    public string companynametext;
    public string companycodetext = "";

    public string UserTypetoggle = "";
    public string VslTypeValue = "";
    public int vesselimo = 9999999;

    public string designationtext;
    public string emailinputtext;
    public string Countrcodeinputtext;
    public string WhatsAppNumberinputtext;
    public string passwordinputtext;

    public GameObject PopUp;
    public TextMeshProUGUI PopUpMsg;
    public Image Popupbackground;
    public GameObject registermobile;

    public TwilioMessaging msgbody;
    public SendGridEmailSender emailsender;
    public bool Successfullyregistered;

    int id;
    string name = "";
    string company = "";
    string designation = "";
    string email = "";
    string password = "";
    string countrycode = "";
    string phone = "";

    string companycode = "";
    public string companyguid = "";
    public string IncomingAPI = "";
    public int    tokenbalance = 0;
    public string usertype = "";
    

    public string apitoken = "";
    public string tokenvalidity = "";
    public string marketset = "";
    public string markets = "";
    public string sessionstate = "";
    string timestamp;
    string result = "";
    string message = "";
    //string[] subscribedmarkets;
    DateTime now;
    long unixTimeStampInSeconds;
    public float time = 0.0f; //Seconds to read the text

    public GameObject Mobileregnpanel;
    public GameObject Login_signuppanel;
    public GameObject Signupmobile;

    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    string code = "";
    public GameObject EnterEmailcodeInput;
    public GameObject CheckCodeBtn;
    public GameObject ResendCodeEmailBtn;

    public TextMeshProUGUI codevalidatingheader;
    bool resendcode;
    public bool directlogin;

    public TMP_InputField[] inputFields;

    public GameObject PasswordResetPnl;
    string usernamefgtpswd;
    string useremailforgotpwd;
    public TMP_InputField PasswordInputReset;
    public TMP_InputField ReenterPasswordInputReset;
    public bool forgotpwdclicked;
    string obscuredUsername;
    string obscuredDomain;
    string domain;
    string msgtoserver;
    public GameObject emailauthcodepanel;
    public TMP_InputField OTPRegnPage;
    // State to track if the keyboard is currently shown or hidden
    //private bool isKeyboardActive = false;


    //    public void Awake()
    //    {

    //#if UNITY_ANDROID
    //        GetAndroidDeviceName();

    //#elif UNITY_IOS

    // GetIOSDeviceName();
    //#endif
    //    }

    //#if UNITY_ANDROID
    //    public string GetAndroidDeviceName()
    //    {
    //    string deviceName = "Unknown";
    //    string deviceModel = "Unknown";

    //    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    //    {
    //        using (AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
    //        {
    //            using (AndroidJavaClass build = new AndroidJavaClass("android.os.Build"))
    //            {
    //                deviceName = build.GetStatic<string>("MANUFACTURER") + " " + build.GetStatic<string>("MODEL");
    //            }
    //        }
    //    }
    //    Debug.Log($"Device Model: {deviceModel}");
    //    Debug.Log($"Device Name: {deviceName}");


    //    return deviceName;
    //    }
    //#endif

    //#if UNITY_IOS

    //    public string GetIOSDeviceName()
    //{
    //    string deviceModel = Device.generation.ToString();
    //    string deviceName = Device.systemVersion;
    //        Debug.Log($"Device Model: {deviceModel}");
    //        Debug.Log($"Device Name: {deviceName}");

    //        return $"{deviceModel} ({deviceName})";
    //}
    //#endif

    public void Start()
    {
        inputFields = new TMP_InputField []{NameInput,Company,CompanyCode,Designation,CountryCodeInput,WhatsappnbrInput,EmailInput };
        NameInput.text = "";
        Company.text = "";
        CompanyCode.text = "";
        vesselimo = 9999999;
        Designation.text = "";
        CountryCodeInput.text = "";
        WhatsappnbrInput.text = "";
        EmailInput.text = "";
        EmailInputhidden.text = "";
        PasswordInput.text = "";
        ReenterPasswordInput.text = "";
        Successfullyregistered = false;
        IncomingAPI = "";
        time = 5.0f;
        EnterEmailcodeInput.SetActive(false);
        ResendCodeEmailBtn.SetActive(false);
        CheckCodeBtn.SetActive(false);
        codevalidatingheader.text = "";
        resendcode = false;
        directlogin = false;
        PasswordResetPnl.SetActive(false);
        PasswordInputReset.text = "";
        ReenterPasswordInputReset.text = "";
        forgotpwdclicked = false;
        usernamefgtpswd = "";
        useremailforgotpwd = "";
        emailinputtext = "";
        msgtoserver = "";
        OTPRegnPage.text = "";
    }

    public void ToggleMobileInput()
    {
#if UNITY_IOS || UNITY_ANDROID

       //  Toggle the shouldHideMobileInput on mobile devices only

        foreach (TMP_InputField input in inputFields)
        {
            if (input.shouldHideMobileInput)
            {
                input.shouldHideMobileInput = false;
                //Debug.Log("MADE IT FALSE");
            }

            else 
            {
                input.shouldHideMobileInput = true;
                //Debug.Log("MADE IT TRUE");
            }
        }

#else
        Debug.Log("This feature is only available on mobile platforms.");
#endif
    }


    public void HaveCompanyauthcode ()
    {
        if (CompanyCodeToggle.isOn == false)
        {
            CompanyCode.interactable = false;
        }

        else
        {
            CompanyCode.interactable = true;
            //CompanyCode.text = "";
        }

    }

    public void checkifregistered()
    {
        msgtoserver = "";
        forgotpwdclicked = false;
        PasswordResetPnl.SetActive(false);

        int latestId = 0;
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        latestId = mlocationDb.LatestId;

        if (latestId > 0)

        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Oops!! This device is already signedup, please click on forgot password and reset your credentials or simply re-install the App to register as a new user. ";
            //StartCoroutine(HidePopUp());
        }

        else
        {
            Mobileregnpanel.SetActive(true);
            Login_signuppanel.SetActive(true);
            Signupmobile.SetActive(true);
            CompanyCodeToggle.isOn = true;
            CompanyCodeToggle.interactable = false;
        }
        
    }

    public void RegisterButtonClick()

    {
        
        Successfullyregistered = false;
            if (NameInput.text == "" || Company.text == "" || (CompanyCodeToggle.isOn == true && CompanyCode.text == "") || Designation.text == "" || CountryCodeInput.text == "" || WhatsappnbrInput.text == "" || EmailInput.text == "" || PasswordInput.text == "" || ReenterPasswordInput.text == "" || EmailInput.text.Contains("@") == false)
            {
                if (NameInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your Name.";
                }

                else if (Company.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your Company Name.";
                }

                else if (CompanyCodeToggle.isOn == true && CompanyCode.text == "")

                {

                PopUp.SetActive(true);
                PopUpMsg.text = "<b>Authentication Code missing!!</b> " + Environment.NewLine + "<b>Company Inspector:</b> use your own Company Code (from Online Cloud account), this allows you to connect and download your Company, SIRE 2.0 and RightShip checklists onto your device." + Environment.NewLine + Environment.NewLine + "<b>Independent Inspector:</b> enter <b>ORION</b> as AUTHCODE to download only SIRE 2.0 and RightShip checklists.";

            }

            else if (Designation.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your designation.";
                }

                //else if (EmailInputhidden.text == "" || EmailInputhidden.text.Contains("@") == false)
                //{
                //    PopUp.SetActive(true);
                //    PopUpMsg.text = "Please enter a valid email id.";
                //}

                else if (EmailInput.text == "" || EmailInput.text.Contains("@") == false)
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter a valid email id.";
                }
                else if (CountryCodeInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your WhatsApp mobile number's Country code.";
                }

                else if (WhatsappnbrInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your WhatsApp mobile number.";
                }

                else if (PasswordInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your password.";
                }


                else if (ReenterPasswordInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please re-enter your password to confirm.";
                }

            else if (OTPRegnPage.text == "")
            {
                PopUp.SetActive(true);
                PopUpMsg.text = "Please enter the OTP received on email. If not received click on the SEND OTP again.";
            }
        }


            //else if (EmailInput.text != EmailInputhidden.text)
            //{
            //    PopUp.SetActive(true);
            //    PopUpMsg.text = "Emailids don't match, please re-enter.";
            //}

        else if (PasswordInput.text != ReenterPasswordInput.text)
            {
                PopUp.SetActive(true);
                PopUpMsg.text = "Passwords don't match, please re-enter.";
            }

            else
            {
                if (UserType.isOn)
                {
                    // Toggle is on, use the value in a string
                    UserTypetoggle = "Individual";

                }
                else
                {
                    // Toggle is off, use the value in a string
                    UserTypetoggle = "Ship";
                }

                if (VesselType.value != 0)
                {
                    VslTypeValue = VesselType.options[VesselType.value].text;
                    vesselimo = int.Parse(VesselIMONumber.text.ToString());
                }

                else
                {
                    VslTypeValue = "-";
                    vesselimo = 9999999;
                }

                nameinputtext = NameInput.text;
                companynametext = Company.text;
                companycodetext = CompanyCode.text;
                companyguid = "Awaited";

                tokenbalance = -10;

                designationtext = Designation.text;
                emailinputtext = EmailInput.text;
                Countrcodeinputtext = CountryCodeInput.text;
                WhatsAppNumberinputtext = WhatsappnbrInput.text;
                passwordinputtext = PasswordInput.text.Trim();
            //PopUp.SetActive(true);
            //PopUpMsg.text = "Thank you " + nameinputtext.ToString() + " for registering with Inspect NAU. " + System.Environment.NewLine + " Your credentials have been sent on WhatsAPP.";
            //Successfullyregistered = true;
            //AddData();

            //StartCoroutine(SendRegnDataToCloud());
            //PopUpMsg.text = "";

            validateemailCode1();
            //openvalidateemail();//THIS LINE WAS OVER_RIDDEN WITH ABOVE CHANGED TO THIS TO OVERRIDE OTP METHOD AND PROCEED TO DIRECT REGISTRATION

        }
        //StartCoroutine(HidePopUp());

    }

    public void forgotpassword()
    {
        msgtoserver = "";
        
        PopUpMsg.text = "";
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();

        if (mlocationDb.LatestId != 0)
        {
            usernamefgtpswd = mlocationDb.name;
            emailinputtext = mlocationDb.useremail;
            forgotpwdclicked = true;
            useremailforgotpwd = emailinputtext;
            PasswordResetPnl.SetActive(true);
            PasswordInputReset.text = "";
            ReenterPasswordInputReset.text = "";


            int atIndex = useremailforgotpwd.IndexOf('@');

            string username = useremailforgotpwd.Substring(0, atIndex);
            domain = useremailforgotpwd.Substring(atIndex + 1);

            // Obscure the username and domain
            obscuredUsername = username.Length > 2
                ? username.Substring(0, 2) + "***" + "@"+ domain
                : "***";

            //obscuredDomain = domain.Length > 5
            //    ? "***" + domain.Substring(domain.Length - 5)
            //    : "***";

            //validateemailCode();
            openvalidateemail();
        }

        else
        {
            Debug.Log("There is no user registered.");
            PopUp.SetActive(true);
            PopUpMsg.text = "Not applicable as there is no user registered.";
            PasswordResetPnl.SetActive(false);
            PasswordInputReset.text = "";
            ReenterPasswordInputReset.text = "";
        }
           
    }

    public void openvalidateemail()
    {

        PopUp.SetActive(true);
        PopUpMsg.text = "";
        EnterEmailcodeInput.GetComponent<TMP_InputField>().text = "";
        EnterEmailcodeInput.SetActive(true);
        ResendCodeEmailBtn.SetActive(true);
        //CheckCodeBtn.SetActive(true); //OLD BUTTON TO VALIDATE CODE, MERGED WITH CHANGE PASSWORD BUTTON DOES THE JOB...

        if (forgotpwdclicked == true)
        {
            codevalidatingheader.text = "Please enter the new password and the six digit code received on your emailid; " + obscuredUsername + ".";
        }
        else
        {
            codevalidatingheader.text = "Please enter the six digit code received on your emailid to complete the registration process...";
        }

        StartCoroutine(EnableResendButtonAfterDelay(30)); // 30-second timer
        generateemailcode(); //THIS LINE WAS OVER_RIDDEN
       
    }

    public void Resendemailcode()
    {
        resendcode = true;
        generateemailcode();

        StartCoroutine(EnableResendButtonAfterDelay(30)); // 30-second timer
    }

    public void SendemailcodeFmEmailFIeld()
    {
       if (EmailInput.text == "" || EmailInput.text.Contains("@") == false)
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Please enter a valid email id.";
        }

       else
        {
            generateemailcode();
            StartCoroutine(EnableResendButtonAfterDelay(30)); // 30-second timer
        }
    }

    private IEnumerator EnableResendButtonAfterDelay(float seconds)
    {
        Button btn = ResendCodeEmailBtn.GetComponent<Button>(); // Get Button component
        btn.interactable = false; // Disable button

        yield return new WaitForSeconds(seconds);
        btn.interactable = true; // Re-enable button
        //resendcode = false;
    }

    public void generateemailcode ()
    {
        int codeLength = 6; // Set the length of each code 
        int numberOfCodes = 1;
        string contentVariables = "";

        for (int i = 0; i < numberOfCodes; i++)
        {
            code = GenerateRandomCode(codeLength); //COMMENTED ON 22nd APR as the code was causing OTP issues on email at ICS DUBAI
            //code = "INSNAU";
            Debug.Log("Generated Code: " + code);
        }

        msgbody.body = "";
        //msgbody.body = "Dear *" + nameinputtext.ToString() + "* here are your credentials for login, Emailid: *" + emailinputtext + "*, Phone number: *" + Countrcodeinputtext + WhatsAppNumberinputtext + "* and password: *" + passwordinputtext + "*. Thank you.";

        //DO NOT SEND PASSWORD ON WHATSAPP.
        //passwordinputtext = "********";//DO NOT SEND PASSWORD ON WHATSAPP.


        if (forgotpwdclicked == true)
        {
            contentVariables = JsonConvert.SerializeObject(new Dictionary<string, System.Object>()
        {
            {"1", nameinputtext.ToString()},
            {"2", "Inspect NAU"},
            {"3", emailinputtext},
            {"4", Countrcodeinputtext + WhatsAppNumberinputtext},
            {"5", "********"}
            //{"5", passwordinputtext}
        }, Formatting.Indented);

        }
        else
        {
            contentVariables = JsonConvert.SerializeObject(new Dictionary<string, System.Object>()
        {
            {"1", NameInput.text.ToString()},
            {"2", "Inspect NAU"},
            {"3", EmailInput.text},
            {"4", CountryCodeInput.text + WhatsappnbrInput.text},
            {"5", "********"}
            //{"5", passwordinputtext}
        }, Formatting.Indented);
        }

      

        msgbody.contentvariables = contentVariables;
        //msgbody.Content_Sid = "HX8cf6e1e6b8116e9455cc2a7f1faecdb3";
        msgbody.Content_Sid = "HX4ea2d9b661f0e1c9c0ddf032bd9a924a";
        //msgbody.body = "Hello *mohit*, welcome to EquityAnalyze.";

        emailsender.subject = code.ToUpper() + " - (case sensitive) InspectNAU six digit email authentication code";

            
        if (forgotpwdclicked == true)
        {
            //emailsender.body = "Dear " + usernamefgtpswd.ToString() + ", " + Environment.NewLine + Environment.NewLine + "Your six digit password reset authentication code (case sensitive) is: " + code.ToUpper() + " please enter on the APP screen to validate your email and complete the password reset process." + Environment.NewLine+ Environment.NewLine +" Thank you & Best Regards" + Environment.NewLine +"InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";
            emailsender.toEmail = emailinputtext.ToString(); // Replace with the recipient's email

            emailsender.body = "Dear " + usernamefgtpswd.ToString() + ",<br><br>";

            emailsender.body += "Your six-digit password reset authentication code (case sensitive) is: ";
            emailsender.body += "<strong>" + code.ToUpper() + "</strong><br>";
            emailsender.body += "Please enter this code on the app screen to validate your email and complete the password reset process.<br><br>";

            emailsender.body += "Thank you & Best Regards,<br>";
            emailsender.body += "<strong>InspectNAU Administrator</strong><br>";
            emailsender.body += "Support Email: <a href='mailto:orionapps@orionmarineconcepts.com'>orionapps@orionmarineconcepts.com</a><br>";

        }
        else
        {
            //emailsender.body = "Dear " + nameinputtext.ToString() + ", "+ Environment.NewLine + Environment.NewLine + "Your six digit email authentication code (case sensitive) is: " + code.ToUpper() + " please enter on the APP screen to validate your email and complete the registration process." + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";
            emailsender.toEmail = EmailInput.text.ToString(); // Replace with the recipient's email

            emailsender.body = "Dear " + NameInput.text.ToString() + ",<br><br>";

            emailsender.body += "Your six-digit email authentication code (case sensitive) shared on your email ("+ EmailInput.text.ToString() + ") is: ";
            emailsender.body += "<strong>" + code.ToUpper() + "</strong><br>";
            emailsender.body += "Please enter this code on the app screen to validate your email and complete the registration process.<br><br>";

            emailsender.body += "Thank you & Best Regards,<br>";
            emailsender.body += "<strong>InspectNAU Administrator</strong><br>";
            emailsender.body += "Support Email: <a href='mailto:orionapps@orionmarineconcepts.com'>orionapps@orionmarineconcepts.com</a><br>";

        }
        //emailsender.body = "This is a testemail from SendGrid.";

        //msgbody.SendSMS();
        emailsender.OnSendEmailButtonClicked();

        if (resendcode == false && forgotpwdclicked == true)
        {
            codevalidatingheader.text = "Please enter the six digit code received on your emailid; " + obscuredUsername + "@" + obscuredDomain + " to change your password.";
        }

        if (resendcode == false && forgotpwdclicked == false)
        {
            codevalidatingheader.text = "Please enter the six digit code received on input emailid to authenticate and complete the registration process.";
        }

        else if (resendcode == true)
        {
            codevalidatingheader.text = "Code has been resent on email...";
        }

        resendcode = false;
    }


    private string GenerateRandomCode(int length)
    {
        StringBuilder result = new StringBuilder(length);
        string digits = "0123456789";
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            //result.Append(chars[random.Next(chars.Length)]);
            result.Append(digits[random.Next(digits.Length)]); 
        }

        return result.ToString();
    }


    public void ResetPasswordSave()
    {
        EnterEmailcodeInput.GetComponent<TMP_InputField>().textComponent.color = Color.black;
        codevalidatingheader.text = "";
        if (PasswordInputReset.text == "")
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Please enter your desired password.";
            
        }

        else if (ReenterPasswordInputReset.text == "")
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Please re-enter your desired password to confirm.";
            
        }

        else if (PasswordInputReset.text != ReenterPasswordInputReset.text)
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Passwords don't match, please check and re-enter.";
            
        }

        else
        {
            validateemailCode();
        }
    }


    public void validateemailCode() //MOVED TO "validateemailCode1" with OTP entry on registration section, BECOZ It was causing issues on some Android Phones, made default generated and entered code directly...
    {
        string generatedcode = code.ToUpper(); //COMMENTED ON 22nd APR as the code was causing OTP issues on email at ICS DUBAI
        //string generatedcode = "INSNAU";

        string enteredcode = EnterEmailcodeInput.GetComponent<TMP_InputField>().text.Trim();

        //string enteredcode = "INSNAU"; //OTP ISSUE As mentioned above, hardcoded to make it same as generated code...

        if (string.IsNullOrEmpty(enteredcode))
        {
            EnterEmailcodeInput.GetComponent<TMP_InputField>().text = "Required";
            EnterEmailcodeInput.GetComponent<TMP_InputField>().textComponent.color = Color.red;
            codevalidatingheader.text = "Please enter the authentication code to continue...";

            if (EnterEmailcodeInput.activeSelf == false)
            {
                PopUpMsg.text = "Please wait while we register you on the cloud. This may take upto 20 seconds, please do not leave this area or refresh.";
            }
            else
            {
                PopUpMsg.text = "";
            }
            
            
        }

        else if (enteredcode == generatedcode && forgotpwdclicked == false)
        {
            codevalidatingheader.text = "Thank you, code successfully validated...";
            PopUp.SetActive(true);
            if (EnterEmailcodeInput.activeSelf == false)
            {
                PopUpMsg.text = "Please wait while we register you on the cloud. This may take upto 20 seconds, please do not leave this area or refresh.";
            }
            else
            {
                PopUpMsg.text = "";
            }

            EnterEmailcodeInput.SetActive(false);
            ResendCodeEmailBtn.SetActive(false);
            CheckCodeBtn.SetActive(false);
            codevalidatingheader.text = "";
            msgtoserver = "newuser";
            StartCoroutine(SendRegnDataToCloud());
        }
        else if (enteredcode == generatedcode && forgotpwdclicked == true )
        {
            if (PasswordInputReset.text == "" || ReenterPasswordInputReset.text == "" || PasswordInputReset.text != ReenterPasswordInputReset.text)

            {
                codevalidatingheader.text = "Thank you, code successfully validated, please enter your desired password below.";
                if (EnterEmailcodeInput.activeSelf == false)
                {
                    PopUpMsg.text = "Please wait while we register you on the cloud.";
                }
                else
                {
                    PopUpMsg.text = "";
                }


                emailauthcodepanel.SetActive(false);
            }

            else
            {
                codevalidatingheader.text = "Thank you, code successfully validated, password reset request sent to the server, please wait while we get the confirmation.";
                PopUp.SetActive(true);
                if (EnterEmailcodeInput.activeSelf == false)
                {
                    PopUpMsg.text = "Please wait while we register you on the cloud.";
                }
                else
                {
                    PopUpMsg.text = "";
                }

                EnterEmailcodeInput.SetActive(false);
                ResendCodeEmailBtn.SetActive(false);
                CheckCodeBtn.SetActive(false);
                codevalidatingheader.text = "";

                msgtoserver = "updatepassword";
                StartCoroutine(SendRegnDataToCloud());

            }
        }
        else
        {
            codevalidatingheader.text = "Invalid or incorrect code entered, please retry...";
            EnterEmailcodeInput.GetComponent<TMP_InputField>().text = "Invalid or Incorrect";
            EnterEmailcodeInput.GetComponent<TMP_InputField>().textComponent.color = Color.red;
            if (EnterEmailcodeInput.activeSelf == false)
            {
                PopUpMsg.text = "Please wait while we register you on the cloud.";
            }
            else
            {
                PopUpMsg.text = "";
            }
        }
    }

    public void validateemailCode1() //09th May separated the validation code for registration from forgot password.
    {
        string generatedcode = code.ToUpper();
        //string generatedcode = "INSNAU";

        string enteredcode = OTPRegnPage.GetComponent<TMP_InputField>().text.Trim();
        //string enteredcode = "INSNAU"; //OTP ISSUE As mentioned above, hardcoded to make it same as generated code...

        if (string.IsNullOrEmpty(enteredcode))
        {
            OTPRegnPage.GetComponent<TMP_InputField>().text = "Required";
            PopUp.SetActive(true);
            PopUpMsg.text = "Please enter the OTP received on email.";
        }

        else if (enteredcode == generatedcode)
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Thank you, OTP successfully validated. Please wait while we register your account on cloud.";


            msgtoserver = "newuser";
            StartCoroutine(SendRegnDataToCloud());
        }
       
        else
        {
            PopUp.SetActive(true);
            PopUpMsg.text = "Invalid or Incorrect OTP entered, please check and retry...";
        }


    }

    public void ShowHidePassword()
    {
        if (PasswordInput.contentType == TMP_InputField.ContentType.Standard)
        {
            PasswordInput.contentType = TMP_InputField.ContentType.Password;

        }
        else
        {
            PasswordInput.contentType = TMP_InputField.ContentType.Standard;
        }
        PasswordInput.ForceLabelUpdate();
    }

    public void ShowHidePasswordReset()
    {
        if (PasswordInputReset.contentType == TMP_InputField.ContentType.Standard)
        {
            PasswordInputReset.contentType = TMP_InputField.ContentType.Password;

        }
        else
        {
            PasswordInputReset.contentType = TMP_InputField.ContentType.Standard;
        }
        PasswordInputReset.ForceLabelUpdate();
    }

    IEnumerator SendRegnDataToCloud()
    {
        //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = "ankush1maurya@gmail.com", password = "123456" });
        //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = emailinputtext, password = passwordinputtext });

        string bodyJsonString = "";


        if (msgtoserver == "newuser" && forgotpwdclicked == false)
        {
            bodyJsonString = JsonUtility.ToJson(new AuthData()
            {
                neworupdate = msgtoserver,
                name = nameinputtext.Replace("'", "''"),
                companyname = companynametext.Replace("'", "''"),
                companycode = companycodetext.Replace("'", "''"),
                designation = designationtext.Replace("'", "''"),
                email = emailinputtext.Replace("'", "''"),
                password = passwordinputtext.Trim(),
                Countrycode = Countrcodeinputtext.Replace("'", "''"),
                WhatsAppNumber = WhatsAppNumberinputtext,

                usertype = UserTypetoggle,
                vslType = VslTypeValue,
                imonumber = vesselimo,
                tokenremaining = tokenbalance,
                tokenrequest = 0

            });
        }
        if (msgtoserver == "updatepassword" && forgotpwdclicked == true)
        {
            bodyJsonString = JsonUtility.ToJson(new AuthData()
            {
                neworupdate = msgtoserver,
                email = emailinputtext,
                password = PasswordInputReset.text.Trim(),
                
            });
        }

        var request = new UnityWebRequest("https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");

            PopUpMsg.color = Color.red;
            PopUp.SetActive(true);
            PopUpMsg.text = "Oops !! Something went wrong, please check your internet connection or contact your APP administrator for details.";
            //StartCoroutine(HidePopUp());
        }

        else
        {

            string rawJson = Encoding.Default.GetString(request.downloadHandler.data);
            //TokenData tokenObj = TokenData.CreateFromJSON(rawJson);
            //Debug.Log(tokenObj.token);
            //Debug.Log(tokenObj.message);
            //Debug.Log(tokenObj.result);

            if (String.IsNullOrEmpty(rawJson))
            {
                PopUpMsg.color = Color.red;
                PopUp.SetActive(true);
                PopUpMsg.text = "Registration failed! seems you are offline, registration can be done only when online.";
                //PopUpMsg.text = "Registration failed! seems you are offline, registration can be done only when online.";
                //StartCoroutine(HidePopUp());
            }

            //message = tokenObj.message.ToString();
            if (rawJson.Contains("Error")) //SHOULD NOT GET REGISTERED>.....
            {
                PopUpMsg.color = Color.red;
                Debug.Log("Invalid user.");
                PopUp.SetActive(true);
                PopUpMsg.text = "Sorry, unable to register there was an error, please contact the support team.";

                //PopUpMsg.text = "Sorry, unable to register there was an error while saving data on the server, please contact the APP administrator.";
                //StartCoroutine(HidePopUp());
            }

            if (rawJson.Contains("Exists")) //User Already Exists!!

            {
                PopUpMsg.color = Color.black;
                PopUp.SetActive(true);
                PopUpMsg.text = "Oops!! There is already a device connected with this emailid. You can register with a new emailid as a new user or De-link the other device from on your online account at; inspectnau.orionmarineconcepts.com/loginpage.aspx and then register.";
                //StartCoroutine(HidePopUp());

                companycodetext = "";
                companyguid = "Awaited";
                IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";

                Successfullyregistered = false;
                //AddData();
            }

            if (rawJson.Contains("deactivated")) //User was Deactivated by the Admin!!

            {
                PopUpMsg.color = Color.black;
                PopUp.SetActive(true);
                PopUpMsg.text = "Oops!! This email id has been deactivated by the administrator, please ask your Company or Orion Administrator to re-activate your account.";
                //StartCoroutine(HidePopUp());

                companycodetext = "";
                companyguid = "Awaited";
                IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";

                Successfullyregistered = false;
                //AddData();
            }

            if (rawJson.Contains("No Authentication sent")) //But NEw email user...

            {
                //string IncomingAPILink = result.Split('=')[1];

                PopUpMsg.color = Color.black;
                PopUp.SetActive(true);
                PopUpMsg.text = "Registered as an independent user, not connected to any Company. You can enter the authentication code later from your profile section." + System.Environment.NewLine + " Your credentials have been sent on email and WhatsAPP.";
                //companycodetext = "";
                //IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";
                //companyguid = "Awaited";
                //Successfullyregistered = true;
                //directlogin = false;
                //AddData();

                //StartCoroutine(HidePopUp());
            }

            if (rawJson.Contains("Incorrect Authentication code")) //But NEw email user...

            {
                PopUpMsg.color = Color.black;
                PopUp.SetActive(true);
                PopUpMsg.text = "<b>Incorrect Authentication Code. Please Read Carefully!!</b> " + Environment.NewLine + "<b>Company Inspector:</b> use your own Company Code (from Online Cloud account), this allows you to connect and download your Company, SIRE 2.0 and RightShip checklists onto your device." + Environment.NewLine + Environment.NewLine + "<b>Independent Inspector:</b> enter <b>ORION</b> as AUTHCODE to download only SIRE 2.0 and RightShip checklists.";
                //StartCoroutine(HidePopUp());

                //companycodetext = "";
                //companyguid = "Awaited";
                //IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";

                //Successfullyregistered = true;
                //directlogin = false;
                //AddData();
            }

            if (rawJson.Contains("GUID not available")) //But NEw email user...

            {
                PopUpMsg.color = Color.black;
                PopUp.SetActive(true);
                PopUpMsg.text = "Invalid authentication code entered, you have been registered as independent user. You can enter the authentication code later from your profile section." + System.Environment.NewLine + " Your credentials have been sent on email and WhatsAPP.";
                //StartCoroutine(HidePopUp());

                companycodetext = "";
                companyguid = "Awaited";
                IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";

                Successfullyregistered = true;
                directlogin = false;
                StartCoroutine(HidePopUp());
                AddData();
            }

            if (rawJson.Contains("Successful")&& forgotpwdclicked == false) //CORRECT authentication code sent with a new email id.
            {
                string guid = rawJson.Split('=')[1];
                companyguid = guid.Trim().Replace("\"", "");

                IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";

                //table_LoginConfig mlocationDb = new table_LoginConfig();
                //string columndataquery = "CompanyGuid = '" + guid+ "'";

                PopUp.SetActive(true);
                PopUpMsg.text = "Thank you " + nameinputtext.ToString() + " for registering with Inspect NAU. " + System.Environment.NewLine + " Your credentials have been sent on email and WhatsAPP.";
                Successfullyregistered = true;

                //mlocationDb.Updatedata(columndataquery);
                directlogin = false;
                AddData();
                StartCoroutine(HidePopUp());
                //mlocationDb.close();
            }

            if (rawJson.Contains("PasswordUpdated") && forgotpwdclicked == true) //CORRECT authentication code sent with a new email id.
            {
                PopUp.SetActive(true);
                PopUpMsg.text = "Thank you your password has been reset successfully. Please close this window and proceed to login.";

                updatepasswordinDB();
            }

            if (rawJson.Contains("User Not Found!!")) //CORRECT authentication code sent with a new email id.
            {
                PopUp.SetActive(true);
                PopUpMsg.text = "You don't seem to be registered.";

            }

            

            //StartCoroutine(HidePopUp());
        }
    }

    public void AddData()
    {
       table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        //mlocationDb.getLatestID();

        mlocationDb.deleteAllData(); //PEDNING TO CHEKC HOW TO CLOSE THESE CONNECTIONS

        tokenvalidity = "4070929577"; // 01st Jan 2099, basically endless
        marketset = "";
        markets = "NSE,BSE,NASDAQ,NYSE,LSE";
        sessionstate = "N";
        //apitoken = "MOBILEREGISTRATION";
        apitoken = IncomingAPI;

        mlocationDb.addData(new LoginEntity
            (id, nameinputtext.Replace("'", "''"), companynametext.Replace("'", "''"),
            designationtext.Replace("'", "''"), emailinputtext.Replace("'", "''"), passwordinputtext.Replace("'", "''"),
            Countrcodeinputtext.Replace("'", "''") + WhatsAppNumberinputtext, companycodetext, companyguid, tokenbalance, UserTypetoggle, vesselimo, VslTypeValue,
            apitoken, tokenvalidity,
            marketset, markets, sessionstate, "newuser.jpeg", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

        //NEW FOLDER CREATED FOR STORING USERPROFILE DATA, LIKE PHOTO OR OTHER...

        if (!Directory.Exists(Application.persistentDataPath + "/" + "Profile"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + "Profile");
        }

        if (directlogin == false)
        {
            MsgforTwilio();
        }
      else if (directlogin == true)
        {
            Debug.Log("No need to send email coming in directly as administrator.");
        }
        Debug.Log("Data Added");
        connection.Close();
        //StartCoroutine(SendRegnDataToCloud());
    }


    public void updatepasswordinDB()
    {

        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();

        string columndataquery = "Password = '" + PasswordInputReset.text.Trim() +"' where cast(Id as int) = " + mlocationDb.LatestId;
        mlocationDb.Updatedata(columndataquery);
    }

    public void MsgforTwilio()
    {
       
        msgbody.body = "";
        //msgbody.body = "Dear *" + nameinputtext.ToString() + "* here are your credentials for login, Emailid: *" + emailinputtext + "*, Phone number: *" + Countrcodeinputtext + WhatsAppNumberinputtext + "* and password: *" + passwordinputtext + "*. Thank you.";

        //DO NOT SEND PASSWORD ON WHATSAPP.
       // passwordinputtext = "********";//DO NOT SEND PASSWORD ON WHATSAPP.

        string contentVariables = JsonConvert.SerializeObject(new Dictionary<string, System.Object>()
        {
            {"1", nameinputtext.ToString()},
            {"2", "Inspect NAU"},
            {"3", emailinputtext},
            {"4", Countrcodeinputtext + WhatsAppNumberinputtext},
            {"5", "********"}
            //{"5", passwordinputtext}
        }, Formatting.Indented);

        msgbody.contentvariables = contentVariables;
        //msgbody.Content_Sid = "HX8cf6e1e6b8116e9455cc2a7f1faecdb3";
        msgbody.Content_Sid = "HX4ea2d9b661f0e1c9c0ddf032bd9a924a";
        //msgbody.body = "Hello *mohit*, welcome to EquityAnalyze.";
        msgbody.body = "Hi " + nameinputtext.ToString() + " your login credentials for inspectNAU mobile APP are email; " + emailinputtext.ToString() + " and phone number; " + Countrcodeinputtext + WhatsAppNumberinputtext + " with password; " + passwordinputtext + ". Thank you.";

        emailsender.subject = "Welcome aboard InspectNAU - Let's get started!";

        emailsender.toEmail = emailinputtext.ToString(); // Replace with the recipient's email
        //emailsender.body = "Hi " + nameinputtext.ToString() + " your login credentials for inspectNAU mobile APP are email; " + emailinputtext.ToString() + " and phone number; " + Countrcodeinputtext + WhatsAppNumberinputtext + " with password; " + passwordinputtext + ". Thank you.";

        //emailsender.body = "Hi " + nameinputtext.ToString() + "," +System.Environment.NewLine + System.Environment.NewLine + "Your login credentials for inspectNAU mobile APP are;" + System.Environment.NewLine+ "email; "+ emailinputtext.ToString() + System.Environment.NewLine + "Phone number; " + Countrcodeinputtext + WhatsAppNumberinputtext + System.Environment.NewLine + "Password; *********" + Environment.NewLine + "You can manage your account online from; inspectnau.orionmarineconcepts.com/loginpage.aspx" + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com."; ;

        emailsender.body = "Dear " + nameinputtext.ToString() + ",<br><br>";

        emailsender.body += "Welcome aboard InspectNAU! We’re excited to have you join us in revolutionizing maritime inspections.<br><br>";

        //emailsender.body += "I’m <strong>Capt. Mohit Sabharwal</strong>, a mariner with years of seafaring and technology development experience and the founder of Orion Marine Concepts personally welcome you to the InspectNAU.<br>";
        //emailsender.body += "Having spent years at sea and personally experiencing the challenges of vessel inspections and compliance.<br>";
        //emailsender.body += "This inspired us to develop InspectNAU—a solution built by a mariner, for mariners.<br>";
        //emailsender.body += "Our goal is to leverage technology to simplify inspections, enhance compliance, and improve operational efficiency for ship operators and inspectors alike.<br><br>";
        emailsender.body += "With InspectNAU, you can efficiently manage inspections, access vital reports, and ensure compliance with the latest industry standards—including seamless SIRE 2.0, RightShip 3.1 integration as well as your own inspections and audit regimes.<br><br>";

        emailsender.body += "Your login credentials for InspectNAU mobile app are:<br><hr>";
        emailsender.body += "<strong>Email:</strong> " + emailinputtext.ToString() + "<br>";
        emailsender.body += "<strong>Phone Number:</strong> " + Countrcodeinputtext + WhatsAppNumberinputtext + "<br>";
        emailsender.body += "<strong>Password:</strong>********* (encrypted).<br><br>";

        emailsender.body += "<strong>Getting Started:</strong><br><hr>";
        emailsender.body += "To help you experience the full potential of InspectNAU, we are offering <strong>two free inspections and a sample template to try out (this does not effect the credits).</strong><br>";
        emailsender.body += "After that, you can conveniently purchase additional credits online by logging in with the same credentials as your app login at:<br>";
        emailsender.body += "<a href='https://inspectnau.orionmarineconcepts.com/'>InspectNAU Online</a>.<br><br>";

        emailsender.body += "<strong>Want Custom Checklists Included?</strong><br><hr>";
        emailsender.body += "If you need your own custom checklists (inspections and/or audits) included in InspectNAU, connect with us and we’ll help you onboard them seamlessly.<br>";
        emailsender.body += "You can reach out to me or my team at <a href='mailto:orionapps@orionmarineconcepts.com'>orionapps@orionmarineconcepts.com</a> to get started or if you have any questions, our team is here to assist you.<br><br>";

        emailsender.body += "Thank you & Best Regards,<br><br>";
        emailsender.body += "<strong>Kanishka Agarwal</strong><br><br>";
        emailsender.body += "Client Relations Manager<br>";
        
        emailsender.body += "<a href='https://orionmarineconcepts.com/'>Company Website</a> | ";
        emailsender.body += "<a href='mailto:kanishka@orionmarineconcepts.com'>kanishka@orionmarineconcepts.com</a><br>";

        //emailsender.body = "This is a testemail from SendGrid.";

        msgbody.SendSMS();
       emailsender.OnSendEmailButtonClicked();

    }

    public void onclickPhoneCautionbtn()

    {
        PopUp.SetActive(true);
        PopUpMsg.text = "Inspect NAU needs WhatsApp for sending credentials and also sharing updates. Please enter valid Country Code and WhatsApp number.";
        //StartCoroutine(HidePopUp());
    }

    public void onclickAuthCautionbtn()

    {
        PopUp.SetActive(true);

        RectTransform rectTransform = PopUp.gameObject.GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = 1300f; // Set the height to 800
        rectTransform.sizeDelta = size;

        RectTransform rectTransformPopUpMsg = PopUpMsg.GetComponent<RectTransform>();

        if (rectTransformPopUpMsg != null)
        {
            // Set the height to 1200 while keeping other dimensions unchanged
            rectTransformPopUpMsg.sizeDelta = new Vector2(rectTransformPopUpMsg.sizeDelta.x, 1100);
        }

        //PopUpMsg.text = "<b>Please Read Carefully!!</b> "+Environment.NewLine+"<b>Company Inspector:</b> use your own Company Code (from Online Cloud account), this allows you to connect and download your Company, SIRE 2.0 and RightShip checklists onto your device." + Environment.NewLine + Environment.NewLine + "<b>Independent Inspector:</b> enter <b>ORION</b> as AUTHCODE to download only SIRE 2.0 and RightShip checklists.";

        float height = Popupbackground.rectTransform.rect.height;
        RectTransform rt = Popupbackground.rectTransform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 620f);


        PopUpMsg.alignment = TextAlignmentOptions.Left;

        PopUpMsg.text =
       "<b>Authentication Code</b>" + Environment.NewLine + Environment.NewLine +

       "1. '<b>ORION</b>' is your Authentication Code if you are an Individual inspector/auditor." + Environment.NewLine + Environment.NewLine +

       "2. If you are a Company or a team of inspectors/auditors, write to <b>orionapps@orionmarineconcepts.com</b> to get your team onboarded within 24 hrs.";


        //PopUpMsg.text =
        //"<b>Important Regarding; Authentication Code</b>" + Environment.NewLine+Environment.NewLine +

        //"1.  Use '<b>ORION</b>' as your Authentication Code." +Environment.NewLine + "if you are an <b>Individual User</b>, you will be able to use only default checklists; SIRE 2.0 and RightShip 3.1, etc." + Environment.NewLine + Environment.NewLine +

        //"2. <b>Company Authentication Code</b>: " + Environment.NewLine+"Register online to get a Code for your Company for your inspectors to use on the Mobile APP." + Environment.NewLine + Environment.NewLine +
        //          "<b>Company Definition</b>:" + Environment.NewLine+  Environment.NewLine+
        //        "a. You wish to use your or client company checklists along with SIRE 2.0 and RightShip 3.1 (included for free)." + Environment.NewLine +
        //         "b. A Ship Manager/Owner with various inspectors, Auditors, Superintendents." + Environment.NewLine +
        //         "c. Independent Inspecting Company who perform inspections for other companies using their specific audit or inspection checklists.";

        //StartCoroutine(HidePopUp());
    }

    public void clickedcancelPopup()
    {
        RectTransform rectTransform = PopUp.gameObject.GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = 600f; // Set the height to 600
        rectTransform.sizeDelta = size;

        RectTransform rectTransformPopUpMsg = PopUpMsg.GetComponent<RectTransform>();
        PopUpMsg.alignment = TextAlignmentOptions.Center;

        float height = Popupbackground.rectTransform.rect.height;
        RectTransform rt = Popupbackground.rectTransform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 460f);


        if (rectTransformPopUpMsg != null)
        {
            // Set the height to 1200 while keeping other dimensions unchanged
            rectTransformPopUpMsg.sizeDelta = new Vector2(rectTransformPopUpMsg.sizeDelta.x, 295);
        }
    }

    IEnumerator HidePopUp()
    {
        time = 10.0f;
        yield return new WaitForSeconds(time);

        PopUp.gameObject.SetActive(false);
        RectTransform rectTransform = PopUp.gameObject.GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = 600f; // Set the height to 600
        rectTransform.sizeDelta = size;

        RectTransform rectTransformPopUpMsg = PopUpMsg.GetComponent<RectTransform>();

        if (rectTransformPopUpMsg != null)
        {
            // Set the height to 1200 while keeping other dimensions unchanged
            rectTransformPopUpMsg.sizeDelta = new Vector2(rectTransformPopUpMsg.sizeDelta.x, 295);
        }

        if (Successfullyregistered == true)
        {
        registermobile.SetActive(false);
        SceneManager.LoadScene("loader",LoadSceneMode.Single);
        }
        
    }
}