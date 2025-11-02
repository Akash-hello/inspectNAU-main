using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance;
    public GameObject photoPanel;
    public Transform photoParent;
    public GameObject photoPrefab;

    private Dictionary<string, List<PhotoData>> regionPhotos = new Dictionary<string, List<PhotoData>>();
    private string currentRegion;

    private void Awake() { Instance = this; }

    public void OpenPhotoPanel(string regionName)
    {
        currentRegion = regionName;
        photoPanel.SetActive(true);
        LoadPhotos();
    }

    public void UploadPhoto(Sprite newPhoto, string photoName, string observation, float grade)
    {
        if (!regionPhotos.ContainsKey(currentRegion))
            regionPhotos[currentRegion] = new List<PhotoData>();

        PhotoData newPhotoData = new PhotoData
        {
            photo = newPhoto,
            name = photoName,
            observation = observation,
            grade = grade
        };

        regionPhotos[currentRegion].Add(newPhotoData);
        DisplayPhoto(newPhotoData);
    }

    private void LoadPhotos()
    {
        foreach (Transform child in photoParent)
            Destroy(child.gameObject);

        if (regionPhotos.ContainsKey(currentRegion))
        {
            foreach (PhotoData photoData in regionPhotos[currentRegion])
                DisplayPhoto(photoData);
        }
    }

    private void DisplayPhoto(PhotoData photoData)
    {
        GameObject newPhoto = Instantiate(photoPrefab, photoParent);
        newPhoto.transform.Find("Image").GetComponent<Image>().sprite = photoData.photo;
        newPhoto.transform.Find("Name").GetComponent<Text>().text = photoData.name;
        newPhoto.transform.Find("Observation").GetComponent<Text>().text = photoData.observation;
        newPhoto.transform.Find("Grade").GetComponent<Text>().text = "Grade: " + photoData.grade;
    }

    public Dictionary<string, List<PhotoData>> GetPhotoData() { return regionPhotos; }
}

[System.Serializable]
public class PhotoData
{
    public Sprite photo;
    public string name;
    public string observation;
    public float grade;
}
