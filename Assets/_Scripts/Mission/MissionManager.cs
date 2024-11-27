using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//任务进度信息
[Serializable]
public class MissionProgress
{
    public MissionInformation missionInfo;
    public int missionStage;//任务阶段
    public bool unlock;//任务是否解锁 解锁了才能接收
    public bool receive;//任务是否接收 接收了才能进行任务
    public bool finish;//任务是否完成

    public MissionProgress(MissionInformation missionInfo, int missionStage, bool unlock, bool receive, bool finish)
    {
        this.missionInfo = missionInfo;
        this.missionStage = missionStage;
        this.unlock = unlock;
        this.receive = receive;
        this.finish = finish;
    }
}
public class MissionManager : Singleton<MissionManager>
{
    public MissionListSO missionListSO;

    public List<MissionProgress> missionProgressList;

    private void Start()
    {
        missionProgressList = new List<MissionProgress>();

        foreach(var missionData in missionListSO.MissionList)
        {
            missionProgressList.Add(new MissionProgress(missionData, 0, true,false,false));
        }
    }

    public MissionInformation GetMissionInfoById(int id)
    {
        return missionListSO.MissionList.Find(x=>x.MissionId == id);
    }

    public void SetMissionReceive(int missionId,bool receive)
    {
        MissionProgress progress = missionProgressList.Find(x => x.missionInfo.MissionId == missionId);

        if (progress != null)
        {
            progress.receive = receive;
        }
    }


    public void SetMissionFinish(int missionId, bool finish)
    {
        MissionProgress progress = missionProgressList.Find(x => x.missionInfo.MissionId == missionId);

        if (progress != null)
        {
            progress.finish = finish;
        }
    }


}
