using UnityEngine;

public class FinalPuzzleScene : MonoBehaviour
{
    public CutsceneText cutscene;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            StartCoroutine(cutscene.ShowMultiple(
                "This area... it seems like it takes all of my knowledge from this place to test me",
                "Each floor has a different challenge I must face... I can do this.",
                "However it does seem like there are now doors with multiple buttons that need to be pressed",
                "I can do this... maybe solving this puzzle will lead to a way out of here"
            ));
        }
    }
}