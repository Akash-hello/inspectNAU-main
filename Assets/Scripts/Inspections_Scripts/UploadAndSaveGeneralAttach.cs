using DataBank;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadAndSaveGeneralAttach : MonoBehaviour
{
    public string OriginPath;
    public GameObject attachmentpanel;
    string filename;
    string FileExt;
    Double Filesize;
    int ObsDBid;
    public TextMeshProUGUI InspPrimaryId;
    public TextMeshProUGUI AttachmentFolderpath;

    public TextMeshProUGUI attachmentfilename;
    public RawImage m_RawImage;
    //Select a Texture in the Inspector to change to
    //public Texture m_Texture;
    //System.Drawing.Image compressedphoto;
    //Bitmap Photo;
    bool Media;
    public GameObject defaulticon;
    string attachmentsfolderpath;
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 0.0f;
    public GameObject fileviewerobj;
    public TextMeshProUGUI NoAttchdTxt;
    string mediafilespath;
    string documentspath;

    public GameObject FilesViewImgAccordion;
    public GameObject FilesViewImgPrefab;
    public RectTransform ParentPanel;

    public void OpenFileUploader()
    {

        if (InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString() != "" && int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()) != 0)

        {
            attachmentfilename.text = "";
            filename = "";
            m_RawImage.GetComponent<RawImage>().texture = null;
            FileExt = "";
            Filesize = 0;
            //attachmentsfolderpath = "";
            Media = false;

            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection2 = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

            attachmentsfolderpath = mlocationdb1.folderpath.ToString();

            attachmentpanel.SetActive(true);
            StartCoroutine(Fileviewer());
        }

        else
        {
            errorpopup.gameObject.SetActive(true);
           
            PopUpMsg.text = "This function is available once vessel name and imo number are saved.";
            StartCoroutine(HidePopUp());
            NoAttchdTxt.gameObject.SetActive(true);
            fileviewerobj.SetActive(true);
        }
           
    }

    public void activatefileviewer()
    {
        StartCoroutine(Fileviewer());
    }

    public void uploadfile()
    {
        time = 2.0f;
        filename = "";
        m_RawImage.GetComponent<RawImage>().texture = null;
        attachmentfilename.text = "";
        FileExt = "";
        Filesize = 0;
        Media = false;
        defaulticon.SetActive(false);
        //attachmentsfolderpath = AttachmentFolderpath.GetComponent<TextMeshProUGUI>().text.ToString();

        string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpg,jpeg,pdf,xls,xlsx,doc,docx");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation Cancelled");
            else

            {
                OriginPath = path;
                filename = Path.GetFileName(path);
                attachmentpanel.SetActive(true);
                long filesize = new System.IO.FileInfo(OriginPath).Length;
                Filesize = (filesize / 1024 ^ 2) / 1024;
                if (Filesize <= 1)
                {
                    attachmentfilename.text = filename + ", (" + " <1 MB)";
                }
                if (Filesize > 1)
                {
                    attachmentfilename.text = filename + ", (" + Filesize + " MB)";
                }
                FileExt = Path.GetExtension(OriginPath).ToLower();
                
                Debug.Log("Picked File: " + OriginPath+"-"+ filename+"Size;" + Filesize +" MB");

                if (FileExt == ".jpeg"|| FileExt == ".jpg"|| FileExt == ".png")
                {
                   Media = true;
                }

                StartCoroutine(LoadTexture(OriginPath, m_RawImage));
            }
        }, //new string[] { FileType }) // Commented on 22nd May as Android was giving message; "No Apps can perofrm this action" as per https://forum.unity.com/threads/native-file-picker-for-android-ios-open-source.912890/page-3
           NativeFilePicker.ConvertExtensionToFileType("png"), NativeFilePicker.ConvertExtensionToFileType("jpg"), NativeFilePicker.ConvertExtensionToFileType("jpeg"),
           NativeFilePicker.ConvertExtensionToFileType("pdf"), NativeFilePicker.ConvertExtensionToFileType("xls"), NativeFilePicker.ConvertExtensionToFileType("xlsx"), NativeFilePicker.ConvertExtensionToFileType("doc")
           , NativeFilePicker.ConvertExtensionToFileType("docx"),NativeFilePicker.ConvertExtensionToFileType("mov"));

    }

    IEnumerator LoadTexture(String originpath ,RawImage image)
    {
        //if (Media == true)
        //{
        //    WWW www = new WWW(OriginPath);
        //    while (!www.isDone)
        //        yield return null;
        //    m_RawImage.GetComponent<RawImage>().texture = www.texture;
        //}

        //else
        //{
        //    defaulticon.SetActive(true);
        //}

        

        if (Media == true)
        {
            //UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///Users/mohitsabharwal/Documents/D/PhotoGraphs/ALL_PHOTOGRAPHS/2_FOR_PRINTING/DSC02228.JPG");
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(originpath);

            request.uri = new Uri(request.uri.AbsoluteUri.Replace("https://localhost", "file://"));

            yield return request.SendWebRequest();

            //while (!request.isDone)

            //yield return request;

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Successfully Downloaded image.");
                var texture = DownloadHandlerTexture.GetContent(request);
                image.GetComponent<RawImage>().texture = texture;

            }

            //image.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        }

        else
        {
            defaulticon.SetActive(true);
        }


    }

    public void Savefile()
    {
        if (!Directory.Exists(Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + "GeneralAttachments"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + "GeneralAttachments");
        }
        //Media = true;
        //filename = "Testphoto.jpg";
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection2 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        
        int ID = 0;
        ObsDBid = 0;
        string Attachment_Title = "";
        string Attachment_Name = "";
        string timesuffixforname = "";
        string Attachment_Path = "";
        string Attachment_Details_1 = "";
        string Attachment_Details_2 = "";
        string Attachment_Details_3 = "";
        string Attachment_Details_4 = "";
        string Active = "";
        time = 2.0f;

        table_Inspection_Attachments mlocationDb = new table_Inspection_Attachments();
        using var connection = mlocationDb.getConnection();

        if (!String.IsNullOrEmpty(filename) && Media == false)
        {
            timesuffixforname = DateTime.Now.ToString("s").Replace(":", "");
            string destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + "GeneralAttachments" + "/" + timesuffixforname + "_GenFile_" + filename.Replace("'", "");

            Attachment_Title = "GenFile";
            Attachment_Name = timesuffixforname + "_GenFile_" + filename.Replace("'", "");
            Attachment_Path = attachmentsfolderpath + "/" + "General" + "/" + "GeneralAttachments" + "/";
            Attachment_Details_1 = FileExt;
            Attachment_Details_2 = Filesize.ToString();
            Attachment_Details_3 = "";
            Attachment_Details_4 = "";
            Active = "Y";


            File.Copy(OriginPath, destination, true);
            //NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(destination, (success) =>Debug.Log("File Exported: " + success));

            mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()),
                ObsDBid,
                Attachment_Title,
                Attachment_Name,
                Attachment_Path,
                Attachment_Details_1,
                Attachment_Details_2,
                Attachment_Details_3,
                Attachment_Details_4,
                Active,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

            errorpopup.gameObject.SetActive(true);
            defaulticon.SetActive(false);
            PopUpMsg.text = "Attachment saved.";
            StartCoroutine(HidePopUp());
            StartCoroutine(Fileviewer());
            RefreshBtn();
        }
        else if (!String.IsNullOrEmpty(filename) && Media == true)
        {
            Texture2D itemBGTex = m_RawImage.texture as Texture2D;

            byte[] itemBGBytes = itemBGTex.EncodeToJPG(10);

            timesuffixforname = DateTime.Now.ToString("s").Replace(":", "");
            string destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + "GeneralAttachments" + "/" + timesuffixforname + "_GenMedia_" + filename.Replace("'", "");

            Attachment_Title = "GenMedia";
            Attachment_Name = timesuffixforname + "_GenMedia_" + filename.Replace("'", "");
            Attachment_Path = attachmentsfolderpath + "/" + "General" + "/" + "GeneralAttachments" + "/";
            Attachment_Details_1 = FileExt;
            Attachment_Details_2 = Filesize.ToString();
            Attachment_Details_3 = "";
            Attachment_Details_4 = "";
            Active = "Y";

            File.WriteAllBytes(destination, itemBGBytes);

            mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()),
                ObsDBid,
                Attachment_Title,
                Attachment_Name,
                Attachment_Path,
                Attachment_Details_1,
                Attachment_Details_2,
                Attachment_Details_3,
                Attachment_Details_4,
                Active,
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")));

            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "File saved.";
            StartCoroutine(HidePopUp());
            StartCoroutine(Fileviewer());
            RefreshBtn();
        }

        else
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "No Media/File Selected.";
            StartCoroutine(HidePopUp());
            Debug.Log("No File Selected.");
        }
        mlocationDb.close();
    }

    public void Cancelupload()
    {
        if (!String.IsNullOrEmpty(filename))
        {
            attachmentfilename.text = "";
            filename = "";
            m_RawImage.GetComponent<RawImage>().texture = null;
            FileExt = "";
            Filesize = 0;
            attachmentsfolderpath = "";
            Media = false;
        }
        attachmentpanel.SetActive(false);
    }

    public void RefreshBtn()
    {
        if (!String.IsNullOrEmpty(filename))
        {
            attachmentfilename.text = "";
            filename = "";
            m_RawImage.GetComponent<RawImage>().texture = null;
            FileExt = "";
            Filesize = 0;
            //attachmentsfolderpath = "";
            Media = false;
        }
    }

   public void RotateBtn()
    {
        if (!String.IsNullOrEmpty(filename))
        {
            m_RawImage.gameObject.transform.Rotate(new Vector3(0, 0, 90), 90);
        }

        else
            Debug.Log("Nothing to rotate.");
    }


    IEnumerator Fileviewer()
    {
        if (InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString() != "" && int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()) != 0)

        { 
            ObsDBid = 0;
            string attachmentsfolderpath1 = "";

            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

            attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

            yield return null;
            for (int i = 0; i < ParentPanel.transform.childCount; ++i)
            {
                Destroy(ParentPanel.transform.GetChild(i).gameObject);
            }

            string query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(InspPrimaryId.text.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + ObsDBid + "'"; //+ " and trim(Attachment_Title) = 'Media'";

            table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
            using var connection2 = mlocationdb2.getConnection();
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
                    if (x._Attachment_Title.Trim() == "GenMedia")
                    {
                        MediaFiles.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
                    }
                    else
                        Files.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
                }
            }

            else
            {
                Debug.Log("There are no files associated to this observation. ");
            }
            var mediafileCount = MediaFiles.Count;
            var filesCount = Files.Count;

            if (mediafileCount != 0 || filesCount != 0)
            {
                NoAttchdTxt.gameObject.SetActive(false);
                fileviewerobj.SetActive(true);
            }
            else
            {
                NoAttchdTxt.gameObject.SetActive(true);
                fileviewerobj.SetActive(true);
            }

            if (mediafileCount > 0)
            {
                foreach (var media in MediaFiles)
                {
                    if (File.Exists(media.ToString()))
                    {
                        Debug.Log("GenMedia Files Found " + media.ToString());

                        byte[] FiletextureBytes = File.ReadAllBytes(media);
                        Texture2D thumbnail = new Texture2D(2, 2);
                        thumbnail.LoadImage(FiletextureBytes);
                        thumbnail.Apply();

                        FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
                        FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
                        FilesViewImgAccordion.transform.GetComponent<RawImage>().texture = thumbnail;
                        FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "General" + "/" + "GeneralAttachments" + "/";
                        FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media);
                        FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media).Split("GenMedia_")[1];
                        FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                        FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.ToString();
                    }
                    else
                        Debug.Log("There was no file of this name; " + media.ToString());
                }

            }
            else
            {
                Debug.Log("No Media Files Found in the folder.");
            }

            if (filesCount > 0)
            {
                foreach (string currentFile in Files)
                {
                    if (File.Exists(currentFile.ToString()))
                    {
                        Debug.Log("Documents Found " + currentFile.ToString());

                        FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
                        FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
                        FilesViewImgAccordion.transform.Find("DefaultImg").gameObject.SetActive(true);
                        FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "General" + "/" + "GeneralAttachments" + "/";
                        FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile);
                        FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile).Split("GenFile_")[1];
                        FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                        FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.ToString();
                    }
                    else
                        Debug.Log("There was no file of this name; " + currentFile.ToString());
                }

            }
            else
            {
                Debug.Log("No Documents Found in the folder.");
            }

            //mlocationdb1.close();
            //mlocationdb2.close();
        }

        else
        {
            NoAttchdTxt.gameObject.SetActive(true);
            for (int i = 0; i < ParentPanel.transform.childCount; ++i)
            {
                Destroy(ParentPanel.transform.GetChild(i).gameObject);
            }
            fileviewerobj.SetActive(true);
        }
    }

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);
        time = 2.0f;
        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }
}
