using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game");//��ӳ���·��
    }
    // Update is called once per frame
    public void Quit()
    {
        Application.Quit();
    }
}
