using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MagnetObject magnet = collision.GetComponent<MagnetObject>();
        if (magnet != null)
        {
            Destroy(magnet.gameObject);
        }
    }
}