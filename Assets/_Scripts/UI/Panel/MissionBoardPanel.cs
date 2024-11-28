using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBoardPanel : BasePanel
{
    
    [SerializeField]
    private Text MissionName,ClientName, MissionTime, MissionDetail;
    [SerializeField]
    private Button ClosePanelBtn, StartMissionBtn;
    [SerializeField]
    private Transform SlectBtnsRoot;

    private int _currentSlectMissionId;

    private void Start()
    {

        ClosePanelBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel(panelName);
        });

        StartMissionBtn.onClick.AddListener(() =>
        {
            SceneLoader.Instance.LoadScene(MissionManager.Instance.missionListSO.MissionList[_currentSlectMissionId].MissionLevelName,"ÈëË¯ÖÐ......");
        });
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);


        InitSelectBtns();
    }

    private void InitSelectBtns()
    {

        foreach (var missionProgress in MissionManager.Instance.missionProgressList)
        {
            if(missionProgress.receive && !missionProgress.finish)
            {
                GameObject selectBtnGO = Instantiate(Resources.Load<GameObject>("UI/MissionSelectBtn"), SlectBtnsRoot);

                selectBtnGO.GetComponent<MissionSelectBtn>().SetMissionProgress(missionProgress,this);
            }
        }

    }
    public void MissionDisplay(MissionProgress progress)
    {

        if (progress != null)
        {
            _currentSlectMissionId = progress.missionInfo.MissionId;
            MissionName.text = progress.missionInfo.MissionName;
            ClientName.text = progress.missionInfo.ClientName;
            MissionTime.text = progress.missionInfo.TimeLimit;
            MissionDetail.text = progress.missionInfo.MissionDetail;
        }
    }


    //public void Set

    //void Update()
    //{
    //    CopyMission(MissionList, CurrentMission, id);
    //    MissionDisplay(CurrentMission);
    //}

    //public MissionInformation GetMissionInformation(int id)
    //{
    //    return MissionList.MissionList.Find(i=>i.MissionId == id);
    //}

    //void CopyMission(MissionListSO sourse,CurrentMissionSO currentMission, int id)
    //{
    //    MissionInformation copyMission = sourse.MissionList.Find(mission=>mission.MissionId ==id);
    //    if (copyMission != null)
    //    {
    //        if (unlock[id-1] == true)
    //        {
    //            currentMission.MissionId = copyMission.MissionId;
    //            currentMission.MissionName = copyMission.MissionName;
    //            currentMission.ClientName = copyMission.ClientName;
    //            currentMission.TimeLimit = copyMission.TimeLimit;
    //            currentMission.MissionDetail = copyMission.MissionDetail;
    //        }
    //        else
    //            currentMission.hideData();
    //    }
    //    else
    //    {
    //        currentMission.clearData();
    //    }
    //}


    //public void OnButtonClick(string arg)
    //{
    //    id = int.Parse(arg);
    //    if (id>=10)
    //    {
    //        unlock[id-10] = true;
    //    }

    //    Debug.Log(id-10);
    //}
}
