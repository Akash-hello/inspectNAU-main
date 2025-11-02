using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountryCodeSelected : MonoBehaviour
{
    public Button selectitem;
    TMP_InputField inputField;
    public TextMeshProUGUI Countrycode;
    //public GameObject Countrysscrollview;

    public void Start()
    {
        //Countrysscrollview.SetActive(true);
        //Countrysscrollview = GameObject.FindGameObjectWithTag("Countrysscrollview").gameObject;
        inputField = GameObject.FindGameObjectWithTag("countrycodeinput").GetComponent<TMP_InputField>();
    }
    public void Onselectingitem()
    {
        inputField.text  = Countrycode.text.Split('.')[1];

        //Debug.Log(inputField.text.ToString());
    }
}
