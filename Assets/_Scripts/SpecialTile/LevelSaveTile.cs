using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ڴ浵��Ƭ
public class LevelSaveTile : MonoBehaviour, IEnterTileSpecial
{
    public int minNeedStep;//��ǰ�ô浵�㵽ͨ�����ٻ���Ҫ�Ĳ��������Կ�ԣЩ��
    public void Apply()
    {
        GameManager.Instance.SavePlayerData(GetComponent<LogicTile>(), minNeedStep);
    }
}
