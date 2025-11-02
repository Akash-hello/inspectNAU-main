using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public QADatabaseLoader dbLoader;
    public string location = "Engine Room"; // Pass dynamically from UI
    public string rank = "Chief Engineer"; // Pass dynamically
    public string loadStrategy = "location"; // or "rank" or "lazy"
    public GameObject chatContent, chatBubblePrefab;
    public TMP_InputField inputField;
    public Button submitButton;

    private List<QADatabaseLoader.QAItem> questions;
    private int currentIndex = 0;

    //void Start()
    //{
    //    if (loadStrategy == "location")
    //        questions = dbLoader.LoadByLocation(location);
    //    else if (loadStrategy == "rank")
    //        questions = dbLoader.LoadByRank(rank);
    //    else
    //        questions = dbLoader.LoadLazy(0, 10);

    //    //DisplayNextQuestion();

    //    ShowCurrentQuestion();
    //    Debug.Log("Remaining (Location): " + dbLoader.CountRemainingUnanswered(location, "location"));
    //    Debug.Log("Remaining (Rank): " + dbLoader.CountRemainingUnanswered(rank, "rank"));
    //    Debug.Log("Remaining (Overall): " + dbLoader.CountRemainingUnanswered("", "overall"));
    //}

    public void DisplayNextQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            AddMessage("✅ Inspection complete.");
            return;
        }

        var q = questions[currentIndex];
        AddMessage($"📘 {q.QuestionNumber}: {q.QuestionText}");

        if (!string.IsNullOrEmpty(q.Objective))
            AddMessage($"📌 Objective: {q.Objective}");

        if (!string.IsNullOrEmpty(q.HumanElement))
            AddMessage($"👥 Human Element: {q.HumanElement}");

        if (!string.IsNullOrEmpty(q.ProcessElement))
            AddMessage($"📋 Process Element: {q.ProcessElement}");

        if (!string.IsNullOrEmpty(q.HardwareElement))
            AddMessage($"⚙️ Hardware Element: {q.HardwareElement}");

        currentIndex++;
    }

    void ShowCurrentQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            AddBotMessage("✅ Inspection complete.");
            inputField.interactable = false;
            submitButton.interactable = false;
            return;
        }

        var q = questions[currentIndex];

        AddBotMessage($"📘 {q.QuestionNumber}: {q.QuestionText}");

        if (!string.IsNullOrWhiteSpace(q.Objective))
            AddBotMessage($"📌 Objective: {q.Objective}");

        if (!string.IsNullOrWhiteSpace(q.HumanElement))
            AddBotMessage($"👥 Human Element: {q.HumanElement}");

        if (!string.IsNullOrWhiteSpace(q.ProcessElement))
            AddBotMessage($"📋 Process Element: {q.ProcessElement}");

        if (!string.IsNullOrWhiteSpace(q.HardwareElement))
            AddBotMessage($"⚙️ Hardware Element: {q.HardwareElement}");
    }

    public void SubmitAnswer()
    {
        string userAnswer = inputField.text.Trim();
        if (string.IsNullOrEmpty(userAnswer))
            return;

        AddUserMessage(userAnswer);
        inputField.text = "";

        // Normally you'd save the answer here to SQLite using RowID
        currentIndex++;
        DisplayNextQuestion();
        //ShowCurrentQuestion();
    }

    void AddBotMessage(string text)
    {
        GameObject bubble = Instantiate(chatBubblePrefab, chatContent.transform);
        bubble.GetComponentInChildren<TMP_Text>().text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent.GetComponent<RectTransform>());
    }

    void AddMessage(string text)
    {
        GameObject bubble1 = Instantiate(chatBubblePrefab, chatContent.transform);
        bubble1.GetComponentInChildren<TMP_Text>().text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent.GetComponent<RectTransform>());
    }

    void AddUserMessage(string text)
    {
        GameObject bubble = Instantiate(chatBubblePrefab, chatContent.transform);
        bubble.GetComponentInChildren<TMP_Text>().text = "🧍 " + text;
        bubble.GetComponent<Image>().color = new Color(0.8f, 0.95f, 1f); // light blue for user
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatContent.GetComponent<RectTransform>());
    }

}
