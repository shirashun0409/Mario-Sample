using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// タイトル画面のUI制御クラス
/// </summary>
public class TitleUI : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private TextMeshProUGUI pressSpaceText;

    void Start()
    {
        // テキストが設定されていない場合は自動で検索
        if (titleText == null)
        {
            titleText = GameObject.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        }

        if (pressSpaceText == null)
        {
            pressSpaceText = GameObject.Find("PressSpaceText")?.GetComponent<TextMeshProUGUI>();
        }

        // テキスト内容を設定
        if (titleText != null)
        {
            titleText.text = "MARIO SAMPLE";
        }

        if (pressSpaceText != null)
        {
            pressSpaceText.text = "PRESS SPACE TO START";
        }
    }

    void Update()
    {
        // 点滅アニメーション（PRESS SPACEテキスト）
        if (pressSpaceText != null)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 2f));
            Color color = pressSpaceText.color;
            color.a = alpha;
            pressSpaceText.color = color;
        }

        // スペースキーでゲーム開始
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }
    }
}