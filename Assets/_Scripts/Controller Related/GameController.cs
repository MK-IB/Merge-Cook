using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

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
                ;
        }

    }
}