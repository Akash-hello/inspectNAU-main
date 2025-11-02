using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionTemplatesMainClass
{
    public List<Version> Version { get; set; }
    //public Version Data { get; set; }
    public List<Chapter> Chapters { get; set; }
    public List<Question> Question { get; set; }
}

//PENDING IN THE API, BELOW 12 COMMENTED fields have to be added for SIRE 2.0 after  public string Details_8 { get; set; }

public class Chapter
    {
    public int Cloud_DB_ID { get; set; }
    public string Cloud_DB_ParentID { get; set; }
    public string Version_Number { get; set; }
    public string Version_Date { get; set; }
    public string Template_Section_Ques { get; set; }   //CHAPTER NUMBER GOES HERE.
    public string Details_1 { get; set; }               // CHAPTER TEXT GOES HERE.
    public string Details_2 { get; set; }               //ALWAYS BLANK
    public string Details_3 { get; set; }               //ALWAYS BLANK
    public string Details_4 { get; set; }
    public string Details_5 { get; set; }
    public string Details_6 { get; set; }
    public string Details_7 { get; set; }
    public string Details_8 { get; set; }

    public string ShortQuestionText { get; set; }
    public string VesselTypes { get; set; }
    public string ROVIQSequence { get; set; }
    public string Publications { get; set; }
    public string Objective { get; set; }
    public string IndustryGuidance { get; set; }
    public string TMSAKPI { get; set; }
    public string IMOISMCode { get; set; }
    public string InspectionGuidance { get; set; }
    public string SuggestedInspectorActions { get; set; }
    public string ExpectedEvidence { get; set; }
    public string PotentialGroundsNegativeObservation { get; set; }


    public string Active { get; set; }
    public string Timestamp { get; set; }
}

    public class Question
    {
    public int    Cloud_DB_ID { get; set; }
    public string Cloud_DB_ParentID { get; set; }
    public string Version_Number { get; set; }
    public string Version_Date { get; set; }
    public string Template_Section_Ques { get; set; }   //QUESTION NUMBER GOES HERE.
    public string Details_1 { get; set; }               // QUESTION TEXT GOES HERE.
    public string Details_2 { get; set; }               // TYPE OF INSP (SIRE,CDI,DEFECT,COND, etc.) Goes here.
    public string Details_3 { get; set; }               // RISK, Low, Med, High, etc. Goes here.
    public string Details_4 { get; set; }
    public string Details_5 { get; set; }
    public string Details_6 { get; set; }
    public string Details_7 { get; set; }
    public string Details_8 { get; set; }

    public string ShortQuestionText { get; set; }   // USE For CATEGORY - Human - SIRE 2.0
    public string VesselTypes { get; set; }
    public string ROVIQSequence { get; set; }       //Bears the Master Categories for grouping for ROVIQ option.
    public string Publications { get; set; }        // USE For CATEGORY - Process
    public string Objective { get; set; }           //OBJECTIVE
    public string IndustryGuidance { get; set; }    // USE For CATEGORY - Hardware
    public string TMSAKPI { get; set; }             //TMSA KPI
    public string IMOISMCode { get; set; }          //SMS Ref
    public string InspectionGuidance { get; set; }  //Tagged Rank
    public string SuggestedInspectorActions { get; set; }   // Question Type; C / R1 / R2
    public string ExpectedEvidence { get; set; }            //USE FOR MANDATORY PHOTOGRAPH
    public string PotentialGroundsNegativeObservation { get; set; }

    public string Active { get; set; }
    public string Timestamp { get; set; }
}

public class Version
{
        public int Cloud_DB_ID { get; set; }
        public string Cloud_DB_ParentID { get; set; } //ALWAYS BLANK, SINCE THIS IS PARENT ITSELF
        public string Version_Number { get; set; }
        public string Version_Date { get; set; }
        public string Template_Section_Ques { get; set; } //INSPECTION TYPE GOES HERE
        public string Details_1 { get; set; }               //DECIDES THE ANSWER GROUP FOR THE ENTIRE INSPECTION TEMPLATE (Yes/No..., Good/Fair..., Obs/NC...)
        public string Details_2 { get; set; }               //USE THIS TO DETECT ANY UPDATE TO THE VERSION, INITIAL is "", then 1, 2, ...
        public string Details_3 { get; set; }
        public string Details_4 { get; set; }
        public string Details_5 { get; set; }
        public string Details_6 { get; set; }
        public string Details_7 { get; set; }
        public string Details_8 { get; set; }

    public string ShortQuestionText { get; set; }
    public string VesselTypes { get; set; }
    public string ROVIQSequence { get; set; }
    public string Publications { get; set; }
    public string Objective { get; set; }
    public string IndustryGuidance { get; set; }
    public string TMSAKPI { get; set; }
    public string IMOISMCode { get; set; }
    public string InspectionGuidance { get; set; }
    public string SuggestedInspectorActions { get; set; }
    public string ExpectedEvidence { get; set; }
    public string PotentialGroundsNegativeObservation { get; set; }

    public string Active { get; set; }
        public string Timestamp { get; set; }
    }

public class FromDatabase_Insptemplates
{
    public int ID { get; set; }
    public int Template_ID { get; set; }
    public int Cloud_DB_ID { get; set; }
    public string Cloud_DB_ParentID { get; set; }
    public string Version_Number { get; set; }
    public string Version_Date { get; set; }
    public string Template_Section_Ques { get; set; }
    public string Details_1 { get; set; }
    public string Details_2 { get; set; }
    public string Details_3 { get; set; }
    public string Details_4 { get; set; }
    public string Details_5 { get; set; }
    public string Details_6 { get; set; }
    public string Details_7 { get; set; }
    public string Details_8 { get; set; }

    public string ShortQuestionText { get; set; }
    public string VesselTypes { get; set; }
    public string ROVIQSequence { get; set; }
    public string Publications { get; set; }
    public string Objective { get; set; }
    public string IndustryGuidance { get; set; }
    public string TMSAKPI { get; set; }
    public string IMOISMCode { get; set; }
    public string InspectionGuidance { get; set; }
    public string SuggestedInspectorActions { get; set; }
    public string ExpectedEvidence { get; set; }
    public string PotentialGroundsNegativeObservation { get; set; }

    public string Active { get; set; }
    public string Timestamp { get; set; }
}
