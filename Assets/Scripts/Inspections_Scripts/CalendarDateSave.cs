using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;
//using Unity.XR.CoreUtils;
using UnityEngine.Pool;
using System.Globalization;

public class CalendarDateSave : MonoBehaviour
{
    public TMP_InputField inputFieldCo;
    public RectTransform DatePickerfPanel;
    public GameObject DatePickerObj;
    public GameObject DatePickerPrefab;
    public GameObject[] Calendars;
    public GameObject savebtn;
    public GameObject closebtn;
    Vector3 orginalPosition;
    //public GameObject backpanel;
    //public GameObject mainbackground;

    public bool obsdatepicker;

    public void Obsdatepickeropen()
    {
        obsdatepicker = true;
        openDatePicker();
    }

    public void openDatePicker()
    {
      
        Calendars = GameObject.FindGameObjectsWithTag("datepicker");

        foreach (GameObject Calendar in Calendars)
        {
            Calendar.GetComponent<Button>().interactable = false;
        }

       
        //inputFieldCo = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.GetComponent<TMP_InputField>();
        DatePickerfPanel.gameObject.SetActive(true);
        for (int i = 0; i < DatePickerfPanel.transform.childCount; ++i) { Destroy(DatePickerfPanel.transform.GetChild(i).gameObject); }
        DatePickerObj = Instantiate(DatePickerPrefab);
        DatePickerObj.transform.SetParent(DatePickerfPanel, false);
        
        savebtn.transform.SetParent(DatePickerObj.transform, false);


        if (obsdatepicker == true)
        {
            savebtn.transform.localPosition = new Vector3(100, -200, 0);
        }
        else
        {
            savebtn.transform.localPosition = new Vector3(-80, -360, 0);
        }
        orginalPosition = savebtn.gameObject.transform.position;

        if (inputFieldCo.text =="")
        {
            Debug.Log("Blank input field found....");
            DatePickerObj.GetComponent<DatePickerControl>().fecha = System.DateTime.Now;
        }

        else if (DatePickerObj.gameObject.name.Contains("DateTimePicker"))
        {
            //string format = "ddd dd-MM-yyyy HH:mm";

            string[] formats = { "ddd dd-MM-yyyy HH:mm", "ddd dd/MM/yyyy HH:mm", "dd-MM-yyyy HH:mm", "dd/MM/yyyy HH:mm",
             "ddd-dd-MM-yyyy HH:mm",
                "MM/dd/yyyy HH:mm",
                "yyyy-MM-dd HH:mm",
                "ddd, dd MMM yyyy HH:mm" };

            DateTime parsedDate;
            bool success = DateTime.TryParseExact(inputFieldCo.text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

            if (success)
            {
                DatePickerObj.GetComponent<DatePickerControl>().fecha = parsedDate;
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.day.text = parsedDate.Day.ToString();
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.month.text = parsedDate.Month.ToString();
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.year.text = parsedDate.Year.ToString();
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldTime.hour.text = parsedDate.Hour.ToString();
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldTime.minute.text = parsedDate.Minute.ToString();

                Debug.Log("This date and time has been selected...." + inputFieldCo.text);
                Debug.Log("Parsed Date: " + parsedDate);
            }
            else
            {
                DatePickerObj.GetComponent<DatePickerControl>().fecha = System.DateTime.Now;
                Debug.LogError("Failed to parse the date");
            }

        }

        else if (DatePickerObj.gameObject.name.Contains("DatePicker"))
        {

            string[] formats = {"ddd dd/MM/yyyy 'Hrs'","dd/MM/yyyy 'Hrs'","ddd/dd/MM/yyyy 'Hrs'",
                "ddd-dd-MM-yyyy 'Hrs'",
                "ddd dd-MM-yyyy 'Hrs'",
                "dd-MM-yyyy 'Hrs'",
                "MM/dd/yyyy 'Hrs'",
                "yyyy-MM-dd 'Hrs'",
                "ddd, dd MMM yyyy 'Hrs'" }; // Add more if necessary

            DateTime parsedDate;
            bool successdate = DateTime.TryParseExact(inputFieldCo.text.Trim() + " Hrs", formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);

            // Attempt to parse the input string to DateTime object
            if (successdate)
            {
                // Extract the required components and update DatePickerObj
                DatePickerObj.GetComponent<DatePickerControl>().fecha = parsedDate;
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.day.text = parsedDate.Day.ToString();
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.month.text = parsedDate.Month.ToString();
                DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.year.text = parsedDate.Year.ToString();

                Debug.Log("This date and time has been selected: " + inputFieldCo.text);
            }
            else
            {
                Debug.LogError("Failed to parse the date. Check the input format or device regional settings.");
                DatePickerObj.GetComponent<DatePickerControl>().fecha = System.DateTime.Now;
            }


            // Define the format of the input string
            //string format = "ddd-dd-MM-yyyy 'Hrs'";

            //// Parse the input string to DateTime object
            //DateTime parsedDate = DateTime.ParseExact(inputFieldCo.text.Trim()+ " Hrs", format, CultureInfo.InvariantCulture);

            //// Extract the required components

            //DatePickerObj.GetComponent<DatePickerControl>().fecha = parsedDate;
            //DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.day.text = parsedDate.Day.ToString();
            //DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.month.text = parsedDate.Month.ToString();
            //DatePickerObj.GetComponent<DatePickerControl>().inputFieldDate.year.text = parsedDate.Year.ToString();

            //Debug.Log("This date and time has been selected...." + inputFieldCo.text);
        }


        closebtn.transform.SetParent(DatePickerObj.transform, false);

        if (obsdatepicker == true)
        {
            closebtn.transform.localPosition = new Vector3(385, 190, 0);
        }
        else
        {
            closebtn.transform.localPosition = new Vector3(300, 190, 0);
        }


        
        //backpanel.GetComponent<Image>().color = Color.grey;
        //mainbackground.GetComponent<Image>().color = Color.grey;
    }

    public void DateCalSaveBtn()
    {
        savebtn.transform.SetParent (this.gameObject.transform, false);
        savebtn.gameObject.transform.position = orginalPosition;
        closebtn.transform.SetParent(this.gameObject.transform, false);
        closebtn.gameObject.transform.position = orginalPosition;

        //inputFieldCo = Calendar.transform.parent.gameObject.GetComponent<TMP_InputField>();
        inputFieldCo.text = DatePickerObj.transform.Find("DateTime").GetComponentInChildren<Text>().text.ToString();
        for (int i = 0; i < DatePickerfPanel.transform.childCount; ++i) { Destroy(DatePickerfPanel.transform.GetChild(i).gameObject); }

        Calendars = GameObject.FindGameObjectsWithTag("datepicker");

        foreach (GameObject Calendar in Calendars)
        {
            Calendar.GetComponent<Button>().interactable = true;
        }
        DatePickerfPanel.gameObject.SetActive(false);
        Debug.Log(inputFieldCo.text.ToString());
        //backpanel.GetComponent<Image>().color = Color.white;
        //mainbackground.GetComponent<Image>().color = Color.white;
        obsdatepicker = false;
    }

    public void DateCalCloseBtn()
    {
        savebtn.transform.SetParent(this.gameObject.transform, false);
        savebtn.gameObject.transform.position = orginalPosition;
        closebtn.transform.SetParent(this.gameObject.transform, false);
        closebtn.gameObject.transform.position = orginalPosition;
        
        for (int i = 0; i < DatePickerfPanel.transform.childCount; ++i) { Destroy(DatePickerfPanel.transform.GetChild(i).gameObject); }

        Calendars = GameObject.FindGameObjectsWithTag("datepicker");

        foreach (GameObject Calendar in Calendars)
        {
            Calendar.GetComponent<Button>().interactable = true;
        }
        DatePickerfPanel.gameObject.SetActive(false);
        Debug.Log(inputFieldCo.text.ToString());
        //backpanel.GetComponent<Image>().color = Color.white;
        //mainbackground.GetComponent<Image>().color = Color.white;
        obsdatepicker = false;
    }

    public void cleardatetime()
    {
        inputFieldCo.text = "";
    }
}
