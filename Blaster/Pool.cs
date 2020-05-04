using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
   //not a component
   //creating projectiles and reusing
   public  List<T> Create<T>(GameObject prefab, int count)
      where T: MonoBehaviour
   {
      //New list to return 
      List<T> newPool = new List<T>();
      
      
      //Create Projectiles
      for (int i = 0; i < count; i++)
      {
         GameObject projectileObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
         T newProjectile = projectileObject.GetComponent<T>();

         newPool.Add(newProjectile);
      }
      return newPool;
   }
}

public class ProjectilePool : Pool
{
   public List<Projectile> m_Projectiles = new List<Projectile>();

   public ProjectilePool(GameObject prefab, int count) //not monobehavior so constructor
   {
      m_Projectiles = Create<Projectile>(prefab, count);
   }

   public void DisableAllProjectiles()
   {
      foreach (Projectile projectile in m_Projectiles)
      {
         projectile.SetInnactive();
      }
   }
}
