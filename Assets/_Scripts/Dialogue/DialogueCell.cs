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
    Standard,//��׼��һ��Ի�
    Select,//�þ�Ի���һ�����Ҫѡ���ѡ��
}

public enum CellFlag
{
    Begin,
    End,
    None,
}
