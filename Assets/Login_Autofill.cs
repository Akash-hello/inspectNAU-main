using UnityEngine;
using UnityEngine.UI;
using System.Data;
using TMPro;
using System;
using DataBank;

public class Login_Autofill : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    //public Button loginButton;

    private string dbPath;
    private string savedEmail;
    private string savedPassword;
    bool userregistered;

    void Start()
    {
        userregistered = false;
        LoadUserData();

        emailInput.onValueChanged.AddListener(OnEmailTyping);
        //loginButton.onClick.AddListener(OnLoginClicked);
    }

    void LoadUserData()
    {
        table_LoginConfig mLocationDb = new table_LoginConfig();

        using (var connection = mLocationDb.getConnection())
        {
            
            using (var countCommand = connection.CreateCommand())
            {
                countCommand.CommandText = "SELECT COUNT(*) FROM LoginConfig WHERE ID = 1";
                int count = Convert.ToInt32(countCommand.ExecuteScalar());

                if (count > 0)
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT Email, Password FROM LoginConfig WHERE ID = 1";

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                savedEmail = reader["Email"].ToString().Trim();
                                userregistered = true;
                                // savedPassword = reader["Password"].ToString().Trim();
                            }
                        }
                    }
                }
                else
                {
                    userregistered = false;
                    Debug.Log("No LoginConfig record with ID = 1 found.");
                }
            }
            connection.Close();
        }
    }


    void OnEmailTyping(string input)
    {
        if (userregistered == true)
        {
            if (!string.IsNullOrEmpty(input) && savedEmail.StartsWith(input, StringComparison.OrdinalIgnoreCase))
            {
                 emailInput.text = savedEmail;
                //passwordInput.text = savedPassword;
                emailInput.MoveTextEnd(false); // move cursor to end

            }
        }
        else
            Debug.Log("No user registered.");
    }


    //void OnLoginClicked()
    //{
    //    string email = emailInput.text.Trim();
    //    string password = passwordInput.text.Trim();

    //    Debug.Log($"Login clicked with Email: {email}");
    //    // Proceed with your authentication logic here
    //}
}
