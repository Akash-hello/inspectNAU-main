using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellCheckmanager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;             // The TMP InputField to monitor
    public RectTransform underlineContainer;      // Usually inputField.textViewport
    public GameObject underlinePrefab;            // A red 2px UI Image prefab

    [Header("Underline Fine-Tuning")]
    public float xPadding = 1f;                   // Horizontal offset for better alignment
    public float yOffset = 2f;                    // Vertical offset to place underline below baseline

    private HashSet<string> validWords = new HashSet<string>();
    private List<GameObject> currentUnderlines = new List<GameObject>();

    void Start()
    {
        LoadDictionary();
        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    void LoadDictionary()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "wordlist.txt");
        if (File.Exists(path))
        {
            validWords = new HashSet<string>(
                File.ReadAllLines(path)
                .Select(w => w.Trim().ToLower())
                .Where(w => !string.IsNullOrEmpty(w))
            );
        }
        else
        {
            Debug.LogError("wordlist.txt not found at: " + path);
        }
    }

    void OnTextChanged(string input)
    {
        StopAllCoroutines();
        StartCoroutine(DelayedSpellCheck(input));
    }

    IEnumerator DelayedSpellCheck(string input)
    {
        yield return null; // Wait 1 frame for TMP to update layout and character info

        inputField.ForceLabelUpdate();
        TMP_TextInfo textInfo = inputField.textComponent.textInfo;

        // 🛡 Prevent index errors if field is blank or layout isn't ready
        if (string.IsNullOrWhiteSpace(input) || textInfo.characterCount == 0)
        {
            ClearUnderlines();
            yield break;
        }

        ClearUnderlines();

        string[] words = input.Split(' ');
        int wordPointer = 0;
        int charPointer = 0;

        while (wordPointer < words.Length && charPointer < textInfo.characterCount)
        {
            string word = words[wordPointer];
            string clean = new string(word.Where(char.IsLetter).ToArray()).ToLower();

            int wordCharCount = word.Length;
            int startIndex = charPointer;

            while (startIndex < textInfo.characterCount && textInfo.characterInfo[startIndex].character == ' ')
            {
                startIndex++;
            }

            int endIndex = Mathf.Min(startIndex + wordCharCount - 1, textInfo.characterCount - 1);

            if (startIndex < textInfo.characterCount && endIndex < textInfo.characterCount)
            {
                if (!validWords.Contains(clean))
                {
                    TMP_CharacterInfo startChar = textInfo.characterInfo[startIndex];
                    TMP_CharacterInfo endChar = textInfo.characterInfo[endIndex];

                    Vector3 worldStart = inputField.textComponent.transform.TransformPoint(startChar.bottomLeft);
                    Vector3 worldEnd = inputField.textComponent.transform.TransformPoint(endChar.bottomRight);

                    Vector3 localStart = underlineContainer.InverseTransformPoint(worldStart) + new Vector3(xPadding, 0, 0);
                    Vector3 localEnd = underlineContainer.InverseTransformPoint(worldEnd);

                    float underlineWidth = Mathf.Max(2f, localEnd.x - localStart.x);

                    GameObject underline = Instantiate(underlinePrefab, underlineContainer);
                    RectTransform rt = underline.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(localStart.x, localStart.y - yOffset);
                    rt.sizeDelta = new Vector2(underlineWidth, 2f);

                    currentUnderlines.Add(underline);
                }

                charPointer = endIndex + 1;
            }
            else
            {
                break;
            }

            wordPointer++;
        }
    }

    void ClearUnderlines()
    {
        foreach (var u in currentUnderlines)
            Destroy(u);
        currentUnderlines.Clear();
    }
}
