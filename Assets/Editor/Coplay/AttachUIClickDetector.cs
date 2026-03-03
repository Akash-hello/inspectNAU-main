using UnityEngine;
using UnityEditor;

public class AttachUIClickDetector
{
    public static string Execute()
    {
        GameObject calamityController = GameObject.Find("Calamity controller");
        if (calamityController == null)
        {
            return "Error: Could not find 'Calamity controller' GameObject";
        }

        // Check if component already exists
        UIClickDetector existingComponent = calamityController.GetComponent<UIClickDetector>();
        if (existingComponent != null)
        {
            return "UIClickDetector component already exists on 'Calamity controller'";
        }

        // Add the component
        calamityController.AddComponent<UIClickDetector>();
        
        // Mark scene as dirty so changes are saved
        EditorUtility.SetDirty(calamityController);
        
        return "Successfully attached UIClickDetector to 'Calamity controller'";
    }
}
