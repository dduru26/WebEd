using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class JourneyLine : MonoBehaviour
{
    [SerializeField] private Transform[] phasePoints;       // 5 objects, IN process order
    [SerializeField] private float heightOffset = 0.15f;    // lift line above the objects

    void Start()
    {
        var lr = GetComponent<LineRenderer>();
        if (phasePoints == null || phasePoints.Length == 0) return;

        lr.positionCount = phasePoints.Length;
        for (int i = 0; i < phasePoints.Length; i++)
        {
            if (phasePoints[i] != null)
                lr.SetPosition(i, phasePoints[i].position + Vector3.up * heightOffset);
        }
    }
}
