using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    public enum GameState
    {
        Started,
        Win,
        Lose
    }

    public GameState gameState;

    private void Awake()
    {
        Instance = this;
    }
    
    
}
