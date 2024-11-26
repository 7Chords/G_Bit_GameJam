using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentMission", menuName = "CustomizedSO/CurrentMission")]
public class CurrentMissionSO : ScriptableObject
{
    [Header("任务信息")]
    public int MissionId;
    public string MissionName;
    public string ClientName;
    public string TimeLimit;
    [TextArea]
    public string MissionDetail;
    public void clearData()
    {
        MissionId = 0;
        MissionName = string.Empty;
        MissionDetail = string.Empty;
        ClientName = string.Empty;
        TimeLimit = string.Empty;
        MissionDetail = string.Empty;
    }
    public void hideData()
    {
        MissionId = 0;
        MissionName = "???";
        MissionDetail = "???";
        ClientName = "???";
        TimeLimit = "???";
        MissionDetail = "???";
    }
}
