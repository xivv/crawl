using UnityEngine;

public class Loadable : MonoBehaviour
{

    private bool isLoaded = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsLoaded()
    {
        return isLoaded;
    }

    public void SetLoaded()
    {
        isLoaded = true;
    }

    public virtual void Load()
    {

    }
}
