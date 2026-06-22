using UnityEngine;

// Holds the text content for one design-thinking phase.
// Create assets via: right-click in Project window > Create > DesignStudio > PhaseData
[CreateAssetMenu(fileName = "PhaseData", menuName = "DesignStudio/PhaseData")]
public class PhaseData : ScriptableObject
{
    public string phaseName;                       // e.g. "Empathize"
    [TextArea(5, 12)] public string description;   // the explanatory paragraph
    public int phaseOrder;                         // 1-5, used by ProgressTracker
}
