using UnityEngine;

/// <summary>
/// BGMと効果音を管理するクラス
/// EmptyObjectにアタッチして使用
/// </summary>
public class SoundManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    [SerializeField]
    private AudioClip bgmTitle;       // タイトルBGM

    [SerializeField]
    private AudioClip bgmGame;        // ゲームBGM

    [Header("効果音")]
    [SerializeField]
    private AudioClip seJump;         // ジャンプ音

    [SerializeField]
    private AudioClip seItem;         // アイテム取得音

    [SerializeField]
    private AudioClip seStomp;        // 踏みつけ音

    [SerializeField]
    private AudioClip seGameOver;     // ゲームオーバー音

    [SerializeField]
    private AudioClip seClear;        // クリア音

    [Header("音量設定")]
    [SerializeField]
    private float bgmVolume = 0.5f;   // BGM音量

    [SerializeField]
    private float seVolume = 1.0f;    // 効果音音量

    // AudioSourceコンポーネント
    private AudioSource bgmSource;    // BGM用
    private AudioSource seSource;     // 効果音用

    void Awake()
    {
        // シングルトンパターン（GameManagerと同じ！）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// AudioSourceを2つ作成（BGM用と効果音用）
    /// </summary>
    private void SetupAudioSources()
    {
        // BGM用のAudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;        // ループ再生
        bgmSource.volume = bgmVolume;
        bgmSource.playOnAwake = false;

        // 効果音用のAudioSource
        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;        // ループしない
        seSource.volume = seVolume;
        seSource.playOnAwake = false;
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    public void PlayBGM(string bgmName)
    {
        AudioClip clip = null;

        switch (bgmName)
        {
            case "title":
                clip = bgmTitle;
                break;
            case "game":
                clip = bgmGame;
                break;
        }

        if (clip != null && bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// BGMを停止する
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    /// <summary>
    /// 効果音を再生する
    /// </summary>
    public void PlaySE(string seName)
    {
        AudioClip clip = null;

        switch (seName)
        {
            case "jump":
                clip = seJump;
                break;
            case "item":
                clip = seItem;
                break;
            case "stomp":
                clip = seStomp;
                break;
            case "gameover":
                clip = seGameOver;
                break;
            case "clear":
                clip = seClear;
                break;
        }

        if (clip != null)
        {
            // PlayOneShotは同時に複数の音を鳴らせる
            seSource.PlayOneShot(clip, seVolume);
        }
    }
}