﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMap : MonoBehaviour
{

    public static GameObject player;
    private Vector3 offset;

    private TimeStatus timeStatus = TimeStatus.DAY;
    public Sunlight sunlight;


    public static void CreatePlayer(Vector3 position)
    {
        if (PlayerController.position != null)
        {
            player = Instantiate(Resources.Load<GameObject>("prefabs/Player"), new Vector3(PlayerController.position.x, PlayerController.position.y), Quaternion.identity);
        }
        else
        {
            player = Instantiate(Resources.Load<GameObject>("prefabs/Player"), new Vector3(0.5f, 0.5f), Quaternion.identity);
        }
    }

    private void Awake()
    {
        // This way we can start from the worldmap in the unity editor
        if (PlayerController.instance == null)
        {
            SceneManager.LoadScene("GameData", LoadSceneMode.Single);
        }
        else
        {
            PlayerController.instance.heroes.Add(HeroBreeder.Breed(1));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer(PlayerController.position);
        offset = transform.position - player.transform.position;
    }

    public static void MoveBack()
    {
        player.GetComponent<MovingObject>().MoveBack();
    }

    public static void ZoomOnPlayer()
    {
        PlayerController.position = player.transform.position;
        PlayerController.previousPosition = player.GetComponent<MovingObject>().previousPosition;
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        ZoomOnPlayer();
        return;
        DateTime time = DateTime.Now;

        if (time.Minute <= 15 && timeStatus != TimeStatus.DAY)
        {
            sunlight.Day();
            timeStatus = TimeStatus.DAY;
        }
        else if (time.Minute > 15 && time.Minute <= 30 && timeStatus != TimeStatus.DAWN)
        {
            sunlight.Dawn();
            timeStatus = TimeStatus.DAWN;
        }
        else if (time.Minute > 30 && time.Minute <= 45 && timeStatus != TimeStatus.NIGHT)
        {
            sunlight.Night();
            timeStatus = TimeStatus.NIGHT;
        }
        else if (time.Minute > 45 && time.Minute <= 60 && timeStatus != TimeStatus.DUSK)
        {
            sunlight.Dusk();
            timeStatus = TimeStatus.DUSK;
        }
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
