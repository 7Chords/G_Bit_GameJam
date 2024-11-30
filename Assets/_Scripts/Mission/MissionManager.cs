using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 任务进度信息
[Serializable]
public class MissionProgress
{
    public MissionInformation missionInfo;
    public int missionStage; // 任务阶段
    public bool unlock;      // 任务是否解锁，解锁了才能接收
    public bool receive;     // 任务是否接收，接收了才能进行任务
    public bool finish;      // 任务是否完成
    public bool answer;      //任务是否回复

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
    /// 保存任务进度到用户偏好
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
    /// 从用户偏好加载任务进度
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
