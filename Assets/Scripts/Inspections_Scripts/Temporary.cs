using DataBank;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

public class Temporary : MonoBehaviour
{
    public string OriginPath;
    public RawImage m_RawImage;


    public  void tempupdatequery()

    {
        //table_Inspection_template mlocationDb1 = new table_Inspection_template();
        //string columndataquery = "ROVIQSequence = 'Pre-board, Bridge, Internal Accommodation, Cargo Control Room, Engine Room, Engine Control Room, Steering Gear, Forecastle, Mooring Decks, Exterior Decks, Main Deck, Cargo Manifold, Pumproom, Lifeboat deck, Compressor Room, Aft Mooring Deck, Documentation, Chief Engineer, Emergency Headquarters, Approaching Vessel, Anywhere, Interview' " +
        //    "" + "where  Template_Section_Ques like '%SIRE%' and cast(Cloud_DB_ParentID as int) = 0";

        //string columndataquery = "Details_3 = 'Port Bow, Starboard Bow, Port Mid Ship, Starboard Mid Ship, Port Quarter, Starboard Quarter, General view of the main deck from the monkey island, Side of the accommodation (Port), The accommodation front, Side of the accommodation (Starboard), Lifeboat (Starboard), Lifeboat (port), The after end of the main deck (Starboard), The manifold area (Starboard), The forward end of the main deck (Starboard), The after end of the main deck (port), The manifold area (Port), The forward end of the main deck (port), Catwalk, Windlass and winches (Port), The poop deck taken from above, Aft Mooring winches (Starboard), Windlass and winches (Starboard), Aft Mooring winches (Port), Foc’s’le space, Midship locker port, Midship locker Starboard, Pump Room, Steering Gear Room, Hospital, Bridge, general view, Galley, CCR, Officer`s Messroom, Crew Messroom, Alleways, General view of the compartment from the top platform, Bottom plate (port), ER Bilges (Port), Purifier room, Bottom Plate (Starboard), ER Bilges (Starboard), Diesel generators, Engine Workshop, Boilers, Steering compartment, Emergency fire pump, Emergency generator' " +
        //    "" + "where  Template_Section_Ques like '%SIRE%' and cast(Cloud_DB_ParentID as int) = 0";


        //mlocationDb1.Updatedata(columndataquery);

        //"Port Bow, Starboard Bow, Port Mid Ship, Starboard Mid Ship, Port Quarter, Starboard Quarter, General view of the main deck from the monkey island, Side of the accommodation (Port), The accommodation front, Side of the accommodation (Starboard), Lifeboat (Starboard), Lifeboat (port), The after end of the main deck (Starboard), The manifold area (Starboard), The forward end of the main deck (Starboard), The after end of the main deck (port), The manifold area (Port), The forward end of the main deck (port), Catwalk, Windlass and winches (Port), The poop deck taken from above, Aft Mooring winches (Starboard), Windlass and winches (Starboard), Aft Mooring winches (Port), Foc’s’le space, Midship locker port, Midship locker Starboard, Pump Room, Steering Gear Room, Hospital, Bridge, general view, Galley, CCR, Officer`s Messroom, Crew Messroom, Alleways, General view of the compartment from the top platform, Bottom plate (port), ER Bilges (Port), Purifier room, Bottom Plate (Starboard), ER Bilges (Starboard), Diesel generators, Engine Workshop, Boilers, Steering compartment, Emergency fire pump, Emergency generator"


        //THIS IS FOR DECLARING THE CHECKLIST HAS ROVIQ AND THEIR MAIN HEADINGS

        //=================\\

        //table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        // table_Inspection_Observations mlocationDb1 = new table_Inspection_Observations();
        //table_LoginConfig mlocationDb1 = new table_LoginConfig();

        //using var connection1 = mlocationDb1.getConnection();
        //string columndataquery3 = "Marketsetting = 'SIRE_ApiKey:SIRE@#ASA1234' " + "where cast(ID  as int) in (1)";
        ////string columndataquery3 = "Marketsetting = '' " + "where cast(ID  as int) in (1)";

        table_LoginConfig mLocationDb = new table_LoginConfig();
        using var connection = mLocationDb.getConnection();
        string columndataquery = "CompanyCode = 'ORION', CompanyGuid = 'oRio^8Id7L'";
        
        //string columndataquery = "CompanyCode = 'OSMTC', CompanyGuid = '*06La8R!N!'";
        //string columndataquery = "TokenBalance = '-2'";
        mLocationDb.Updatedata(columndataquery);
       // connection1.Close();

        // //string columndataquery1 = "Insp_Details_2 = 'Pre-board, Bridge, Internal Accommodation, Cargo Control Room, Engine Room, Engine Control Room, Steering Gear, Forecastle, Mooring Decks, Exterior Decks, Main Deck, Cargo Manifold, Pumproom, Lifeboat deck, Compressor Room, Aft Mooring Deck, Documentation, Chief Engineer, Emergency Headquarters, Approaching Vessel, Anywhere, Interview' " +
        //  //   "" + "where  Type like '%SIRE%'";

        // string columndataquery1 = "Status = 'D' where cast(ID as int) = 5";

        //mlocationDb.Updatedata(columndataquery1);

        //table_Inspection_Observations mlocationDb = new table_Inspection_Observations();

        //string columndataquery1 = "Publications = 'PROCESS ELEMENT 7:: The vessel operator should have developed a fuel-handling manual for LNG bunkers that includes emergency procedures to identify the actions that vessel staff must take in the short-and medium - term The Emergency Response Plan should cover all emergency situations identified in the LNG Bunkering Operations Risk Assessment and may designate responsibilities for local authorities, hospitals, local fire brigades, PIC, Master and selected personnel from the bunkering facility. As a minimum, the following situations should be covered where appropriate:• LNG leakage and spill on the receiving ship, on the bunkering facility or from the LNG transfer system. • Gas detection. • Fire in the bunkering area. • Unexpected movement of the vessel due to failure or loosening of mooring lines.• Unexpected moving of the truck tanker.• Unexpected venting on the receiving ship or on the bunkering facility. • Loss of power.' " + "where cast(ID  as int) in (7)";

        ////string columndataquery2 = "ShortQuestionText = 'HUMAN ELEMENT 7:: T he accompanying Officer must be familiar with the emergency procedures contained in the fuel-handling manual for LNG bunkers.' " + "where cast(ID  as int) in (7)";

        ////string columndataquery3 = "IndustryGuidance = 'HARDWARE ELEMENT 7:: The Hardware Element goes here' " + "where cast(ID  as int) in (7)";

        //string columndataquery4 = "Objective = 'OBJECTIVE 7:: To ensure that the crew will respond effectively to an emergency situation involving LNG bunker operations in accordance with the vessels shipboard emergency response plans.' " + "where cast(ID  as int) in (7)";

        //string columndataquery5 = "TMSAKPI = 'TMSA KPI 7:: KPI 11.1.1 requires that detailed vessel emergency response plans include initial notification procedures and cover all credible emergency scenarios.' " + "where cast(ID  as int) in (7)";

        ////string columndataquery6 = "IMOISMCode = 'SMS Ref 7:: GET AND FEED THE SMS REFERENCE HERE.' " + "where cast(ID  as int) in (7)";

        //string columndataquery7 = "InspectionGuidance = 'Tagged Rank 7:: Chief Officer, Crew, ' " + "where cast(ID  as int) in (7)";

        //string columndataquery8 = "SuggestedInspectorActions = 'R1' " + "where cast(ID  as int) in (7)";

        //string columndataquery1 = "Obs_Details_1 = 'Did the ship contribute to smooth and timely completion of operations?' " + "where cast(ID  as int) in (41)";
        //string columndataquery2 = "Obs_Details_1 = 'Did the shore operations proceed without any delays?' " + "where cast(ID  as int) in (42)";
        //string columndataquery3 = "Obs_Details_1 = 'Is there a clear process in place for handling heavy ballast operations, whether through a specific procedure or general guidelines (if any) required by international requirements? ' " + "where cast(ID  as int) in (72)";

        //string columndataquery1 = "Obs_Details_1 = 'Did the ship contribute to smooth and timely completion of operations?' " + "where cast(ID  as int) in (41)";
        //string columndataquery2 = "Obs_Details_1 = 'Did the shore operations proceed without any delays?' " + "where cast(ID  as int) in (42)";
        ////string columndataquery3 = "Obs_Details_1 = 'Is there a clear process in place for handling heavy ballast operations, whether through a specific procedure or general guidelines (if any) required by international requirements? ' " + "where cast(ID  as int) in (72)";

        //string columndataquery3 = "Selected_Answer = '9-Satisfactory' " + "where cast(ID  as int) in (963)";


        // mlocationDb1.Updatedata(columndataquery1);
        //mlocationDb1.Updatedata(columndataquery2);
        //mlocationDb1.Updatedata(columndataquery3);

        //connection1.Close();

        //mlocationDb.Updatedata(columndataquery4);
        //mlocationDb.Updatedata(columndataquery5);
        ////mlocationDb.Updatedata(columndataquery6);
        //mlocationDb.Updatedata(columndataquery7);
        //mlocationDb.Updatedata(columndataquery8);
        ////mlocationDb.Updatedata(columndataquery9);

        //ShortQuestionText_Sire = ""; // USE For CATEGORY - Human
        //Publications_Sire = ""; // USE For CATEGORY - Process
        //IndustryGuidance = "";  // USE For CATEGORY - Hardware
        //VesselTypes_Sire = "";
        //ROVIQSequence_Sire = ""; //Bears the Master Categories for grouping for ROVIQ option.
        //Objective_Sire = "";    //OBJECTIVE
        //TMSAKPI = "";           //TMSA KPI
        //IMOISMCode = "";        //SMS Ref
        //InspectionGuidance = "";//Tagged Rank
        //SuggestedInspectorActions = ""; // Question Type; C / R1 / R2
        //ExpectedEvidence = ""; //USE FOR MANDATORY PHOTOGRAPH

        //string SummaryComments = "2nd Inspection Comment for the Sire 2.0 inspection carried out at Rotterdam, the ship was in satisfactory condition and well trained crew and officers. The inspection was carried out whilst sailing onboard for 8 days and on roation with different crew members. Several Officers and Crew were interviewed for their knowledge and company SMS, mostly were found satisfactory.";

        //string somedata = "Port Bow, Starboard Bow, Port Mid Ship, Starboard Mid Ship, Port Quarter, Starboard Quarter, General view of the main deck from the monkey island, Side of the accommodation (Port), The accommodation front, Side of the accommodation (Starboard), Lifeboat (Starboard), Lifeboat (port), The after end of the main deck (Starboard), The manifold area (Starboard), The forward end of the main deck (Starboard), The after end of the main deck (port), The manifold area (Port), The forward end of the main deck (port), Catwalk, Windlass and winches (Port), The poop deck taken from above, Aft Mooring winches (Starboard), Windlass and winches (Starboard), Aft Mooring winches (Port), Foc’s’le space, Midship locker port, Midship locker Starboard, Pump Room, Steering Gear Room, Hospital, Bridge, general view, Galley, CCR, Officer`s Messroom, Crew Messroom, Alleways, General view of the compartment from the top platform, Bottom plate (port), ER Bilges (Port), Purifier room, Bottom Plate (Starboard), ER Bilges (Starboard), Diesel generators, Engine Workshop, Boilers, Steering compartment, Emergency fire pump, Emergency generator";

        //string somedata = "D";


        //table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        //mlocationDb.UpdateStatusPublished(1);//CHANGE STATUS OF THE INSPECTION TO PUBLISHED.

        //mlocationDb.ChangeStatusDraft(2);//CHANGE STATUS OF THE INSPECTION TO PUBLISHED.

        // string query = "Insp_Details_4 = '/2_CargoOperations_2024-10-22T173629' where cast(ID as int) = 2";

        // mlocationDb.Updatedata(query);

        //table_Config mLocationDb = new table_Config();
        //mLocationDb.Updatedata(" Https = 'httpdropdown.ToString()HiddenonUI', ServerIP = 'API_Ip_Address.text.ToString()HiddenonUI', ServerPort = 'API_Port.text.ToString()HiddenonUI', IncomingAPI = '', OutGoingAPI = ''");

        // table_LoginConfig mLocationDb = new table_LoginConfig();
        //using var connection = mLocationDb.getConnection();
        ////string columndataquery = "Name = 'Mohit', Company = 'Orion', Designation = 'Director', Email = 'someone@orionmarineconcepts.com', Phone = '919810606650',CompanyCode = 'AUBH2' ";
        //string columndataquery = "TokenBalance = '-2'";
        //mLocationDb.Updatedata(columndataquery);

        //mlocationDb.UpdateComment(somedata, 1, "Status");

        //mlocationDb1.close();

        //table_Inspection_Attachments mlocationDb2 = new table_Inspection_Attachments();
        //string query = "Attachment_Name = '358_2024-10-23T140555Rating_5_Media_3_que3_upload.jpg' where cast(ID as int) = '14' and cast(Inspection_PrimaryDetails_ID as int) = '2' ";
        //mlocationDb2.Updatedata(query);    

        //string columnquery = "cast(Inspection_PrimaryDetails_ID as int) = 1 and Attachment_Title like '%Human%' or Attachment_Title like '%Process%' or Attachment_Title like '%Hardware%'";
        //mlocationDb2.deleteDataByquery(columnquery);

        //mlocationDb2.close();

        //table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        //using var connection = mlocationDb2.getConnection();
        //string columnquery = "Obs_Details_1 = ' Were the Master and Officers familiar with the company procedures for the use of Position Reference Systems (PRS), and was the equipment in satisfactory condition with sensor offset data readily available to the DPO?' where cast(Inspection_PrimaryDetails_ID as int) = 1 and cast(ID as int) = '593'";
        //mlocationDb2.Updatedata(columnquery);

        //table_Inspection_Observations mlocationDb2 = new table_Inspection_Observations();
        //using var connection = mlocationDb2.getConnection();
        //string columnquery = "RiskCategory = '' where Obs_Details_8 like '%Question%' ";
        //mlocationDb2.Updatedata(columnquery);


        //table_Inspection_template mlocationDb2 = new table_Inspection_template();
        //using var connection = mlocationDb2.getConnection();
        //string columnquery = "Details_2 = 'SIRE' where Details_2 like '%Pre Vetting Inspection (SIRE 2.0)%'  ";
        //mlocationDb2.Updatedata(columnquery);


        //table_Inspection_PrimaryDetails mlocationDb = new table_Inspection_PrimaryDetails();
        //using var connection1 = mlocationDb.getConnection();
        ////string columndataquery1 = "Insp_Details_2 = 'Pre-board, Bridge, Internal Accommodation, Cargo Control Room, Engine Room, Engine Control Room, Steering Gear, Forecastle, Mooring Decks, Exterior Decks, Main Deck, Cargo Manifold, Pumproom, Lifeboat deck, Compressor Room, Aft Mooring Deck, Documentation, Chief Engineer, Emergency Headquarters, Approaching Vessel, Anywhere, Interview' " +
        ////    "" + "where  Type like '%SIRE%'";

        //string columndataquery1 = "Type = 'SIRE'" + "where  Type like '%Pre Vetting Inspection (SIRE 2.0)%'";

        //mlocationDb.Updatedata(columndataquery1);

        //mlocationDb.close();
        // mLocationDb.close();
    }
    // Start is called before the first frame update
    //public void displayphoto()
    //{
    //    OriginPath = "C:/Users/msabh/AppData/LocalLow/Launchfort Technologies/Orion Inspections App/19_SIRE_2024-02-16T072212/MediaFiles/847_2024-02-16T075801_IMG_0848.JPG";
    //    StartCoroutine(LoadTexture());
    //}

    //IEnumerator LoadTexture()
    //{

    //        WWW www = new WWW(OriginPath);
    //        while (!www.isDone)
    //        yield return null;
    //        m_RawImage.GetComponent<RawImage>().texture = www.texture;

    //}
}
