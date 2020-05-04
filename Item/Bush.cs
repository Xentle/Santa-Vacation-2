using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bush : MonoBehaviour
{
    public float monsterStopTime = 3f;
    public float monsterStopRange = 50f;

    private BaseHealth baseHealth;

    private AudioSource itemAudioSource;
    public AudioClip useClip;

    private Collider[] colls;
    public LayerMask EnemyLayer;



    private void Awake()
    {
        baseHealth = GameObject.FindGameObjectWithTag("Base").GetComponent<BaseHealth>();
        itemAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            itemAudioSource.PlayOneShot(useClip);

            colls = Physics.OverlapSphere(baseHealth.gameObject.transform.position, monsterStopRange, EnemyLayer);

            for (int i = 0; i < colls.Length; i++)
            {
                GameObject enemy = colls[i].gameObject; //후정
                enemy.GetComponent<NavMeshAgent>().isStopped = true;
                //후정 
                Debug.Log(enemy.name + "stopped attacking");
                enemy.GetComponent<Animator>().enabled = false;
            }
            Debug.Log("Monster Stop");

            this.GetComponent<MeshCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;

            Invoke("MonsterMove", monsterStopTime);
        }
    }


    public void MonsterMove()
    {
        colls = Physics.OverlapSphere(baseHealth.gameObject.transform.position, monsterStopRange, EnemyLayer);


        for (int i = 0; i < colls.Length; i++)
        {
            GameObject enemy = colls[i].gameObject;

            var distance = Vector3.Distance(baseHealth.gameObject.transform.position, enemy.transform.position);

            if (enemy.name == "Boximon Cyclopes(Clone)" &&
                distance > enemy.GetComponent<Enemy_ranged>().attackDistance)
            {
                enemy.GetComponent<NavMeshAgent>().isStopped = false;
            }
            enemy.GetComponent<NavMeshAgent>().isStopped = false;

            //후정
            Debug.Log(enemy.name + "resume attacking");
            enemy.GetComponent<Animator>().enabled = true;
        }
        Debug.Log("Monster Move");

        //System.Array.Clear(colls, 0, colls.Length);



        Destroy(this.gameObject);
    }
}
