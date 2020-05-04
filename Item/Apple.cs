using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public float playerHeal = 50f;
    private PlayerHealth playerHealth;

    private AudioSource itemAudioSource;
    public AudioClip useClip;


    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        itemAudioSource = GetComponent<AudioSource>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            itemAudioSource.PlayOneShot(useClip);

            playerHealth.RestoreHealth(playerHeal);
            Debug.Log("Player Heal");

            this.GetComponent<SphereCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;

            Invoke("DestroyObject", 3f);
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
