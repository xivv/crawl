using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("WorldMap", LoadSceneMode.Single);
        PlayerController.position = new Vector2(0.5f, 0.5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
