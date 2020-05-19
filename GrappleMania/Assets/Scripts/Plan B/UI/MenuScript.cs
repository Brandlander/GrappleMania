using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject MainUI;
    public GameObject LevelSelectUI;
    public GameObject ControlsScreen;

    public void ChangeScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void LevelSelectActivate()
    {
        MainUI.SetActive(false);
        LevelSelectUI.SetActive(true);
    }

    public void LevelSelectDeactivate()
    {
        MainUI.SetActive(true);
        LevelSelectUI.SetActive(false);
    }

    public void ControlsScreenActivate()
    {
        MainUI.SetActive(false);
        ControlsScreen.SetActive(true);
    }

    public void ControlsScreenDeactivate()
    {
        MainUI.SetActive(true);
        ControlsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game exited successfully");
    }
}
