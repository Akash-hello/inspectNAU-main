using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FixButtonRaycast
{
    public static string Execute()
    {
        string result = "";
        
        // Find Ships_Photo_Image and disable raycastTarget
        GameObject shipsPhotoImage = GameObject.Find("Canvas/Inspection_Information_Canvas_5/InnerPanel_1/Viewport/Content/ShipPhotoEmpty/ShipPhoto/Ships_Photo_Image");
        if (shipsPhotoImage != null)
        {
            RawImage rawImage = shipsPhotoImage.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.raycastTarget = false;
                EditorUtility.SetDirty(rawImage);
                result += "Disabled raycastTarget on Ships_Photo_Image\n";
            }
        }
        
        // Find Camera and disable raycastTarget
        GameObject cameraObj = GameObject.Find("Canvas/Inspection_Information_Canvas_5/InnerPanel_1/Viewport/Content/ShipPhotoEmpty/ShipPhoto/Camera");
        if (cameraObj != null)
        {
            RawImage rawImage = cameraObj.GetComponent<RawImage>();
            if (rawImage != null)
            {
                rawImage.raycastTarget = false;
                EditorUtility.SetDirty(rawImage);
                result += "Disabled raycastTarget on Camera\n";
            }
        }
        
        // Fix the button's targetGraphic
        GameObject btnUploadPhoto = GameObject.Find("Canvas/Inspection_Information_Canvas_5/InnerPanel_1/Viewport/Content/ShipPhotoEmpty/ShipPhoto/BTN_UploadPhoto");
        if (btnUploadPhoto != null)
        {
            Button button = btnUploadPhoto.GetComponent<Button>();
            Image image = btnUploadPhoto.GetComponent<Image>();
            if (button != null && image != null)
            {
                button.targetGraphic = image;
                EditorUtility.SetDirty(button);
                result += "Set targetGraphic on BTN_UploadPhoto button\n";
            }
        }
        
        if (string.IsNullOrEmpty(result))
        {
            return "No changes made - objects not found";
        }
        
        return result + "\nButton should now be clickable!";
    }
}
