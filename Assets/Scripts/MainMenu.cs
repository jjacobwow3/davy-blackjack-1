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

    public void RulesBack()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Map()
    {
        SceneManager.LoadScene("Map");
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
