using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestBoardPanel : BasePanel
{
    [SerializeField]
    private Button _closePanelBtn;
    public Transform RequestBtnsRoot;
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        InitRequestBtns();
    }

    private void Start()
    {
        _closePanelBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel(panelName);
        });
    }

    public void InitRequestBtns()
    {
        if (MissionManager.Instance.missionProgressList.Count == 0)
        {
            Debug.LogWarning("MissionProgressList is not initialized.");
            return;
        }
        
        foreach (var missionProgress in MissionManager.Instance.missionProgressList)
        {
            if (missionProgress.unlock && !missionProgress.finish && !missionProgress.receive)
            {
                GameObject selectBtnGO = Instantiate(Resources.Load<GameObject>("UI/RequestBtn"), RequestBtnsRoot);

                selectBtnGO.GetComponent<RequestBtn>().SeMissionProgress(missionProgress);
            }
        }
    }
}
