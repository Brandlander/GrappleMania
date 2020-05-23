using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    // On collision check if it was the player and finish the level if it is
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            LevelFinished();
    }

    public void LevelFinished()
    {
        if (SceneManager.GetActiveScene().name == "GameScreen")
            SceneManager.LoadScene("Level2");
        else if (SceneManager.GetActiveScene().name == "Level2")
            SceneManager.LoadScene("WinScreen");
    }
}
