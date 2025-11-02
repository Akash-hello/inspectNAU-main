using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectScript : MonoBehaviour
{
    private ScrollRect scrollrect;
    private bool mouseDown, buttonDown, buttonUp;
    public Vector2 startPosition;
    public float parallaxAmount = 0.05f;
    private float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        deltaTime = 50f;
        scrollrect = GetComponent<ScrollRect>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonDown)
        {
            ScrollDown();
        }
        else if (buttonUp)
        {
            ScrollUp();
        }
    }

    public void ButtonDownisPressed()
    {
        mouseDown = true;
        buttonDown = true;
    }

    public void ButtonUpisPressed()
    {
        mouseDown = true;
        buttonUp = true;
    }

    private void ScrollDown()
    {
        if(Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            buttonDown = false;
        }
        else
        {
            scrollrect.verticalNormalizedPosition -= 100.0f * deltaTime;
        }
    }

    private void ScrollUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            buttonUp = false;
        }
        else
        {
            scrollrect.verticalNormalizedPosition += 0.01f * deltaTime;
        }
    }

}
