using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelView : MonoBehaviour
{
    public Transform ribbon;
    public Image item;

    private void OnEnable()
    {
        StartCoroutine(ShowUIs());
    }

    IEnumerator ShowUIs()
    {
        ribbon.DOScaleX(0, 0.3f).From();
        yield return new WaitForSeconds(0.5f);
        //item.sprite = ItemHolder.instance.GetCurrentItemInLevel();
        item.transform.DOScale(Vector3.one, 0.3f);
    }
}
