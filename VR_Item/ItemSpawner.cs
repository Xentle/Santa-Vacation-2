using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;

    public Transform centerTransform;
    public float distance;              // centerTransform로부터 얼마나 떨어진 곳까지 아이템을 생성할지

    public float maxSpawnTime = 10f;    // 아이템 생성 최대 간격
    public float minSpawnTime = 2f;     // 아이템 생성 최소 간격
    private float timeBetSpawn;         // 아이템 생성 간격 (Max와 Min 사이 랜덤 값)
    private float lastSpawnTime;
    public float duration = 5f;        // 아이템 생성 후 얼마나 지속되는지
    public float leveltime;
    private float cur_leveltime;
    // private List<Enemy> enemies = new List<Enemy>();

    private float timer = 0;


    void Start()
    {
        timeBetSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        lastSpawnTime = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > cur_leveltime)
        {
            cur_leveltime += leveltime;
            maxSpawnTime *= 0.8f;
            minSpawnTime *= 0.8f;
        }

        if(Time.time >= lastSpawnTime + timeBetSpawn)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(minSpawnTime, maxSpawnTime);
            Spawn();
        }
    }

    // 아이템 생성
    private void Spawn()
    {
        Vector3 spawnPosition = GetRandomPoint(centerTransform.position, distance);

        GameObject item = items[Random.Range(0, items.Length)];
        GameObject spawnItem = Instantiate(item, spawnPosition, Quaternion.identity);

        Destroy(spawnItem.GetComponent<MeshRenderer>(), duration);
        Destroy(spawnItem.GetComponent<SphereCollider>(), duration);
        Destroy(spawnItem, duration * 2);    
    }

    // 중심이 center, 반지름이 range인 구 안에 임의의 점을 골라
    // y좌표가 5이상이면 반환

    private Vector3 GetRandomPoint(Vector3 center, float range)
    {
        do
        {
            Vector3 randomPosition = Random.insideUnitSphere * range + center;

            if (randomPosition.y >= 5f)
            {
                return randomPosition;
            }

        } while (true);
    }
}
