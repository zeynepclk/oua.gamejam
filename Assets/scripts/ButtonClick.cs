using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public void Exitgame()
    {
        Application.Quit();
    }
    public void MainScene()
    {
        SceneManager.LoadScene("LevelDesign");
    }
    public void HowtoPlay()
    {
        SceneManager.LoadScene("HowtoPlay");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
