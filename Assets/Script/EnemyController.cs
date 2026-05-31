using UnityEngine;

/// <summary>
/// 敵の動作を制御するクラス
/// Enemyスプライトにアタッチして使用
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private bool moveRight = true;

    [Header("移動範囲")]
    [SerializeField]
    private bool usePatrol = true;

    [SerializeField]
    private float patrolDistance = 3f;

    [Header("壁検知")]
    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private float wallCheckDistance = 0.5f;

    [SerializeField]
    private LayerMask groundLayer;

    // コンポーネント参照
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // パトロール用
    private Vector3 startPosition;
    private float currentDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 初期位置を記録
        startPosition = transform.position;

        // 初期方向を設定
        currentDirection = moveRight ? 1f : -1f;

        // Rigidbody2Dの設定
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // タグを設定（インスペクターでも設定可能）
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            gameObject.tag = "Enemy";
        }
    }

    void Update()
    {
        // ゲームプレイ中のみ動作
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // パトロール移動
        if (usePatrol)
        {
            Patrol();
        }

        // 壁検知による反転
        CheckWall();

        // 移動適用
        Move();
    }

    /// <summary>
    /// パトロール範囲内で移動する
    /// </summary>
    private void Patrol()
    {
        float distanceFromStart = transform.position.x - startPosition.x;

        // パトロール範囲を超えたら反転
        if (distanceFromStart > patrolDistance)
        {
            currentDirection = -1f;
        }
        else if (distanceFromStart < -patrolDistance)
        {
            currentDirection = 1f;
        }
    }

    /// <summary>
    /// 壁を検知したら反転する
    /// </summary>
    private void CheckWall()
    {
        Vector2 checkPos;
        if (wallCheck != null)
        {
            checkPos = wallCheck.position;
        }
        else
        {
            checkPos = (Vector2)transform.position + Vector2.right * currentDirection * 0.5f;
        }

        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.right * currentDirection, wallCheckDistance, groundLayer);

        if (hit.collider != null)
        {
            // 壁に当たったら反転
            currentDirection *= -1f;
        }
    }

    /// <summary>
    /// 移動を適用する
    /// </summary>
    private void Move()
    {
        rb.linearVelocity = new Vector2(currentDirection * moveSpeed, rb.linearVelocity.y);

        // スプライトの向きを変更
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = currentDirection < 0;
        }
    }
    /// <summary>
    /// プレイヤーに踏まれた時の処理
    /// </summary>
    public void OnStomped()
    {
        // 動きを止める
        rb.linearVelocity = Vector2.zero;

        // コライダーを無効化（もう当たらないように）
        GetComponent<BoxCollider2D>().enabled = false;

        // 潰れるアニメーション（Y方向に縮める）
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y * 0.3f,
            transform.localScale.z
        );

        // 少し待ってから消す
        Destroy(gameObject, 0.3f);
    }
    /// <summary>
    /// デバッグ用のギズモを描画
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // パトロール範囲を表示
        Gizmos.color = Color.yellow;
        Vector3 pos = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawLine(pos + Vector3.left * patrolDistance, pos + Vector3.right * patrolDistance);

        // 壁検知レイを表示
        Gizmos.color = Color.red;
        Vector3 wallCheckPos = wallCheck != null ? wallCheck.position : transform.position;
        float dir = Application.isPlaying ? currentDirection : (moveRight ? 1f : -1f);
        Gizmos.DrawRay(wallCheckPos, Vector3.right * dir * wallCheckDistance);
    }
}