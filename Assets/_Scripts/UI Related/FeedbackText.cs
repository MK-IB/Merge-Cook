using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FeedbackText : MonoBehaviour
{
    public static FeedbackText instance;
    
    public List<Sprite> texts;
    public Image feedbackText;

    private void Awake()
    {
        instance = this;
    }

    public void PlayTextEffect()
    {
        feedbackText.sprite = texts[Random.Range(0, texts.Count)];
        feedbackText.gameObject.SetActive(true);
        feedbackText.color = new Color(255,255,255,255);
        feedbackText.DOColor(new Color(0,0,0,0),  0.5f);
        feedbackText.transform.DOScale(Vector3.one * 2, 0.5f).OnComplete(() =>
        {
            feedbackText.transform.localScale = Vector3.zero;
        });
    }
    
    void Update()
    {
        
    }
}
