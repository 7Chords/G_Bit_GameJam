using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���������Ϣ
[Serializable]
public class MissionProgress
{
    public MissionInformation missionInfo;
    public int missionStage;//����׶�
    public bool unlock;//�����Ƿ���� �����˲��ܽ���
    public bool receive;//�����Ƿ���� �����˲��ܽ�������
    public bool finish;//�����Ƿ����

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
