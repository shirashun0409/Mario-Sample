using UnityEngine;

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
}

/// <summary>  
///プレイヤー