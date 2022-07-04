using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCheckerForStackFood : MonoBehaviour
{
    public static ItemCheckerForStackFood instance;
    
    public List<Transform> stackPositions;
    public Transform foodStack;
    private Vector3 _oldPos, _newPos;
    private List<Transform> _itemPositions;
    private int _itemPosCounter;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //_oldPos = foodStack.GetChild(0).localPosition;
        //_oldPos = new Vector3(_oldPos.x, _oldPos.y + 0.5f, _oldPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            UIController.instance.UpdateCookStatus();
            other.GetComponent<Collider>().enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            
            other.transform.parent = foodStack;
            other.transform.DOLocalMove(foodStack.GetChild(_itemPosCounter++).localPosition, 0.3f);
            //_newPos = new Vector3(_oldPos.x, _oldPos.y + 0.25f, _oldPos.z);

            if(other.transform.name.Contains("Ommellete")) other.transform.eulerAngles = Vector3.right * -90;
            
            _oldPos = _newPos;
        }
    }

    public void PutTheTopBread()
    {
        GameObject topBread = foodStack.GetChild(1).gameObject;
        topBread.SetActive(true);
        topBread.transform.DOLocalMove(foodStack.GetChild(_itemPosCounter).localPosition, 0.3f);
    }
}
