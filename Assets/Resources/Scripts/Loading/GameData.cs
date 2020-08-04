using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    List<LoadableEscape> loadables = new List<LoadableEscape>();

    // Start is called before the first frame update
    void Start()
    {
        loadables.AddRange(GetComponents<LoadableEscape>());

        foreach (LoadableEscape loadable in loadables)
        {
            loadable.Load();
        }

        // Load Dialog Control


        if (IsDataLoaded())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            throw new System.Exception("Could not load all data. For culprit check above in DEBUG.");
        }
    }

    public bool IsDataLoaded()
    {
        foreach (LoadableEscape loadable in loadables)
        {
            if (!loadable.IsLoaded())
            {
                Debug.Log("Couldnt load " + loadable);
                return false;
            }
        }

        return true;
    }

    void Update()
    {

    }
}
