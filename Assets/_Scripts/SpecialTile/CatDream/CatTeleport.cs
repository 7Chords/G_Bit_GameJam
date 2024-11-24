using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTeleport : MonoBehaviour, ITileSpecial
{
    public CatTeleport anotherTeleport;

    private PlayerController _player;
    public void Apply()
    {
        _player = FindObjectOfType<PlayerController>();

        _player.transform.position = anotherTeleport.transform.position;

        _player.currentStandTile = anotherTeleport.GetComponent<LogicTile>();

    }
}
