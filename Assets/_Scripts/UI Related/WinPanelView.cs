using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WinPanelView : MonoBehaviour
{
    public Transform ribbon;
    public Transform item;

    private void OnEnable()
    {
        StartCoroutine(ShowUIs());
    }

    IEnumerator ShowUIs()
    {
        ribbon.DOScaleX(0, 0.3f).From();
        yield return new WaitForSeconds(0.5f);
        item.DOScale(Vector3.one, 0.3f);
    }
}
