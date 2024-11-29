using System;
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

    public Image buttonImage;
    public Color enabledColor = Color.white;
    public Color disabledColor = new Color(1f, 1f, 1f, .5f);

    private Text _interactText;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        _interactText = transform.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        LoadSettings();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleAudio();
        SaveSettings();
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
            ChangeVolume(previousVolume);
            isMuted = false;
            buttonImage.color = enabledColor;
        }
        else
        {
            previousVolume = type == "bgm" ? AudioManager.Instance.mainVolume : previousVolume;
            ChangeVolume(0f);
            isMuted = true;
            buttonImage.color = disabledColor;
        }
    }

    private void ChangeVolume(float value)
    {
        if (type == "bgm")
        {
            AudioManager.Instance.ChangeBgmVolume(value);
        }
        else if (type == "sfx")
        {
            AudioManager.Instance.ChangeSfxVolume(value);
        }
    }

    /// <summary>
    /// 保存设置
    /// </summary>
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat(type + "_volume", previousVolume);
        PlayerPrefs.SetInt(type + "_isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 加载设置
    /// </summary>
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(type + "_volume"))
        {
            previousVolume = PlayerPrefs.GetFloat(type + "_volume");
        }

        if (PlayerPrefs.HasKey(type + "_isMuted"))
        {
            isMuted = PlayerPrefs.GetInt(type + "_isMuted") == 1;
        }
        
        ChangeVolume(isMuted ? 0f : previousVolume);
        buttonImage.color = isMuted ? disabledColor : enabledColor;
    }
}
