using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataBank;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
#if UNITY_STANDALONE || UNITY_EDITOR
using SFB; // StandaloneFileBrowser for Windows/macOS
#endif

public class UploadAndSave : MonoBehaviour
{
    public string OriginPath;
    public GameObject attachmentpanel;
    public string filename;
    public string FileExt;
    Double Filesize;
    public TextMeshProUGUI ObsDBid;
    public TextMeshProUGUI InspPrimaryId;
    public TextMeshProUGUI AttachmentFolderpath;

    public TextMeshProUGUI attachmentfilename;
    public RawImage m_RawImage;
    //Select a Texture in the Inspector to change to
    //public Texture m_Texture;
    //System.Drawing.Image compressedphoto;
    //Bitmap Photo;
    public bool Media;
    public GameObject defaulticon;
    string attachmentsfolderpath;
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    //float time = 0.0f;
    public GameObject fileviewerobj;
    string mediafilespath;
    string documentspath;

    public GameObject FilesViewImgAccordion;
    public GameObject FilesViewImgPrefab;
    public RectTransform ParentPanel;

    public GameObject Sliderpanel;
    public Slider Photocondition;
    public TextMeshProUGUI conditionvalue;

    public GameObject ObsContr;
    public bool livephoto;

    public TextMeshProUGUI Questions_Text;
    public GameObject completionstatusindicator;
    public TextMeshProUGUI Mediacount;
    public TMP_InputField filenameforsave;
    public GameObject livebtn;
    public GameObject library;
    public GameObject Filesbtn;
    bool standardphoto;
    public bool fromstandardphotos;

    public GameObject[] speechcubes;
    private bool[] originalStates;

    private void Start()
    {
        // Initialize and store the original states of all SpeechCubes
        originalStates = new bool[speechcubes.Length];
        for (int i = 0; i < speechcubes.Length; i++)
        {
            if (speechcubes[i] != null)
                originalStates[i] = speechcubes[i].activeSelf;
        }

    }


    public void comingfromstandardphotos()
    {
        fromstandardphotos = true;
    }

    public void OpenFileUploader()
    {
        filenameforsave.gameObject.SetActive(false);

        // Deactivate all SpeechCubes
        foreach (var obj in speechcubes)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Kartik - corrected this value from 800 to 469 for better aligning with upload and save panel
        ObsContr.gameObject.GetComponent<LayoutElement>().preferredHeight = 469;
            Sliderpanel.SetActive(false);
            Photocondition.value = 0;
            conditionvalue.text = "-1";

            attachmentfilename.text = "";
            livephoto = false;
            filename = "";
            filenameforsave.text = "";  
            m_RawImage.GetComponent<RawImage>().texture = null;
            FileExt = "";
            Filesize = 0;
            //attachmentsfolderpath = "";
            Media = false;
            livebtn.SetActive(true);
            library.SetActive(true);
        if (fromstandardphotos == true)
        {
            Filesbtn.SetActive(false);
        }

        else
        {
            Filesbtn.SetActive(true);
        }

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        standardphoto = false;
        attachmentpanel.SetActive(true);

        if (!Directory.Exists(Application.persistentDataPath + attachmentsfolderpath + "/" + "General"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + attachmentsfolderpath + "/" + "General");
        }

        if (!Directory.Exists(Application.persistentDataPath + attachmentsfolderpath + "/" + "MediaFiles"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + attachmentsfolderpath + "/" + "MediaFiles");
        }

        if (!Directory.Exists(Application.persistentDataPath + attachmentsfolderpath + "/" + "Filespath"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + attachmentsfolderpath + "/" + "Filespath");
        }

        StartCoroutine(Fileviewer());

    }

