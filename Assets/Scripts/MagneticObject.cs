using UnityEngine;

public class MagnetObject : MonoBehaviour
{
    public float magnetForce = 10f;
    
    public void Attract(Vector2 sourcePosition)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Debug.Log("Applying force to: " + gameObject.name);

        Vector2 direction = (sourcePosition - rb.position).normalized;
        rb.AddForce(direction * magnetForce, ForceMode2D.Force);
    }

    public void Repel(Vector2 sourcePosition)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = (rb.position - sourcePosition).normalized;
        rb.AddForce(direction * magnetForce, ForceMode2D.Force);
        Debug.Log("Repel magnet");
    }
}