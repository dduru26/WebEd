using UnityEngine;
using UnityEngine.InputSystem;   // NEW Input System

// Sticky-note feature, now stages 1-2: PICK UP + DRAG.
// Drop/match and the win hookup get added next.
public class DragMatchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;            // your fixed scene camera

    [Header("Settings")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float liftHeight = 0.15f;  // how far above the desk the note floats while held

    private Note heldNote;       // the note currently picked up
    private Plane dragPlane;     // the invisible surface the pointer is projected onto

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (cam == null || Pointer.current == null) return;

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(pointerPos);

        // --- PICK UP: on press, grab a Note the ray hits ---
        if (heldNote == null && Pointer.current.press.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                Note note = hit.collider.GetComponent<Note>();
                if (note != null && !note.placed)
                {
                    heldNote = note;
                    // horizontal plane at the note's current height -> it slides along the desk
                    dragPlane = new Plane(Vector3.up, heldNote.transform.position);
                }
            }
        }

        // --- DRAG: while held, project the pointer onto the plane and move the note ---
        if (heldNote != null && Pointer.current.press.isPressed)
        {
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                point.y += liftHeight;                  // float it just above the surface
                heldNote.transform.position = point;
            }
        }

        // --- RELEASE: placeholder until the drop/match stage replaces it ---
        if (heldNote != null && Pointer.current.press.wasReleasedThisFrame)
        {
            heldNote = null;                            // for now it just stays where dropped
        }
    }
}