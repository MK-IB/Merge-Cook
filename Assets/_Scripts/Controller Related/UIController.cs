using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    [Header("HUD RELATED")]
    public Slider cookSlider;
    public GameObject moneyDisplayContent;
    public TextMeshProUGUI gameStateIndicatorText;

    [Header("GAMEPLAY RELATED")] 
    public GameObject preparationDoneButton;
    public GameObject decorationDoneButton;
    
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
                cookSlider.gameObject.SetActive(true);
                cookSlider.transform.DOScaleX(0, 0.5f).From();
                break;
            case MainController.StateOfGame.Preparation:
                gameStateIndicatorText.SetText("PREPARING");
                break;
            case MainController.StateOfGame.Decoration:
                gameStateIndicatorText.SetText("DECORATION");
                CameraController.instance.decorationCamera.SetActive(true);
                PreparingPot.instance.StartCoroutine(PreparingPot.instance.MoveForDecoration());
                break;
            case MainController.StateOfGame.Serving:
                gameStateIndicatorText.SetText("SERVING");
                break;
            case MainController.StateOfGame.EatingDone:
                moneyDisplayContent.SetActive(true);
                moneyDisplayContent.transform.DOScale(Vector3.zero, 1).From();
                break;
        }
    }
    public void UpdateCookStatus()
    {
        sliderValue += 0.05f;
        cookSlider.value = sliderValue;
        if (sliderValue >= 1)
        {
            cookSlider.transform.DOScaleX(0, 0.5f);
            preparationDoneButton.SetActive(true);
            PlayController.instance.HideGrid();
            PlayController.instance.enabled = false;
        }
    }

    public void On_Preparation_DoneButtonClicked()
    {
        MainController.instance.SetActionType(MainController.StateOfGame.Decoration);
    }
    public void On_Decoration_DoneButtonClicked()
    {
        MainController.instance.SetActionType(MainController.StateOfGame.Serving);
    }
}
