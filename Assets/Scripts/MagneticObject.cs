using UnityEngine;

public enum MagnetType
{
    Colored,
    Normal
}
public enum MagnetColor
{
    Blue,
    Red
}

public class MagnetObject : MonoBehaviour
{
    public MagnetType type = MagnetType.Colored;
    public MagnetColor currentColor = MagnetColor.Blue;

    public float magnetForce = 10f;
    public float maxVelocity = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyMagnet(Vector3 sourcePosition, bool attract)
    {
        Vector2 direction;

        if (type == MagnetType.Normal)
        {
            direction = attract
                ? (sourcePosition - transform.position).normalized
                : (transform.position - sourcePosition).normalized;
        }
        else
        {
            if (currentColor == MagnetColor.Blue)
                direction = (sourcePosition - transform.position).normalized;
            else
                direction = (transform.position - sourcePosition).normalized;
        }

        rb.AddForce(direction * magnetForce);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxVelocity);
    }

    public void ToggleColor()
    {
        if (type != MagnetType.Colored)
            return;

        currentColor = currentColor == MagnetColor.Blue ? MagnetColor.Red : MagnetColor.Blue;
        UpdateColorVisual();
    }

    private void UpdateColorVisual()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (type == MagnetType.Normal)
        {
            sr.color = Color.white;
            return;
        }

        sr.color = currentColor == MagnetColor.Blue ? Color.cyan : Color.red;
    }
}