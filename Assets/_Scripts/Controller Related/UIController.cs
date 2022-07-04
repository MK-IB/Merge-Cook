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
    public GameObject HUD;
    public Slider cookSlider;
    public GameObject moneyDisplayContent;
    public TextMeshProUGUI moneyCountText;
    public RectTransform moneyIcon;
    public TextMeshProUGUI gameStateIndicatorText;
    public string dishName;
    public GameObject bombCountPanel;
    public bool isBombLevel;
    public bool isStackLevel;

    [Header("GAMEPLAY RELATED")] 
    public GameObject preparationDoneButton;
    public GameObject decorationDoneButton;
    public GameObject moneyControlledParticle;
    public GameObject moneyEarnedPanel;
    public GameObject moneyBundleOnCanvas;
    public GameObject moneyUIPrefab;
    public RectTransform moneyspawnPos;
    public GameObject winCanvas;
    public GameObject failCanvas;

    [Header("VAR DECLARATIONS")]
    [SerializeField] public float sliderValue;

    private int _moneyCount;
    
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(isBombLevel) bombCountPanel.SetActive(true);
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
                gameStateIndicatorText.SetText("PREPARING " + dishName);
                break;
            case MainController.StateOfGame.Decoration:
                gameStateIndicatorText.SetText("DECORATION");
                CameraController.instance.decorationCamera.SetActive(true);
                
                if(isStackLevel) PreparingPot.instance.StartCoroutine(PreparingPot.instance.MoveStackForDecoration());
                else PreparingPot.instance.StartCoroutine(PreparingPot.instance.MoveForDecoration());
                break;
            case MainController.StateOfGame.Serving:
                gameStateIndicatorText.SetText("SERVING");
                break;
            case MainController.StateOfGame.EatingDone:
                moneyDisplayContent.SetActive(true);
                moneyDisplayContent.transform.DOScale(Vector3.zero, 1).From();
                break;
            case MainController.StateOfGame.Win:
                StartCoroutine(ShowWinUIs());
                break;
            case MainController.StateOfGame.Lose:
                DOVirtual.DelayedCall(3f, () =>
                {
                    HUD.SetActive(false);
                    failCanvas.SetActive(true);
                });
                break;
        }
    }
    public void UpdateCookStatus()
    {
        sliderValue += 0.1f;
        cookSlider.value = sliderValue;
        if (sliderValue >= 1)
        {
            cookSlider.transform.DOScaleX(0, 0.5f);
            preparationDoneButton.SetActive(true);
            PlayController.instance.HideGrid();
            InputControl.instance.enabled = false;
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

    IEnumerator ShowWinUIs()
    {
        moneyEarnedPanel.SetActive(true);
        moneyEarnedPanel.transform.DOScaleX(0, 0.3f).From();
        yield return new WaitForSeconds(1.8f);
        
        Vector3 moneyBundleWorldPos = Camera.main.ScreenToWorldPoint(moneyBundleOnCanvas.transform.position);
        //moneyControlledParticle.transform.position = new Vector3(moneyBundleWorldPos.x,moneyBundleWorldPos.y, moneyControlledParticle.transform.position.z);
        //moneyControlledParticle.GetComponent<ParticleControlScript>().PlayControlledParticles(moneyControlledParticle.transform.position, moneyIcon);
        StartCoroutine(UpdateMoneyOnWin());
        yield return new WaitForSeconds(2);
        winCanvas.SetActive(true);
    }

    IEnumerator UpdateMoneyOnWin()
    {
        int currentAmount = PlayerPrefs.GetInt("money");
        int newEarnedAmount = PlayerPrefs.GetInt("money") + 100;

        /*for (int i = 0; i < 20; i++)
        {
            RectTransform moneyUI = Instantiate(moneyUIPrefab, moneyUIPrefab.transform.position, Quaternion.identity).GetComponent<RectTransform>();
            moneyUI.transform.parent = moneyDisplayContent.transform;
            moneyUI.anchoredPosition = moneyspawnPos.anchoredPosition;
            yield return new WaitForSeconds(2f/20f);
            moneyUI.DOAnchorPos(moneyIcon.anchoredPosition, 0.3f);
        }*/
        float waitTime = 2f / 100f;
        for (int i = currentAmount; i <= newEarnedAmount; i++)
        {
            moneyCountText.text = i.ToString();
            yield return new WaitForSeconds(waitTime);
        }
        PlayerPrefs.SetInt("money", newEarnedAmount);
    }

    int _bombCounter;
    public void UpdateBombCounter()
    {
        bombCountPanel.transform.GetChild(_bombCounter++).GetChild(0).gameObject.SetActive(true);
        if(_bombCounter >= 3) MainController.instance.SetActionType(MainController.StateOfGame.Lose);
    }
}
