using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadedBlackPanel : BasePanel
{

    public override void OpenPanel(string name)
    {
        panelName = name;

        canvasGroup.alpha = 1;

        gameObject.SetActive(true);
    }
}
