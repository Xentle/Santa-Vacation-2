using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroUIManager : MonoBehaviour
{
    public Button playButton;
    public Button helpButton;
    public Button exitButton;

    public GameObject firstHelpImage;
    public GameObject secondHelpImage;
    public GameObject thirdHelpImage;

    public AudioSource introAudioSource;

    public AudioClip buttonClip;



    public void ChangeToPlayScene()
    {
        introAudioSource.PlayOneShot(buttonClip);
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        introAudioSource.PlayOneShot(buttonClip);
        Application.Quit();
    }

    public void OpenFirstHelpImage()
    {
        introAudioSource.PlayOneShot(buttonClip);
        firstHelpImage.SetActive(true);
    }

    public void CloseFirstHelpImage()
    {
        introAudioSource.PlayOneShot(buttonClip);
        firstHelpImage.SetActive(false);
    }

    public void OpenSecondHelpImage()
    {
        introAudioSource.PlayOneShot(buttonClip);
        secondHelpImage.SetActive(true);
    }

    public void CloseSecondHelpImage()
    {
        introAudioSource.PlayOneShot(buttonClip);
        secondHelpImage.SetActive(false);
    }

    public void OpenThirdHelpImage()
    {
        introAudioSource.PlayOneShot(buttonClip);
        thirdHelpImage.SetActive(true);
    }

    public void CloseThirdHelpImage()
    {
        introAudioSource.PlayOneShot(buttonClip);
        thirdHelpImage.SetActive(false);
    }
}
