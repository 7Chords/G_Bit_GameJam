using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonPersistent<UIManager>
{
    //<�����,���Ԥ����·��>
    private Dictionary<string, string> _panelPathDict;
    //Ԥ�Ƽ������ֵ䣬<�����,���Ԥ����>
    private Dictionary<string, GameObject> _uiPrefabDict;
    //��ǰ�Ѵ򿪽����ֵ䣬<�����,���>
    private Dictionary<string, BasePanel> _panelDict;
    //�����صĸ��ڵ�
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

    //����ֵ��ʼ��
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
    /// ��UI��� �ⲿֱ�ӻ�ȡ��������
    /// </summary>
    /// <param name="name"></param>
    /// <returns>�򿪵�UI���ű�</returns>
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;
        //��������Ƿ��Ѿ���
        if(_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogWarning($"��Ϊ{name}��UI����Ѿ���");

            return null;
        }
        //��������Ƿ������·�������ֵ���
        string path = "";
        if(!_panelPathDict.TryGetValue(name,out path))
        {
            Debug.LogWarning($"��Ϊ{name}��UI��岻������·�������ֵ���");

            return null;
        }

        //���ؽ���Ԥ�Ƽ�
        GameObject panelPrefab = null;
        if(!_uiPrefabDict.TryGetValue(name, out panelPrefab))
        {
            string prefabPath = path;

            panelPrefab = Resources.Load<GameObject>(prefabPath);

            _uiPrefabDict.Add(name, panelPrefab);
        }

        //���ɲ��򿪽���

        GameObject panelObj = Instantiate(panelPrefab,UIRoot,false);

        panel = panelObj.GetComponent<BasePanel>();

        _panelDict.Add(name, panel);

        return panel;

    }

    /// <summary>
    /// �ر�UI��� �ⲿֱ�ӻ�ȡ��������
    /// </summary>
    /// <param name="name"></param>
    /// <returns>�Ƿ�رճɹ�</returns>
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        //�����ǰ�����Ѿ��� ������ر�
        if (!_panelDict.TryGetValue(name, out panel))
        {
            Debug.LogWarning($"��Ϊ{name}��UI��嵱ǰ��δ�򿪣��޷��ر�");

            return false;
        }
        panel.ClosePanel();

        _panelDict.Remove(name);

        return true;
    }

}
