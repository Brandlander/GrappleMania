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
        SceneManager.LoadScene("WinScreen");
    }
}
