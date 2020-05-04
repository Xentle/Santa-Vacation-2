using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject arrow;
    public ParticleSystem explosion;
   public float m_Lifetime = 5.0f; //lifetime of projectile, destroy when not hit at some point
   private Rigidbody m_Rigidbody = null; // using it alot
   public float duration = 2f;

   private AudioSource itemAudioSource;
   public AudioClip useClip;
  
   public float damage = 100.0f;
   
   float deltaTime = 0;
   private void Awake()
   { 
       
       itemAudioSource = GetComponent<AudioSource>();
      m_Rigidbody = GetComponent<Rigidbody>();
      SetInnactive(); //when instantiated, immediately disable
      
   }

   private void OnCollisionEnter(Collision other)
   {
       Instantiate(explosion, this.transform.position, Quaternion.identity);

       var target = other.gameObject.GetComponent<LivingEntity>();
       //Debug.Log(other.gameObject.name);
       if (other.gameObject.tag == "Enemy")
       {
           itemAudioSource.Play();
           //
           Destroy(other.gameObject);
           
           //script don't need in steam vr
           /*ContactPoint contact = other.contacts[0];
           var message = new DamageMessage();
           message.amount = damage;
           message.damager = gameObject;
           message.hitPoint = contact.point;
           message.hitNormal = contact.normal;
           //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
           target.ApplyDamage(message);*/
           //Destroy(this.gameObject);
       }
       Destroy(GetComponent<MeshRenderer>());
       Destroy(GetComponent<BoxCollider>());
       // Destroy(arrow, duration);
       // SetInnactive(); //disable itself when hit
   }

   public void Launch(Blaster blaster)
   {
       //Position on blaster
       
       transform.position = blaster.m_Barrel.position; 
       transform.rotation = blaster.m_Barrel.rotation;
       
       //Activate, enable
       gameObject.SetActive(true);
       
       //apply force, Fire, and track
       //Todo:setup force
       m_Rigidbody.AddRelativeForce(Vector3.forward * blaster.m_Force, ForceMode.Impulse);
       StartCoroutine(TrackLifetime());
   }

   private IEnumerator TrackLifetime()
   {
       yield return new WaitForSeconds(m_Lifetime);
       SetInnactive();
   }

   public void SetInnactive()
   {
       m_Rigidbody.velocity = Vector3.zero;
       m_Rigidbody.angularVelocity = Vector3.zero;
       
       gameObject.SetActive(false);
   }
}
