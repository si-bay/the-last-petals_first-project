using UnityEngine;

public class BotController : MonoBehaviour
{
    [Header("⚙️ Movement")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float patrolRange = 4f;

    [Header("👀 Detection")]
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    [Header("⚔️ Attack")]
    public int damage = 1;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPos;
    private bool facingRight = false;
    private bool isChasing = false;

    private Collider2D detectedPlayer; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position;
    }

    void Update()
    {
        // Simpan hasil deteksi ke field kelas
        detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        isChasing = detectedPlayer != null;

        anim.SetBool("isChasing", isChasing);
        anim.SetBool("isMoving", rb.linearVelocity.x != 0);

        HandleMovement();
        HandleFlip();
    }

    void HandleMovement()
    {
        float speed = isChasing ? chaseSpeed : moveSpeed;
        float direction;

        if (isChasing)
        {
            // Arahkan ke player
            direction = Mathf.Sign(detectedPlayer.transform.position.x - transform.position.x);
        }
        else
        {
            float dist = transform.position.x - startPos.x;
            direction = facingRight ? 1 : -1;
            if (Mathf.Abs(dist) > patrolRange) direction *= -1;
        }

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        facingRight = direction > 0;
    }


    void HandleFlip()
    {
        if (facingRight) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                lastAttackTime = Time.time;
                anim.SetTrigger("onAttack");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}