using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

/// <summary>
/// 動く足場を制御するクラス
/// ブロックのPrefabにアタッチして使用
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    // 移動方向の種類
    public enum MoveDirection
    {
        Horizontal,  // 横方向
        Vertical     // 縦方向
    }

    [Header("移動設定")]
    [SerializeField]
    private MoveDirection direction = MoveDirection.Horizontal;

    [SerializeField]
    private float moveDistance = 3f;   // 移動距離

    [SerializeField]
    private float moveSpeed = 2f;     // 移動速度

    // 内部変数
    private Vector3 startPosition;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // ゲームプレイ中のみ動作
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState
                != GameManager.GameState.Playing)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        //Sin関数で往復移動（-1〜1の値 × 移動距離） 
        float offset = Mathf.Sin(elapsedTime * moveSpeed)
                       * moveDistance;

        if (direction == MoveDirection.Horizontal)
        {
            //横方向に移動
            transform.position = new Vector3(
                startPosition.x + offset,
                startPosition.y,
                startPosition.z
            );
        }
        else
        {
            //縦方向に移動
            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + offset,
                startPosition.z
            );
        }
    }

    /// <summary>  
    ///プレイヤーが乗ったら子プロジェクトにする
    /// </suammary>
    private void OnCollisionEnter2D(Collision2D collision)

    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーが上に乗った時だけ親子にする
            float playerBottom = collision.transform.position.y -
                collision.collider.bounds.extents.y;
            float platformTop = transform.position.y +
                GetComponent<Collider2D>().bounds.extents.y;

            if (playerBottom >= platformTop - 0.1f)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    /// <summary>
    /// プレイヤーが降りたら親子関係を解除する
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}




