using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class questionsprefab : MonoBehaviour

{
    public TextMeshProUGUI Questions_Text;
    public RectTransform QuesbackgroundRect;
    public RectTransform QuesTextRect;

    public Button Save;
    public TextMeshProUGUI InspectionPrimaryID;
    public TextMeshProUGUI QuestionDBid;
    public TextMeshProUGUI ChapterDBID;
    public TextMeshProUGUI AttachmentFolderpath;
    public TextMeshProUGUI ShortQues;
    public TextMeshProUGUI ROVIQ;
    public TextMeshProUGUI VesselTypes;
    public TMP_InputField Observation_Input_Area;
    public TextMeshProUGUI Completionstatus;
    public List<GameObject> answergroupmodels = new List<GameObject>();
    public GameObject AnswerGroup;
    public Image RiskCategoryBackground;
    public TextMeshProUGUI RiskCategory;
    
    public TMP_InputField Date;
    public TextMeshProUGUI HeaaderTextForROVIQ;
    public TextMeshProUGUI InspType;

    //public GameObject Sire20_Categories;

    public GameObject ObservationContainer;

    public Button HumanBtn;
    public Image HumanIcn;
    public GameObject HumanPnl;
    public TMP_InputField Hobservationdetail;
    public Toggle Humandeficiency;
    public Toggle Humanasexpected;

    public Button ProcessBtn;
    public Image ProcessIcn;
    public GameObject ProcessPnl;
    public TMP_InputField Pobservationdetail;
    public Toggle Processdeficiency;
    public Toggle Processasexpected;

    public Button HardwareBtn;
    public Image HardwareIcn;
    public GameObject HardwarePnl;
    public TMP_InputField Hardobservationdetail;
    public Toggle Hardwaredeficiency;
    public Toggle Hardwareasexpected;

    public Button TMSABTN;
    public Image TMSAIcn;

    public Button ObjectiveBtn;
    public Image ObjectiveIcn;

    public Button ObjectiveBtnNonSire;
    public Image  ObjectiveIcnNonSire;

    public TextMeshProUGUI TaggedRanks;

    public GameObject Defaulticon;
    public GameObject Corequestion;
    public GameObject Rotation1;
    public GameObject Rotation2;

    public bool isphotomandatory;

    public GameObject[] switchforstandardphotos;
    public Button RightclickArrow;

    public GameObject[] micButtons; //MIC BUTTONS on the observation and SIRE panels are controlled from here.
    public GameObject[] micCubes; //MIC BUTTONS on the observation and SIRE panels are controlled from here.

    void Start()
    {
#if UNITY_ANDROID
    foreach (GameObject btn in micButtons)
    {
        if (btn.activeSelf)
            btn.SetActive(false);
    }
    foreach (GameObject cube in micCubes)
    {
        if (cube.activeSelf)
            cube.SetActive(false);
    }
#else
        foreach (GameObject btn in micButtons)
        {
            if (!btn.activeSelf)
                btn.SetActive(true);
        }
        foreach (GameObject cube in micCubes)
        {
            if (!cube.activeSelf)
                cube.SetActive(true);
        }
#endif
    }
}