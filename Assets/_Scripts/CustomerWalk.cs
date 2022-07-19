using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerWalk : MonoBehaviour
{
    private Animator _animator;
    public Transform movePos;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            WalkForOrder();
        }
    }

    void WalkForOrder()
    {
        _animator.SetTrigger("walk");
        transform.DOMove(movePos.position, 5f).OnComplete(() => { _animator.SetTrigger("idle");});
    }
}
