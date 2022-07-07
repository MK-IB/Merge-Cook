using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemHolder : MonoBehaviour
{
    public static ItemHolder instance;

    public List<GameObject> vegList;
    public List<Sprite> ItemsPerLevelUnlock;
    public List<string> dishPerLevel;

    private void Awake()
    {
        instance = this;
        dishPerLevel = new List<string>()
        {
            "VEG. CURRY", "VEG. CURRY", "SALAD"
        };
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            Instantiate(vegList[Random.Range(0, vegList.Count)], new Vector3(0, 0, -1), Quaternion.identity);
        }*/
    }

    public void SpawnItems(int index, Vector3 pos)
    {
        GameObject veg = Instantiate(vegList[index], pos, vegList[index].transform.rotation);
        UIController.instance.UpdateCookStatus();
        
        if(veg.GetComponent<Item>())
        {
            GameObject spawnFx = EffectsController.instance.spawnFx;
            Instantiate(spawnFx, pos, spawnFx.transform.rotation);
        }else Instantiate(EffectsController.instance.bombSpawnFx, pos, Quaternion.identity);
       
        SoundController.instance.PlayClip(SoundController.instance.spawn);
        FeedbackText.instance.PlayTextEffect();
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        SoundController.instance.StartCoroutine(SoundController.instance.PlayGridSpawnPops());
    }

    public string GetCurrentDishName()
    {
        int dishPerLevelCounter = PlayerPrefs.GetInt("DishPerLevelCounter", 0);
        PlayerPrefs.SetInt("ItemPerLevelCounter", ++dishPerLevelCounter);
        return dishPerLevel[dishPerLevelCounter];
    }
    public Sprite GetCurrentItemInLevel()
    {
        int itemPerLevelCounter = PlayerPrefs.GetInt("ItemPerLevelCounter", 0);
        PlayerPrefs.SetInt("ItemPerLevelCounter", ++itemPerLevelCounter);
        return ItemsPerLevelUnlock[itemPerLevelCounter];
    }
    
}
