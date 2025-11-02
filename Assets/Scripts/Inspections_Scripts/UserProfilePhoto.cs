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
using Color = UnityEngine.Color;

public class UserProfilePhoto : MonoBehaviour
{
    public string OriginPath;
    string filename;
    string FileExt;
    Double Filesize;
   
    public RawImage m_RawImage;
    //public RawImage LogoutBtnImg;
    //System.Drawing.Image compressedphoto;
    //Bitmap Photo;
    bool Media;
    public GameObject defaulticon;
   
    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 8.0f;
    public GameObject fileviewerobj;
    string mediafilespath;
    string documentspath;

    public RawImage clientlogoimg;
    public GameObject inspectNAU;
    string filename1;
    Double Filesize1;
    string FileExt1;

    // Start is called before the first frame update

    public void Start ()
    {
        OriginPath = Application.persistentDataPath + "/" + "Profile" + "/" + "ClientLogoImg.jpg";

        if (File.Exists(OriginPath.ToString()))
        {
            Texture2D texture1 = NativeGallery.LoadImageAtPath(OriginPath, 1024, false);
            clientlogoimg.GetComponent<RawImage>().texture = texture1;
            inspectNAU.SetActive(false);
        }

        else
        {
            inspectNAU.SetActive(true);
        }
    }

    public void uploadphotofmgallery()
    {
        filename = "";
        m_RawImage.GetComponent<RawImage>().texture = null;
        FileExt = "";
        Filesize = 0;
        Media = false;
        defaulticon.SetActive(false);

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path,1024,false);
                if (texture == null)
                {
                    LoadPhotoFmDB();
                    Debug.Log("Couldn't load texture from " + path);
                    //defaulticon.SetActive(true);
                    return;
                }

