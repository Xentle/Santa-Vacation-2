using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;


    private PlayerMovement playerMovement;
    private Animator playerAnimator;

    public bool undamaged;



    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
        undamaged = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.maxValue = startHealth;
        healthSlider.value = health;

        playerMovement.enabled = true;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!undamaged) // 대미지를 받을 수 있는 상태라면
        {
            base.OnDamage(damage, hitPoint, hitNormal);
            playerAnimator.SetTrigger("isDamaged");
            healthSlider.value = health;
        }
    }
    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        
        if (!base.ApplyDamage(damageMessage)) return false;
        if (!dead && !undamaged)
        {
            healthSlider.value = health;
            playerAnimator.SetTrigger("isDamaged");
            
        }
        return true;
    }
    public override void RestoreHealth(float newHealth)
    {
        
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    public override void Die()
    {
        Debug.Log("player dead!");
        healthSlider.value = 0;
        base.Die();
        playerAnimator.SetTrigger("isDie");
        GameManager.gameManager.EndGame();
    }
}