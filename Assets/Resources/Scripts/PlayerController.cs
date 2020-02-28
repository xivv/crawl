using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public List<Unit> heroes = new List<Unit>();
    public static PlayerController instance;
    public Vector2 playerPosition;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one player instance, dont create a new one");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        playerPosition = new Vector2(Convert.ToSingle(0.5), Convert.ToSingle(0.5));
    }
}
