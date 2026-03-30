using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object entering the laser is a magnetic object
        MagnetObject magnet = collision.GetComponent<MagnetObject>();

        // If it is, destroy it immediately
        if (magnet != null)
        {
            Destroy(magnet.gameObject);
        }
    }
}