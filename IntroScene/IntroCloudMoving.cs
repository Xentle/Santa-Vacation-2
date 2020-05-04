using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCloudMoving : MonoBehaviour
{
    public float speed = 1f;


    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if(transform.position.x >= 63)
        {
            transform.position += new Vector3(-120, 0, 0);
        }
    }
}
