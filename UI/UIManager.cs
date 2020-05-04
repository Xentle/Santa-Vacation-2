using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager
    {
        get
        {
            if (m_uiManager == null)
            {
                m_uiManager = FindObjectOfType<UIManager>();
            }
            return m_uiManager;
        }
    }

    private static UIManager m_uiManager;

    public Text woodText;
    public Text stoneText;
    public Text timer;
    public Button restartButton;

    public PlayerResource playerResource;

    public float timeLimit;


    void Update()
    {
        if (!GameManager.gameManager.isGameover)
        {
            timeLimit -= Time.deltaTime;
            timer.text = Mathf.Round(timeLimit).ToString();


            if(timeLimit<=10)
            {
                timer.fontStyle = FontStyle.Bold;
            }

            if (timeLimit <= 0)
            {
                timeLimit = 0;
                timer.text = "0";

                SceneManager.LoadScene("vrClearScene");
            }
        }
    }


    public void UpdateWood()
    {
        woodText.text = playerResource.wood.ToString();
    }


    public void UpdateStone()
    {
        stoneText.text = playerResource.stone.ToString();
    }

    public void ActiveRestartButton(bool active)
    {
        restartButton.gameObject.SetActive(active);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
