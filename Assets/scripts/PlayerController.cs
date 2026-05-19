using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour // ✅ NAMA DIKEMBALIKAN KE PlayerController
{
    [Header("️ Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;

    [Header("🔍 Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.25f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isFacingRight = true;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        HandleJump();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    private void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        }

        // ️ Reset tiap frame saat di tanah → anti-stuck jump
        if (isGrounded) jumpCount = 0;
    }

    private void Move()
    {
        float hInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(hInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount++;
            }
        }
    }

    private void HandleFlip()
    {
        float hInput = Input.GetAxis("Horizontal");
        bool wantsToFlip = (hInput > 0 && !isFacingRight) || (hInput < 0 && isFacingRight);

        if (wantsToFlip && spriteRenderer != null)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !isFacingRight;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}