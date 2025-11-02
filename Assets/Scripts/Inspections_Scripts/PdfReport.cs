using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataBank;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

using SimpleJSON;
using System.Text;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Linq;
//using UnityAndroidOpenUrl;
using System.Text.RegularExpressions;
//using System.Drawing.Imaging;
using System.Data;
using Image = UnityEngine.UI.Image;

public class PdfReport : MonoBehaviour
{
    string path = null;
    string InspSign = "";
    string MasterSign = "";
    public RawImage QRCode;
    public RawImage Coverpagetop;
    RawImage cover;
    public RawImage Logo1;
    public RawImage CompanyLogo;
    public RawImage alternateToCompanylogo;
    public RawImage HeaderLine;
    public RawImage FooterLine;
    private UnityEngine.UI.Image image = null;
    public Text ItemCode;
    public Text setitemcode;
    public Text ItemName;
    int selecteditemqty = 10;

    public TextMeshProUGUI Shipsname;
    public TextMeshProUGUI ShipImo;
    public TextMeshProUGUI InspectType;
    public TextMeshProUGUI InspectDatFmTo;
    public TextMeshProUGUI Openingmeeting;

    public TextMeshProUGUI Inspectorname;
    public TextMeshProUGUI InspectorsQualifications;
    public TextMeshProUGUI InspectingCompanysName;
    public TextMeshProUGUI MasterName;
    public TextMeshProUGUI ChiefEng;
    public TextMeshProUGUI ChiefOffr;
    public TextMeshProUGUI SecondEngr;

    public TextMeshProUGUI TotalAnswered;
    //public TextMeshProUGUI TotalObs;

    int answergroup;

    public GameObject LabelAnsweredImg;

    public TextMeshProUGUI LabelGeneral;
    public TextMeshProUGUI Label1;
    public TextMeshProUGUI Label2;
    public TextMeshProUGUI Label3;
    public TextMeshProUGUI Label4;
    public TextMeshProUGUI Label5;
    public TextMeshProUGUI LabelGeneralCount;
    public TextMeshProUGUI Label1Count;
    public TextMeshProUGUI Label2Count;
    public TextMeshProUGUI Label3Count;
    public TextMeshProUGUI Label4Count;
    public TextMeshProUGUI Label5Count;

    public TextMeshProUGUI LowRiskCount;
    public TextMeshProUGUI MedRiskCount;
    public TextMeshProUGUI HighRiskCount;

    public TextMeshProUGUI HumanCount;
    public TextMeshProUGUI ProcessCount;
    public TextMeshProUGUI HardwareCount;

    public TextMeshProUGUI CorequesCount;
    public TextMeshProUGUI Rotation1Count;
    public TextMeshProUGUI Rotation2Count;

    public TextMeshProUGUI PrimarytableID;

    public InspectionsShipInfo InspectionsInfo;

    public iTextSharp.text.Image LOGO;

    public HeaderFooter headerftrclass;
    public string Summarycomments;
    public string Reportstatus;

    PdfPCell recapdata = new PdfPCell();
    PdfPCell tablecell = new PdfPCell();
    PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
    PdfPCell blankcell = new PdfPCell(new Phrase(" "));
    public string Chaptername = "";
    public int Obsid;
    public List<int> ObservationIds;
    List<String> MediaFiles = new List<string>();
    List<String> Files = new List<string>();
    //public Image uiImage;

    public Camera camera;

    public GameObject PiechartsOrignal; // The GameObject you want to reparent
    public GameObject PiechartsRisk;
    public GameObject PiechartsSireCat;
    public GameObject PiechartsSireQuest;

    public Canvas targetCanvas;
    public GameObject targetCanvasOn;

    [SerializeField]
    private RenderTextureFormat renderTextureFormat;

    private RenderTexture renderTexture;
    private int downResFactor = 1;

    Document document;
    PdfWriter writer;

    string suffixforphotos;
    PdfPTable tablestandardphotos;
    List<String> StandardPhotoHdrs;

    iTextSharp.text.Image ImageForPieCharts;

    Texture2D piechart1texture;
    GameObject copiedObject;
    string destination1;

    public GameObject errorpopup;

    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;
    public bool riskanalysis = true; //TO CHECK IF THIS CHECKLIST QUESTIONS HAVE BEEN MARKED WITH RISK
    public bool onlyforexport;

    public void GenerateFile()
    {
       table_LoginConfig mLocationDb = new table_LoginConfig();
        using var connection = mLocationDb.getConnection();
        mLocationDb.getLatestID();

        if (mLocationDb.tokenbalance<-10|| mLocationDb.tokenbalance == 0)
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Sorry unable to generate a report as you do not have any credits remaining.";
            time = 4.0f;
            InspectionsInfo.emailsender.toEmail = mLocationDb.useremail.ToString(); // Replace with the recipient's email
            InspectionsInfo.emailsender.subject = "InspectNAU APP Low Credits Remaining Notification!";
            //string url = "https://inspectnau.orionmarineconcepts.com/loginpage.aspx";

            InspectionsInfo.emailsender.body = "Hi " + mLocationDb.name.ToString() + "." + Environment.NewLine + Environment.NewLine + "You do not have any credits remaining. " + Environment.NewLine + "We recommend you to always maintain sufficient credits to be able to publish inspections, especially since this requires internet connection, you can manage your credits online by going to; inspectnau.orionmarineconcepts.com/loginpage.aspx (use same login credentials as the mobile APP)." + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";

            InspectionsInfo.emailsender.OnSendEmailButtonClicked();
            connection.Close();
        }

        else if (mLocationDb.tokenbalance > 0 && mLocationDb.tokenbalance <= 2 && !mLocationDb.useremail.Trim().Contains("administrator"))
            {
            InspectionsInfo.emailsender.toEmail = mLocationDb.useremail.ToString(); // Replace with the recipient's email
            InspectionsInfo.emailsender.subject = "InspectNAU APP Low Credits Remaining Notification!";
            //string url = "https://inspectnau.orionmarineconcepts.com/loginpage.aspx";
            
            InspectionsInfo.emailsender.body = "Hi " + mLocationDb.name.ToString() + "." + Environment.NewLine + Environment.NewLine + "You have only '" + mLocationDb.tokenbalance.ToString() + "' credits remaining. " + Environment.NewLine + "We recommend you to always maintain sufficient credits to be able to publish inspections, especially since this requires internet connection, you can manage your credits online by going to; inspectnau.orionmarineconcepts.com/loginpage.aspx (use same login credentials as the mobile APP)." + Environment.NewLine + Environment.NewLine + " Thank you & Best Regards" + Environment.NewLine + "InspectNAU Administrator." + Environment.NewLine + "Support Email; orionapps@orionmarineconcepts.com.";

           InspectionsInfo.emailsender.OnSendEmailButtonClicked();
            connection.Close();

            GenerateFileContinue();
        }
        
