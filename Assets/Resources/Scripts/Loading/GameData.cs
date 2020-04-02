using UnityEngine;

public class GameData : MonoBehaviour
{

    private bool dataLoaded = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AbilityLoader.instance != null && ItemLoader.instance != null)
        {

            if (!AbilityLoader.instance.IsLoaded())
            {
                AbilityLoader.instance.Load();
            }

            if (!ItemLoader.instance.IsLoaded())
            {
                ItemLoader.instance.Load();
            }

            dataLoaded = true;
        }
    }
}
