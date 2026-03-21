using UnityEngine;

public class PlayerMagnetism : MonoBehaviour
{
    public float magnetRange = 5f;
    public KeyCode magnetKey = KeyCode.E;
    public KeyCode repelKey = KeyCode.Q;

    private void Update()
    {
        bool attracting = Input.GetKey(magnetKey);
        bool repelling = Input.GetKey(repelKey);

        if (attracting || repelling)
            ApplyMagnet(attracting);
        else
            ResetMagnetObjects();
    }
    
    private void ApplyMagnet(bool attract)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (var hit in hits)
        {
            MagnetObject magnet = hit.GetComponent<MagnetObject>();
            if (magnet == null)
                continue;

            magnet.SetDynamic(true);
            magnet.ApplyMagnet(transform.position, attract);
        }
    }
    
    private void ResetMagnetObjects()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (var hit in hits)
        {
            MagnetObject magnet = hit.GetComponent<MagnetObject>();
            if (magnet == null)
                continue;

            magnet.SetDynamic(false); // זה גם מאפס מהירות
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}