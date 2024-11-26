using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//触发对话的特殊瓦片
public class DialogueTile : MonoBehaviour, IEnterTileSpecial
{
    public DialogueBlock dialogueBlock;
    public void Apply()
    {
        UIManager.Instance.OpenPanel("DialoguePanel");
    }
}
