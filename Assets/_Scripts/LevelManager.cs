using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("层级配置")]
    public List<ScrollingChunk> groundChunks = new List<ScrollingChunk>();
    public float groundSpeedMultiplier = 1.0f;

    [Header("速度控制 (随时间增加)")]
    public float initialSpeed = 5f;
    public float maxSpeed = 12f;         // 速度上限
    public float acceleration = 0.1f;    // 每秒增加的速度
    
    public float CurrentGlobalSpeed { get; private set; }
    private float groundLastX;
    private Camera mainCam;

    void Awake() { Instance = this; }

    void Start()
    {
        mainCam = Camera.main;
        CurrentGlobalSpeed = initialSpeed;

        if (groundChunks.Count > 0)
        {
            // 按X轴排序并计算初始末尾位置
            groundChunks = groundChunks.OrderBy(c => c.transform.position.x).ToList();
            var last = groundChunks.Last();
            groundLastX = last.transform.position.x + last.GetWidth() / 2;
            foreach (var c in groundChunks) c.RefreshEntities();
        }
    }

    void Update()
    {
        if (CurrentGlobalSpeed < maxSpeed)
            CurrentGlobalSpeed += acceleration * Time.deltaTime;

        UpdateLayer(groundChunks, ref groundLastX, groundSpeedMultiplier);
    }

    void UpdateLayer(List<ScrollingChunk> chunkList, ref float lastX, float multiplier)
    {

        if (chunkList == null || chunkList.Count == 0) return;

        float speed = CurrentGlobalSpeed * multiplier;

        float recycleThreshold = mainCam.transform.position.x - 20f; 

        for (int i = 0; i < chunkList.Count; i++)
        {
            ScrollingChunk chunk = chunkList[i];

            chunk.transform.position += Vector3.left * speed * Time.deltaTime;

            float chunkWidth = chunk.GetWidth();
            if (chunk.transform.position.x + chunkWidth / 2 < recycleThreshold)
            {
                chunk.transform.position = new Vector3(lastX + chunkWidth, chunk.transform.position.y, 0);
                lastX = chunk.transform.position.x; 

                // 刷新地块内容（敌人/道具）
                chunk.RefreshEntities();
            }
        }
    }
}