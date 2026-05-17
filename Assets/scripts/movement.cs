using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("⚙️ Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;

    [Header("🔍 Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.25f; // Sedikit lebih besar agar tidak mudah terlepas
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
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        // 🔑 FIX: Reset SETIAP FRAME saat di tanah. 
        // Lebih stabil daripada cek transisi karena mencegah missed frame saat mendarat.
        if (isGrounded)
        {
            jumpCount = 0;
        }
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
                Debug.Log($"🚀 Lompat {jumpCount}/{maxJumps}");
            }
            else
            {
                Debug.Log(" Limit tercapai. Mendarat dulu.");
            }
        }
    }

    private void HandleFlip()
    {
        float hInput = Input.GetAxis("Horizontal");
        if (hInput > 0 && !isFacingRight) Flip();
        else if (hInput < 0 && isFacingRight) Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        if (spriteRenderer != null)
        {
            Vector3 scale = spriteRenderer.transform.localScale;
            scale.x *= -1;
            spriteRenderer.transform.localScale = scale;
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