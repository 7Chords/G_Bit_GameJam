using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//¾ÖÄÚ´æµµÍßÆ¬
public class LevelSaveTile : MonoBehaviour, IEnterTileSpecial
{
    public void Apply()
    {
        GameManager.Instance.SavePlayerData(GetComponent<LogicTile>());
    }
}
