using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollButtonScript : MonoBehaviour, IPointerDownHandler 
{
    [SerializeField]
    private ScrollRectScript scrollRectscript;
   
    [SerializeField]
    private bool isDownbutton;

    public void OnPointerDown (PointerEventData eventData)
    {
        if (isDownbutton)
        {
            scrollRectscript.ButtonDownisPressed();
            
        }
        else
        {
            scrollRectscript.ButtonUpisPressed();
            
        }
    }
}
