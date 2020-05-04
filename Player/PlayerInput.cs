using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";

    public float move { get; private set; }
    public float rotate { get; private set; }

    public Animator playerAnimator;
    

    void Update()
    {
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);

        if (move != 0)
        {
            playerAnimator.SetBool("isMove", true);
        }
        else
        {
            playerAnimator.SetBool("isMove", false);
        }
    }
}
