using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialoguePanel: BasePanel,IPointerDownHandler
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


    protected override void Awake()
    {
        base.Awake();

        _characterImage = transform.GetChild(0).GetComponent<Image>();

        _characterNameText = transform.GetChild(1).GetComponent<Text>();

        _contentText = transform.GetChild(2).GetComponent<Text>();

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
        if(!_contentText.text.Equals(_block.Cells[_index].Content))
        {
            _contentText.text = "";
            _contentText.DOText(_block.Cells[_index].Content, _block.Cells[_index].Content.Length * 0.05f);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_waitingForSelect)
        {
            return;
        }

        if(_readyToEnd)
        {
            UIManager.Instance.ClosePanel(panelName);
            return;
        }

        NextDialogue();
        RefreshDialogue();
    }

    public void SetSelectDialogue(int jumpToIndex)
    {
        _waitingForSelect = false;

        _index = jumpToIndex -1 ;

        NextDialogue();

        RefreshDialogue();

        selectPanel.gameObject.SetActive(false);
    }
}
