using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro; // 因为你使用了 Text (TMP)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI 面板引用")]
    public GameObject gamePanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("分数文本引用")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    private int currentScore = 0;

    void Awake() => Instance = this;

    void Start()
    {
        Time.timeScale = 1;
        gamePanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        UpdateScoreUI();

        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            // 停止之前的任何音量动画
            AudioManager.Instance.musicSource.DOKill(); 
            AudioManager.Instance.musicSource.volume = 1f; 
        
            //重新播放
            if (!AudioManager.Instance.musicSource.isPlaying)
            {
                AudioManager.Instance.musicSource.Play();
            }
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
        if (finalScoreText != null) finalScoreText.text = "积分: " + currentScore;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0; // 游戏停止
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1; // 游戏恢复
    }

    public void ShowGameOver()
    {
        //停止背景音乐，防止和死亡音乐重叠
        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            AudioManager.Instance.musicSource.Stop(); 
        }

        //播放死亡音效
        if (AudioManager.Instance != null) 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound);
        }
        if (AudioManager.Instance != null) 
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound);

        if (finalScoreText != null) finalScoreText.text = "Score: " + currentScore;

        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.transform.DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); 

        Time.timeScale = 0; 
        if (finalScoreText != null) finalScoreText.text = "Score: " + currentScore;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        DOTween.KillAll();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("正在退出游戏...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}