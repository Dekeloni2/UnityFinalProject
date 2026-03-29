using UnityEngine;

public class RedBoxCutscene : MonoBehaviour
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
                "Seems like my powers are only able to push the red box",
                "I wonder what will happen if I push this yellow button", 
                "If I press F, I should click it."
            ));
        }
    }
}