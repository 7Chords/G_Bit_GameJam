using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ڴ浵��Ƭ
public class LevelSaveTile : MonoBehaviour, IEnterTileSpecial
{
    public void Apply()
    {
        GameManager.Instance.SavePlayerData(GetComponent<LogicTile>());
    }
}
