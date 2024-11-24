using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CatTeleport : MonoBehaviour, IEnterTileSpecial
{
    public CatTeleport anotherTeleport;

    private PlayerController _player;
    public void Apply()
    {
        _player = FindObjectOfType<PlayerController>();

        _player.CancelWalkableTileVisualization();
        _player.transform.position = anotherTeleport.transform.position;

        _player.currentStandTile = anotherTeleport.GetComponent<LogicTile>();
        _player.ActivateWalkableTileVisualization();


    }
}
