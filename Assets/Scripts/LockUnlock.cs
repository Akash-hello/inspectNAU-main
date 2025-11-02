using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockUnlock : MonoBehaviour
{
    public Gaze gazescript;
    public Button play;
    public GameObject Play;
    public GameObject pause;
    
    private void Start()
    {
        Play.SetActive(true);
        pause.SetActive(false);
    }
    
    public void LockUnlockGaze()

    {
        if (Play.activeSelf)
        {
            Play.SetActive(false);
            pause.SetActive(true);
        }

        else if (pause.activeSelf)
        {
            Play.SetActive(true);
            pause.SetActive(false);
        }

        gazescript.enabled = !gazescript.enabled;
    }
}
