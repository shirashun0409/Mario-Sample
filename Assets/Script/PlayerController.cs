using NUnit.Framework;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの操作を制御するクラス
/// Playerスプライトにアタッチして使用
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float jumpForce = 10f;

    [Header("接地判定")]
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundCheckRadius = 0.2f;

    [SerializeField]
    private LayerMask groundLayer;

    [Header("落下判定")]
    [SerializeField]
    private float fallThreshold = -10f;

    [Header("踏みつけ設定")]
    [SerializeField]
    private float stompBounceForce = 8f;  // 踏んだ後の跳ね返り力

    [Header("ダブルジャンプ設定")]
    [SerializeField]
    private int maxJumpCount = 2;  // 最大ジャンプ回数（2=ダブルジャンプ）

    // ジャンプ回数のカウント
    private int currentJumpCount = 0;

    // コンポーネント参照
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // 状態
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Rigidbody2Dの設定
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // ゲームプレイ中のみ操作可能
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        // 接地判定
        CheckGround();

        // 移動処理
        HandleMovement();

        // ジャンプ処理
        HandleJump();

        // 落下判定
        CheckFall();
    }

    /// <summary>
    /// 接地判定を行う
    /// </summary>
    private void CheckGround()
    {
        //前のフレームの設置状態を保存
        bool wasGrounded = isGrounded;

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = Physics2D.OverlapCircle(
                transform.position + Vector3.down * 0.5f,
                groundCheckRadius,
                groundLayer
            );
        }
        //着地した瞬間ジャンプ回数をリセット
        if (!wasGrounded && isGrounded)
        {
            currentJumpCount = 0;
        }
    }

    /// <summary>
    /// 左右移動を処理する
    /// </summary>
    private void HandleMovement()
    {
        float horizontal = 0f;

        // キーボードがあるかチェック
        if (Keyboard.current != null)
        {
            // 左右キーで移動
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal = -1f;
            }
            else if (Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal = 1f;
            }
        }

        // 移動適用
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        // スプライトの向きを変更
        if (horizontal != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = horizontal < 0;
        }
    }

    /// <summary>
    /// ジャンプを処理する
    /// </summary>
    private void HandleJump()
    {
        if (Keyboard.current != null &&
            Keyboard.current.upArrowKey.wasPressedThisFrame &&
            currentJumpCount < maxJumpCount)
        {
            // ジャンプ実行
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x, jumpForce);
            currentJumpCount++;
        }

    }

    /// <summary>
    /// 落下判定を行う
    /// </summary>
    private void CheckFall()
    {
        // 一定以下に落下したらゲームオーバー
        if (transform.position.y < fallThreshold)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    /// <summary>
    /// 敵との衝突時の処理
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //衝突点を取得して、上から踏んだか判定
            //プレイヤーの下はしが敵の中心より上にあれば、踏んだ
            float playerBottom = transform.position.y -
            GetComponent<BoxCollider2D>().bounds.extents.y;
            float enemyCenter = collision.transform.position.y;

            if (playerBottom > enemyCenter)
            {

                //踏みつき成功
                //敵を倒す
                EnemyController enemy =
            collision.gameObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.OnStomped();
                }
                //プレイヤーが少し跳ねる
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x, stompBounceForce);

            }
            else
            {
                // 横からあたった   (ゲームオーバー)    
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }
        }
    }


    /// <summary>
    /// 接地判定用のギズモを描画（デバッグ用）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 checkPos = groundCheck != null ? groundCheck.position : transform.position + Vector3.down * 0.5f;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }
}