using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InspectionListManager : MonoBehaviour
{
    public GameObject inspectionRowPrefab;  // Assign in Inspector
    public Transform contentContainer;      // Assign ScrollView Content
    public GameObject inspectionDetailsPanel; // Reference to details panel

    private List<Inspection1> inspections = new List<Inspection1>();

    public void LoadInspections(List<Inspection1> fetchedInspections)
    {
        // Clear previous entries
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        inspections = fetchedInspections;

        foreach (var insp in inspections)
        {
            GameObject row = Instantiate(inspectionRowPrefab, contentContainer);
            row.transform.Find("IMOText").GetComponent<TMP_Text>().text = insp.imo;
            row.transform.Find("DateText").GetComponent<TMP_Text>().text = insp.Date;
            row.transform.Find("PortText").GetComponent<TMP_Text>().text = insp.Port;
            row.transform.Find("InspectorText").GetComponent<TMP_Text>().text = insp.inspector;
            row.transform.Find("DeficienciesText").GetComponent<TMP_Text>().text = insp.deficiencies.ToString();
            row.transform.Find("StatusText").GetComponent<TMP_Text>().text = insp.Status;

            // Button Click Event to Load Details
            Button selectBtn = row.transform.Find("SelectButton").GetComponent<Button>();
            selectBtn.onClick.AddListener(() => ShowInspectionDetails(insp));
        }
    }

    private void ShowInspectionDetails(Inspection1 inspection)
    {
        // Activate Details Panel
        inspectionDetailsPanel.SetActive(true);
        inspectionDetailsPanel.transform.Find("IMOText").GetComponent<TMP_Text>().text = inspection.imo;
        inspectionDetailsPanel.transform.Find("DateText").GetComponent<TMP_Text>().text = inspection.Date;
        inspectionDetailsPanel.transform.Find("PortText").GetComponent<TMP_Text>().text = inspection.Port;
        inspectionDetailsPanel.transform.Find("InspectorText").GetComponent<TMP_Text>().text = inspection.inspector;
        inspectionDetailsPanel.transform.Find("DeficienciesText").GetComponent<TMP_Text>().text = inspection.deficiencies.ToString();
    }
}

// Sample Data Structure
[System.Serializable]
public class Inspection1
{
    public string imo;
    public string Date;
    public string Port;
    public string inspector;
    public int deficiencies;
    public string Status;
}
