using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����Ի���������Ƭ
public class DialogueTile : MonoBehaviour, IEnterTileSpecial
{
    public DialogueBlock dialogueBlock;
    public void Apply()
    {
        UIManager.Instance.OpenPanel("DialoguePanel");
    }
}
