using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public float range;
    public GameObject effect;
    GameObject[] Enemylist;
    float distance;
    private AudioSource itemAudioSource;
    public AudioClip useClip;

    // Start is called before the first frame update
    void Start()
    {
        itemAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player")
        {
            Debug.Log("asoijfdiosanxiconvxiconvzionvvcvczzxcioxnoin");
            Enemylist = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject Enemy in Enemylist)
            {
                distance = (Enemy.GetComponent<Transform>().position - transform.position).magnitude;
                if (distance < range)
                {
                    Destroy(Enemy);
                }
            }
            Instantiate(effect, transform.position, Quaternion.identity);
            itemAudioSource.PlayOneShot(useClip);
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.gameObject, 2.0f);
        }
    }
}
