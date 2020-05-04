using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    public float undamgedTime = 3f;
    private PlayerHealth playerHealth;

    private AudioSource itemAudioSource;
    public AudioClip useClip;

    private float previousHealth;


    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        itemAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            previousHealth = playerHealth.health;

            itemAudioSource.PlayOneShot(useClip);

            playerHealth.undamaged = true;
            Debug.Log("Undamaged");

            this.GetComponent<MeshCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;

            Invoke("Damaged", undamgedTime);
        }
    }

    public void Damaged()
    {
        if (playerHealth.dead == true)
        {
            playerHealth.health = 0f;
        }
        else
        {
            playerHealth.health = previousHealth;
        }

        playerHealth.undamaged = false;

        Debug.Log("damaged");

        Destroy(this.gameObject);
    }
}
