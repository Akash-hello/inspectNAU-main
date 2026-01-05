using System;
using UnityEngine;
using UnityEngine.UI;

public class DynamicQuestionHeightFixer
{
    public static string Execute()
    {
        try
        {
            // Find the Questions_(List_Item) GameObject
            GameObject questionsListItem = GameObject.Find("Canvas/Inspections_Block_Canvas_6/Content_Layout/Right_Panel/Questions_Block/Viewport/Content/New_Questions_Container_Prefab (1)/Questions_(List_Item)");
            
            if (questionsListItem == null)
                return "Error: Could not find Questions_(List_Item)";
            
            // Add ContentSizeFitter component if it doesn't exist
            ContentSizeFitter contentSizeFitter = questionsListItem.GetComponent<ContentSizeFitter>();
            if (contentSizeFitter == null)
            {
                contentSizeFitter = questionsListItem.AddComponent<ContentSizeFitter>();
            }
            
            // Configure ContentSizeFitter to use preferred size for height
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            
            // Update LayoutElement to allow flexible height
            LayoutElement layoutElement = questionsListItem.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                layoutElement.flexibleHeight = 1f;
                layoutElement.preferredHeight = -1f;
            }
            
            // Find and update Observation_Content_Holder
            Transform observationHolderTransform = questionsListItem.transform.Find("Observation_Content_Holder");
            if (observationHolderTransform != null)
            {
                GameObject observationHolder = observationHolderTransform.gameObject;
                LayoutElement obsLayoutElement = observationHolder.GetComponent<LayoutElement>();
                
                if (obsLayoutElement != null)
                {
                    obsLayoutElement.minHeight = -1f;
                    obsLayoutElement.flexibleHeight = 1f;
                }
            }
            
            // Force layout rebuild
            LayoutRebuilder.MarkLayoutForRebuild(questionsListItem.GetComponent<RectTransform>());
            
            return "Successfully configured dynamic height adjustment:\n" +
                   "- Added ContentSizeFitter to Questions_(List_Item)\n" +
                   "- Set ContentSizeFitter to use PreferredSize for vertical fit\n" +
                   "- Updated LayoutElement to allow flexible height\n" +
                   "- Configured Observation_Content_Holder for dynamic sizing\n" +
                   "- Forced layout rebuild";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}\n{ex.StackTrace}";
        }
    }
}
