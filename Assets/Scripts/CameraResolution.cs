using UnityEngine;
using System.Collections;

public class CameraResolution : MonoBehaviour
{
    public Resolution screenwidth;
    public Resolution screenheight;
    Camera camera;
    float scaleheight = 0f;

    public void Awake()
    {
        camera = GetComponent<Camera>();
       //StartCoroutine(UpdateScreenRes());
    }

    IEnumerator UpdateScreenRes()
    {
        while (true)
        {
            // set the desired aspect ratio (the values in this example are
            // hard-coded for 16:9, but you could make them into public
            // variables instead so you can set them at design time)
            //if ((float)Screen.width > (float)Screen.height || (float)Screen.width >= 1768)
            //{

            float targetaspect = 1080f / 1920.0f;

            // determine the game window's current aspect ratio
            float windowaspect = (float)Screen.width / (float)Screen.height;

            // current viewport height should be scaled by this amount
            float scaleheight = windowaspect / targetaspect;

            // obtain camera component so we can modify its viewport
            Camera camera = GetComponent<Camera>();

            // if scaled height is less than current height, add letterbox
            if (scaleheight < 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f;
                rect.height = 1.0f;
                rect.x = 0;
                rect.y = 0;
                //rect.y = (1.0f - scaleheight) / 2.0f;
                //Debug.Log("First IF" + (float)Screen.width + " -- height " + (float)Screen.height);
                camera.rect = rect;
            }

            //else if ((float)Screen.width < (float)Screen.height)
            //{
            //    float scalewidth = 1.0f / scaleheight;

            //    Rect rect = camera.rect;
            //    Debug.Log("ELSE IF" + (float)Screen.width + " -- height " + (float)Screen.height);
            //    rect.width = 1.0f;
            //    rect.height = 1.0f;
            //    rect.x = 0;
            //    rect.y = 0;
            //    Debug.Log("width" + rect.width + " -- height " + rect.height);
            //    camera.rect = rect;
            //}
            else
            { // add pillarbox
                float scalewidth = 1.0f / scaleheight;

                Rect rect = camera.rect;

                rect.width = scalewidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f;
                rect.y = 0;
                //Debug.Log("width" + rect.width + " -- height " + rect.height);
                camera.rect = rect;
                //Debug.Log("LAST ELSE" + (float)Screen.width + " -- height " + (float)Screen.height);
            }
            yield return null;
        }
    }
   
}
