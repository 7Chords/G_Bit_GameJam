using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int missionId;

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
        SavePlayerData(PlayerController.Instance.currentStandTile,
            PlayerController.Instance.StepManager.GetRemainingSteps());
    }

    private void OnFinishGame()
    {
        //关卡结束 ui？
        SceneLoader.Instance.LoadScene("OfficeScene","苏醒中......");
        MissionManager.Instance.SetMissionFinish(missionId, true);
        
    }

    //存储玩家局内数据 存档瓦片会调用
    public void SavePlayerData(LogicTile tile,int minNeedStep)
    {
        saveTile = tile;

        if(PlayerController.Instance.StepManager.GetRemainingSteps()<minNeedStep)
        {
            saveStepAmount = minNeedStep;
        }
        else
        {
            saveStepAmount = PlayerController.Instance.StepManager.GetRemainingSteps();
        }
    }

    //加载玩家局内数据 死亡或步数耗尽时会调用
    public void LoadPlayerData()
    {
        Instantiate(Resources.Load<GameObject>("Effect/ScaryGreen"),transform.position, Quaternion.identity);
        
        PlayerController.Instance.CancelWalkableTileVisualization();

        PlayerController.Instance.currentStandTile = saveTile;

        PlayerController.Instance.transform.position = saveTile.transform.position;

        PlayerController.Instance.StepManager.SetRemainSteps(saveStepAmount);

        PlayerController.Instance.ActivateWalkableTileVisualization();

        EventManager.OnPlayerLoadData?.Invoke();
        
        // 初始化一下玩家
        StealthManager.Instance.DisableStealth();
        
        // 初始化其他对象
        InitializeEnemies();
    }
    
    private void InitializeEnemies()
    {
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();

        foreach (BaseEnemy enemy in enemies)
        {
            enemy.SetEnemyToFlag();
        }
    }


}
