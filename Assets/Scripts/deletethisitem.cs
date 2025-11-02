using DataBank;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deletethisitem : MonoBehaviour
{
    public Text ID;
    public int itemid;
    public GameObject PopupImg;
    public Text PopUpmsg;
    //table_MyStocks mlocationDb;
    public float time = 0.0f;

    public void Start()
    {
        time = 0.5f;
        PopupImg.gameObject.SetActive(false);
        table_MyStocks mlocationDb = new table_MyStocks();
        using var connection = mlocationDb.getConnection();
        mlocationDb.deleteItemCall = this;
        itemid = int.Parse(ID.text);
    }

    public void Deletebtnclick()
    {
        table_MyStocks mlocationDb = new table_MyStocks();
        using var connection = mlocationDb.getConnection();
        using System.Data.IDataReader reader = mlocationDb.deleteItemById(itemid);
        ItemDeleted();
        //mlocationDb.deleteDataByString(itemid.ToString().Trim());
    }

    public void ItemDeleted()
    {
        PopupImg.gameObject.SetActive(true);
        PopUpmsg.color = Color.white;
        PopUpmsg.text = "Deleted from watchlist!!";
        //Debug.Log("ITEM DELETED");
        StartCoroutine(HidePopUp());
    }


    IEnumerator HidePopUp()
    {
        yield return new WaitForSeconds(time);

        PopupImg.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}
