using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するマネージャークラス
/// EmptyObjectにアタッチして使用
/// </summary>
public class GameManager : MonoBehaviour
{
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
        // タイトル、ゲームオーバー、ゲームクリア画面でスペースキー入力を処理
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            switch (CurrentState)
            {
                case GameState.Title:
                    StartGame();
                    break;
                case GameState.GameOver:
                case GameState.GameClear:
                    ReturnToTitle();
                    break;
            }
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
        Debug.Log("アイテム取得: " + itemCount + " / " + requiredItemCount);

        // クリア条件を達成したらゲームクリア
        if (itemCount >= requiredItemCount)
        {
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
}