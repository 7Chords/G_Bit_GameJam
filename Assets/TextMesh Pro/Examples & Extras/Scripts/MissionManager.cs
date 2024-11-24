using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    
    public CurrentMissionSO CurrentMission;
    public MissionPanelSO MissionList;
    int id;


    [SerializeField]
    Text MissionName,ClientName, MissionTime, MissionDetail;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        copyMission(MissionList, CurrentMission, id);
        MissionDisplay(CurrentMission);
    }
    public MissionInformation GetMissionInformation(int id)
    {
        return MissionList.MissionList.Find(i=>i.MissionId == id);
    }
    void copyMission(MissionPanelSO sourse,CurrentMissionSO currentMission, int id)
    {
        MissionInformation copyMission = sourse.MissionList.Find(mission=>mission.MissionId ==id);
        if (copyMission != null)
        {
            currentMission.MissionId = copyMission.MissionId;
            currentMission.MissionName = copyMission.MissionName;
            currentMission.ClientName = copyMission.ClientName;
            currentMission.TimeLimit = copyMission.TimeLimit;
            currentMission.MissionDetail = copyMission.MissionDetail;
        }
        else
        {
            currentMission.clearData();
        }
    }
    void MissionDisplay(CurrentMissionSO currentMission)
    {
        if (currentMission != null)
        {
            MissionName.text = currentMission.MissionName;
            ClientName.text = currentMission.ClientName;
            MissionTime.text = currentMission.TimeLimit;
            MissionDetail.text = currentMission.MissionDetail;
        }
    }
    public void OnButtonClick(string arg)
    {
        id = int.Parse(arg);
        Debug.Log(id);
    }
}
