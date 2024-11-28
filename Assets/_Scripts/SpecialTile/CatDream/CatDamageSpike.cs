using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatDamageSpike : MonoBehaviour, IEnterTileSpecial
{
    public CatDamageSpike _anotherSpikeTile;
    public void Apply()
    {
        if(enabled)
        {
            Debug.Log("Player Dead!");
            PlayerController.Instance.Dead();
        }
    }

    private void OnEnable()
    {
        EventManager.OnPlayerMove += ChangeSpikeTile;

        EventManager.OnPlayerLoadData += DisableSelf;
    }

    private void DisableSelf()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerMove -= ChangeSpikeTile;

        EventManager.OnPlayerLoadData -= DisableSelf;

    }

    private void ChangeSpikeTile()
    {
        _anotherSpikeTile.enabled = true;

        this.enabled = false;


    }
}
