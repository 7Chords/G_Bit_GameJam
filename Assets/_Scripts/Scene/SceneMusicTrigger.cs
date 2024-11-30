using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    public string BGMName;
    private void Start()
    {
        AudioManager.Instance.PlayBgm(BGMName);
    }
}
