using UnityEngine.UI;
using System.IO;
using UnityEngine;
using System;
using System.Collections;
using System.Globalization;
using TMPro;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Object = UnityEngine.Object;

public class capturephoto : MonoBehaviour
{
    public RawImage rawImage; // UI element to display the camera feed
    public AspectRatioFitter aspectRatioFitter; // Aspect ratio fitter to maintain aspect ratio
    public Button captureButton; // Button to capture the photo
    public RawImage savephoto;

    public WebCamTexture webCamTexture;
    public TextMeshProUGUI attachmentfilename;
    //public TextMeshProUGUI savingpath;

    public UploadAndSave uploadfilescript;
    public TMP_InputField Photoname;

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time;

    public GameObject Sliderpanel;
    public Slider Photocondition;
    public TextMeshProUGUI conditionvalue;

    public GameObject CameraPhotoObject1;
    public GameObject canvasposition;

    public bool flashonoff;
    public Button Flash;
    public Image strikeoff;
    
    public TextMeshProUGUI messages;
    public float flashDuration = 0.5f; // Duration of each flash
    public float doubleFlashDelay = 0.2f; // Delay between double flashes

    public GameObject FlashBtnIOS;
    public GameObject FlashBtnAndroid;

    public MyAwesomeFlashlightAndroid AndroidFlash;

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _SetTouchLevel(float level);
    [DllImport("__Internal")]
    private static extern void _TurnOn();
    [DllImport("__Internal")]
    private static extern void _TurnOff();
#endif
    

    private void Start()
    {
        FlashBtnIOS.SetActive(false);
        FlashBtnAndroid.SetActive(true);
#if UNITY_IOS
      FlashBtnIOS.SetActive(true);
      FlashBtnAndroid.SetActive(false);

#elif UNITY_ANDROID
        FlashBtnIOS.SetActive(false);
        FlashBtnAndroid.SetActive(false); //Made FALSE as Android Flash Function was not yet working... 13th Dec 2024...
        //AndroidFlash = new MyAwesomeFlashlightAndroid();
#endif

        strikeoff.gameObject.SetActive(true);
        //Flash.GetComponent<Button>().image.color = Color.grey;
        flashonoff = false;

    }

#if UNITY_IOS || UNITY_WEBGL
    private bool CheckPermissionAndRaiseCallbackIfGranted(UserAuthorization authenticationType, Action authenticationGrantedAction)
    {
        if (Application.HasUserAuthorization(authenticationType))
        {
            if (authenticationGrantedAction != null)
                authenticationGrantedAction();

            return true;
        }
        return false;
    }

    private IEnumerator AskForPermissionIfRequired(UserAuthorization authenticationType, Action authenticationGrantedAction)
    {
        if (!CheckPermissionAndRaiseCallbackIfGranted(authenticationType, authenticationGrantedAction))
        {
            yield return Application.RequestUserAuthorization(authenticationType);
            if (!CheckPermissionAndRaiseCallbackIfGranted(authenticationType, authenticationGrantedAction))
                Debug.LogWarning($"Permission {authenticationType} Denied");
        }
    }

#endif

    

    public void Startcamera()
    {
        uploadfilescript.livephoto = false;
        //savephoto.gameObject.SetActive(false);

#if UNITY_IOS || UNITY_WEBGL
        StartCoroutine(AskForPermissionIfRequired(UserAuthorization.WebCam, () => { InitializeCamera(); }));
        return;

#endif
        // Start the device camera

        InitializeCamera();

    }

    private void InitializeCamera()
    {
        //Canvasforlivecam = GameObject.FindGameObjectWithTag("livecamcanvas");

        //Canvasforlivecam.SetActive(true);
        webCamTexture = new WebCamTexture();
        rawImage.texture = webCamTexture;
        aspectRatioFitter.aspectRatio = (float)webCamTexture.width / (float)webCamTexture.height;


#if UNITY_EDITOR
        rawImage.rectTransform.localScale = new Vector3(1, 1, 1);
#elif UNITY_IPHONE
rawImage.rectTransform.localScale = new Vector3(-1, 1, 1);
#elif UNITY_ANDROID
rawImage.rectTransform.localScale = new Vector3(-1, -1, 1);
      
#endif

        webCamTexture.Play();

        Sliderpanel.SetActive(false);
        Photocondition.value = 0;
        conditionvalue.text = "";

        time = 4.0f;

    }

    private Texture2D StampTimestamp(Texture2D source, string stampText)
    {
        // Always add "Live" as a suffix
        stampText = stampText + " (Live)";

        // Create a render target matching the image size
        var rt = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        // Temp camera
        var camGO = new GameObject("StampCam");
        var cam = camGO.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0);  // fully transparent
        cam.orthographic = true;
        cam.targetTexture = rt;
        cam.cullingMask = 1 << 30; // isolate on custom layer if desired

