using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    //void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
    bool ApplyDamage(DamageMessage damageMessage); 
    //공격을 실행한 obj, 가해진 위치 , normal
}
