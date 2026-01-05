using System;
using UnityEngine;

public class AttachDynamicLayoutHandler
{
    public static string Execute()
    {
        try
        {
            // Find all question prefab instances in the scene
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            int attachedCount = 0;
            
            foreach (GameObject obj in allObjects)
            {
                // Look for the Questions_(List_Item) objects
                if (obj.name == "Questions_(List_Item)")
                {
                    // Check if it already has the component
                    DynamicLayoutHeightHandler handler = obj.GetComponent<DynamicLayoutHeightHandler>();
                    if (handler == null)
                    {
                        // Add the component
                        obj.AddComponent<DynamicLayoutHeightHandler>();
                        attachedCount++;
                    }
                }
            }
            
            return $"Successfully attached DynamicLayoutHeightHandler to {attachedCount} question prefab instances";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}\n{ex.StackTrace}";
        }
    }
}
