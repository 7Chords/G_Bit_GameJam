using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunTest : MonoBehaviour
{
    // Start is called before the first frame update

    public DialogueBlock TestBlock;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.A))
        {
            DialoguePanel dialoguaPanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;

            dialoguaPanel.StartDialogue(TestBlock);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlayBgm("HappyDay");

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.Instance.StopBgm("HappyDay");

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySfx("Click");

        }
    }
}
