using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicEnemyAI : MonoBehaviour
{
    [Header("Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRadius = 5f;
    public int damageToPlayer = 20;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Adjust difficulty depending on current level
        if (GameManager.Instance != null)
        {
            float difficultyMultiplier = 1f + (GameManager.Instance.currentLevel * 0.2f);
            chaseSpeed *= difficultyMultiplier;
            damageToPlayer = Mathf.RoundToInt(damageToPlayer * difficultyMultiplier);
        }
    }

    private void Update()
    {
        if (playerTransform == null || (GameManager.Instance != null && GameManager.Instance.isGameOver))
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Patrol;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null || (GameManager.Instance != null && GameManager.Instance.isGameOver)) return;

        if (currentState == State.Chase)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * chaseSpeed * Time.fixedDeltaTime);
        }
        else if (currentState == State.Patrol)
        {
            // IDLE state when not chasing.
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageToPlayer);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
