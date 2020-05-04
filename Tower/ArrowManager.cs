using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public Vector3 dir;
    public float speed;
    public float damage = 20.0f;
    public float range;
    float deltaTime = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        deltaTime += Time.deltaTime;
        if (deltaTime > range)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        var target = col.gameObject.GetComponent<LivingEntity>();
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Enemy")
        {
            // Destroy(col.gameObject);
            //Destroy(this.gameObject);

            ContactPoint contact = col.contacts[0];
            var message = new DamageMessage();
            message.amount = damage;
            message.damager = gameObject;
            message.hitPoint = contact.point;
            message.hitNormal = contact.normal;
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            target.ApplyDamage(message);
            Destroy(this.gameObject);
        }
    }
}
