using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("⚙️ Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;

    [Header("🔍 Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.25f;
    public LayerMask groundLayer;

    [Header(" Animation")]
    private Animator anim;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private bool isFacingRight = true;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // Inisialisasi Animator
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        HandleJump();
        HandleFlip();

        // Update parameter animasi setiap frame
        if (anim != null)
        {
            anim.SetBool("isGrounded", isGrounded);
            // Pakai velocity biar tidak "lari di tempat" saat nabrak tembok
            anim.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        }
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