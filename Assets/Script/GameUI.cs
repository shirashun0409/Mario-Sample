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

    void Start()
    {
        // テキストが設定されていない場合は自動で検索
        if (itemCountText == null)
        {
            itemCountText = GameObject.Find("ItemCountText")?.GetComponent<TextMeshProUGUI>();
        }

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// UIを更新する
    /// </summary>
    private void UpdateUI()
    {
        if (itemCountText != null && GameManager.Instance != null)
        {
            int current = GameManager.Instance.GetItemCount();
            int required = GameManager.Instance.GetRequiredItemCount();
            itemCountText.text = "ITEMS: " + current + " / " + required;
        }
    }
}