using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearUIManager : MonoBehaviour
{
    public Canvas canvas;
    public Transform santaTransform;

    public AudioSource clearAudioSource;
    public AudioClip buttonClip;


    private void Update()
    {
        if(santaTransform.position.z >= 35)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void ChangeToPlayScene()
    {
        clearAudioSource.PlayOneShot(buttonClip);
        SceneManager.LoadScene("MainScene");
    }

}
