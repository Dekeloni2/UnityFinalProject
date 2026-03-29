using UnityEngine;

public class MagnetSpawnButton : MonoBehaviour
{
    public GameObject magneticItemPrefab;
    public Transform spawnPoint;
    public Animator animator;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            SpawnMagneticItem();
        }
    }

    private void SpawnMagneticItem()
    {
        if (animator != null)
            animator.SetTrigger("Pressed");

        Instantiate(magneticItemPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
}