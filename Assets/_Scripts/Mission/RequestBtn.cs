using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RequestBtn : MonoBehaviour
{
    private Button _btn;

    private Text _requestText;

    private MissionProgress _progress;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _requestText = transform.GetChild(0).GetComponent<Text>();
    }
    private void Start()
    {
        _btn.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel("RequestBoardPanel");

            DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;

            //对话块的位置 对话框命名规则：任务名+"_"+接受委托
            DialogueBlock block = Resources.Load<DialogueBlock>("Data/Dialogue/" +
                _progress.missionInfo.MissionName + "_" + "接受委托");

            dialoguePanel.StartDialogue(block);

            MissionManager.Instance.SetMissionReceive(_progress.missionInfo.MissionId, true);
        });
    }

    public void SeMissionProgress(MissionProgress progress)
    {
        _progress = progress;

        _requestText.text = progress.missionInfo.MissionName;
    }

    //void Update()
    //{   
    //    if (id == 1)
    //    {
    //        UIManager.Instance.OpenPanel("SceneLoadedBlackPanel");
    //        UIManager.Instance.ClosePanel("SceneLoadedBlackPanel");
    //        DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
    //        if (dialoguePanel)
    //            dialoguePanel.StartDialogue(TestBlocks[0]);
    //        else
    //            id = 0;
    //    }
    //}
    //public void OnButtonClick(string arg)
    //{
    //   id = int.Parse(arg);
    //}
}
