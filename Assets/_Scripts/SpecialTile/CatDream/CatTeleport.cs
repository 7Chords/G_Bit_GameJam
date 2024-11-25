using DG.Tweening;
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

        Sequence s = DOTween.Sequence();

        s.Append(_player.transform.DOScale(0, 0.5f).OnComplete(()=>{
            _player.transform.position = anotherTeleport.transform.position;
        }).OnStart(() =>
        {
            _player.CancelWalkableTileVisualization();
        }));

        s.Append(_player.transform.DOScale(0.2f, 0.5f).OnComplete(()=>{
            _player.currentStandTile = anotherTeleport.GetComponent<LogicTile>();
            _player.ActivateWalkableTileVisualization();
        }));



    }
}
