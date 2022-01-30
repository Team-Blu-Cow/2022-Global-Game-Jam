using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        m_mainMenu = GameObject.Find("Main Menu");
        m_optionsMenu = GameObject.Find("Options Menu");

        m_optionsMenu.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwitchMenus()
    {
        if (m_mainMenu.activeSelf)
        {
            m_mainMenu.SetActive(false);
            m_optionsMenu.SetActive(true);
        }
        else
        {
            m_mainMenu.SetActive(true);
            m_optionsMenu.SetActive(false);
        }
    }

}
