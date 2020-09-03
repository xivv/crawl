using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : ControllingElement
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

        NewGame();
    }

    public void NewGame()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("WorldMap", LoadSceneMode.Single);
        PlayerController.position = new Vector2(0.5f, 0.5f);
    }

    public void QuitGame()
    {
        gameObject.SetActive(false);
        Application.Quit();
    }

    public void LoadCharacterMenu()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("CharacterMenu", LoadSceneMode.Additive);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        CloseMenu();
    }

    public static void OpenMenu()
    {
        instance.start.SetActive(false);
        instance.characters.SetActive(true);
        instance.load.SetActive(true);
        instance.save.SetActive(true);
        instance.menu.SetActive(true);
        instance.quit.SetActive(true);
        instance.gameObject.SetActive(true);
    }

    public static void CloseMenu()
    {
        instance.start.SetActive(true);
        instance.characters.SetActive(false);
        instance.load.SetActive(true);
        instance.save.SetActive(false);
        instance.menu.SetActive(false);
        instance.quit.SetActive(true);
        instance.gameObject.SetActive(true);
    }

    public static void HideMenu()
    {
        instance.gameObject.SetActive(false);
    }

    public new static bool IsKeyBlocking()
    {
        if (instance == null)
        {
            return false;
        }

        return instance.gameObject.activeSelf;
    }
}
