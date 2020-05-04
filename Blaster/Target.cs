using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
   public Color m_FlashDamageColor = Color.white;
   private MeshRenderer m_MeshRenderer = null;

   private Color m_OriginalColor = Color.white;

   private int m_MaxHealth = 2;
   private int m_Health = 0;

   private void Awake()
   {
      //throw new NotImplementedException();
      m_MeshRenderer = GetComponent<MeshRenderer>();
      
   }

   private void OnEnable()
   {
      //throw new NotImplementedException();
      ResetHealth();
   }

   private void OnCollisionEnter(Collision other)
   {
      throw new NotImplementedException();
   }

   void Damage()
   {
      
   }

   IEnumerator Flash()
   {
      yield return null;
   }

   void RemoveHealth()
   {
      
   }

   void ResetHealth()
   {
      
   }

   private void Kill()
   {
      
   }
}
