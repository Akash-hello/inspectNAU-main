using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using DataBank;
using TMPro;

public class SendGridEmailSender : MonoBehaviour
{
    //Replace with your SendGrid API key
    private const string apiKey = "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";
    private const string sendGridUrl = "https://api.sendgrid.com/v3/mail/send";
    public string toEmail;
    public string cc;
    //public List<string>();
    public string subject;
    public string body;

    public GameObject PopUp;
    public TextMeshProUGUI PopUpMsg;

    public registrationmobile fromregistrations;
   

    public void OnSendEmailButtonClicked()
    {
        //toEmail = "msabharwal@orionmarineconcepts.com"; // Replace with the recipient's email
        //subject = ("Test Email");
        //body = "This is a testemail from SendGrid.";
        cc = "orionserver@orionmarineconcepts.com";

        List<string> emails = new List<string> { toEmail, cc };

        foreach (string email in emails)
        {
            SendEmail(email, subject, body);
        }
        //SendEmail(toEmail, subject, body);
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        StartCoroutine(SendEmailCoroutine(toEmail, subject, body));
    }

    private IEnumerator SendEmailCoroutine(string toEmail, string subject, string body)
    {
        
        var emailData = new
        {
            personalizations = new[]
            {
                new { to = new[] {new { email = toEmail }},
                   
                    subject = subject
                }
            },
            from = new { email = "orionapps@orionmarineconcepts.com" }, // Replace with your email
            content = new[]
            {
                new { type = "text/html", value = body }
            }
        };

        string jsonData = JsonConvert.SerializeObject(emailData);
        byte[] postData = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(sendGridUrl, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");

            if (fromregistrations != null)
            {
                fromregistrations.codevalidatingheader.text = "";
            }
            
            PopUpMsg.color = Color.red;
            PopUp.SetActive(true);
            PopUpMsg.text = "Please check your internet connection.";
            //StartCoroutine(HidePopUp());
        }

        else
        {

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending email: " + request.error.ToString());
            }
            else
            {
                if (fromregistrations.forgotpwdclicked == false)
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "OTP sent on your Email. Please fill the other registration details and enter the OTP in the last field to complete registration.";
                    Debug.Log("Email sent successfully!");

                }

                else
                {
                    PopUp.SetActive(true);
                    PopUpMsg.text = "";

                }

            }
        }
        PopUpMsg.color = Color.black;
    }
}