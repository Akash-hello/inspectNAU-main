using UnityEngine;
using UnityEngine.UI;          // CanvasScaler, GridLayoutGroup, Horizontal/VerticalLayoutGroup
#if UNITY_EDITOR
using UnityEditor;
#endif

/// Attach this to the same GameObject that has your Canvas + CanvasScaler.
/// It will NOT touch mobile; on Windows/macOS it only adjusts scaler percentages
/// and (optionally) a grid's columns so the UI breathes in landscape.
public class ResponsiveScalar : MonoBehaviour
{
    [Header("Scaler profiles")]
    // Portrait-first (your current mobile)
    public Vector2 mobileReference = new Vector2(1080, 1920);
    [Range(0, 1)] public float mobileMatch = 1f;  // 1 = match height

    // Desktop landscape
    public Vector2 desktopReference = new Vector2(1920, 1080);
    [Range(0, 1)] public float desktopMatch = 0.3f; // bias width on desktop

    [Header("Optional layout tuning (desktop only)")]
    public GridLayoutGroup cardsGrid;           // e.g., the area with your tiles/cards
    public int desktopColumns = 3;              // spread wider
    public int mobileColumns = 1;              // keep stacked on phones

    [Tooltip("Extra scale applied to the Canvas when on desktop (1 = none).")]
    public float desktopExtraScale = 1.0f;

    CanvasScaler scaler;
    Canvas canvas;

    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        canvas = GetComponent<Canvas>();

        if (!scaler || !canvas)
        {
            Debug.LogWarning("ResponsiveScaler: Canvas/CanvasScaler not found on this GameObject.");
            return;
        }

#if UNITY_STANDALONE
        ApplyDesktopProfile();
#else
        ApplyMobileProfile();
#endif
    }

    void ApplyMobileProfile()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = mobileReference;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = mobileMatch;

        if (cardsGrid != null)
        {
            cardsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            cardsGrid.constraintCount = Mathf.Max(1, mobileColumns);
        }

        // Ensure no manual scale applied
        canvas.scaleFactor = 1f;
        transform.localScale = Vector3.one;
    }

    void ApplyDesktopProfile()
    {
        // Fullscreen if you want (optional)
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = false;

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = desktopReference;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = desktopMatch;

        if (cardsGrid != null)
        {
            cardsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            cardsGrid.constraintCount = Mathf.Max(2, desktopColumns);
        }

        // Gentle global bump if needed (keeps proportions)
        if (!Mathf.Approximately(desktopExtraScale, 1f))
            transform.localScale = Vector3.one * desktopExtraScale;
    }
}
