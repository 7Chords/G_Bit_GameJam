using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBoom : MonoBehaviour, IEnterTileSpecial
{
    private PlayerController _player;
    public void Apply()
    {
        _player = GetComponent<PlayerController>();
        //dead?
    }
}
