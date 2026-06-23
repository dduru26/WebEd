using UnityEngine;

// On each draggable sticky note (the thin cube).
// matchOrder must equal the phaseOrder of the DesignPhaseObject this note belongs to
// (Empathize = 1 ... Test = 5).
public class Note : MonoBehaviour
{
    public int matchOrder;                              // 1-5, the phase this note answers

    [HideInInspector] public Vector3 homePosition;      // where it snaps back to if dropped wrong
    [HideInInspector] public bool placed;               // true once matched correctly
    [HideInInspector] public DesignPhaseObject currentTarget;  // target it's overlapping right now

    void Awake()
    {
        homePosition = transform.position;
    }

    // GetComponentInParent (not GetComponent) so it still finds the script when the
    // collider is on a child mesh of the model - this is what fixes the keyboard.
    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<DesignPhaseObject>();
        if (target != null) currentTarget = target;     // ignore floor, walls, other notes
    }

    void OnTriggerExit(Collider other)
    {
        var target = other.GetComponentInParent<DesignPhaseObject>();
        if (target != null && target == currentTarget) currentTarget = null;
    }
}