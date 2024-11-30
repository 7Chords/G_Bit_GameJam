using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���������Ϣ
[Serializable]
public class MissionProgress
{
    public MissionInformation missionInfo;
    public int missionStage; // ����׶�
    public bool unlock;      // �����Ƿ�����������˲��ܽ���
    public bool receive;     // �����Ƿ���գ������˲��ܽ�������
    public bool finish;      // �����Ƿ����
    public bool answer;      //�����Ƿ�ظ�

    public MissionProgress(MissionInformation missionInfo, int missionStage, bool unlock, bool receive, bool finish, bool answer)
    {
        this.missionInfo = missionInfo;
        this.missionStage = missionStage;
        this.unlock = unlock;
        this.receive = receive;
        this.finish = finish;
        this.answer = answer;
    }
}

public class MissionManager : SingletonPersistent<MissionManager>
{
    public MissionListSO missionListSO;

    public List<MissionProgress> missionProgressList;

    private void Start()
    {
        missionProgressList = new List<MissionProgress>();
        
        LoadMissionProgress();
        
        LoadMissionProgress();
    }

    public void LoadDefaultMission()
    {
        if (missionProgressList.Count == 0)
        {
            foreach (var missionData in missionListSO.MissionList)
            {
                missionProgressList.Add(new MissionProgress(missionData, 0, true, false, false,false));
            }
        }
    }

    public MissionInformation GetMissionInfoById(int id)
    {
        return missionListSO.MissionList.Find(x => x.MissionId == id);
    }

    public void SetMissionReceive(int missionId, bool receive)
    {
        MissionProgress progress = missionProgressList.Find(x => x.missionInfo.MissionId == missionId);

        if (progress != null)
        {
            progress.receive = receive;
            SaveMissionProgress();
        }
    }

    public void SetMissionFinish(int missionId, bool finish)
    {
        MissionProgress progress = missionProgressList.Find(x => x.missionInfo.MissionId == missionId);

        if (progress != null)
        {
            progress.finish = finish;
            SaveMissionProgress();
        }
    }

    /// <summary>
    /// ����������ȵ��û�ƫ��
    /// </summary>
    public void SaveMissionProgress()
    {
        List<string> missionDataList = new List<string>();

        foreach (var progress in missionProgressList)
        {
            string data = JsonUtility.ToJson(new MissionProgress(
                progress.missionInfo, 
                progress.missionStage, 
                progress.unlock, 
                progress.receive, 
                progress.finish,
                progress.answer
            ));
            missionDataList.Add(data);
        }

        PlayerPrefs.SetString("MissionProgress", string.Join("|", missionDataList));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ���û�ƫ�ü����������
    /// </summary>
    public void LoadMissionProgress()
    {
        missionProgressList.Clear();

        if (PlayerPrefs.HasKey("MissionProgress"))
        {
            string savedData = PlayerPrefs.GetString("MissionProgress");
            string[] missionDataArray = savedData.Split('|');

            foreach (var data in missionDataArray)
            {
                MissionProgress progress = JsonUtility.FromJson<MissionProgress>(data);
                
                MissionInformation missionInfo = GetMissionInfoById(progress.missionInfo.MissionId);
                progress.missionInfo = missionInfo;

                missionProgressList.Add(progress);
            }
        }
    }
}
