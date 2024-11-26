using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FunTest : MonoBehaviour
{
    // Start is called before the first frame update

    public DialogueBlock TestBlock;

    public TileBase tileBase;

    public Tilemap tileMap;

    public Vector3Int changeTileCellPos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            tileMap.SetTile(changeTileCellPos, tileBase);
        }
    }
}
