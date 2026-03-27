using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
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
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    public float stopChance = 0.2f;
    public float stopDuration = 1f;

    [Header("Sight")]
    public float sightRange = 6f;
    public LayerMask playerLayer;

    [Header("Magnet Interaction")]
    public float magnetStunTime = 2f;
    public float heavyMassDuration = 2f;
    public float heavyMass = 50f;
    public float normalMass = 5f;

    [Header("Fall Death")]
    public float fallDeathTime = 3f;
    public float fallYThreshold = -10f;

    private float magnetTimer = 0f;
    private float fallTimer = 0f;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;  

    private int direction = 1;
    private bool isStopping = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb.mass = normalMass;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("IsHeavy", state == EnemyState.Heavy);

        bool walking =
            state == EnemyState.Patrol ||
            state == EnemyState.Chase;

        animator.SetBool("IsWalking", walking);
    }
    
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right * direction * sightRange, Color.red);
        Debug.Log(state);

        if (state == EnemyState.Dead)
            return;

        CheckFall();

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

    //Checks if a player is in front of it
    private bool PlayerInSight()
    {
        // אם השחקן רחוק מדי — לא רואים אותו
        if (Vector2.Distance(transform.position, player.position) > sightRange)
            return false;

        // אם השחקן לא נמצא בכיוון שהאויב מסתכל — לא רואים אותו
        float dirToPlayer = Mathf.Sign(player.position.x - transform.position.x);
        if (dirToPlayer != direction)
            return false;

        // Raycast קדימה בלבד
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

            //Checks if there is a wall in front of it
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, 0.5f);
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                direction *= -1;
            }

            //Random stop
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

    
    //Chases the player
    private void Chase()
    {
        if (!PlayerInSight())
        {
            state = EnemyState.Patrol;
            return;
        }

        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);
    }

    //What happens when the player uses E or Q keys for Magnetism

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


    //Function for if the player uses Magnetism for too long
    //Enemy increases Mass and doesn't magnetize
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

       //Stops movement during magnetize
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        magnetTimer += Time.deltaTime;

        if (magnetTimer >= magnetStunTime)
            StartCoroutine(BecomeHeavy());
    }

    public void ResetMagnetTimer()
    {
        if (state != EnemyState.Heavy)
            magnetTimer = 0f;
    }

    private IEnumerator MagnetDeny()
    {
        state = EnemyState.Heavy;
        rb.mass = heavyMass;

        yield return new WaitForSeconds(heavyMassDuration);

        rb.mass = normalMass;
        magnetTimer = 0f;

        state = EnemyState.Patrol;
    }

    //Falling functions
    private void CheckFall()
    {
        if (transform.position.y < fallYThreshold && state != EnemyState.Dead)
        {
            state = EnemyState.Falling;
        }
    }

    private void Falling()
    {
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallDeathTime)
        {
            state = EnemyState.Dead;
            Destroy(gameObject);
        }
    }
}