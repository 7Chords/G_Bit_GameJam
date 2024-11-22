using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    public string audioName;

    public string audioPath;

}


[ExcelAsset]
public class AudioDatas : ScriptableObject
{
    public List<AudioData> audioDataList;

}
