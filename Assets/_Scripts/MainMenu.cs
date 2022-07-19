using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("level", 1) > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
        }
    }
}
