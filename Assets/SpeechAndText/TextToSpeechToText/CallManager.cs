using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using System.Runtime.InteropServices;


public class CallManager : MonoBehaviour
{
    public bool ongoingphonecall;

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RegisterAudioInterruptionNotifications();

#endif

    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        // Register to listen for iOS audio session interruptions
        RegisterAudioInterruptionNotifications();
#endif

    }
    //to test this in the Unity Editor, you can simulate the calls manually:
    //void Update()
    //{
    //    // Simulate phone call received with the "P" key
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        OnInterruptionBegan("Simulated Call Start");
    //    }

    //    // Simulate phone call ended with the "R" key
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        OnInterruptionEnded("Simulated Call End");
    //    }
    //}

    public void OnInterruptionBegan(string message)
    {
        ongoingphonecall = true;
    }

    // This method will be called when an audio interruption ends
    public void OnInterruptionEnded(string message)
    {
        ongoingphonecall = false;
    }
}
