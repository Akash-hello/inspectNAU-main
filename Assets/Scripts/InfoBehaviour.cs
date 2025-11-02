using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoBehaviour : MonoBehaviour
{
    const float SPEED = 6f;
    [SerializeField]
    Transform SectionInfo;
    Vector3 desiredScale = Vector3.zero;
    public TextMeshPro SectionInfotext;
    public Text countrycode;
    // Start is called before the first frame update
    void Start()
    {
        if (countrycode.text == "us")
        {
            SectionInfotext.text = "U.S.A.";

        }

        if (countrycode.text == "gb")
        {
            SectionInfotext.text = "U.K.";

        }

        if (countrycode.text == "in")
        {
            SectionInfotext.text = "India";

        }

        if (countrycode.text == "sg")
        {
            SectionInfotext.text = "Singapore";

        }

        if (countrycode.text == "cn")
        {
            SectionInfotext.text = "China";

        }

        if (countrycode.text == "hk")
        {
            SectionInfotext.text = "Hongkong";

        }

        if (countrycode.text == "ru")
        {
            SectionInfotext.text = "Russia";

        }

        if (countrycode.text == "br")
        {
            SectionInfotext.text = "Brazil";

        }

        if (countrycode.text == "jp")
        {
            SectionInfotext.text = "Japan";

        }

        if (countrycode.text == "nl")
        {
            SectionInfotext.text = "Netherlands";

        }

        if (countrycode.text == "gr")
        {
            SectionInfotext.text = "Greece";

        }

        if (countrycode.text == "fr")
        {
            SectionInfotext.text = "France";

        }

        if (countrycode.text == "ae")
        {
            SectionInfotext.text = "UAE";

        }

        if (countrycode.text == "de")
        {
            SectionInfotext.text = "Denmark";

        }

        if (countrycode.text == "no")
        {
            SectionInfotext.text = "Norway";

        }

        if (countrycode.text == "it")
        {
            SectionInfotext.text = "Italy";

        }
    }
    // Update is called once per frame
    void Update()
    {
        SectionInfo.localScale = Vector3.Lerp(SectionInfo.localScale, desiredScale, Time.deltaTime * SPEED);
    }

    public void OpenInfo()
    {
        
        desiredScale = Vector3.one;
    }

    public void CloseInfo()
    {
        desiredScale = Vector3.zero;
    }
}
