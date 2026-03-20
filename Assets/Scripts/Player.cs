using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    public Transform spriteTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
        UpdateAnimations();
    }
    private void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal");

        Vector2 velocity = rb.linearVelocity;
        velocity.x = inputX * moveSpeed;
        rb.linearVelocity = velocity;

        if (inputX > 0)
            spriteTransform.localScale = new Vector3(1, 1, 1);
        else if (inputX < 0)
            spriteTransform.localScale = new Vector3(-1, 1, 1);
    }

    private void Jump()
    {
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    private void UpdateAnimations()
    {
        bool isWalking = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }


    private void CheckGround()
    {
        if (groundCheck == null) return;

        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = hit != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}