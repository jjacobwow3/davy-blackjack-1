using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

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

    public void IntroOne()
    {
        SceneManager.LoadScene("IntroFish");
    }

    public void IntroTwo()
    {
        SceneManager.LoadScene("IntroShark");
    }

    public void IntroThree()
    {
        SceneManager.LoadScene("IntroKraken");
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("GameFish");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("GameShark");
    }

    public void LevelThree()
    {
        SceneManager.LoadScene("GameKraken");
    }

    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
