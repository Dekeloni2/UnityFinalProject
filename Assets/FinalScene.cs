using System.Collections;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    public CutsceneText cutscene;
    private bool played = false;
    
    public void Open()
    {
        if (!played)
        {
            played = true;
            StartCoroutine(OpenSequence());
        }
    }

    private IEnumerator OpenSequence()
    {
        yield return StartCoroutine(cutscene.ShowMultiple(
            "This was quite the challenge... but I managed to get through it",
            "This door probably leads to the next area of this place...",
            "....",
            "I am really terrified, but perhaps this will tell me more about my origin."
        ));
        
        gameObject.SetActive(false);
    }
}