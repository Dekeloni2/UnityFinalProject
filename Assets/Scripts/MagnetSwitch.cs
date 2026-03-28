using UnityEngine;

public class MagnetSwitch : MonoBehaviour
{
    public enum RequiredColor { Any, Red, Blue }
    public RequiredColor requiredColor = RequiredColor.Any;
    
    public DoorCutscene door;
    public DoorFunction  doorFunction;
    public Animator animator;

    private bool isPressed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPressed)
            return;

        MagnetObject magnet = collision.GetComponent<MagnetObject>();
        if (magnet == null)
            return;

        if (IsCorrectColor(magnet))
        {
            Debug.Log("Switch activated!");

            isPressed = true;
            animator.SetBool("Pressed", true);
            door.Open();
            doorFunction.Open();
        }
    }

    private bool IsCorrectColor(MagnetObject magnet)
    {
        //Will open a door if any color
        if (requiredColor == RequiredColor.Any)
            return true;

        //Will open the door if the magnet type is normal
        if (magnet.type == MagnetType.Normal)
            return true;

        //Color matching for both red and blue
        if (requiredColor == RequiredColor.Red && magnet.currentColor == MagnetColor.Red)
            return true;

        if (requiredColor == RequiredColor.Blue && magnet.currentColor == MagnetColor.Blue)
            return true;

        return false;
    }
}