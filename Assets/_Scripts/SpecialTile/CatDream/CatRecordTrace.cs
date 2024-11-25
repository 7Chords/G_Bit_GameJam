using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRecordTrace : MonoBehaviour, IEnterTileSpecial
{
    private PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }
    public void Apply()
    {
        if (!_player.IsMoving)
            _player.SetRecordingPath(true);//开始记录猫猫走过的路径
    }
}
