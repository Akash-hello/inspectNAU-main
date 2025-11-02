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

public class ShipsImage : MonoBehaviour
{
    public string OriginPath;
    string filename;
    string FileExt;
    Double Filesize;

    public RawImage m_RawImage;

    //System.Drawing.Image compressedphoto;
    //Bitmap Photo;
    bool Media;
    public GameObject defaulticon;

    public GameObject errorpopup;
    public TextMeshProUGUI PopUpMsg;
    float time = 2.0f;
    //public GameObject fileviewerobj;
    string mediafilespath;
    string documentspath;

    public GameObject ImageFileObj;
    string attachmentsfolderpath;
    public TextMeshProUGUI PrimarytableID;

    public GameObject ShipImageDisabledTxt;

    public void uploadphotofmgallery()
    {
        filename = "";
        m_RawImage.GetComponent<RawImage>().texture = null;
        FileExt = "";
        Filesize = 0;
        Media = false;
        defaulticon.SetActive(false);
        ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                if (texture == null)
                {
                    checkifShipPhotoexists();
                    Debug.Log("Operation Cancelled");
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
                    ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
                }

            }

            else
            {
                // User canceled the selection
                checkifShipPhotoexists();
            }
        });

        Debug.Log("Permission result: " + permission);

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

    //    NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
    //    {
    //        if (path == null)
    //        {
    //            checkifShipPhotoexists();
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
        if (Media == true)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(OriginPath);

            request.uri = new Uri(request.uri.AbsoluteUri.Replace("https://localhost", "file://"));

            yield return request.SendWebRequest();

            while (!request.isDone) yield return request;

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Successfully Downloaded image.");
                var texture = DownloadHandlerTexture.GetContent(request);
                m_RawImage.GetComponent<RawImage>().texture = texture;

            }
        }

        else
        {
            defaulticon.SetActive(true);
            ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
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
        filename = ImageFileObj.name.ToString() + ".jpg";
        if (!String.IsNullOrEmpty(filename) && Media == true)

        {

            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection1 = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString()));

            attachmentsfolderpath = mlocationdb1.folderpath.ToString();

            //CONVERT SPRITE TO TEXTURE2D and APPLY TO THE SIGNATURE IMAGE

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

            Texture2D itemBGTex = m_RawImage.texture as Texture2D;
            //Texture2D itemBGTex = SignatureImage.texture as Texture2D;
            if (itemBGTex !=null)
            {
                byte[] itemBGBytes = itemBGTex.EncodeToJPG(); //FOR COMPRESSION INPUT A NUMBER HERE .EncodeToJPG(10)...
                string destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + filename;
                string destination2 = Application.persistentDataPath + attachmentsfolderpath + "/" + "MediaFiles" + "/" + filename;
                string destination3 = Application.persistentDataPath + attachmentsfolderpath + "/" + "Filespath" + "/" + filename;

                File.WriteAllBytes(destination, itemBGBytes);
                File.WriteAllBytes(destination2, itemBGBytes);
                File.WriteAllBytes(destination3, itemBGBytes);

            }
            else
            {
                errorpopup.gameObject.SetActive(true);
                PopUpMsg.text += Environment.NewLine+"No Ships photo selected (for info only, you upload later) and/or mandatory fields incomplete.";
                //StartCoroutine(HidePopUp());
            }

           
            //mlocationdb1.close();
        }

        else
        {

            errorpopup.gameObject.SetActive(true);
            PopUpMsg.text += Environment.NewLine+ "No Ships photo selected (for info only, you upload later) and/or mandatory fields incomplete.";
            //StartCoroutine(HidePopUp());

        }
        
    }

    public void checkifShipPhotoexists()
    {
        if (PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString() !="" && int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString()) != 0)
        {
            filename = ImageFileObj.name.ToString() + ".jpg";
            table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
            using var connection1 = mlocationdb1.getConnection();
            mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString()));

            attachmentsfolderpath = mlocationdb1.folderpath.ToString();
            OriginPath = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + filename;

            if (File.Exists(OriginPath))
            {
                Media = true;
                Texture2D texture = NativeGallery.LoadImageAtPath(OriginPath, 1024, false);
                m_RawImage.GetComponent<RawImage>().texture = texture;
                //StartCoroutine(LoadTexture());
                defaulticon.SetActive(false);
                ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
            }

            else
            {
                m_RawImage.GetComponent<RawImage>().texture = null;
                defaulticon.SetActive(true);
                ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
            }
            //mlocationdb1.close();
        }
        

        else
        {
            m_RawImage.GetComponent<RawImage>().texture = null;
            defaulticon.SetActive(true);
            ShipImageDisabledTxt.GetComponent<TextMeshProUGUI>().enabled = false;
        }

       
    }

    //public void LoadPhotoFmDB()
    //{
    //    table_LoginConfig mlocationDb = new table_LoginConfig();
    //    mlocationDb.getLatestID();

    //    if (mlocationDb.LatestId != 0 && !String.IsNullOrEmpty(mlocationDb.profilephotoname))
    //    {
    //        filename = mlocationDb.profilephotoname.Trim().ToString();
    //        OriginPath = Application.persistentDataPath + "/" + "General" + "/" + filename;

    //        if (File.Exists(OriginPath.ToString()))
    //        {
    //            Media = true;
    //            StartCoroutine(LoadTexture());
    //        }

    //        else
    //            defaulticon.SetActive(true);
    //    }

    //    else
    //    {
    //        defaulticon.SetActive(true);
    //    }

    //    mlocationDb.close();
    //}

    

    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        errorpopup.gameObject.SetActive(false);
        PopUpMsg.text = "";
    }
}

