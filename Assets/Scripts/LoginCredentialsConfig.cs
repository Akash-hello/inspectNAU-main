using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.IO;

namespace DataBank
{

    public class LoginCredentialsConfig : MonoBehaviour
{
    public TMP_InputField EmailInput; //Also used for Emailid or Mobile Number on UI
    public TMP_InputField PasswordInput;
    public TwilioMessaging msgbody;
    public SendGridEmailSender emailsender;
    string emailinputtext;
    string passwordinputtext;
    public GameObject PopUp;
    public TextMeshProUGUI PopUpMsg;
    public int LoginBtnIdentity; // = 1 means online Mobile, = 2 for Online login check.

    //For email sending
    string toEmail = "";
    string subject = "";
    string body = "";


        int id;
        string name = "";
        string company = "";

        string companycodetext = "";
        string companyguid = "";
        int    token = 0;
        string UserTypetoggle = "";
        int    vesselimo = 9999999;
        string VslTypeValue = "";

        string designation = "";
        string email = "";
        string password = "";
        string phone = "";
        string apitoken = "";
        string tokenvalidity = "";
        string marketset = "";
        string markets = "";
        string sessionstate = "";
        string timestamp;
        string result = "";
        string message = "";
        
        //string[] subscribedmarkets;
        DateTime now;
        long unixTimeStampInSeconds;

    public float time = 8.0f; //Seconds to read the text

        registrationmobile registeradministrator;

        //Solved issues of; unable to close due to unfinalized statements or unfinished backups on XCODE, by opening and closing connections on the table_LoginConfig.cs and using finally to
        //close and dispose reader, db as well as the connection.
        //mistakes made on calling from other function, was using "USING table_LoginConfig mlocationDb = new table_LoginConfig();", do not use "USING", simply table_LoginConfig ml.......
        //The variables can be called as returns from other class or even "`mlocationDb.LatestId", this will not cause any issues now...

        private void Awake()
        {
            int latestid = 0 ;
            string session = "";
            now = DateTime.Now;
            LoginBtnIdentity = 0;
            
            unixTimeStampInSeconds = ((DateTimeOffset)now).ToUnixTimeSeconds();
            var mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.getLatestID();
            latestid = mlocationDb.LatestId;
            session = mlocationDb.Session;

            //CLOSED Debug.Log("Session is active ?? " + mlocationDb.Session.ToString());//+" Market set is; " + mlocationDb.MarketSet.ToString());
            if (latestid != 0)
            {
                if (session == "Y")// REMOVED THIS FROM CHECKING AS THE FILED WAS USED FOR reuqesting Tokens and not subscription && mlocationDb.subscriptionvalid > unixTimeStampInSeconds)
                {
                    //Debug.Log("USER SESSION IS ACTIVE AND TOKEN VALID.");
                    connection.Close();
                    //SceneManager.LoadScene("InspectionsMainScene", LoadSceneMode.Single);
                    SceneManager.LoadScene("loader",LoadSceneMode.Single);
                }

            }
            connection.Close();
            //StartCoroutine(CheckLoginTable());
            //mlocationDb.close();
        }

        //IEnumerator CheckLoginTable()
        //{
        //    time = 3.0f;
        //    yield return new WaitForSeconds(time);

        //    table_LoginConfig mlocationDb = new table_LoginConfig();
        //    mlocationDb.getLatestID();

        //    //Debug.Log("Session is active ?? " + mlocationDb.Session.ToString());//+" Market set is; " + mlocationDb.MarketSet.ToString());
        //    if (mlocationDb.LatestId != 0)
        //    {
        //        if (mlocationDb.Session == "Y" && mlocationDb.subscriptionvalid > unixTimeStampInSeconds)
        //        {
        //            //Debug.Log("USER SESSION IS ACTIVE AND TOKEN VALID.");
        //            SceneManager.LoadScene("EquityAnalyzeMainScene");
        //            //SceneManager.LoadScene("loader");
        //        }

        //    }

        //}

        public void LoginButtonMobile ()

        {
            LoginBtnIdentity = 1;
            LoginButtonClick();

        }

        public void LoginButtonOnlineCheck()

        {
            LoginBtnIdentity = 2;
            LoginButtonClick();

        }

        public void LoginButtonClick ()

