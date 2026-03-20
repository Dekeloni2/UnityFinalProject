using UnityEngine;

public class MagnetObject : MonoBehaviour
{
    public float magnetForce = 10f;

    private Rigidbody2D rb;
    private bool isBeingMagnetized = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Attract(Vector2 sourcePosition)
    {
        isBeingMagnetized = true;
        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 direction = (sourcePosition - rb.position).normalized;
        rb.AddForce(direction * magnetForce, ForceMode2D.Force);
    }

    public void Repel(Vector2 sourcePosition)
    {
        isBeingMagnetized = true;

        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 direction = (rb.position - sourcePosition).normalized;
        rb.AddForce(direction * magnetForce, ForceMode2D.Force);
    }

    private void LateUpdate()
    {
        if (!isBeingMagnetized)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }

        isBeingMagnetized = false;
    }
}