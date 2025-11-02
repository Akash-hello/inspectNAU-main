using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class ClickHandler : MonoBehaviour
{
    public UnityEvent upEvent;
    public UnityEvent downEvent;

    void OnMouseDown()
    {
        Debug.Log("Down");
        downEvent?.Invoke();
    }

    // Update is called once per frame
    void OnMouseUp()
    {
        Debug.Log("Up");
        upEvent?.Invoke();
    }
}
