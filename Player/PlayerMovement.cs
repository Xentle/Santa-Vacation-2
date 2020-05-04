using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float rotateSpeed = 180f;
    public bool canMove = true;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            Rotate();
            Move();
        }

        
    }

    private void Move()
    {
        //Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        //playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

        //slding effect 후정
        playerRigidbody.AddForce(playerInput.move * transform.forward, ForceMode.Impulse);
        
    }

    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }
}
