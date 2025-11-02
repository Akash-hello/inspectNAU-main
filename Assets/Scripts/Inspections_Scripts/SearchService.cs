using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBank;
using TMPro;
using UnityEngine;

/// <summary>
/// Reusable smart search service for filtering Inspection_template DB.
/// Supports progressive AND and OR search via "&" and "," respectively.
/// </summary>
public class SearchService : MonoBehaviour
{
    public TMP_InputField searchInput;
    public GameObject PopUp;
    public TextMeshProUGUI PopUpMsg;

    public void ClearSearchBox()
    {
        searchInput.text = "";
    }


        public void OnSearchClicked()
    {
        string input = searchInput.text;
        List<QAItem> results = SearchByGroupedKeywords(input);

        foreach (var item in results)
        {
            Debug.Log($"{item.RowID}:{item.QuestionNumber}: {item.QuestionText}");
        }
    }

    /// <summary>
    /// Parses mixed keyword groups: 
    /// - comma (,) for OR between groups 
    /// - ampersand (&) for AND within group
    /// </summary>
    public List<List<string>> ParseKeywordGroups(string input)
    {
        return input.Split(',')
            .Select(group => group
                .Split('&')
                .Select(k => k.Trim().ToLower())
                .Where(k => !string.IsNullOrEmpty(k))
                .Distinct()
                .ToList()
            )
            .Where(g => g.Count > 0)
            .ToList();

    }

    public List<QAItem> SearchByGroupedKeywords(string input)
    {
        List<List<string>> keywordGroups = ParseKeywordGroups(input);
        List<QAItem> totalResults = new List<QAItem>();

        foreach (var group in keywordGroups)
        {
            if (group.Count == 0) continue;

            List<QAItem> groupResults = new List<QAItem>();

            // STEP 1: SQLite search using the first keyword in group
            table_Inspection_Observations mLocationDb = new table_Inspection_Observations();
            using (var connection = mLocationDb.getConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    string kw = group[0];
                    cmd.CommandText = @"
                        SELECT * FROM Inspection_Observations 
                        WHERE Obs_Details_8 LIKE '%Question%' AND cast(Inspection_PrimaryDetails_ID as int) = '3' AND (
                            LOWER(Obs_Details_1) LIKE @kw OR 
                            LOWER(InspectionGuidance) LIKE @kw OR 
                            LOWER(ROVIQSequence) LIKE @kw
                        )";
                    cmd.Parameters.AddWithValue("@kw", "%" + kw + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            groupResults.Add(new QAItem
                            {
                                RowID = int.Parse(reader["ID"].ToString()),
                                QuestionNumber = reader["Template_Section_Ques"].ToString(),
                                QuestionText = reader["Obs_Details_1"].ToString(),
                                ROVIQSequence = reader["ROVIQSequence"].ToString(),
                                HumanElement = reader["ShortQuestionText"].ToString(),
                                ProcessElement = reader["Publications"].ToString(),
                                HardwareElement = reader["IndustryGuidance"].ToString(),
                                Objective = reader["Objective"].ToString(),
                                Rank = reader["InspectionGuidance"].ToString(),
                                SelectedAnswer = reader["Selected_Answer"].ToString()
                            });
                        }
                    }
                }
                connection.Close();
            }

            // STEP 2: In-memory AND logic filtering on remaining keywords
            for (int i = 1; i < group.Count; i++)
            {
                string kw = group[i];
                groupResults = groupResults
                    .Where(q =>
                        (!string.IsNullOrEmpty(q.QuestionText) && q.QuestionText.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(q.Rank) && q.Rank.ToLower().Contains(kw)) ||
                        (!string.IsNullOrEmpty(q.ROVIQSequence) && q.ROVIQSequence.ToLower().Contains(kw)))
                    .ToList();
            }

            totalResults.AddRange(groupResults);
        }

        return totalResults.Distinct().ToList();

    }

    public void SmartSearchInfo()
    {
        PopUp.SetActive(true);
        PopUpMsg.text = "<b>Smart Roving Options: </b> " + Environment.NewLine + "<b>1•</b> Comma (,) → separate terms Example: Chief Engineer, Deck." + Environment.NewLine+

"<b>2•</b> Ampersand(&) → combined filter Example: Chief Engineer & Deck.";
    }

    // Nested data class for results
    public class QAItem
    {
        public int RowID;
        public string QuestionNumber;
        public string QuestionText;
        public string ROVIQSequence;
        public string HumanElement;
        public string ProcessElement;
        public string HardwareElement;
        public string Objective;
        public string Rank;
        public string SelectedAnswer;
    }
}
