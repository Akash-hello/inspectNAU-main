using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicBtnController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject MicBtnForSummary;

public void MicActivation()
    {
#if UNITY_ANDROID
MicBtnForSummary.SetActive(false);
#else
        MicBtnForSummary.SetActive(true);
#endif
    }
}
