using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRecordTrace : MonoBehaviour, IEnterTileSpecial
{
    public void Apply()
    {
        if (!PlayerController.Instance.IsMoving)
            PlayerController.Instance.SetRecordingPath(true);//��ʼ��¼èè�߹���·��
    }
}
