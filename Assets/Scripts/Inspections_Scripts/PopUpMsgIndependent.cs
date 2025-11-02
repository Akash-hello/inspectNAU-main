using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpMsgIndependent : MonoBehaviour
{
    public GameObject errorpopup;

    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;

    public void WarningMessage()
    {
        errorpopup.gameObject.SetActive(true);
        PopUpMsg.text = "Publishing your report, please be patient, this may take sometime depending on the number of photos and records.";
        time = 4.0f;
        //StartCoroutine(HidePopUp());
       
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        PopUpMsg.color = Color.black;
    }
}
