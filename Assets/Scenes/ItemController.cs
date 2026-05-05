using UnityEngine;

/// <summary>
/// アイテムの動作を制御するクラス
/// Itemスプライトにアタッチして使用
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ItemController : MonoBehaviour
{
    [Header("アニメーション設定")]
    [SerializeField]
    private bool useFloatAnimation = true;

    [SerializeField]
    private float floatSpeed = 2f;

    [SerializeField]
    private float floatAmount = 0.3f;

    [Header("回転設定")]
    [SerializeField]
    private bool useRotation = false;

    [SerializeField]
    private float rotationSpeed = 90f;

    // 初期位置
    private Vector3 startPosition;

    // 経過時間
    private float elapsedTime = 0f;

    void Start()
    {
        // 初期位置を記録
        startPosition = transform.position;

        // Colliderをトリガーに設定
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }

        // タグを設定（インスペクターでも設定可能）
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
        {
            gameObject.tag = "Item";
        }
    }

    void Update()
    {
        // ゲームプレイ中のみアニメーション
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameManager.GameState.Playing)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        // 浮遊アニメーション
        if (useFloatAnimation)
        {
            FloatAnimation();
        }

        // 回転アニメーション
        if (useRotation)
        {
            RotateAnimation();
        }
    }

    /// <summary>
    /// 上下に浮遊するアニメーション
    /// </summary>
    private void FloatAnimation()
    {
        float newY = startPosition.y + Mathf.Sin(elapsedTime * floatSpeed) * floatAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    /// <summary>
    /// 回転アニメーション
    /// </summary>
    private void RotateAnimation()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤーとの衝突時の処理
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // アイテムを取得
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectItem();
            }
            Destroy(gameObject);
        }
    }
}