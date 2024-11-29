using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEnterTile : MonoBehaviour, IEnterTileSpecial, IExitTileSpecial
{
    public void Apply()
    {
        AudioManager.Instance.PlaySfx("Grass");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<PlayerController>() != null)
            {
                StealthManager.Instance.EnableStealth();                
            }

        }
    }

    public void OnExit()
    {
        StealthManager.Instance.DisableStealth();
    }
}

