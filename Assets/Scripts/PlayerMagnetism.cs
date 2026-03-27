using UnityEngine;

public class PlayerMagnetism : MonoBehaviour
{
    public float magnetRange = 5f;
    public KeyCode attractKey = KeyCode.E;
    public KeyCode repelKey = KeyCode.Q;

    private bool attracting;
    private bool repelling;

    private void Update()
    {
        attracting = Input.GetKey(attractKey);
        repelling = Input.GetKey(repelKey);
    }

    //This takes care of the Magnetism not being consitent.
    //It was mostly depending on the player's FPS.
    private void FixedUpdate()
    {
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
            //Magnetic Objects magnetism
            MagnetObject magnet = hit.GetComponent<MagnetObject>();
            if (magnet != null)
                magnet.ApplyMagnet(transform.position, attract);

            // Enemies magnetism
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyMagnetPhysics(transform.position, attract);
                enemy.ApplyMagnetForce();
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