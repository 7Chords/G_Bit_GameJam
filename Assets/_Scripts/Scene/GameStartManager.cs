using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public DialogueBlock block;
    private const string FirstTimeKey = "IsFirstTime";

    void Start()
    {
        MissionManager.Instance.UpdateSouvenirDisplay();
        CheckFirstTime();
    }
    
    public void CheckFirstTime()
    {
        if (!PlayerPrefs.HasKey(FirstTimeKey))
        {
            DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
            dialoguePanel.StartDialogue(block);
            
            PlayerPrefs.SetInt(FirstTimeKey, 0);
            PlayerPrefs.Save();
        }
    }
}
