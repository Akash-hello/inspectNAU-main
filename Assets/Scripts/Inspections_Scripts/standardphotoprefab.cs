
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class standardphotoprefab : MonoBehaviour

{
    public TextMeshProUGUI Questions_Text;
    public TextMeshProUGUI InspectionPrimaryID;
    public TextMeshProUGUI AttachmentFolderpath;
    public GameObject fileuploader;
    public GameObject completionstatusindicator;
    public TextMeshProUGUI mediacount;

    private void Start()
    {
        fileuploader.GetComponent<UploadAndSave>().ObsDBid.text = "0";
        
    }
}