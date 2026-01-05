using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script handles dynamic height adjustment for question prefabs when observation panels are toggled.
/// It monitors visibility changes and triggers layout rebuilds to ensure proper spacing.
/// </summary>
public class DynamicLayoutHeightHandler : MonoBehaviour
{
    private RectTransform rectTransform;
    private LayoutElement layoutElement;
    private ContentSizeFitter contentSizeFitter;
    private VerticalLayoutGroup verticalLayoutGroup;
    private CanvasGroup canvasGroup;
    
    // References to observation panels that can be toggled
    private GameObject observationContentHolder;
    private GameObject sire20ObservationContainer;
    
    private bool lastObservationState = false;
    private bool lastSire20State = false;

    void Start()
    {
        // Cache components
        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        
        // Find observation panels
        Transform obsHolder = transform.Find("Observation_Content_Holder");
        if (obsHolder != null)
        {
            observationContentHolder = obsHolder.gameObject;
            lastObservationState = observationContentHolder.activeSelf;
        }
        
        Transform sire20Holder = transform.Find("Sire20Observation_Content_Holder");
        if (sire20Holder != null)
        {
            sire20ObservationContainer = sire20Holder.gameObject;
            lastSire20State = sire20ObservationContainer.activeSelf;
        }
    }

    void Update()
    {
        // Check if observation panels have changed visibility
        bool observationStateChanged = false;
        
        if (observationContentHolder != null)
        {
            if (observationContentHolder.activeSelf != lastObservationState)
            {
                lastObservationState = observationContentHolder.activeSelf;
                observationStateChanged = true;
            }
        }
        
        if (sire20ObservationContainer != null)
        {
            if (sire20ObservationContainer.activeSelf != lastSire20State)
            {
                lastSire20State = sire20ObservationContainer.activeSelf;
                observationStateChanged = true;
            }
        }
        
        // If visibility changed, rebuild layout
        if (observationStateChanged)
        {
            RebuildLayout();
        }
    }

    /// <summary>
    /// Rebuilds the layout hierarchy to recalculate heights based on current content
    /// </summary>
    public void RebuildLayout()
    {
        if (rectTransform == null)
            return;
        
        // Mark for rebuild
        LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        
        // Also rebuild parent if it exists
        RectTransform parentRect = rectTransform.parent as RectTransform;
        if (parentRect != null)
        {
            LayoutRebuilder.MarkLayoutForRebuild(parentRect);
        }
    }

    /// <summary>
    /// Force immediate layout recalculation
    /// </summary>
    public void ForceLayoutUpdate()
    {
        StartCoroutine(ForceLayoutUpdateCoroutine());
    }

    private IEnumerator ForceLayoutUpdateCoroutine()
    {
        // Wait for end of frame to ensure all changes are applied
        yield return new WaitForEndOfFrame();
        
        if (rectTransform != null)
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}
