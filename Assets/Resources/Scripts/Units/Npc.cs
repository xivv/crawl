using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Npc : Interactable
{
    public override void Interact()
    {
        Talk();
    }

    public void Talk()
    {
        SceneManager.LoadSceneAsync("DialogModule", LoadSceneMode.Additive);
        //   DialogControl.ShowText(null);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator LoadScene(string scene)
    {
        // Start loading the scene
        UnityEngine.AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
    }

}
