using UnityEngine;
using System.Collections;

public class ColorSwitchButton : MonoBehaviour
{
    private bool playerInRange = false;        // True when the player is standing inside the button's trigger area
    public MagnetObject targetBox;             // The magnetic box whose color will be changed when the button is pressed

    private Animator animator;                 // Animator used to play the button press animation

    // Cutscene system reference
    public CutsceneText cutscene;

    private void Awake()
    {
        // Log the current state of the cutscene flag for debugging
        Debug.Log("Cutscene flag = " + CutsceneFlags.yellowButtonExplained);

        // Cache the animator component
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // If the player is inside the trigger and presses F, activate the button
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Button pressed!");

            // Play the button press animation
            if (animator != null)
                animator.SetTrigger("Pressed");

            // Toggle the color of the assigned magnetic box
            if (targetBox != null)
                targetBox.ToggleColor();
            else
                Debug.LogWarning("No targetBox assigned!");

            // Trigger the cutscene if it hasn't been shown yet
            ActivateScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect when the player enters the button's trigger area
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detect when the player leaves the trigger area
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left trigger");
        }
    }

    public void ActivateScene()
    {
        Debug.Log("Before: " + CutsceneFlags.yellowButtonExplained);

        // Only play the cutscene the first time the button is used
        if (!CutsceneFlags.yellowButtonExplained)
        {
            CutsceneFlags.yellowButtonExplained = true;
            Debug.Log("Cutscene triggered!");
            StartCoroutine(ShowYellowButtonCutscene());
        }

        Debug.Log("After: " + CutsceneFlags.yellowButtonExplained);
    }

    private IEnumerator ShowYellowButtonCutscene()
    {
        // Display a short explanation about how yellow buttons work
        yield return StartCoroutine(cutscene.ShowMultiple(
            "I see. Yellow buttons change the box colors.",
            "Red is only able to be pushed, blue is only able to be pulled",
            "Let's pull this box to the button"
        ));
    }
}