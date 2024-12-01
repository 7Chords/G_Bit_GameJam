using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SouvnirUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text _interactText;

    private void Awake()
    {
        _interactText = transform.GetChild(0).GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(false);
    }
}
