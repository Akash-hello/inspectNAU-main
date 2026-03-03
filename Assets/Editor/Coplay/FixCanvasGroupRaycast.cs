using UnityEngine;
using UnityEditor;

public class FixCanvasGroupRaycast
{
    public static string Execute()
    {
        string result = "";
        
        // Find Inspection_Information_Canvas_5
        GameObject canvas5 = GameObject.Find("Canvas/Inspection_Information_Canvas_5");
        if (canvas5 == null)
        {
            return "Inspection_Information_Canvas_5 not found";
        }
        
        CanvasGroup canvasGroup = canvas5.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            return "CanvasGroup not found on Inspection_Information_Canvas_5";
        }
        
        // Check current state
        result += $"Before fix:\n";
        result += $"  blocksRaycasts: {canvasGroup.blocksRaycasts}\n";
        result += $"  interactable: {canvasGroup.interactable}\n";
        result += $"  alpha: {canvasGroup.alpha}\n";
        
        // Fix: Enable blocksRaycasts
        canvasGroup.blocksRaycasts = true;
        EditorUtility.SetDirty(canvasGroup);
        
        result += $"\nAfter fix:\n";
        result += $"  blocksRaycasts: {canvasGroup.blocksRaycasts}\n";
        result += $"  interactable: {canvasGroup.interactable}\n";
        result += $"  alpha: {canvasGroup.alpha}\n";
        
        result += "\nButton should now be clickable! Save the scene to persist changes.";
        
        return result;
    }
}
