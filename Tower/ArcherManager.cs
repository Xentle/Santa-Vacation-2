using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManager : MonoBehaviour
{
    public float range;
    public float Hp;
    public float damage;
    public float attackSpeed;
    public GameObject Arrow;
    GameObject[] Enemylist;
    float minDistance;
    int minIndex;
    float tempDistance;
    int tempIndex;
    float deltaTime;
    Vector3 targetDir;
    Vector3 rotateangle;
    Vector3 forward = new Vector3(0.0f, 0.0f, 1.0f);
    Vector3 arrowForward = new Vector3(0.0f, 0.0f, 1.0f);
    Quaternion m = new Quaternion();
    ArrowManager arrowManager;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Enemylist = GameObject.FindGameObjectsWithTag("Enemy");
        minDistance = (Enemylist[0].GetComponent<Transform>().position - transform.position).magnitude;
        minIndex = 0;
        tempIndex = 0;
        foreach (GameObject Enemy in Enemylist)
        {
            tempDistance = (Enemy.GetComponent<Transform>().position - transform.position).magnitude;
            if (tempDistance < minDistance)
            {
                minDistance = tempDistance;
                minIndex = tempIndex;
            }
            tempIndex++;
        }

        targetDir = Enemylist[minIndex].GetComponent<Transform>().position - transform.position;
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
                // m = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, 1.0f), targetDir);
                m = Quaternion.FromToRotation(arrowForward, targetDir);
                Arrow.GetComponent<ArrowManager>().dir = Vector3.Normalize(targetDir);
                Instantiate(Arrow, transform.position, m * Quaternion.identity);
            }
        }
        else
        {
            deltaTime = 0;
            Arrow.GetComponent<ArrowManager>().dir = Vector3.zero;
        }
    }
}
