using UnityEngine;
using UnityEngine.InputSystem;   // NEW Input System

// Attach to the player's CAMERA (the FPS prefab's camera), or to a fixed camera
// if you remove movement for mobile.
//
// Uses Pointer.current instead of Mouse.current so the SAME build responds to
// both a desktop mouse click (WebGL) and a finger tap (Android). Pointer is the
// base class for Mouse, Touchscreen and Pen.
//
// Handles BOTH required mechanics:
//   - Raycasting (detects which object you clicked / tapped)
//   - Hover highlight (extra feature: tints the object under the cursor on desktop)
public class PhaseInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;               // drag the camera here
    [SerializeField] private PanelController panel;     // drag the Canvas panel controller here

    [Header("Settings")]
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private string phaseTag = "DesignPhase";
    [SerializeField] private Color highlightColor = new Color(1f, 0.85f, 0.3f);

    // hover state
    private Renderer hoveredRenderer;
    private Color originalColor;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (cam == null || Pointer.current == null) return;

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(pointerPos);

        // --- HOVER HIGHLIGHT (desktop nicety; on touch it just tints the last tap) ---
        if (Physics.Raycast(ray, out RaycastHit hover, maxDistance) &&
            hover.collider.CompareTag(phaseTag))
        {
            SetHover(hover.collider.GetComponent<Renderer>());
        }
        else
        {
            ClearHover();
        }

        // --- CLICK / TAP TO OPEN PANEL ---
        // Pointer.press covers mouse left-button AND a finger touch.
        if (Pointer.current.press.wasPressedThisFrame)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance) &&
                hit.collider.CompareTag(phaseTag))
            {
                var obj = hit.collider.GetComponent<DesignPhaseObject>();
                if (obj != null && obj.data != null && panel != null)
                    panel.Show(obj.data);
            }
        }
    }

    private void SetHover(Renderer r)
    {
        if (r == hoveredRenderer) return;   // already highlighting this one
        ClearHover();
        if (r == null) return;

        hoveredRenderer = r;
        originalColor = r.material.color;
        r.material.color = highlightColor;
    }

    private void ClearHover()
    {
        if (hoveredRenderer != null)
        {
            hoveredRenderer.material.color = originalColor;
            hoveredRenderer = null;
        }
    }
}