using UnityEngine;
using System.Collections;
using DG.Tweening; // 如果你使用了 DOTween 动画

public class EnemyController : MonoBehaviour
{
    [Header("战斗配置")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private bool hasAppeared = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
    }
    void OnEnable()
    {
        currentHealth = maxHealth;
        hasAppeared = false;
        //隐藏贴图，关闭碰撞
        if (spriteRenderer != null) 
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.enabled = false; 
        }
        if (enemyCollider != null) enemyCollider.enabled = false;
        
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (!hasAppeared)
        {
            // 获取摄像机在世界坐标的右边缘
            float cameraRightEdge = Camera.main.transform.position.x + 9f;
            // 如果敌人进入了视野范围
            if (transform.position.x < cameraRightEdge)
            {
                Appear();
            }
        }
    }

    private void Appear()
    {
        hasAppeared = true;
        
        // 开启贴图和碰撞
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (enemyCollider != null) enemyCollider.enabled = true;
        // 增加一个出场效果：0.3秒内从小变大弹出
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void TakeDamage(int damage)
    {
        // 还没出场时免疫伤害
        if (!hasAppeared) return;

        currentHealth -= damage;
        StopCoroutine(HitFlash()); // 防止连续受击时协程冲突
        StartCoroutine(HitFlash());
        
        if (currentHealth <= 0) Die();
    }

    private IEnumerator HitFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    private void Die()
    {
        // 增加分数
        if (GameManager.Instance != null && hasAppeared) {
            GameManager.Instance.AddScore(10); // 每个怪10分
        }

        transform.DOScale(Vector3.zero, 0.15f).OnComplete(() => {
            gameObject.SetActive(false);
        });
        AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDieSound);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 只有出现在视野里的敌人才能撞击玩家
        if (!hasAppeared) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.TakeDamage(1);
            Die();
        }
    }
}