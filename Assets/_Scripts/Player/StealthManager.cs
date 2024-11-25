using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StealthManager : MonoBehaviour
{
    private bool isInvisible;
    public bool IsInvisible => isInvisible;

    public Action<bool> OnStealthStateChanged;

    private void SetInvisible(bool value)
    {
        if (isInvisible != value)
        {
            isInvisible = value;
            
            OnStealthStateChanged?.Invoke(isInvisible);
        }
    }

    public void EnableStealth()
    {
        SetInvisible(true);
    }

    public void DisableStealth()
    {
        SetInvisible(false);
    }

    public void EnableStealth(float duration)
    {
        SetInvisible(true);

        // 隐身一段时间后恢复
        Invoke(nameof(DisableStealth), duration);
    }
}

