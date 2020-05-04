using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : LivingEntity
{
    public Slider healthSlider;

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.maxValue = startHealth;
        healthSlider.value = health;
    }

    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        healthSlider.value = health;
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        
        if (!base.ApplyDamage(damageMessage)) return false;
        healthSlider.value = health;
        return true;
    }

    public override void Die()
    {
        base.Die();
        GameManager.gameManager.EndGame();
    }
}
