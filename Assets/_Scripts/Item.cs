using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Item : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public GameObject dropFx;
    public GameObject piecesParent;

    private void Start()
    {
        
    }

    public void ReleaseItemOnCurry(Transform pan)
    {
        GetComponent<Collider>().enabled = false;
        trailRenderer.enabled = false;
        piecesParent.SetActive(true);
        piecesParent.transform.parent = pan;
        pan.GetComponent<PreparingPot>().cookedSlices.Add(piecesParent);
        /*dropFx.SetActive(true);
        dropFx.transform.parent = null;*/

        transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => { gameObject.SetActive(false); });
    }
}
