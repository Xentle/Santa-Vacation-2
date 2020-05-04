using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public float playerHeal = 50f;      // 회복량
    public PlayerHealth playerHealth;
    //private GameObject cameraRig;

    
    private AudioSource itemAudioSource;
    public AudioClip useClip;

    


    private void Awake()
    {
       // cameraRig = GameObject.FindGameObjectWithTag("MainCamera");
       
       //playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
       playerHealth = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<PlayerHealth>();
       itemAudioSource = GetComponent<AudioSource>();   
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Projectile")
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
