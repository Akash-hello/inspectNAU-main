using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Gaze : MonoBehaviour
{
    List<InfoBehaviour> infos = new List<InfoBehaviour>();
    public NewsRequest newsrequest;
    public GameObject GoHasinfo;
    public bool shownews;
   

    void Start()
    {
        shownews = false;
        infos = FindObjectsOfType<InfoBehaviour>().ToList();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            GoHasinfo = hit.collider.gameObject;
            if (GoHasinfo.CompareTag("hasinfo"))
            {
                OpenInfo(GoHasinfo.GetComponent<InfoBehaviour>());
                
                newsrequest.ParentPanel = GoHasinfo.transform.Find("SectionInfo/Canvas/Panel/basepanel/Containerpanel").GetComponent<RectTransform>();
                newsrequest.countrycode = GoHasinfo.transform.Find("CountryCode").GetComponentInChildren<Text>().text.ToString();
                if (shownews == false)
                {
                    newsrequest.Getnewsdata();
                    shownews = true;
                }
            }
            else
            {
                CloseAll();
                
            }
        }
        else
        {
            shownews = false;
        }
    }

    void OpenInfo(InfoBehaviour desiredinfo)
    {
        foreach (InfoBehaviour info in infos)
        {
            if (info == desiredinfo)
            {
                
                //    newsrequest.Getnewsdata();
                
                info.OpenInfo();
            }
            else
            {
                info.CloseInfo();
               
            }
        }
    }

   public void CloseAll()
    {
     foreach (InfoBehaviour info in infos)
        {
            info.CloseInfo();
        }
    }

}
