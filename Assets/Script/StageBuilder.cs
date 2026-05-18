using UnityEngine;

/// <summary>
/// ステージを構築するクラス
/// Prefabを使って、配列で指定した位置にブロックを配置する
/// </summary>
public class StageBuilder : MonoBehaviour
{
    [Header("Prefab設定")]
    [SerializeField]
    private GameObject blockPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject itemPrefab;

    [Header("ステージ設定")]
    [SerializeField]
    private float blockSize = 1.0f;

    [SerializeField]
    private Vector2 stageOffset = new Vector2(-3f, -3f);  // ステージ開始位置のオフセット

    // ステージデータ（0=空、1=ブロック、2=敵、3=アイテム）
    // 配列の下の行が下のY座標、左の列が左のX座標
    private int[,] stageData = new int[,]
    {
        // 横方向 →  (X座標: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24)

        // Y=0（一番下の地面）
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        // Y=1
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        // Y=2
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        // Y=3
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        // Y=4
        { 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0 },
        // Y=5
        { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
        // Y=6
        { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0 },
        // Y=7
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
    };

    void Start()
    {
        BuildStage();
    }

    /// <summary>
    /// ステージを構築する
    /// </summary>
    private void BuildStage()
    {
        int height = stageData.GetLength(0);
        int width = stageData.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cellType = stageData[y, x];

                // オフセットを加えた位置を計算
                Vector3 position = new Vector3(
                    x * blockSize + stageOffset.x,
                    y * blockSize + stageOffset.y,
                    0
                );

                switch (cellType)
                {
                    case 1:
                        SpawnObject(blockPrefab, position, "Blocks");
                        break;
                    case 2:
                        SpawnObject(enemyPrefab, position, "Enemies");
                        break;
                    case 3:
                        SpawnObject(itemPrefab, position, "Items");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// オブジェクトを生成する
    /// </summary>
    private void SpawnObject(GameObject prefab, Vector3 position, string parentName)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"Prefabが設定されていません: {parentName}");
            return;
        }

        Transform parent = GetOrCreateParent(parentName);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity, parent);
        obj.name = $"{prefab.name}_{position.x}_{position.y}";
    }

    /// <summary>
    /// 親オブジェクトを取得または作成する
    /// </summary>
    private Transform GetOrCreateParent(string parentName)
    {
        GameObject parent = GameObject.Find(parentName);
        if (parent == null)
        {
            parent = new GameObject(parentName);
        }
        return parent.transform;
    }

    /// <summary>
    /// エディタでステージを可視化（Gizmo）
    /// </summary>
    private void OnDrawGizmos()
    {
        if (stageData == null) return;

        int height = stageData.GetLength(0);
        int width = stageData.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cellType = stageData[y, x];
                Vector3 position = new Vector3(
                    x * blockSize + stageOffset.x,
                    y * blockSize + stageOffset.y,
                    0
                );

                switch (cellType)
                {
                    case 1:
                        Gizmos.color = Color.gray;
                        Gizmos.DrawWireCube(position, Vector3.one * blockSize * 0.9f);
                        break;
                    case 2:
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(position, blockSize * 0.4f);
                        break;
                    case 3:
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(position, blockSize * 0.3f);
                        break;
                }
            }
        }
    }
}