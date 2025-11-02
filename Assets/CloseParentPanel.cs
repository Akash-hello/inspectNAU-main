using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseParentPanel : MonoBehaviour
{
    public GameObject StockRecapParentOBJ;
    // Start is called before the first frame update
    public void closeparentgameobject()
    {
        StockRecapParentOBJ = GameObject.FindGameObjectWithTag("StockRecapParentOBJ");
        StockRecapParentOBJ.GetComponentInChildren<CanvasGroup>().alpha = 0.01f;
        StockRecapParentOBJ.transform.SetSiblingIndex(1);
    }
}
