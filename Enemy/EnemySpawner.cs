using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Enemy_basic enemyPrefab1;
    public Enemy_tracker enemyPrefab2;
    public Enemy_sky enemyPrefab3;
    public Enemy_ranged enemyPrefab4;
    private Vector3 randomPosition;
    public float mapSize;
    float level = 2, deltatime;
    public float leveltime;
    private float cur_leveltime;
    // private List<Enemy> enemies = new List<Enemy>();

    public float waitTime;
    private float timer = 0;

    private void Awake()
    {
        randomPosition = Utility.GetRandomPointOnNavMesh(
        Vector3.zero, 20f, NavMesh.AllAreas);
        cur_leveltime = leveltime;
    }

    void Update()
    {
        timer += Time.deltaTime;
        deltatime += Time.deltaTime;

        if (timer > cur_leveltime)
        {
            cur_leveltime += leveltime;
            level += 1;
            waitTime *= 0.5f;
        }

        if (deltatime > waitTime)
        {
            CreateEnemy();
            deltatime = 0;
        }
    }

    private void CreateEnemy()
    {
        float x, z;
        float random = Random.Range(2.0f, 6.0f);
        x = Random.Range(30.0f, -40.0f);
        z = 45.0f;
        
        if (random >= 2.0f && random <= 3.1f)
        {
            Enemy_basic enemy3 = Instantiate(enemyPrefab1, new Vector3(x, 0.5f, z), Quaternion.identity);
        }
        else if (random > 3.1f && random <= 4.2f)
        {
            Enemy_tracker enemy4 = Instantiate(enemyPrefab2, new Vector3(x, 0.5f, z), Quaternion.identity);
        }
        else if (random > 4.2f && random <= 4.9f)
        {
            Enemy_sky enemy5 = Instantiate(enemyPrefab3, new Vector3(x, 2.8f, z), Quaternion.identity);
        }
        else if (random > 4.9f && random <= 6.0f)
        {
            Enemy_ranged enemy6 = Instantiate(enemyPrefab4, new Vector3(x, 0.5f, z), Quaternion.identity);
        }
    }
}