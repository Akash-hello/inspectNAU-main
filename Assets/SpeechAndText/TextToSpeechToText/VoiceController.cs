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

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    [SerializeField]
    TextMeshProUGUI uiText;
    [SerializeField]
    TMP_InputField observationdetail;
    [SerializeField]
    TMP_InputField[] observationinputfields;
    //[SerializeField]
    //TextMeshProUGUI [] uiTexts;
    GameObject speechcubegameobject;
    public TMP_InputField lastClickedField = null; // Store the last clicked field
    private bool isListening = false; // Track if SpeechToText is currently running

    void OnEnable()
    {
        speechcubegameobject = GameObject.FindGameObjectWithTag("speechcube");
        lastClickedField = null;

        Setup(LANG_CODE);
#if UNITY_ANDROID
        // commented by kartik
        //SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif

        //#if UNITY_IOS && !UNITY_EDITOR
        //        // Register to listen for iOS audio session interruptions
        //        RegisterAudioInterruptionNotifications();
        //#endif

        // Create a string array to store the names of the GameObjects
        uiText.text = "";
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;
        //CheckPermission(); CHANGED ON 12th MAR FOR TESTING Speech to Text on .AAB, since working on .APK, moved into Start function just for this...

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
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


    void CheckPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif

    }

    // This method will be called when an audio interruption begins THIS WAS MOVED TO WORK FROM CALLMANAGER SCRIPT
    //public void OnInterruptionBegan(string message)
    //{
    //    Debug.Log("This came from OBJECTIVE C CODE "+message);
    //    StopListening();
    //    speechcubegameobject.gameObject.GetComponent<BoxCollider>().enabled = false;

    //}

    //// This method will be called when an audio interruption ends
    //public void OnInterruptionEnded(string message)
    //{
    //    Debug.Log("This came from OBJECTIVE C CODE " + message);
    //    speechcubegameobject.gameObject.GetComponent<BoxCollider>().enabled = true;

    //}

   
    #region Text to Speech

    public void StartSpeaking(string message)
    {
       
            Debug.Log("Talking started....1");
            TextToSpeech.instance.StartSpeak(message);
       
    }

    public void StopSpeaking()
    {
        Debug.Log("Talking started...2.");
        TextToSpeech.instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking started....");
    }

    void OnSpeakStop()
    {
        Debug.Log("Talking stopped....");
    }

    #endregion

    #region Speech To Text


    public void OnInputFieldClicked(TMP_InputField clickedField)
    {
        // Update the last clicked field
        lastClickedField = clickedField;
    }

    public void StartListening()
    {
        if (!speechcubegameobject.GetComponent<CallManager>().ongoingphonecall)
        {
            if (lastClickedField != null && lastClickedField.gameObject.activeInHierarchy)
            {
                if (isListening)
                {
                    StopListening();
                }

                // Set the observation detail to the last clicked input field
                //observationdetail = lastClickedField;

                // Highlight the active field
                lastClickedField.GetComponent<Image>().color = Color.yellow;

                Debug.Log("Listening started...." + lastClickedField.name);
                SpeechToText.instance.StartRecording();
                isListening = true;
            }
            else
            {
                Debug.LogWarning("No valid input field selected or active.");
            }
        }
        else
        {
            Debug.Log("Some Call in Progress");
            StopListening();
        }
    }

    public void StopListening()
    {
        Debug.Log("Listening End....");
        if (isListening)
        {
            // Check if there is a valid last clicked field
            if (lastClickedField != null && lastClickedField.gameObject.activeInHierarchy)
        {
            // Reset the color of the last clicked field
            lastClickedField.GetComponent<Image>().color = Color.white;

            // Log the field being reset
            Debug.Log("Stopped listening on field: " + lastClickedField.name);
        }
        else
        {
            Debug.LogWarning("No active field to stop listening from.");
        }
            isListening = false;
        }
        // Stop the speech-to-text recording
        SpeechToText.instance.StopRecording();
    }


    

    void OnFinalSpeechResult(string result)
    {
        Debug.Log("resultt " + result);
        uiText.text = result;

        if (string.IsNullOrEmpty(lastClickedField.text) && uiText.text.ToString().Trim() != "nil")
        {
            lastClickedField.text = uiText.text.ToString();
        }
        else if (uiText.text.ToString().Trim() != "nil")
        {
            lastClickedField.text = lastClickedField.text + " " + uiText.text.ToString();
        }

    }

    void OnPartialSpeechResult(string result)
    {
        Debug.Log("resultt " + result);
        uiText.text = result;
        lastClickedField.text = uiText.text.ToString();
    }

#endregion

    void Setup(string code)
    {
        TextToSpeech.instance.Setting(code, 1, 1);
        SpeechToText.instance.Setting(code);
    }
}


//OLD CODE BLOCKS, TO BE DELETED AFTER TESTING DONE...

//    void Start()
//    {
//        speechcubegameobject = GameObject.FindGameObjectWithTag("speechcube");
//        lastClickedField = null;

//        Setup(LANG_CODE);
//#if UNITY_ANDROID
//        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
//#endif

////#if UNITY_IOS && !UNITY_EDITOR
////        // Register to listen for iOS audio session interruptions
////        RegisterAudioInterruptionNotifications();
////#endif

//        // Create a string array to store the names of the GameObjects
//        uiText.text = "";
//        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
//        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
//        TextToSpeech.instance.onDoneCallback = OnSpeakStop;
//        CheckPermission();
//    }

//public void StartListeningOLD()
//{
//    if (!speechcubegameobject.GetComponent<CallManager>().ongoingphonecall == true)
//    {

//        Debug.Log("Listening started...." + observationinputfields[0].name.ToString());
//        observationdetail = observationinputfields[0];

//        observationinputfields[0].GetComponent<Image>().color = Color.yellow;
//        SpeechToText.instance.StartRecording();
//    }

//    else
//    {
//        Debug.Log("Some Call in Progress");
//        StopListening();
//    }
//}

//public void StopListeningOLD()
//{
//    Debug.Log("Listening End....");
//    observationinputfields[0].GetComponent<Image>().color = Color.white;
//    SpeechToText.instance.StopRecording();
//}

//public void StartListeningHuman()
//{
//    Debug.Log("Listening started...." + observationinputfields[1].name.ToString());
//    observationdetail = observationinputfields[1];
//    observationinputfields[1].GetComponent<Image>().color = Color.yellow;
//    SpeechToText.instance.StartRecording();
//}

//public void StopListeningHuman()
//{
//    Debug.Log("Listening End....");
//    observationinputfields[1].GetComponent<Image>().color = Color.white;
//    SpeechToText.instance.StopRecording();
//}

//public void StartListeningProcess()
//{
//    Debug.Log("Listening started...." + observationinputfields[2].name.ToString());
//    observationdetail = observationinputfields[2];
//    observationinputfields[2].GetComponent<Image>().color = Color.yellow;
//    SpeechToText.instance.StartRecording();
//}

//public void StopListeningProcess()
//{
//    Debug.Log("Listening End....");
//    observationinputfields[2].GetComponent<Image>().color = Color.white;
//    SpeechToText.instance.StopRecording();
//}


//public void StartListeningHardware()
//{
//    Debug.Log("Listening started...." + observationinputfields[3].name.ToString());
//    observationdetail = observationinputfields[3];
//    observationinputfields[3].GetComponent<Image>().color = Color.yellow;
//    SpeechToText.instance.StartRecording();
//}

//public void StopListeningHardware()
//{
//    Debug.Log("Listening End....");
//    observationinputfields[3].GetComponent<Image>().color = Color.white;
//    SpeechToText.instance.StopRecording();
//}