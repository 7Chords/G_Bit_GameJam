using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonPersistent<UIManager>
{
    //<面板名,面板预制体路径>
    private Dictionary<string, string> _panelPathDict;
    //预制件缓存字典，<面板名,面板预制体>
    private Dictionary<string, GameObject> _uiPrefabDict;
    //当前已打开界面字典，<面板名,面板>
    private Dictionary<string, BasePanel> _panelDict;
    //面板挂载的根节点
    private Transform _uiRoot;
    public Transform UIRoot
    {
        get
        {
            if(_uiRoot == null)
            {
                _uiRoot = GameObject.Find("Canvas").transform;
            }
            return _uiRoot;
        }
    }

    public UIDatas uiDatas;

    private UIManager()
    {
        InitDicts();
    }

    //相关字典初始化
    private void InitDicts()
    {

        _panelPathDict = new Dictionary<string, string>();

        foreach(var data in uiDatas.uiDataList)
        {
            _panelPathDict.Add(data.uiName, data.uiPath);
        }

        _uiPrefabDict = new Dictionary<string, GameObject>();

        _panelDict = new Dictionary<string, BasePanel>();
    }


    /// <summary>
    /// 打开UI面板 外部直接获取单例调用
    /// </summary>
    /// <param name="name"></param>
    /// <returns>打开的UI面板脚本</returns>
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        //检查该面板是否已经打开
        if(_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogWarning($"名为{name}的UI面板已经打开");

            return null;
        }
        //检查该面板是否存在于路径配置字典中
        string path = "";
        if(!_panelPathDict.TryGetValue(name,out path))
        {
            Debug.LogWarning($"名为{name}的UI面板不存在于路径配置字典中");

            return null;
        }

        //加载界面预制件
        GameObject panelPrefab = null;
        if(!_uiPrefabDict.TryGetValue(name, out panelPrefab))
        {
            string prefabPath = path;

            panelPrefab = Resources.Load<GameObject>(prefabPath);

            _uiPrefabDict.Add(name, panelPrefab);
        }

        //生成并打开界面

        GameObject panelObj = Instantiate(panelPrefab,UIRoot,false);

        panel = panelObj.GetComponent<BasePanel>();

        _panelDict.Add(name, panel);

        return panel;

    }

    /// <summary>
    /// 关闭UI面板 外部直接获取单例调用
    /// </summary>
    /// <param name="name"></param>
    /// <returns>是否关闭成功</returns>
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        //如果当前界面已经打开 才允许关闭
        if (!_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogWarning($"名为{name}的UI面板当前并未打开，无法关闭");

            return false;
        }
        panel.ClosePanel();

        _panelDict.Remove(name);

        return true;
    }

}
