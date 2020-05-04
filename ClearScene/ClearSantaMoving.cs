using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSantaMoving : MonoBehaviour
{
    public float speed = 1.5f;


    void Update()
    {
        if (transform.position.z <= 35)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime);
        }
    }
}
