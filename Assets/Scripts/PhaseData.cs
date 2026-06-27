using UnityEngine;

// Holds the text content for one design-thinking phase.
// Create assets via: right-click in Project window > Create > DesignStudio > PhaseData
[CreateAssetMenu(fileName = "PhaseData", menuName = "DesignStudio/PhaseData")]
public class PhaseData : ScriptableObject
{
    public string phaseName;                       
    [TextArea(5, 12)] public string description;   
    public int phaseOrder;                         
}
