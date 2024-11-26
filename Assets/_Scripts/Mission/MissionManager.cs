using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���������Ϣ
[Serializable]
public class MissionProgressInfo
{
    public int missionId;//����id
    public int missionStage;//����׶�
    public bool unlock;//�����Ƿ���� �����˲��ܽ���
    public bool receive;//�����Ƿ���� �����˲��ܽ�������

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
