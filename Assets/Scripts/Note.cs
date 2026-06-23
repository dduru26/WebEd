using UnityEngine;

// Put this on each draggable sticky note (the thin cube).
// matchOrder must equal the phaseOrder of the DesignPhaseObject this note
// belongs to (Empathize = 1, Define = 2, Ideate = 3, Prototype = 4, Test = 5).
// DragMatchManager reads it when the note is dropped to decide if the match is correct.
public class Note : MonoBehaviour
{
    public int matchOrder;                            // 1-5, the phase this note answers

    [HideInInspector] public Vector3 homePosition;    // where it snaps back to if dropped wrong
    [HideInInspector] public bool placed;             // true once it's been matched correctly

    void Awake()
    {
        homePosition = transform.position;            // remember the starting spot
    }
}