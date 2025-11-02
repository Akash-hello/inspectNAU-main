using UnityEngine;

public class AndroidContentOpenerWrapper
{
    public static void OpenContent(string filePath)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                AndroidJavaObject file = new AndroidJavaObject("java.io.File", filePath);
                if (!file.Call<bool>("exists"))
                {
                    Debug.LogError("File not found: " + filePath);
                    return;
                }

                AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
                AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

                string authority = "com.LaunchfortTechnologies.InspectNau.fileprovider";
                AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", context, authority, file);

                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.intent.action.VIEW");
                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/pdf");
                intent.Call<AndroidJavaObject>("addFlags", 1 << 1); // FLAG_GRANT_READ_URI_PERMISSION

                currentActivity.Call("startActivity", intent);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error opening content: " + e.Message);
            }
        }
        else
        {
            Debug.Log("This platform is not Android.");
        }
    }
}
