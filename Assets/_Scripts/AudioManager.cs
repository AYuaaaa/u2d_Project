using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("音频源")]
    public AudioSource musicSource; // 用于播放背景音乐（循环）
    public AudioSource sfxSource;   // 用于播放即时音效

    [Header("音频剪辑")]
    public AudioClip bgm;           // 背景音乐
    public AudioClip jumpSound;     // 跳跃音效
    public AudioClip eatItemSound;  // 吃到道具
    public AudioClip enemyDieSound; // 敌人死亡
    public AudioClip gameOverSound; // 游戏结束

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景时不销毁音效管理
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 游戏开始自动播放 BGM
        if (bgm != null) PlayMusic(bgm);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }
}