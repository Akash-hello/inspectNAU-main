using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{
    public class InspectionTemplateEntity 
    {
        //Such ?Entity? classes are used for easy data handling. It?s not like everyone will remember the order of columns in their tables.
        public int    _ID;
        public int    _Inspection_Template_ID;
        public int    _Cloud_DB_ID;
        public string _Cloud_DB_ParentID;
        public string _Version_Number;
        public string _Version_Date;
        public string _Template_Section_Ques;
        public string _Details_1;
        public string _Details_2;
        public string _Details_3;
        public string _Details_4;
        public string _Details_5;
        public string _Details_6;
        public string _Details_7;
        public string _Details_8;

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

        public InspectionTemplateEntity(int ID, int Inspection_Template_ID, int Cloud_DB_ID, string Cloud_DB_ParentID, string Version_Number, 
            string Version_Date, string Template_Section_Ques, string Details_1, string Details_2, string Details_3, string Details_4, 
            string Details_5, string Details_6, string Details_7, string Details_8, string ShortQuestionText, string VesselTypes, 
            string ROVIQSequence, string Publications, string Objective, string IndustryGuidance, string TMSAKPI, string IMOISMCode, 
            string InspectionGuidance, string SuggestedInspectorActions, string ExpectedEvidence, string PotentialGroundsObs, 
            string Active, string Timestamp)
        {
            _ID = ID;
            _Inspection_Template_ID = Inspection_Template_ID;
            _Cloud_DB_ID = Cloud_DB_ID;
            _Cloud_DB_ParentID = Cloud_DB_ParentID;
            _Version_Number = Version_Number;
            _Version_Date = Version_Date;
            _Template_Section_Ques = Template_Section_Ques;
            _Details_1 = Details_1;
            _Details_2 = Details_2;
            _Details_3 = Details_3;
            _Details_4 = Details_4;
            _Details_5 = Details_5;
            _Details_6 = Details_6;
            _Details_7 = Details_7;
            _Details_8 = Details_8;

            //Below related specifically to the SIRE 2.0 (Year 2024)
            _ShortQuestionText_Sire = ShortQuestionText;
            _VesselTypes_Sire       = VesselTypes;
            _ROVIQSequence_Sire     = ROVIQSequence;
            _Publications_Sire      = Publications;
            _Objective_Sire         = Objective;
            _IndustryGuidance       = IndustryGuidance;
            _TMSAKPI                = TMSAKPI;
           _IMOISMCode              = IMOISMCode;
            _InspectionGuidance     = InspectionGuidance;
            _SuggestedInspectorActions  = SuggestedInspectorActions;
            _ExpectedEvidence           = ExpectedEvidence;
            _PotentialGroundsNegativeObservation = PotentialGroundsObs;

            _Active = Active;
            _Timestamp = Timestamp;


        }

        public static InspectionTemplateEntity getFakeInspectionTemplate()
        {
            return new InspectionTemplateEntity (0, 1,3025, "86", "7", "22-01-2019 05:50:11", "1.1", "Name of the vessel:", "(sire2.0)", "", "General Information", "General Information", "Note: Guidance data","","", "", "", "", "", "", "", "", "", "", "", "", "", "Y",DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }
}