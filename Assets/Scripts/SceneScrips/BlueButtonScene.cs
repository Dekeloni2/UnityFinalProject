using UnityEngine;

public class BlueButtonCutscene : MonoBehaviour
{
    public CutsceneText cutscene;
    private bool triggered = false;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger hit: " + other.name);
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected — starting cutscene");
            triggered = true;

            StartCoroutine(cutscene.ShowMultiple(
                "A blue button? Seems like only blue boxes can use it.",
                "Perhaps if I can find a blue box I can continue"));
        }
        else
        {
            Debug.Log("But it's NOT the player");
        }
    }
}