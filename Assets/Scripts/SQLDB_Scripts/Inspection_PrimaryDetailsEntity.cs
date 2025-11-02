using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBank
{

    public class Inspection_PrimaryDetailsEntity 
    {
        //Such ?Entity? classes are used for easy data handling. It?s not like everyone will remember the order of columns in their tables.
        public int    _ID;
        public string _Vessel_Name;
        public string _Vessel_IMO;
       
        public string _Type;
        public int    _Checklist_ID;
        public string _Vessel_TYPE;
        public string _Vessel_Extra1;
        public string _Vessel_Extra2;
        public string _Vessel_Extra3;
        public string _Inspection_Date;
        public string _Inspector_Name;
        public string _Inspector_Company;
        public string _Location_Port_Sea;
        public string _Port;
        public string _Country;
        public string _Opening_Meeting_DateTimeFm;
        public string _Opening_Meeting_DateTimeTo;
        public string _Closing_Meeting_DateTimeFm;
        public string _Closing_Meeting_DateTimeTo;
        public string _Master_Name;
        public string _Chief_Engineer_Name;
        public string _Chief_Officer_Name;
        public string _Second_Engr_Name;
        public string _Comments;
        public string _Status; // N = New, D = Draft, P = Published, Z = Deleted, O = Re-opened....
        public string _Insp_Details_1;
        public string _Insp_Details_2;
        public string _Insp_Details_3;
        public string _Insp_Details_4;
        public string _Timestamp;


        public Inspection_PrimaryDetailsEntity(int ID, string Vessel_Name, string Vessel_IMO, string Type, int Checklist_ID, string vesseltype, string vesselextra1, string vesselextra2, string vesselextra3, string Inspection_Date, string Inspector_Name, string Inspector_Company, string Location_Port_Sea, string Port, string Country, string Opening_Meeting_DateTimeFm, string Opening_Meeting_DateTimeTo, string Closing_Meeting_DateTimeFm, string Closing_Meeting_DateTimeTo, string Master_Name, string Chief_Engineer_Name, string Chief_Officer_Name, string Second_Engr_Name, string Comments, string Status, string Insp_Details_1, string Insp_Details_2, string Insp_Details_3, string Insp_Details_4, string Timestamp)
        {
            _ID = ID;
            _Vessel_Name = Vessel_Name;
            _Vessel_IMO = Vessel_IMO;

          
            _Type = Type;

            _Checklist_ID = Checklist_ID;
            _Vessel_TYPE = vesseltype;
            _Vessel_Extra1 = vesselextra1;
            _Vessel_Extra2 = vesselextra2;
            _Vessel_Extra3 = vesselextra3;
            _Inspection_Date = Inspection_Date;
            _Inspector_Name = Inspector_Name;
            _Inspector_Company = Inspector_Company;
            _Location_Port_Sea = Location_Port_Sea;
            _Port = Port;
            _Country = Country;
            _Opening_Meeting_DateTimeFm = Opening_Meeting_DateTimeFm;
            _Opening_Meeting_DateTimeTo = Opening_Meeting_DateTimeTo;
            _Closing_Meeting_DateTimeFm = Closing_Meeting_DateTimeFm;
            _Closing_Meeting_DateTimeTo = Closing_Meeting_DateTimeTo;
            _Master_Name = Master_Name;
            _Chief_Engineer_Name = Chief_Engineer_Name;
            _Chief_Officer_Name = Chief_Officer_Name;
            _Second_Engr_Name = Second_Engr_Name;
            _Comments = Comments;
            _Status = Status;
            _Insp_Details_1 = Insp_Details_1;
            _Insp_Details_2 = Insp_Details_2;
            _Insp_Details_3 = Insp_Details_3;
            _Insp_Details_4 = Insp_Details_4;
            _Timestamp = Timestamp;

        }   
        public static Inspection_PrimaryDetailsEntity getFakeInspectionPrimaryDetails()
        {
            return new Inspection_PrimaryDetailsEntity (0, "Champion_Tide", "9876543", "(sire2.0)", 6, "Tanker", "Extra1","Extra2","Extra3","18-12-2023", "Capt. Vivek Ponani", "Shell Petroleum", "At Port", "Houston", "United States", "18-12-2023", "0.520833333333333", "19-12-2023", "06:00", "Capt. Bonanza", "Sudhakar Rao", "Andy Lewis","Second Engineer Name" ,"The comments go here", "Draft", "-", "-", "-", "-", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
    }
}