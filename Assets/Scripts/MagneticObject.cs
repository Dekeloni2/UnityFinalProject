using UnityEngine;

public enum MagnetType
{
    Colored,
    Normal
}
public enum MagnetColor
{
    Blue,
    Red,
    Orange
}

public class MagnetObject : MonoBehaviour
{
    public MagnetType type = MagnetType.Colored;
    public MagnetColor currentColor = MagnetColor.Blue;

    public float jumpBoostMultiplier = 2f;
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
            switch (currentColor)
            {
                case MagnetColor.Blue:
                    direction = (sourcePosition - transform.position).normalized;
                    break;

                case MagnetColor.Red:
                    direction = (transform.position - sourcePosition).normalized;
                    break;

                case MagnetColor.Orange:
                    direction = attract
                        ? (sourcePosition - transform.position).normalized
                        : (transform.position - sourcePosition).normalized;
                    break;

                default:
                    direction = Vector2.zero;
                    break;
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentColor == MagnetColor.Orange && collision.collider.CompareTag("Player"))
        {
            // בדיקה שהשחקן הגיע מלמעלה
            if (collision.contacts[0].normal.y < -0.5f)
            {
                PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.ApplyJumpBoost(jumpBoostMultiplier);
                }
            }
        }
    }
    
    private void UpdateColorVisual()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (type == MagnetType.Normal)
        {
            sr.color = Color.white;
            return;
        }

        if (currentColor == MagnetColor.Blue)
            sr.color = Color.blue;
        else if (currentColor == MagnetColor.Red)
            sr.color = Color.red;
        else if (currentColor == MagnetColor.Orange)
            sr.color = new Color(1f, 0.5f, 0f);
    }
}