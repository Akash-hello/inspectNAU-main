using System.Collections.Generic;
//using System.Data;
using UnityEngine;

using DataBank;

public class QADatabaseLoader : MonoBehaviour
{
    //public string dbPath = "URI=file:InspectNAU_InspectionTemplate_Extended.sqlite";

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

    public List<QAItem> LoadByLocation(string location)
    {
        return QueryDB($"SELECT * FROM Inspection_Observations WHERE ROVIQSequence LIKE '%{location}%' AND Obs_Details_8 LIKE '%Question%' ORDER BY ROVIQSequence ASC");
    }

    public List<QAItem> LoadByRank(string rank)
    {
        return QueryDB($"SELECT * FROM Inspection_Observations WHERE InspectionGuidance LIKE '%{rank}%' AND Obs_Details_8 LIKE '%Question%' ORDER BY ROVIQSequence ASC");
    }

    public List<QAItem> LoadLazy(int offset = 0, int limit = 10)
    {
        return QueryDB($"SELECT * FROM Inspection_Observations WHERE Obs_Details_8 LIKE '%Question%' ORDER BY ROVIQSequence ASC LIMIT {limit} OFFSET {offset}");
    }

    public int CountRemainingUnanswered(string filter = "", string filterType = "")
    {
        string whereClause = "Obs_Details_8='Question' AND (Selected_Answer IS NULL OR TRIM(Selected_Answer) = '')";
        if (filterType == "location") whereClause += $" AND ROVIQSequence LIKE '%{filter}%'";
        if (filterType == "rank") whereClause += $" AND InspectionGuidance LIKE '%{filter}%'";
        if (filterType == "overall") whereClause += "";

        table_Inspection_Observations mLocationDb = new table_Inspection_Observations();

        using (var connection = mLocationDb.getConnection())
        {
            
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT COUNT(*) FROM Inspection_Observations WHERE {whereClause}";
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            
        }
        
    }

    private List<QAItem> QueryDB(string sql)
    {
        List<QAItem> list = new List<QAItem>();

        table_Inspection_template mLocationDb = new table_Inspection_template();

        using (var connection = mLocationDb.getConnection())
        {
            
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = sql;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new QAItem
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

                            //RowID = int.Parse(reader["RowID"].ToString()),
                            //QuestionNumber = reader["QuestionNumber"].ToString(),
                            //QuestionText = reader["QuestionText"].ToString(),
                            //ROVIQSequence = reader["ROVIQSequence"].ToString(),
                            //HumanElement = reader["HumanElement"].ToString(),
                            //ProcessElement = reader["ProcessElement"].ToString(),
                            //HardwareElement = reader["HardwareElement"].ToString(),
                            //Objective = reader["Objective"].ToString(),
                            //Rank = reader["Rank"].ToString(),
                            //SelectedAnswer = reader["SelectedAnswer"].ToString()
                        });
                    }
                }
            }
        }

        return list;
    }
}
