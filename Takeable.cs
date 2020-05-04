using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Takeable : MonoBehaviour
{
    public bool StopMove = false;
    [HideInInspector]
    public Vr_pointer m_ActivePointer = null;
}
