using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System;

public class NewsRequest : MonoBehaviour
{
    public string url;
    public string countrycode;
    //public string countryname;
    public Text headingcountryname;
    public Text newsource;
    public Text displaynewsitem;
    public GameObject newsprefab;
    private GameObject newsitems;
    public RectTransform ParentPanel;
    private Button readmore;
    public string readmoreURL;
    public Gaze Gazescript;

    public int open;
    public Texture2D dummytexture;
    Texture2D myTexture;

    public void Getnewsdata()
    {
        //countrycode = "gb";
        //ParentPanel = Gazescript.GoHasinfo.transform.Find("SectionInfo/Canvas/Panel/basepanel/Containerpanel").GetComponent<RectTransform>();
        //Gazescript.shownews = false;
        for (int i = 0; i < ParentPanel.transform.childCount; ++i) { Destroy(ParentPanel.transform.GetChild(i).gameObject); }
        //url = "https://newsapi.org/v2/top-headlines?country=" + "in" + "&category=business&apiKey=e0df765363a6472ab3f6bd14670fe1bf";
        url = "https://newsapi.org/v2/top-headlines?country=" + countrycode + "&category=business&apiKey=e0df765363a6472ab3f6bd14670fe1bf";
        
        StartCoroutine(NewsArticlerequest());
        
    }


   public IEnumerator NewsArticlerequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
       
        if (request.isNetworkError||request.isHttpError)
        {
            Debug.Log(request.error + "There was a error");
        }
        else
        {
            var newsarticle = JsonConvert.DeserializeObject<NewsResponse>(request.downloadHandler.text);
            foreach (Article art in newsarticle.articles)
            {
                
                if (art.description ==null)
                {
                    Debug.Log("News items finished..");
                }
                else
                {
                    
                    newsource.text = System.Convert.ToString(art.title).Trim();
                    displaynewsitem.text = System.Convert.ToString(art.description).Trim();
                    var Image = art.urlToImage;
                    var publishedat = art.publishedAt;
                    var content = art.content;
                    var source = art.source.name;
                    readmoreURL = art.url;
                   
                    newsitems = Instantiate(newsprefab);
                    newsitems.transform.SetParent(ParentPanel, false);

                    newsitems.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().text = newsource.text;
                    newsitems.transform.Find("Description").GetComponentInChildren<TextMeshProUGUI>().text = displaynewsitem.text;
                    newsitems.transform.Find("Source").GetComponentInChildren<TextMeshProUGUI>().text = source.ToString(); 
                    newsitems.transform.Find("PublishedAt").GetComponentInChildren<TextMeshProUGUI>().text = publishedat.ToString();
                    //newsitems.transform.Find("Content").GetComponentInChildren<TextMeshProUGUI>().text = content.ToString();
                    newsitems.transform.Find("ReadmoreBtn/UrlLink").GetComponentInChildren<TextMeshProUGUI>().text = readmoreURL.ToString();
                    
                    if (art.urlToImage == null)
                    {
                        StartCoroutine(GetTexturefornoimage(newsitems, Image));
                        Debug.Log("There's no image..");
                    }
                    else
                    {
                        StartCoroutine(GetTexture(newsitems, Image));
                    }
                }

            }
            Debug.Log(newsarticle.ToString());
            }

    }

    IEnumerator GetTexture(GameObject item, string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        try
        {
            myTexture = DownloadHandlerTexture.GetContent(www);
           
            if (myTexture != null)
            {
                Rect rec = new Rect(0, 0, myTexture.width/3, myTexture.height/2);
                (item.transform.Find("NewsImage").GetComponent<Image>()).sprite = Sprite.Create(myTexture, rec, new Vector2(0, 0), .01f);
            }
            
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        
    }
    IEnumerator GetTexturefornoimage (GameObject item, string url)
    {
        yield return new WaitForSeconds(0f);
        try
        {
           
            myTexture = dummytexture;
            
            if (myTexture != null)
            {
                Rect rec = new Rect(0, 0, (myTexture.width), myTexture.height);
                (item.transform.Find("NewsImage").GetComponent<Image>()).sprite = Sprite.Create(myTexture, rec, new Vector2(0, 0), .01f);
            }
            
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }

}
