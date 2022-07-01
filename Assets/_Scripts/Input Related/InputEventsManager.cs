
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventsManager : MonoBehaviour
{
    public static InputEventsManager instance;
    public event Action SwipeRightEvent;
    public event Action SwipeLeftEvent;
    public event Action SwipeUpEvent;
    public event Action SwipeDownEvent;
    public event Action LevelCompleteEvent;

    private void Awake()
    {
        instance = this;
    }

    public void StartSwipeRightEvent()
    {
        if (SwipeRightEvent != null)
            SwipeRightEvent();
    }
    public void StartSwipeLeftEvent()
    {
        if (SwipeLeftEvent != null)
            SwipeLeftEvent();
    }
    public void StartSwipeUpEvent()
    {
        if (SwipeUpEvent != null)
            SwipeUpEvent();
    }
    public void StartSwipeDownEvent()
    {
        if (SwipeDownEvent != null)
            SwipeDownEvent();
    }

    public void StartLevelCompleteEvent()
    {
        if (LevelCompleteEvent != null)
            LevelCompleteEvent();
    }
}
