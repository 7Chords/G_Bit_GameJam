using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBlockTile : MonoBehaviour,IEnterTileSpecial
{
    private LogicTile _ownerLogicTile;
    private void Awake()
    {
        _ownerLogicTile = GetComponent<LogicTile>();
    }

    private void Start()
    {
        _ownerLogicTile.SetLogicWalkable(false);
    }
    public void Apply()
    {
        
    }




}
