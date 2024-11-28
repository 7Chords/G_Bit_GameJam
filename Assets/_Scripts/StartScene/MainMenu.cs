using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("OfficeScene");
    }
    // Update is called once per frame
    public void Quit()
    {
        Application.Quit();
    }
}
