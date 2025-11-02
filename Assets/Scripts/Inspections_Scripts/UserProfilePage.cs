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
    public class UserProfilePage : MonoBehaviour
    {
        public TMP_InputField NameInput;
        public TMP_InputField Company;
        public TMP_InputField CompanyCode;
        public Toggle CompanyCodeToggle;
        public TMP_InputField Designation;
        public TMP_InputField EmailInput;

        public TMP_InputField WhatsappnbrInput;

        string nameinputtext;
        string companynametext;
        string designationtext;
        string emailinputtext;
        string companycodetext;
        public TextMeshProUGUI BalanceTokens;
        public GameObject BalanceTokensFreetext;
        int tokenbalance;


        string WhatsAppNumberinputtext;

        public GameObject PopUp;
        public TextMeshProUGUI PopUpMsg;
        string popuptext;
        public float time = 0.0f; //Seconds to read the text
        public string msgtoserver;
        public Button SubmitTokenRequest;
        public Button CheckTokenStatus;
        int tokenrequestedforcloud;
        bool profileupdate;
        public UserProfilePhoto profilephotoupdate;

        public void Loaduserprofile()

        {

            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.getLatestID();


            if (mlocationDb.LatestId != 0)
            {
                if (mlocationDb.companyguid.Trim().ToLower() == "awaited" || string.IsNullOrEmpty(mlocationDb.companyguid.Trim().ToLower()))
                {
                    CompanyCodeToggle.isOn = false;
                    CompanyCode.interactable = false;
                }

                else //This means there is already a company attached..
                {
                    CompanyCodeToggle.isOn = true;
                    CompanyCodeToggle.interactable = false;
                    CompanyCode.interactable = false;
                    CompanyCode.text = mlocationDb.companyAuthcode.ToString().Trim();
                }

                NameInput.text = mlocationDb.name.ToString();
                Company.text = mlocationDb.companyname.ToString();
                Designation.text = mlocationDb.designation.ToString();
                EmailInput.text = mlocationDb.useremail.ToString();
                WhatsappnbrInput.text = mlocationDb.whatsappnumber.ToString();

                //if (string.IsNullOrEmpty(mlocationDb.companyAuthcode.ToString().Trim()))
                //{
                //    CompanyCodeToggle.isOn = false;
                //    CompanyCode.interactable = false;
                //}

                //else
                //{
                //    CompanyCodeToggle.isOn = true;
                //    CompanyCode.text = mlocationDb.companyAuthcode.ToString().Trim();
                //}
                // CompanyCode.text = mlocationDb;
            }

            else
            {
                PopUp.SetActive(true);
                PopUpMsg.text = "Oops!! Something worng, no profile data found.";
                StartCoroutine(HidePopUp());
            }

            string OriginPath = Application.persistentDataPath + "/" + "Profile" + "/" + "ClientLogoImg.jpg";

            if (File.Exists(OriginPath.ToString()))
            {
                Texture2D texture1 = NativeGallery.LoadImageAtPath(OriginPath, 1024, false);
                profilephotoupdate.clientlogoimg.GetComponent<RawImage>().texture = texture1;
                
            }

            else
            {
                profilephotoupdate.inspectNAU.SetActive(true);
            }

            //mlocationDb.close();
        }


        public void HaveCompanyauthcode()
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

        public void SaveBtnClicked()
        {
            msgtoserver = "";
            profileupdate = false;
            if (NameInput.text == "" || Company.text == "" || (CompanyCodeToggle.isOn == true && CompanyCode.text == "") || Designation.text == "" || WhatsappnbrInput.text == "" || EmailInput.text == "" || EmailInput.text.Contains("@") == false)
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
                    PopUpMsg.text = "Please enter your Company checklists linking code given by your company admin. If you don't have it now, you can add later from the profile menu.";

                }

                else if (Designation.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your designation.";
                }

                else if (EmailInput.text == "" || EmailInput.text.Contains("@") == false)
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter a valid email id.";
                }


                else if (WhatsappnbrInput.text == "")
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Please enter your WhatsApp mobile number.";
                }

                profilephotoupdate.Savefile();
            }

            else
            {
                nameinputtext = NameInput.text;
                companynametext = Company.text;
                designationtext = Designation.text;
                emailinputtext = EmailInput.text;
                companycodetext = CompanyCode.text;
                WhatsAppNumberinputtext = WhatsappnbrInput.text;
                msgtoserver = "updateuser";
                profileupdate = true;
                profilephotoupdate.Savefile();

                if (EmailInput.text.Trim() != "administrator@orion.com")
                {
                    StartCoroutine(SendRegnDataToCloud());
                }

                else
                {
                    UpdateUserProfile();
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Your profile was updated successfully.";
                }
            }
            StartCoroutine(HidePopUp());
        }

        public void TokenRequest() //THIS FUNCTION SHOULD BE ATTACHED TO THE TOKEN REQUEST onto the Gameobject in the Main Inspection scene, Canvas --> Header --> Game Object "TokensRequestprofilePic"
        {
            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.getLatestID();
            PopUp.SetActive(true);

            if (mlocationDb.companyguid.Trim().ToLower() == "awaited" || string.IsNullOrEmpty(mlocationDb.companyguid.Trim().ToLower()))
            {
                PopUpMsg.text = "You have: " + BalanceTokens.text.ToString() + " tokens remaining. " + System.Environment.NewLine + "However, you can not request for tokens as you are not registered with any company on the cloud. Please enter authentication code (case sensitive) for your company from User Profile.";
                SubmitTokenRequest.gameObject.SetActive(false);
                CheckTokenStatus.gameObject.SetActive(false);
            }

            else
            {
                tokenrequestedforcloud = 0;
                PopUpMsg.color = Color.black;
                msgtoserver = "";

                if (mlocationDb.tokenrequestedflag == 1)
                {
                    SubmitTokenRequest.gameObject.SetActive(false);
                    CheckTokenStatus.gameObject.SetActive(true);
                }

                else
                {
                    SubmitTokenRequest.gameObject.SetActive(true);
                    CheckTokenStatus.gameObject.SetActive(false);
                }

                PopUp.SetActive(true);
                int tokenvalueindb = 0;

                if (int.Parse(BalanceTokens.text.ToString()) < 0)

                {
                    tokenvalueindb = Math.Abs(int.Parse(BalanceTokens.text.ToString()));
                    PopUpMsg.text = "You have: " + tokenvalueindb + " FREE token/s remaining. " + System.Environment.NewLine + "You can request for more tokens using the button. If approved by Company admin, it usually takes approx. 30 mins to receive the tokens.This requires live internet connectivity.";

                }

                else
                {
                    PopUpMsg.text = "You have: " + BalanceTokens.text.ToString() + " tokens remaining. " + System.Environment.NewLine + "You can request for more tokens using the button. If approved by Company admin, it usually takes approx. 30 mins to receive the tokens.This requires live internet connectivity.";

                }

            }



            //mlocationDb.close();
        }

        public void TokenRequesttoserver()
        {
            PopUpMsg.color = Color.black;
            msgtoserver = "tokenrequested";// USED FOR tokensrequested";
            tokenrequestedforcloud = 1;
            profileupdate = false; //REQUEST for tokens not profile.
            PopUp.SetActive(true);
            PopUpMsg.text = "Sending Request.....";// the tokens request to the server, please wait few seconds for server confirmation for receipt of your request.";
            StartCoroutine(SendRegnDataToCloud());
        }

        public void CheckingTokenStatus()
        {
            PopUpMsg.color = Color.black;
            msgtoserver = "Tokensstatus";// USED FOR tokensrequested";
            tokenrequestedforcloud = 1;
            profileupdate = false; //REQUEST for tokens not profile.
            PopUp.SetActive(true);
            PopUpMsg.text = "Checking, please wait....";// the tokens request to the server, please wait few seconds for server confirmation for receipt of your request.";
            StartCoroutine(SendRegnDataToCloud());
        }

        IEnumerator SendRegnDataToCloud()
        {
            popuptext = "";

            SubmitTokenRequest.gameObject.SetActive(false);
            CheckTokenStatus.gameObject.SetActive(false);
            string result = "";
            string guid = "";
            string columndataquery = "";

            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.getLatestID();

            //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = "ankush1maurya@gmail.com", password = "123456" });
            //string bodyJsonString = JsonUtility.ToJson(new AuthData() { email = emailinputtext, password = passwordinputtext });
            string bodyJsonString = "";

            if (profileupdate == true)
            {
                bodyJsonString = JsonUtility.ToJson(new AuthData()
                {
                    neworupdate = msgtoserver,
                    name = nameinputtext.Replace("'", "''"),
                    companyname = companynametext.Replace("'", "''"),
                    companycode = companycodetext.Replace("'", "''"),
                    designation = designationtext.Replace("'", "''"),
                    email = emailinputtext.Replace("'", "''"),
                    password = mlocationDb.password.Trim(),
                    WhatsAppNumber = WhatsAppNumberinputtext,
                    tokenremaining = int.Parse(BalanceTokens.text.ToString()),
                    tokenrequest = 0

                });
            }

            else if (profileupdate == false && msgtoserver == "tokenrequested")

            {
                bodyJsonString = JsonUtility.ToJson(new AuthData()
                {
                    neworupdate = msgtoserver,
                    name = mlocationDb.name,
                    companyname = mlocationDb.companyname,
                    companycode = mlocationDb.companyAuthcode,
                    designation = mlocationDb.designation,
                    email = mlocationDb.useremail,
                    password = mlocationDb.password.Trim(),
                    WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),
                    tokenremaining = int.Parse(BalanceTokens.text.ToString()),
                    tokenrequest = tokenrequestedforcloud,
                    granttoken = 0
                });
            }

            else if (profileupdate == false && msgtoserver == "Tokensstatus")

            {
                bodyJsonString = JsonUtility.ToJson(new AuthData()
                {
                    neworupdate = msgtoserver,
                    name = mlocationDb.name,
                    companyname = mlocationDb.companyname,
                    companycode = mlocationDb.companyAuthcode,
                    designation = mlocationDb.designation,
                    email = mlocationDb.useremail,
                    password = mlocationDb.password.Trim(),
                    WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),
                    tokenremaining = int.Parse(BalanceTokens.text.ToString()),
                    tokenrequest = tokenrequestedforcloud,
                    granttoken = 0
                });
            }

            else if (profileupdate == false && msgtoserver == "updatetokenvalues")

            {
                bodyJsonString = JsonUtility.ToJson(new AuthData()
                {
                    neworupdate = msgtoserver,
                    name = mlocationDb.name.Trim(),
                    companyname = mlocationDb.companyname.Trim(),
                    companycode = mlocationDb.companyAuthcode,
                    designation = mlocationDb.designation.Trim(),
                    email = mlocationDb.useremail,
                    password = mlocationDb.password.Trim(),
                    WhatsAppNumber = mlocationDb.whatsappnumber.ToString(),
                    tokenremaining = mlocationDb.tokenbalance,
                    tokenrequest = 0,
                    granttoken = 0
                });
            }

            var request = new UnityWebRequest("https://hsseqapi.orionmarineconcepts.com/api/User_Registration/Post", "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            try
            {
                string rawJson = Encoding.Default.GetString(request.downloadHandler.data);

                if (String.IsNullOrEmpty(rawJson))
                {
                    PopUpMsg.color = Color.red;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Oops! you need internet access for this action, seems you are offline, please try when online or in good connectivity."; //Oops! did not connect with server, maybe you are offline, please try when online or in good connectivity."
                    //StartCoroutine(HidePopUp());
                }

                //message = tokenObj.message.ToString();
                else if (rawJson.Contains("Error"))
                {
                    PopUpMsg.color = Color.red;
                    Debug.Log("Invalid user.");
                    PopUp.SetActive(true);
                    PopUpMsg.text = "There was an error while updating data, either you are offline, else contact the support team.";
                    //StartCoroutine(HidePopUp());
                }

                else if (rawJson.Contains("No Authentication sent"))

                {
                    PopUpMsg.color = Color.black;
                    PopUp.SetActive(true);

                    PopUpMsg.text = "Profile updated an INDEPENDENT user as no Company code entered.";

                    columndataquery = "CompanyCode = '', CompanyGuid = 'Awaited'";

                    mlocationDb.Updatedata(columndataquery);
                    //mlocationDb.close();
                    UpdateUserProfile();

                    //StartCoroutine(HidePopUp());
                }

                else if (rawJson.Contains("Incorrect Authentication code"))

                {
                    PopUpMsg.color = Color.black;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Profile updated an INDEPENDENT user as invalid Company code entered.";
                    //StartCoroutine(HidePopUp());

                    columndataquery = "CompanyCode = '', CompanyGuid = 'Awaited'";

                    mlocationDb.Updatedata(columndataquery);
                    //mlocationDb.close();
                    UpdateUserProfile();
                }

                else if (rawJson.Contains("Successful"))
                {
                    PopUpMsg.color = Color.black;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Profile updated successfully.";
                    //StartCoroutine(HidePopUp());

                    guid = rawJson.Split('=')[1];

                    columndataquery = "CompanyGuid = '" + guid.Trim().Replace("\"", "") + "'";
                    CompanyCodeToggle.isOn = true;
                    CompanyCodeToggle.interactable = false;
                    CompanyCode.interactable = false;

                    mlocationDb.Updatedata(columndataquery);
                    //mlocationDb.close();
                    UpdateUserProfile();
                }

                else if (rawJson.Contains("token requested"))
                {
                    PopUpMsg.color = Color.black;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "Tokens requested received on server. If approved by Company admin, time depends on the company admins approval. Internet connectivity required for updating tokens on APP.";
                    //StartCoroutine(HidePopUp());

                    columndataquery = "Tokendate = '1'";
                    SubmitTokenRequest.gameObject.SetActive(false);
                    CheckTokenStatus.gameObject.SetActive(true);

                    mlocationDb.Updatedata(columndataquery);
                    //mlocationDb.close();
                    

                }

                else if (rawJson.Contains("Values Updated"))
                {
                    PopUpMsg.color = Color.black;
                    PopUp.SetActive(true);
                    PopUpMsg.text = "The server has been updated with your token values.";
                    //StartCoroutine(HidePopUp());

                    SubmitTokenRequest.gameObject.SetActive(true);
                    CheckTokenStatus.gameObject.SetActive(false);

                    columndataquery = "Tokendate = '0'";
                    mlocationDb.Updatedata(columndataquery);

                    if (mlocationDb.tokenbalance < 0) //FREE CREDITS MODE
                    {
                        BalanceTokensFreetext.SetActive(true);
                        BalanceTokens.text = mlocationDb.tokenbalance.ToString();
                        BalanceTokens.GetComponent<TMP_Text>().color = Color.white;

                    }

                    else if (mlocationDb.tokenbalance <= 2)
                    {
                        BalanceTokensFreetext.SetActive(false);
                        BalanceTokens.text = mlocationDb.tokenbalance.ToString();
                        BalanceTokens.GetComponent<TMP_Text>().color = Color.red;
                    }

                    else
                    {
                        BalanceTokensFreetext.SetActive(false);
                        BalanceTokens.text = mlocationDb.tokenbalance.ToString();
                        BalanceTokens.GetComponent<TMP_Text>().color = Color.black;
                    }
                   
                   // mlocationDb.close();
                    StartCoroutine(HidePopUp());
                }


                else if (rawJson.Contains("flag")) //Tokensstatus checking
                {
                    string flagtext = "";
                    int flag = 0;

                    string granttext = "";
                    int grantedvalue = 0;
                    string status = "";


                    //StartCoroutine(HidePopUp());

                    flagtext = rawJson.Split(',')[0];
                    flag = int.Parse(flagtext.Split('=')[1]);

                    granttext = rawJson.Split(',')[1];
                    grantedvalue = int.Parse(granttext.Split('=')[1]);

                    status = rawJson.Split(',')[2]; ;

                    if (grantedvalue < 0) // Declined when -1 is received...
                    {
                        columndataquery = "Tokendate = '0'";

                        PopUpMsg.color = Color.black;
                        PopUp.SetActive(true);
                        PopUpMsg.text = "Tokens declined by server.";

                        SubmitTokenRequest.gameObject.SetActive(true);
                        CheckTokenStatus.gameObject.SetActive(false);
                        mlocationDb.Updatedata(columndataquery);
                        //mlocationDb.close();
                        msgtoserver = "updatetokenvalues";
                        StartCoroutine(SendRegnDataToCloud());
                    }

                    else if (grantedvalue > 0) // ALLOCATED
                    {
                        int updatetokensinDB = 0;
                        if (int.Parse(BalanceTokens.text.ToString()) <= 0)
                        {
                            updatetokensinDB = grantedvalue;
                        }

                        else
                        {
                            updatetokensinDB = grantedvalue + int.Parse(BalanceTokens.text.ToString());
                        }

                        columndataquery = "Tokendate = '0', TokenBalance = '" + updatetokensinDB + "'";

                        PopUpMsg.color = Color.black;
                        PopUp.SetActive(true);
                        PopUpMsg.text = "Hooray!! you have been granted " + grantedvalue + " Tokens by the server.";

                        SubmitTokenRequest.gameObject.SetActive(true);
                        CheckTokenStatus.gameObject.SetActive(false);
                        mlocationDb.Updatedata(columndataquery);
                        //mlocationDb.close();
                        msgtoserver = "updatetokenvalues";
                        StartCoroutine(SendRegnDataToCloud());
                    }
                    else // PENDING
                    {
                        PopUpMsg.color = Color.black;
                        PopUp.SetActive(true);
                        PopUpMsg.text = "Tokens approval pending from server.";

                        columndataquery = "Tokendate = '1'";
                        SubmitTokenRequest.gameObject.SetActive(false);
                        CheckTokenStatus.gameObject.SetActive(true);
                        mlocationDb.Updatedata(columndataquery);
                    }

                }

                else
                {
                    PopUp.SetActive(true);
                    PopUpMsg.color = Color.black;
                    PopUpMsg.text = "Did not receive any response, please try again, if you are the administrator, this data is static and will not be updated.";
                }

                profileupdate = false;
                //mlocationDb.close();

            }

            catch (Exception ex)
            {

                Debug.Log(request.error + " ex " + ex.Message + "There was a error on this line" + ex.StackTrace);
                //Debug.LogError("Response Code: " + request.responseCode);
                //Debug.LogError("Response: " + request.downloadHandler.text);
                PopUpMsg.color = Color.red;
                PopUp.SetActive(true);
                PopUpMsg.text = "Oops !! Something went wrong, please check your internet connection or contact support team for details.";
                //StartCoroutine(HidePopUp());
            }


        }

        public void UpdateUserProfile()
        {
            table_LoginConfig mlocationDb = new table_LoginConfig();
            using var connection = mlocationDb.getConnection();
            mlocationDb.getLatestID();
            if (mlocationDb.LatestId != 0)
            {
                tokenbalance = mlocationDb.tokenbalance;
            }
            string columndataquery = "Name = '"+ nameinputtext.Replace("'", "''") + "', Company = '"+ companynametext.Replace("'", "''") + "', CompanyCode = '" + companycodetext.Replace("'", "''") + "', Designation = '" + designationtext.Replace("'", "''") + "', Email = '" + emailinputtext.Replace("'", "''") + "', Phone = '" + WhatsAppNumberinputtext + "' where cast(Id as int) = "+ mlocationDb.LatestId;
            mlocationDb.Updatedata(columndataquery);
            //PopUp.SetActive(true);
            //PopUpMsg.text = popuptext;

            //if (CompanyCodeToggle.isOn == true && CompanyCode.text !="")
            //{
            //    StartCoroutine(SendRegnDataToCloud());
            //}

            //mlocationDb.close();
            Loaduserprofile();
        }



        public void onclickAuthCautionbtn()

        {
            PopUp.SetActive(true);
            //PopUpMsg.text = "This is a Company Authentication Code.";//This is a Company Linking Code, this is required to download and configure all your Company Checklists onto your device.
            PopUpMsg.text = "<b>Company Inspector:</b> This allows you to connect and download SIRE 2.0 and RightShip checklists onto your device." + Environment.NewLine + "<b>Independent Inspector:</b> enter <b>ORION</b> as AUTHCODE to download only SIRE 2.0 and RightShip checklists.";

            StartCoroutine(HidePopUp());
        }
        IEnumerator HidePopUp()
    {
        time = 5.0f;
        yield return new WaitForSeconds(time);

        PopUp.gameObject.SetActive(false);
    }
}
}
