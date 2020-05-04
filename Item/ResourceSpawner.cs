using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject[] resources;

    public Transform baseTransform;
    public float maxDistance;              // Base로부터 얼마나 떨어진 곳까지 아이템을 생성할지
    public float minDistance;

    public float maxSpawnTime = 10f;    // 아이템 생성 최대 간격
    public float minSpawnTime = 5f;     // 아이템 생성 최소 간격
    private float timeBetSpawn;         // 아이템 생성 간격 (Max와 Min 사이 랜덤 값)
    private float lastSpawnTime;
    public float duration = 30f;        // 아이템 생성 후 얼마나 지속되는지


    void Start()
    {
        timeBetSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        lastSpawnTime = 0f;
    }

    void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            lastSpawnTime = Time.time;
            timeBetSpawn = Random.Range(minSpawnTime, maxSpawnTime);
            Spawn();
        }
    }

    // 아이템 생성
    private void Spawn()
    {
        Vector3 spawnPosition = GetRandomPoint(baseTransform.position, minDistance, maxDistance);

        GameObject item = resources[Random.Range(0, resources.Length)];
        GameObject spawnItem = Instantiate(item, spawnPosition, Quaternion.identity);

        Destroy(spawnItem, duration);
    }

    // 중심이 center, 반지름이 distance인 구 안에 임의의 점을 골라,
    // 그 점과 가장 가까운 내비메시위의 점을 반환
    private Vector3 GetRandomPoint(Vector3 center, float minRange, float maxRange)
    {
        do
        {
            Vector3 randomPosition = Random.insideUnitSphere * maxRange + center;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, maxRange, NavMesh.AllAreas);

            if (Mathf.Pow(hit.position.x, 2.0f) + Mathf.Pow(hit.position.z, 2.0f) > Mathf.Pow(minRange, 2.0f))
            {
                return hit.position;
            }

        } while (true);
    }
}
