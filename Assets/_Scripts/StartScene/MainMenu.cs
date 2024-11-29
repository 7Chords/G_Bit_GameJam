using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressAnyKeyToStart : MonoBehaviour
{
    public Text pressAnyKeyText;              // ���������ʾ���ı�
    public float fadeInDuration = 1f;         // �������ʱ��
    public float fadeOutDuration = 1f;        // ��������ʱ��
    private bool _keyPressed = false;

    private void Start()
    {
        pressAnyKeyText.canvasRenderer.SetAlpha(0);
        pressAnyKeyText.CrossFadeAlpha(1, fadeInDuration, false);
    }

    private void Update()
    {
        if (!_keyPressed && Input.anyKeyDown)
        {
            _keyPressed = true;
            StartGame();
        }
    }

    private void StartGame()
    {
        pressAnyKeyText.CrossFadeAlpha(0, fadeOutDuration, false);
        
        Invoke(nameof(LoadNextScene), fadeOutDuration);
    }

    private void LoadNextScene()
    {
        SceneLoader.Instance.LoadScene("OfficeScene","...");
    }
}
