using UnityEngine;

public class CornBullet : MonoBehaviour
{
    public float speed = 10f;
    public float explosionRadius = 3f; // 爆炸半径
    public GameObject explosionEffect; // 拖入爆炸特效
    public LayerMask enemyLayer;

    void Update() {
        transform.position += Vector3.right * speed * Time.deltaTime;

        float screenRightEdge = Camera.main.transform.position.x + 12f;
        if (transform.position.x > screenRightEdge) {
        gameObject.SetActive(false); // 回收到对象池
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy2") || other.CompareTag("Enemy")) {
            Explode();
            gameObject.SetActive(false);
        }
    }

    void Explode() {
        //视觉效果
        if(explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //大范围检测
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemy in results) {
            if (enemy.CompareTag("Enemy2") || enemy.CompareTag("Enemy")) {
                var ec = enemy.GetComponent<EnemyController>();
                if (ec != null) ec.TakeDamage(99); // 爆炸直接秒杀
            }
        }
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}