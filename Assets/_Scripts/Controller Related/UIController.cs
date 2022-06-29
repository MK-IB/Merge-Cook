using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    public Slider cookSlider;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public float sliderValue;
    
    public void UpdateCookStatus()
    {
        sliderValue += 0.1f;
        cookSlider.value = sliderValue;
    }
}