        else
        {
            connection.Close();
            GenerateFileContinue();
        }
        
    }
    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        PopUpMsg.color = Color.black;
    }

    public void GenerateFileContinue() //THIS IS THE FUNCTION CALLED ON CLICKING REPORT BUTTON FROM INSP PREVIEW FOR GENERATING PDF REPORT...
    {
      
        //DataTable dt = CreateRandomDataTable(200);
        //DataTable dt1 = CreateRandomDataTable1(10);

        string folderPath = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString()+ "/General";

        // Check if the directory exists
        if (Directory.Exists(folderPath))
        {
            // Get the list of directories in the folder
            string[] directories = Directory.GetFiles(folderPath);

            // Print the list of directories
            //foreach (string directory in directories)
            //{
            //    Debug.Log("Directory: " + directory);
            //}
        }
        else
        {
            Debug.LogWarning("Directory does not exist: " + folderPath);
        }

        path = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf";

        if (File.Exists(path))
            File.Delete(path);

        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            document = new Document(PageSize.A4, 20, 20, 60, 60);

            writer = PdfWriter.GetInstance(document, fileStream);
            writer.SetFullCompression(); // Apply full PDF compression
            writer.CompressionLevel = PdfStream.BEST_COMPRESSION; // Highest compression level for PDF

            HeaderFooter headerFooter = new HeaderFooter();

            headerFooter.VesselName = Shipsname.text.ToString();
            headerFooter.VesselImo = ShipImo.text.ToString();
            headerFooter.InspectionType = InspectType.text.ToString() + " REPORT";
            headerFooter.Inspectedby = Inspectorname.text.ToString();
            headerFooter.Inspectiondate = InspectDatFmTo.text.ToString();
            headerFooter.logo = Logo1;
            headerFooter.Companylogo = CompanyLogo;
            headerFooter.AlternateToCompanylogo = alternateToCompanylogo;

            writer.PageEvent = headerFooter; //CALLING THE HEADERFOOTER CLASS FOR PAGE NUMBER.

            document.Open();

            Obsid = 0;
            document.Add(new Paragraph(50, "\u00a0"));
            BaseColor customColor = new BaseColor(9, 46, 72); // A shade of purple
            var Reportheadingfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, iTextSharp.text.Font.NORMAL, customColor);
            var headingfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, iTextSharp.text.Font.UNDERLINE);
            var DraftFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
            var subheadingfont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.NORMAL);
            var subheadingfont = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            var inspectiondetailheads = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL);
            var smallinfotext = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL);
            var signaturehdgfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE);

            // 1. Vessel Image path and top cover photo

            // Set logo position and size

            cover = Coverpagetop.gameObject.GetComponent<RawImage>();

            Texture2D Coverheader = (Texture2D)cover.texture;
            //iTextSharp.text.Jpeg coverpic = new Jpeg(Coverheader.EncodeToJPG());

            iTextSharp.text.Image coverpic = ConvertTextureToPdfImageCaverPage(Coverheader);

            coverpic.SetAbsolutePosition(document.LeftMargin -20, document.Top - 60); // Adjust as needed
            coverpic.ScaleToFit(600, 575); // Resize the logo

            // Add the logo to the page
            // PdfContentByte canvas1 = writer.DirectContent;
            //canvas1.AddImage(coverpic);
            document.Add(coverpic);
            iTextSharp.text.Image coverLogo = null;
            if (CompanyLogo.texture != null)
            {
                Texture2D CoverPageLogo = (Texture2D)CompanyLogo.texture;
                coverLogo = ConvertTextureToPdfImageCaverPage(CoverPageLogo);

            }
            else
            {
                Texture2D CoverPageLogo = (Texture2D)Logo1.texture;
                coverLogo = ConvertTextureToPdfImageCaverPage(CoverPageLogo);

            }
            coverLogo.SetAbsolutePosition(document.LeftMargin + 50, document.Top - 110); // Adjust as needed
            coverLogo.ScaleToFit(50, 50); // Resize the logo

            document.Add(coverLogo);

            //var shipImagePath = "/Users/mohitsabharwal/Library/Application Support/Launchfort Technologies/Orion Inspections App/Synchronisation/Chemical_Tanker.jpg";
            var shipImagePath = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/General" + "/Ships_Photo_Image.jpg";


            Paragraph InspectionReport = new Paragraph("VESSEL " + InspectType.text.ToString() + " REPORT", Reportheadingfont);
            InspectionReport.IndentationLeft = 57f;  // Left margin
            InspectionReport.SpacingBefore = 60;
            InspectionReport.Alignment = Element.ALIGN_LEFT;
            document.Add(InspectionReport);

            Paragraph Vesselname = new Paragraph(Shipsname.text.ToString().Trim() + " (" + ShipImo.text.ToString() + ")", subheadingfont1);
            Vesselname.SpacingAfter = 5;
            Vesselname.IndentationLeft = 57f;  // Left margin
            Vesselname.SpacingAfter = 20f;
            Vesselname.Alignment = Element.ALIGN_LEFT;
            document.Add(Vesselname);

            //Paragraph VesselIMO = new Paragraph(ShipImo.text.ToString(), subheadingfont1);
            //VesselIMO.SpacingAfter = 10;
            //VesselIMO.Alignment = Element.ALIGN_LEFT;
            //document.Add(VesselIMO);

            if (File.Exists(shipImagePath))
            {
                Debug.Log(Application.persistentDataPath);
                //var shipImage = iTextSharp.text.Image.GetInstance(shipImagePath);
                Texture2D Shiptexture = NativeGallery.LoadImageAtPath(shipImagePath, 1024, false);

                iTextSharp.text.Image shipImage = ConvertTextureToPdfImage(Shiptexture,10);

                shipImage.ScaleToFit(250f, 250f);
                shipImage.Alignment = Element.ALIGN_CENTER;
                document.Add(shipImage);

                Debug.Log("I DID MY JOB REST IS YOUR PROBLEM..");
            }

            else
            {
                PdfPTable TableVslImage = new PdfPTable(1);
                PdfPCell VesselImage = new PdfPCell { PaddingLeft = 15, PaddingTop = 15, PaddingBottom = 15, PaddingRight = 15 };
                VesselImage.AddElement(new Chunk("NO VESSEL IMAGE UPLOADED", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED)));
                TableVslImage.AddCell(VesselImage);
                document.Add(TableVslImage);
            }

            // 2. Vessel Name INspection details etc.
            //DateTime reportDate = DateTime.Now;
            //Paragraph dateParagraph = new Paragraph($"Report Date: {reportDate.ToShortDateString()}");

            // 2. INspection details etc.


            Paragraph headinginspdetails = new Paragraph("Inspection Details", headingfont);
            headinginspdetails.SpacingBefore = 20;
            headinginspdetails.SpacingAfter = 20;
            headinginspdetails.Alignment = Element.ALIGN_LEFT;
            headinginspdetails.IndentationLeft = 57f;
            document.Add(headinginspdetails);


            PdfPTable Tableinspdetails = new PdfPTable(1);

            Phrase inspectiondetails = new Phrase();
            Chunk inspectionname = new Chunk("Inspection: ", inspectiondetailheads);
            inspectiondetails.Add(inspectionname);
            Chunk inspectiondetail = new Chunk(InspectType.text.ToString(), smallinfotext);
            inspectiondetails.Add(inspectiondetail);
            tablecell = new PdfPCell(inspectiondetails);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase inspectordetails = new Phrase();
            Chunk inspectornamehead = new Chunk("Inspected by: ", inspectiondetailheads);
            inspectordetails.Add(inspectornamehead);
            Chunk inspectorname = new Chunk(Inspectorname.text.ToString(), smallinfotext);
            inspectordetails.Add(inspectorname);

            tablecell = new PdfPCell(inspectordetails);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase inspectorqualification = new Phrase();
            Chunk inspectorqualhead = new Chunk("Inspector's Qualification: ", inspectiondetailheads);
            inspectorqualification.Add(inspectorqualhead);
            Chunk inspectorqual = new Chunk(InspectorsQualifications.text.ToString(), smallinfotext);
            inspectorqualification.Add(inspectorqual);

            tablecell = new PdfPCell(inspectorqualification);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);


            Phrase inspectingCompanydetails = new Phrase();
            Chunk inspectingCompanyHead = new Chunk("Inspecting Company: ", inspectiondetailheads);
            inspectingCompanydetails.Add(inspectingCompanyHead);
            Chunk inspectingCompany = new Chunk(InspectingCompanysName.text.ToString(), smallinfotext);
            inspectingCompanydetails.Add(inspectingCompany);

            tablecell = new PdfPCell(inspectingCompanydetails);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase inspectiondate = new Phrase();
            Chunk inspectdatetimehd = new Chunk("Inspection Date and Time: ", inspectiondetailheads);
            inspectiondate.Add(inspectdatetimehd);
            Chunk inspectdatetimedets = new Chunk(InspectDatFmTo.text.ToString(), smallinfotext);
            inspectiondate.Add(inspectdatetimedets);
            tablecell = new PdfPCell(inspectiondate);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase Master = new Phrase();
            Chunk Masterhd = new Chunk("Master: ", inspectiondetailheads);
            Master.Add(Masterhd);
            Chunk Masternamedt = new Chunk(MasterName.text.ToString(), smallinfotext);
            Master.Add(Masternamedt);
            tablecell = new PdfPCell(Master);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase Cheng = new Phrase();
            Chunk Chenghd = new Chunk("Chief Engineer: ", inspectiondetailheads);
            Cheng.Add(Chenghd);
            Chunk Chengdt = new Chunk(ChiefEng.text.ToString(), smallinfotext);
            Cheng.Add(Chengdt);
            tablecell = new PdfPCell(Cheng);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase Choffr = new Phrase();
            Chunk Choffrhd = new Chunk("Chief Officer: ", inspectiondetailheads);
            Choffr.Add(Choffrhd);
            Chunk Chofrdt = new Chunk(ChiefOffr.text.ToString(), smallinfotext);
            Choffr.Add(Chofrdt);
            tablecell = new PdfPCell(Choffr);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);


            Phrase secndengr = new Phrase();
            Chunk secenghd = new Chunk("Second Engineer: ", inspectiondetailheads);
            secndengr.Add(secenghd);
            Chunk secengrdt = new Chunk(SecondEngr.text.ToString(), smallinfotext);
            secndengr.Add(secengrdt);
            tablecell = new PdfPCell(secndengr);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase meetings = new Phrase();
            Chunk mtghd = new Chunk("Opening and closing meetings: ", inspectiondetailheads);
            meetings.Add(mtghd);
            tablecell = new PdfPCell(meetings);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            Phrase meetingdt = new Phrase();
            Chunk mtgdt = new Chunk(Openingmeeting.text.ToString(), smallinfotext);
            meetingdt.Add(mtgdt);
            tablecell = new PdfPCell(meetingdt);// { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 2;

            Tableinspdetails.AddCell(tablecell);

            document.Add(Tableinspdetails);

            //SIGNATURES MASTER AND INSPECTOR

            PdfPTable TableSign = new PdfPTable(3);

            blankcell.Colspan = 4;
            blankcell.FixedHeight = 70f; //SPACE BETWEEN THE VESSEL IMAGE AND THE SIGNATURES AREA
            blankcell.Border = Rectangle.NO_BORDER;
            TableSign.AddCell(blankcell);

            TableSign.HorizontalAlignment = Element.ALIGN_RIGHT;

            
            TableSign.TotalWidth = 200f;
            TableSign.WidthPercentage = (100.0f);

            MasterSign = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/General" + "/Master_Sign_ForSaving.jpg";
            InspSign = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/General" + "/Inspect_Sign_ForSaving.jpg";

            if (File.Exists(MasterSign))
            {
                //var mastersign = iTextSharp.text.Image.GetInstance(MasterSign);
                    
                 Texture2D Mastersigntext = NativeGallery.LoadImageAtPath(MasterSign, 1024, false);
                 iTextSharp.text.Image mastersign = ConvertTextureToPdfImage(Mastersigntext,20);
                 //mastersign.ScaleToFit(30, 30);

                mastersign.Alignment = Element.ALIGN_LEFT;
                //mastersign.ScaleAbsolute(50, 50);
                mastersign.ScaleToFit(80, 80);

                tablecell = new PdfPCell(new Phrase(new Phrase("")));

                PdfPCell cell_1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                cell_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_1.AddElement(new Chunk("Master's Signatures: ", signaturehdgfont));
                cell_1.Border = Rectangle.NO_BORDER;
                cell_1.Colspan = 2;
                TableSign.AddCell(cell_1);

                PdfPCell cell_2 = new PdfPCell(mastersign) { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                cell_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_2.Border = Rectangle.NO_BORDER;
                //cell_2.AddElement(mastersign);
                cell_2.Colspan = 1;
                TableSign.AddCell(cell_2);

            }
            else
            {
                PdfPCell cell_1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 5, PaddingRight = 0 };
                cell_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_1.AddElement(new Chunk("Master's Signatures: ", signaturehdgfont));
                cell_1.Border = Rectangle.NO_BORDER;
                cell_1.Colspan = 2;
                TableSign.AddCell(cell_1);

                PdfPCell cell_2 = new PdfPCell { PaddingLeft = 0, PaddingTop = 0, PaddingBottom = 5, PaddingRight = 0 };
                cell_2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell_2.VerticalAlignment = Element.ALIGN_LEFT;
                cell_2.Border = Rectangle.NO_BORDER;
                cell_2.AddElement(new Chunk("Pending.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED)));
                cell_2.Colspan =1;
                TableSign.AddCell(cell_2);

            }

            if (File.Exists(InspSign))
            {
                Texture2D Inspsigntext = NativeGallery.LoadImageAtPath(InspSign, 1024, false);
                //var inspsign = iTextSharp.text.Image.GetInstance(InspSign);
                iTextSharp.text.Image inspsign = ConvertTextureToPdfImage(Inspsigntext,20);
                inspsign.ScaleToFit(80, 80);

                tablecell = new PdfPCell(new Phrase(new Phrase("")));

                //var InsSign = iTextSharp.text.Image.GetInstance(inspsign);

                PdfPCell cell_1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 5, PaddingBottom = 0, PaddingRight = 0 };
                cell_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_1.AddElement(new Chunk("Inspector's Signatures: ", signaturehdgfont));
                cell_1.Border = Rectangle.NO_BORDER;
                cell_1.Colspan = 2;
                TableSign.AddCell(cell_1);

                PdfPCell cell_2 = new PdfPCell(inspsign) { PaddingLeft = 0, PaddingTop = 5, PaddingBottom = 0, PaddingRight = 0 };
                cell_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell_2.VerticalAlignment = Element.ALIGN_LEFT;
                cell_2.Border = Rectangle.NO_BORDER;
                cell_2.Colspan = 1;
                TableSign.AddCell(cell_2);

               

            }
            else
            {
                PdfPCell cell_1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 5, PaddingBottom = 0, PaddingRight = 0 };
                cell_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell_1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_1.AddElement(new Chunk("Inspector's Signatures: ", signaturehdgfont));
                cell_1.Border = Rectangle.NO_BORDER;
                cell_1.Colspan = 2;
                TableSign.AddCell(cell_1);

                PdfPCell cell_2 = new PdfPCell { PaddingLeft = 0, PaddingTop = 5, PaddingBottom = 0, PaddingRight = 0 };
                cell_2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell_2.VerticalAlignment = Element.ALIGN_TOP;
                cell_2.Border = Rectangle.NO_BORDER;
                cell_2.AddElement(new Chunk("Pending.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED)));
                cell_2.Colspan = 1;
                TableSign.AddCell(cell_2);

            }

            // Calculate the X and Y position for the table
            float xPos = document.PageSize.Width - document.RightMargin - 200;
            float yPos = document.BottomMargin + 10f;

            // Write the table to the specified position
            TableSign.WriteSelectedRows(0, -1, xPos, yPos + TableSign.TotalHeight, writer.DirectContent);

            //document.Add(TableSign);

            //3. 2nd Page Summary and Overview details
            document.NewPage();


            Paragraph heading = new Paragraph("Summary", headingfont);
            heading.SpacingBefore = 20;

            heading.Alignment = Element.ALIGN_LEFT;
            document.Add(heading);

            Summarycomments = "";
            Reportstatus = "";
            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));
            Summarycomments = mlocationdb1.SummaryComments.ToString();
            Reportstatus = mlocationdb1.reportstatus.ToString().Trim();

            headerFooter.reportstaus = Reportstatus;

            var summaryParagraph = new Paragraph(Summarycomments);

            //var summaryParagraph = new Paragraph("Ship Report Summary\n\nThe WMO Weather observation report from ship message is intended for ships that have been recruited by national meteorological services to undertake weather observations at sea. This practice aligns with the provisions of SOLAS chapter V, regulation 5, and the World Meteorological Organization?s Voluntary Observing Ship (VOS) Scheme1. These ships play a crucial role in collecting valuable weather data while navigating the open waters.Similar to the synoptic surface observation code for land stations, the ship surface observation report is transmitted at 6 - hourly intervals at the standard hours of observation: 0000, 0600, 1200, and 1800 UTC.The report provides essential information about weather conditions, including temperature, wind speed, humidity, and cloud cover.Here?s the symbolic form of the message for the synoptic weather report from a ship station:");
            summaryParagraph.SpacingBefore = 20;
            summaryParagraph.SpacingAfter = 20;
            document.Add(summaryParagraph);

            //// Shifted to 3rd page frm 2nd Page add inspection Overview here........
            document.NewPage();

            Paragraph Answeredstatus = new Paragraph("Answers Recap", headingfont);
            Answeredstatus.SpacingBefore = 10;
            Answeredstatus.SpacingAfter = 10;

            Answeredstatus.Alignment = Element.ALIGN_LEFT;
            document.Add(Answeredstatus);


            PdfPTable tablerecap = new PdfPTable(2);
            tablerecap.WidthPercentage = 50f;
            tablerecap.HorizontalAlignment = Element.ALIGN_CENTER;
            ////// THIS CAN BE USED FOR ANY RAW IMAGE FROM UNITY TO PDF           

            //RawImage icon = LabelAnsweredImg.gameObject.GetComponent<RawImage>();
            //Texture2D LabelAnswered = (Texture2D)icon.texture;
            //iTextSharp.text.Jpeg AnsweredIcon = new Jpeg(LabelAnswered.EncodeToJPG());
            //AnsweredIcon.ScaleToFit(100, 75);

            //PdfPCell imageCell = new PdfPCell(AnsweredIcon);
            //imageCell.Border = Rectangle.NO_BORDER; //REMOVE BORDER FROM A CELL
            //imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //imageCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //imageCell.Rowspan = 6;
            //tablerecap.AddCell(imageCell);

            recapdata = new PdfPCell(new Phrase($"Answered: ", subheadingfont1));
            recapdata.Border = Rectangle.NO_BORDER; //REMOVE BORDER FROM A CELL
            //recapdata.HorizontalAlignment = Element.ALIGN_RIGHT; 
            tablerecap.AddCell(recapdata);

            recapdata = new PdfPCell(new Phrase(TotalAnswered.text.ToString(), subheadingfont1));
            //cell.Colspan = 2;
            recapdata.Border = Rectangle.NO_BORDER;

            tablerecap.AddCell(recapdata);

            //////

            recapdata = new PdfPCell(new Phrase(LabelGeneral.text.ToString() + ":", subheadingfont1)) { PaddingLeft = 0, PaddingTop = 10, PaddingBottom = 20, PaddingRight = 0 };
            recapdata.Border = Rectangle.NO_BORDER; //REMOVE BORDER FROM A CELL
            //recapdata.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablerecap.AddCell(recapdata);

            string LabelcountvalueGen = "";
            if (string.IsNullOrEmpty(LabelGeneralCount.text.ToString()))
            {
                LabelcountvalueGen = "-";
            }

            else
            {
                LabelcountvalueGen = LabelGeneralCount.text.ToString();

            }

            recapdata = new PdfPCell(new Phrase(LabelcountvalueGen, subheadingfont1)) { PaddingLeft = 0, PaddingTop = 10, PaddingBottom = 20, PaddingRight = 0 };
            //cell.Colspan = 2;
            recapdata.Border = Rectangle.NO_BORDER;
            tablerecap.AddCell(recapdata);

            document.Add(tablerecap);




            PdfPTable TablePieChartsheader = new PdfPTable(3);
            TablePieChartsheader.HorizontalAlignment = Element.ALIGN_RIGHT;
            TablePieChartsheader.WidthPercentage = (100.0f);

            PdfPCell cell_pieheader = new PdfPCell { PaddingLeft = 0, PaddingTop = 10, PaddingBottom = 30, PaddingRight = 0 };
            cell_pieheader.HorizontalAlignment = Element.ALIGN_MIDDLE;
            cell_pieheader.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_pieheader.AddElement(new Chunk("Observations and Risk Analysis (if applicable)", headingfont));
            cell_pieheader.Border = Rectangle.NO_BORDER;
            cell_pieheader.Colspan = 6;
            TablePieChartsheader.AddCell(cell_pieheader);

            document.Add(TablePieChartsheader);

            camera.enabled = true;
            targetCanvasOn.SetActive(true);

            copiedObject = Instantiate(PiechartsOrignal, PiechartsOrignal.transform.position, PiechartsOrignal.transform.rotation);

            // Optionally, set the name of the copied object to distinguish it
            copiedObject.name = PiechartsOrignal.name + "_Copy";

            copiedObject.transform.SetParent(targetCanvas.transform, false);

            copiedObject.transform.localPosition = new Vector3(-128, 0, 0);
            // Optionally, reset the local position, rotation, and scale if needed
            //copiedObject.transform.localPosition = Vector3.zero;

            RectTransform uitransform = copiedObject.GetComponent<RectTransform>();

            uitransform.anchorMin = new Vector2(0.5f, 0.5f);
            uitransform.anchorMax = new Vector2(0.5f, 0.5f);
            uitransform.pivot = new Vector2(0.5f, 0.5f);

            //copiedObject.transform.localRotation = Quaternion.identity;
            copiedObject.transform.localScale = Vector3.one;

            renderTexture = camera.targetTexture;

            RenderTexture.active = renderTexture;

            camera.Render();


            piechart1texture = new Texture2D(renderTexture.width, renderTexture.height);

            piechart1texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            piechart1texture.Apply();

            byte[] itemBGBytes = piechart1texture.EncodeToJPG(); //FOR COMPRESSION INPUT A NUMBER HERE .EncodeToJPG(10)...

            iTextSharp.text.Image pichart = ConvertTextureToPdfImage(piechart1texture,50);

            pichart.Alignment = Element.ALIGN_MIDDLE;
            pichart.ScaleAbsolute(120, 120);
            pichart.ScaleToFit(100, 100);

            PdfPTable TablePieCharts = new PdfPTable(6);
            TablePieCharts.HorizontalAlignment = Element.ALIGN_RIGHT;
            TablePieCharts.WidthPercentage = (100.0f);

            PdfPCell piecell1 = new PdfPCell(pichart) { PaddingLeft = 0, PaddingTop = 20, PaddingBottom = 0, PaddingRight = 0 };
            piecell1.Border = PdfPCell.NO_BORDER; // Optional: Remove border
            piecell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            piecell1.VerticalAlignment = Element.ALIGN_TOP;
            piecell1.Border = Rectangle.NO_BORDER;
            piecell1.Colspan = 2;
            TablePieCharts.AddCell(piecell1);

           Destroy(copiedObject);

            string Labelcountvalue1 = "";
            string Labelcountvalue2 = "";
            string Labelcountvalue3 = "";
            string Labelcountvalue4 = "";
            string Labelcountvalue5 = "";

            if (string.IsNullOrEmpty(Label1Count.text.ToString()))
            {
                Labelcountvalue1 = "-";
            }

            else
            {
                Labelcountvalue1 = Label1Count.text.ToString();

            }

            //recapdata = new PdfPCell(new Phrase(Label1.text.ToString() + ":"+ Labelcountvalue1, subheadingfont1));
            //recapdata.Colspan = 1;
            //recapdata.Border = Rectangle.NO_BORDER;
            //TablePieCharts.AddCell(recapdata);

            if (string.IsNullOrEmpty(Label2Count.text.ToString()))
            {
                Labelcountvalue2 = "-";
            }

            else
            {
                Labelcountvalue2 = Label2Count.text.ToString();

            }

            if (string.IsNullOrEmpty(Label3Count.text.ToString()))
            {
                Labelcountvalue3 = "-";
            }

            else
            {
                Labelcountvalue3 = Label3Count.text.ToString();

            }

            if (string.IsNullOrEmpty(Label4Count.text.ToString()))
            {
                Labelcountvalue4 = "-";
            }

            else
            {
                Labelcountvalue4 = Label4Count.text.ToString();

            }

            if (string.IsNullOrEmpty(Label5Count.text.ToString()))
            {
                Labelcountvalue5 = "-";
            }

            else
            {
                Labelcountvalue5 = Label5Count.text.ToString();

            }

            //recapdata = new PdfPCell();
            //recapdata.Colspan = 1;
            //recapdata.Border = Rectangle.NO_BORDER; 

            recapdata = new PdfPCell { PaddingLeft = 0, PaddingTop = 40, PaddingBottom = 0, PaddingRight = 0 };
            recapdata.AddElement(new Chunk(Label1.text.ToString() + " " + Labelcountvalue1, subheadingfont));
            recapdata.AddElement(new Chunk(Label2.text.ToString() + " " + Labelcountvalue2, subheadingfont));
            recapdata.AddElement(new Chunk(Label3.text.ToString() + " " + Labelcountvalue3, subheadingfont));
            recapdata.AddElement(new Chunk(Label4.text.ToString() + " " + Labelcountvalue4, subheadingfont));
            recapdata.AddElement(new Chunk(Label5.text.ToString() + " " + Labelcountvalue5, subheadingfont));
            recapdata.Colspan = 1;
            recapdata.Border = Rectangle.NO_BORDER;
            TablePieCharts.AddCell(recapdata);

            document.Add(TablePieCharts);

            //TEST IN IOS PDF STARTS HERE

            //CHECK IF QUESTIONS HAVE RISK ASSIGNED TO THEM OR NOT
            

            if (LowRiskCount.text.Trim() == "-"&& MedRiskCount.text.Trim() == "-"&& HighRiskCount.text.Trim() == "-")
            {
                riskanalysis = false;
                recapdata = new PdfPCell { PaddingLeft = 0, PaddingTop = 40, PaddingBottom = 0, PaddingRight = 0 };
                recapdata.AddElement(new Chunk(""));
                recapdata.Border = Rectangle.NO_BORDER;
                recapdata.Colspan = 3;
                TablePieCharts.AddCell(recapdata);

            }

            else //SIMPLY ADDED A BLANK CELL WITHOUT THE RISK CHART
            {
                
                riskanalysis = true;
                GameObject copiedObject1 = Instantiate(PiechartsRisk, PiechartsRisk.transform.position, PiechartsRisk.transform.rotation);

                // Optionally, set the name of the copied object to distinguish it
                copiedObject1.name = PiechartsRisk.name + "_Copy";

                copiedObject1.transform.SetParent(targetCanvas.transform, false);

                copiedObject1.transform.localPosition = new Vector3(-128, 0, 0);
                // Optionally, reset the local position, rotation, and scale if needed
                //copiedObject.transform.localPosition = Vector3.zero;

                RectTransform uitransform1 = copiedObject1.GetComponent<RectTransform>();

                uitransform.anchorMin = new Vector2(0.5f, 0.5f);
                uitransform.anchorMax = new Vector2(0.5f, 0.5f);
                uitransform.pivot = new Vector2(0.5f, 0.5f);

                //copiedObject.transform.localRotation = Quaternion.identity;
                copiedObject1.transform.localScale = Vector3.one;

                renderTexture = camera.targetTexture;

                RenderTexture.active = renderTexture;

                camera.Render();

                Texture2D texture1 = new Texture2D(
                    renderTexture.width,
                    renderTexture.height);

                texture1.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture1.Apply();

                byte[] piechartsrisk = texture1.EncodeToJPG();

                //File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "radialImage.jpg"), piecharts);

                //iTextSharp.text.Jpeg piechartprintrisk = new Jpeg(texture1.EncodeToJPG());
                //var piechartprintrisk1 = iTextSharp.text.Image.GetInstance(piechartprintrisk);

                iTextSharp.text.Image pichart2 = ConvertTextureToPdfImage(texture1, 50);

                pichart2.Alignment = Element.ALIGN_RIGHT;
                pichart2.ScaleAbsolute(120, 120);
                pichart2.ScaleToFit(100, 100);
                //document.Add(pichart2);

                PdfPCell riskpiecell = new PdfPCell(pichart2) { PaddingLeft = 0, PaddingTop = 18, PaddingBottom = 0, PaddingRight = 0 };
                riskpiecell.HorizontalAlignment = Element.ALIGN_RIGHT;
                riskpiecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                riskpiecell.Border = Rectangle.NO_BORDER;
                riskpiecell.Colspan = 2;
                TablePieCharts.AddCell(riskpiecell);

                Destroy(copiedObject1);

                string LabelcountLowRisk = "";
                string LabelcountMedRisk = "";
                string LabelcountHighRisk = "";

                if (string.IsNullOrEmpty(LowRiskCount.text.ToString()))
                {
                    LabelcountLowRisk = "-";
                }

                else
                {
                    LabelcountLowRisk = LowRiskCount.text.ToString();

                }

                if (string.IsNullOrEmpty(MedRiskCount.text.ToString()))
                {
                    LabelcountMedRisk = "-";
                }

                else
                {
                    LabelcountMedRisk = MedRiskCount.text.ToString();

                }

                if (string.IsNullOrEmpty(HighRiskCount.text.ToString()))
                {
                    LabelcountHighRisk = "-";
                }

                else
                {
                    LabelcountHighRisk = HighRiskCount.text.ToString();

                }

                recapdata = new PdfPCell { PaddingLeft = 0, PaddingTop = 40, PaddingBottom = 0, PaddingRight = 0 };
                recapdata.AddElement(new Chunk("Low Risk: " + " " + LabelcountLowRisk, subheadingfont));
                recapdata.AddElement(new Chunk("Med Risk: " + " " + LabelcountMedRisk, subheadingfont));
                recapdata.AddElement(new Chunk("High Risk: " + " " + LabelcountHighRisk, subheadingfont));
                recapdata.Border = Rectangle.NO_BORDER;
                recapdata.Colspan = 1;
                TablePieCharts.AddCell(recapdata);
            }
            document.Add(TablePieCharts);


            //// SIRE 2.0 ANALYSIS PIE CHARTS HERE

            if (mlocationdb1.inspectiontype.ToLower().Trim().Replace(" ", "").Contains("(sire2.0)"))
            {
                PdfPTable TablePieCharts1 = new PdfPTable(6);
                TablePieCharts1.HorizontalAlignment = Element.ALIGN_RIGHT;
                TablePieCharts1.WidthPercentage = (100.0f);

                GameObject copiedObject2 = Instantiate(PiechartsSireCat, PiechartsSireCat.transform.position, PiechartsSireCat.transform.rotation);

                // Optionally, set the name of the copied object to distinguish it
                copiedObject2.name = PiechartsSireCat.name + "_Copy";

                copiedObject2.transform.SetParent(targetCanvas.transform, false);

                copiedObject2.transform.localPosition = new Vector3(0, 0, 0);
                // Optionally, reset the local position, rotation, and scale if needed
                //copiedObject.transform.localPosition = Vector3.zero;

                RectTransform uitransform2 = copiedObject2.GetComponent<RectTransform>();

                uitransform.anchorMin = new Vector2(0.5f, 0.5f);
                uitransform.anchorMax = new Vector2(0.5f, 0.5f);
                uitransform.pivot = new Vector2(0.5f, 0.5f);

                //copiedObject.transform.localRotation = Quaternion.identity;
                copiedObject2.transform.localScale = Vector3.one;

                renderTexture = camera.targetTexture;

                RenderTexture.active = renderTexture;

                camera.Render();

                Texture2D texture2 = new Texture2D(
                    renderTexture.width,
                    renderTexture.height);

                texture2.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture2.Apply();

                byte[] sirecatg = texture2.EncodeToJPG();

                //File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "radialImage.jpg"), piecharts);

                //iTextSharp.text.Jpeg sirecatgprint = new Jpeg(texture2.EncodeToJPG());
                //var sirecatgprint1 = iTextSharp.text.Image.GetInstance(sirecatgprint);

                iTextSharp.text.Image pichart3 = ConvertTextureToPdfImage(texture2,50);

                pichart3.Alignment = Element.ALIGN_RIGHT;
                pichart3.ScaleAbsolute(120, 120);
                pichart3.ScaleToFit(100, 100);
                //document.Add(piechartprint1);

                PdfPCell cell_pieheader1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 10, PaddingBottom = 30, PaddingRight = 0 };
                cell_pieheader1.HorizontalAlignment = Element.ALIGN_MIDDLE;
                cell_pieheader1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_pieheader1.AddElement(new Chunk("SIRE 2.0 Categories and Questions", headingfont));
                cell_pieheader1.Border = Rectangle.NO_BORDER;
                cell_pieheader1.Colspan = 6;
                TablePieCharts1.AddCell(cell_pieheader1);

                //PdfPCell ObsPie1 = new PdfPCell { PaddingLeft = 0, PaddingTop = 20, PaddingBottom = 0, PaddingRight = 0 };
                //ObsPie1.HorizontalAlignment = Element.ALIGN_RIGHT;
                //ObsPie1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //ObsPie1.AddElement(sirecatgprint1);
                //ObsPie1.Border = Rectangle.NO_BORDER;
                //ObsPie1.Colspan = 3;
                //TablePieCharts1.AddCell(ObsPie1);

                PdfPCell sirecatcell = new PdfPCell (pichart3) { PaddingLeft = 0, PaddingTop = 20, PaddingBottom = 0, PaddingRight = 0 };
                sirecatcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                sirecatcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                sirecatcell.Border = Rectangle.NO_BORDER;
                sirecatcell.Colspan = 2;
                TablePieCharts1.AddCell(sirecatcell);

                Destroy(copiedObject2);

                string LabelcountHuman = "";
                string LabelcountProcess = "";
                string LabelcountHardware = "";

                if (string.IsNullOrEmpty(HumanCount.text.ToString()))
                {
                    LabelcountHuman = "-";
                }

                else
                {
                    LabelcountHuman = HumanCount.text.ToString();

                }

                if (string.IsNullOrEmpty(ProcessCount.text.ToString()))
                {
                    LabelcountProcess = "-";
                }

                else
                {
                    LabelcountProcess = ProcessCount.text.ToString();

                }

                if (string.IsNullOrEmpty(HardwareCount.text.ToString()))
                {
                    LabelcountHardware = "-";
                }

                else
                {
                    LabelcountHardware = HardwareCount.text.ToString();

                }

                recapdata = new PdfPCell { PaddingLeft = 0, PaddingTop = 40, PaddingBottom = 0, PaddingRight = 0 };
                recapdata.AddElement(new Chunk("Human Element: " + " " + LabelcountHuman, subheadingfont));
                recapdata.AddElement(new Chunk("Process Element: " + " " + LabelcountProcess, subheadingfont));
                recapdata.AddElement(new Chunk("Hardware Element: " + " " + LabelcountHardware, subheadingfont));
                recapdata.Border = Rectangle.NO_BORDER;
                recapdata.Colspan = 1;
                TablePieCharts1.AddCell(recapdata);


                // ADDING PIE CHART FOR SIRE CORE and ROTATION QUESTIONS;

                GameObject copiedObject3 = Instantiate(PiechartsSireQuest, PiechartsSireQuest.transform.position, PiechartsSireQuest.transform.rotation);

                // Optionally, set the name of the copied object to distinguish it
                copiedObject3.name = PiechartsSireQuest.name + "_Copy";

                copiedObject3.transform.SetParent(targetCanvas.transform, false);

                copiedObject3.transform.localPosition = new Vector3(-128, 0, 0);
                // Optionally, reset the local position, rotation, and scale if needed
                //copiedObject.transform.localPosition = Vector3.zero;

                RectTransform uitransform3 = copiedObject3.GetComponent<RectTransform>();

                uitransform.anchorMin = new Vector2(0.5f, 0.5f);
                uitransform.anchorMax = new Vector2(0.5f, 0.5f);
                uitransform.pivot = new Vector2(0.5f, 0.5f);

                //copiedObject.transform.localRotation = Quaternion.identity;
                copiedObject3.transform.localScale = Vector3.one;

                renderTexture = camera.targetTexture;

                RenderTexture.active = renderTexture;

                camera.Render();

                Texture2D texture3 = new Texture2D(
                    renderTexture.width,
                    renderTexture.height);

                texture3.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture3.Apply();

                byte[] sirequest = texture3.EncodeToJPG();

                //File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "radialImage.jpg"), piecharts);

                //iTextSharp.text.Jpeg sirequestprint = new Jpeg(texture3.EncodeToJPG());
                //var sirequestprint1 = iTextSharp.text.Image.GetInstance(sirequestprint);

                iTextSharp.text.Image pichart4 = ConvertTextureToPdfImage(texture3,50);

                pichart4.Alignment = Element.ALIGN_RIGHT;
                pichart4.ScaleAbsolute(120, 120);
                pichart4.ScaleToFit(100, 100);
                //document.Add(piechartprint1);

                PdfPCell sirequestcell = new PdfPCell(pichart4) { PaddingLeft = 0, PaddingTop = 20, PaddingBottom = 0, PaddingRight = 0 };
                sirequestcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                sirequestcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                sirequestcell.Border = Rectangle.NO_BORDER;
                sirequestcell.Colspan = 2;
                TablePieCharts1.AddCell(sirequestcell);

                Destroy(copiedObject3);

                string LabelcountCore = "";
                string LabelcountRot1 = "";
                string LabelcountRot2 = "";

                if (string.IsNullOrEmpty(CorequesCount.text.ToString()))
                {
                    LabelcountCore = "-";
                }

                else
                {
                    LabelcountCore = CorequesCount.text.ToString();

                }

                if (string.IsNullOrEmpty(Rotation1Count.text.ToString()))
                {
                    LabelcountRot1 = "-";
                }

                else
                {
                    LabelcountRot1 = Rotation1Count.text.ToString();

                }

                if (string.IsNullOrEmpty(Rotation2Count.text.ToString()))
                {
                    LabelcountRot2 = "-";
                }

                else
                {
                    LabelcountRot2 = Rotation2Count.text.ToString();

                }

                recapdata = new PdfPCell { PaddingLeft = 0, PaddingTop = 40, PaddingBottom = 0, PaddingRight = 0 };
                recapdata.AddElement(new Chunk("Core Ques.: " + " " + CorequesCount.text.ToString(), subheadingfont));
                recapdata.AddElement(new Chunk("Rotation 1 Ques.: " + " " + Rotation1Count.text.ToString(), subheadingfont));
                recapdata.AddElement(new Chunk("Rotation 2 Ques.: " + " " + Rotation2Count.text.ToString(), subheadingfont));
                recapdata.Border = Rectangle.NO_BORDER;
                recapdata.Colspan = 1;
                TablePieCharts1.AddCell(recapdata);


                document.Add(TablePieCharts1);
            }
            camera.enabled = false;
            Destroy(copiedObject);
            targetCanvasOn.SetActive(false);

            document.NewPage();


            ////////////////////////////////

            //4. 4th page Adding tables from here on

            ////////////////////////////////
            ///

            var ObsPageheading = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);

            Paragraph ObsPageheader = new Paragraph("OBSERVATIONS", ObsPageheading);
            //ObsPageheader.IndentationLeft = 57f;  // Left margin
            //ObsPageheader.SpacingBefore = 60;
            ObsPageheader.Alignment = Element.ALIGN_CENTER;
            document.Add(ObsPageheader);

           table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
            using var connection1 = mlocationDb.getConnection();
            //string query = "SELECT TRIM(Selected_Answer, ' -ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz') AS Answer FROM Inspection_Observations where cast(Inspection_PrimaryDetails_ID as int)= " + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + " GROUP BY Selected_Answer;";

            string query1 = "select b.Template_Section_Ques as 'ChapterName',a.*,b.Obs_Details_1 as 'ChapterDescription' from Inspection_Observations a left join Inspection_Observations " +
                "b on a.Cloud_DB_ParentID = b.Cloud_DB_ID and TRIM(a.Inspection_PrimaryDetails_ID) = TRIM(b.Inspection_PrimaryDetails_ID) " +
                " where TRIM(a.Inspection_PrimaryDetails_ID)= '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' " +
                "and TRIM(a.Observation_Text)!= '' and a.Observation_Text is not null AND substr(a.Selected_Answer, 1, instr(a.Selected_Answer, '-') - 1) NOT IN('999') and TRIM(a.Obs_Details_8) = 'Question' and TRIM(b.Obs_Details_8) = 'Chapter'" +
                "AND substr(a.Selected_Answer, 1, instr(a.Selected_Answer, '-') - 1) IN('1', '2', '6','10','11','12');";

            // If anything marked Poor, NC or as No, etc. i.e.
            // Toggles (Selected_Answer) on the Questions Prefab -- SIRE 2.0, other Insp, Condition insp and Audits - 0_AsExpected_Toggle | 0_No_Deficiency_Toggle | 0_Good_Toggle , 1_Not_As_Expected_Toggle |1_Deficiency_Toggle , 2_Poor_Toggle, 3_NotSeen_Toggle, 4_NA_Toggle, 5_Yes_Toggle, 6_No_Toggle, 7_NotSeen_Toggle, 9_Satisfactory_Toggle, 10_MajorNC_Toggle,11_MinorNC_Toggle, 12_OBS_Toggle, 13_NotSeen_Toggle

            //string query1 = "select b.Template_Section_Ques as 'ChapterName',a.* from Inspection_Observations a INNER join Inspection_Observations b on a.Cloud_DB_ParentID = b.Cloud_DB_ID and TRIM(a.Inspection_PrimaryDetails_ID) = TRIM(b.Inspection_PrimaryDetails_ID) where TRIM(a.Inspection_PrimaryDetails_ID)= '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' and TRIM(a.Observation_Text)!= '' and a.Observation_Text is not null AND TRIM(a.Selected_Answer) NOT like '%999%' and TRIM(b.Obs_Details_8) = 'Chapter' ;";
            using System.Data.IDataReader Obsrecordreader = mlocationDb.getDatabyQuery(query1);
           

            List<String> observations = new List<string>();

            int serialforObs = 0;

            if (((System.Data.Common.DbDataReader)Obsrecordreader).HasRows)
            {
                while (Obsrecordreader.Read())
                {
                    var headingfont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                    var cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);

                    if (Obsrecordreader[0].ToString() != "")
                    {
                        observations.Add(Obsrecordreader[0].ToString().Trim());  //Chapter Name
                        observations.Add(Obsrecordreader[1].ToString().Trim());  //ID - Obs Id
                        observations.Add(Obsrecordreader[3].ToString().Trim());  //ID - Obs CLOUD DB ID WHICH IS CONNECTED TO THE ATTACHMENTS
                        observations.Add(Obsrecordreader[7].ToString().Trim());  // Template_Section_Ques - Question No.
                        observations.Add(Obsrecordreader[8].ToString().Trim());  // Selected_Answer
                        observations.Add(Obsrecordreader[9].ToString().Trim());  // Date
                        observations.Add(Obsrecordreader[10].ToString().Trim()); // Time
                        observations.Add(Obsrecordreader[12].ToString().Trim()); // Observation_Text
                        observations.Add(Obsrecordreader[13].ToString().Trim()); // RiskCategory
                        observations.Add(Obsrecordreader[16].ToString().Trim()); // Obs_Details_1 - The Question
                        observations.Add(Obsrecordreader[20].ToString().Trim()); // Obs_Details_5 - Section
                        observations.Add(Obsrecordreader[26].ToString().Trim()); // ROVIQSEQUENCE - ROVING
                        observations.Add(Obsrecordreader[38].ToString().Trim()); // CHAPTER DESCRIPTION

                        Obsid = int.Parse(Obsrecordreader[3].ToString());

                        PdfPTable tableChapters = new PdfPTable(4);
                        tableChapters.WidthPercentage = 100;
                        //tableChapters.SpacingBefore = 15;
                        tableChapters.SetWidths(new float[] { 1, 3, 1, 1 });
                        tableChapters.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPTable tableObsId = new PdfPTable(4);
                        tableObsId.WidthPercentage = 100;
                        //tableObsId.SpacingBefore = 15;
                        tableObsId.SetWidths(new float[] { 1, 3, 1, 1 });
                        tableObsId.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPTable tableObsanddetails = new PdfPTable(4);
                        tableObsanddetails.WidthPercentage = 100;
                        //tableObsanddetails.SpacingBefore = 15;
                        tableObsanddetails.SetWidths(new float[] { 1, 3, 1, 1 });
                        tableObsanddetails.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPTable tablemediaNfiles = new PdfPTable(4);
                        tablemediaNfiles.WidthPercentage = 100;
                        //tablemediaNfiles.SpacingBefore = 15;
                        tablemediaNfiles.SetWidths(new float[] { 1, 3, 1, 1 });
                        tablemediaNfiles.HorizontalAlignment = Element.ALIGN_CENTER;


                        // CHAPTER + SECTIONS

                        blankcell.Colspan = 4;
                        blankcell.FixedHeight = 10f;
                        blankcell.Border = Rectangle.NO_BORDER;
                        tableChapters.AddCell(blankcell);

                        if (Chaptername != Obsrecordreader[0].ToString().Trim())
                        {

                            //tablecell = new PdfPCell(new Phrase(Obsrecordreader[0].ToString().Trim() +" " + Obsrecordreader[38].ToString().Trim() + " | " + "Section: " + Obsrecordreader[20].ToString().Trim(), headingfont1));//"Chapter: " + SECTION NAME
                            tablecell = new PdfPCell(new Phrase(Obsrecordreader[0].ToString().Trim() + " " + Obsrecordreader[38].ToString().Trim(), headingfont1));//"Chapter: " + SECTION NAME COMMENTED

                            tablecell.Colspan = 4;
                            tablecell.Border = Rectangle.NO_BORDER;
                            tablecell.HorizontalAlignment = Element.ALIGN_LEFT;
                            tablecell.CellEvent = new RoundedBorderChapters();
                            tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;

                            tableChapters.AddCell(tablecell);

                            blankcell.Colspan = 1;
                            blankcell.Border = Rectangle.NO_BORDER;
                            tableChapters.AddCell(blankcell);

                            //tableObservations.AddCell(tablecell);
                            blankcell.Colspan = 4;
                            blankcell.FixedHeight = 10f;
                            blankcell.Border = Rectangle.NO_BORDER;
                            tableChapters.AddCell(blankcell);
                            document.Add(tableChapters);
                        }

                        blankcell.Colspan = 4;
                        blankcell.Border = Rectangle.NO_BORDER;
                        tableObsId.AddCell(blankcell);

                        // OBSID + RISK
                        serialforObs += 1; //Obsrecordreader[1].ToString().Trim()
                        tablecell = new PdfPCell(new Phrase("Obs ID: " + serialforObs + " | " + "Question No.: " + Obsrecordreader[7].ToString().Trim(), cellcontentfonts)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };
                        tablecell.Border = Rectangle.NO_BORDER;
                        tablecell.Colspan = 2;
                        tablecell.CellEvent = new RoundedBorderObservations();
                        tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                        tableObsId.AddCell(tablecell);

                        //blankcell.Colspan = 2;
                        //blankcell.Border = Rectangle.NO_BORDER;

                        //tableObservations.AddCell(blankcell);

                        if (Obsrecordreader[13].ToString().Trim() == "High")
                        {
                            cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
                        }

                        else if (Obsrecordreader[13].ToString().Trim() == "Medium")
                        {
                            cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.ORANGE);
                        }

                        else if (Obsrecordreader[13].ToString().Trim() == "Low")
                        {
                            cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.MAGENTA);
                        }

                        Phrase riskandstatus = new Phrase();
                        Chunk riskhead = new Chunk("", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE));

                        if (riskanalysis == true) 
                        {
                            riskhead = new Chunk("Risk: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE));

                        }

                        else
                        {
                            riskhead = new Chunk("", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE));

                        }
                        riskandstatus.Add(riskhead);

                        Chunk risk = new Chunk(Obsrecordreader[13].ToString().Trim(), cellcontentfonts);
                        riskandstatus.Add(risk);

                        tablecell = new PdfPCell(riskandstatus) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };

                        tablecell.Border = Rectangle.NO_BORDER;
                        tablecell.Colspan = 2;

                        tablecell.CellEvent = new RoundedBorderObservations();
                        //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                        tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tableObsId.AddCell(tablecell);

                        ////////

                        //BELOW BLANK ROW BETWEEN Obs ID, Question number RIsk and the next Block for Observations text and details..

                        ////////

                        //cellBlankRow.Colspan = 4;
                        //cellBlankRow.HorizontalAlignment = 1;
                        //cellBlankRow.Border = Rectangle.NO_BORDER;
                        //cellBlankRow.FixedHeight = 3f;
                        //tableObsId.AddCell(cellBlankRow);

                        document.Add(tableObsId);

                        blankcell.Colspan = 4;
                        blankcell.Border = Rectangle.NO_BORDER;
                        blankcell.FixedHeight = 2f;
                        tableObsanddetails.AddCell(blankcell);

                        var cellcontentobs = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        var cellcontentobs1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLUE);
                        var cellcontentobs2 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        // QUESTION NO. + QUESTION TEXT + ANSWER

                        //tablecell = new PdfPCell(new Phrase("Ques.: "+Obsrecordreader[7].ToString().Trim(), cellcontentobs));
                        tablecell = new PdfPCell() { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 5, PaddingRight = 5 };
                        tablecell.AddElement(new Chunk("Question: " + "\n", cellcontentobs1));
                        tablecell.AddElement(new Chunk(Obsrecordreader[16].ToString().Trim(), cellcontentobs2));
                        tablecell.Colspan = 3;
                        tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        tablecell.CellEvent = new RoundedBorderObservationcontent();
                        tablecell.Border = Rectangle.NO_BORDER;
                        //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                        tablecell.HorizontalAlignment = Element.ALIGN_CENTER;

                        tableObsanddetails.AddCell(tablecell);

                        int RemainingMedia = 0;
                        int RemainingFiles = 0;

                        int serialnumbermedia = 1;
                        int serialnumberfiles = 1;


                        var cellcontentobs3 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLUE);
                        var cellcontentobs4 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.RED);
                        var cellcontentobs5 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                        /////WHETHER GENERAL CATEGORY OR OBSERVATION QUESTION
                        /////

                        if (!String.IsNullOrEmpty(Obsrecordreader[8].ToString().Trim()) || Obsrecordreader[8].ToString().Trim() != "") //CHECK IF THERE IS ANY ANSWER SELECTED.
                        {
                            if (Obsrecordreader[8].ToString().Contains("999")) //ATTACHMENTS WITH GENERAL CATEGORY OF INSPECTIONS.

                            {
                                tablecell = new PdfPCell(new Phrase("--")) { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                                tablecell.Border = Rectangle.NO_BORDER;
                                tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tablecell.CellEvent = new RoundedBorderObservationcontent();
                                //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                tableObsanddetails.AddCell(tablecell);

                                //var cellcontentobs1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLUE);
                                //var cellcontentobs2 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                                tablecell = new PdfPCell(new Phrase(" "));

                                tablecell.AddElement(new Chunk("Details: " + "\n", cellcontentobs1));
                                tablecell.AddElement(new Chunk(Obsrecordreader[12].ToString().Trim(), cellcontentobs2));
                                tablecell.Colspan = 4;
                                tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tablecell.CellEvent = new RoundedBorderObservationcontent();
                                tablecell.Border = Rectangle.NO_BORDER;
                                //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                tableObsanddetails.AddCell(tablecell);

                            }

                            else
                            {
                                tablecell = new PdfPCell(new Phrase("[ " + Obsrecordreader[8].ToString().Trim().Split('-')[1] + " ]", headingfont1)) { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                                tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tablecell.CellEvent = new RoundedBorderObservationcontent();
                                //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                tablecell.Border = Rectangle.NO_BORDER;
                                tableObsanddetails.AddCell(tablecell);

                                tablecell.AddElement(new Chunk("Observation: " + "\n", cellcontentobs4));

                                if (String.IsNullOrEmpty(Obsrecordreader[9].ToString().Trim()))
                                {
                                    tablecell.AddElement(new Chunk(Obsrecordreader[12].ToString().Trim(), cellcontentobs2));
                                }

                                else
                                {
                                    tablecell.AddElement(new Chunk(Obsrecordreader[12].ToString().Trim() + "\n" + "Date: " + Obsrecordreader[9].ToString().Trim(), cellcontentobs2));
                                }

                                tablecell.Colspan = 4;
                                tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tablecell.CellEvent = new RoundedBorderObservationcontent();
                                tablecell.Border = Rectangle.NO_BORDER;
                                //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                tableObsanddetails.AddCell(tablecell);

                            }
                        }

                        else

                        {
                            tablecell = new PdfPCell(new Phrase("")) { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                            tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tablecell.CellEvent = new RoundedBorderObservationcontent();
                            //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                            tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            tablecell.Border = Rectangle.NO_BORDER;
                            tableObsanddetails.AddCell(tablecell);

                            tablecell.AddElement(new Chunk("Observation: " + "\n", cellcontentobs4));

                            if (String.IsNullOrEmpty(Obsrecordreader[9].ToString().Trim()))
                            {
                                tablecell.AddElement(new Chunk(Obsrecordreader[12].ToString().Trim(), cellcontentobs2));
                            }

                            else
                            {
                                tablecell.AddElement(new Chunk(Obsrecordreader[12].ToString().Trim() + "\n" + "Date: " + Obsrecordreader[9].ToString().Trim(), cellcontentobs2));
                            }

                            tablecell.Colspan = 4;
                            tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tablecell.CellEvent = new RoundedBorderObservationcontent();
                            tablecell.Border = Rectangle.NO_BORDER;
                            //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                            tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            tableObsanddetails.AddCell(tablecell);
                        }

                        document.Add(tableObsanddetails);
                        //blankcell.Colspan = 4;
                        //tableObservations.AddCell(blankcell);

                        blankcell.Colspan = 4;
                        blankcell.Border = Rectangle.NO_BORDER;
                        tablemediaNfiles.AddCell(blankcell);


                        ///////// FETCH ATTACHMENTS MEDIA AND FILES
                        #region 

                        MediaFiles = new List<string>();
                        Files = new List<string>();
                        string attachmentsfolderpath1 = "";

                        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));

                        attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

                        string query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "'"; //+ " and trim(Attachment_Title) = 'Media'";

                        table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
                        using var connection2 = mlocationdb2.getConnection();
                        using System.Data.IDataReader reader = mlocationdb2.SelectDataByquery(query);
                        tableObsanddetails = new PdfPTable(4);
                        cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);

                        List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();
                        

                        while (reader.Read())
                        {
                            Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                            int.Parse(reader[0].ToString()),
                            int.Parse(reader[1].ToString()),
                            int.Parse(reader[2].ToString()),
                            reader[3].ToString().Trim(),
                            reader[4].ToString(),
                            reader[5].ToString().Trim(),
                            reader[6].ToString().Trim(),
                            reader[7].ToString().Trim(),
                            reader[8].ToString().Trim(),
                            reader[9].ToString().Trim(),
                            reader[10].ToString().Trim(),
                            reader[11].ToString().Trim());

                            //Debug.Log("Stock Code: " + entity._stocksym);
                            myList.Add(entity);

                            var output1 = JsonUtility.ToJson(entity, true);
                            
                        }
                        reader.Dispose();
                        if (myList.Count != 0)
                        {
                            foreach (var x in myList)
                            {
                                if (x._Attachment_Title.Trim() == "Media")
                                {
                                    MediaFiles.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
                                }
                                else if (x._Attachment_Title.Trim() == "File")
                                    Files.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());

                                if (x._Attachment_Details_1.Trim().ToLower() == "sire20" &&(x._Attachment_Title.Trim() == "HumanElement" || x._Attachment_Title.Trim() == "ProcessElement" || x._Attachment_Title.Trim() == "HardwareElement"))
                                {
                                    if(x._Attachment_Details_2.Trim() == "1")

                                    {
                                        PdfPCell tablecell2 = new PdfPCell(new Phrase(" "));

                                        tablecell2.AddElement(new Chunk(x._Attachment_Title.Trim() + "\n", cellcontentobs3));
                                        //tablecell2.AddElement(new Chunk("Not As Expected" + "\n", cellcontentobs2));
                                        tablecell2.AddElement(new Chunk(x._Attachment_Name, cellcontentobs2));
                                        tablecell2.Colspan = 3;
                                        //tablecell2.VerticalAlignment = Element.ALIGN_LEFT;
                                        tablecell2.CellEvent = new RoundedBorderObservationcontent();
                                        tablecell2.Border = Rectangle.NO_BORDER;
                                        //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                        tablecell2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tableObsanddetails.AddCell(tablecell2);

                                        tablecell = new PdfPCell(new Phrase("[ "+"Not As Expected"+" ]", cellcontentfonts)) { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                                        tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        tablecell.CellEvent = new RoundedBorderObservationcontent();
                                        //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                        tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tablecell.Border = Rectangle.NO_BORDER;
                                        tableObsanddetails.AddCell(tablecell);

                                    }

                                   else if (x._Attachment_Details_2.Trim() == "0")

                                    {
                                        PdfPCell tablecell2 = new PdfPCell(new Phrase(" "));

                                        tablecell2.AddElement(new Chunk(x._Attachment_Title.Trim() + "\n", cellcontentobs3));
                                        //tablecell2.AddElement(new Chunk("As Expected" + "\n", cellcontentobs2));
                                        tablecell2.AddElement(new Chunk(x._Attachment_Name, cellcontentobs2));
                                        tablecell2.Colspan = 3;
                                        //tablecell2.VerticalAlignment = Element.ALIGN_LEFT;
                                        tablecell2.CellEvent = new RoundedBorderObservationcontent();
                                        tablecell2.Border = Rectangle.NO_BORDER;
                                        //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                        tablecell2.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tableObsanddetails.AddCell(tablecell2);

                                        tablecell = new PdfPCell(new Phrase("[ " + "As Expected" + " ]", cellcontentobs2)) { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                                        tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        tablecell.CellEvent = new RoundedBorderObservationcontent();
                                        //tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;
                                        tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tablecell.Border = Rectangle.NO_BORDER;
                                        tableObsanddetails.AddCell(tablecell);
                                    }
                                    
                                }
                            }

                            document.Add(tableObsanddetails);
                        }

                        else
                        {
                            Debug.Log("There are no files associated to this observation.");
                        }

                        var mediafileCount = MediaFiles.Count;
                        var filesCount = Files.Count;
                        RemainingMedia = mediafileCount;
                        RemainingFiles = filesCount;
                        tablemediaNfiles.SplitLate = false; // Ensures rows move to the next page if they don't fit entirely
                        tablemediaNfiles.SplitRows = true;




                        if (mediafileCount > 0)
                        {
                            cellBlankRow.Colspan = 4;
                            cellBlankRow.HorizontalAlignment = 1;
                            cellBlankRow.Border = Rectangle.NO_BORDER;
                            cellBlankRow.FixedHeight = 10f;
                            tablemediaNfiles.AddCell(cellBlankRow);

                            tablecell = new PdfPCell(new Phrase("Objective Evidence", cellcontentobs5)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                            tablecell.CellEvent = new MediaFilesholder();
                            tablecell.Border = Rectangle.NO_BORDER;
                            tablecell.Colspan = 4;
                            tablecell.FixedHeight = 10f;
                            tablecell.HorizontalAlignment = Element.ALIGN_LEFT;

                            tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tablemediaNfiles.AddCell(tablecell);

                            blankcell.Colspan = 4;
                            blankcell.FixedHeight = 5f;
                            tablemediaNfiles.AddCell(blankcell);

                            document.Add(tablemediaNfiles);
                            tablemediaNfiles = new PdfPTable(4);
                            tablemediaNfiles.WidthPercentage = 100;
                            //tablemediaNfiles.SpacingBefore = 15;
                            tablemediaNfiles.SetWidths(new float[] { 1, 3, 1, 1 });
                            tablemediaNfiles.HorizontalAlignment = Element.ALIGN_CENTER;


                            PdfPTable table1 = new PdfPTable(2);

                            //table1.SplitLate = false;
                            //table1.SplitRows = true;

                            PdfPTable tablemediaNfiles1=new PdfPTable(4);
                            foreach (var media in MediaFiles)
                            {
                                if (File.Exists(media.ToString()))
                                {
                                    if (serialnumbermedia % 2 != 0 || MediaFiles.Count == 1)
                                    {
                                        tablemediaNfiles1 = new PdfPTable(4);
                                        tablemediaNfiles1.WidthPercentage = 100;
                                        //tablemediaNfiles.SpacingBefore = 15;
                                        tablemediaNfiles1.SetWidths(new float[] { 1, 3, 1, 1 });
                                        tablemediaNfiles1.HorizontalAlignment = Element.ALIGN_CENTER;


                                        table1 = new PdfPTable(2);
                                    }
                                    // Debug.Log("Media Files Found " + media.ToString() + "Total media Files: " + mediafileCount.ToString());

                                    //var Obsimagepath = MediaFiles.ToString();
                                    tablecell = new PdfPCell(new Phrase(new Phrase("")));
                                    var Obsimagepath = media.ToString();
                                    
                                    Texture2D Mediafile = NativeGallery.LoadImageAtPath(Obsimagepath, 1024, false);
                                    //var inspsign = iTextSharp.text.Image.GetInstance(InspSign);
                                    iTextSharp.text.Image Obsimage = ConvertTextureToPdfImage(Mediafile,10);

                                    //heading.SpacingBefore = 20;
                                    Obsimage.ScaleAbsolute(200f, 150f);

                                    // Set minimum width and height
                                    // Define minimum and maximum dimensions

                                    float minWidth = 150f;   // Minimum width
                                    float minHeight = 100f;  // Minimum height
                                    float maxWidth = 200f;   // Maximum width
                                    float maxHeight = 150f;  // Maximum height

                                    // Get the current dimensions of the image
                                    float imgWidth = Obsimage.Width;
                                    float imgHeight = Obsimage.Height;

                                    // Maintain aspect ratio while ensuring the image fits within min and max bounds
                                    if (imgWidth < minWidth || imgHeight < minHeight || imgWidth > maxWidth || imgHeight > maxHeight)
                                    {
                                        // Calculate ratios to scale the image to fit the minimum and maximum dimensions
                                        float widthRatio = minWidth / imgWidth;
                                        float heightRatio = minHeight / imgHeight;
                                        float maxWidthRatio = maxWidth / imgWidth;
                                        float maxHeightRatio = maxHeight / imgHeight;

                                        // Use the largest ratio for scaling up, ensuring minimum size, and the smallest for scaling down
                                        float scaleUp = Math.Max(widthRatio, heightRatio);    // To scale up to minimum size
                                        float scaleDown = Math.Min(maxWidthRatio, maxHeightRatio);  // To scale down to maximum size

                                        // Final scale factor ensuring it meets both the minimum and maximum size requirements
                                        float finalScale = Math.Min(scaleDown, Math.Max(1, scaleUp));

                                        // Apply the scaling while maintaining the aspect ratio
                                        Obsimage.ScaleAbsolute(imgWidth * finalScale, imgHeight * finalScale);
                                    }
                                        Phrase phrase = new Phrase();

                                    // Add the image to the phrase
                                    phrase.Add(new Chunk(Obsimage, 0, 0, true));

                                    // Add a line break and then the text
                                    phrase.Add(Chunk.NEWLINE);  // Add a line break
                                    Match ratingMatch = Regex.Match(media, @"Rating_[0-5]");

                                    string rating = ratingMatch.Success ? ratingMatch.Value : "Rating_Not_Found";

                                    phrase.Add(new Chunk(serialnumbermedia + ". " + Path.GetFileName(media).Split("_Media_")[1]+"-"+rating, cellcontentobs5));  // Add the image name as text

                                    // Step 5: Add the Phrase to a PdfPCell
                                    PdfPCell cell = new PdfPCell(phrase);
                                    
                                    //cell.Border = PdfPCell.NO_BORDER; // Optional: Remove border
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER; // Center align image and text


                                    table1.AddCell(cell);

                                    RemainingMedia -= 1;
                                    serialnumbermedia += 1;

                                    bool temp = false;

                                    if ((RemainingMedia == 0 || mediafileCount == 1) && mediafileCount % 2 != 0)
                                    {
                                        blankcell.Colspan = 1;
                                        table1.AddCell(blankcell);
                                        temp = true;
                                    }

                                    else
                                    {

                                        Debug.Log("Even Number of media files.");
                                    }
                                    if (serialnumbermedia % 2 != 0 || MediaFiles.Count == 1 || temp)
                                    {
                                        PdfPCell AddingNested = new PdfPCell();
                                        AddingNested.Colspan = 4;
                                        AddingNested.CellEvent = new RoundedBorderObservationcontent();
                                        AddingNested.Border = Rectangle.NO_BORDER;
                                        AddingNested.AddElement(table1);
                                        tablemediaNfiles1.AddCell(AddingNested);

                                        tablecell = new PdfPCell();
                                        tablecell.Colspan = 4;
                                        tablecell.Border = Rectangle.NO_BORDER;
                                        tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        tablemediaNfiles1.AddCell(tablecell);

                                        cellBlankRow.Colspan = 4;
                                        cellBlankRow.HorizontalAlignment = 1;
                                        cellBlankRow.Border = Rectangle.NO_BORDER;
                                        cellBlankRow.FixedHeight = 10f;
                                        tablemediaNfiles1.AddCell(cellBlankRow);

                                        document.Add(tablemediaNfiles1);
                                    }
                                }


                                else
                                {
                                    Debug.Log("There was no file of this name; " + media.ToString());
                                    //blankcell.Colspan = 4;
                                    //tableObservations.AddCell(blankcell);
                                }

                            }

                            Debug.Log("REMAINING MEDIA COUNT == " + RemainingMedia.ToString());
                            // NESTED TABLE FOR IMAGES AND FILES

                            //PdfPCell AddingNested = new PdfPCell();
                            //AddingNested.Colspan = 4;
                            //AddingNested.CellEvent = new RoundedBorderObservationcontent();
                            //AddingNested.Border = Rectangle.NO_BORDER;
                            //AddingNested.AddElement(table1);
                            //tablemediaNfiles.AddCell(AddingNested);
                            //if (mediafileCount < 3)
                            //{
                            //    tablemediaNfiles.KeepTogether = true;
                            //}
                            //else
                            //{
                            //    tablemediaNfiles.KeepTogether = false;
                            //}
                            //blankcell.Colspan = 4;
                            //tableObservations.AddCell(blankcell);
                        }

                        blankcell.Colspan = 4;
                        blankcell.FixedHeight = 5f;
                        tablemediaNfiles.AddCell(blankcell);

                        if (filesCount > 0)
                        {
                            cellBlankRow.Colspan = 4;
                            cellBlankRow.HorizontalAlignment = 1;
                            cellBlankRow.Border = Rectangle.NO_BORDER;
                            cellBlankRow.FixedHeight = 10f;
                            tablemediaNfiles.AddCell(cellBlankRow);

                            tablecell = new PdfPCell(new Phrase("Documents", cellcontentobs5)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };
                            tablecell.CellEvent = new MediaFilesholder();
                            tablecell.Border = Rectangle.NO_BORDER;
                            tablecell.Colspan = 4;
                            tablecell.FixedHeight = 15f;
                            tablecell.HorizontalAlignment = Element.ALIGN_LEFT;

                            tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tablemediaNfiles.AddCell(tablecell);

                            //blankcell.Colspan = 4;
                            //tableObservations.AddCell(blankcell);

                            foreach (string currentFile in Files)
                            {
                                if (File.Exists(currentFile.ToString()))
                                {
                                    Debug.Log("Documents Found " + currentFile.ToString() + "Total docu Files: " + filesCount.ToString());

                                    tablecell = new PdfPCell(new Phrase(serialnumberfiles + ". " + Path.GetFileName(currentFile).Split("_File_")[1], cellcontentobs5));
                                    tablecell.Colspan = 4;
                                    tablecell.CellEvent = new RoundedBorderObservationcontent();
                                    tablecell.Border = Rectangle.NO_BORDER;

                                    tablemediaNfiles.AddCell(tablecell);

                                    RemainingFiles -= 1;
                                    serialnumberfiles += 1;

                                    //if ((RemainingFiles == 1 || filesCount == 1) && filesCount % 2 != 0)
                                    //{
                                    //    blankcell.Colspan = 1;

                                    //    tableObservations.AddCell(blankcell);
                                    //}

                                    //else
                                    //{
                                    //    Debug.Log("Even Number of media files.");
                                    //}
                                }
                                else
                                {
                                    Debug.Log("There was no file of this name; " + currentFile.ToString());
                                    tablecell = new PdfPCell(new Phrase("No documents attached.", cellcontentobs5));
                                    tablecell.Colspan = 4;
                                    tablecell.Border = Rectangle.NO_BORDER;
                                    tablecell.CellEvent = new RoundedBorderObservationcontent();
                                    tablecell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    tablemediaNfiles.AddCell(tablecell);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("No Documents Found in the folder.");
                            //blankcell.Colspan = 4;
                            //tableObservations.AddCell(blankcell);
                        }

                        #endregion

                        //tableObservations.SpacingAfter = 10f;

                        tablecell = new PdfPCell();
                        tablecell.Colspan = 4;
                        tablecell.Border = Rectangle.NO_BORDER;
                        tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tablemediaNfiles.AddCell(tablecell);

                        cellBlankRow.Colspan = 4;
                        cellBlankRow.HorizontalAlignment = 1;
                        cellBlankRow.Border = Rectangle.NO_BORDER;
                        cellBlankRow.FixedHeight = 10f;
                        tablemediaNfiles.AddCell(cellBlankRow);

                        //tablecell = new PdfPCell(new Phrase("---------------"));
                        //tablecell.Colspan = 4;
                        //tablecell.Border = Rectangle.NO_BORDER;
                        //tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //tableObservations.AddCell(tablecell);

                        document.Add(tablemediaNfiles);

                        mlocationdb2.close();
                    }

                    Chaptername = Obsrecordreader[0].ToString().Trim();

                }
            }

            else
            {
                // The reader has no rows

                Paragraph NoObservations = new Paragraph("No Observations recorded for this inspection.", smallinfotext);
                //NoObservations.IndentationLeft = 57f;  // Left margin
                NoObservations.SpacingBefore = 20;
                NoObservations.Alignment = Element.ALIGN_CENTER;
                document.Add(NoObservations);


                Debug.Log("No rows found.");
            }
            document.NewPage();

            //if (Reportstatus == "D" || Reportstatus == "N")
            //{
            //    Paragraph ReportStopsHere = new Paragraph("Report in draft, detailed report is available only for signed and published inspections.", DraftFont);
            //    //ReportStopsHere.IndentationLeft = 57f;  // Left margin
            //    ReportStopsHere.SpacingBefore = 20;
            //    ReportStopsHere.Alignment = Element.ALIGN_CENTER;
            //    document.Add(ReportStopsHere);
            //}

            //else
            //{
                Entirechecklisttopdf();
            //}


            document.SetMargins(30, 30, 100, 30);
            document.Close();
            mlocationDb.close();
            mlocationdb1.close();

            writer.Close();
        }

       
        //StartCoroutine(OpenPDF());

        if (onlyforexport != true)
        {
            OpenPDF(path);
        }
        else
        {
            onlyforexport = false;
            return;
        }

        onlyforexport = false;
        //PrintFiles(); // FOR WINDOWS TO SEE OPENING OF PDF YOU CAN UNCOMMENT THIS
    }

   
    iTextSharp.text.Image ConvertTextureToPdfImage(Texture2D texture, int compression)
    {
       
        // Convert Texture2D to byte array (PNG format)
        byte[] imageBytes = texture.EncodeToJPG(compression);

        // Check if the imageBytes array is null or empty
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return null;
        }

        // Try-catch block to handle any potential exceptions
        try
        {
            // Convert byte array to iTextSharp Image
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imageBytes);
            //pdfImage.SetDpi(72, 72);
