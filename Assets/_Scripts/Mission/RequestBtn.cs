using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RequestBtn : MonoBehaviour
{
    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    public List<DialogueBlock> TestBlocks;
    public MissionListSO MissionList;
    int id = 0;

    void Start()
    {
        
    }
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
                id = 0;
        }
    }
    public void OnButtonClick(string arg)
    {
       id = int.Parse(arg);
    }
}
