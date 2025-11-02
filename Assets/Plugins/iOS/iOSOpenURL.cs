using UnityEngine;
using System.Runtime.InteropServices;

public class iOSOpenURL : MonoBehaviour
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _openPDF(string filePath);
#endif

    public void OpenPDF(string filePath)
    {
#if UNITY_IOS
        _openPDF(filePath);
#endif
    }
}

