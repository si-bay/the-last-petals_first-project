using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 8f;
    [Tooltip("Jendela waktu maksimal setelah lompatan 1 untuk melakukan double jump.")]
    public float doubleJumpWindow = 0.03f; 

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true;

    // State Double Jump
    private int jumpCount = 0;          // 0=Di tanah, 1=Lompat 1, 2=Lompat 2 (MAX)
    private float firstJumpTime = 0f;   // Mencatat waktu saat lompatan pertama terjadi

    [SerializeField] private SpriteRenderer playerSprite;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        HandleJumpInput();
        HandleSpriteFlip();
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

        // 🔁 RESET: Hanya terjadi saat transisi Udara → Tanah
        if (isGrounded && !wasGrounded)
        {
            jumpCount = 0;
            firstJumpTime = 0f;
            Debug.Log(" Menyentuh tanah -> JumpCount di-reset ke 0");
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
            // 1️⃣ LOMPAT PERTAMA (Saat di tanah)
            if (isGrounded)
            {
                PerformJump();
                jumpCount = 1;
                firstJumpTime = Time.time; // Mulai hitung waktu 0.03s
                Debug.Log("🚀 Lompat 1! Timer 0.03s dimulai.");
            }
            // 2️ DOUBLE JUMP (Saat di udara, jumpCount==1, DAN masih dalam 0.03s)
            else if (jumpCount == 1 && (Time.time - firstJumpTime <= doubleJumpWindow))
            {
                PerformJump();
                jumpCount = 2; // 🔒 KUNCI: Capai limit 2x
                Debug.Log("🚀 Double Jump! Limit 2x tercapai. Lompatan ke-3 akan gagal.");
            }
            // 3️⃣ GAGAL (Jika jumpCount==2 ATAU sudah lewat 0.03s)
            else
            {
                Debug.Log($"❌ Lompatan ke-{jumpCount + 1} GAGAL. (Limit 2x sudah terpenuhi / Waktu 0.03s habis)");
            }
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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