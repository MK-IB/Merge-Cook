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
    [SerializeField] private bool keepThePiece; 

    private void Start()
    {
        
    }

    public void ReleaseItemOnCurry(Transform pan)
    {
        if(!keepThePiece)
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
        else
        {
            pan.GetComponent<PreparingPot>().cookedSlices.Add(gameObject);
            transform.parent = pan;
            transform.DOScale(transform.localScale * 0.8f, 1);
        }
    }
}
