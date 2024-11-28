using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game");//Ìí¼Ó³¡¾°Â·¾¶
    }
    // Update is called once per frame
    public void Quit()
    {
        Application.Quit();
    }
}
