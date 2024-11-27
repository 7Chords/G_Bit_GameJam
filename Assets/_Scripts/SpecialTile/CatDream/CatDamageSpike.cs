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
    }
    private void OnDisable()
    {
        EventManager.OnPlayerMove -= ChangeSpikeTile;
    }

    private void ChangeSpikeTile()
    {
        _anotherSpikeTile.enabled = true;

        this.enabled = false;


    }
}
