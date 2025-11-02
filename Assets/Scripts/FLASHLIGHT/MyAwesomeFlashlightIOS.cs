using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;
using TMPro;

public class MyAwesomeFlashlightIOS : MonoBehaviour
{
    public TextMeshProUGUI messages;
    public float flashDuration = 0.5f; // Duration of each flash
    public float doubleFlashDelay = 0.2f; // Delay between double flashes

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _SetTouchLevel(float level);
    [DllImport("__Internal")]
    private static extern void _TurnOn();
    [DllImport("__Internal")]
    private static extern void _TurnOff();
#endif

#if UNITY_IOS

    public void TurnOnFlashlight()
    {
        StartCoroutine(TurnOnFlashlightstart());
    }


    public IEnumerator TurnOnFlashlightstart()
    {
        flashDuration = 0.5f; // Duration of each flash
        doubleFlashDelay = 0.2f; // Delay between double flashes

        _TurnOn();
        yield return new WaitForSeconds(flashDuration);
        _TurnOff();

        StartCoroutine(DoubleFlash());
        messages.text = "Recived command to Turn ON...";
    }

    public IEnumerator DoubleFlash()
    {

        // First flash
        _TurnOn();
        yield return new WaitForSeconds(flashDuration);
        _TurnOff();

        // Small delay before the second flash
        yield return new WaitForSeconds(doubleFlashDelay);

        // Second flash
        _TurnOn();
        yield return new WaitForSeconds(flashDuration);
        _TurnOff();
    }

    public void TurnOffFlashlight()
    {
        _TurnOff();
        messages.text = "Recived command to Turn OFF...";
    }
#endif

    public void TurnOn()
    {
#if UNITY_IOS
        _TurnOn();
#endif
    }

    public void TurnOff()
    {
#if UNITY_IOS
        _TurnOff();
#endif
    }

}
