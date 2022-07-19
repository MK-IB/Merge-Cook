using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour
{
    public GameObject hand, text;

    private void Start()
    {
        if (PlayerPrefs.GetInt("levelnumber", 1) == 1)
        {
            hand.SetActive(true);
            text.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
