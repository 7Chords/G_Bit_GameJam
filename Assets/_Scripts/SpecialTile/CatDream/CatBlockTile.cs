using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBlockTile : MonoBehaviour,IEnterTileSpecial
{
    private LogicTile _ownerLogicTile;

    public GameObject StoneBlockGo;
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
        //
    }

    public void MakingWalkable()
    {
        _ownerLogicTile.SetLogicWalkable(true);

        GameObject effectGO = Instantiate(Resources.Load<GameObject>("Partical/StoneBroken"), StoneBlockGo.transform.position, Quaternion.identity);

        Destroy(StoneBlockGo);

        Destroy(effectGO,0.5f);


    }






}
