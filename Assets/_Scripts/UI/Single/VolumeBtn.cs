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
    /// �л���������
    /// </summary>
    private void ToggleAudio()
    {
        if (isMuted)
        {
            // �ָ�����
            ChangeVolume(1.0f);
            isMuted = false;
            buttonImage.color = enabledColor;
        }
        else
        {
            // ���浱ǰ����������
            previousVolume = AudioManager.Instance.mainVolume;
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
        }else if (type == "sfx")
        {
            AudioManager.Instance.ChangeSfxVolume(value);
        }
    }
}
