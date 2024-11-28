using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialoguePanel: BasePanel
{

    #region 对话组件

    private Image _characterImage;

    private Text _characterNameText;

    private Text _contentText;

    #endregion

    public SelectPanel selectPanel;

    private DialogueBlock _block;//SO

    private int _index;

    private bool _readyToEnd;

    private bool _waitingForSelect;

    private bool _hasRegistedGameStartCalling;
    
    private bool _isTyping; // 是否正在播放打字机效果

    private Tween _typingTween; // DOTween 打字机动画

    protected override void Awake()
    {
        base.Awake();

        _characterNameText = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();

        _contentText = transform.GetChild(1).GetChild(1).GetComponent<Text>();

        _characterImage = transform.GetChild(0).GetComponent<Image>();

    }

    private void Start()
    {
        _index = 0;

    }

    public void StartDialogue(DialogueBlock block)
    {
        _block = block;
        RefreshDialogue();
    }

    private void NextDialogue()
    {
        if (_block.Cells[_index+1].CellType==CellType.Select)
        {
            _waitingForSelect = true;
            selectPanel.gameObject.SetActive(true);
            int tempIndex = _index + 1;
            while (_block.Cells[tempIndex].CellType == CellType.Select)
            {
                selectPanel.AddCell(_block.Cells[tempIndex], this);
                tempIndex++;
            }
        }
        else
        {
            _index++;
        }

        if(_block.Cells[_index].CellFlag == CellFlag.End)
        {
            _readyToEnd = true;
        }
    }

    private void RefreshDialogue()
    {
        _characterImage.sprite = _block.Cells[_index].CharacterSprite;
        _characterNameText.text = _block.Cells[_index].CharacterName;
        
        _typingTween?.Kill();
        _isTyping = true;
        
        _contentText.text = "";
        
        _typingTween = _contentText.DOText(
            _block.Cells[_index].Content,
            _block.Cells[_index].Content.Length * 0.05f
        ).SetEase(Ease.Linear).OnComplete(() =>
        {
            _isTyping = false;
        });
    }

    public void OnPointerDown()
    {
        if (_waitingForSelect)
        {
            return;
        }

        // 如果对话已经显示完毕，进入下一段
        if (!_isTyping)
        {
            if (_readyToEnd)
            {
                UIManager.Instance.ClosePanel(panelName);
                if (_hasRegistedGameStartCalling)
                {
                    EventManager.OnGameStarted?.Invoke();
                }
                return;
            }

            NextDialogue();
            RefreshDialogue();
        }
        else
        {
            // 如果正在打字，点击时直接完成打字动画并显示完整文本
            _typingTween.Complete();
        }
    }

    public void SetSelectDialogue(int jumpToIndex)
    {
        _waitingForSelect = false;

        _index = jumpToIndex -1 ;

        NextDialogue();

        RefreshDialogue();

        selectPanel.gameObject.SetActive(false);
    }
    
    public void RegistGameStartCalling()
    {
        _hasRegistedGameStartCalling = true;
    }
}