    {
            if (EmailInput.text == "" || PasswordInput.text == "" )//|| EmailInput.text.Contains("@") == false)
            {
                if (EmailInput.text == "" )//|| EmailInput.text.Contains("@") == false)
                {
                PopUp.SetActive(true);
                PopUpMsg.text = "Please enter your registered email id."; 
                }
                
                else if (PasswordInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your password.";
                }
                LoginBtnIdentity = 0;
            }

            else if (LoginBtnIdentity == 1) //Local Mobile DB login Check.
            {
                emailinputtext = EmailInput.text;
                passwordinputtext = PasswordInput.text;
                PopUp.SetActive(true);
                PopUpMsg.text = "Thank you, please wait while we check your credentials.";
                LoginBtnIdentity = 0;
                LoginMobileCheck();
            }

            else if (LoginBtnIdentity == 2) // Online user identification and Check.
            {
                emailinputtext = EmailInput.text;
                passwordinputtext = PasswordInput.text;
                PopUp.SetActive(true);
                PopUpMsg.text = "Thank you, please wait while we check your credentials.";
                LoginBtnIdentity = 0;
                StartCoroutine(Login());
            }

            StartCoroutine(HidePopUp());

        }

        
    IEnumerator Login()
    {
            
                //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = "ankush1maurya@gmail.com", password = "123456" });
                string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = emailinputtext, password = passwordinputtext });
                var request = new UnityWebRequest("https://api.equityanalyze.com/token", "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                string rawJson = Encoding.Default.GetString(request.downloadHandler.data);
                TokenData tokenObj = TokenData.CreateFromJSON(rawJson);
                //Debug.Log(tokenObj.token);
                //Debug.Log(tokenObj.message);
                //Debug.Log(tokenObj.result);
                if (String.IsNullOrEmpty(rawJson))
                {
                    PopUpMsg.color = Color.red;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Login failed! Please check your connection.";
                    StartCoroutine(HidePopUp());
                }
                else
                {
                    result = tokenObj.result.ToString();

                }

                //message = tokenObj.message.ToString();
                if (result == "error"|| result == "Error")
                {
                    PopUpMsg.color = Color.red;
                    Debug.Log("Invalid user.");
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Login failed! Please check your login details.";
                    StartCoroutine(HidePopUp());
                }

                if (String.IsNullOrEmpty(rawJson))

                {
                    PopUpMsg.color = Color.red;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Login failed! Please check your connection.";
                    StartCoroutine(HidePopUp());
                }

                else
                {

                    string token = tokenObj.token;
                    var parts = token.Split('.');

                    if (parts.Length > 2)
                    {
                        var decode = parts[1];
                        var padLength = 4 - decode.Length % 4;
                        if (padLength < 4)
                        {
                            decode += new string('=', padLength);
                        }
                        var bytes = System.Convert.FromBase64String(decode);
                        var userInfo = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                        AuthData userObj = AuthData.CreateFromJSON(userInfo);
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(userObj.exp);
                        //subscribedmarkets = new string[] { "" };

                        if (userObj.exp > unixTimeStampInSeconds && result == "success")
                        {
                            Debug.Log("User credentials are valid.");
                            email = userObj.email.ToString();
                            apitoken = userObj.userid.ToString();
                            //tokenvalidity = userObj.exp.ToString();
                        tokenvalidity = "0"; //THIS FIELD WAS USED FOR Whether the user has generated a token request or not...

                        if (userObj.subscriptions != "")
                            {

                                //subscribedmarkets = userObj.subscriptions.Split(',').ToArray();
                                markets = userObj.subscriptions.ToUpper();
                                AddData();
                                SceneManager.LoadScene("loader", LoadSceneMode.Single);
                            }
                            else
                            {
                                PopUpMsg.color = Color.red;
                                PopUp.SetActive(true);
                                PopUpMsg.text = "Oops! You have not subscribed to any stock exchanges.";
                            }

                        }
                        else if (userObj.exp < unixTimeStampInSeconds)
                        {
                            PopUpMsg.color = Color.red;
                            Debug.Log("Subscription expired.");
                            PopUp.SetActive(true);
                            PopUpMsg.text = "Oops! Subscription expired, please check and renew.";
                        }

                        PopUpMsg.color = Color.black;
                        // marketset
                        StartCoroutine(HidePopUp());
                    }
                }
            
       
            
            //JSONNode jsonResult = JSON.Parse(rawJson);
            //PrintOnScreen(jsonResult)

        }

