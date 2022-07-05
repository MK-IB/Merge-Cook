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
        if (other.gameObject.name.Contains("Bomb"))
        {
            //explosion
            GameObject spawnFx = EffectsController.instance.explosionFx;
            Instantiate(spawnFx, other.transform.position, spawnFx.transform.rotation);
            other.gameObject.SetActive(false);
            UIController.instance.UpdateBombCounter();
        }

        if (other.gameObject.CompareTag("failItem"))
        {
            GameController.Instance.CheckLevelFailForDirtyItemAddition();
            if(PreparingPot.instance)
            PreparingPot.instance.cookedSlices.Add(other.gameObject);
            UIController.instance.UpdateFailItemCounter();
        }
    }
}
