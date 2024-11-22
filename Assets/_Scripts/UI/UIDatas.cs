using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData
{
    public string uiName;

    public string uiPath;

}


[ExcelAsset]
public class UIDatas : ScriptableObject
{
    public List<UIData> uiDataList;

}
