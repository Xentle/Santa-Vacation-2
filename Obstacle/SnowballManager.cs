using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class SnowballManager : MonoBehaviour
{
    Vector3 dir, dir2, dir3;
    public ParticleSystem explosion;
    private Rigidbody rb_self;
    // private GameObject gameObject;
    private Transform transform;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        rb_self = GetComponent<Rigidbody>();
        dir.z = Random.Range(1.0f, -1.0f);
        dir.x = 1.0f;
        dir.y = 0.0f;
        Vector3.Normalize(dir);
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        moveCharacter(dir);
        Debug.Log("Snoball pos : " + transform.position.x);
        if (transform.position.x > 45.0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Snowball" + other.gameObject.tag);
        if (other.gameObject.tag == "Enemy")
        {
            dir2.y = 1.0f;
            dir2.z = Random.Range(1.0f, -1.0f);
            dir2.x = Random.Range(1.0f, -1.0f);
            dir3.x = Random.Range(1.0f, -1.0f);
            dir3.y = Random.Range(1.0f, -1.0f);
            dir3.z = Random.Range(1.0f, -1.0f);
            agent = other.gameObject.GetComponent<NavMeshAgent>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (agent != null)
                agent.enabled = false;
            
                rb.isKinematic = false;
            rb.AddForce(dir2 * 300);
            rb.AddTorque(dir3 * 100);
            /*gameObject = other.gameObject;
            Invoke("ByeByeMonster", 2.8f);*/
            Destroy(other.gameObject,3.2f);
        }
            
    }


    void moveCharacter(Vector3 direction)
    {
        rb_self.AddForce(direction * 30);
    }

    /*void ByeByeMonster()
    {
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
    }*/
}
