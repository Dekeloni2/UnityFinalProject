using UnityEngine;

public class MagneticItemCutscene : MonoBehaviour
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
                "It seems I need to get this to move so I can jump above this wall",
                "Maybe if I press E or Q I can move it"
            ));
        }
    }
}