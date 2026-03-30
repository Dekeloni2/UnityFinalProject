using System.Collections;
using UnityEngine;

public class FinalScene : MonoBehaviour
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
            "This was quite the challenge... but I managed to get through it",
            "This door probably leads to the next area of this place...",
            "....",
            "I am really terrified, but perhaps this will tell me more about my origin."
        ));
        }
    }
}