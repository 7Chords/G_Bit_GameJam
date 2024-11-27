using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameStarted;
    public DialogueBlock levelEntryBlock;

    private void Start()
    {
        StartGameEntryDialogue();
    }
    private void OnEnable()
    {

        EventManager.OnGameFinished += FinishGame;
    }

    private void OnDisable()
    {
        EventManager.OnGameFinished -= FinishGame;

    }
    private void StartGameEntryDialogue()
    {
        DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
        dialoguePanel.StartDialogue(levelEntryBlock);
        dialoguePanel.RegistGameStartCalling();

    }

    private void FinishGame()
    {
        //¹Ø¿¨½áÊø ui£¿
        SceneLoader.Instance.LoadScene("OfficeScene","ËÕÐÑÖÐ......");
    }


}
