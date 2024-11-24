using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatStandFlag : MonoBehaviour,ITileSpecial
{
    public static int _getFlagAmount;

    private static bool _allFlagTake;

    private int _flagAmount;
    private void Start()
    {
        _flagAmount = FindObjectsOfType<CatStandFlag>().Length;
    }
    public void Apply()
    {
        _getFlagAmount++;

        if (_getFlagAmount == _flagAmount)
        {
            _allFlagTake = true;
        }
    }
}
