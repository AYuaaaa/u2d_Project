using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 1;
    public string targetTag = "Enemy2"; // 直接用字符串，不引用任何类

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        
        float screenRightEdge = Camera.main.transform.position.x + 12f;
    
        if (transform.position.x > screenRightEdge) {
        gameObject.SetActive(false); // 回收到对象池
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            // 获取敌人组件并扣血
            var enemy = other.GetComponent<EnemyController>();
            if (enemy != null) enemy.TakeDamage(damage);
            
            gameObject.SetActive(false);
        }
    }
}