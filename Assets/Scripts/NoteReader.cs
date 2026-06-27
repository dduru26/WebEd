using UnityEngine;
using TMPro;

// Shows a single note's text in an enlarged overlay.
// Put this on the Canvas (or any object) and wire its two slots.
public class NoteReader : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot; 
    [SerializeField] private TMP_Text noteText;     

    public void Show(string text)
    {
        if (noteText != null) noteText.text = text;
        if (panelRoot != null) panelRoot.SetActive(true);
    }

    public void Hide()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
    }
}