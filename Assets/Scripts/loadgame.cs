using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class loadgame : MonoBehaviour
{
    public void LoadGame()
    {
        ScoreManager.coinAmount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");

    }
}
