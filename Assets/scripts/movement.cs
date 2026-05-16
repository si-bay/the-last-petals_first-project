using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float doubleJumpWindow = 0.3f; // Toleransi waktu untuk double jump

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    // State Machine Double Jump
    private float jumpTimer = 0f;
    private bool canDoubleJump = false;

    [SerializeField] private SpriteRenderer playerSprite;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        HandleJumpInput();
        HandleSpriteFlip();
        UpdateTimer();
    }

    private void FixedUpdate()
    {
        CheckGround();
        MoveCharacter();
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 🔒 RESET KUNCI LOMPAT: Hanya terjadi saat transisi Udara -> Tanah
        if (isGrounded && !wasGrounded)
        {
            canDoubleJump = true; // Buka kembali kesempatan lompat
            jumpTimer = 0f;
        }
    }

    private void MoveCharacter()
    {
        float hInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(hInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                // 1️⃣ LOMPAT PERTAMA (dari tanah)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                canDoubleJump = true;
                jumpTimer = doubleJumpWindow;
            }
            else if (canDoubleJump && jumpTimer > 0f)
            {
                // 2️ DOUBLE JUMP (di udara, dalam window 0.3s)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                
                // 🔒 KUNCI LOMPAT: Matikan kesempatan lompat lagi
                canDoubleJump = false; 
                jumpTimer = 0f;
            }
            //  Jika tidak masuk kedua kondisi di atas, input DIABAIKAN.
            // Player TIDAK bisa lompat lagi sampai menyentuh tanah.
        }
    }

    private void UpdateTimer()
    {
        if (jumpTimer > 0f) jumpTimer -= Time.deltaTime;
    }

    private void HandleSpriteFlip()
    {
        float hInput = Input.GetAxis("Horizontal");
        bool wantsToFlip = (hInput > 0 && !isFacingRight) || (hInput < 0 && isFacingRight);
        if (wantsToFlip && playerSprite != null)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = playerSprite.transform.localScale;
            scale.x = isFacingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            playerSprite.transform.localScale = scale;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}