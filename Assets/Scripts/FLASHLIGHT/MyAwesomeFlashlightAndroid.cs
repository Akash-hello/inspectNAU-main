using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

#if UNITY_ANDROID
using UnityEngine.Android;
using System;
#endif

public class MyAwesomeFlashlightAndroid : MonoBehaviour
{
    public TextMeshProUGUI messages;
    public Image strikeoff;
#if UNITY_ANDROID
    private AndroidJavaObject camera;
    private AndroidJavaObject cameraParams;
    private AndroidJavaObject cameraManager;
    private string cameraID;
    private bool isFlashOn = false;
#endif
    public bool flashonoff;
    public capturephoto ClickPhoto;

    void Start()
    {
        //ClickPhoto = new capturephoto();
#if UNITY_ANDROID
        // Check permissions and initialize the flashlight
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            InitializeFlashlight();
        }
        else
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
        strikeoff.gameObject.SetActive(true);
        flashonoff = false;
    }

    private void InitializeFlashlight()
    {
#if UNITY_ANDROID
        try
        {
            // Check if device supports a flashlight
            if (!SystemInfo.supportsGyroscope)
            {
                Debug.LogError("No flashlight feature found on this device.");
                messages.text = "No flashlight feature found on this device.";
                return;
            }
            else
            {
                AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                cameraManager = activity.Call<AndroidJavaObject>("getSystemService", "camera");
                cameraID = cameraManager.Call<string[]>("getCameraIdList")[0];

            }
              
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize flashlight: {e.Message}");
            messages.text = $"Failed to initialize flashlight: {e.Message}";
        }
#endif
    }

    public void TurnOnFlashlight()
    {
        ToggleFlashlight(true);
        messages.text = "Recived command to Turn ON...";
        strikeoff.gameObject.SetActive(false);
    }

    public void TurnOffFlashlight()
    {
        ToggleFlashlight(false);
        messages.text = "Received command to Turn OFF...";
        strikeoff.gameObject.SetActive(true);
    }

    public void TogglestrilkeoffImage()
    {
        flashonoff = !flashonoff;
        if (flashonoff)
        {
            //Flash.GetComponent<Button>().image.color = Color.white;
            strikeoff.gameObject.SetActive(false);
            Debug.Log("Flash on Mode");
            //ToggleFlashlight(true);
            //TurnOnFlashlight();
        }

        else
        {
            strikeoff.gameObject.SetActive(true);
            //Flash.GetComponent<Button>().image.color = Color.grey;
            Debug.Log("Flash Off");
            //ToggleFlashlight(false);
            //TurnOffFlashlight();
        }
    }

    public void ToggleLight()
    {
       // flashonoff = !flashonoff;

        if (flashonoff)
        {
            Debug.Log("Flash On Mode");

            // Stop the camera if using WebCamTexture
            if (ClickPhoto.webCamTexture != null && ClickPhoto.webCamTexture.isPlaying)
                //ClickPhoto.webCamTexture.Stop();

            // Toggle the flashlight
            ToggleFlashlight(true);

            // Resume camera after a delay if needed
            StartCoroutine(ResumeCameraAfterFlash());
        }
        else
        {
            Debug.Log("Flash Off Mode");
            ToggleFlashlight(false);
        }
    }

    public void ToggleFlashlight(bool turnOn)
    {
         // If WebCamTexture is in use, it might not expose the Camera object directly
            if (ClickPhoto.webCamTexture != null && ClickPhoto.webCamTexture.isPlaying)
            {
                AndroidJavaObject camera = new AndroidJavaObject("android.hardware.Camera");
                AndroidJavaObject parameters = camera.Call<AndroidJavaObject>("getParameters");

                if (turnOn)
                {
                    parameters.Call("setFlashMode", "torch");
                }
                else
                {
                    parameters.Call("setFlashMode", "off");
                }

                camera.Call("setParameters", parameters);
                Debug.Log($"Flashlight toggled: {turnOn}");
            }
            else
            {
                Debug.LogError("Camera is not active or available.");
            }
        
    }


//    public void ToggleFlashlight(bool turnOn)
//    {
//#if UNITY_ANDROID
//        try
//        {
//            cameraManager.Call("setTorchMode", cameraID, turnOn);
//            messages.text = "setTorchMode -- "+ turnOn;
            

//            isFlashOn = turnOn;
//        }
//        catch (Exception e)
//        {
//            messages.text = "ERROR RECEIVED -- " + e.Message +"-" + e.StackTrace;
//            Debug.LogError($"Failed to toggle flashlight: {e.Message}  - {e.StackTrace}");
//        }
//#endif
//    }

    private IEnumerator ResumeCameraAfterFlash()
    {
        yield return new WaitForSeconds(1.5f); // Delay to ensure torch mode toggled
        if (ClickPhoto.webCamTexture != null)
            ClickPhoto.webCamTexture.Play();
        strikeoff.gameObject.SetActive(false);
        ClickPhoto.AfterAndroidFlash();
    }

    void OnDestroy()
    {
#if UNITY_ANDROID
        // Release camera resources on exit
        if (camera != null)
        {
            camera.Call("release");
            camera = null;
        }
#endif
    }
}
