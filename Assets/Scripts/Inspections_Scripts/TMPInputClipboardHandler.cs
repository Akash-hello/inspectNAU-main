using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMPInputClipboardHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_InputField tmpInputField;
    private TouchScreenKeyboard keyboard;

    private float longPressDuration = 0.5f; // Time in seconds to register a long press
    private float pressStartTime;
    private bool longPressDetected = false;
    private bool isPressing = false;

    void Update()
    {
        // Check if the user has pressed long enough to trigger a long press
        if (isPressing && !longPressDetected && (Time.time - pressStartTime) > longPressDuration)
        {
            longPressDetected = true; // Mark that a long press has been detected
        }
    }

    // Called when the user starts pressing the input field
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        longPressDetected = false; // Reset long press detection
        pressStartTime = Time.time;

        // Open the native keyboard if not already active
        if (keyboard == null || !keyboard.active)
        {
            keyboard = TouchScreenKeyboard.Open(tmpInputField.text, TouchScreenKeyboardType.Default, false, false, false, false);
        }
    }

    // Called when the user stops pressing the input field
    public void OnPointerUp(PointerEventData eventData)
    {
        if (longPressDetected)
        {
            PasteFromClipboard();
        }

        // Reset the state after releasing the press
        isPressing = false;
        longPressDetected = false;
    }

    // Method to paste text from the clipboard
    private void PasteFromClipboard()
    {
        string clipboardText = GUIUtility.systemCopyBuffer;
        if (!string.IsNullOrEmpty(clipboardText))
        {
            // Insert clipboard text at the current caret position
            int caretPosition = tmpInputField.caretPosition;
            string currentText = tmpInputField.text;
            tmpInputField.text = currentText.Insert(caretPosition, clipboardText);
            tmpInputField.caretPosition = caretPosition + clipboardText.Length;

            // Clear the clipboard after pasting
            GUIUtility.systemCopyBuffer = string.Empty;
        }

        
    }
}
