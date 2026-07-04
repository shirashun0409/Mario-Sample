using UnityEngine;
using TMPro;

/// <summary>
/// ゲーム画面のUI制御クラス
/// アイテム取得数などを表示
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI itemCountText;

    [SerializeField]
    private TextMeshProUGUI scoreText;    // スコア表示用

    [SerializeField]
    private TextMeshProUGUI timerText;    // タイマー表示用

    void Start()
    {
        // テキストが設定されていない場合は自動で検索
        if (itemCountText == null)
        {
            itemCountText = GameObject.Find("ItemCountText")?.GetComponent<TextMeshProUGUI>();
        }

    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // アイテム表示（既存）
        if (itemCountText != null)
        {
            itemCountText.text = "ITEMS: " +
                GameManager.Instance.GetItemCount() + " / " +
                GameManager.Instance.GetRequiredItemCount();
        }

        // スコア表示
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " +
                GameManager.Instance.GetScore();
        }

        // タイマー表示
        if (timerText != null)
        {
            // 残り時間を整数に切り上げて表示
            int timeInt = Mathf.CeilToInt(
                GameManager.Instance.GetRemainingTime());
            timerText.text = "TIME: " + timeInt;

            // 残り10秒以下は赤くする
            if (timeInt <= 10)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }
}