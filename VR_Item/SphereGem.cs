using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereGem : MonoBehaviour
{
    public float monsterStopTime = 3f;
    public float monsterStopRange = 200f;

    private PlayerHealth playerHealth;

    private AudioSource itemAudioSource;
    public AudioClip useClip;

    

    private Collider[] colls;
    public LayerMask EnemyLayer;



    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<PlayerHealth>();
        itemAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            itemAudioSource.PlayOneShot(useClip);

            colls = Physics.OverlapSphere(playerHealth.gameObject.transform.position, monsterStopRange, EnemyLayer);

            for (int i = 0; i < colls.Length; i++)
            {
                GameObject enemy = colls[i].gameObject; //후정
                enemy.GetComponent<NavMeshAgent>().isStopped = true;
                //후정 
                Debug.Log(enemy.name + "stopped attacking");
                enemy.GetComponent<Animator>().enabled = false;
            }
            Debug.Log("Monster Stop");

            this.GetComponent<SphereCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;

            

            Invoke("MonsterMove", monsterStopTime);
        }
    }


    public void MonsterMove()
    {
        colls = Physics.OverlapSphere(playerHealth.gameObject.transform.position, monsterStopRange, EnemyLayer);


        for (int i = 0; i < colls.Length; i++)
        {
            GameObject enemy = colls[i].gameObject;

            var distance = Vector3.Distance(playerHealth.gameObject.transform.position, enemy.transform.position);

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
