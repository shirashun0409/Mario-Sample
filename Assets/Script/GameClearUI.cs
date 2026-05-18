using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// ゲームクリア画面のUI制御クラス
/// </summary>
public class GameClearUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI gameClearText;

    [SerializeField]
    private TextMeshProUGUI pressSpaceText;

    void Start()
    {
        // テキストが設定されていない場合は自動で検索
        if (gameClearText == null)
        {
            gameClearText = GameObject.Find("GameClearText")?.GetComponent<TextMeshProUGUI>();
        }

        if (pressSpaceText == null)
        {
            pressSpaceText = GameObject.Find("PressSpaceText")?.GetComponent<TextMeshProUGUI>();
        }

        // テキスト内容を設定
        if (gameClearText != null)
        {
            gameClearText.text = "GAME CLEAR";
        }

        if (pressSpaceText != null)
        {
            pressSpaceText.text = "PRESS SPACE TO TITLE";
        }
    }

    void Update()
    {
        // 点滅アニメーション
        if (pressSpaceText != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 2f));
            Color color = pressSpaceText.color;
            color.a = alpha;
            pressSpaceText.color = color;
        }

        // スペースキーでタイトルへ
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ReturnToTitle();
            }
        }
    }
}