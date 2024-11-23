using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 所有UI界面的父类
/// </summary>

[RequireComponent(typeof(CanvasGroup))]
public class BasePanel : MonoBehaviour
{
    protected bool hasRemoved = false;

    protected string panelName;

    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void OpenPanel(string name)
    {
        panelName = name;

        canvasGroup.alpha = 0;

        gameObject.SetActive(true);

        Sequence s = DOTween.Sequence();

        s.Append(canvasGroup.DOFade(1, 0.5f));
    }

    public virtual void ClosePanel()
    {
        hasRemoved = true;

        canvasGroup.alpha = 1;

        Sequence s = DOTween.Sequence();

        s.Append(canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        }));

    }
}
