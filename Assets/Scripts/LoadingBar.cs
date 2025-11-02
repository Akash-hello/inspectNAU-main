using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingBar : MonoBehaviour {

    public Image loadBar;
	// Use this for initialization
	void Start () {
        loadBar.fillAmount = 0f;
        
        StartCoroutine(loader());
    }
	
	//// Update is called once per frame
	//void Update () {
 //       loadBar.fillAmount += 0.008f;

 //       if(loadBar.fillAmount==1f)
 //       {
 //          SceneManager.LoadScene("InspectionsMainScene", LoadSceneMode.Single);
 //       }
       
 //   }

    // Update is called once per frame
    IEnumerator loader()


    {

        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InspectionsMainScene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("2_InspectionsMainScene_LandScape");
        
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadBar.fillAmount = asyncLoad.progress; // Update progress bar
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Activate scene once loading is complete
            }
            yield return null;
        }



        //float time = 0.5f;
        //yield return new WaitForSeconds(time);
    
        ////loadBar.fillAmount += 0.008f;
        //loadBar.fillAmount += 1f;

        //if (loadBar.fillAmount == 1f)
        //{
        //    //SceneManager.LoadScene("InspectionsMainScene", LoadSceneMode.Additive);
        //    SceneManager.LoadScene("InspectionsMainScene");
        //   // SceneManager.UnloadSceneAsync("loader");
        //}
       
    }
}
