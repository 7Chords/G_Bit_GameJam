using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    public static Action OnPlayerMove;

    public static Action OnGameStarted;

    //��Ϸ�����¼� �ξ��Ľ��һ����ʤ�� ���˻����ľ���ص����ڵĴ浵�� �ʹ󹷹�ͨ�� ���ڶ����������� �о��ڴ浵��Ƭ
    public static Action OnGameFinished;

    public static Action OnPlayerLoadData;//��Ҿ��ڼ��ش浵 һЩ����Ҫ�ָ�ԭ��

}
