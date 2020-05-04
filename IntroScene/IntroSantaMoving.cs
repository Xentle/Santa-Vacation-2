using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSantaMoving : MonoBehaviour
{
    private Animator animator;

    public float duration;
    private float lastTime;


    void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetBool("isBasePoint", false);
    }


    void Update()
    {
        
        if(transform.position.z >= 0.2)
        {
            animator.SetBool("isBasePoint", true);

            if(Time.time >= lastTime + duration)
            {
                lastTime = Time.time;


                int ranNum = Random.Range(0, 3);

                if (ranNum == 0)
                {
                    WavingArm();
                }
                else if (ranNum == 1)
                {
                    FistUp();
                }
                else
                {
                    SwingBody();
                }
            }
        }


    }

    void  WavingArm()
    {
        animator.SetTrigger("wavingHand");
    }

    void FistUp()
    {
        animator.SetTrigger("fistUp");
    }

    void SwingBody()
    {
        animator.SetTrigger("swingBody");
    }


    
}