    public void LoginMobileCheck()

        {
           
            //THIS AREA IS FOR GIVING ADMIN ACCESS FOR APP Store to test and also 30 day's validity on a direct Password...
            DateTime olderDate = new DateTime(2025, 06, 01); // Year, Month, Day
            // Get today's date
            DateTime today = DateTime.Today;
            if (EmailInput.text.ToString().ToLower() == "administrator@orion.com" && PasswordInput.text.ToString() == "test@permit007" && today < olderDate.AddDays(90))
            {
                registeradministrator = new registrationmobile();

                registeradministrator.nameinputtext = "Administrator";
                registeradministrator.companynametext = "Test Company";
                registeradministrator.designationtext = "Test Designation";
                registeradministrator.emailinputtext = "administrator@orion.com";
                registeradministrator.passwordinputtext = "test@permit007";
                registeradministrator.Countrcodeinputtext = "91";
                registeradministrator.WhatsAppNumberinputtext = "9810606650";
                registeradministrator.companycodetext = "AUBH2";
                registeradministrator.companyguid = "xdMP^2Id7L";
                registeradministrator.tokenbalance = -2;
                registeradministrator.UserTypetoggle = "Individual";
                registeradministrator.vesselimo = 9999999;
                registeradministrator.VslTypeValue = "-";
                registeradministrator.IncomingAPI = "https://hsseqapi.orionmarineconcepts.com/api/HSSEQ/Get?authkey=";
                registeradministrator.tokenvalidity = "4070929577";
                registeradministrator.marketset = "";
                registeradministrator.markets = "NSE,BSE,NASDAQ,NYSE,LSE";
                registeradministrator.sessionstate = "N";

                registeradministrator.directlogin = true;

                registeradministrator.AddData();
                SceneManager.LoadScene("loader", LoadSceneMode.Single);
            }

            else //For Normal Users...
            {
                int latestid = 0;
               
                table_LoginConfig mlocationDb = new table_LoginConfig();
                using var connection = mlocationDb.getConnection();
                mlocationDb.getLatestID();
                latestid = mlocationDb.LatestId;

                if (latestid != 0)
                {
                    if ((mlocationDb.useremail.ToString().ToLower() == EmailInput.text.ToString().ToLower() || mlocationDb.whatsappnumber.ToString() == EmailInput.text.ToString()) && mlocationDb.password.ToString() == PasswordInput.text.ToString())
                    {
                        msgbody.body = "";
                        Debug.Log("CHECKED THE CREDENTIALS");
                        mlocationDb.Updatedata(" Sessionstate = '" + "Y" + "' ,TimeStamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "'");//where Id = '" + mlocationDb.LatestId + "'
                        msgbody.body = "Hello *" + mlocationDb.useremail.ToString() + "*, welcome to Inspect NAU, you can manage your account online from; inspectnau.orionmarineconcepts.com/loginpage.aspx (use same login credentials as the mobile APP).";
                        msgbody.Content_Sid = "HX4ea2d9b661f0e1c9c0ddf032bd9a924a";
                        msgbody.SendSMS();

                        emailsender.toEmail = mlocationDb.useremail.ToString(); // Replace with the recipient's email
                        emailsender.subject = "Welcome to inspectNAU";

                        //string url = "https://inspectnau.orionmarineconcepts.com/loginpage.aspx";

                        //emailsender.body = "Hi " + mlocationDb.name.ToString() + ","+ Environment.NewLine + Environment.NewLine + "Welcome to Inspect NAU. " + Environment.NewLine + "You can manage your account online from; inspectnau.orionmarineconcepts.com/loginpage.aspx (use same login credentials as the mobile APP)." + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";

                        emailsender.body = "Dear " + mlocationDb.name.ToString() + ",<br><br>";

                        emailsender.body += "Thank you for logging into InspectNAU!<br><br>";

                        emailsender.body += "<strong>Managing your credits:</strong><br><hr>";
                        emailsender.body += "To help you experience the full potential of InspectNAU, we are offering <strong>two free inspections and a sample template to try out (this does not effect the credits).</strong><br>";
                        emailsender.body += "After that, you can conveniently purchase additional credits online by logging in with the same credentials as your app login at:<br>";
                        emailsender.body += "<a href='https://inspectnau.orionmarineconcepts.com/'>InspectNAU Online</a>.<br><br>";

                        emailsender.body += "<strong>Want Custom Checklists Included?</strong><br><hr>";
                        emailsender.body += "If you need your own custom checklists (inspections and/or audits) included in InspectNAU, connect with us and we’ll help you onboard them seamlessly.<br>";
                        emailsender.body += "You can reach out to us at <a href='mailto:orionapps@orionmarineconcepts.com'>orionapps@orionmarineconcepts.com</a> to get started or if you have any questions, our team is here to assist you.<br><br>";

                        emailsender.body += "Thank you & Best Regards,<br><br>";
                        emailsender.body += "<strong>InspectNAU Administrator</strong><br><br>";
                        emailsender.body += "Orion Marine Concepts<br>";
                        emailsender.body += "<a href='https://orionmarineconcepts.com/'>Company Website</a> | ";
                        emailsender.body += "<a href='mailto:orionapps@orionmarineconcepts.com'>msabharwal@orionmarineconcepts.com</a><br>";



                        //emailsender.body = "This is a testemail from SendGrid.";

                        //  emailsender.body = "Dear " + nameinputtext.ToString() + ", " + Environment.NewLine + Environment.NewLine + "Your six digit email authentication code is: " + code.ToUpper() + " please enter on the APP screen to validate your email and complete the registration process." + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";

                        emailsender.OnSendEmailButtonClicked();

                        SceneManager.LoadScene("loader", LoadSceneMode.Single);
                       
                    }

                    else
                    {
                        Debug.Log(mlocationDb.useremail.ToString() + "_____" + mlocationDb.whatsappnumber.ToString() + "_____" + mlocationDb.password.ToString());
                        Debug.Log("Invalid Details.");
                        PopUp.SetActive(true);
                        PopUpMsg.text = "Login failed! Please check your username and/or password details.";
                        StartCoroutine(HidePopUp());
                    }
                }

                else
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Login failed! You don't seem to be registered, please register and try again.";
                    StartCoroutine(HidePopUp());
                }
                //mlocationDb.close();
            }
            
        }