    public void uploadphotofmgallery()
    {
        filename = "";
        m_RawImage.GetComponent<RawImage>().texture = null;
        FileExt = "";
        Filesize = 0;
        Media = false;
        defaulticon.SetActive(false);
        livephoto = false;

#if UNITY_STANDALONE || UNITY_EDITOR
        // ----- Windows/macOS (desktop) – pick images only -----
        var extensions = new[]
        {
        new ExtensionFilter("Images", "png","jpg","jpeg","heic")
    };
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an image", "", extensions, false);
        if (paths == null || paths.Length == 0 || string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log("Operation Cancelled");
            return;
        }

        var path = paths[0];

        filename = Path.GetFileName(path);
        long filesizeBytes = new System.IO.FileInfo(path).Length;
        Filesize = (double)filesizeBytes / (1024d * 1024d); // MB
        FileExt = Path.GetExtension(path).ToLowerInvariant();

        // Load image into RawImage
        StartCoroutine(LoadTexture("file://" + path, m_RawImage));

        livebtn.SetActive(false);
        library.SetActive(false);
        Filesbtn.SetActive(false);

        Media = true; // this keeps Savefile() path for media unchanged
        filenameforsave.gameObject.SetActive(true);

        if (Filesize <= 1) attachmentfilename.text = filename + ", (<1 MB)";
        else attachmentfilename.text = filename + ", (" + Filesize.ToString("0.##") + " MB)";

        Sliderpanel.SetActive(true);
        Photocondition.value = 0;
        conditionvalue.text = "";

        defaulticon.SetActive(false);

#else
    // ----- iOS/Android (unchanged) -----
    NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
    {
        Debug.Log("Image path: " + path);
        if (path != null)
        {
            Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
            if (texture == null)
            {
                errorpopup.gameObject.SetActive(true);
                defaulticon.SetActive(false);
                PopUpMsg.text = "No photograph or unsupported file type detected, try another type.";
                StartCoroutine(HidePopUp());
                Debug.Log("Couldn't load texture from " + path);
                return;
            }
            else
            {
                filename = Path.GetFileName(path);
                long filesize = new System.IO.FileInfo(path).Length;
                Filesize = (filesize / 1024 ^ 2) / 1024; // (kept as-is)

                FileExt = Path.GetExtension(path).ToLower();

                Debug.Log("Picked File: " + path + "-" + filename + "Size;" + Filesize + " MB");

                m_RawImage.GetComponent<RawImage>().texture = texture;
                livebtn.SetActive(false);
                library.SetActive(false);
                Filesbtn.SetActive(false);

                Media = true;
                filenameforsave.gameObject.SetActive(true);
                if (Filesize <= 1)
                    attachmentfilename.text = filename + ", (" + " <1 MB)";
                if (Filesize > 1)
                    attachmentfilename.text = filename + ", (" + Filesize + " MB)";

                Sliderpanel.SetActive(true);
                Photocondition.value = 0;
                conditionvalue.text = "";

                defaulticon.SetActive(false);
            }
        }
    });

    Debug.Log("Permission result: " + permission);
#endif
    }


    public void uploadfile()
    {
        // reset
        filename = "";
        m_RawImage.GetComponent<RawImage>().texture = null;
        attachmentfilename.text = "";
        FileExt = "";
        Filesize = 0;
        Media = false;
        defaulticon.SetActive(false);
        livephoto = false;

#if UNITY_STANDALONE || UNITY_EDITOR
        // ----- Windows/macOS (desktop) -----
        var extensions = new[]
        {
        new ExtensionFilter("Images", "png","jpg","jpeg","heic"),
        new ExtensionFilter("Documents", "pdf","xls","xlsx","doc","docx"),
        new ExtensionFilter("Videos", "mov","mp4")
    };
        var paths = StandaloneFileBrowser.OpenFilePanel("Select a file", "", extensions, false);
        if (paths == null || paths.Length == 0 || string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log("Operation Cancelled");
            return;
        }

        filenameforsave.gameObject.SetActive(false);
        OriginPath = paths[0];
        filename = Path.GetFileName(OriginPath);
        attachmentpanel.SetActive(true);

        long filesizeBytes = new System.IO.FileInfo(OriginPath).Length;
        // correct MB conversion; leaves existing mobile math untouched
        Filesize = (double)filesizeBytes / (1024d * 1024d);

        if (Filesize <= 1) attachmentfilename.text = filename + ", (<1 MB)";
        else attachmentfilename.text = filename + ", (" + Filesize.ToString("0.##") + " MB)";

        FileExt = Path.GetExtension(OriginPath).ToLowerInvariant();

        Debug.Log("Picked File: " + OriginPath + "-" + filename + " Size: " + Filesize.ToString("0.##") + " MB");

        // mark media for images (so Savefile() keeps working as-is)
        if (FileExt == ".jpeg" || FileExt == ".jpg" || FileExt == ".png" || FileExt == ".heic")
        {
            Media = true;
            filenameforsave.gameObject.SetActive(true);
        }

        livebtn.SetActive(false);
        library.SetActive(false);
        Filesbtn.SetActive(false);

        // preview only images; use file:// for desktop
        if (Media)
            StartCoroutine(LoadTexture("file://" + OriginPath, m_RawImage));
        else
            defaulticon.SetActive(true);

#else
    // ----- iOS/Android (unchanged) -----
    string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpg,jpeg,pdf,xls,xlsx,doc,docx");

    NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
    {
        if (path == null)
            Debug.Log("Operation Cancelled");
        else
        {
            filenameforsave.gameObject.SetActive(false);
            OriginPath = path;
            filename = Path.GetFileName(path);
            attachmentpanel.SetActive(true);
            long filesize = new System.IO.FileInfo(OriginPath).Length;
            Filesize = (filesize / 1024 ^ 2) / 1024; // (kept as-is to not disturb your mobile logic)

            if (Filesize <= 1)
                attachmentfilename.text = filename + ", (" + " <1 MB)";
            if (Filesize > 1)
                attachmentfilename.text = filename + ", (" + Filesize + " MB)";

            FileExt = Path.GetExtension(OriginPath).ToLower();

            Debug.Log("Picked File: " + OriginPath + "-" + filename + "Size;" + Filesize + " MB");

            if (FileExt == ".jpeg"|| FileExt == ".jpg"|| FileExt == ".png" || FileExt == ".heic")
            {
                Media = true;
                filenameforsave.gameObject.SetActive(true);
            }
            livebtn.SetActive(false);
            library.SetActive(false);
            Filesbtn.SetActive(false);
            StartCoroutine(LoadTexture(OriginPath, m_RawImage));
        }
    },
        NativeFilePicker.ConvertExtensionToFileType("png"),
        NativeFilePicker.ConvertExtensionToFileType("jpg"),
        NativeFilePicker.ConvertExtensionToFileType("jpeg"),
        NativeFilePicker.ConvertExtensionToFileType("pdf"),
        NativeFilePicker.ConvertExtensionToFileType("xls"),
        NativeFilePicker.ConvertExtensionToFileType("xlsx"),
        NativeFilePicker.ConvertExtensionToFileType("doc"),
        NativeFilePicker.ConvertExtensionToFileType("docx"),
        NativeFilePicker.ConvertExtensionToFileType("mov"));
