using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public float baseHeal = 50f;
    private BaseHealth baseHealth;

    private AudioSource itemAudioSource;
    public AudioClip useClip;


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

            baseHealth.RestoreHealth(baseHeal);
            Debug.Log("Base Heal");

            this.GetComponent<MeshCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;

            Invoke("DestroyObject", 3f);
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
