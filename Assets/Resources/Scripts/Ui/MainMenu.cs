using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static MainMenu instance;

    public GameObject start;
    public GameObject characters;
    public GameObject load;
    public GameObject save;
    public GameObject menu;
    public GameObject quit;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one menu instance, dont create a new one");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void NewGame()
    {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("WorldMap", LoadSceneMode.Single);
        PlayerController.position = new Vector2(0.5f, 0.5f);
    }

    public void QuitGame()
    {
        this.gameObject.SetActive(false);
        Application.Quit();
    }

    public void LoadCharacterMenu()
    {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("CharacterMenu", LoadSceneMode.Additive);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        OutgameMenu();
    }

    public void IngameMenu()
    {
        start.SetActive(false);
        characters.SetActive(true);
        load.SetActive(true);
        save.SetActive(true);
        menu.SetActive(true);
        quit.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OutgameMenu()
    {
        start.SetActive(true);
        characters.SetActive(false);
        load.SetActive(true);
        save.SetActive(false);
        menu.SetActive(false);
        quit.SetActive(true);
        gameObject.SetActive(true);
    }

    private void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
        {
            this.gameObject.SetActive(false);
        }
    }
}