;            return pdfImage;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    iTextSharp.text.Image ConvertTextureToPdfImageCaverPage(Texture2D texture)
    {

        // Convert Texture2D to byte array (PNG format)
        byte[] imageBytes = texture.EncodeToJPG();

        // Check if the imageBytes array is null or empty
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return null;
        }

        // Try-catch block to handle any potential exceptions
        try
        {
            // Convert byte array to iTextSharp Image
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imageBytes);
            pdfImage.SetDpi(72, 72);
            ; return pdfImage;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    ////////////////////////////////

    //5. COmplete inspection table, without photos etc. Adding tables from here on

    ////////////////////////////////

    public void Entirechecklisttopdf()
    {
        var Mainheading = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
        var headingfont2 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        var headingfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);
        var Obstable = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        BaseColor ChpaterBackgroundColor = new BaseColor(241, 243, 249); // Custom background color
        BaseColor headerBackgroundColor = new BaseColor(9, 46, 72); // Custom background color
        List<string> ChaptIDSection = new List<string>();
        Paragraph Checklistheader = new Paragraph("INSPECTION CHECKLIST", Mainheading);
        //Checklistheader.IndentationLeft = 57f;  // Left margin
        Checklistheader.SpacingAfter = 20;
        Checklistheader.Alignment = Element.ALIGN_CENTER;
        document.Add(Checklistheader);

        table_Inspection_Observations mlocationDb = new table_Inspection_Observations();
        using var connection = mlocationDb.getConnection();

        //string query1 = "select b.Template_Section_Ques as 'ChapterName' ,a.* from Inspection_Observations a left join Inspection_Observations b on a.Cloud_DB_ParentID = b.Cloud_DB_ID and TRIM(a.Inspection_PrimaryDetails_ID) = TRIM(b.Inspection_PrimaryDetails_ID) where TRIM(a.Inspection_PrimaryDetails_ID)= '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "'; ";// and TRIM(a.Observation_Text)!= '' and a.Observation_Text is not null";

        string query1 = "select b.Template_Section_Ques as 'ChapterName' ,a.Obs_Details_5 as 'Section'," +
            "a.Cloud_DB_ParentID,b.Obs_Details_1 as 'ChapterDescription' from Inspection_Observations a left join Inspection_Observations b on a.Cloud_DB_ParentID = b.Cloud_DB_ID " +
            "and TRIM(a.Inspection_PrimaryDetails_ID)= TRIM(b.Inspection_PrimaryDetails_ID) where TRIM(a.Inspection_PrimaryDetails_ID) = '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' " +
            "and TRIM(a.Obs_Details_8) = 'Question' and TRIM(b.Obs_Details_8) = 'Chapter' GROUP by b.Template_Section_Ques ," +
            "a.Obs_Details_5 ,a.Cloud_DB_ParentID" + " ORDER BY " +
    "CAST(SUBSTR(a.Template_Section_Ques || '.', 1,INSTR(a.Template_Section_Ques || '.', '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(a.Template_Section_Ques || '.',INSTR(a.Template_Section_Ques || '.', '.') + 1, INSTR(SUBSTR(a.Template_Section_Ques || '.',INSTR(a.Template_Section_Ques || '.', '.') + 1), '.') - 1) AS INTEGER) ASC," +
    "CAST(SUBSTR(a.Template_Section_Ques || '.',INSTR(SUBSTR(a.Template_Section_Ques || '.',INSTR(a.Template_Section_Ques || '.', '.') + 1), '.') + 1) AS INTEGER) ASC;";

        using System.Data.IDataReader Obsrecordreader = mlocationDb.getDatabyQuery(query1);

        while (Obsrecordreader.Read()) // READING CHAPTERS HERE
        {
            List<String> observations = new List<string>();

            ChaptIDSection = new List<string>();

            var headingfont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            var cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);

            if (Obsrecordreader[0].ToString().Trim() != "")
            {
                observations.Add(Obsrecordreader[0].ToString().Trim());  //Chapter Name
                observations.Add(Obsrecordreader[1].ToString().Trim());  //Section 
                observations.Add(Obsrecordreader[2].ToString().Trim());  //Cloud DB parent ID for the Chapters
                observations.Add(Obsrecordreader[3].ToString().Trim());  //CHAPTER DESCRIPTION
            }

            PdfPTable tableChapters1 = new PdfPTable(4);
            tableChapters1.WidthPercentage = 100;
            //tableChapters1.SpacingBefore = 15;
            tableChapters1.SetWidths(new float[] { 1, 3, 1, 1 });
            tableChapters1.HorizontalAlignment = Element.ALIGN_CENTER;

            blankcell.Colspan = 4;
            blankcell.FixedHeight = 10f;
            blankcell.Border = Rectangle.NO_BORDER;
            tableChapters1.AddCell(blankcell);

            //tablecell = new PdfPCell(new Phrase(observations[0] +" "+ observations[3]+ " | " + "Section: " + observations[1], headingfont2));

            tablecell = new PdfPCell(new Phrase(observations[0] + " " + observations[3] , headingfont2));

            tablecell.Colspan = 4;
            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.BackgroundColor = ChpaterBackgroundColor;
            tablecell.HorizontalAlignment = Element.ALIGN_LEFT;
            tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //tablecell.CellEvent = new MediaFilesholder();
            tablecell.BorderColor = iTextSharp.text.BaseColor.BLUE;

            tableChapters1.AddCell(tablecell);

            blankcell.Colspan = 1;
            blankcell.Border = Rectangle.NO_BORDER;
            tableChapters1.AddCell(blankcell);

            //string joinedString = string.Join(", ", observations);
            //Debug.Log(joinedString);
            
            document.Add(tableChapters1);

            ChaptIDSection.Add($"{observations[2]},{observations[1]}");

            //foreach (var item in ChaptIDSection)
            //{
                string[] splitChecklistHeaders = { "" };

                // Split the first string in the list by commas to get individual headers

                splitChecklistHeaders = ChaptIDSection[0].Split(',');

                string query2 = "select a.* from Inspection_Observations a left join Inspection_Observations b on cast(a.Cloud_DB_ParentID as int) = " +
                "cast(b.Cloud_DB_ID as int) and TRIM(a.Inspection_PrimaryDetails_ID) = TRIM(b.Inspection_PrimaryDetails_ID) where TRIM(a.Inspection_PrimaryDetails_ID) = '" +
                "" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' and cast(a.Cloud_DB_ParentID as int)= '" + int.Parse(splitChecklistHeaders[0]) +
                "' and TRIM(a.Obs_Details_5)= '" + splitChecklistHeaders[1] + "' and TRIM(a.Obs_Details_8) = 'Question' and TRIM(b.Obs_Details_8) = 'Chapter'; ";// and TRIM(a.Observation_Text)!= '' and a.Observation_Text is not null;";
                using System.Data.IDataReader Obsrecordreader1 = mlocationDb.getDatabyQuery(query2);

                List<String> observations1 = new List<string>();

            //CREATING HEADER FOR THE OBSERVATION TABLE
            List<string> headers = new List<string> { };
            int headercount = 0;
            if (riskanalysis == true)
            {
                headers = new List<string> { "Ques. No.", "Question", "Observation/Comment", "Risk", "Ans/Cond", "Date", };
                headercount = 6;
            }
                else
            {
                headers = new List<string> { "Ques. No.", "Question", "Observation/Comment", "Ans/Cond", "Date", };
                headercount = 5;
            }

                // Create a table with a number of columns equal to the number of split headers
                PdfPTable tableheader = new PdfPTable(headercount);
                tableheader.SplitLate = false; // Ensures rows move to the next page if they don't fit entirely
                tableheader.SplitRows = true;

                tableheader.WidthPercentage = 100;
            if (headercount == 6)
            {
                tableheader.SetWidths(new float[] { 0.5f, 1.8f, 1.8f, 0.5f, 0.5f, 0.6f });
            }
                else
            {
                tableheader.SetWidths(new float[] { 0.5f, 1.8f, 1.8f, 0.5f, 0.6f });
            }

                // Add each header to the table as a separate cell
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header.Trim(), headingfont));
                    cell.BackgroundColor = headerBackgroundColor;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell.CellEvent = new RoundedBorderObservations();
                    cell.Padding = 5;
                    tableheader.AddCell(cell);
                }


                while (Obsrecordreader1.Read()) // READING QUESTIONS UNDER THE CHAPTER HERE
                {
                    observations1 = new List<string>();
                    if (Obsrecordreader1[0].ToString().Trim() != "")
                    {
                        observations1.Add(Obsrecordreader1[6].ToString().Trim());  // Template_Section_Ques - Question No.
                        observations1.Add(Obsrecordreader1[15].ToString().Trim()); // Obs_Details_1 - The Question
                        observations1.Add(Obsrecordreader1[11].ToString().Trim()); // Observation_Text
                        observations1.Add(Obsrecordreader1[12].ToString().Trim()); // RiskCategory
                        

                    if (string.IsNullOrEmpty(Obsrecordreader1[7].ToString().Trim()) | Obsrecordreader1[7].ToString().Trim() == "" || Obsrecordreader1[7].ToString().Trim().Contains("Free")) // Selected_Answer
                        {
                            observations1.Add("-");
                        }

                        else

                        {
                            observations1.Add(Obsrecordreader1[7].ToString().Trim().Split('-')[1]);
                        }

                    observations1.Add(Obsrecordreader1[8].ToString().Trim()); // Date

                    if (String.IsNullOrEmpty(observations1[5]))
                    {
                        observations1[5] = "";
                    }

                    else
                    {
                        observations1[5] = observations1[5];
                    }

                    }

                //string joinedString = string.Join(", ", observations1);
                //Debug.Log(joinedString);
                List<string> ObservationsList1 = new List<string>() {};
                if (headercount == 6)
                {
                    ObservationsList1 = new List<string>() { observations1[0], observations1[1], observations1[2], observations1[3], observations1[4], observations1[5] };
                }
                else if (headercount == 5)

                {
                    ObservationsList1 = new List<string>() { observations1[0], observations1[1], observations1[2], observations1[4], observations1[5] };
                }

                    foreach (string item in ObservationsList1)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(item.Trim(), Obstable));
                        //cell.BackgroundColor = headerBackgroundColor;

                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 5;
                        tableheader.AddCell(cell);
                    }

                }
            document.Add(tableheader);
            

        }
        document.NewPage();

        ObservationMediafile();
        

    }


    public void ObservationMediafile()

    {
        var Mainheading = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
        var cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);
        var cellcontentobs2 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        var cellcontentobs3 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLUE);
        var cellcontentobs4 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.RED);
        var cellcontentobs5 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

        int RemainingMedia = 0;
        int RemainingFiles = 0;

        int serialnumbermedia = 1;
        int serialnumberfiles = 1;

        Paragraph MediaFilesheader = new Paragraph("MEDIA AND FILE UPLOADS", Mainheading);

        MediaFilesheader.Alignment = Element.ALIGN_CENTER;
        document.Add(MediaFilesheader);

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));

        PdfPTable tableObsanddetails = new PdfPTable(4);
        tableObsanddetails.WidthPercentage = 100;

        //tableObsanddetails.SpacingBefore = 15;
        tableObsanddetails.SetWidths(new float[] { 1, 3, 1, 1 });
        tableObsanddetails.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPTable tablemediaNfiles = new PdfPTable(4);
        tablemediaNfiles.WidthPercentage = 100;
        //tablemediaNfiles.SpacingBefore = 15;
        tablemediaNfiles.SetWidths(new float[] { 1, 3, 1, 1 });
        tablemediaNfiles.HorizontalAlignment = Element.ALIGN_CENTER;
        ///////// FETCH ATTACHMENTS MEDIA AND FILES
        #region

        MediaFiles = new List<string>();
        Files = new List<string>();
        string attachmentsfolderpath1 = "";

        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));

        attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

        //string query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(Obsrecordreader[3].ToString().Trim()) + "'"; //+ " and trim(Attachment_Title) = 'Media'";

        string query = "cast(Inspection_PrimaryDetails_ID as int) = '"
            + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text)
            + "' and (Attachment_Title like '%Media%' OR Attachment_Title like '%File%') and cast(Inspection_Observations_ID as int) != '0'" +
            " order by cast(Inspection_Observations_ID as int) "; //+ " and trim(Attachment_Title) = 'Media'";

        table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
        using var connection2 = mlocationdb2.getConnection();
        using System.Data.IDataReader reader = mlocationdb2.SelectDataByquery(query);
        tableObsanddetails = new PdfPTable(4);
        cellcontentfonts = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);

        List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();

        while (reader.Read())
        {
            Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
            int.Parse(reader[0].ToString()),
            int.Parse(reader[1].ToString()),
            int.Parse(reader[2].ToString()),
            reader[3].ToString().Trim(),
            reader[4].ToString(),
            reader[5].ToString().Trim(),
            reader[6].ToString().Trim(),
            reader[7].ToString().Trim(),
            reader[8].ToString().Trim(),
            reader[9].ToString().Trim(),
            reader[10].ToString().Trim(),
            reader[11].ToString().Trim());

            //Debug.Log("Stock Code: " + entity._stocksym);
            myList.Add(entity);
            //ObservationIds.Add(entity._ID);
            var output1 = JsonUtility.ToJson(entity, true);

        }
        reader.Dispose();
        if (myList.Count != 0)
        {
            foreach (var x in myList)
            {
                if (x._Attachment_Title.Trim() == "Media")
                {
                    MediaFiles.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());

                }
                else if (x._Attachment_Title.Trim() == "File")
                {
                    Files.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
                }

            }

            document.Add(tableObsanddetails);
        }

        else
        {
            Debug.Log("There are no files associated to this observation.");
        }

        var mediafileCount = MediaFiles.Count;
        var filesCount = Files.Count;
        RemainingMedia = mediafileCount;
        RemainingFiles = filesCount;
        tablemediaNfiles.SplitLate = false; // Ensures rows move to the next page if they don't fit entirely
        tablemediaNfiles.SplitRows = true;
        int loop_var = 0;

        if (mediafileCount > 0)
        {


            if (mediafileCount % 8 == 0)
        {


            loop_var = mediafileCount / 8;
        }
        else
        {
            loop_var = Convert.ToInt32(Math.Floor(Convert.ToDouble(mediafileCount / 8)) + 1);
        }

        for (int i = 0; i < loop_var; i++)
        {

            
                PdfPTable tablemediaNfiles0 = new PdfPTable(4);
                tablemediaNfiles0.WidthPercentage = 100;
                //tablemediaNfiles.SpacingBefore = 15;
                tablemediaNfiles0.SetWidths(new float[] { 1, 3, 1, 1 });
                tablemediaNfiles0.HorizontalAlignment = Element.ALIGN_CENTER;
                ///////// FETCH ATTACHMENTS MEDIA AND FILES


                cellBlankRow.Colspan = 4;
                cellBlankRow.HorizontalAlignment = 1;
                cellBlankRow.Border = Rectangle.NO_BORDER;
                cellBlankRow.FixedHeight = 10f;
                tablemediaNfiles0.AddCell(cellBlankRow);

                tablecell = new PdfPCell(new Phrase("Uploads against various questions", cellcontentobs5)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                tablecell.CellEvent = new MediaFilesholder();
                tablecell.Border = Rectangle.NO_BORDER;
                tablecell.Colspan = 4;
                tablecell.FixedHeight = 10f;
                tablecell.HorizontalAlignment = Element.ALIGN_LEFT;

                tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tablemediaNfiles0.AddCell(tablecell);

                blankcell.Colspan = 4;
                blankcell.FixedHeight = 5f;
                tablemediaNfiles0.AddCell(blankcell);

                PdfPTable table1 = new PdfPTable(2);

                //table1.SplitLate = false;
                //table1.SplitRows = true;

                List<string> temp_media = new List<string>();
                int cnt_temp = 0;
                foreach (var media in MediaFiles)
                {
                    
                    if (((i+1) * 8) > cnt_temp && (i * 8) <= cnt_temp)
                        temp_media.Add(media);
                    cnt_temp++;
                }

                foreach (var media in temp_media)
                {
                    if (File.Exists(media.ToString()))
                    {
                        // Debug.Log("Media Files Found " + media.ToString() + "Total media Files: " + mediafileCount.ToString());

                        //var Obsimagepath = MediaFiles.ToString();
                        tablecell = new PdfPCell(new Phrase(new Phrase("")));
                        var Obsimagepath = media.ToString();

                        Texture2D Mediafile = NativeGallery.LoadImageAtPath(Obsimagepath, 1024, false);
                        //var inspsign = iTextSharp.text.Image.GetInstance(InspSign);

                        FileInfo fileInfo = new FileInfo(Obsimagepath);
                        long fileSizeInBytes = fileInfo.Length; // Get file size in bytes
                        int percentageMultiplier = 0;
                        if (fileSizeInBytes > 200 * 1024) // 100 KB in bytes
                        {
                            float sizeInKB = fileSizeInBytes / 1024f; // Convert to KB
                            percentageMultiplier = (int)Math.Round((200f / sizeInKB) * 100); // Find percentage multiplier

                            Debug.Log($"File size is {sizeInKB:F2} KB. Reduce to 200 KB with multiplier: {percentageMultiplier:F2}");
                        }
                        else
                        {
                            percentageMultiplier = 100;
                            Debug.Log("File size is already less than or equal to 100 KB.");
                        }

                        iTextSharp.text.Image Obsimage = ConvertTextureToPdfImage(Mediafile, percentageMultiplier);

                        // Set minimum width and height
                        // Define minimum and maximum dimensions

                        float minWidth = 150f;   // Minimum width
                        float minHeight = 100f;  // Minimum height
                        float maxWidth = 200f;   // Maximum width
                        float maxHeight = 150f;  // Maximum height

                        // Get the current dimensions of the image
                        float imgWidth = Obsimage.Width;
                        float imgHeight = Obsimage.Height;

                        // Maintain aspect ratio while ensuring the image fits within min and max bounds
                        if (imgWidth < minWidth || imgHeight < minHeight || imgWidth > maxWidth || imgHeight > maxHeight)
                        {
                            // Calculate ratios to scale the image to fit the minimum and maximum dimensions
                            float widthRatio = minWidth / imgWidth;
                            float heightRatio = minHeight / imgHeight;
                            float maxWidthRatio = maxWidth / imgWidth;
                            float maxHeightRatio = maxHeight / imgHeight;

                            // Use the largest ratio for scaling up, ensuring minimum size, and the smallest for scaling down
                            float scaleUp = Math.Max(widthRatio, heightRatio);    // To scale up to minimum size
                            float scaleDown = Math.Min(maxWidthRatio, maxHeightRatio);  // To scale down to maximum size

                            // Final scale factor ensuring it meets both the minimum and maximum size requirements
                            float finalScale = Math.Min(scaleDown, Math.Max(1, scaleUp));

                            // Apply the scaling while maintaining the aspect ratio
                            Obsimage.ScaleAbsolute(imgWidth * finalScale, imgHeight * finalScale);
                        }

                        Phrase phrase = new Phrase();

                        // Add the image to the phrase
                        phrase.Add(new Chunk(Obsimage, 0, 0, true));

                        // Add a line break and then the text
                        phrase.Add(Chunk.NEWLINE);  // Add a line break

                        Match ratingMatch = Regex.Match(media, @"Rating_[0-5]");

                        string rating = ratingMatch.Success ? ratingMatch.Value : "Rating_Not_Found";


                        phrase.Add(new Chunk("Q." + Path.GetFileName(media).Split("_Media_")[1]+"-"+rating, cellcontentobs5));  // Add the image name as text, Instead of serialnumbermedia, added the question number..

                        //serialnumbermedia (was replaced with Q.No.)

                        // Step 5: Add the Phrase to a PdfPCell
                        PdfPCell cell = new PdfPCell(phrase);
                        //cell.Border = PdfPCell.NO_BORDER; // Optional: Remove border
                        cell.HorizontalAlignment = Element.ALIGN_CENTER; // Center align image and text

                        //float minRowHeight = 150f;  // Change this value as needed
                        //cell.FixedHeight = minRowHeight;

                        table1.AddCell(cell);


                        RemainingMedia -= 1;
                        serialnumbermedia += 1;

                        if ((RemainingMedia == 1 || temp_media.Count == 1) && temp_media.Count % 2 != 0)
                        {
                            blankcell.Colspan = 1;

                            //blankcell.FixedHeight = minRowHeight;
                            
                            table1.AddCell(blankcell);
                        }

                        else
                        {

                            Debug.Log("Even Number of media files.");
                        }
                    }


                    else
                    {
                        Debug.Log("There was no file of this name; " + media.ToString());
                        //blankcell.Colspan = 4;
                        //tableObservations.AddCell(blankcell);
                    }


                }

                Debug.Log("REMAINING MEDIA COUNT == " + RemainingMedia.ToString());
                //NESTED TABLE FOR IMAGES AND FILES

                PdfPCell AddingNested = new PdfPCell();
                AddingNested.Colspan = 4;
                AddingNested.CellEvent = new RoundedBorderObservationcontent();
                AddingNested.Border = Rectangle.NO_BORDER;
                AddingNested.AddElement(table1);
                tablemediaNfiles0.AddCell(AddingNested);
                if (mediafileCount < 3)
                {
                    tablemediaNfiles0.KeepTogether = true;
                }
                else
                {
                    tablemediaNfiles0.KeepTogether = false;

                }


                //blankcell.Colspan = 4;
                //tableObservations.AddCell(blankcell);
                blankcell.Colspan = 4;
                blankcell.FixedHeight = 5f;
                tablemediaNfiles0.AddCell(blankcell);
                document.Add(tablemediaNfiles0);
                document.NewPage();
            }

        }


        PdfPTable tablemediaNfiles1 = new PdfPTable(4);
        tablemediaNfiles1.WidthPercentage = 100;
        //tablemediaNfiles1.SpacingBefore = 15;
        tablemediaNfiles1.SetWidths(new float[] { 1, 3, 1, 1 });
        tablemediaNfiles1.HorizontalAlignment = Element.ALIGN_CENTER;

        if (filesCount > 0)
            {
                cellBlankRow.Colspan = 4;
                cellBlankRow.HorizontalAlignment = 1;
                cellBlankRow.Border = Rectangle.NO_BORDER;
                cellBlankRow.FixedHeight = 10f;
            tablemediaNfiles1.AddCell(cellBlankRow);

                tablecell = new PdfPCell(new Phrase("Documents", cellcontentobs5)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 4, PaddingRight = 0 };
                tablecell.CellEvent = new MediaFilesholder();
                tablecell.Border = Rectangle.NO_BORDER;
                tablecell.Colspan = 4;
                tablecell.FixedHeight = 15f;
                tablecell.HorizontalAlignment = Element.ALIGN_LEFT;

                tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablemediaNfiles1.AddCell(tablecell);

                //blankcell.Colspan = 4;
                //tableObservations.AddCell(blankcell);

                foreach (string currentFile in Files)
                {
                    if (File.Exists(currentFile.ToString()))
                    {
                        Debug.Log("Documents Found " + currentFile.ToString() + "Total docu Files: " + filesCount.ToString());

                        tablecell = new PdfPCell(new Phrase(serialnumberfiles + ". " + Path.GetFileName(currentFile).Split("_File_")[1], cellcontentobs5));
                        tablecell.Colspan = 4;
                        tablecell.CellEvent = new RoundedBorderObservationcontent();
                        tablecell.Border = Rectangle.NO_BORDER;

                    tablemediaNfiles1.AddCell(tablecell);

                        RemainingFiles -= 1;
                        serialnumberfiles += 1;

                        //if ((RemainingFiles == 1 || filesCount == 1) && filesCount % 2 != 0)
                        //{
                        //    blankcell.Colspan = 1;

                        //    tableObservations.AddCell(blankcell);
                        //}

                        //else
                        //{
                        //    Debug.Log("Even Number of media files.");
                        //}
                    }
                    else
                    {
                        Debug.Log("There was no file of this name; " + currentFile.ToString());
                        tablecell = new PdfPCell(new Phrase("No documents attached.", cellcontentobs5));
                        tablecell.Colspan = 4;
                        tablecell.Border = Rectangle.NO_BORDER;
                        tablecell.CellEvent = new RoundedBorderObservationcontent();
                        tablecell.HorizontalAlignment = Element.ALIGN_LEFT;
                    tablemediaNfiles1.AddCell(tablecell);
                    }
                }
            }
            else
            {
                Debug.Log("No Documents Found in the folder.");
                //blankcell.Colspan = 4;
                //tableObservations.AddCell(blankcell);
            }

            #endregion

            tablecell = new PdfPCell();
            tablecell.Colspan = 4;
            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
        tablemediaNfiles1.AddCell(tablecell);

            cellBlankRow.Colspan = 4;
            cellBlankRow.HorizontalAlignment = 1;
            cellBlankRow.Border = Rectangle.NO_BORDER;
            cellBlankRow.FixedHeight = 10f;
        tablemediaNfiles1.AddCell(cellBlankRow);

            //tablecell = new PdfPCell(new Phrase("---------------"));
            //tablecell.Colspan = 4;
            //tablecell.Border = Rectangle.NO_BORDER;
            //tablecell.HorizontalAlignment = Element.ALIGN_CENTER;
            //tableObservations.AddCell(tablecell);

            document.Add(tablemediaNfiles1);

            //mlocationdb1.close();
            mlocationdb2.close();

        //document.NewPage();
        CheckStandardPhotos();
    }

    

    public void CheckStandardPhotos()
    {
        var Mainheading = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
        var cellcontentobs5 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        suffixforphotos = "";
        StandardPhotoHdrs = new List<string>();

        List<string> standardphotos = new List<string>();
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));
        standardphotos = mlocationdb1.standardphotos.ToString().Split(',').Select(x => x.Trim().ToLower()).ToList();

        string attachmentsfolderpath = "";
        table_Inspection_PrimaryDetails mlocationdb2 = new table_Inspection_PrimaryDetails();
        using var connection2 = mlocationdb2.getConnection();
        mlocationdb2.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));
        attachmentsfolderpath = mlocationdb2.folderpath.ToString();
        int photosectionID = 0;
        

        //tablestandardphotos = new PdfPTable(4);


        if ((standardphotos != null) && (standardphotos.Count > 1)) //THIS NEEDS TO BE TESTED, SINCE 1 is mentioned, what if only 1 photo is uploaded..
        {
            
            Paragraph Checklistheader = new Paragraph("STANDARD PHOTOGRAPHS", Mainheading);
            Checklistheader.SpacingAfter = 10f;
            Checklistheader.Alignment = Element.ALIGN_CENTER;
            document.Add(Checklistheader);

            

                foreach (string heading in standardphotos)
                {
                    tablestandardphotos = new PdfPTable(4);
                    tablestandardphotos.WidthPercentage = 100;
                    //tablestandardphotos.SpacingBefore = 15;
                    tablestandardphotos.SetWidths(new float[] { 1, 3, 1, 1 });
                    tablestandardphotos.HorizontalAlignment = Element.ALIGN_CENTER;

                    tablestandardphotos.SplitLate = false; // Ensures rows move to the next page if they don't fit entirely
                    tablestandardphotos.SplitRows = true;

                    photosectionID += 1;
                    Debug.Log(photosectionID + "_" + heading.ToString());

                    cellBlankRow.Colspan = 4;
                    cellBlankRow.HorizontalAlignment = 1;
                    cellBlankRow.Border = Rectangle.NO_BORDER;
                    cellBlankRow.FixedHeight = 10f;
                    tablestandardphotos.AddCell(cellBlankRow);

                    tablecell = new PdfPCell(new Phrase(heading.ToString().ToUpper(), cellcontentobs5)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
                    tablecell.CellEvent = new MediaFilesholder();
                    tablecell.Border = Rectangle.NO_BORDER;
                    tablecell.Colspan = 4;
                    tablecell.FixedHeight = 10f;
                    tablecell.HorizontalAlignment = Element.ALIGN_LEFT;

                    tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tablestandardphotos.AddCell(tablecell);

                    blankcell.Colspan = 4;
                    blankcell.FixedHeight = 5f;
                    tablestandardphotos.AddCell(blankcell);

                    suffixforphotos = "_" + "S" + photosectionID + "_";
                    StandardPhotoHdrs.Add(suffixforphotos);
                    StandardPhotosNested();
                    //document.Add(tablestandardphotos);
                }
            
        }

        else
        {
            tablestandardphotos = new PdfPTable(4);
            Paragraph Checklistheader = new Paragraph("OTHER PHOTOGRAPHS", Mainheading);
            Checklistheader.SpacingAfter = 10f;
            tablestandardphotos.WidthPercentage = 100;
            //tablestandardphotos.SpacingBefore = 15;
            tablestandardphotos.SetWidths(new float[] { 1, 3, 1, 1 });
            tablestandardphotos.HorizontalAlignment = Element.ALIGN_CENTER;

            tablestandardphotos.SplitLate = false; // Ensures rows move to the next page if they don't fit entirely
            tablestandardphotos.SplitRows = true;

            Checklistheader.Alignment = Element.ALIGN_CENTER;
            document.Add(Checklistheader);


            Debug.Log("1_" + "Other photos");
            suffixforphotos = "_" + "O1" + "_";

            cellBlankRow.Colspan = 4;
            cellBlankRow.HorizontalAlignment = 1;
            cellBlankRow.Border = Rectangle.NO_BORDER;
            cellBlankRow.FixedHeight = 10f;
            tablestandardphotos.AddCell(cellBlankRow);

            tablecell = new PdfPCell(new Phrase("Other photos".ToUpper(), cellcontentobs5)) { PaddingLeft = 5, PaddingTop = 0, PaddingBottom = 0, PaddingRight = 0 };
            tablecell.CellEvent = new MediaFilesholder();
            tablecell.Border = Rectangle.NO_BORDER;
            tablecell.Colspan = 4;
            tablecell.FixedHeight = 10f;
            tablecell.HorizontalAlignment = Element.ALIGN_LEFT;

            tablecell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablestandardphotos.AddCell(tablecell);

            blankcell.Colspan = 4;
            blankcell.FixedHeight = 5f;
            tablestandardphotos.AddCell(blankcell);
            StandardPhotoHdrs.Add(suffixforphotos);
            StandardPhotosNested();
        }
       
        //StandardPhotosNested();
        //document.Add(tablestandardphotos);
    }


        void PrintFiles()
    {
        Debug.Log(path);
        if (path == null)
            return;

        if (File.Exists(path))
        {
            Debug.Log("file found");
            //var startInfo = new System.Diagnostics.ProcessStartInfo(path);
            //int i = 0;
            //foreach (string verb in startInfo.Verbs)
            //{
            //    // Display the possible verbs.
            //    Debug.Log(string.Format("  {0}. {1}", i.ToString(), verb));
            //    i++;
            //}
        }
        else
        {
            Debug.Log("file not found");
            return;
        }
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = path;
        //process.StartInfo.Verb = "print";

        process.Start();
        process.WaitForExit();
    }

    //IEnumerator OpenPDF()
    //{
    //    yield return new WaitForSeconds(1.0f);

    //    string templatePDFName = "InspectionReport.pdf";
    //    var pathToTemplatePDF = Path.Combine(Application.persistentDataPath, templatePDFName);
    //    Application.OpenURL(Application.persistentDataPath + "/2_SIRE_2024-04-29T152706" + "/InspectionReport.pdf");
    //    //AndroidOpenUrl.OpenFile(pathToTemplatePDF);

    //    #if UNITY_EDITOR
    //            Application.OpenURL(Application.persistentDataPath + "/2_SIRE_2024-04-29T152706" + "/InspectionReport.pdf");
    //    #endif
    //}

    public void StandardPhotosNested()
    {
       
        string attachmentsfolderpath1 = "";
        //string suffixforphotos = "";
        string query = "";
        int RemainingMedia = 0;
        int loop_var = 0;
        var cellcontentobs5 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

        if (StandardPhotoHdrs.Count > 0)

        {
            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection1 = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text));

            attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

            table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
            using var connection2 = mlocationdb2.getConnection();
            string ObsDBid = "0";
            query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(ObsDBid) + "'" + " and Attachment_Name like '%" + suffixforphotos + "%';";

            Debug.Log(query.ToString());

            using System.Data.IDataReader reader = mlocationdb2.SelectDataByquery(query);

            List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();

            while (reader.Read())
            {
                Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
                int.Parse(reader[0].ToString()),
                int.Parse(reader[1].ToString()),
                int.Parse(reader[2].ToString()),
                reader[3].ToString().Trim(),
                reader[4].ToString(),
                reader[5].ToString().Trim(),
                reader[6].ToString().Trim(),
                reader[7].ToString().Trim(),
                reader[8].ToString().Trim(),
                reader[9].ToString().Trim(),
                reader[10].ToString().Trim(),
                reader[11].ToString().Trim());

                //Debug.Log("Stock Code: " + entity._stocksym);
                myList.Add(entity);

                var output1 = JsonUtility.ToJson(entity, true);

            }
            reader.Dispose();
            List<String> MediaFiles = new List<string>();
            List<String> Files = new List<string>();

            if (myList.Count != 0)
            {

                foreach (var x in myList)
                {
                    if (x._Attachment_Title.Trim() == "Media")
                    {
                        MediaFiles.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
                    }
                    else if (x._Attachment_Title.Trim() == "File")
                        Files.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
                }
            }

            else
            {
                Debug.Log("There are no files associated to this observation. ");
            }
            var mediafileCount = MediaFiles.Count;
            var filesCount = Files.Count;


            if (mediafileCount > 0)
            {
                RemainingMedia = mediafileCount;


                if (mediafileCount % 8 == 0)
                {
                    loop_var = mediafileCount / 8;
                }
                else
                {
                    loop_var = Convert.ToInt32(Math.Floor(Convert.ToDouble(mediafileCount / 8)) + 1);
                }

                for (int i = 0; i < loop_var; i++)
                {

                    tablestandardphotos = new PdfPTable(4);
                    tablestandardphotos.WidthPercentage = 100;
                    //tablestandardphotos.SpacingBefore = 15;
                    tablestandardphotos.SetWidths(new float[] { 1, 3, 1, 1 });
                    tablestandardphotos.HorizontalAlignment = Element.ALIGN_CENTER;

                    tablestandardphotos.SplitLate = false; // Ensures rows move to the next page if they don't fit entirely
                    tablestandardphotos.SplitRows = true;

                    PdfPTable table1 = new PdfPTable(2);
                    table1.SplitLate = false;
                    table1.SplitRows = true;
                    
                    List<string> temp_media = new List<string>();
                    int cnt_temp = 0;
                    foreach (var media in MediaFiles)
                    {

                        if (((i + 1) * 8) > cnt_temp && (i * 8) <= cnt_temp)
                            temp_media.Add(media);
                        cnt_temp++;
                    }
                    
                    foreach (var media in temp_media)
                    {
                        if (File.Exists(media.ToString()))
                        {
                            //tablecell = new PdfPCell(new Phrase(new Phrase("")));
                            var Obsimagepath = media.ToString();
                            //var Obsimage = iTextSharp.text.Image.GetInstance(Obsimagepath);

                            FileInfo fileInfo = new FileInfo(Obsimagepath);
                            long fileSizeInBytes = fileInfo.Length; // Get file size in bytes
                            int percentageMultiplier = 0;

                            if (fileSizeInBytes > 200 * 1024) // 100 KB in bytes
                            {
                                float sizeInKB = fileSizeInBytes / 1024f; // Convert to KB
                                percentageMultiplier = (int)Math.Round((200f / sizeInKB) * 100); // Find percentage multiplier

                                Debug.Log($"File size is {sizeInKB:F2} KB. Reduce to 200 KB with multiplier: {percentageMultiplier:F2}");
                            }
                            else
                            {
                                percentageMultiplier = 100;
                                Debug.Log("File size is already less than or equal to 200 KB.");
                            }

                            Texture2D Mediafile = NativeGallery.LoadImageAtPath(Obsimagepath, 1024, false);
                            iTextSharp.text.Image Obsimage = ConvertTextureToPdfImage(Mediafile, percentageMultiplier);
                            Obsimage.ScaleAbsolute(200f, 150f);
                            //PdfPCell cell_1 = new PdfPCell(Obsimage) { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 15 };
                            //    cell_1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //    //cell_1.AddElement(Obsimage);
                            //    cell_1.Border = Rectangle.NO_BORDER;
                            //    cell_1.AddElement(new Chunk(Path.GetFileName(media).Split("_Media_")[1], cellcontentobs5));

                            //    table1.AddCell(cell_1);

                            Phrase phrase = new Phrase();

                            // Add the image to the phrase
                            phrase.Add(new Chunk(Obsimage, 0, 0, true));

                            // Add a line break and then the text
                            phrase.Add(Chunk.NEWLINE);  // Add a line break

                            Match ratingMatch = Regex.Match(media, @"Rating_[0-5]");

                            string rating = ratingMatch.Success ? ratingMatch.Value : "Rating_Not_Found";

                            phrase.Add(new Chunk(Path.GetFileName(media).Split("_Media_")[1]+"-"+rating, cellcontentobs5));  // Add the image name as text

                            // Step 5: Add the Phrase to a PdfPCell
                            PdfPCell cell = new PdfPCell(phrase);
                            //cell.Border = PdfPCell.NO_BORDER; // Optional: Remove border
                            cell.HorizontalAlignment = Element.ALIGN_CENTER; // Center align image and text

                            table1.AddCell(cell);

                            RemainingMedia -= 1;

                            if ((RemainingMedia == 0 || temp_media.Count == 1) && temp_media.Count % 2 != 0)
                            {
                                blankcell.Colspan = 1;
                                table1.AddCell(blankcell);
                            }

                            else
                            {
                                Debug.Log("Even Number of media files.");
                            }

                            //    FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
                            //    FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
                            //    FilesViewImgAccordion.transform.GetComponent<RawImage>().texture = thumbnail;
                            //    FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "MediaFiles" + "/";
                            //    FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media);
                            //    FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media).Split("_Media_")[1];
                            //    //Users
                            //    if (Path.GetFileName(media).Contains("Rating"))
                            //    {
                            //        FilesViewImgAccordion.transform.Find("RatingBk/Rating").GetComponent<TextMeshProUGUI>().text = "Rated; " + Path.GetFileName(media).Split("_Media_")[0].Split("Rating_")[1];
                            //    }

                            //    else
                            //    {
                            //        FilesViewImgAccordion.transform.Find("RatingBk/Rating").GetComponent<TextMeshProUGUI>().text = "No Rating ";
                            //    }

                            //    FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                            //    FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();
                        }
                        else
                            Debug.Log("There was no file of this name; " + media.ToString());
                    }

                    PdfPCell AddingNested = new PdfPCell();
                    AddingNested.Colspan = 4;
                    AddingNested.CellEvent = new RoundedBorderObservationcontent();
                    AddingNested.Border = Rectangle.NO_BORDER;
                    AddingNested.AddElement(table1);
                    tablestandardphotos.AddCell(AddingNested);
                    if (mediafileCount < 3)
                    {
                        tablestandardphotos.KeepTogether = true;
                    }
                    else
                    {
                        tablestandardphotos.KeepTogether = false;
                    }

                    document.Add(tablestandardphotos);
                    if (loop_var != i + 1)
                    {
                        document.NewPage();
                    }

                }

            }
            else
            {
                PdfPTable table1 = new PdfPTable(2);
                table1.SplitLate = false;
                table1.SplitRows = true;

                PdfPCell cell_1 = new PdfPCell { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 15 };
                cell_1.HorizontalAlignment = Element.ALIGN_MIDDLE;

                cell_1.AddElement(new Chunk("None.", cellcontentobs5));
                cell_1.Colspan = 2;
                cell_1.Border = Rectangle.NO_BORDER;
                table1.AddCell(cell_1);

                PdfPCell AddingNested = new PdfPCell();
                AddingNested.Colspan = 4;
                AddingNested.CellEvent = new RoundedBorderObservationcontent();
                AddingNested.Border = Rectangle.NO_BORDER;
                AddingNested.AddElement(table1);
                tablestandardphotos.AddCell(AddingNested);
                tablestandardphotos.KeepTogether = true;
                document.Add(tablestandardphotos);
            }

            
               // mlocationdb1.close();
                mlocationdb2.close();
            }
        
        else
        {
            Debug.Log("No Media Files Found in the folder.");
        }

            //document.Add(tablestandardphotos);
        

    }

   
    public void OpenPDFDISCARD(string reportpath)
    {

#if UNITY_EDITOR

        if (File.Exists(reportpath))
        {
            System.Diagnostics.Process.Start(reportpath);
        }
        else
        {
            return;
        }
        #elif UNITY_IPHONE

        //string sourceFilePath = Path.Combine(Application.persistentDataPath, InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf");
        string destinationFilePath = Path.Combine(GetiOSDocumentsPath(), "InspectionReport.pdf");

        CopyFileToNativeDirectory(reportpath, destinationFilePath);

//#elif UNITY_ANDROID
//         if (File.Exists(filePath))
//        {
//            // Open the PDF file using the default PDF viewer
//            Application.OpenURL(path);
//        }
//        else
//        {
//            Debug.LogError("File not found: " + filePath);
//        }          
#endif
    }

        void CopyFileToNativeDirectory(string sourcePath, string destinationPath)
    {
        try
        {
            // Ensure the source file exists
            if (File.Exists(sourcePath))
            {
                // Ensure the destination directory exists
                string destinationDirectory = Path.GetDirectoryName(destinationPath);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Copy the file
                File.Copy(sourcePath, destinationPath, overwrite: true);
                Debug.Log("File copied successfully to " + destinationPath);
            }
            else
            {
                Debug.LogError("Source file does not exist: " + sourcePath);
            }
        }
        catch (IOException ioEx)
        {
            Debug.LogError("IO Exception: " + ioEx.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    string GetiOSDocumentsPath()
    {
        string documentsPath = null;

#if UNITY_IOS
        // Fetch the Documents path on iOS
        documentsPath = Path.Combine(UnityEngine.Application.persistentDataPath, "../Documents");
        documentsPath = Path.GetFullPath(documentsPath);
#endif

        return documentsPath;
    }




public void OpenPDF(string reportpath)
    {

#if UNITY_EDITOR

        if (File.Exists(path))
        {
            System.Diagnostics.Process.Start(path);
            Application.OpenURL(path);
            
        }
        else
        {
            return;
        }
//#elif UNITY_IPHONE

////File.Copy(OriginPath, destination, true);

//  //string reportpath1 = Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf";
//  string reportpath1 = InspectionsInfo.Folderpath.ToString().Trim() + "/InspectionReport.pdf";
//  iOSOpenURL opener = new iOSOpenURL();
//  opener.OpenPDF(reportpath1);
        
#elif UNITY_IPHONE||UNITY_ANDROID
         if (File.Exists(path))
        {
            // Open the PDF file using the default PDF viewer
           
        //AndroidContentOpenerWrapper.OpenContent(Application.persistentDataPath + InspectionsInfo.Folderpath.ToString() + "/InspectionReport.pdf");
     // Use NativeShare to share the file
            new NativeShare()
                .AddFile(path) // Attach the PDF file
                .SetSubject("Sharing PDF") // Optional: Set the subject
                .SetText("Please check out this PDF!") // Optional: Add a message
                .SetTitle("Share via") // Optional: Set the title of the share dialog
                .Share(); // Invoke the share dialog
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }          
#endif



    }
}



public partial class HeaderFooter : PdfPageEventHelper
{
    public RawImage logo;
    public string reportstaus;
    public RawImage Companylogo;
    public RawImage AlternateToCompanylogo;
    RawImage HeaderLine;
    RawImage FooterLine;
    public string VesselName; // field from above class
    public string VesselImo; // field from above class
    public string Inspectedby; // field from above class
    public string  Inspectiondate;// field from above class
    public string InspectionType; // field from above class
    protected float tableHeight;
    protected PdfPTable HeaderTbl;
    PdfPCell tablecell = new PdfPCell();

    public override void OnStartPage(PdfWriter writer, Document doc)
    {
        if (writer.PageNumber > 1)

        {
            base.OnStartPage(writer, doc);
            HeaderTbl = new PdfPTable(1);
            PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));

            var DraftFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,18, iTextSharp.text.Font.NORMAL,iTextSharp.text.BaseColor.RED);
            var headerfont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,9, iTextSharp.text.Font.NORMAL);
            var headerfont2 = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);
            // Set logo position and size
            iTextSharp.text.Image mylogo = null; 

            if (Companylogo.texture != null)
            {
                Texture2D LogoTexture = (Texture2D)Companylogo.texture;
                
                mylogo = ConvertTextureToPdfImage(LogoTexture);
               
            }
            else
            {
                Texture2D CoverPageLogo = (Texture2D)AlternateToCompanylogo.texture;
                mylogo = ConvertTextureToPdfImage(CoverPageLogo);

            }
            mylogo.SetAbsolutePosition(doc.LeftMargin + 20, doc.Top -10); // Adjust as needed
            mylogo.ScaleToFit(30, 30); // Resize the logo
           
            // Create the header table with 3 columns
            HeaderTbl = new PdfPTable(3);
            HeaderTbl.TotalWidth = 575f; // Adjust the total width of the header table
            HeaderTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            // Set column widths
            float[] columnWidths = { 250f, 150f, 175f }; // Custom widths
            HeaderTbl.SetWidths(columnWidths);
            //Document Name Cell
            PdfPCell docNameCell = new PdfPCell(new Phrase(""));

            Chunk chunk1 = new Chunk(InspectionType, headerfont1);
            Chunk chunk2 = new Chunk(Environment.NewLine + VesselName + " (" + VesselImo + ")", headerfont2);

            // Create a Phrase and add both Chunks
            Phrase phrase = new Phrase();
            phrase.Add(chunk1);
            phrase.Add(chunk2);
            
            docNameCell = new PdfPCell(phrase);
            docNameCell.Border = Rectangle.NO_BORDER;
            docNameCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            docNameCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeaderTbl.AddCell(docNameCell);

            //Document Name Cell
            PdfPCell reportstatus = new PdfPCell(new Phrase(""));
            Chunk chunk3 = new Chunk("", DraftFont);

            if (reportstaus == "D" || reportstaus == "N")
            {
               chunk3 = new Chunk("DRAFT REPORT", DraftFont);
            }
            else

            {
                chunk3 = new Chunk("", DraftFont);
            }

            // Create a Phrase and add both Chunks
            Phrase phrase1 = new Phrase();
            phrase1.Add(chunk3);

            reportstatus = new PdfPCell(phrase1);
            reportstatus.Border = Rectangle.NO_BORDER;
            reportstatus.VerticalAlignment = Element.ALIGN_MIDDLE;
            reportstatus.HorizontalAlignment = Element.ALIGN_CENTER;
            HeaderTbl.AddCell(reportstatus);

            // Logo Cell
            PdfPCell logoCell = new PdfPCell(mylogo);
            logoCell.Border = Rectangle.NO_BORDER;
            logoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            HeaderTbl.AddCell(logoCell);

            //PdfPCell cell = new PdfPCell();
            //cell.Border = PdfPCell.NO_BORDER;
            //cell.FixedHeight = 20f;  // Set the height of the cell

            PdfContentByte canvas = writer.DirectContent;
            canvas.SetLineWidth(4f); // Line thickness
            canvas.MoveTo(doc.LeftMargin-10, doc.Top +10); // Starting point
            canvas.LineTo(doc.Right+10, doc.Top + 10); // Ending point
            canvas.SetRGBColorStroke(9, 46, 72); // Blue color (R, G, B)
            //canvas.Rectangle(50f, 50f, 600f, 2f); // Starting point
                                                  // Ending point
            canvas.Stroke();

            
            canvas.SetLineWidth(2f); // Line thickness
            canvas.MoveTo(doc.LeftMargin - 10, doc.Top + 5); // Starting point
            canvas.LineTo(doc.Right + 10, doc.Top + 5); // Ending point
            canvas.SetRGBColorStroke(211, 211, 211); // Grey color (R, G, B)
                                                 //canvas.Rectangle(50f, 50f, 600f, 2f); // Starting point
                                                 // Ending point
            canvas.Stroke();

            HeaderTbl.WriteSelectedRows(0, -1, 10, 825, writer.DirectContent);

        }
    }

    public override void OnEndPage(PdfWriter writer, Document doc) //For adding Page Number to each page
    {

        var footerfnt1 = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);
        var footerfnt2 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, iTextSharp.text.Font.NORMAL);

        if (writer.PageNumber > 1)
        //Page Number

        {
            //Report Date, Copyright.


            PdfPTable footerTbl1 = new PdfPTable(1);

            PdfPCell docNameCell = new PdfPCell(new Phrase(""));

            Chunk chunk1 = new Chunk("Inspected By: ", footerfnt1);
            Chunk chunk2 = new Chunk(Inspectedby + " (" + Inspectiondate + ")", footerfnt2);

            // Create a Phrase and add both Chunks
            Phrase phrase = new Phrase();
            phrase.Add(chunk1);
            phrase.Add(chunk2);

            //string ReportExportedDate = "Report Extracted: " + DateTime.Now.ToString("dd MMM yyyy hh:mm") + " Hrs " + System.Environment.NewLine + "Copyright @ 2024 Orion Marine Concepts";

            Paragraph footer1 = new Paragraph(phrase);

            //Paragraph footer1 = new Paragraph(ReportExportedDate, basefont);
            footer1.Alignment = Element.ALIGN_LEFT;

            footerTbl1.TotalWidth = 200;
            //float footerHeight = 50;
            footerTbl1.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell2 = new PdfPCell(footer1);
            cell2.Border = 0;
            cell2.PaddingLeft = 10;
            footerTbl1.AddCell(cell2);
            footerTbl1.WriteSelectedRows(0, -1, 10, 25, writer.DirectContent);
            //float footerY = doc.BottomMargin - footerHeight;
            //footerTbl1.WriteSelectedRows(0, -1, doc.LeftMargin, footerY + footerTbl1.TotalHeight, writer.DirectContent);

            //Company Website Link.

            PdfPTable footerTbl2 = new PdfPTable(2);
            footerTbl2.TotalWidth = 250;
            footerTbl2.SetWidths(new float[] { 50f, 200f });
            // Set logo position and size

            //logo = GameObject.FindGameObjectWithTag("logo").GetComponent<RawImage>(); // Replace with your logo path
            Texture2D LogoTexture = (Texture2D)logo.texture;
            //iTextSharp.text.Jpeg mylogo = new Jpeg(LogoTexture.EncodeToJPG());

            iTextSharp.text.Image mylogo = ConvertTextureToPdfImage(LogoTexture);

            mylogo.ScaleToFit(40f, 40f); // Adjust the logo size
            //mylogo.SetAbsolutePosition(doc.Right, doc.Top);

            // Logo Cell
            PdfPCell logoCell = new PdfPCell(mylogo);
            logoCell.Border = Rectangle.NO_BORDER;
            logoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            footerTbl2.AddCell(logoCell);

            var Linkfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7, iTextSharp.text.Font.UNDERLINE, BaseColor.BLUE);
            Anchor anchor = new Anchor("https://www.orionmarineconcepts.com", Linkfont);
            anchor.Reference = "https://www.orionmarineconcepts.com";
            Paragraph footer2 = new Paragraph("Powered by: " + anchor.Reference, Linkfont);
            footer2.Alignment = Element.ALIGN_CENTER;

            
            //footerTbl2.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell3 = new PdfPCell(footer2);
            cell3.Border = Rectangle.NO_BORDER;
            cell3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell3.PaddingLeft = 10;
            footerTbl2.AddCell(cell3);

            footerTbl2.WriteSelectedRows(0, -1, 225, 25, writer.DirectContent);


            PdfPTable footerTbl = new PdfPTable(1);
            string pagenumber = "Page - " + writer.PageNumber.ToString();
            Paragraph footer = new Paragraph(pagenumber, footerfnt2);
            footer.Alignment = Element.ALIGN_RIGHT;

            footerTbl.TotalWidth = 200;

            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cell = new PdfPCell(footer);
            cell.Border = 0;
            cell.PaddingLeft = 10;
            footerTbl.AddCell(cell);
            footerTbl.WriteSelectedRows(0, -1, 540, 25, writer.DirectContent);
            //footerTbl1.WriteSelectedRows(0, -1, doc.left(),doc.top() + ((doc.topMargin() + tableHeight) / 2), writer.DirectContent);


            // Draw a line above the footer
            PdfContentByte canvas = writer.DirectContent;
            //canvas.MoveTo(doc.LeftMargin, doc.PageSize.Height - doc.TopMargin + 45f);
            canvas.SetLineWidth(1f); // Line thickness
            canvas.SetRGBColorStroke(9, 46, 72); // Green color (R, G, B)
            canvas.MoveTo(doc.LeftMargin, 30); // Starting point
            canvas.LineTo(doc.Right, 30); // Ending point
            //canvas.LineTo(doc.PageSize.Width - doc.RightMargin, doc.PageSize.Height - doc.TopMargin + 45f);
            canvas.Stroke();

        }

        
    }

    public float getTableHeight()
    {
        return tableHeight;
    }

    iTextSharp.text.Image ConvertTextureToPdfImage(Texture2D texture)
    {
        if (texture == null)
        {
            return null;
        }

        // Convert Texture2D to byte array (PNG format)
        byte[] imageBytes = texture.EncodeToJPG();

        // Check if the imageBytes array is null or empty
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return null;
        }

        // Try-catch block to handle any potential exceptions
        try
        {
            // Convert byte array to iTextSharp Image
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imageBytes);
            //pdfImage.SetDpi(100,100);
            return pdfImage;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}

