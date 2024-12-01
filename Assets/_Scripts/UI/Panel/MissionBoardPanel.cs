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
    [SerializeField]
    private Text missionOperatorText;

    private void Start()
    {
        _currentSlectMissionId = -1;

        ClosePanelBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel(panelName);
        });

        StartMissionBtn.onClick.AddListener(() =>
        {
            MissionProgress progress = MissionManager.Instance.missionProgressList[_currentSlectMissionId];

            if(!progress.finish)
            {
                UIManager.Instance.ClosePanel(panelName);
                SceneLoader.Instance.LoadScene(progress.missionInfo.MissionLevelName, "入睡中......");
            }
            else
            {
                UIManager.Instance.ClosePanel("MissionBoardPanel");
                DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
                dialoguePanel.StartDialogue(Resources.Load<DialogueBlock>("Data/Dialogue/" + progress.missionInfo.MissionName + "_" + "交付委托"));
                
                MissionManager.Instance.SetMissionAnswer(_currentSlectMissionId,true);
            }
        });
    }

    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);


        InitSelectBtns();
    }
    
    private void InitSelectBtns()
    {
        foreach (Transform child in SlectBtnsRoot)
        {
            Destroy(child.gameObject);
        }

        bool hasAvailableMissions = false;
        
        HideMissionDetails();
        
        foreach (var missionProgress in MissionManager.Instance.missionProgressList)
        {
            if (missionProgress.receive && !missionProgress.answer)
            {
                hasAvailableMissions = true;
                GameObject selectBtnGO = Instantiate(Resources.Load<GameObject>("UI/MissionSelectBtn"), SlectBtnsRoot);
                selectBtnGO.GetComponent<MissionSelectBtn>().SetMissionProgress(missionProgress, this);
            }
        }
    }
    
    private void HideMissionDetails()
    {
        MissionName.text = "";
        ClientName.text = "";
        MissionTime.text = "";
        MissionDetail.text = "";

        StartMissionBtn.interactable = false;
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

            StartMissionBtn.interactable = true;

            if(progress.finish)
            {
                missionOperatorText.text = "回复委托";
            }
            else
            {
                missionOperatorText.text = "开始委托";

            }
        }
        else
        {
            HideMissionDetails();
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
