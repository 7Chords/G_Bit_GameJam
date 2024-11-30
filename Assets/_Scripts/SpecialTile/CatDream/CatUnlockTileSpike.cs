using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CatUnlockTileSpike : MonoBehaviour, IEnterTileSpecial
{
    public List<CatDamageSpike> spikeTiles;
    public void Apply()
    {
        foreach (var spikeTile in spikeTiles)
        {
            spikeTile.enabled = true;
        }
    }
}
