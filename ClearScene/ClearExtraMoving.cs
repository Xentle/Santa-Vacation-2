using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearExtraMoving : MonoBehaviour
{
    public GameObject player;
    public float speed = 0.6f;
    public float distance;


    void Update()
    {
        if ((transform.position - player.transform.position).magnitude >= distance)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
