using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public static ItemHolder instance;

    public List<GameObject> vegList;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnItems(int index, Vector3 pos)
    {
        GameObject veg = Instantiate(vegList[index], pos, vegList[index].transform.rotation);
        veg.transform.DOScale(Vector3.zero, 0.2f).From().OnComplete(() =>
        {
            veg.transform.DOScale(Vector3.one * 0.65f, 2);
        });
    }
    
}
