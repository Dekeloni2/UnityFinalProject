using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;                 // Horizontal movement speed
    public float jumpForce = 7f;                 // Vertical force applied when jumping
    public Transform groundCheck;                // Point used to detect if the player is standing on the ground
    public float groundCheckRadius = 0.2f;       // Radius of the ground detection circle
    public LayerMask groundLayer;                // Which layers count as "ground"

    private Rigidbody2D rb;                      // Reference to the player's Rigidbody2D
    private Animator animator;                   // Reference to the player's Animator
    private bool isGrounded;                     // True when the player is touching the ground
    public Transform spriteTransform;            // Used to flip the sprite based on movement direction

    private void Awake()
    {
        // Cache components for performance and cleaner code
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Handle input and animation updates every frame
        Move();
        Jump();
        UpdateAnimations();
    }

    private void Move()
    {
        // Read horizontal input (-1, 0, 1)
        float inputX = Input.GetAxisRaw("Horizontal");

        // Apply horizontal movement while keeping the current vertical velocity
        Vector2 velocity = rb.linearVelocity;
        velocity.x = inputX * moveSpeed;
        rb.linearVelocity = velocity;

        // Flip the sprite depending on movement direction
        if (inputX > 0)
            spriteTransform.localScale = new Vector3(1, 1, 1);
        else if (inputX < 0)
            spriteTransform.localScale = new Vector3(-1, 1, 1);
    }

    private void Jump()
    {
        // Check if the player is standing on the ground
        CheckGround();

        // Only allow jumping when grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Jump!
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void ApplyJumpBoost(float multiplier)
    {
        // External method used by power-ups or triggers to boost jump height
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * multiplier);
    }

    private void UpdateAnimations()
    {
        // Player is considered walking if horizontal velocity is above a small threshold
        bool isWalking = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;

        // Create a small circle under the player to detect ground collisions
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ground check radius in the editor for easier debugging
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}