using UnityEngine;

public class LaserDestroyScene : MonoBehaviour
{
    public CutsceneText cutscene;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[LaserDestroyScene] Trigger entered by: " + other.name);
        if (triggered)
            return;

        if (other.CompareTag("Laser"))
            return;

        if (other.CompareTag("MagneticObject"))
        {
            triggered = true;

            StartCoroutine(cutscene.ShowMultiple(
                "Damn, I probably needed that box",
                "What is this? Another button?",
                "Lets press it and see what happens"));
        }
    }
}
