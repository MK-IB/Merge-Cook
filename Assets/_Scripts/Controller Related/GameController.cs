using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool dirtyItemAdded;

    private void Awake()
    {
        Instance = this;
    }
    
    private void OnEnable()
    {
        MainController.GameStateChanged += GameManager_OnGameStateChanged;
    }
    private void OnDisable()
    {
        MainController.GameStateChanged -= GameManager_OnGameStateChanged;
    }
    
    void GameManager_OnGameStateChanged(MainController.StateOfGame newState, MainController.StateOfGame oldState)
    {
        switch(newState)
        {
            case MainController.StateOfGame.Started:
                print("Level started...");
                MainController.instance.SetActionType(MainController.StateOfGame.Preparation);
                break;
            case MainController.StateOfGame.EatingDone:
                CameraController.instance.cinemachineBrain.m_DefaultBlend.m_Time = 1;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void On_ContinueButtonPressed()
    {
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }
    
    public void On_RetryButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int _dirtyItemCounter;
    public void CheckLevelFailForDirtyItemAddition()
    {
        _dirtyItemCounter++;
        if (_dirtyItemCounter >= 3)
            dirtyItemAdded = true;
    }
}