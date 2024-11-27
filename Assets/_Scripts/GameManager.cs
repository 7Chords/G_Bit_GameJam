using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool gameStarted;

    public DialogueBlock levelEntryBlock;

    public int saveStepAmount;

    public LogicTile saveTile;

    private void Start()
    {
        StartGameEntryDialogue();
    }
    private void OnEnable()
    {
        EventManager.OnGameStarted += OnGameStart;
        EventManager.OnGameFinished += OnFinishGame;
    }

    private void OnDisable()
    {
        EventManager.OnGameStarted -= OnGameStart;
        EventManager.OnGameFinished -= OnFinishGame;
    }
    private void StartGameEntryDialogue()
    {
        DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
        dialoguePanel.StartDialogue(levelEntryBlock);
        dialoguePanel.RegistGameStartCalling();

    }

    private void OnGameStart()
    {
        //开始存个档
        SavePlayerData(PlayerController.Instance.currentStandTile);
    }

    private void OnFinishGame()
    {
        //关卡结束 ui？
        SceneLoader.Instance.LoadScene("OfficeScene","苏醒中......");
    }

    //存储玩家局内数据 存档瓦片会调用
    public void SavePlayerData(LogicTile tile)
    {
        saveTile = tile;

        saveStepAmount = PlayerController.Instance.StepManager.GetRemainingSteps();
    }

    //加载玩家局内数据 死亡或步数耗尽时会调用
    public void LoadPlayerData()
    {
        PlayerController.Instance.CancelWalkableTileVisualization();

        PlayerController.Instance.currentStandTile = saveTile;

        PlayerController.Instance.transform.position = saveTile.transform.position;

        PlayerController.Instance.StepManager.SetRemainSteps(saveStepAmount);

        PlayerController.Instance.ActivateWalkableTileVisualization();

    }


}
