using UnityEngine;
using UnityEngine.InputSystem;     // NEW Input System
using UnityEngine.EventSystems;    // to detect when the pointer is over UI

// Attach to the fixed scene camera.
// Raycasts to a tagged desk object on click and opens its info panel; also tints on hover.
public class PhaseInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private PanelController panel;

    [Header("Settings")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private string phaseTag = "DesignPhase";
    [SerializeField] private Color highlightColor = new Color(1f, 0.85f, 0.3f);

    private Renderer hoveredRenderer;
    private Color originalColor;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (cam == null || Pointer.current == null) return;

        // If the pointer is over a UI element (a panel or a Close button),
        // don't let the click fall through into the 3D scene.
        bool overUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(pointerPos);

        // --- HOVER HIGHLIGHT ---
        if (!overUI && Physics.Raycast(ray, out RaycastHit hover, maxDistance) &&
            hover.collider.CompareTag(phaseTag))
        {
            SetHover(hover.collider.GetComponent<Renderer>());
        }
        else
        {
            ClearHover();
        }

        // --- CLICK TO OPEN PANEL ---
        if (!overUI && Pointer.current.press.wasPressedThisFrame)
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
        if (r == hoveredRenderer) return;
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