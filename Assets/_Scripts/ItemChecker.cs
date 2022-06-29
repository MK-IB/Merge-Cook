using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            UIController.instance.UpdateCookStatus();
            other.GetComponent<TrailRenderer>().enabled = false;
        }
    }
}
