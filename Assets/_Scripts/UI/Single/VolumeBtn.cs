using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button toggleButton;
    public string type;
    
    private float previousVolume = 1.0f;
    private bool isMuted = false;
    
    private Text _interactText;
    private void Awake()
    {
        _interactText = transform.GetChild(0).GetComponent<Text>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleAudio();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 切换音量开关
    /// </summary>
    private void ToggleAudio()
    {
        if (isMuted)
        {
            // 恢复音量
            ChangeVolume(1.0f);
            isMuted = false;
        }
        else
        {
            // 保存当前音量并静音
            previousVolume = AudioManager.Instance.mainVolume;
            ChangeVolume(0f);
            isMuted = true;
        }
    }

    private void ChangeVolume(float value)
    {
        if (type == "bgm")
        {
            AudioManager.Instance.ChangeBgmVolume(value);
        }else if (type == "sfx")
        {
            AudioManager.Instance.ChangeSfxVolume(value);
        }
    }
}
