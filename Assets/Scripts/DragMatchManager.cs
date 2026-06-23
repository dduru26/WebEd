using UnityEngine;
using UnityEngine.InputSystem;   // NEW Input System, same as PhaseInteractor

// Sticky-note feature, stage 1 of 4: PICK UP.
// Attach to an empty GameObject named "DragMatchManager".
// This slice only detects and grabs a note. Drag, drop/match, and the win
// hookup get added to this same script in the next roadmap stages.
public class DragMatchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;            // your fixed scene camera

    [Header("Settings")]
    [SerializeField] private float maxDistance = 50f;

    private Note heldNote;                           // the note currently picked up

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (cam == null || Pointer.current == null) return;

        // --- PICK UP: on press, raycast and grab a Note if we hit one ---
        if (heldNote == null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 pointerPos = Pointer.current.position.ReadValue();
            Ray ray = cam.ScreenPointToRay(pointerPos);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                Note note = hit.collider.GetComponent<Note>();
                if (note != null && !note.placed)        // ignore already-matched notes
                {
                    heldNote = note;
                    Debug.Log("Picked up: " + note.name); // proof of life; remove later
                }
            }
        }

        // --- RELEASE: placeholder until the drop/match stage replaces it ---
        if (heldNote != null && Pointer.current.press.wasReleasedThisFrame)
        {
            Debug.Log("Released: " + heldNote.name);
            heldNote = null;
        }
    }
}