using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Frame_VSync : MonoBehaviour
{
    public float Rate = 50.0f;
    float currentFrameTime;

    // Start is called before the first frame update
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
        currentFrameTime = Time.realtimeSinceStartup;

        // Reduce texture quality
        QualitySettings.globalTextureMipmapLimit = 1; // Half resolution

        // Disable Anti-Aliasing
        QualitySettings.antiAliasing = 0;

        // Reduce shadow settings
        QualitySettings.shadowResolution = ShadowResolution.Low;
        QualitySettings.shadowDistance = 20f;

        StartCoroutine("WaitForNextFrame");
    }

    // Update is called once per frame
    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / Rate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }

}