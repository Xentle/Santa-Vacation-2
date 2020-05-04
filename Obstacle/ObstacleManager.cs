using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public float SnowballSpawnTime;
    public GameObject snowball;
    float deltaSnowballSpawnTime;
    float x, z;

    // Update is called once per frame
    void Update()
    {
        deltaSnowballSpawnTime += Time.deltaTime;
        
        if(deltaSnowballSpawnTime > SnowballSpawnTime)
        {
            deltaSnowballSpawnTime = 0;
            for (int i = 0; i < 2; i++)
            {
                // x = -42 or 42        z = 25 ~ 50
                x = Random.Range(1.0f, -1.0f);
                z = Random.Range(25.0f, 50.0f);
                if (x >= 0)
                    x = 42.0f;
                else
                    x = -42.0f;
                Instantiate(snowball, new Vector3(x, 0.6f, z), Quaternion.identity);
            }
        }
    }
}
