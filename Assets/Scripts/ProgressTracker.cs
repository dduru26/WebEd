using System.Collections.Generic;
using UnityEngine;

// Attach to an empty GameObject. Tracks which phases the user has opened.
// When all 5 have been viewed, fires confetti and shows a win message.
// Extra feature: progress + particle system win-state.
public class ProgressTracker : MonoBehaviour
{
    [SerializeField] private ParticleSystem confetti;   // from Kenney Particle Pack
    [SerializeField] private GameObject winText;        // "You completed the design journey!"
    [SerializeField] private int totalPhases = 5;

    private readonly HashSet<int> viewed = new HashSet<int>();
    private bool completed = false;

    void Start()
    {
        if (winText != null) winText.SetActive(false);
    }

    public void MarkViewed(int phaseOrder)
    {
        viewed.Add(phaseOrder);

        if (!completed && viewed.Count >= totalPhases)
        {
            completed = true;
            if (confetti != null) confetti.Play();
            if (winText != null) winText.SetActive(true);
        }
    }
}
