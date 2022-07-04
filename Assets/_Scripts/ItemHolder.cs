using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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

    public void SpawnItems(int index, Vector3 pos)
    {
        GameObject veg = Instantiate(vegList[index], pos, vegList[index].transform.rotation);
        
        if(veg.GetComponent<Item>())
        {
            GameObject spawnFx = EffectsController.instance.spawnFx;
            Instantiate(spawnFx, pos, spawnFx.transform.rotation);
        }else Instantiate(EffectsController.instance.bombSpawnFx, pos, Quaternion.identity);
        Vector3 origScale = veg.transform.localScale;
        veg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBounce).From().OnComplete(() =>
        {
            veg.transform.DOScale(origScale * 0.65f, 2);
        });
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
