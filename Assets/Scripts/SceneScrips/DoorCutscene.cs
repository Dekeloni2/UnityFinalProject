using System.Collections;
using UnityEngine;

public class DoorCutscene : MonoBehaviour
{
    public CutsceneText cutscene;      // Reference to the system that displays cutscene dialogue
    private bool played = false;       // Ensures the cutscene only plays once

    public void Open()
    {
        Debug.Log($"[DoorCutscene] Open() called on {gameObject.name}");

        // If this is the first time the door is being opened, trigger the cutscene
        if (!played)
        {
            Debug.Log("[DoorCutscene] First time opening, triggering cutscene");
            played = true;
            StartCoroutine(ActivateScene());
        }
        else
        {
            // If the cutscene already played once, skip it
            Debug.Log("[DoorCutscene] Cutscene already played, skipping");
        }

        // The door always opens, regardless of whether the cutscene plays
        OpenDoor();
    }

    private void OpenDoor()
    {
        // Disables the door object so the player can pass through
        Debug.Log("[DoorCutscene] Door disabled");
        gameObject.SetActive(false);
    }

    public IEnumerator ActivateScene()
    {
        Debug.Log($"[DoorCutscene] ActivateScene() called. Flag = {CutsceneFlags.doorCutscene}");

        // Prevents the cutscene from playing multiple times globally
        if (CutsceneFlags.doorCutscene)
        {
            Debug.Log("[DoorCutscene] Global cutscene flag already true, skipping cutscene");
            yield break;
        }

        // Mark the cutscene as triggered so it won't repeat
        CutsceneFlags.doorCutscene = true;
        Debug.Log("[DoorCutscene] Cutscene triggered!");

        // Play the actual cutscene dialogue
        yield return StartCoroutine(ShowDoorCutscene());
    }

    private IEnumerator ShowDoorCutscene()
    {
        Debug.Log("[DoorCutscene] Starting ShowDoorCutscene()");

        // Display multiple lines of dialogue in sequence
        yield return StartCoroutine(cutscene.ShowMultiple(
            "I see, doors can only be unlocked once I move a box on the buttons.",
            "I cannot manually push the button by standing on it, I have to use a box",
            "I will keep that in mind for the future."
        ));

        Debug.Log("[DoorCutscene] Cutscene finished");
    }
}