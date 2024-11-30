using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTeleport : MonoBehaviour, IEnterTileSpecial
{
    public CatTeleport anotherTeleport;

    private LogicTile _startTile;
    public void Apply()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<PlayerController>() != null) ApplyTeleport();
        }
    }

    private void ApplyTeleport()
    {

        AudioManager.Instance.PlaySfx("Teleport");
        _startTile = PlayerController.Instance.currentStandTile;

        Sequence s = DOTween.Sequence();

        s.Append(PlayerController.Instance.transform.DOScale(0, 0.5f).OnComplete(()=>{
            PlayerController.Instance.transform.position = anotherTeleport.transform.position;
        }).OnStart(() =>
        {
            StealthManager.Instance.EnableStealth();
            PlayerController.Instance.CancelWalkableTileVisualization();
        }));

        s.Append(PlayerController.Instance.transform.DOScale(1f, 0.3f).OnComplete(()=>{
            if(_startTile == PlayerController.Instance.currentStandTile)//防止和回溯方块起冲突
            {
                StealthManager.Instance.DisableStealth();
                PlayerController.Instance.currentStandTile = anotherTeleport.GetComponent<LogicTile>();
                PlayerController.Instance.ActivateWalkableTileVisualization();
            }
        }));        
    }
}
