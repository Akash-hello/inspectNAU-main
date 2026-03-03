using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIClickDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                foreach (RaycastResult result in results)
                {
                    RectTransform rectTransform = result.gameObject.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        Debug.Log($"Clicked UI Object: {result.gameObject.name}, Rect: {rectTransform.rect}");
                    }
                }
            }
            else
            {
                Debug.Log("No UI element clicked.");
            }
        }
    }
}
