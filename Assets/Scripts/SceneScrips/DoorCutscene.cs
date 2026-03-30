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
            StartCoroutine(ActivateScene());
        }
    }

    public IEnumerator ActivateScene()
    {
        if (CutsceneFlags.doorCutscene) yield break;
        CutsceneFlags.doorCutscene = true;
        Debug.Log("Cutscene triggered!");
        yield return StartCoroutine(ShowDoorCutscene());
    }

    private IEnumerator ShowDoorCutscene()
    {
        yield return StartCoroutine(cutscene.ShowMultiple(
            "I see, doors can only be unlocked once I move a box on the buttons.",
            "I cannot manually push the button by standing on it, I have to use a box",
            "I will keep that in mind for the future."
        ));

        gameObject.SetActive(false);
    }
}