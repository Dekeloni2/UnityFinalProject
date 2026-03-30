using UnityEngine;

public class BlueButtonCutscene : MonoBehaviour
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
                "A blue button? Seems like only blue boxes can use it.",
                "Perhaps if I can find a blue box I can continue"));
        }
        else
        {
        }
    }
}