#endif
    }





    //BELOW WORKS FINE AND WAS OLD COMMENTED 30th Aug 2025, replaced with the new for MAC AND WINDOWS STANDALONE included.
    //public void uploadphotofmgallery()
    //{
    //    filename = "";
    //    m_RawImage.GetComponent<RawImage>().texture = null;
    //    FileExt = "";
    //    Filesize = 0;
    //    Media = false;
    //    defaulticon.SetActive(false);
    //    livephoto = false;

    //    NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
    //    {
    //        Debug.Log("Image path: " + path);
    //        if (path != null)
    //        {
    //            // Create Texture from selected image
    //            Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
    //            if (texture == null)
    //            {
    //                //LoadPhotoFmDB();
    //                errorpopup.gameObject.SetActive(true);
    //                defaulticon.SetActive(false);
    //                PopUpMsg.text = "No photograph or unsupported file type detected, try another type.";
    //                StartCoroutine(HidePopUp());
    //                Debug.Log("Couldn't load texture from " + path);
    //                //defaulticon.SetActive(true);
    //                return;
    //            }


    //            //else if (texture != null && Path.GetExtension(path) == ".heic")
    //            //{
    //            //    texture = NativeGallery.LoadImageAtPath(path, markTextureNonReadable: false);
    //            //    string jpegPath = Path.Combine(Application.temporaryCachePath, "tmp.jpg");
    //            //    File.WriteAllBytes(jpegPath, texture.EncodeToJPG());

    //            //    filename = Path.GetFileName(jpegPath);
    //            //    long filesize = new System.IO.FileInfo(jpegPath).Length;
    //            //    Filesize = (filesize / 1024 ^ 2) / 1024;

    //            //    FileExt = Path.GetExtension(jpegPath).ToLower();

    //            //    Debug.Log("Picked File: " + jpegPath + "-" + filename + "Size;" + Filesize + " MB");

    //            //    m_RawImage.GetComponent<RawImage>().texture = texture;

    //            //        Media = true;
    //            //        filenameforsave.gameObject.SetActive(true);
    //            //        if (Filesize <= 1)
    //            //        {
    //            //            attachmentfilename.text = filename + ", (" + " <1 MB)";
    //            //        }
    //            //        if (Filesize > 1)
    //            //        {
    //            //            attachmentfilename.text = filename + ", (" + Filesize + " MB)";
    //            //        }


    //            //        Sliderpanel.SetActive(true);
    //            //        Photocondition.value = 0;
    //            //        conditionvalue.text = "";


    //            //    defaulticon.SetActive(false);

    //            //    DestroyImmediate(texture);
    //            //}

    //            else
    //            {
    //                filename = Path.GetFileName(path);
    //                long filesize = new System.IO.FileInfo(path).Length;
    //                Filesize = (filesize / 1024 ^ 2) / 1024;

    //                FileExt = Path.GetExtension(path).ToLower();

    //                Debug.Log("Picked File: " + path + "-" + filename + "Size;" + Filesize + " MB");

    //                m_RawImage.GetComponent<RawImage>().texture = texture;
    //                livebtn.SetActive(false);
    //                library.SetActive(false);
    //                Filesbtn.SetActive(false);
    //                //if (FileExt == ".jpeg" || FileExt == ".jpg" || FileExt == ".png" || FileExt == ".heic")
    //                //{
    //                Media = true;
    //                    filenameforsave.gameObject.SetActive(true);
    //                    if (Filesize <= 1)
    //                    {
    //                        attachmentfilename.text = filename + ", (" + " <1 MB)";
    //                    }
    //                    if (Filesize > 1)
    //                    {
    //                        attachmentfilename.text = filename + ", (" + Filesize + " MB)";
    //                    }


    //                    Sliderpanel.SetActive(true);
    //                    Photocondition.value = 0;
    //                    conditionvalue.text = "";
    //                //}

    //                defaulticon.SetActive(false);
    //            }

    //        }

    //        //else
    //        //{
    //        //    // User canceled the selection
    //        //    LoadPhotoFmDB();
    //        //}
    //    });

    //    Debug.Log("Permission result: " + permission);

    //} 

    //BELOW WORKS FINE AND WAS OLD COMMENTED 30th Aug 2025, replaced with the new for MAC AND WINDOWS STANDALONE included.
    //public void uploadfile()
    //{
    //    //time = 2.0f;
    //    filename = "";
    //    m_RawImage.GetComponent<RawImage>().texture = null;
    //    attachmentfilename.text = "";
    //    FileExt = "";
    //    Filesize = 0;
    //    Media = false;
    //    defaulticon.SetActive(false);
    //    livephoto = false;
    //    //attachmentsfolderpath = AttachmentFolderpath.GetComponent<TextMeshProUGUI>().text.ToString();

    //    string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpg,jpeg,pdf,xls,xlsx,doc,docx");

    //    NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
    //    {
    //        if (path == null)
    //            Debug.Log("Operation Cancelled");
    //        else

    //        {
    //            filenameforsave.gameObject.SetActive(false);
    //            OriginPath = path;
    //            filename = Path.GetFileName(path);
    //            attachmentpanel.SetActive(true);
    //            long filesize = new System.IO.FileInfo(OriginPath).Length;
    //            Filesize = (filesize / 1024 ^ 2) / 1024;
    //            if (Filesize <= 1)
    //            {
    //                attachmentfilename.text = filename + ", (" + " <1 MB)";
    //            }
    //            if (Filesize > 1)
    //            {
    //                attachmentfilename.text = filename + ", (" + Filesize + " MB)";
    //            }
    //            FileExt = Path.GetExtension(OriginPath).ToLower();

    //            Debug.Log("Picked File: " + OriginPath+"-"+ filename+"Size;" + Filesize +" MB");

    //            if (FileExt == ".jpeg"|| FileExt == ".jpg"|| FileExt == ".png" || FileExt == ".heic")
    //            {
    //               Media = true;
    //                filenameforsave.gameObject.SetActive(true);
    //            }
    //            livebtn.SetActive(false);
    //            library.SetActive(false);
    //            Filesbtn.SetActive(false);
    //            StartCoroutine(LoadTexture(OriginPath, m_RawImage));
    //        }
    //    }, //new string[] { FileType }) // Commented on 22nd May as Android was giving message; "No Apps can perofrm this action" as per https://forum.unity.com/threads/native-file-picker-for-android-ios-open-source.912890/page-3
    //       NativeFilePicker.ConvertExtensionToFileType("png"), NativeFilePicker.ConvertExtensionToFileType("jpg"), NativeFilePicker.ConvertExtensionToFileType("jpeg"),
    //       NativeFilePicker.ConvertExtensionToFileType("pdf"), NativeFilePicker.ConvertExtensionToFileType("xls"), NativeFilePicker.ConvertExtensionToFileType("xlsx"), NativeFilePicker.ConvertExtensionToFileType("doc")
    //       , NativeFilePicker.ConvertExtensionToFileType("docx"), NativeFilePicker.ConvertExtensionToFileType("mov"));

    //}

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

            Sliderpanel.SetActive(true);
            Photocondition.value = 0;
            conditionvalue.text = "";

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Successfully Downloaded image.");
                var texture = DownloadHandlerTexture.GetContent(request);
                image.GetComponent<RawImage>().texture = texture;
                livebtn.SetActive(false);
                library.SetActive(false);
                Filesbtn.SetActive(false);
            }

            //image.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        }

        else
        {
            
            defaulticon.SetActive(true);
        }


    }

    public void Photoconditionslider()
    {
        conditionvalue.text = Photocondition.value.ToString();
    }


    public void filenameforstandardphotos()
    {
        
        Debug.Log(filename.ToString());
        if (!String.IsNullOrEmpty(filename))
        {
            filename = Questions_Text.text.ToString().Split('_')[0] + "_" + filename;
            standardphoto = true;
            Savefile();
        }
       else
        {
            standardphoto = false;
            errorpopup.gameObject.SetActive(true);
            defaulticon.SetActive(false);
            PopUpMsg.text = "No media file found for saving.";
            StartCoroutine(HidePopUp());
        }
        
    }

   
   public void Savefile()
    {

        string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ._-";

        if ((String.IsNullOrEmpty(filenameforsave.text.ToString()) || filenameforsave.text.ToString() == "") && Media == true)
        {
           
            errorpopup.gameObject.SetActive(true);
            defaulticon.SetActive(false);
            PopUpMsg.text = "Please enter a name for saving your media file or photo.";
            StartCoroutine(HidePopUp());
        }

        else if (filenameforsave.text.Any(ch => !allowedChars.Contains(ch)))
        {
            errorpopup.gameObject.SetActive(true);
            defaulticon.SetActive(false);
            PopUpMsg.text = "Filename contains invalid characters! Use only letters, numbers, underscores, dashes, and spaces.";
            StartCoroutine(HidePopUp());

        }


        else
        {
            //Media = true;
            //filename = "Testphoto.jpg";
            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

            attachmentsfolderpath = mlocationdb1.folderpath.ToString();

            int ID = 0;
            string Attachment_Title = "";
            string Attachment_Name = "";
            string timesuffixforname = "";
            string Attachment_Path = "";
            string Attachment_Details_1 = "";
            string Attachment_Details_2 = "";
            string Attachment_Details_3 = "";
            string Attachment_Details_4 = "";
            string Active = "";
            //time = 2.0f;

            table_Inspection_Attachments mlocationDb = new table_Inspection_Attachments();
            using var connection1 = mlocationDb.getConnection();

            if (!String.IsNullOrEmpty(filename) && Media == false)
            {
                //filenameforsave.gameObject.SetActive(false);
                timesuffixforname = DateTime.Now.ToString("s").Replace(":", "");
                string destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "Filespath" + "/" + ObsDBid.text.ToString() + "_" + timesuffixforname + "_File_" + Questions_Text.text.ToString().Split(':')[0] + "_"+filename.Replace("'", "").Replace(" ", "_");

                Attachment_Title = "File";
                Attachment_Name = ObsDBid.text.ToString() + "_" + timesuffixforname + "_File_" + Questions_Text.text.ToString().Split(':')[0] + "_" + filename.Replace("'", "").Replace(" ", "_");
                Attachment_Path = attachmentsfolderpath + "/" + "Filespath" + "/";
                Attachment_Details_1 = FileExt;
                Attachment_Details_2 = Filesize.ToString();
                Attachment_Details_3 = "";
                Attachment_Details_4 = "";
                Active = "Y";


                File.Copy(OriginPath, destination, true);
                //NativeFilePicker.Permission permission = NativeFilePicker.ExportFile(destination, (success) =>Debug.Log("File Exported: " + success));

                mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                    int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()),
                    int.Parse(ObsDBid.text.ToString()),
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
                PopUpMsg.text = "File saved.";
                StartCoroutine(HidePopUp());
                StartCoroutine(Fileviewer());
                RefreshBtn();
            }
            else if (!String.IsNullOrEmpty(filename) && Media == true)
            {
                //filenameforsave.gameObject.SetActive(true);

                if (standardphoto == true)
                {
                    filename = "O1_"+ filenameforsave.text.ToString() + FileExt;
                }

                else
                {
                    filename = Questions_Text.text.ToString().Split(':')[0] + "_" + filenameforsave.text.ToString() + FileExt;
                }
                
                Texture2D itemBGTex = m_RawImage.texture as Texture2D;
                byte[] itemBGBytes;
                float zRotation = m_RawImage.gameObject.transform.eulerAngles.z;

                Debug.Log("Current Z Rotation: " + zRotation);
                

                if (zRotation != 0)
                {
                    
                    Texture2D rotatedTexture = RotateTexture(itemBGTex, zRotation);
                    itemBGBytes = rotatedTexture.EncodeToJPG(10);
                }

                else
                {
                    itemBGBytes = itemBGTex.EncodeToJPG(10);
                }

                timesuffixforname = DateTime.Now.ToString("s").Replace(":", "");
                string destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "MediaFiles" + "/" + ObsDBid.text.ToString() + "_" + timesuffixforname + "Rating_" + conditionvalue.text + "_Media_" + filename.Replace("'", "").Replace(" ", "_");

                if (String.IsNullOrEmpty(conditionvalue.text.ToString()))
                {
                    errorpopup.gameObject.SetActive(true);
                    defaulticon.SetActive(false);
                    PopUpMsg.text = "Please assign a condition value for this photograph.";
                    StartCoroutine(HidePopUp());
                }

                else
                {
                    Attachment_Title = "Media";
                    Attachment_Name = ObsDBid.text.ToString() + "_" + timesuffixforname + "Rating_" + conditionvalue.text + "_Media_" + filename.Replace("'", "").Replace(" ", "_");
                    Attachment_Path = attachmentsfolderpath + "/" + "MediaFiles" + "/";
                    Attachment_Details_1 = FileExt;
                    Attachment_Details_2 = Filesize.ToString();
                    Attachment_Details_3 = conditionvalue.text;
                    Attachment_Details_4 = "";
                    Active = "Y";

                    File.WriteAllBytes(destination, itemBGBytes);

                    mlocationDb.addData(new Inspection_AttachmentsEntity(ID,
                        int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()),
                        int.Parse(ObsDBid.text.ToString()),
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
                    filenameforsave.text = "";
                    PopUpMsg.text = "File saved.";


                    if (livephoto == true)
                    {
                        string albumName = int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()) + "_InspectNAU_Inspection";// Specify a consistent album name
                        
                        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(itemBGTex, albumName, filename,
                            (success, path) =>
                            {
                                Debug.Log("Media save result: " + success + " " + path);
                            });
                        Destroy(itemBGTex);
                    }

                    //if (livephoto == true)
                    //{
                    //    NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(itemBGTex, filename.Replace(".png", ""), ".png", (success, path) =>

                    //    Debug.Log("Media save result: " + success + " " + path));
                    //    Destroy(itemBGTex);
                    //}
                    StartCoroutine(HidePopUp());
                    StartCoroutine(Fileviewer());
                    RefreshBtn();
                }



            }

            else
            {
                filenameforsave.text = "";
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text = "No Media/File Selected.";
                StartCoroutine(HidePopUp());
                Debug.Log("No File Selected.");
            }
            if (int.Parse(ObsDBid.text.ToString()) == 0) //indicates this is a standard photograph section.
            {
                string query = "";
                int mediacount = 0;
                int obsDBid = 0;
                string suffix = Questions_Text.text.ToString().Split('_')[0];
                table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
                using var connection2 = mlocationdb2.getConnection();
                query = " Inspection_Attachments where cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + obsDBid + "'" + " and Attachment_Name like '%_" + suffix + "_%';";
                mediacount = mlocationdb2.CountbasisQuery(query);

                if (mediacount > 0)
                {
                    completionstatusindicator.GetComponent<Image>().color = Color.green;
                    Mediacount.text = mediacount.ToString();
                }

                //mlocationdb2.close();
            }
            
           
            
           // mlocationDb.close();
        }
    }

    private Texture2D RotateTexture(Texture2D originalTexture, float angle)
    {
        int width = originalTexture.width;
        int height = originalTexture.height;

        // Create a new texture with swapped width and height if rotating 90 or 270 degrees
        Texture2D rotatedTexture = new Texture2D(height, width);

        // Rotate each pixel in the original texture to the new texture position
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // New coordinates after rotation
                int newX = y;
                int newY = width - x - 1;

                rotatedTexture.SetPixel(newX, newY, originalTexture.GetPixel(x, y));
            }
        }
       
        rotatedTexture.Apply();
        return rotatedTexture;
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

            Sliderpanel.SetActive(false);
            Photocondition.value = 0;
            conditionvalue.text = "";
            m_RawImage.gameObject.transform.rotation = Quaternion.identity;
            livebtn.SetActive(true);
            library.SetActive(true);

            if (fromstandardphotos == true)
            {
                Filesbtn.SetActive(false);
            }

            else
            {
                Filesbtn.SetActive(true);
            }
        }
        ObsContr.gameObject.GetComponent<LayoutElement>().preferredHeight = 400;
        attachmentpanel.SetActive(false);

        // Restore GameObjects to their original states
        for (int i = 0; i < speechcubes.Length; i++)
        {
            if (speechcubes[i] != null)
                speechcubes[i].SetActive(originalStates[i]);
        }

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

            Sliderpanel.SetActive(false);
            Photocondition.value = 0;
            conditionvalue.text = "";
            m_RawImage.gameObject.transform.rotation = Quaternion.identity;
            livephoto = false;
            filenameforsave.gameObject.SetActive(false);
            //errorpopup.gameObject.SetActive(true);
            //PopUpMsg.text = "Refreshed.";
            //StartCoroutine(HidePopUp());

            }

        else
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Nothing to refresh.";
            StartCoroutine(HidePopUp());
        }
        livebtn.SetActive(true);
        library.SetActive(true);

        if (fromstandardphotos == true)
        {
            Filesbtn.SetActive(false);
        }

        else
        {
            Filesbtn.SetActive(true);
        }

        defaulticon.SetActive(false);
    }

   public void OLDRotateBtn()  //WITHOUT TIMESTAMP ROTATION IGNORE commented on 21st Aug 2025
    {
        Vector3 normalsize = new Vector3(1.0f, 1.0f, 1.0f);
        m_RawImage.rectTransform.localScale = normalsize;

        if (!String.IsNullOrEmpty(filename))
        {
            m_RawImage.gameObject.transform.Rotate(new Vector3(0, 0, 90), 90);
        }

        else
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Nothing to rotate.";
            StartCoroutine(HidePopUp());

        }
            

        float zRotation = m_RawImage.gameObject.transform.eulerAngles.z;
        if (Mathf.Approximately(zRotation, 180f)|| Mathf.Approximately(zRotation, 0f))
        {
           
            m_RawImage.rectTransform.localScale = normalsize;
        }

        else
        {
            Vector3 newScale = new Vector3(0.8f, 0.8f, 1.0f);
            m_RawImage.rectTransform.localScale = newScale;
        }

    }

    public void RotateBtn() //IGNORING TIMESTAMP WHEN ROTATING added on 21st Aug 2025
    
    {
        Vector3 normalsize = new Vector3(1f, 1f, 1f);
        m_RawImage.rectTransform.localScale = normalsize;

        if (string.IsNullOrEmpty(filename))
        {
            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text = "Nothing to rotate.";
            StartCoroutine(HidePopUp());
            return;
        }

        // Rotate the pixels instead of the UI transform
        var tex = m_RawImage.texture as Texture2D;
        if (tex == null) return;

        var rotated = RotateTexture(tex, 90); // your existing pixel-rotate
        rotated.Apply(false, false);

        // Assign rotated texture and ensure the UI stays unrotated
        m_RawImage.texture = rotated;
        m_RawImage.rectTransform.localEulerAngles = Vector3.zero;

        // Optional: keep your scale rule
        float w = rotated.width;
        float h = rotated.height;
        if (Mathf.Approximately(w % 2, 0) && Mathf.Approximately(h % 2, 0)) // or your own condition
            m_RawImage.rectTransform.localScale = normalsize;
        else
            m_RawImage.rectTransform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }



    IEnumerator FileviewerONHOLD()
    {
        string attachmentsfolderpath1 = "";

        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

        yield return null;
            for (int i = 0; i < ParentPanel.transform.childCount; ++i)
            {
                Destroy(ParentPanel.transform.GetChild(i).gameObject);
            }

            string mediasourceDirectory = Application.persistentDataPath + attachmentsfolderpath1 + "/" + "MediaFiles";
            //FileExt = Path.GetExtension(mediasourceDirectory).ToLower();
            string filessourceDirectory = Application.persistentDataPath + attachmentsfolderpath1 + "/" + "Filespath";

            mediafilespath = "";
                //try
                //{
                var mediafileCount = Directory.EnumerateFiles(mediasourceDirectory).Count();
                var filesCount = Directory.EnumerateFiles(filessourceDirectory).Count();

              if (mediafileCount == 0 && filesCount == 0 )
              {
                   fileviewerobj.SetActive(false);
              }

              else
              {
                fileviewerobj.SetActive(true);

                if (mediafileCount > 0)
                {
                string[] mediafiles = { ".png", ".jpeg", ".jpg" };

                foreach (var file in Directory.EnumerateFiles(mediasourceDirectory, "*.*", SearchOption.AllDirectories).Where(x => mediafiles.Any(x.ToLower().EndsWith)))
                {
                    Debug.Log("Media Files Found " + file.ToString());

                    mediafilespath = file.ToString();

                    byte[] FiletextureBytes = File.ReadAllBytes(file);
                    Texture2D thumbnail = new Texture2D(2, 2);
                    thumbnail.LoadImage(FiletextureBytes);
                    thumbnail.Apply();

                    FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
                    FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
                    FilesViewImgAccordion.transform.GetComponent<RawImage>().texture = thumbnail;
                    FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "MediaFiles" + "/";
                    FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file);
                    FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(file).Split("_Media_")[1];
                    FilesViewImgAccordion.transform.Find("Rating").GetComponent<TextMeshProUGUI>().text = "Rating; " + Path.GetFileName(file).Split("_Media_")[0].Split("Rating_")[1];
                    FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                    FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();
                                            
                }

                }
                else
                {
                Debug.Log("No Media Files Found in the folder.");
                }

                if (filesCount > 0)
                {
                var Otherfiletypes = Directory.EnumerateFiles(filessourceDirectory.ToLower(), "*", SearchOption.AllDirectories);

                foreach (string currentFile in Otherfiletypes)
                {
                    FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
                    FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
                    FilesViewImgAccordion.transform.Find("DefaultImg").gameObject.SetActive(true);
                    FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "Filespath" + "/";
                    FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile);
                    FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile).Split("_File_")[1];

                    FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                    FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();

                    Debug.Log("Documents Found " + currentFile.ToString());
                }

                }
                else
                {
                Debug.Log("No Documents Found in the folder.");
                }

              }
        //mlocationdb1.close();
        //}

        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e.Message);
    }

    IEnumerator Fileviewer()
    {
        string attachmentsfolderpath1 = "";
        string suffixforphotos = "";
        string query = "";
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

        yield return null;
        for (int i = 0; i < ParentPanel.transform.childCount; ++i)
        {
            Destroy(ParentPanel.transform.GetChild(i).gameObject);
        }
         table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
        using var connection2 = mlocationdb2.getConnection();
        if (int.Parse(ObsDBid.text.ToString())!=0)

        {
         query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(InspPrimaryId.text.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(ObsDBid.text.ToString()) + "'"; //+ " and trim(Attachment_Title) = 'Media'";
        }

        else
        {
          suffixforphotos = Questions_Text.text.ToString().Split('_')[0];
          query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(InspPrimaryId.text.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(ObsDBid.text.ToString()) + "'" + " and Attachment_Name like '%_" + suffixforphotos + "_%';";

        }

        using System.Data.IDataReader reader  = mlocationdb2.SelectDataByquery(query);

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
        fileviewerobj.SetActive(true);
        //if (mediafileCount != 0 || filesCount != 0)
        //{
        //    fileviewerobj.SetActive(true);
        //}
        //else
        //{
        //    fileviewerobj.SetActive(false);
        //}

        if (mediafileCount > 0)
        {
           foreach (var media in MediaFiles)
            {
                if (File.Exists(media.ToString()))
                {
                    //Debug.Log("Media Files Found " + media.ToString());

                    byte[] FiletextureBytes = File.ReadAllBytes(media);
                    Texture2D thumbnail = new Texture2D(2, 2);
                    thumbnail.LoadImage(FiletextureBytes);
                    thumbnail.Apply();

                    
                    FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
                    FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
                    FilesViewImgAccordion.transform.GetComponent<RawImage>().texture = thumbnail;
                    FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "MediaFiles" + "/";
                    FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media);
                    FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media).Split("_Media_")[1];
//Users
                    if (Path.GetFileName(media).Contains("Rating"))
                    {
                        FilesViewImgAccordion.transform.Find("RatingBk/Rating").GetComponent<TextMeshProUGUI>().text = "Rated; " + Path.GetFileName(media).Split("_Media_")[0].Split("Rating_")[1];
                    }

                    else
                    {
                        FilesViewImgAccordion.transform.Find("RatingBk/Rating").GetComponent<TextMeshProUGUI>().text = "No Rating ";
                    }
                    
                    FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                    FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();
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
                FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "Filespath" + "/";
                FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile);
                FilesViewImgAccordion.transform.Find("ShowFileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile).Split("_File_")[1];
                FilesViewImgAccordion.transform.Find("RatingBk").gameObject.SetActive(false);
                FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
                FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();
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
        //}

        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e.Message);
    }


    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(2.0f);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }
}
