using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraPhoto : MonoBehaviour
{
    public RawImage photoImage;
    //public GameObject CameraAccrodion;
    public GameObject CameraPhotoObject1;
    public GameObject canvasposition;
    //public RectTransform ParentPanel;
    //public GameObject parentpanel;


    //private void Start()
    //{
    //    //parentpanel = GameObject.FindGameObjectWithTag("devicecampnl");
    //    //ParentPanel = parentpanel.GetComponent<RectTransform>();
    //    CameraPhotoObject = GameObject.FindGameObjectWithTag("cameraphoto");
    //}

    public void ActivateCamera()
    {
        TakePicture(1024);
    }

    public void TakePicture(int maxSize = 1024)
    {
        NativeCamera.TakePicture((path) =>
        {
            if (path != null)
            {
                // 1) Either send the image file at path to another PC directly (image size might be a few MBs depending on camera quality)

                // 2) Or, create a Texture2D (scaled down if needed to satisfy maxSize pixels in each dimension) from the image file and send it to another PC
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                photoImage.texture = texture;
                photoImage.gameObject.SetActive(true);
            }
        }, maxSize);
    }

    public void activatecamerapanel()
    {
        canvasposition = GameObject.FindGameObjectWithTag("canvaspositions");
        canvasposition.GetComponent<CanvasPositionsMgr>().ActivateLiveCam();

        if (canvasposition != null && CameraPhotoObject1 != null)
        {
            // Set the parent of the childObject to parentObject
            CameraPhotoObject1.transform.SetParent(canvasposition.GetComponent<CanvasPositionsMgr>().LivecamCanvas.transform);

            // Optionally reset the child's local position, rotation, and scale
            CameraPhotoObject1.transform.localPosition = Vector3.zero;
            CameraPhotoObject1.transform.localRotation = Quaternion.identity;
            CameraPhotoObject1.transform.localScale = Vector3.one;

            Debug.Log("Child Object is now a child of Parent Object.");
        }

        CameraPhotoObject1.SetActive(true);
        CameraPhotoObject1.GetComponent<capturephoto>().Startcamera();
    }

    //IEnumerator activatecamerapanel()
    //{
    //    yield return null;
    //    for (int i = 0; i < ParentPanel.transform.childCount; ++i)
    //    {
    //        Destroy(ParentPanel.transform.GetChild(i).gameObject);
    //    }

    //    CameraAccrodion = Instantiate(CameraPhotoObject);
    //    CameraPhoto Camera = CameraAccrodion.GetComponent<CameraPhoto>();
    //    CameraAccrodion.transform.SetParent(ParentPanel, false);

    //    //CameraAccrodion.transform.GetComponent<RawImage>().texture = thumbnail;
    //    //CameraAccrodion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "MediaFiles" + "/";
    //    //CameraAccrodion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file);
    //    //CameraAccrodion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file).Split("_Media_")[1];
    //    //CameraAccrodion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
    //    //CameraAccrodion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();

    //}

}
