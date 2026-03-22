using UnityEngine;

public class MagnetSwitch : MonoBehaviour
{
    public enum RequiredColor { Any, Red, Blue }
    public RequiredColor requiredColor = RequiredColor.Any;

    public Door door;
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
        }
    }

    private bool IsCorrectColor(MagnetObject magnet)
    {
        // אם הכפתור מוגדר כ-Any → כל בלוק מפעיל אותו
        if (requiredColor == RequiredColor.Any)
            return true;

        // אם הבלוק הוא Normal → מפעיל כל כפתור
        if (magnet.type == MagnetType.Normal)
            return true;

        // התאמת צבעים
        if (requiredColor == RequiredColor.Red && magnet.currentColor == MagnetColor.Red)
            return true;

        if (requiredColor == RequiredColor.Blue && magnet.currentColor == MagnetColor.Blue)
            return true;

        return false;
    }
}