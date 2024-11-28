using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//局内存档瓦片
public class LevelSaveTile : MonoBehaviour, IEnterTileSpecial
{
    public int minNeedStep;//当前该存档点到通关至少还需要的步数（可以宽裕些）
    public void Apply()
    {
        GameManager.Instance.SavePlayerData(GetComponent<LogicTile>(), minNeedStep);
    }
}
