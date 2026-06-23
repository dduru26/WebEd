using UnityEngine;
using UnityEngine.InputSystem;   // NEW Input System
using TMPro;

// Sticky-note feature: pick up, drag, drop/match, plus tap-to-read.
// A short press that barely moves = read the note. A press-and-drag = match it.
public class DragMatchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;                // your fixed scene camera
    [SerializeField] private NoteReader noteReader;     // overlay that shows a note's text enlarged

    [Header("Settings")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float liftHeight = 0.15f;  // how high a note floats while dragged
    [SerializeField] private float snapHeight = 0f;     // how high a matched note rests above its object
    [SerializeField] private float tapThreshold = 10f;  // pixels of movement below which a press counts as a tap

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

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(pointerPos);

        // --- PRESS: identify the note under the pointer ---
        if (Pointer.current.press.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                Note note = hit.collider.GetComponentInParent<Note>();
                if (note != null)
                {
                    tappedNote = note;
                    pressPos = pointerPos;
                    if (!note.placed)                   // only unplaced notes can be dragged
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
            Debug.Log("Correct! " + correctCount + "/5");
        }
        else
        {
            heldNote.transform.position = heldNote.homePosition;
        }
    }

    private string GetNoteText(Note n)
    {
        var t = n.GetComponentInChildren<TMP_Text>();
        return t != null ? t.text : n.name;
    }
}