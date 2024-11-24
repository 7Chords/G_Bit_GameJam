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
        if (stealthManager != null)
        {
            stealthManager.EnableStealth();
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

