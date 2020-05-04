using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : LivingEntity
{
    public Slider healthSlider;
    public GameObject effect;

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
        Debug.Log("Why is OnDamage called?");
        base.OnDamage(damage, hitPoint, hitNormal);

        healthSlider.value = health;
    }

    public override bool ApplyDamage(DamageMessage damageMessage)
    {
        Debug.Log("TowerHealth: " + health);
        if (!base.ApplyDamage(damageMessage)) return false;
        healthSlider.value = health;
        return true;
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("Tower Destroy");
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
