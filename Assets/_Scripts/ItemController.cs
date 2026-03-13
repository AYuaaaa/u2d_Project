using UnityEngine;
using DG.Tweening; // 引入 DOTween 增加动效

public class ItemController : MonoBehaviour
{
    [Header("道具配置")]
    public int weaponIndex = 1;      // 对应 Player 列表里的第几个武器
    public float duration = 10f;     // 持续时间

    private SpriteRenderer spriteRenderer;
    private Collider2D itemCollider;
    private bool hasAppeared = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        hasAppeared = false;

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (itemCollider != null) itemCollider.enabled = false;

        // 初始缩放设为 0
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        //进入视野才激活
        if (!hasAppeared)
        {
            float cameraRightEdge = Camera.main.transform.position.x + 9.5f; 
            if (transform.position.x < cameraRightEdge)
            {
                Appear();
            }
        }
    }

    private void Appear()
    {
        hasAppeared = true;

        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (itemCollider != null) itemCollider.enabled = true;

        // 道具弹出的动效：有个回弹效果 (OutBack)
        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
        
        transform.DOMoveY(transform.position.y + 0.2f, 0.6f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasAppeared) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyPowerUp(weaponIndex, duration);
                Debug.Log("吃到道具！形态切换中...");
            }

            transform.DOKill();
            
            gameObject.SetActive(false);
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.eatItemSound);
    }
}