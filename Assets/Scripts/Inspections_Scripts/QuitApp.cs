using UnityEngine;

public class QuitApp : MonoBehaviour
{
    // Call from UI Button
    public void OnQuitClicked()
    {
        StartCoroutine(QuitRoutine());
    }

    // Optional: keyboard shortcuts (Esc to prompt, Cmd+Q on macOS)
    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Escape))
            OnQuitClicked();

#if UNITY_STANDALONE_OSX
        if (Input.GetKey(KeyCode.LeftCommand) && Input.GetKeyDown(KeyCode.Q))
            OnQuitClicked();
#endif
#endif
    }

    System.Collections.IEnumerator QuitRoutine()
    {
        // TODO: put any “save/flush/close DB” code here if needed
        // e.g., Savefile();  Logs.Flush();  DB.Close();

        yield return null; // let last UI frame render

#if UNITY_EDITOR
        // Stop play mode in Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
