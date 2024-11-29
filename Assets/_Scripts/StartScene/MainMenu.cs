using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressAnyKeyToStart : MonoBehaviour
{
    public Text pressAnyKeyText;              // 按任意键提示的文本
    public float fadeInDuration = 1f;         // 淡入持续时间
    public float fadeOutDuration = 1f;        // 淡出持续时间
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
