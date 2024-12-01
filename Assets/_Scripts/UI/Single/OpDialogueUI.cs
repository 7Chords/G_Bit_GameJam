using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpDialogueUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Text _interactText;

    public DialogueBlock block;

    private void Awake()
    {
        _interactText = transform.GetChild(0).GetComponent<Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("Click");
        
        DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
        dialoguePanel.StartDialogue(block);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(false);
    }
}
