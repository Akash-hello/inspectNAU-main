using System;
using UnityEngine;
using UnityEngine.UI;

public class MyButtonHandler : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        try
        {
            DatabaseManager.Instance.ExecuteNonQuery("INSERT INTO MyTable (Column1) VALUES ('Value1')");
            Debug.Log("Button clicked and database operation performed.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Database operation failed: " + ex.Message);
        }
    }
}
