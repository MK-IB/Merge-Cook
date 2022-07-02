using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ServedPlate : MonoBehaviour
{
    public Transform curry;

    public void MakeCurryLevel()
    {
        curry.DOLocalMoveZ(0, 3);
    }
}
