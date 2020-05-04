using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCloudMoving : MonoBehaviour
{
    public float speed = 1f;


    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
