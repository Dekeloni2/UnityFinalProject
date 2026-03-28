using UnityEngine;
using System.Collections;

public class ColorSwitchButton : MonoBehaviour
{
    private bool playerInRange = false;
    public MagnetObject targetBox;

    private Animator animator;

    // Cutscene
    public CutsceneText cutscene;
    private bool played = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Button pressed!");

            if (animator != null)
                animator.SetTrigger("Pressed");

            if (targetBox != null)
                targetBox.ToggleColor();
            else
                Debug.LogWarning("No targetBox assigned!");

            //Will activate the cutscene
            ActivateScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left trigger");
        }
    }

    public void ActivateScene()
    {
        if (!played)
        {
            played = true;
            StartCoroutine(ShowYellowButtonCutscene());
        }
    }

    private IEnumerator ShowYellowButtonCutscene()
    {
        yield return StartCoroutine(cutscene.ShowMultiple(
            "I see. Yellow buttons change the box colors.",
            "Red is only able to be pushed, blue is only able to be pulled",
            "Let's pull this box to the button"
        ));
    }
}