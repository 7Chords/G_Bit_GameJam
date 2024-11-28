using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatUnlockTileSpike : MonoBehaviour, IEnterTileSpecial
{
    public CatDamageSpike spikeTile;
    public void Apply()
    {
        spikeTile.enabled = true;
    }
}
