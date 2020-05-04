using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public struct DamageMessage
{
    public GameObject damager;
    public float amount;
    
    public Vector3 hitPoint; //공격 가해진 위치
    public Vector3 hitNormal; //공격 가해진 방향의 반대방향

}
