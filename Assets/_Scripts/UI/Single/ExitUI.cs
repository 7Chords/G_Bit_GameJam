using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button exitBtn;
    private Text _interactText;

    void Start()
    {
        exitBtn = GetComponent<Button>();
        _interactText = transform.GetChild(0).GetComponent<Text>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ExitGame();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _interactText.gameObject.SetActive(false);
    }

    private void ExitGame()
    {
        SceneLoader.Instance.LoadScene("OfficeScene","купяжп......");
    }
}