                else
                {
                    filename = Path.GetFileName(path);
                    long filesize = new System.IO.FileInfo(path).Length;
                    Filesize = (filesize / 1024 ^ 2) / 1024;

                    FileExt = Path.GetExtension(path).ToLower();

                    Debug.Log("Picked File: " + path + "-" + filename + "Size;" + Filesize + " MB");
                    
                    m_RawImage.GetComponent<RawImage>().texture = texture;

                    Media = true;

                    //if (FileExt == ".jpeg" || FileExt == ".jpg" || FileExt == ".png")
                    //{
                    //    Media = true;
                    //}

                    defaulticon.SetActive(false);
                }
               
            }

            else
            {
                // User canceled the selection
                LoadPhotoFmDB();
            }
        });

        Debug.Log("Permission result: " + permission);

    }

    public void clientcompanylogo()
    {
        filename1 = "";
        clientlogoimg.GetComponent<RawImage>().texture = null;
        FileExt1 = "";
        Filesize1 = 0;
        Media = false;
        //defaulticon.SetActive(false);

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture1 = NativeGallery.LoadImageAtPath(path, 1024, false);
                
                if (texture1 == null)
                {
                    inspectNAU.SetActive(true);
                    return;
                }

                else
                {
                    filename1 = "ClientLogoImg.jpg";
                    long filesize = new System.IO.FileInfo(path).Length;
                    Filesize = (filesize / 1024 ^ 2) / 1024;

                    FileExt = Path.GetExtension(path).ToLower();

                    Debug.Log("Picked File: " + path + "-" + filename1 + "Size;" + Filesize + " MB");

                   
                    if (filesize > 50000)
                    {
                        errorpopup.gameObject.SetActive(true);
                        PopUpMsg.color = Color.red;
                        PopUpMsg.text = "The selected image is not suitable for logo, please select a high resolution square image of size less than 50 KB.";
                       
                        StartCoroutine(HidePopUp());
                        inspectNAU.SetActive(true);
                    }

                    else
                    {
                        clientlogoimg.GetComponent<RawImage>().texture = texture1;
                        Texture2D itemBGTex = clientlogoimg.texture as Texture2D;

                        byte[] itemBGBytes = itemBGTex.EncodeToJPG();

                        string destination = Application.persistentDataPath + "/" + "Profile" + "/" + filename1;

                        File.WriteAllBytes(destination, itemBGBytes);
                        inspectNAU.SetActive(false);
                        Media = true;
                    }
                    
                }

            }

            else
            {
                OriginPath = Application.persistentDataPath + "/" + "Profile" + "/" + "ClientLogoImg.jpg";

                if (File.Exists(OriginPath.ToString()))
                {
                    Texture2D texture1 = NativeGallery.LoadImageAtPath(OriginPath, 1024, false);
                    clientlogoimg.GetComponent<RawImage>().texture = texture1;
                    inspectNAU.SetActive(false);
                }

                else
                {
                    inspectNAU.SetActive(true);
                }

                
            }
        });

        Debug.Log("Permission result: " + permission);
    }

    public void CancelClientLogo()
    {
        OriginPath = Application.persistentDataPath + "/" + "Profile" + "/" + "ClientLogoImg.jpg";

        if (File.Exists(OriginPath.ToString()))
        {
            File.Delete(OriginPath.ToString());
            clientlogoimg.GetComponent<RawImage>().texture = null;
            inspectNAU.SetActive(true);
        }

        else
        {
            inspectNAU.SetActive(true);
        }
    }


    //BELOW Function replaced with above for NATIVE GALLERY PHOTO FROM NATIVE GALLERY instead of NATIVE FILE PICKER.

    //public void uploadphoto()
    //{
    //    filename = "";
    //    m_RawImage.GetComponent<RawImage>().texture = null;
    //    FileExt = "";
    //    Filesize = 0;
    //    Media = false;
    //    defaulticon.SetActive(false);

    //    string FileType = NativeFilePicker.ConvertExtensionToFileType("png,jpg,jpeg");
    //    //string[] FileTypes = new string[] { NativeFilePicker.ConvertExtensionToFileType("png"), NativeFilePicker.ConvertExtensionToFileType("jpg"), NativeFilePicker.ConvertExtensionToFileType("jpeg") };
    //    NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
    //    {
    //        if (path == null)
    //        {
    //            LoadPhotoFmDB();
    //            Debug.Log("Operation Cancelled");
    //        }

    //        else

    //        {
    //            OriginPath = path;
    //            filename = Path.GetFileName(path);
    //            long filesize = new System.IO.FileInfo(OriginPath).Length;
    //            Filesize = (filesize / 1024 ^ 2) / 1024;

    //            FileExt = Path.GetExtension(OriginPath).ToLower();

    //            Debug.Log("Picked File: " + OriginPath + "-" + filename + "Size;" + Filesize + " MB");

    //            if (FileExt == ".jpeg" || FileExt == ".jpg" || FileExt == ".png")
    //            {
    //                Media = true;
    //            }

    //            StartCoroutine(LoadTexture());
    //        }
    //    }, //new string[] { FileType }) // Commented on 22nd May as Android was giving message; "No Apps can perofrm this action" as per https://forum.unity.com/threads/native-file-picker-for-android-ios-open-source.912890/page-3
    //       NativeFilePicker.ConvertExtensionToFileType("png"), NativeFilePicker.ConvertExtensionToFileType("jpg"), NativeFilePicker.ConvertExtensionToFileType("jpeg"));

    //}

    IEnumerator LoadTexture()
    {
        //if (Media == true)
        //{
        //    WWW www = new WWW(OriginPath);
        //    while (!www.isDone)
        //        yield return null;
        //    m_RawImage.GetComponent<RawImage>().texture = www.texture;
        //    defaulticon.SetActive(false);
        //}

        //else
        //{
        //    defaulticon.SetActive(true);
        //}

        if (Media == true)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(OriginPath);

            request.uri = new Uri(request.uri.AbsoluteUri.Replace("https://localhost", "file://"));

            yield return request.SendWebRequest();

            while (!request.isDone) yield return request;

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                defaulticon.SetActive(true);
            }
            else
            {
                Debug.Log("Successfully Downloaded image.");
                var texture = DownloadHandlerTexture.GetContent(request);
                m_RawImage.GetComponent<RawImage>().texture = texture;

                defaulticon.SetActive(false);
                //LogoutBtnImg.GetComponent<RawImage>().texture = texture;
            }

        }

        else
        {
            defaulticon.SetActive(true);
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
    public void Savefile()
    {
        time = 5.0f;
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();

       if (!String.IsNullOrEmpty(filename) && Media == true)
        {
            Texture2D itemBGTex = m_RawImage.texture as Texture2D;

            byte[] itemBGBytes = itemBGTex.EncodeToJPG(10);

            string destination = Application.persistentDataPath +  "/" + "Profile" + "/" + filename;

            File.WriteAllBytes(destination, itemBGBytes);
            string columndataquery = "photofilename = '" + filename.Replace("'", "''") + "' where cast(Id as int) = " + mlocationDb.LatestId;
            mlocationDb.Updatedata(columndataquery);
            //mlocationDb.close();
            // StartCoroutine(Fileviewer());

        }

        else
        {
            Debug.Log("No Profile photo Selected."); //COMMENTED BELOW POP UP as this was coming with other and seems to be two popups one after the other, no need for this one...
            //errorpopup.gameObject.SetActive(true);
            //PopUpMsg.text = "No Profile photo Selected.";
            //mlocationDb.close();
            //StartCoroutine(HidePopUp());
        }

    }

    public void LoadPhotoFmDB()
    {
        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.getLatestID();
        defaulticon.SetActive(false);
        if (mlocationDb.LatestId != 0 && !String.IsNullOrEmpty(mlocationDb.profilephotoname))
        {
        filename = mlocationDb.profilephotoname.Trim().ToString();
        OriginPath = Application.persistentDataPath + "/" + "Profile" + "/" + filename;
            
          if(File.Exists(OriginPath.ToString()))
            {
                Media = true;
                Texture2D texture = NativeGallery.LoadImageAtPath(OriginPath, 1024, false);
                m_RawImage.GetComponent<RawImage>().texture = texture;
                //StartCoroutine(LoadTexture());
            }

          else
                defaulticon.SetActive(true);
        }

        else
        {
            defaulticon.SetActive(true);
        }

        //mlocationDb.close();
    }

    //IEnumerator Fileviewer()
    //{
    //    string attachmentsfolderpath1 = "";

    //    table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
    //    mlocationdb1.getDataBypassedId(int.Parse(InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString()));

    //    attachmentsfolderpath1 = mlocationdb1.folderpath.ToString();

    //    yield return null;
    //    for (int i = 0; i < ParentPanel.transform.childCount; ++i)
    //    {
    //        Destroy(ParentPanel.transform.GetChild(i).gameObject);
    //    }

    //    string query = "cast(Inspection_PrimaryDetails_ID as int) = '" + int.Parse(InspPrimaryId.text.ToString()) + "' and cast(Inspection_Observations_ID as int) = '" + int.Parse(ObsDBid.text.ToString()) + "'"; //+ " and trim(Attachment_Title) = 'Media'";

    //    table_Inspection_Attachments mlocationdb2 = new table_Inspection_Attachments();
    //    System.Data.IDataReader reader = mlocationdb2.SelectDataByquery(query);

    //    List<Inspection_AttachmentsEntity> myList = new List<Inspection_AttachmentsEntity>();

    //    while (reader.Read())
    //    {
    //        Inspection_AttachmentsEntity entity = new Inspection_AttachmentsEntity(
    //        int.Parse(reader[0].ToString()),
    //        int.Parse(reader[1].ToString()),
    //        int.Parse(reader[2].ToString()),
    //        reader[3].ToString().Trim(),
    //        reader[4].ToString(),
    //        reader[5].ToString().Trim(),
    //        reader[6].ToString().Trim(),
    //        reader[7].ToString().Trim(),
    //        reader[8].ToString().Trim(),
    //        reader[9].ToString().Trim(),
    //        reader[10].ToString().Trim(),
    //        reader[11].ToString().Trim());

    //        //Debug.Log("Stock Code: " + entity._stocksym);
    //        myList.Add(entity);

    //        var output1 = JsonUtility.ToJson(entity, true);

    //    }

    //    List<String> MediaFiles = new List<string>();
    //    List<String> Files = new List<string>();

    //    if (myList.Count != 0)
    //    {
    //        foreach (var x in myList)
    //        {
    //            if (x._Attachment_Title.Trim() == "Media")
    //            {
    //                MediaFiles.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
    //            }
    //            else
    //                Files.Add(Application.persistentDataPath + x._Attachment_Path.Trim() + x._Attachment_Name.Trim());
    //        }
    //    }

    //    else
    //    {
    //        Debug.Log("There are no files associated to this observation. ");
    //    }
    //    var mediafileCount = MediaFiles.Count;
    //    var filesCount = Files.Count;

    //    if (mediafileCount != 0 || filesCount != 0)
    //    {
    //        fileviewerobj.SetActive(true);
    //    }
    //    else
    //    {
    //        fileviewerobj.SetActive(false);
    //    }

    //    if (mediafileCount > 0)
    //    {
    //        foreach (var media in MediaFiles)
    //        {
    //            if (File.Exists(media.ToString()))
    //            {
    //                Debug.Log("Media Files Found " + media.ToString());

    //                byte[] FiletextureBytes = File.ReadAllBytes(media);
    //                Texture2D thumbnail = new Texture2D(2, 2);
    //                thumbnail.LoadImage(FiletextureBytes);
    //                thumbnail.Apply();

    //                FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
    //                FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
    //                FilesViewImgAccordion.transform.GetComponent<RawImage>().texture = thumbnail;
    //                FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "MediaFiles" + "/";
    //                FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(media);
    //                FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
    //                FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();
    //            }
    //            else
    //                Debug.Log("There was no file of this name; " + media.ToString());
    //        }

    //    }
    //    else
    //    {
    //        Debug.Log("No Media Files Found in the folder.");
    //    }

    //    if (filesCount > 0)
    //    {
    //        foreach (string currentFile in Files)
    //        {
    //            if (File.Exists(currentFile.ToString()))
    //            {
    //                Debug.Log("Documents Found " + currentFile.ToString());

    //                FilesViewImgAccordion = Instantiate(FilesViewImgPrefab);
    //                FilesViewImgAccordion.transform.SetParent(ParentPanel, false);
    //                FilesViewImgAccordion.transform.Find("DefaultImg").gameObject.SetActive(true);
    //                FilesViewImgAccordion.transform.Find("FilePath").GetComponent<TextMeshProUGUI>().text = attachmentsfolderpath1 + "/" + "Filespath" + "/";
    //                FilesViewImgAccordion.transform.Find("FileName").GetComponent<TextMeshProUGUI>().text = Path.GetFileName(currentFile);
    //                FilesViewImgAccordion.transform.Find("InspPrimaryId").GetComponent<TextMeshProUGUI>().text = InspPrimaryId.GetComponent<TextMeshProUGUI>().text.ToString();
    //                FilesViewImgAccordion.transform.Find("ObsId").GetComponent<TextMeshProUGUI>().text = ObsDBid.GetComponent<TextMeshProUGUI>().text.ToString();
    //            }
    //            else
    //                Debug.Log("There was no file of this name; " + currentFile.ToString());
    //        }

    //    }
    //    else
    //    {
    //        Debug.Log("No Documents Found in the folder.");
    //    }

    //    //}

    //    //}
    //    //catch (Exception e)
    //    //{
    //    //    Console.WriteLine(e.Message);
    //}

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
        PopUpMsg.color = Color.black;
    }
}