public class RoundedBorderChapters : IPdfPCellEvent
{
    public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] canvas)
    {

        PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        BaseColor Colorfillchapters = new iTextSharp.text.BaseColor(211, 226, 253); // DARK BLUE FOR Obse level heading.
        //BaseColor ColorfillObservations = new iTextSharp.text.BaseColor(21, 70, 116); // DARK BLUE FOR Obse level heading.

        cb.RoundRectangle(
          rect.Left - 2f,
          rect.Bottom - 4f,
          rect.Width + 0,
        rect.Height + 5, 7 //This controls the round off..
        );

        //PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        //cb.RoundRectangle(
        //  rect.Left - 2f,
        //  rect.Bottom - 2f,
        //  rect.Width + 4,
        //  rect.Height + 4, 8

        //);
        cb.SetColorFill(Colorfillchapters);
        cb.Fill();
        cb.SetLineWidth(1f);
        cb.SetCMYKColorStrokeF(0f, 0f, 0f, 1f);
        cb.Stroke();

    }
}


public class RoundedBorderObservations : IPdfPCellEvent
{
    public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] canvas)
    {

        PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        //BaseColor Colorfillchapters = new iTextSharp.text.BaseColor(241, 243, 248); // DARK BLUE FOR Obse level heading.
        BaseColor ColorfillObservations = new iTextSharp.text.BaseColor(21, 70, 116); // DARK BLUE FOR Obse level heading.

        cb.RoundRectangle(
          rect.Left - 2f,
          rect.Bottom - 4f,
          rect.Width + 0,
        rect.Height + 5, 0 //This controls the round off..
        );

        //cb.CurveTo(400f, 700f, 400f - 20f, 700f, 400f - 20f, 700f);
        //cb.CurveTo(100f, 700f, 100f, 700f - 20f, 100f, 700f - 20f);


        cb.SetColorFill(ColorfillObservations);
        cb.Fill();
        cb.SetLineWidth(1f);
        cb.SetCMYKColorStrokeF(0f, 0f, 0f, 1f);
        cb.Stroke();



    }
}

