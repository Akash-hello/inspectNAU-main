using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonGroupController : MonoBehaviour
{
    [Header("Button Group Settings")]
    [SerializeField] private Button[] buttonsInGroup;
    
    [Header("Button Images")]
    [SerializeField] private Sprite unclickedImage;
    [SerializeField] private Sprite clickedImage;
    
    [Header("Button Colors")]
    [SerializeField] private Color unclickedImageColor = Color.white;
    [SerializeField] private Color clickedImageColor = Color.gray;
    [SerializeField] private Color unclickedTextColor = Color.black;
    [SerializeField] private Color clickedTextColor = Color.white;
    
    private int currentSelectedIndex = -1;
    
    private void Start()
    {
        InitializeButtons();
    }
    
    private void InitializeButtons()
    {
        for (int i = 0; i < buttonsInGroup.Length; i++)
        {
            if (buttonsInGroup[i] != null)
            {
                int buttonIndex = i;
                buttonsInGroup[i].onClick.AddListener(() => OnButtonClicked(buttonIndex));
                
                if (i == 0)
                {
                    // Set the first button (Home) as selected by default
                    SetButtonAsClicked(i);
                    currentSelectedIndex = 0;
                }
                else
                {
                    SetButtonToUnclicked(i);
                }
            }
        }
    }
    
    public void OnButtonClicked(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= buttonsInGroup.Length)
            return;
            
        ResetAllButtonsToUnclicked();
        
        SetButtonAsClicked(buttonIndex);
        
        currentSelectedIndex = buttonIndex;
    }
    
    public void OnButtonClickedByReference(Button clickedButton)
    {
        for (int i = 0; i < buttonsInGroup.Length; i++)
        {
            if (buttonsInGroup[i] == clickedButton)
            {
                OnButtonClicked(i);
                break;
            }
        }
    }
    
    private void ResetAllButtonsToUnclicked()
    {
        for (int i = 0; i < buttonsInGroup.Length; i++)
        {
            if (buttonsInGroup[i] != null)
            {
                SetButtonToUnclicked(i);
            }
        }
    }
    
    private void SetButtonToUnclicked(int buttonIndex)
    {
        if (buttonsInGroup[buttonIndex] != null)
        {
            // Handle button image sprite only (no color change)
            Image buttonImage = buttonsInGroup[buttonIndex].GetComponent<Image>();
            if (buttonImage != null && unclickedImage != null)
            {
                buttonImage.sprite = unclickedImage;
            }
            
            // Handle child components colors only
            SetButtonChildColors(buttonsInGroup[buttonIndex], unclickedTextColor);
        }
    }
    
    private void SetButtonAsClicked(int buttonIndex)
    {
        if (buttonsInGroup[buttonIndex] != null)
        {
            // Handle button image sprite only (no color change)
            Image buttonImage = buttonsInGroup[buttonIndex].GetComponent<Image>();
            if (buttonImage != null && clickedImage != null)
            {
                buttonImage.sprite = clickedImage;
            }
            
            // Handle child components colors only
            SetButtonChildColors(buttonsInGroup[buttonIndex], clickedTextColor);
        }
    }
    
    private void SetButtonChildColors(Button button, Color textColor)
    {
        // Handle child TextMeshPro components only
        TextMeshProUGUI[] childTMPs = button.GetComponentsInChildren<TextMeshProUGUI>(false);
        foreach (TextMeshProUGUI tmp in childTMPs)
        {
            // Skip the button's own TMP component if it exists
            if (tmp.transform != button.transform)
            {
                tmp.color = textColor;
            }
        }
        
        // Handle child Text components only
        Text[] childTexts = button.GetComponentsInChildren<Text>(false);
        foreach (Text text in childTexts)
        {
            // Skip the button's own Text component if it exists
            if (text.transform != button.transform)
            {
                text.color = textColor;
            }
        }
        
        // Handle child Image components only (like icons)
        Image[] childImages = button.GetComponentsInChildren<Image>(false);
        foreach (Image img in childImages)
        {
            // Skip the button's own main image component
            if (img.transform != button.transform)
            {
                img.color = textColor;
            }
        }
    }
    
    public void ResetSelection()
    {
        ResetAllButtonsToUnclicked();
        currentSelectedIndex = -1;
    }
    
    public int GetCurrentSelectedIndex()
    {
        return currentSelectedIndex;
    }
    
    public Button GetCurrentSelectedButton()
    {
        if (currentSelectedIndex >= 0 && currentSelectedIndex < buttonsInGroup.Length)
        {
            return buttonsInGroup[currentSelectedIndex];
        }
        return null;
    }
}