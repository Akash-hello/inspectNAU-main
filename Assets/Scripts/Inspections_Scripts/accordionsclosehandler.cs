using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class accordionsclosehandler : MonoBehaviour
{
    public GameObject downarrowbutton;
    public GameObject quesdownarrowbutton;
    public Button pendingquesButton;

    //This is for the Chapter Accordions
    public void CollapseOpenAccordions ()

    {
        GameObject[] openaccordions = GameObject.FindGameObjectsWithTag("openaccordion");
        foreach (GameObject accordion in openaccordions)
        {
            var currentButton = accordion.GetComponent<Button>();
            if (currentButton != null)
            {
                currentButton.onClick.Invoke();
                currentButton.tag = "Untagged";
            }
            //accordion.gameObject.GetComponent<Button>().onClick.Invoke();
            //accordion.gameObject.tag = "Untagged";
        }

        AssignTagowndownbtn();
    }

    public void AssignTagowndownbtn()
    {
        downarrowbutton.gameObject.tag = "openaccordion";
        pendingquesButton.GetComponent<Button>().interactable = false;
    }

    public void Downarrowclicked()
    {
        downarrowbutton.gameObject.tag = "Untagged";
        if (pendingquesButton.GetComponent<Image>().color == Color.green)
        {
            pendingquesButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            pendingquesButton.GetComponent<Button>().interactable = true;
        }
        
    }

    //This area underneath takes care of closing of the Questions Accordions to be collapsed

    public void CollapseQuesAccordions()

    {
        GameObject[] openaccordions = GameObject.FindGameObjectsWithTag("quesopenaccordion");
        foreach (GameObject accordion in openaccordions)
        {
            accordion.gameObject.GetComponent<Button>().onClick.Invoke();
            accordion.gameObject.tag = "Untagged";
        }
        AssignDownArrowTag();
    }

    public void AssignDownArrowTag()
    {
        quesdownarrowbutton.gameObject.tag = "quesopenaccordion";
    }

    public void QuestionsDownArrow()
    {
        quesdownarrowbutton.gameObject.tag = "Untagged";
    }
}
