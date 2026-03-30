using UnityEngine;

public class MagnetSwitch : MonoBehaviour
{
    // Determines which magnet color is required to activate the switch
    public enum RequiredColor { Any, Red, Blue }
    public RequiredColor requiredColor = RequiredColor.Any;

    public DoorCutscene door;          // door that will open when the switch is activated
    public Animator animator;          // Animator for the button press animation

    private bool isPressed = false;    // Ensures the switch can only be activated once

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent multiple activations
        if (isPressed)
            return;

        // Only magnetic objects can activate the switch
        MagnetObject magnet = collision.GetComponent<MagnetObject>();
        if (magnet == null)
            return;

        // Check if the magnet matches the required color
        if (IsCorrectColor(magnet))
        {

            isPressed = true;

            // Play button press animation
            animator.SetBool("Pressed", true);

            // Trigger the door opening sequence
            door.Open();
        }
    }

    private bool IsCorrectColor(MagnetObject magnet)
    {
        // If any color is allowed, accept all magnets
        if (requiredColor == RequiredColor.Any)
            return true;

        // Normal magnets always count as valid (they have no color restriction)
        if (magnet.type == MagnetType.Normal)
            return true;

        // Color-specific checks
        if (requiredColor == RequiredColor.Red && magnet.currentColor == MagnetColor.Red)
            return true;

        if (requiredColor == RequiredColor.Blue && magnet.currentColor == MagnetColor.Blue)
            return true;

        // If none of the above matched, the magnet is not valid
        return false;
    }
}