using UnityEngine;

public class ButtonCutscene : MonoBehaviour
{
    public CutsceneText cutscene;
    private bool triggered = false;
    void Start()
    {
        cutscene.ResetText();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            StartCoroutine(cutscene.ShowMultiple(
                "What is this, a button?",
                "And a huge door?",
                "Maybe if I push the box with my powers the door will open?"
            ));
        }
    }
}