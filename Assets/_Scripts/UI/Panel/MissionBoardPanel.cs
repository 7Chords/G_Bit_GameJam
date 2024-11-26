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

    private List<Button> _selectBtnList;

    private int _currentSlectMissionId;

    private void Start()
    {

        ClosePanelBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel(panelName);
        });

        StartMissionBtn.onClick.AddListener(() =>
        {

        });
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);


        InitSelectBtns();
    }

    private void InitSelectBtns()
    {
        _selectBtnList = new List<Button>();

        foreach (var mission in MissionManager.Instance.missionProgressInfoList)
        {
            if(mission.receive)
            {
                GameObject selectBtnGO = Instantiate(Resources.Load<GameObject>("UI/MissionSelectBtn"), SlectBtnsRoot);
                _selectBtnList.Add(selectBtnGO.GetComponent<Button>());
            }
        }

        //for(int  i=0;i<_selectBtnList.Count;i++)
        //{
        //    _selectBtnList[i].onClick.AddListener(() =>
        //    {
        //        Debug.Log(i);
        //        MissionDisplay(MissionManager.Instance.missionProgressInfoList[i]);
        //    });
        //}
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

    void MissionDisplay(MissionProgressInfo info)
    {
        MissionInformation infomation = MissionManager.Instance.missionListSO.MissionList.Find(x=>x.MissionId==info.missionId);
        if (info != null)
        {
            MissionName.text = infomation.MissionName;
            ClientName.text = infomation.ClientName;
            MissionTime.text = infomation.TimeLimit;
            MissionDetail.text = infomation.MissionDetail;
        }
    }
    
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
