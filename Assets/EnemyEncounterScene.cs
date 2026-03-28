using UnityEngine;

public class EnemyEncounterScene : MonoBehaviour
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
                "What is this supposed to be?",
                "Seems like a type of enemy. I probably shouldn't get near it",
                "Perhaps if I use my powers I can move it to the pit?."));
        }
    }
}