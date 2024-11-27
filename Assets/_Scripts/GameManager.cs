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
        //��ʼ�����
        SavePlayerData(PlayerController.Instance.currentStandTile);
    }

    private void OnFinishGame()
    {
        //�ؿ����� ui��
        SceneLoader.Instance.LoadScene("OfficeScene","������......");
    }

    //�洢��Ҿ������� �浵��Ƭ�����
    public void SavePlayerData(LogicTile tile)
    {
        saveTile = tile;

        saveStepAmount = PlayerController.Instance.StepManager.GetRemainingSteps();
    }

    //������Ҿ������� ���������ľ�ʱ�����
    public void LoadPlayerData()
    {
        PlayerController.Instance.CancelWalkableTileVisualization();

        PlayerController.Instance.currentStandTile = saveTile;

        PlayerController.Instance.transform.position = saveTile.transform.position;

        PlayerController.Instance.StepManager.SetRemainSteps(saveStepAmount);

        PlayerController.Instance.ActivateWalkableTileVisualization();

    }


}
