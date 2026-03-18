using UnityEngine;

public class PlayerMagnetism : MonoBehaviour
{
    public float magnetRange = 5f;
    public KeyCode attractKey = KeyCode.E;
    public KeyCode repelKey = KeyCode.Q;

    private void Update()
    {
        if (Input.GetKey(attractKey))
        {
            ApplyMagnet(true);
        }
        else if (Input.GetKey(repelKey))
        {
            ApplyMagnet(false);
        }
    }

    private void ApplyMagnet(bool attract)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRange);

        foreach (var hit in hits)
        {
            MagnetObject magnet = hit.GetComponent<MagnetObject>();
            if (magnet == null)
                continue;

            if (attract)
                magnet.Attract(transform.position);
            else
                magnet.Repel(transform.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}