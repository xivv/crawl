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
        if (!dataLoaded && AbilityLoader.instance != null && ItemLoader.instance != null)
        {
            AbilityLoader.instance.Load();
            ItemLoader.instance.Load();

            dataLoaded = true;
        }
    }
}
