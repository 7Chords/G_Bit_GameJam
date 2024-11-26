using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEnterTile : MonoBehaviour, IEnterTileSpecial, IExitTileSpecial
{
    private StealthManager stealthManager;

    private void Start()
    {
        stealthManager = FindObjectOfType<StealthManager>();
        if (stealthManager == null)
        {
            Debug.LogError("StealthManagerŒ¥’“µΩ");
        }
    }

    public void Apply()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var collider in colliders)
        {
            if(collider.GetComponent<PlayerController>())
                AudioManager.Instance.PlaySfx("grass");
                if (stealthManager != null)
                {
                    stealthManager.EnableStealth();
                }
        }
    }

    public void OnExit()
    {
        if (stealthManager != null)
        {
            stealthManager.DisableStealth();
        }
    }
}

