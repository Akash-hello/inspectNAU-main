using UnityEngine;
using UnityEngine.UI;

public class ShipRegionManage : MonoBehaviour
{
    public GameObject regionPrefab;
    public Transform regionParent;
    public Slider horizontalSlider, verticalSlider;
    public Button assignRegionsButton;
    public RawImage displayImage;
    public int maxHorizontalDivisions = 8;
    public int maxVerticalDivisions = 3;

    void Start()
    {
        //assignRegionsButton.onClick.AddListener(AssignRegions);
    }

    void AssignRegions()
    {
        int horizontalDivisions = Mathf.Clamp((int)horizontalSlider.value, 1, maxHorizontalDivisions);
        int verticalDivisions = Mathf.Clamp((int)verticalSlider.value, 1, maxVerticalDivisions);

        float regionWidth = displayImage.rectTransform.rect.width / horizontalDivisions;
        float regionHeight = displayImage.rectTransform.rect.height / verticalDivisions;

        int regionIndex = 1;
        for (int y = 0; y < verticalDivisions; y++)
        {
            for (int x = 0; x < horizontalDivisions; x++)
            {
                GameObject newRegion = Instantiate(regionPrefab, regionParent);
                RectTransform rt = newRegion.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(regionWidth, regionHeight);
                rt.anchoredPosition = new Vector2(x * regionWidth, -y * regionHeight);
                newRegion.GetComponentInChildren<Text>().text = regionIndex.ToString();
                int capturedIndex = regionIndex;
                newRegion.GetComponent<Button>().onClick.AddListener(() => OnRegionClicked(capturedIndex));
                regionIndex++;
            }
        }
    }

    void OnRegionClicked(int regionNumber)
    {
        Debug.Log("Region " + regionNumber + " clicked.");
    }
}