        public void ForgotpasswordOld() //THIS WAS MOVED TO THE REGISTRATIONMOBILE.cs and made into a reset button instead of sending password on email.
        {
            string name = "";
            string useremail = "";
            string whatsappnum = "";
            string password = "";

            int latestid = 0;

            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.getLatestID();
            latestid = mlocationDb.LatestId;

            if (latestid != 0)
            {
                name = mlocationDb.name.ToString();
                useremail = mlocationDb.useremail.ToString();
                whatsappnum = mlocationDb.whatsappnumber.ToString();
                password = mlocationDb.password.ToString(); //DO NOT SEND PASSWORD ON EMAIL.
                //password = "********";

                // msgbody.body = "";
                // msgbody.body = "Dear *" + mlocationDb.name.ToString()+ "* here are your credentials for login, Emailid: *" + mlocationDb.useremail.ToString() + "*, Phone number: *" + mlocationDb.whatsappnumber.ToString() + "* and password: *" + mlocationDb.password.ToString() + "*. Thank you.";


                string contentVariables = JsonConvert.SerializeObject(new Dictionary<string, System.Object>()
        {
            {"1", name},
            {"2", "Inspect NAU"},
            {"3", useremail},
            {"4", whatsappnum},
            {"5", password}
        }, Formatting.Indented);

                msgbody.contentvariables = contentVariables;
                //msgbody.Content_Sid = "HX8cf6e1e6b8116e9455cc2a7f1faecdb3";
                msgbody.Content_Sid = "HX4ea2d9b661f0e1c9c0ddf032bd9a924a";
                //msgbody.body = "Hello *mohit*, welcome to EquityAnalyze.";
                msgbody.SendSMS();

                emailsender.toEmail = useremail; // Replace with the recipient's email
                emailsender.subject = "inspectNAU - Forgot Password Clicked - your credentials";
                //emailsender.body = "Hi " + name + Environment.NewLine + Environment.NewLine + "You have clicked forgot password, here are your login credentials for the inspectNAU mobile APP, email; " + useremail + " and phone number; " + whatsappnum + " with password; " + password + ". Thank you." + Environment.NewLine + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";

                emailsender.body = "Hi " + name +","+ Environment.NewLine + Environment.NewLine + "You have clicked forgot password, please reset your password from the inspectNAU mobile APP" + Environment.NewLine + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";


                //emailsender.body = "This is a testemail from SendGrid.";

                //emailsender.body = "Dear " + nameinputtext.ToString() + ", " + Environment.NewLine + Environment.NewLine + "Your six digit email authentication code is: " + code.ToUpper() + " please enter on the APP screen to validate your email and complete the registration process." + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";

                emailsender.OnSendEmailButtonClicked();
            }

            else
            {
                PopUp.SetActive(true);
                PopUpMsg.text = "You are not registered.";
                StartCoroutine(HidePopUp());
            }

        }

