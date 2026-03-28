using UnityEngine;

public class LaserScene : MonoBehaviour
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
                "This laser is extremely dangerous",
                "Looks like I cannot bring any boxes with me if this laser is here",
                "I should be careful with it."
            ));
        }
    }
}