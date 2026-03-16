using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの操作を制御するクラス
/// Playerスプライトにアタッチして使用
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("接地判定")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("落下判定")]
    [SerializeField] private float fallThreshold = -10f;

    // コンポーネント
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // 状態
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // ゲームプレイ中のみ操作可能
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        CheckGround();
        HandleMovement();
        HandleJump();
        CheckFall();
    }

    /// 接地判定
    private void CheckGround()
    {
        Vector3 checkPos =
            groundCheck != null ?
            groundCheck.position :
            transform.position + Vector3.down * 0.5f;

        isGrounded = Physics2D.OverlapCircle(
            checkPos,
            groundCheckRadius,
            groundLayer
        );
    }

    /// 移動
    private void HandleMovement()
    {
        float horizontal = 0f;

        var keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed)
                horizontal = -1f;

            if (keyboard.rightArrowKey.isPressed)
                horizontal = 1f;
        }

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        if (horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0;
        }
    }

    /// ジャンプ
    private void HandleJump()
    {
        var keyboard = Keyboard.current;

        if (keyboard != null &&
            keyboard.upArrowKey.wasPressedThisFrame &&
            isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    /// 落下判定
    private void CheckFall()
    {
        if (transform.position.y < fallThreshold)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    /// 敵衝突
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    /// アイテム取得
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectItem();
            }

            Destroy(other.gameObject);
        }
    }

    /// 接地判定の表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 checkPos =
            groundCheck != null ?
            groundCheck.position :
            transform.position + Vector3.down * 0.5f;

        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }
}