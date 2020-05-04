using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour
{
    PlayerMovement playermovement;
    public float StunTime;
    float deltaStunTime;
    bool checkTime = false;
    bool eventFinished = true;
    string objectName;
    private Rigidbody playerRigidbody;

    void Start()
    {
        playermovement = GetComponent<PlayerMovement>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if(eventFinished)
        {
            objectName = col.gameObject.name;
            if (objectName == "Snowball(Clone)")
            {
                eventFinished = false;
                Destroy(col.gameObject);

                playermovement.canMove = false;
                checkTime = true;
            }

            if (objectName == "Gem(Clone)")
            {
                eventFinished = false;
                Destroy(col.gameObject);

                //playermovement.moveSpeed /= 2.0f;
                playerRigidbody.drag *= 2.0f;

                checkTime = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkTime)
        {
            deltaStunTime += Time.deltaTime;

            if (deltaStunTime > StunTime)
            {
                deltaStunTime = 0;

                if (objectName == "Snowball(Clone)")
                {
                    playermovement.canMove = true;
                }
                else if (objectName == "Gem(Clone)")
                {
                    playerRigidbody.drag /= 2.0f;
                }

                checkTime = false;
                eventFinished = true;
            }
        }
    }
}
