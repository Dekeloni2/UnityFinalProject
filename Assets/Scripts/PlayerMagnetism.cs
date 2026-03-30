using UnityEngine;

public class PlayerMagnetism : MonoBehaviour
{
    public float magnetRange = 5f;          // How far the magnet effect reaches around the player
    public KeyCode attractKey = KeyCode.E;  // Key used to pull objects toward the player
    public KeyCode repelKey = KeyCode.Q;    // Key used to push objects away from the player

    private bool attracting;                // True while the player is holding the attract key
    private bool repelling;                 // True while the player is holding the repel key

    private void Update()
    {
        // Read player input every frame.
        attracting = Input.GetKey(attractKey);
        repelling = Input.GetKey(repelKey);
    }

    // We handle magnet physics inside FixedUpdate so the behavior is consistent
    // and not tied to the player's frame rate. This prevents stronger/weaker magnet
    // effects on different machines.
    private void FixedUpdate()
    {
        bool magnetActive = attracting || repelling;

        if (magnetActive)
            ApplyMagnet(attracting);   // Pass whether we are attracting or repelling
        else
            ResetEnemies();            // Reset enemy magnet timers when magnet is not active
    }

    private void ApplyMagnet(bool attract)
    {
        // Detect all objects within magnetRange around the player
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (var hit in hits)
        {
            // Handle magnet interaction for regular magnetic objects
            MagnetObject magnet = hit.GetComponent<MagnetObject>();
            if (magnet != null)
                magnet.ApplyMagnet(transform.position, attract);

            // Handle magnet interaction for enemies
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Apply physical force toward or away from the player
                enemy.ApplyMagnetPhysics(transform.position, attract);

                // Track how long the magnet is affecting the enemy
                enemy.ApplyMagnetForce();
            }
        }
    }

    private void ResetEnemies()
    {
        // When the magnet is not active, we reset the magnet timer on nearby enemies
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
                enemy.ResetMagnetTimer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow wire circle in the editor to visualize the magnet range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}