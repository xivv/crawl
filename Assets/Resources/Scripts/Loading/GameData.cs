﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{

    private bool dataLoaded = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (AbilityLoader.instance != null && ItemLoader.instance != null && MonsterLoader.instance != null)
        {

            if (!AbilityLoader.instance.IsLoaded())
            {
                AbilityLoader.instance.Load();
            }

            if (!ItemLoader.instance.IsLoaded())
            {
                ItemLoader.instance.Load();
            }

            if (!MonsterLoader.instance.IsLoaded())
            {
                MonsterLoader.instance.Load();
            }

            if (!RaceLoader.instance.IsLoaded())
            {
                RaceLoader.instance.Load();
            }


            dataLoaded = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
