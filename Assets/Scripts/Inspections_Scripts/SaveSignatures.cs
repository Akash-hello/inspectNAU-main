using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataBank;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using FreeDraw;

public class SaveSignatures : MonoBehaviour
{

    string attachmentsfolderpath;
    public RawImage SignatureImage;
    public GameObject ImageFileObj;
    public string filename;
    public TextMeshProUGUI PrimarytableID;
    string destination = "";
    public GameObject DrawingSprite;
    public Drawable drawingscript;
    public GameObject PopUp;
    public TextMeshProUGUI PopUpMsg;

    public void checkifsignexists()
    {
        filename = ImageFileObj.name.ToString() + ".jpg";
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + filename;

        if (File.Exists(destination))
        {
            StartCoroutine(LoadTexture());

        }
        else
        {
            DrawingSprite.SetActive(true);
            drawingscript.ResetCanvas();
        }
        //mlocationdb1.close();
        PopUpMsg.GetComponent<TextMeshProUGUI>().text = "";
    }


    IEnumerator LoadTexture ()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(destination);

        request.uri = new Uri(request.uri.AbsoluteUri.Replace("https://localhost", "file://"));

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Signature exists");
            var texture = DownloadHandlerTexture.GetContent(request);
            SignatureImage.GetComponent<RawImage>().texture = texture;
            DrawingSprite.SetActive(false);
        }
    }

    public void save()
    {
        filename = ImageFileObj.name.ToString()+".jpg";
        //filename = "Inspector_Sign.jpg";
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath = mlocationdb1.folderpath.ToString();

        //CONVERT SPRITE TO TEXTURE2D and APPLY TO THE SIGNATURE IMAGE
        var Texturefmsprite = new Texture2D((int)DrawingSprite.GetComponent<SpriteRenderer>().sprite.rect.width, (int)DrawingSprite.GetComponent<SpriteRenderer>().sprite.rect.height);

        var pixels = DrawingSprite.GetComponent<SpriteRenderer>().sprite.texture.GetPixels((int)DrawingSprite.GetComponent<SpriteRenderer>().sprite.textureRect.x,
                                                (int)DrawingSprite.GetComponent<SpriteRenderer>().sprite.textureRect.y,
                                                (int)DrawingSprite.GetComponent<SpriteRenderer>().sprite.textureRect.width,
                                                (int)DrawingSprite.GetComponent<SpriteRenderer>().sprite.textureRect.height);

        Texturefmsprite.SetPixels(pixels);
        Texturefmsprite.Apply();

        SignatureImage.GetComponent<RawImage>().texture = Texturefmsprite;

        if (!Directory.Exists(Application.persistentDataPath + attachmentsfolderpath + "/" + "General"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + attachmentsfolderpath + "/" + "General");
        }
        Texture2D itemBGTex = SignatureImage.texture as Texture2D;
        //Texture2D itemBGTex = SignatureImage.texture as Texture2D;

        byte[] itemBGBytes = itemBGTex.EncodeToJPG(10);

        string destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + filename;

        File.WriteAllBytes(destination, itemBGBytes);

        PopUp.SetActive(true);
        PopUpMsg.GetComponent<TextMeshProUGUI>().text = "'SAVED'";
        PopUpMsg.GetComponent<TextMeshProUGUI>().color = Color.blue;
        StartCoroutine(HidePopUp());

        //mlocationdb1.close();
    }

    public void delete ()
    {
        DrawingSprite.SetActive(true);
        
        drawingscript.ResetCanvas();

        filename = ImageFileObj.name.ToString() + ".jpg";
        table_Inspection_PrimaryDetails mlocationdb1 = new table_Inspection_PrimaryDetails();
        using var connection1 = mlocationdb1.getConnection();
        mlocationdb1.getDataBypassedId(int.Parse(PrimarytableID.GetComponent<TextMeshProUGUI>().text.ToString()));

        attachmentsfolderpath = mlocationdb1.folderpath.ToString();
        destination = Application.persistentDataPath + attachmentsfolderpath + "/" + "General" + "/" + filename;

        if (File.Exists(destination))
            File.Delete(destination);

        PopUp.SetActive(true);
        PopUpMsg.GetComponent<TextMeshProUGUI>().text = "'CLEARED'";
        PopUpMsg.GetComponent<TextMeshProUGUI>().color = Color.red;
        StartCoroutine(HidePopUp());

        //mlocationdb1.close();
    }

    IEnumerator HidePopUp()
    {
        float time = 2.0f;
        yield return new WaitForSeconds(time);
        PopUpMsg.GetComponent<TextMeshProUGUI>().color = Color.black;
        PopUp.gameObject.SetActive(false);
        
    }
}
