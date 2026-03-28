using System.Collections;
using UnityEngine;

public class DoorCutscene : MonoBehaviour
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
            "It seems that boxes can push buttons and open doors",
            "I will keep this in mind for the future"
        ));
        
        gameObject.SetActive(false);
    }
}