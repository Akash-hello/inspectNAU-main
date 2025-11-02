
using UnityEngine;
using UnityEngine.UI;

public class piechart : MonoBehaviour
{
    public Image[] imagesPieChart;
    //public RawImage[] Pieoutput;
    public float[] values;
   

    // Start is called before the first frame update
    void Start()
    {
        SetValues(values);
    }

    public void SetValues(float[] valuesToSet)
    {
        float totalvalues = 0;
        for (int i = 0;i < imagesPieChart.Length; i++)
        {
            totalvalues += FindPercentage(valuesToSet, i);
            imagesPieChart[i].fillAmount = totalvalues;
        //    Pieoutput[i].texture = imagesPieChart[i].mainTexture;
        //    Pieoutput[i].color = imagesPieChart[i].color;
        }

    

    }

    private float FindPercentage(float[] valuesToSet, int index)
    {
        float totalAmount = 0;
        for (int i = 0; i <valuesToSet.Length; i++)
        {
            totalAmount += valuesToSet[i];
        }

        return valuesToSet[index] / totalAmount;
    }
}
