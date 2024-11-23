using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCell
{
    public int Index;

    public string CharacterName;

    public Sprite CharacterSprite;

    public CellType CellType;

    public int JumpToIndex;

    public CellFlag CellFlag;

    [TextArea(3,5)]
    public string Content;

}

public enum CellType
{
    Standard,//标准的一句对话
    Select,//该句对话是一句玩家要选择的选项
}

public enum CellFlag
{
    Begin,
    End,
    None,
}
