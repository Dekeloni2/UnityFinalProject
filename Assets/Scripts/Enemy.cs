using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Different states the enemy can be in
    public enum EnemyState
    {
        Patrol,
        Stop,
        Chase,
        Heavy,
        Falling,
        Dead
    }

    public EnemyState state = EnemyState.Patrol;

    [Header("Movement")]
    public float patrolSpeed = 2f;         // Speed while patrolling
    public float chaseSpeed = 3f;          // Speed while chasing the player
    public float stopChance = 0.2f;        // Random chance to stop during patrol
    public float stopDuration = 1f;        // How long the enemy stays stopped

    [Header("Sight")]
    public float sightRange = 6f;          // How far the enemy can detect the player
    public LayerMask playerLayer;          // Layer used to detect the player

    [Header("Magnet Interaction")]
    public float magnetStunTime = 2f;      // Time before the enemy becomes "Heavy"
    public float heavyMassDuration = 2f;   // How long the enemy stays heavy
    public float heavyMass = 50f;          // Mass while heavy
    public float normalMass = 5f;          // Normal mass

    [Header("Fall Death")]
    public float fallDeathTime = 3f;       // Time before dying after falling
    public float fallYThreshold = -10f;    // Y position considered a fatal fall

    public CutsceneText cutscene;          // Cutscene system reference
    private bool fallCutscenePlayed = false;
    private bool heavyCutscenePlayed = false;
    private float magnetTimer = 0f;
    private float fallTimer = 0f;

    private Rigidbody2D rb;                // Enemy physics body
    private Transform player;              // Reference to the player
    private Animator animator;             // Controls enemy animations

    private Vector3 originalScale;
    private int direction = 1;             // Current facing direction (1 = right, -1 = left)
    private bool isStopping = false;       // Whether the enemy is currently paused

    private void Awake()
    {
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb.mass = normalMass;
    }

    private void UpdateAnimations()
    {
        // Heavy state animation
        animator.SetBool("IsHeavy", state == EnemyState.Heavy);

        // Walking animation only during patrol or chase
        bool walking =
            state == EnemyState.Patrol ||
            state == EnemyState.Chase;

        animator.SetBool("IsWalking", walking);
    }

    private void Update()
    {
        // Dead enemies do nothing
        if (state == EnemyState.Dead)
            return;

        CheckFall();

        // Handle behavior based on current state
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Stop:
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Heavy:
                break;

            case EnemyState.Falling:
                Falling();
                break;
        }

        UpdateAnimations();
    }

    // Checks if the player is in front of the enemy and within sight range
    private bool PlayerInSight()
    {
        if (Vector2.Distance(transform.position, player.position) > sightRange)
            return false;

        // Player must be in the direction the enemy is facing
        float dirToPlayer = Mathf.Sign(player.position.x - transform.position.x);
        if (dirToPlayer != direction)
            return false;

        // Raycast forward to detect the player
        Vector2 origin = transform.position;
        Vector2 dir = Vector2.right * direction;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir,
            sightRange,
            playerLayer
        );

        return hit.collider != null;
    }

    private void Patrol()
    {
        if (PlayerInSight())
        {
            state = EnemyState.Chase;
            return;
        }

        if (!isStopping)
        {
            rb.linearVelocity = new Vector2(direction * patrolSpeed, rb.linearVelocity.y);

            // Detect walls and turn around
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, 0.5f);
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                direction *= -1;
                transform.localScale = new Vector3(originalScale.x * direction, originalScale.y, originalScale.z);
            }

            // Random stop
            if (Random.value < stopChance * Time.deltaTime)
                StartCoroutine(StopRoutine());
        }
    }

    private IEnumerator StopRoutine()
    {
        isStopping = true;
        state = EnemyState.Stop;

        yield return new WaitForSeconds(stopDuration);

        isStopping = false;
        state = EnemyState.Patrol;
    }

    // Chase behavior
    private void Chase()
    {
        if (!PlayerInSight())
        {
            state = EnemyState.Patrol;
            return;
        }

        float dir = Mathf.Sign(player.position.x - transform.position.x);

        direction = (int)dir;

        // Flip using original scale
        transform.localScale = new Vector3(originalScale.x * direction, originalScale.y, originalScale.z);

        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
    }

    // Applies magnetic force when the player uses magnetism
    public void ApplyMagnetPhysics(Vector3 sourcePosition, bool attract)
    {
        if (state == EnemyState.Heavy || state == EnemyState.Dead)
            return;

        Vector2 direction = attract
            ? (sourcePosition - transform.position)
            : (transform.position - sourcePosition);

        direction.y = 0;
        direction = direction.normalized;

        rb.AddForce(direction * 15f, ForceMode2D.Force);
    }

    // If magnetized for too long, enemy becomes heavy and immune to it
    private IEnumerator BecomeHeavy()
    {
        state = EnemyState.Heavy;
        rb.mass = heavyMass;

        yield return new WaitForSeconds(heavyMassDuration);

        rb.mass = normalMass;
        magnetTimer = 0f;

        state = EnemyState.Patrol;
    }

    public void ApplyMagnetForce()
    {
        if (state == EnemyState.Heavy || state == EnemyState.Dead)
            return;

        // Freeze horizontal movement while magnetized
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        magnetTimer += Time.deltaTime;

        // Become heavy after too much magnet force
        if (magnetTimer >= magnetStunTime)
            StartCoroutine(BecomeHeavy());

        // Play cutscene only once
        if (!heavyCutscenePlayed && cutscene != null)
        {
            heavyCutscenePlayed = true;
            StartCoroutine(cutscene.ShowMultiple(
                "Damn, when he enters this state, my powers don't work on him.",
                "Maybe if I wait he will resume back to normal."
            ));
        }
    }

    public void ResetMagnetTimer()
    {
        if (state != EnemyState.Heavy)
            magnetTimer = 0f;
    }

    // Detects if the enemy has fallen off the map and plays a scene
    private void CheckFall()
    {
        if (transform.position.y < fallYThreshold && state != EnemyState.Dead)
        {
            if (state != EnemyState.Falling)
            {
                state = EnemyState.Falling;

                if (!fallCutscenePlayed && cutscene != null)
                {
                    fallCutscenePlayed = true;
                    StartCoroutine(cutscene.ShowMultiple(
                        "He isn't going to bother me anymore."
                    ));
                }
            }
        }
    }

    private void Falling()
    {
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallDeathTime)
        {
            state = EnemyState.Dead;

            if (cutscene != null)
                cutscene.ResetText();

            Destroy(gameObject);
        }
    }
}