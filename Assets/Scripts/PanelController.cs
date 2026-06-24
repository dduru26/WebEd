using UnityEngine;
using TMPro;


public class PanelController : MonoBehaviour
{
    [Header("Panel UI")]
    [SerializeField] private GameObject panelRoot;   // the Panel GameObject (starts inactive)
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;

    [Header("Optional")]
    [SerializeField] private ProgressTracker tracker;   // for the confetti win-state
    [SerializeField] private AudioSource clickSource;   // plays a click SFX (extra feature)
    [SerializeField] private AudioClip clickClip;

    public void Show(PhaseData data)
    {
        if (panelRoot != null) panelRoot.SetActive(true);
        if (titleText != null) titleText.text = data.phaseName;
        if (bodyText != null) bodyText.text = data.description;

        if (clickSource != null && clickClip != null)
            clickSource.PlayOneShot(clickClip);

        if (tracker != null) tracker.MarkViewed(data.phaseOrder);
    }

    // Wire this to the Close button's OnClick event.
    public void Hide()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
    }
}
