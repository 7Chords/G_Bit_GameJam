using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//任务进度信息
[Serializable]
public class MissionProgressInfo
{
    public int missionId;//任务id
    public int missionStage;//任务阶段
    public bool unlock;//任务是否解锁 解锁了才能接收
    public bool receive;//任务是否接收 接收了才能进行任务

    public MissionProgressInfo(int missionId, int missionStage, bool unlock, bool receive)
    {
        this.missionId = missionId;
        this.missionStage = missionStage;
        this.unlock = unlock;
        this.receive = receive;
    }
}
public class MissionManager : Singleton<MissionManager>
{
    public MissionListSO missionListSO;

    public List<MissionProgressInfo> missionProgressInfoList;

    private void Start()
    {
        missionProgressInfoList = new List<MissionProgressInfo>();

        foreach(var missionData in missionListSO.MissionList)
        {
            missionProgressInfoList.Add(new MissionProgressInfo(missionData.MissionId, 0, true,false));
        }
    }

    



}
