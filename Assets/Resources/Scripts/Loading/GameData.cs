using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    List<Loadable> loadables = new List<Loadable>();

    // Start is called before the first frame update
    void Start()
    {
        loadables.AddRange(GetComponents<Loadable>());

        foreach (Loadable loadable in loadables)
        {
            loadable.Load();
        }

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
        foreach (Loadable loadable in loadables)
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
