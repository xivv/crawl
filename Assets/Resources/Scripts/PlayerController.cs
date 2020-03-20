using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public List<Unit> heroes = new List<Unit>();
    public static PlayerController instance;
    public static Vector3 position;
    public GameObject playerPrefab;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one player instance, dont create a new one");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
