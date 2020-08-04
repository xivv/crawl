using UnityEngine;

// WHY??: https://stackoverflow.com/questions/15574977/wildcard-equivalent-in-c-sharp-generics
public class LoadableEscape : MonoBehaviour
{

    internal LoadableEscape() { }

    protected bool isLoaded = false;

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
