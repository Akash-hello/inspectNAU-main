using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CaretNavigationHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler
{
    public TMP_InputField inputField;

    // For swipe logic
    private float lastCaretMoveTime = 0f;
    private float moveInterval = 0.05f;
    private float swipeSensitivity = 20f;
    private Vector2 lastDragPos;

    // Detect tap to position caret
    public void OnPointerDown(PointerEventData eventData)
    {
        inputField.ActivateInputField();

        int charIndex = TMP_TextUtilities.GetCursorIndexFromPosition(
            inputField.textComponent,
            eventData.position,
            eventData.pressEventCamera
        );

        inputField.caretPosition = charIndex;
        inputField.selectionAnchorPosition = charIndex;
        inputField.selectionFocusPosition = charIndex;
    }

    // Detect start of swipe
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPos = eventData.position;
        lastCaretMoveTime = Time.time;
    }

    // Swipe caret logic
    public void OnDrag(PointerEventData eventData)
    {
        if (!inputField.isFocused) return;

        float deltaX = eventData.position.x - lastDragPos.x;

        if (Mathf.Abs(deltaX) >= swipeSensitivity && (Time.time - lastCaretMoveTime > moveInterval))
        {
            if (deltaX > 0 && inputField.caretPosition < inputField.text.Length)
                inputField.caretPosition++;
            else if (deltaX < 0 && inputField.caretPosition > 0)
                inputField.caretPosition--;

            lastDragPos = eventData.position;
            lastCaretMoveTime = Time.time;

            inputField.selectionAnchorPosition = inputField.caretPosition;
            inputField.selectionFocusPosition = inputField.caretPosition;
        }
    }
}
