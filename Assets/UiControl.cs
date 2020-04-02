using UnityEngine;
using UnityEngine.SceneManagement;

public class UiControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ShowCharacterInventory()
    {
        SceneManager.LoadScene("CharacterInventory", LoadSceneMode.Additive);
        Encounter.instance.Pause();
    }
}
