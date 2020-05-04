using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager
    {
        get { if(m_gameManager==null)
            {
                m_gameManager = FindObjectOfType<GameManager>();
            }
            return m_gameManager;
        }
    }

    private static GameManager m_gameManager;

    public PlayerMovement playerMovement;

    public bool isGameover { get; private set; }

    private void Awake()
    {
        if(gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void EndGame()
    {
        Debug.Log("It's the end game now");
        isGameover = true;
        //playerMovement.enabled = false;

        SceneManager.LoadScene("vrFailScene");
    }
}