        // Temp Canvas (Screen Space - Camera)
        var canvasGO = new GameObject("StampCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
        canvas.planeDistance = 1f;
        canvasGO.layer = 30;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(source.width, source.height);

        canvasGO.AddComponent<GraphicRaycaster>();

        // Background RawImage = the photo
        var rawGO = new GameObject("Photo");
        rawGO.transform.SetParent(canvasGO.transform, false);
        rawGO.layer = 30;

        var raw = rawGO.AddComponent<RawImage>();
        raw.texture = source;

        var rawRect = raw.GetComponent<RectTransform>();
        rawRect.anchorMin = Vector2.zero;
        rawRect.anchorMax = Vector2.one;
        rawRect.offsetMin = Vector2.zero;
        rawRect.offsetMax = Vector2.zero;

        // Text object directly on canvas (no shaded panel anymore)
        var textGO = new GameObject("StampText");
        textGO.transform.SetParent(canvasGO.transform, false);
        textGO.layer = 30;

        var text = textGO.AddComponent<Text>();
        text.text = stampText;
        text.alignment = TextAnchor.LowerRight; // bottom-right inside canvas
        text.color = Color.white;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 10;
        text.resizeTextMaxSize = 24;
        text.raycastTarget = false;

        // Use Unity’s safe built-in font
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var textRect = text.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(1, 0);
        textRect.anchorMax = new Vector2(1, 0);
        textRect.pivot = new Vector2(1, 0);
        textRect.anchoredPosition = new Vector2(-20f, 10f); // 20px from right, 10px above bottom
        textRect.sizeDelta = new Vector2(600f, 60f);

        // Render off-screen
        var prevRT = RenderTexture.active;
        RenderTexture.active = rt;
        cam.Render();

        // Read back into a new Texture2D
        var stamped = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        stamped.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        stamped.Apply();

        // Cleanup
        RenderTexture.active = prevRT;
        cam.targetTexture = null;
        rt.Release();

        UnityEngine.Object.Destroy(rt);
        UnityEngine.Object.Destroy(camGO);
        UnityEngine.Object.Destroy(canvasGO);

        return stamped;
    }


    public void CaptureAndSavePhoto()
    {

#if UNITY_ANDROID  

        if (AndroidFlash.flashonoff)
        {
            AndroidFlash.ToggleLight();
        }

        else
        {
            AfterAndroidFlash();
        }
#elif UNITY_IPHONE || UNITY_EDITOR
        {
            uploadfilescript.Media = true;

            // Capture the photo from the webcam
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            // Rotate the photo by 90 degrees
            Texture2D rotatedPhoto = RotateTexture(photo, 90);

            // ⬇️ Burn an English timestamp into the image
            rotatedPhoto = StampTimestamp(rotatedPhoto, System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture));


            // Encode the rotated texture to PNG
            byte[] bytes = rotatedPhoto.EncodeToPNG();

            uploadfilescript.filename = "TempName" + ".png";
            uploadfilescript.FileExt = ".png";
            uploadfilescript.livephoto = true;

            Sliderpanel.SetActive(true);
            Photocondition.value = 0;
            conditionvalue.text = "";

            // Display the rotated photo
            DisplayCapturedPhoto(rotatedPhoto);
            CameraPhotoObject1.SetActive(false);

            uploadfilescript.livebtn.SetActive(false);
            uploadfilescript.library.SetActive(false);
            uploadfilescript.Filesbtn.SetActive(false);
            uploadfilescript.filenameforsave.gameObject.SetActive(true);

            // Clean up
            Closewebcam();
        }
#endif
    }

    public void AfterAndroidFlash()
    {
        uploadfilescript.Media = true;

        // Capture the photo from the webcam
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        // Rotate the photo by 90 degrees
        Texture2D rotatedPhoto = RotateTexture(photo, 90);

        // Encode the rotated texture to PNG
        byte[] bytes = rotatedPhoto.EncodeToPNG();

        uploadfilescript.filename = "TempName" + ".png";
        uploadfilescript.FileExt = ".png";
        uploadfilescript.livephoto = true;

        Sliderpanel.SetActive(true);
        Photocondition.value = 0;
        conditionvalue.text = "";

        // Display the rotated photo
        DisplayCapturedPhoto(rotatedPhoto);
        CameraPhotoObject1.SetActive(false);

        uploadfilescript.livebtn.SetActive(false);
        uploadfilescript.library.SetActive(false);
        uploadfilescript.Filesbtn.SetActive(false);
        uploadfilescript.filenameforsave.gameObject.SetActive(true);

        // Clean up
       Closewebcam();
    }


    public void Closewebcam()
    {
        canvasposition = GameObject.FindGameObjectWithTag("canvaspositions");
        // Clean up the camera texture when the script is destroyed
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
            webCamTexture = null;
            rawImage.texture = null;
            Debug.Log("Webcam closed.");
        }
        //Photoname.gameObject.SetActive(false);

        canvasposition.GetComponent<CanvasPositionsMgr>().DeActivateLiveCam();
        //savephoto.gameObject.SetActive(false);

#if UNITY_IPHONE
        _TurnOff(); //IOS
#endif
        strikeoff.gameObject.SetActive(true);
    }

    // Helper function to rotate a texture by 90 degrees
    private Texture2D RotateTexture(Texture2D originalTexture, float angle)
    {
        int width = originalTexture.width;
        int height = originalTexture.height;
        Texture2D rotatedTexture = new Texture2D(height, width); // Notice the height and width swap

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                rotatedTexture.SetPixel(j, width - i - 1, originalTexture.GetPixel(i, j));
            }
        }

        rotatedTexture.Apply();
        return rotatedTexture;
    }

    void DisplayCapturedPhoto(Texture2D photo)
    {
        //savephoto.gameObject.SetActive(true);
        // Create a Sprite from the captured photo and display it in the RawImage
        Sprite photoSprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));
        savephoto.texture = photo;
        //rawImage.texture = photo;
        aspectRatioFitter.aspectRatio = (float)photo.width / (float)photo.height;
    }

    public void FlashToggle()
    {

#if UNITY_IPHONE
        //toggles jump to it's opposite value
        
        flashonoff = !flashonoff;
        if (flashonoff)
        {
            //Flash.GetComponent<Button>().image.color = Color.white;
            strikeoff.gameObject.SetActive(false);
            Debug.Log("Flash on Mode");

            _TurnOn();
        }

        else
        {
            strikeoff.gameObject.SetActive(true);
            //Flash.GetComponent<Button>().image.color = Color.grey;
            Debug.Log("Flash Off");

            _TurnOff();
        }
#endif

    }


    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);
        time = 4.0f;
        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }

    
}