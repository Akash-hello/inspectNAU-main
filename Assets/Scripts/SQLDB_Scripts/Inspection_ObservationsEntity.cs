using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{

    public class Inspection_ObservationsEntity
    {
        //Such “Entity” classes are used for easy data handling. It’s not like everyone will remember the order of columns in their tables.
        public int _ID;
        public int _Inspection_PrimaryDetails_ID;
        public int _Cloud_DB_ID;
        public string _Cloud_DB_ParentID;
        public string _Version_Number;
        public string _Version_Date;
        public string _Template_Section_Ques;
        public string _Selected_Answer;
        public string _Date;
        public string _Time;
        public string _SomeDetail;
        public string _Observation_Text;
        public string _RiskCategory;
        public string _TargetDate;
        public string _Recommendation;
        public string _Obs_Details_1;
        public string _Obs_Details_2;
        public string _Obs_Details_3;
        public string _Obs_Details_4;
        public string _Obs_Details_5;
        public string _Obs_Details_6;
        public string _Obs_Details_7;
        public string _Obs_Details_8;

        //Below related specifically to the SIRE 2.0 (Year 2024)
        public string _ShortQuestionText_Sire;
        public string _VesselTypes_Sire;
        public string _ROVIQSequence_Sire;
        public string _Publications_Sire;
        public string _Objective_Sire;
        public string _IndustryGuidance;
        public string _TMSAKPI;
        public string _IMOISMCode;
        public string _InspectionGuidance;
        public string _SuggestedInspectorActions;
        public string _ExpectedEvidence;
        public string _PotentialGroundsNegativeObservation;

        public string _Active;
        public string _Timestamp;
               

        public Inspection_ObservationsEntity(int ID, int Inspection_PrimaryDetails_ID, int Cloud_DB_ID, string Cloud_DB_ParentID, string Version_Number, string Version_Date, string Template_Section_Ques, string Selected_Answer, string Date, string Time, string SomeDetail,string Observation_Text, string RiskCategory, string TargetDate, string Recommendation, string Obs_Details_1, string Obs_Details_2, string Obs_Details_3, string Obs_Details_4, string Obs_Details_5, string Obs_Details_6, string Obs_Details_7, string Obs_Details_8, string ShortQuestionText, string VesselTypes,string ROVIQSequence, string Publications, string Objective, string IndustryGuidance, string TMSAKPI, string IMOISMCode, string InspectionGuidance, string SuggestedInspectorActions, string ExpectedEvidence, string PotentialGroundsObs,string Active, string Timestamp)
        {
            _ID = ID;
            _Inspection_PrimaryDetails_ID = Inspection_PrimaryDetails_ID;
            _Cloud_DB_ID = Cloud_DB_ID;
            _Cloud_DB_ParentID = Cloud_DB_ParentID;
            _Version_Number = Version_Number;
            _Version_Date = Version_Date;
            _Template_Section_Ques = Template_Section_Ques;
            _Selected_Answer = Selected_Answer;
            _Date = Date;
            _Time = Time;
            _SomeDetail = SomeDetail;
            _Observation_Text = Observation_Text;
            _RiskCategory = RiskCategory;
            _TargetDate = TargetDate;
            _Recommendation = Recommendation;
            _Obs_Details_1 = Obs_Details_1;
            _Obs_Details_2 = Obs_Details_2;
            _Obs_Details_3 = Obs_Details_3;
            _Obs_Details_4 = Obs_Details_4;
            _Obs_Details_5 = Obs_Details_5;
            _Obs_Details_6 = Obs_Details_6;
            _Obs_Details_7 = Obs_Details_7;
            _Obs_Details_8 = Obs_Details_8;
            
            //Below related specifically to the SIRE 2.0 (Year 2024)
            _ShortQuestionText_Sire = ShortQuestionText;
            _VesselTypes_Sire = VesselTypes;
            _ROVIQSequence_Sire = ROVIQSequence;
            _Publications_Sire = Publications;
            _Objective_Sire = Objective;
            _IndustryGuidance = IndustryGuidance;
            _TMSAKPI = TMSAKPI;
            _IMOISMCode = IMOISMCode;
            _InspectionGuidance = InspectionGuidance;
            _SuggestedInspectorActions = SuggestedInspectorActions;
            _ExpectedEvidence = ExpectedEvidence;
            _PotentialGroundsNegativeObservation = PotentialGroundsObs;

            _Active = Active;
            _Timestamp = Timestamp;

        }
        public static Inspection_ObservationsEntity getFakeObservationsDetails()
        {
            return new Inspection_ObservationsEntity(0, 1, 4, "86", "2.01", "18-12-2023", "The question here.", "Y", "18-12-2023", "0.520833333333333", "Some details", "The observation text given", "Low", "45292", "Recommendation", "-", "-", "-", "-", "-", "-", "-", "-", "", "", "", "", "", "", "", "", "", "", "", "",  "Y", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }
}