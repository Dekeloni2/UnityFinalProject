using UnityEngine;

public class DarkBoxScript : MonoBehaviour
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
                "Seems like my powers don't work on these 'Dark Boxes'",
                "But maybe they have a gimmick to them, perhaps I should jump on them?"
                ));
        }
    }
}