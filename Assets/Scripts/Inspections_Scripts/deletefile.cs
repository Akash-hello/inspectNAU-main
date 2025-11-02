using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using DataBank;

public class deletefile : MonoBehaviour
{
    public TextMeshProUGUI Filepath;
    public TextMeshProUGUI Filename;
    public TextMeshProUGUI ObsDBid;
    public TextMeshProUGUI InspPrimaryId;

    public string filepath;
    public string deleteattachment;

    public void deletefileonpath()
    {
        filepath = Application.persistentDataPath + Filepath.GetComponent<TextMeshProUGUI>().text+ Filename.GetComponent<TextMeshProUGUI>().text.ToString();
        File.Delete(filepath);
        Destroy(transform.parent.gameObject);
        deleteattachment = "cast(Inspection_PrimaryDetails_ID as int) = '"
            + int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString())
            + "' and cast(Inspection_Observations_ID as int) = '"
            + int.Parse(ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString()) 
            + "' and trim(Attachment_Name) = '" 
            + Filename.GetComponent<TextMeshProUGUI>().text.ToString() 
            + "' and trim(Attachment_path) = '" 
            + Filepath.GetComponent<TextMeshProUGUI>().text.ToString() 
            + "'";

        table_Inspection_Attachments mlocationdb = new table_Inspection_Attachments();
        using var connection = mlocationdb.getConnection();
        mlocationdb.deleteDataByquery(deleteattachment);
        //mlocationdb.close();
    }

}
