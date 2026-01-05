using UnityEngine;
using UnityEngine.UI;

public class FixScrollContent
{
    public static string Execute()
    {
        string contentPath = "Canvas/Inspections_Block_Canvas_6/Content_Layout/Middle_Panel/Inspections_Block/Viewport/Content";
        GameObject contentGO = GameObject.Find(contentPath);
        
        if (contentGO == null)
            return $"Error: Could not find object at {contentPath}";

        // Fix ContentSizeFitter
        var csf = contentGO.GetComponent<ContentSizeFitter>();
        if (csf == null)
            csf = contentGO.AddComponent<ContentSizeFitter>();
        
        // Set vertical fit to PreferredSize so it calculates height based on children
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        // Fix RectTransform
        var rect = contentGO.GetComponent<RectTransform>();
        
        // Pivot to Top-Center (0.5, 1)
        rect.pivot = new Vector2(0.5f, 1f);
        
        // Anchors to Top-Stretch (0,1) to (1,1)
        // This ensures the content width stretches to viewport width, and stays at the top
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        
        // Reset Position to top (y=0)
        // X position 0 because pivot x is 0.5 and anchors stretch
        rect.anchoredPosition = new Vector2(0f, 0f);
        
        // Reset sizeDelta. 
        // X=0 means width matches anchors (stretch).
        // Y=0 means height matches anchors (0), but ContentSizeFitter will override this.
        rect.sizeDelta = new Vector2(0f, 0f);

        return "Fixed ContentSizeFitter and RectTransform on Content object for scrolling.";
    }
}
