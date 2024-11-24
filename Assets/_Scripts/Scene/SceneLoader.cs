using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : SingletonPersistent<SceneLoader>
{

    public float fadeDuration = 1f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    //���س�������������һ��������͸�� ��ת��
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        UIManager.Instance.OpenPanel("SceneLoadedBlackPanel");
        UIManager.Instance.ClosePanel("SceneLoadedBlackPanel");

    }

    public void LoadScene(string sceneName)
    {
        SleepBlackPanel sleepBlackPanel = UIManager.Instance.OpenPanel("SleepBlackPanel") as SleepBlackPanel;

        sleepBlackPanel.StartSleepCounting(fadeDuration, () =>
        {
            SceneManager.LoadScene(sceneName);
        });

    }
}