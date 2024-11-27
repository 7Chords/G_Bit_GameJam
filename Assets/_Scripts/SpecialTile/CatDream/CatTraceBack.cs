using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTraceBack : MonoBehaviour, IEnterTileSpecial
{


    public void Apply()
    {
        PlayerController.Instance.SetRecordingPath(false);
    }
}
