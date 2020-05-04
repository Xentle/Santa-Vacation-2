using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResource : MonoBehaviour
{
    public int wood { get; private set; }
    public int stone { get; private set; }

    public AudioSource resourceAudioSource;

    public AudioClip getResourceClip;
    public AudioClip useResourceClip;

    private void Start()
    {
        wood = 2;
        stone = 2;


    }

    public void GetWood(int num)
    {
        wood += num;

        resourceAudioSource.PlayOneShot(getResourceClip);
    }

    public void GetStone(int num)
    {
        stone += num;

        resourceAudioSource.PlayOneShot(getResourceClip);
    }

    public void UseWood(int num)
    {
        if ((wood-num) <= 0)
        {
            wood = 0;
        }
        else
        {
            wood -= num;
        }

        resourceAudioSource.PlayOneShot(useResourceClip);
    }

    public void UseStone(int num)
    {
        if ((stone - num) <= 0)
        {
            stone = 0;
        }
        else
        {
            stone -= num;
        }

        resourceAudioSource.PlayOneShot(useResourceClip);
    }
}
