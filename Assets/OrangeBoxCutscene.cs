using UnityEngine;

public class OrangeButtonCutscene : MonoBehaviour
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
                "Seems like a new type of box.",
                "I should first push it to the button and then get on top of it"));
        }
    }
}