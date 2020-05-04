using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHealth = 100f;            // 최대 체력
    public float health; // 현재 체력  { get; protected set; } 
    public bool dead;   // true면 죽은 상태, flase면 생존 상태 { get; protected set; }
    public bool onhit;


    public event Action OnDeath;
    
    private const float minTimeBetDamaged = 0.1f;
    private float lastDamagedTime;
    public bool IsInvulnerable
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;
            return true; //공격받은지 0.1초도 되지 않았다면 무적모드
        }

        
    }
    
    // 오브젝트가 활성화 되면 현재체력과 dead 초기화
    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    // 대미지를 받음
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if(health <=0 && !dead)
        {
            Die();
        }
    }
    // 후정 대미지 
    public virtual bool ApplyDamage(DamageMessage damageMessage)
    {
        //공격 실패
        if (IsInvulnerable || damageMessage.damager == gameObject || dead) return false;
        
        //공격 성공
        lastDamagedTime = Time.time;
        health -= damageMessage.amount;

        if (health <= 0) Die();
        return true;
    }

    // 체력 회복
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead)    // 죽은 경우 회복 불가능
        {
            return;
        }

        if ((health + newHealth) <= startHealth)
        {
            health += newHealth;
        }
        else
        {
            health = startHealth;
        }
    }

    // 죽음
    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();
        dead = true;
    }

   
}