        public void InitiliaseDropTables()
        {
            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.DropTable();

            //table_Config mlocationDb1 = new table_Config();
            //using var connection1 = mlocationDb1.getConnection();
            //mlocationDb1.DropTable();

        }

       //public void AddummyData()
       // {
       //     table_LoginConfig mlocationDb = new table_LoginConfig();
       // mlocationDb.addData(new LoginEntity(0, "ankush1maurya@gmail.com", "123456", "919810606650", "APITOKENHEREssassasasasasasas", "2022-05-12", "NSE", "NSE,BSE,XLON,NYSE,HKG", "N",DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

       // }

        public void AddData()
        {
            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            // mlocationDb.getLatestID();

            //Debug.Log("Session is active ?? " + mlocationDb.Session.ToString());//+" Market set is; " + mlocationDb.MarketSet.ToString());
            int count = mlocationDb.totalRecords("LoginConfig");
            Debug.Log("Count Found" + count);
           if (mlocationDb.totalRecords("LoginConfig") == 0)
           {
                Debug.Log("inside if");
                mlocationDb.deleteAllData();
                Debug.Log("deleted");
                //mlocationDb.vacuumAllData();
                //Debug.Log("vacuumAllData");
                //Add Data
                mlocationDb.addData(new LoginEntity(id, name, company, designation, email, passwordinputtext, phone, companycodetext, companyguid, token, UserTypetoggle, vesselimo, VslTypeValue, apitoken, tokenvalidity, marketset, markets, sessionstate,"newuser.jpeg", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
                //NEW FOLDER CREATED FOR STORING USERPROFILE DATA, LIKE PHOTO OR OTHER...                           
                Directory.CreateDirectory(Application.persistentDataPath + "/" + "Profile");                       
                Debug.Log("Data Added");
                mlocationDb.close();
            }                                                                                                      
            else //if (mlocationDb.LatestId == 1)                                                                  
            {
                mlocationDb.Updatedata(" Email = '" + email + "' ,Password = '" + passwordinputtext + "' ,Apitoken = '" + apitoken + "' ,Tokendate = '" + tokenvalidity + "' ,Sessionstate = '" + "Y" + "' ,UserSubscribedmarkets = '" + markets + "' ,TimeStamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "'");//where Id = '" + mlocationDb.LatestId + "'
               
                mlocationDb.close();
            }
            
        }

        IEnumerator HidePopUp()
        {
            time = 8.0f;
            yield return new WaitForSeconds(time);

            PopUp.gameObject.SetActive(false);
        }
    }

[Serializable]
public class AuthData
{

public string neworupdate;
public string name;
public string companyname;
public string companycode;
public string designation;
public string email;
public string password;
public string WhatsAppNumber;
public string usertype;
public string vslType;
public int imonumber;
public int tokenremaining;
        public int totaltokens; 
        public int tokenrequest; // only a flag with value 1 if requested and 0 once completed.
        public int granttoken; // for receiving from cloud server
        public string userid;
        public string Countrycode;
        public string companyguid;
        public Int64  exp;
        public string subscriptions;

        public static AuthData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<AuthData>(jsonString);
        }
    }

    [System.Serializable]
    public class TokenData
    {
        public string result;
        public string token;
        public string message;

        public static TokenData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<TokenData>(jsonString);
        }
    }

    [Serializable]
    public class TwilioContentVariables
    {
        public string one;
        public string two;
        public string three;
        public string four;
        public string five;
        public string six;
    }
}

