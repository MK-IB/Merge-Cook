using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            DOVirtual.DelayedCall(1f, ()=> {
                UIController.instance.UpdateCookStatus();
                item.ReleaseItemOnCurry(transform.parent.parent); 
            });
        }
    }
}
