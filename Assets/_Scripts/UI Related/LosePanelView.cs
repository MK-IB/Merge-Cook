using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LosePanelView : MonoBehaviour
{
    public Transform loseText;
    public Transform emoji;
    public Transform tryAgainButton;

    private void OnEnable()
    {
        StartCoroutine(ShowUIs());
    }
    IEnumerator ShowUIs()
    {
        loseText.DOScaleX(0, 0.3f).From();
        yield return new WaitForSeconds(0.5f);
        emoji.transform.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.5f);
        tryAgainButton.transform.DOScale(Vector3.one, 0.3f);
    }
}
