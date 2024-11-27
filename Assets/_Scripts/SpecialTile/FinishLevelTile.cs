using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelTile : MonoBehaviour, IEnterTileSpecial
{
    public void Apply()
    {
        EventManager.OnGameFinished?.Invoke();
    }
}
