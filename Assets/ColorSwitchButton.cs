using UnityEngine;

public class ColorSwitchButton : MonoBehaviour
{
    private bool playerInRange = false;
    public MagnetObject targetBox;

    private Animator animator;

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
}