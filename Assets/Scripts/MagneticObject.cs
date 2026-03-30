using UnityEngine;

public enum MagnetType
{
    Colored,    // Magnet reacts based on its color (Blue/Red/Orange)
    Normal      // Magnet reacts only based on player input (attract/repel)
}

public enum MagnetColor
{
    Blue,       // Always attracts toward the player
    Red,        // Always repels away from the player
    Orange      // Special: behaves like Normal (attract/repel based on input)
}

public class MagnetObject : MonoBehaviour
{
    public MagnetType type = MagnetType.Colored;     // Determines how this magnet behaves
    public MagnetColor currentColor = MagnetColor.Blue;

    public float jumpBoostMultiplier = 2f;           // Boost applied to the player when bouncing on Orange magnets
    public float magnetForce = 10f;                  // Strength of the magnetic pull/push
    public float maxVelocity = 5f;                   // Maximum allowed speed for stability

    private Rigidbody2D rb;                          // Cached reference to the Rigidbody2D

    private void Awake()
    {
        // Cache the Rigidbody2D for performance
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyMagnet(Vector3 sourcePosition, bool attract)
    {
        Vector2 direction;

        // Normal magnets only depend on the player's input (attract/repel)
        if (type == MagnetType.Normal)
        {
            direction = attract
                ? (sourcePosition - transform.position).normalized     // Pull toward the player
                : (transform.position - sourcePosition).normalized;    // Push away from the player
        }
        else
        {
            // Colored magnets have fixed behavior depending on their color
            switch (currentColor)
            {
                case MagnetColor.Blue:
                    // Blue always attracts toward the player
                    direction = (sourcePosition - transform.position).normalized;
                    break;

                case MagnetColor.Red:
                    // Red always repels away from the player
                    direction = (transform.position - sourcePosition).normalized;
                    break;

                case MagnetColor.Orange:
                    // Orange behaves like a Normal magnet (depends on player input)
                    direction = attract
                        ? (sourcePosition - transform.position).normalized
                        : (transform.position - sourcePosition).normalized;
                    break;

                default:
                    direction = Vector2.zero;
                    break;
            }
        }

        // Apply magnetic force
        rb.AddForce(direction * magnetForce);

        // Clamp velocity to avoid unstable physics or extreme speeds
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxVelocity);
    }

    public void ToggleColor()
    {
        // Only colored magnets can switch between Blue and Red
        if (type != MagnetType.Colored)
            return;

        // Swap between Blue and Red
        currentColor = currentColor == MagnetColor.Blue ? MagnetColor.Red : MagnetColor.Blue;

        // Update the sprite color to reflect the new state
        UpdateColorVisual();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Orange magnets give the player a jump boost when landed on from above
        if (currentColor == MagnetColor.Orange && collision.collider.CompareTag("Player"))
        {
            // Check if the player hit the magnet from above (normal pointing downward)
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
        // Update the sprite color to match the magnet's current state
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (type == MagnetType.Normal)
        {
            // Normal magnets always appear white
            sr.color = Color.white;
            return;
        }

        // Apply color based on magnet type
        if (currentColor == MagnetColor.Blue)
            sr.color = Color.blue;
        else if (currentColor == MagnetColor.Red)
            sr.color = Color.red;
        else if (currentColor == MagnetColor.Orange)
            sr.color = new Color(1f, 0.5f, 0f); // Custom orange color
    }
}