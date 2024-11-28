using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    public static Action OnPlayerMove;

    public static Action OnGameStarted;

    //游戏结束事件 梦境的结果一定是胜利 死了或步数耗尽会回到局内的存档点 和大狗沟通了 局内都是线性流程 有局内存档瓦片
    public static Action OnGameFinished;

    public static Action OnPlayerLoadData;//玩家局内加载存档 一些机关要恢复原样

}
