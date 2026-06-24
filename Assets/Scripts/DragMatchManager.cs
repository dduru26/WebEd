using UnityEngine;
using UnityEngine.InputSystem;   // NEW Input System
using TMPro;
using UnityEngine.EventSystems;   // to detect when the pointer is over UI

// Sticky-note feature: pick up, drag, drop/match, tap-to-read, and the win reaction.
// A short press that barely moves = read the note. A press-and-drag = match it.
public class DragMatchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;                
    [SerializeField] private NoteReader noteReader;     

    [Header("Settings")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float liftHeight = 0.15f;  // how high a note floats while dragged
    [SerializeField] private float snapHeight = 0f;     // how high a matched note rests above its object
    [SerializeField] private float tapThreshold = 10f;  // pixels of movement below which a press counts as a tap

    [Header("Win")]
    [SerializeField] private ParticleSystem confetti;   
    [SerializeField] private GameObject winText;        // optional "all matched" message, hidden by default
    [SerializeField] private int totalNotes = 5;

    private Note heldNote;       // unplaced note being dragged
    private Note tappedNote;     // note under the press point (placed or not) for tap-to-read
    private Vector2 pressPos;
    private Plane dragPlane;
    private int correctCount;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (cam == null || Pointer.current == null) return;

        // If the pointer is over a UI element, don't start a world interaction.
        bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(pointerPos);

        // --- PRESS: identify the note under the pointer ---
        if (!overUI && Pointer.current.press.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                Note note = hit.collider.GetComponentInParent<Note>();
                if (note != null)
                {
                    tappedNote = note;
                    pressPos = pointerPos;
                    if (!note.placed)                   
                    {
                        heldNote = note;
                        dragPlane = new Plane(Vector3.up, note.transform.position);
                    }
                }
            }
        }

        // --- DRAG: move only once the pointer has passed the tap threshold ---
        if (heldNote != null && Pointer.current.press.isPressed)
        {
            if ((pointerPos - pressPos).magnitude > tapThreshold &&
                dragPlane.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                point.y += liftHeight;
                heldNote.transform.position = point;
            }
        }

        // --- RELEASE: tap = read, drag = match ---
        if (tappedNote != null && Pointer.current.press.wasReleasedThisFrame)
        {
            bool wasTap = (pointerPos - pressPos).magnitude <= tapThreshold;

            if (wasTap)
            {
                if (noteReader != null) noteReader.Show(GetNoteText(tappedNote));
            }
            else if (heldNote != null)
            {
                ResolveDrop();
            }

            heldNote = null;
            tappedNote = null;
        }
    }

    private void ResolveDrop()
    {
        DesignPhaseObject target = heldNote.currentTarget;

        bool correct = target != null
                       && target.data != null
                       && target.data.phaseOrder == heldNote.matchOrder;

        if (correct)
        {
            heldNote.transform.position = target.transform.position + Vector3.up * snapHeight;
            heldNote.placed = true;
            heldNote.currentTarget = null;
            correctCount++;
            Debug.Log("Correct! " + correctCount + "/" + totalNotes);

            if (correctCount >= totalNotes) Win();
        }
        else
        {
            heldNote.transform.position = heldNote.homePosition;
        }
    }

    private void Win()
    {
        if (confetti != null) confetti.Play();
        if (winText != null) winText.SetActive(true);
    }

    private string GetNoteText(Note n)
    {
        var t = n.GetComponentInChildren<TMP_Text>();
        return t != null ? t.text : n.name;
    }
}