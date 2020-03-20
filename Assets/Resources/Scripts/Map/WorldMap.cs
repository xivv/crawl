﻿using UnityEngine;

public class WorldMap : MonoBehaviour
{

    public static GameObject player;
    private Vector3 offset;


    public static void CreatePlayer(Vector3 position)
    {
        player = Instantiate(Resources.Load<GameObject>("prefabs/Player"), position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer(PlayerController.position);
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.position = player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
