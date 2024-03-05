using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Intro");
    }

    public void Rules()
    {
        SceneManager.LoadScene("Rules");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
