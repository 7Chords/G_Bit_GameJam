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
        _player.SetRecordingPath(true);//��ʼ��¼èè�߹���·��
    }
}