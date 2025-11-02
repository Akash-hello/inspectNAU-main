using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public GameObject SirePnl;

    public Toggle HumanasExpectedToggle;
    public Toggle HumannotAsExpectedToggle;
    public Toggle ProcessasExpectedToggle;
    public Toggle ProcessnotAsExpectedToggle;
    public Toggle HardwareasExpectedToggle;
    public Toggle HardwarenotAsExpectedToggle;

    public GameObject SireHumanPnl;
    public GameObject SireProcessPnl;
    public GameObject SireHardwarePnl;

    public Toggle yesToggle;
    public Toggle noToggle;
    public Toggle NA_Toggle;
    public Toggle NotSeen_Toggle;


    public TMP_InputField observationdetail;

    private bool isInitialized = false; // Prevent triggers on scene load

    void Start()
    {
        // Add listeners to all "As Expected" and "Not As Expected" toggles
        HumanasExpectedToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        HumannotAsExpectedToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });

        ProcessasExpectedToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        ProcessnotAsExpectedToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });

        HardwareasExpectedToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        HardwarenotAsExpectedToggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });

        NA_Toggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        NotSeen_Toggle.onValueChanged.AddListener(delegate { OnToggleChanged(); });

        // Delay user interaction flag to prevent database values triggering change
        Invoke(nameof(EnableUserInteraction), 1f);
    }

    void EnableUserInteraction()
    {
        isInitialized = true;
    }

    void OnToggleChanged()
    {
        if (!isInitialized || !SirePnl.activeSelf) return; // Ignore DB values & ensure SirePnl is active


        bool humanSelected = SireHumanPnl.gameObject.activeSelf&& (HumanasExpectedToggle.isOn || HumannotAsExpectedToggle.isOn);
        bool processSelected = SireProcessPnl.gameObject.activeSelf && (ProcessasExpectedToggle.isOn || ProcessnotAsExpectedToggle.isOn);
        bool elementSelected = SireHardwarePnl.gameObject.activeSelf && (HardwareasExpectedToggle.isOn || HardwarenotAsExpectedToggle.isOn);

        // Count how many panels are actually active
        int activePanels = (SireHumanPnl.activeSelf ? 1 : 0) +
                           (SireProcessPnl.activeSelf ? 1 : 0) +
                           (SireHardwarePnl.activeSelf ? 1 : 0);

        // Count how many selected answers exist among active panels
        int answeredPanels = (humanSelected ? 1 : 0) + (processSelected ? 1 : 0) + (elementSelected ? 1 : 0);

        Debug.Log($"Active Panels: {activePanels}, Answered Panels: {answeredPanels}");

        // If NA or Not Seen is selected, deselect all Human, Process, and Element toggles
        if (NA_Toggle.isOn || NotSeen_Toggle.isOn)
        {
            Debug.Log("NA or Not Seen selected → Clearing all other selections.");

            if (SireHumanPnl.activeSelf)
            {
                HumanasExpectedToggle.isOn = false;
                HumannotAsExpectedToggle.isOn = false;
            }
            if (SireProcessPnl.activeSelf)
            {
                ProcessasExpectedToggle.isOn = false;
                ProcessnotAsExpectedToggle.isOn = false;
            }
            if (SireHardwarePnl.activeSelf)
            {
                HardwareasExpectedToggle.isOn = false;
                HardwarenotAsExpectedToggle.isOn = false;
            }

            return; // Exit to avoid further processing
        }


        // If at least one active panel is unanswered, don't change Yes/No yet
        if (answeredPanels < activePanels)
        {
            Debug.Log("Some active panels are unanswered. Keeping Yes/No unchanged.");
            return;
        }

        // If any "Not as Expected" toggle is ON, set NO
        if ((SireHumanPnl.gameObject.activeSelf && HumannotAsExpectedToggle.isOn) || (SireProcessPnl.gameObject.activeSelf && ProcessnotAsExpectedToggle.isOn) || (SireHardwarePnl.gameObject.activeSelf && HardwarenotAsExpectedToggle.isOn))
        {
            noToggle.isOn = true;
            yesToggle.isOn = false;
        }
        // If all three are "As Expected", set YES
        else
        {
            yesToggle.isOn = true;
            noToggle.isOn = false;
        }



    }

    void OnDestroy()
    {
        // Remove listeners to prevent memory leaks
        HumanasExpectedToggle.onValueChanged.RemoveAllListeners();
        HumannotAsExpectedToggle.onValueChanged.RemoveAllListeners();
        ProcessasExpectedToggle.onValueChanged.RemoveAllListeners();
        ProcessnotAsExpectedToggle.onValueChanged.RemoveAllListeners();
        HardwareasExpectedToggle.onValueChanged.RemoveAllListeners();
        HardwarenotAsExpectedToggle.onValueChanged.RemoveAllListeners();
    }
}



