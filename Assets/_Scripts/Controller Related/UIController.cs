using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [Header("HUD RELATED")]
    public Slider cookSlider;
    public GameObject moneyDisplayContent;

    [Header("GAMEPLAY RELATED")] 
    public GameObject doneButton;
    
    [Header("VAR DECLARATIONS")]
    [SerializeField] public float sliderValue;
    private void Awake()
    {
        instance = this;
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
        switch (newState)
        {
            case MainController.StateOfGame.Started:
                moneyDisplayContent.SetActive(false);
                doneButton.SetActive(false);
                cookSlider.gameObject.SetActive(true);
                cookSlider.transform.DOScaleX(0, 0.5f).From();
                break;
        }
    }

    
    
    public void UpdateCookStatus()
    {
        sliderValue += 0.1f;
        cookSlider.value = sliderValue;
    }
}
