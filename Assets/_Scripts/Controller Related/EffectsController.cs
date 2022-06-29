using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public static EffectsController instance;
    
    public GameObject spawnEffect;

    private void Awake()
    {
        instance = this;
    }
}
