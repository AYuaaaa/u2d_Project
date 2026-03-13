using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening; 
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSetting {
        public string weaponName;
        public string bulletTag;
        public float fireRate;
        public Sprite mouthSprite;
    }

    public List<WeaponSetting> weaponConfigs = new List<WeaponSetting>();
    public Transform firePoint;
    public SpriteRenderer mouthRenderer;
    public Transform shadowTransform;

    [Header("物理跳跃")]
    public float jumpForce = 12f;
    public float gravity = -35f;
    private Vector3 velocity;
    private float groundY;
    private int jumpCount = 0;
    private bool isGrounded = true;
    private Vector3 initialShadowScale;

    private WeaponSetting currentWeapon;
    private float nextFireTime;

    void Start() {
        groundY = transform.position.y;
        if (shadowTransform != null) initialShadowScale = shadowTransform.localScale;
        if (weaponConfigs.Count > 0) SetWeapon(weaponConfigs[0]);

        StartHop();
    }

    void StartHop() {
        transform.DOLocalMoveY(groundY + 0.2f, 0.3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.OutQuad)
            .SetId("HopAnim"); 
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {

            if (EventSystem.current.IsPointerOverGameObject()) 
            {
                return; 
            }
            if (isGrounded || jumpCount < 2) {
                DOTween.Kill("HopAnim"); 
                
                velocity.y = jumpForce;
                isGrounded = false;
                jumpCount++;

                // 跳跃拉伸效果
                transform.DOScale(new Vector3(0.85f, 1.15f, 1f), 0.1f).OnComplete(() => transform.DOScale(Vector3.one, 0.1f));
            }
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSound);
        }

        // 模拟物理重力
        if (!isGrounded) {
            velocity.y += gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;

            if (transform.position.y <= groundY) {
                transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
                isGrounded = true;
                velocity.y = 0;
                jumpCount = 0;

                StartHop();

                // 落地挤压效果
                transform.DOScale(new Vector3(1.15f, 0.85f, 1f), 0.1f).OnComplete(() => transform.DOScale(Vector3.one, 0.1f));
            }
        }

        // 影子逻辑
        if (shadowTransform != null) {
            float height = transform.position.y - groundY;
            float scale = Mathf.Clamp(1 - (height * 0.15f), 0.4f, 1f);
            shadowTransform.localScale = initialShadowScale * scale;
        }

        if (Time.time >= nextFireTime && currentWeapon != null) {
            Shoot();
            nextFireTime = Time.time + 1f / currentWeapon.fireRate;
        }
    }

    void Shoot() {
        if (ObjectPool.Instance != null)
            ObjectPool.Instance.SpawnFromPool(currentWeapon.bulletTag, firePoint.position, Quaternion.identity);
    }

    public void ApplyPowerUp(int index, float duration) {
        if (index >= 0 && index < weaponConfigs.Count) {
            StopAllCoroutines();
            StartCoroutine(PowerUpRoutine(index, duration));
        }
    }

    private IEnumerator PowerUpRoutine(int index, float duration) {
        SetWeapon(weaponConfigs[index]);
        yield return new WaitForSeconds(duration);
        SetWeapon(weaponConfigs[0]);
    }

    public void SetWeapon(WeaponSetting settings) {
        currentWeapon = settings;
        if (mouthRenderer != null) mouthRenderer.sprite = settings.mouthSprite;
    }

    public void TakeDamage(int dmg) =>Die();
    public void Die() {
        Debug.Log("Game Over!");
        DOTween.KillAll(); 
    
        // 触发 UI 结算
        if (GameManager.Instance != null) {
            GameManager.Instance.ShowGameOver();
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy2") || other.CompareTag("Enemy")) Die();
    }
}