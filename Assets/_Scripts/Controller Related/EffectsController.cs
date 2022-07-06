using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public static EffectsController instance;
    
    public GameObject spawnFx;
    public GameObject bombSpawnFx;
    public GameObject explosionFx;
    public GameObject starExplosion;
    public GameObject emojiExplosion;

    private void Awake()
    {
        instance = this;
    }
}
