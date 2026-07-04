using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するマネージャークラス
/// EmptyObjectにアタッチして使用
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("スコア設定")]
    [SerializeField]
    private int scorePerItem = 100;      // アイテム1個あたりのスコア

    [SerializeField]
    private int scorePerStomp = 200;     // 敵踏みつけのスコア

    [SerializeField]
    private int timeBonus = 10;          // 残り時間1秒あたりのボーナス

    [Header("タイマー設定")]
    [SerializeField]
    private float timeLimit = 60f;       // 制限時間（秒）

    // スコアとタイマー
    private int score = 0;
    private float remainingTime;

    // シングルトンインスタンス
    public static GameManager Instance { get; private set; }

    // ゲームの状態
    public enum GameState
    {
        Title,
        Playing,
        GameOver,
        GameClear
    }

    // 現在のゲーム状態
    public GameState CurrentState { get; private set; }

    // アイテム取得数
    private int itemCount = 0;

    // クリアに必要なアイテム数
    [SerializeField]
    private int requiredItemCount = 3;

    void Awake()
    {
        // シングルトンパターン
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 現在のシーン名から状態を設定
        UpdateStateFromScene();
    }

    void Update()
    {
        // タイマー処理（プレイ中のみ）
        if (CurrentState == GameState.Playing)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                GameOver();  // 時間切れ！
                return;
            }
        }

        // スペースキー入力の処理（既存のコード）
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // ... 既存の処理 ...
        }
    }
    /// <summary>
    /// 現在のシーン名からゲーム状態を更新
    /// </summary>
    private void UpdateStateFromScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "TitleScene":
                CurrentState = GameState.Title;
                break;
            case "GameScene":
                CurrentState = GameState.Playing;
                break;
            case "GameOverScene":
                CurrentState = GameState.GameOver;
                break;
            case "GameClearScene":
                CurrentState = GameState.GameClear;
                break;
        }
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        itemCount = 0;
        score = 0;                         // スコアをリセット
        remainingTime = timeLimit;         // タイマーをリセット
        CurrentState = GameState.Playing;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// タイトル画面に戻る
    /// </summary>
    public void ReturnToTitle()
    {
        itemCount = 0;
        CurrentState = GameState.Title;
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// ゲームオーバーにする
    /// </summary>
    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        SceneManager.LoadScene("GameOverScene");
    }

    /// <summary>
    /// ゲームクリアにする
    /// </summary>
    public void GameClear()
    {
        CurrentState = GameState.GameClear;
        SceneManager.LoadScene("GameClearScene");
    }

    /// <summary>
    /// アイテムを取得した時に呼ばれる
    /// </summary>
    public void CollectItem()
    {
        itemCount++;
        score += scorePerItem;  // スコア加算
        Debug.Log("スコア: " + score +
                  " アイテム: " + itemCount + " / " + requiredItemCount);

        if (itemCount >= requiredItemCount)
        {
            // クリアボーナス: 残り時間 × timeBonus
            int bonus = Mathf.CeilToInt(remainingTime) * timeBonus;
            score += bonus;
            GameClear();
        }
    }

    /// <summary>
    /// 現在のアイテム数を取得
    /// </summary>
    public int GetItemCount()
    {
        return itemCount;
    }

    /// <summary>
    /// 必要なアイテム数を取得
    /// </summary>
    public int GetRequiredItemCount()
    {
        return requiredItemCount;
    }

    /// <summary>
    /// スコアを加算する（敵を倒した時など）
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
    }

    /// <summary>
    /// 現在のスコアを取得
    /// </summary>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// 残り時間を取得
    /// </summary>
    public float GetRemainingTime()
    {
        return remainingTime;
    }


}