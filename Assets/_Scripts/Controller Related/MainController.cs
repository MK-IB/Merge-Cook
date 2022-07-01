using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public enum StateOfGame
    {
        None,
        Started,
        Preparation,
        Decoration,
        Serving,
        EatingDone,
        Win,
        Lose
    }

    public static MainController instance;
    
    public StateOfGame _stateOfGame;
    public static event Action<StateOfGame, StateOfGame> GameStateChanged;

    public StateOfGame GameState
    { 
        get => _stateOfGame;
       private set
        {
            if (value != _stateOfGame)
            {
                StateOfGame old = _stateOfGame;
                _stateOfGame = value;
                if (GameStateChanged != null)
                    GameStateChanged(_stateOfGame, old);
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateGame();
    }

    void CreateGame()
    {
        GameState = StateOfGame.Started;
    }

    public void SetActionType(StateOfGame _curState)
    {
        GameState = _curState;
    }
}
