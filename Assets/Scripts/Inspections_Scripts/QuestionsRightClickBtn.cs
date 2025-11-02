using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionsRightClickBtn : MonoBehaviour
{
    public TextMeshProUGUI Questions_Text;
    public GameObject AnswerGroup;
    public TextMeshProUGUI Insp_Type;
    public GameObject Sire20_Categories;
    public GameObject Sire20_ObservationCntr;
    //public GameObject parentScrollRectObj;
    //public ScrollRect parentScrollRect; //WORKING ON SCROLL RECT HERE FOR RIGHT CLICK, DELETE COmment once done.

    public VoiceController voicecontrol;

    public GameObject answertoggles;
    ToggleGroup answertogglegroup;
    Toggle toggle;

    public List<Toggle> m_Toggles = new List<Toggle>();
    int toggleindex; // Toggles on the Questions Prefab -- SIRE 2.0, Insp, Condn and Audits - 0_AsExpected_Toggle | 0_No_Deficiency_Toggle | 0_Good_Toggle , 1_Not_As_Expected_Toggle |1_Deficiency_Toggle , 2_Poor_Toggle, 3_NotSeen_Toggle, 4_NA_Toggle, 5_Yes_Toggle, 6_No_Toggle, 7_NotSeen_Toggle, 9_Satisfactory_Toggle, 10_MajorNC_Toggle,11_MinorNC_Toggle, 12_OBS_Toggle, 13_NotSeen_Toggle
  

    public void Start()
    {
        voicecontrol = new VoiceController();
    }

    public void checkifQuestion ()
    {
        //parentScrollRectObj = GameObject.FindWithTag("inspectionsblock");
        //parentScrollRectObj.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        if (Questions_Text.text.Contains('?')) //THIS CHECKS WHETHER TO SHOW THE ANSEWER GROUP OR NOT BASED ON THE QUESTION
        {
            AnswerGroup.SetActive(true);
        }

        else
        {
            AnswerGroup.SetActive(false);
        }
            
        if ((Insp_Type.text.ToString().ToLower()).Trim().Replace(" ", "").Contains("(sire2.0)") && Questions_Text.text.Contains('?'))
        {
            Sire20_Categories.SetActive(true);
            Sire20_ObservationCntr.SetActive(true);

            int[] indicesToNull = { 5,6 };
           
            foreach (int index in indicesToNull)
            {
                if (index < m_Toggles.Count) // Ensure index is within bounds
                {
                    m_Toggles[index].interactable = false;
                }
            }

            //Sire20_ObservationCntr.GetComponent<LayoutElement>().preferredHeight = 300f;
        }
        else
        {
            Sire20_Categories.SetActive(false);
            Sire20_ObservationCntr.SetActive(false);
            //Sire20_ObservationCntr.GetComponent<LayoutElement>().preferredHeight = 0f;
        }
    }

    //IMPORTANT; if you go looking how the RIGHT DOWN ARROW works to open the observation panel, read this;
    //THE LISTENER FOR OPENING QUESTIONS AND STANDARD PHOTO PREFABS HAVE BEEN ADDED TO THE "CreatingObsTableForInsp" script instead of the inspector, since both required different methods to be called.
}
