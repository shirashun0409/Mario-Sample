using UnityEngine;

/// <summary>
/// カメラをプレイヤーに追従させるクラス
/// Main Cameraにアタッチして使用
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("追従対象")]
    [SerializeField]
    private Transform target;

    [Header("追従設定")]
    [SerializeField]
    private float smoothSpeed = 5f;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 2, -10);

    [Header("カメラ制限")]
    [SerializeField]
    private bool useBounds = true;

    [SerializeField]
    private float minX = 0f;

    [SerializeField]
    private float maxX = 100f;

    [SerializeField]
    private float minY = 0f;

    [SerializeField]
    private float maxY = 10f;

    void Start()
    {
        // ターゲットが設定されていない場合、Playerタグで検索
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 目標位置を計算
        Vector3 desiredPosition = target.position + offset;

        // 制限を適用
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // スムーズに移動
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }

    /// <summary>
    /// カメラ制限範囲を可視化（デバッグ用）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 1);
        Gizmos.DrawWireCube(center, size);
    }
}