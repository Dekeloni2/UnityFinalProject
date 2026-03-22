using UnityEngine;

public class PlayerMagnetism : MonoBehaviour
{
    public float magnetRange = 5f;
    public KeyCode attractKey = KeyCode.E;
    public KeyCode repelKey = KeyCode.Q;

    private void Update()
    {
        bool attracting = Input.GetKey(attractKey);
        bool repelling = Input.GetKey(repelKey);

        bool magnetActive = attracting || repelling;

        if (magnetActive)
            ApplyMagnet(attracting);
        else
            ResetEnemies();
    }

    private void ApplyMagnet(bool attract)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (var hit in hits)
        {
            // --- מגנט אובייקטים רגילים ---
            MagnetObject magnet = hit.GetComponent<MagnetObject>();
            if (magnet != null)
                magnet.ApplyMagnet(transform.position, attract);

            // --- אויבים ---
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyMagnetPhysics(transform.position, attract);
                enemy.ApplyMagnetForce(); // מודד זמן עד שהוא נהיה כבד
            }
        }
    }

    private void ResetEnemies()
    {
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}