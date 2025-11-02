using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReadMoreURL : MonoBehaviour
{
    string url;
    public TextMeshProUGUI UrlLink;
   
    void Start()
    {
        url = UrlLink.text;
           
    }

    public void TaskOnClick()
    {
        Application.OpenURL(url);
    }
}
