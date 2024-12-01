using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[Serializable]
public class SouvenirData
{
    public int missionId;
    public Button souvenirButton;
}

[System.Serializable]
public class SouvnirManager : MonoBehaviour
{
    [SerializeField] private Transform buttonParent;

    [SerializeField]
    private List<SouvenirData> souvenirDataList = new List<SouvenirData>();
    
    public void ShowSouvenirButton(int missionId)
    {
        var souvenirData = souvenirDataList.Find(data => data.missionId == missionId);
        if (souvenirData != null && souvenirData.souvenirButton != null)
        {
            souvenirData.souvenirButton.gameObject.SetActive(true);
        }
    }
    
    public void HideAllSouvenirButtons()
    {
        foreach (var souvenirData in souvenirDataList)
        {
            if (souvenirData.souvenirButton != null)
            {
                souvenirData.souvenirButton.gameObject.SetActive(false);
            }
        }
    }
}
