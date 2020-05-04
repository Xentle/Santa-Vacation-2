using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private AudioSource itemAudioSource;
    public AudioClip useClip;
    
    private float x, z;
    public float mapSize;
    public GameObject snowball;
    
    private void Awake()
    {
        itemAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            itemAudioSource.PlayOneShot(useClip);

            // 눈덩이 굴러오는 처리 추가
            for (int i = 0; i < 12; i++)
            {
                x = -mapSize;
                z = mapSize + i * 4;
                Instantiate(snowball, new Vector3(x, 0.6f, z), Quaternion.identity);
            }

            this.GetComponent<SphereCollider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;

            Invoke("DestroyObject", 5f);
        }
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
