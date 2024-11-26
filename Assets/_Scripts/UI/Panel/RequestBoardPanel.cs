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
        foreach (var mission in MissionManager.Instance.missionProgressInfoList)
        {
            if (mission.unlock)
            {
                GameObject selectBtnGO = Instantiate(Resources.Load<GameObject>("UI/RequestBtn"), RequestBtnsRoot);
            }
        }
    }
}
