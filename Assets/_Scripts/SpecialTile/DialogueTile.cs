using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//触发对话的特殊瓦片
public class DialogueTile : MonoBehaviour, IEnterTileSpecial
{
    public DialogueBlock dialogueBlock;
    public void Apply()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<PlayerController>() != null)
            {
                if (enabled)
                {
                    DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
                    dialoguePanel.StartDialogue(dialogueBlock);
                }
            
            }
        }

    }
}
