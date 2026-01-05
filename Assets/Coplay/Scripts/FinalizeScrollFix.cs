using UnityEngine;
using UnityEngine.UI;

public class FinalizeScrollFix
{
    public static string Execute()
    {
        string scrollRectPath = "Canvas/Inspections_Block_Canvas_6/Content_Layout/Middle_Panel/Inspections_Block";
        GameObject scrollRectGO = GameObject.Find(scrollRectPath);
        
        if (scrollRectGO == null)
            return $"Error: Could not find object at {scrollRectPath}";

        // 1. Ensure the ScrollRect's Image is enabled for Raycasting
        var image = scrollRectGO.GetComponent<Image>();
        if (image != null)
        {
            if (!image.enabled)
            {
                image.enabled = true;
                // If it was disabled, it might be because they didn't want to see it.
                // We'll set alpha to 0 (fully transparent) but keep it as a raycast target.
                var color = image.color;
                color.a = 0f;
                image.color = color;
            }
        }
        else
        {
            // If no image, add one for raycasting
            image = scrollRectGO.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
        }

        // 2. Refresh the ScrollRect references just in case
        var scrollRect = scrollRectGO.GetComponent<ScrollRect>();
        if (scrollRect != null)
        {
            // Ensure proper scroll sensitivity
            scrollRect.scrollSensitivity = 20f; // Increase sensitivity a bit
            
            // Ensure Viewport and Content are assigned (already checked, but good to be safe)
            if (scrollRect.viewport == null)
                scrollRect.viewport = scrollRectGO.transform.Find("Viewport")?.GetComponent<RectTransform>();
            if (scrollRect.content == null && scrollRect.viewport != null)
                scrollRect.content = scrollRect.viewport.Find("Content")?.GetComponent<RectTransform>();
        }

        return "Enabled Raycast Target on ScrollRect and adjusted sensitivity.";
    }
}