public class RoundedBorderObservationcontent : IPdfPCellEvent
{
    public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] canvas)
    {

        PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        BaseColor Colorfillchapters = new iTextSharp.text.BaseColor(255, 252, 252); // Light grey.
        //BaseColor ColorfillObservations = new iTextSharp.text.BaseColor(21, 70, 116); // DARK BLUE FOR Obse level heading.

        cb.RoundRectangle(
          rect.Left - 2f,
          rect.Bottom - 4f,
          rect.Width + 0,
        rect.Height + 5, 0 //This controls the round off..
        );

        //PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        //cb.RoundRectangle(
        //  rect.Left - 2f,
        //  rect.Bottom - 2f,
        //  rect.Width + 4,
        //  rect.Height + 4, 8

        //);
        cb.SetColorFill(Colorfillchapters);
        cb.Fill();
        cb.SetLineWidth(1f);
        cb.SetCMYKColorStrokeF(0f, 0f, 0f, 0f); //https://colorizer.org/
        cb.Stroke();



    }
}

public class MediaFilesholder : IPdfPCellEvent
{
    public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] canvas)
    {

        PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        BaseColor Colorfillchapters = new iTextSharp.text.BaseColor(241, 243, 249); 
        //BaseColor ColorfillObservations = new iTextSharp.text.BaseColor(21, 70, 116); // DARK BLUE FOR Obse level heading.

        cb.RoundRectangle(
          rect.Left - 2f,
          rect.Bottom - 4f,
          rect.Width + 0,
        rect.Height + 5, 0 //This controls the round off..
        );

        //PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
        //cb.RoundRectangle(
        //  rect.Left - 2f,
        //  rect.Bottom - 2f,
        //  rect.Width + 4,
        //  rect.Height + 4, 8

        //);
        cb.SetColorFill(Colorfillchapters);
        cb.Fill();
        cb.SetLineWidth(0.5f);
        cb.SetCMYKColorStrokeF(0.846f, 0.462f, 0f, 0.49f);
        cb.Stroke();

    }
}