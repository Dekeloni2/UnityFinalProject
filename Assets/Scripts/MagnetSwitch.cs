using UnityEngine;

public class MagnetSwitch : MonoBehaviour
{
    public Door door;
    public Animator animator;

    private bool isPressed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPressed)
            return;

        if (collision.GetComponent<MagnetObject>())
        {
            Debug.Log("Switch activated!");

            isPressed = true;

            animator.SetBool("Pressed", true);

            door.Open();
        }
    }
}