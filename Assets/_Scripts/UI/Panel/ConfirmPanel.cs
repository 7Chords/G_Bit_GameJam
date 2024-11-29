using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel :  BasePanel
{
    [SerializeField]
    private Button ClosePanelBtn, ConfirmBtn;

    private int _currentSlectMissionId;
    
    private void Start()
    {
        ClosePanelBtn.onClick.AddListener((() =>
        {
            UIManager.Instance.ClosePanel(panelName);
        }));
        
        ConfirmBtn.onClick.AddListener((() =>
        {
            ClearSpecificPlayerPref("MissionProgress");
            UIManager.Instance.ClosePanel(panelName);
        }));
    }

    void ClearSpecificPlayerPref(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"Player preference for key '{key}' has been cleared.");
        }
        
        MissionManager.Instance.LoadMissionProgress();
        
        MissionManager.Instance.LoadDefaultMission();
    }
}
