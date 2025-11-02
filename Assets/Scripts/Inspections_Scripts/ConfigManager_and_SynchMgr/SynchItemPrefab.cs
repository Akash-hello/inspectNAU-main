using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DataBank;
using System;
using SQLite4Unity3d;
using System.Threading;

public class SynchItemPrefab : MonoBehaviour
{
    public Image SynchItemImage;
    public TextMeshProUGUI DatabaseID;
    public TextMeshProUGUI MachineID;
    public TextMeshProUGUI Data;
    public TextMeshProUGUI ItemCode;
    public TextMeshProUGUI Instruction;
    public TextMeshProUGUI Quantity;
    public TextMeshProUGUI Processed;
    public Image Status;
    public TextMeshProUGUI Source;
    public TextMeshProUGUI TimeStamp;

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 5.0f;

    public void ExecuteWithRetry(Action action, int maxRetries = 3, int delayMilliseconds = 1000)
    {
        int retries = 0;
        while (true)
        {
            try
            {
                action();
                break; // Exit the loop if the action succeeded
            }
            catch (SQLiteException ex) when (ex.Message.Contains("database is locked"))
            {
                if (++retries == maxRetries)
                    throw; // Rethrow the exception if maximum retries reached
                Thread.Sleep(delayMilliseconds); // Wait before retrying
            }
        }
    }

    public void ChangeStatusForResending()

    {
        ExecuteWithRetry(() =>
        {
            
        table_Synch mLocationDb2 = new table_Synch();
            using var connection1 = mLocationDb2.getConnection();
            string query = "Processed  = 'N' WHERE ID = '" + int.Parse(DatabaseID.text.Replace("#", "")) + "'";
        mLocationDb2.Updatedata(query);
        //mLocationDb2.close();
        });

        errorpopup.gameObject.SetActive(true);
        PopUpMsg.text = "Confirm, file added to queue.";
        StartCoroutine(HidePopUp());
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }
}
