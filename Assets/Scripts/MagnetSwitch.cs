using UnityEngine;

public class MagnetSwitch : MonoBehaviour
{
    public Door door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Switch hit by: " + collision.name);

        if (collision.GetComponent<MagnetObject>())
        {
            Debug.Log("Switch activated!");
            door.Open();
        }
    }
}