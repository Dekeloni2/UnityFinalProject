using System.Collections;
using UnityEngine;

public class DoorCutscene : MonoBehaviour
{
    public CutsceneText cutscene;
    private bool played = false;

    public void Open()
    {
        Debug.Log($"[DoorCutscene] Open() called on {gameObject.name}");

        // הדלת תמיד תיפתח
        OpenDoor();

        // ה-Cutscene רק פעם אחת
        if (!played)
        {
            Debug.Log("[DoorCutscene] First time opening, triggering cutscene");
            played = true;
            StartCoroutine(ActivateScene());
        }
        else
        {
            Debug.Log("[DoorCutscene] Cutscene already played, skipping");
        }
    }

    private void OpenDoor()
    {
        Debug.Log("[DoorCutscene] Door disabled");
        gameObject.SetActive(false);
    }

    public IEnumerator ActivateScene()
    {
        Debug.Log($"[DoorCutscene] ActivateScene() called. Flag = {CutsceneFlags.doorCutscene}");

        if (CutsceneFlags.doorCutscene)
        {
            Debug.Log("[DoorCutscene] Global cutscene flag already true, skipping cutscene");
            yield break;
        }

        CutsceneFlags.doorCutscene = true;
        Debug.Log("[DoorCutscene] Cutscene triggered!");

        yield return StartCoroutine(ShowDoorCutscene());
    }

    private IEnumerator ShowDoorCutscene()
    {
        Debug.Log("[DoorCutscene] Starting ShowDoorCutscene()");

        yield return StartCoroutine(cutscene.ShowMultiple(
            "I see, doors can only be unlocked once I move a box on the buttons.",
            "I cannot manually push the button by standing on it, I have to use a box",
            "I will keep that in mind for the future."
        ));

        Debug.Log("[DoorCutscene] Cutscene finished");
    }
}