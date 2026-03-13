using UnityEngine;
using System.Collections.Generic;

public class ScrollingChunk : MonoBehaviour
{
    [HideInInspector] public float scrollSpeed;

    [Header("空中敌人设置 (Point1)")]
    public Transform airSpawnPoint;
    public string airEnemyTag = "Enemy_Air"; // 对象池空中怪标签
    [Range(0, 1)] public float airSpawnChance = 0.3f;

    [Header("地面敌人设置 (Point2)")]
    public Transform groundSpawnPoint;
    public string groundEnemyTag = "Enemy2"; // 对象池地面怪标签
    [Range(0, 1)] public float groundSpawnChance = 0.5f;

    [Header("道具设置")]
    public Transform[] itemSpawnPoints;
    public string[] itemTags = { "Item1" };
    [Range(0, 1)] public float itemSpawnChance = 0.2f;

    private float chunkWidth;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        chunkWidth = (sr != null) ? sr.bounds.size.x : 18.5f;
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }

    public void RefreshEntities()
    {
        foreach (var obj in spawnedObjects) if (obj != null) obj.SetActive(false);
        spawnedObjects.Clear();

        if (ObjectPool.Instance == null) return;

        SpawnSpecific(airSpawnPoint, airEnemyTag, airSpawnChance);

        SpawnSpecific(groundSpawnPoint, groundEnemyTag, groundSpawnChance);


        foreach (var pt in itemSpawnPoints)
        {
            if (Random.value < itemSpawnChance)
            {
                string t = itemTags[Random.Range(0, itemTags.Length)];
                GameObject go = ObjectPool.Instance.SpawnFromPool(t, pt.position, Quaternion.identity);
                if (go != null) { go.transform.SetParent(transform); spawnedObjects.Add(go); }
            }
        }
    }

    private void SpawnSpecific(Transform pt, string tag, float chance)
    {
        if (pt == null || string.IsNullOrEmpty(tag)) return;
        if (Random.value < chance)
        {
            GameObject go = ObjectPool.Instance.SpawnFromPool(tag, pt.position, Quaternion.identity);
            if (go != null)
            {
                go.transform.SetParent(transform);
                spawnedObjects.Add(go);
            }
        }
    }

    public float GetWidth() => chunkWidth;
}