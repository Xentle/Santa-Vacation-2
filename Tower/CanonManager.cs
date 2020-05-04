using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonManager : MonoBehaviour
{
    public float range;
    public float Hp;
    public float damage;
    public float attackSpeed;
    public GameObject Bomb;
    public GameObject target;
    GameObject[] Enemylist;
    float minDistance;
    int minIndex;
    float tempDistance;
    int tempIndex;
    float deltaTime;
    Vector3 targetDir;
    Vector3 forward = new Vector3(0.0f, 0.0f, 1.0f);
    Quaternion m = new Quaternion();
    BombManager bombManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Enemylist = GameObject.FindGameObjectsWithTag("Enemy");
        minDistance = 2.0f * range;
        minIndex = 0;
        tempIndex = 0;
        foreach (GameObject Enemy in Enemylist)
        {
            if (Enemy.transform.position.y <= 1.0f)
            {
                tempDistance = (Enemy.GetComponent<Transform>().position - transform.position).magnitude;
                if (tempDistance < minDistance)
                {
                    minDistance = tempDistance;
                    minIndex = tempIndex;
                    target = Enemy;
                }
            }
            tempIndex++;
        }

        targetDir = target.transform.position - transform.position;
        if (minDistance < range)
        {
            m = Quaternion.FromToRotation(forward, new Vector3(targetDir.x, 0.0f, targetDir.z));
            transform.rotation = m * transform.rotation;
            // transform.Rotate(0.0f, m.eulerAngles.y, 0.0f, Space.World);
            forward = m * forward;
            // forward.y = 0.0f;

            deltaTime += Time.deltaTime;

            if (deltaTime > 1.0f / attackSpeed)
            {
                deltaTime = 0;
                targetDir = Enemylist[minIndex].GetComponent<Transform>().position - new Vector3(transform.position.x, 1.4f, transform.position.z);
                // m = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, 1.0f), targetDir);
                // Bomb.GetComponent<BombManager>().dir = Vector3.Normalize(targetDir);
                Instantiate(Bomb, new Vector3(transform.position.x, transform.position.y + 1.4f, transform.position.z), Quaternion.identity);
            }
        }
        else
        {
            deltaTime = 0;
            // Bomb.GetComponent<BombManager>().dir = Vector3.zero;
        }

    }
}
