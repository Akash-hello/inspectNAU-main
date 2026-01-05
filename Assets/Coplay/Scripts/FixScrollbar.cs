using UnityEngine;
using UnityEngine.UI;

public class FixScrollbar
{
    public static string Execute()
    {
        string scrollbarPath = "Canvas/Inspections_Block_Canvas_6/Content_Layout/Middle_Panel/Inspections_Block/Scrollbar Vertical";
        GameObject scrollbarGO = GameObject.Find(scrollbarPath);
        
        if (scrollbarGO == null)
            return $"Error: Could not find object at {scrollbarPath}";

        // Fix Scrollbar RectTransform
        RectTransform scrollbarRect = scrollbarGO.GetComponent<RectTransform>();
        // Ensure it has some reasonable width if it's too thin, or fix the sliding area.
        // Current width is 8.5. Let's keep it but fix the children.
        
        // Find Sliding Area
        Transform slidingAreaTrans = scrollbarGO.transform.Find("Sliding Area");
        if (slidingAreaTrans == null)
            return "Error: Sliding Area not found in Scrollbar";
            
        RectTransform slidingAreaRect = slidingAreaTrans.GetComponent<RectTransform>();
        
        // Fix Sliding Area
        // It should stretch to fill the scrollbar (or have small padding)
        slidingAreaRect.anchorMin = Vector2.zero;
        slidingAreaRect.anchorMax = Vector2.one;
        slidingAreaRect.pivot = new Vector2(0.5f, 0.5f);
        slidingAreaRect.sizeDelta = Vector2.zero; // Remove negative padding that caused negative width
        slidingAreaRect.anchoredPosition = Vector2.zero;

        // Find Handle
        Transform handleTrans = slidingAreaTrans.Find("Handle");
        if (handleTrans == null)
            return "Error: Handle not found in Sliding Area";
            
        RectTransform handleRect = handleTrans.GetComponent<RectTransform>();
        
        // Fix Handle
        // Handle size is controlled by Scrollbar script via anchors, so sizeDelta should be zero
        handleRect.sizeDelta = Vector2.zero;
        handleRect.anchoredPosition = Vector2.zero;
        
        // Ensure Scrollbar component is setup correctly
        Scrollbar scrollbar = scrollbarGO.GetComponent<Scrollbar>();
        if (scrollbar != null)
        {
            scrollbar.handleRect = handleRect;
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
        }

        return "Fixed Scrollbar Vertical structure (Sliding Area and Handle RectTransforms).";
    }
}
