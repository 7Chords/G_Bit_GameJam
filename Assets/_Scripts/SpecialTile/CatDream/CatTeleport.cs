using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CatTeleport : MonoBehaviour, IEnterTileSpecial
{
    public CatTeleport anotherTeleport;

    private PlayerController _player;

    private LogicTile _startTile;
    public void Apply()
    {
        _player = FindObjectOfType<PlayerController>();

        _startTile = _player.currentStandTile;

        Sequence s = DOTween.Sequence();

        s.Append(_player.transform.DOScale(0, 0.5f).OnComplete(()=>{
            _player.transform.position = anotherTeleport.transform.position;
        }).OnStart(() =>
        {
            _player.CancelWalkableTileVisualization();
        }));

        s.Append(_player.transform.DOScale(0.2f, 0.3f).OnComplete(()=>{
            if(_startTile == _player.currentStandTile)//防止和回溯方块起冲突
            {
                _player.currentStandTile = anotherTeleport.GetComponent<LogicTile>();
                _player.ActivateWalkableTileVisualization();
            }
        }));



    }
}
