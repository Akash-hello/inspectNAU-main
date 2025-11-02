using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class questionsprefabPencil : MonoBehaviour
{

    public TMP_InputField[] inputFields;

    public void ToggleMobileInput()
    {
#if UNITY_IOS || UNITY_ANDROID

       //  Toggle the shouldHideMobileInput on mobile devices only

        foreach (TMP_InputField input in inputFields)
        {
            if (input.shouldHideMobileInput && input.gameObject.activeInHierarchy)
            {
                input.shouldHideMobileInput = false;
                //Debug.Log("MADE IT FALSE"); Means you can use the pencil....
            }

            else 
            {
                input.shouldHideMobileInput = true;
                //Debug.Log("MADE IT TRUE");
            }
        }

#else
        Debug.Log("This feature is only available on mobile platforms.");
#endif
    }
}
