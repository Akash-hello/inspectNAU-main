using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DataBank;
using System;
using System.IO;
using UnityEngine.Networking;

public class logoutmanager : MonoBehaviour
{
    public GameObject LogoutConfirm;
    public string OriginPath;
    string filename;
    public RawImage m_RawImage;
    //public RawImage Profilepic;
    bool Media;
    public GameObject defaulticon;
    string mediafilespath;
   
    // Start is called before the first frame update
    void Awake()
    {
       
        LogoutConfirm.SetActive(false);

        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();

        if (mlocationDb.LatestId != 0 && !String.IsNullOrEmpty(mlocationDb.profilephotoname))
        {
            filename = mlocationDb.profilephotoname.Trim().ToString();
            OriginPath = Application.persistentDataPath + "/" + "Profile" + "/" + filename;

            if (File.Exists(OriginPath.ToString()))
            {
                Media = true;
                StartCoroutine(LoadTexture());
            }

            else
                defaulticon.SetActive(true);
        }

        else
        {
            defaulticon.SetActive(true);
        }

        //mlocationDb.close();
    }

    // Update is called once per frame

    public void Logout()
    {
        LogoutConfirm.SetActive(true);
        //SceneManager.LoadScene("AIR_TOUCH_WELCOME");
    }
    
    public void YesLogout()
    {
        LogoutConfirm.SetActive(false);

        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        //Update Data
        //mlocationDb.addData(new LoginEntity(id, email, passwordinputtext, phone, apitoken, tokenvalidity, "NSE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));
        mlocationDb.Updatedata(" Sessionstate = '" + "N" + "' ,TimeStamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "'");//where Id = '" + mlocationDb.LatestId + "'");

        SceneManager.LoadScene("LoginSignup1");
       // mlocationDb.close();
    }

    public void NoDontLogout()
    {
        LogoutConfirm.SetActive(false);
    }

   

    IEnumerator LoadTexture()
    {
        //if (Media == true)
        //{
        //    WWW www = new WWW(OriginPath);
        //    while (!www.isDone)
        //        yield return null;
        //    m_RawImage.GetComponent<RawImage>().texture = www.texture;
            
        //    defaulticon.SetActive(false);
        //}

        //else
        //{
        //    defaulticon.SetActive(true);
        //}


        if (Media == true)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(OriginPath);

            request.uri = new Uri(request.uri.AbsoluteUri.Replace("https://localhost", "file://"));

            yield return request.SendWebRequest();

            while (!request.isDone) yield return request;

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                defaulticon.SetActive(true);
            }
            else
            {
                Debug.Log("Successfully Downloaded image.");
                var texture = DownloadHandlerTexture.GetContent(request);
                m_RawImage.GetComponent<RawImage>().texture = texture;
                defaulticon.SetActive(false);
                //LogoutBtnImg.GetComponent<RawImage>().texture = texture;
            }


        }
    }

}

