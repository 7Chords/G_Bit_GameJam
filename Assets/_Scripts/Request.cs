using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Request : MonoBehaviour
{
    public List<DialogueBlock> TestBlocks;
    public MissionPanelSO MissionList;
    int id = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (id == 1)
        {
            UIManager.Instance.OpenPanel("SceneLoadedBlackPanel");
            UIManager.Instance.ClosePanel("SceneLoadedBlackPanel");
            DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
            if (dialoguePanel)
                dialoguePanel.StartDialogue(TestBlocks[0]);
            else
            {
                MissionList.MissionList.Find(i=>i.MissionId==id).unlocked=true;
                id = 0;
            }
        }
    }
    public void OnButtonClick(string arg)
    {
       id = int.Parse(arg);
    }
}
