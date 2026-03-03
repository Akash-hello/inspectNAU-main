using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;

public class CheckButtonOnClick
{
    public static string Execute()
    {
        string result = "";
        
        // Find the button
        GameObject btnUploadPhoto = GameObject.Find("Canvas/Inspection_Information_Canvas_5/InnerPanel_1/Viewport/Content/ShipPhotoEmpty/ShipPhoto/BTN_UploadPhoto");
        if (btnUploadPhoto == null)
        {
            return "BTN_UploadPhoto not found";
        }
        
        Button button = btnUploadPhoto.GetComponent<Button>();
        if (button == null)
        {
            return "Button component not found on BTN_UploadPhoto";
        }
        
        // Check onClick listeners
        int listenerCount = button.onClick.GetPersistentEventCount();
        result += $"Button onClick has {listenerCount} persistent listener(s)\n";
        
        for (int i = 0; i < listenerCount; i++)
        {
            var target = button.onClick.GetPersistentTarget(i);
            var methodName = button.onClick.GetPersistentMethodName(i);
            result += $"  Listener {i}: Target={target?.ToString() ?? "null"}, Method={methodName}\n";
        }
        
        // Check if button is interactable
        result += $"Button interactable: {button.interactable}\n";
        result += $"Button enabled: {button.enabled}\n";
        
        // Check CanvasGroup blocking
        CanvasGroup[] canvasGroups = btnUploadPhoto.GetComponentsInParent<CanvasGroup>(true);
        result += $"\nCanvasGroups in parent hierarchy: {canvasGroups.Length}\n";
        foreach (var cg in canvasGroups)
        {
            result += $"  CanvasGroup on '{cg.gameObject.name}': interactable={cg.interactable}, blocksRaycasts={cg.blocksRaycasts}, alpha={cg.alpha}\n";
        }
        
        // Check if any parent has interactable=false
        bool anyBlockingInteraction = false;
        foreach (var cg in canvasGroups)
        {
            if (!cg.interactable || !cg.blocksRaycasts)
            {
                anyBlockingInteraction = true;
                result += $"\n*** ISSUE FOUND: CanvasGroup on '{cg.gameObject.name}' may be blocking interaction! ***\n";
            }
        }
        
        // Check the ShipPhoto parent for ShipsImage script
        GameObject shipPhoto = GameObject.Find("Canvas/Inspection_Information_Canvas_5/InnerPanel_1/Viewport/Content/ShipPhotoEmpty/ShipPhoto");
        if (shipPhoto != null)
        {
            var shipsImage = shipPhoto.GetComponent<ShipsImage>();
            if (shipsImage != null)
            {
                result += $"\nShipsImage component found on ShipPhoto\n";
            }
        }
        
        return result;
    }
}
