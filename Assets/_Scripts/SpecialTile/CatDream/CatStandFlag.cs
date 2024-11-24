using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatStandFlag : MonoBehaviour,IEnterTileSpecial
{
    public static int _getFlagAmount;

    private static bool _allFlagTake;

    private int _flagAmount;

    private SpriteRenderer _sp;
    private void Start()
    {
        _flagAmount = FindObjectsOfType<CatStandFlag>().Length;

        _sp = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    public void Apply()
    {
        _getFlagAmount++;

        _sp.color = Color.yellow;

        if (_getFlagAmount == _flagAmount)
        {
            _allFlagTake = true;
        }
        Debug.Log(_allFlagTake);
    }
}
