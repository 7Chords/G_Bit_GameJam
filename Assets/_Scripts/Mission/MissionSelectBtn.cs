using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectBtn : MonoBehaviour
{
    private Text _missionText;

    private Button _btn;

    private MissionBoardPanel _missionBoardPanel;

    private MissionProgress _progress;

    private void Awake()
    {
        _btn = GetComponent<Button>();

        _missionText = transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(() =>
        {
            _missionBoardPanel.MissionDisplay(_progress);
        });
    }

    public void SetMissionProgress(MissionProgress progress, MissionBoardPanel _panel)
    {
        _progress = progress;

        _missionBoardPanel = _panel;

        _missionText.text = _progress.missionInfo.MissionName;
    }


}
