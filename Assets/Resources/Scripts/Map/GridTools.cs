﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTools : MonoBehaviour
{

    public static GridTools instance;

    private Tilemap groundTilemap;
    private Tilemap wallTilemap;
    private Tilemap encounterTilemap;
    private Tilemap targetTilemap;

    public TileBase targetUnderground;
    public TileBase targetUndergroundRed;

    // Use this for initialization
    void Start()
    {
        instance = this;


        foreach (Transform child in transform)
        {
            switch (child.name)
            {
                case "Ground":
                    groundTilemap = child.gameObject.GetComponent<Tilemap>();
                    break;
                case "Encounter":
                    encounterTilemap = child.gameObject.GetComponent<Tilemap>();
                    break;
                case "Wall":
                    wallTilemap = child.gameObject.GetComponent<Tilemap>();
                    break;
                case "Target":
                    targetTilemap = child.gameObject.GetComponent<Tilemap>();
                    break;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public static void ClearTargetTileMap()
    {
        instance.targetTilemap.ClearAllTiles();
    }

    // Start is for example top left corner and indicate is from where the distance is measured
    public static void DrawReach(Vector2 startPosition, Vector2 indicatePosition, int reach)
    {
        DrawReach(startPosition, indicatePosition, reach, TargetLayer.REACH);

    }


    public static void PlaceTile(TileBase tileBase, Tilemap tileMap, Vector3Int position)
    {
        tileMap.SetTile(position, tileBase);
    }

    // Start is for example top left corner and indicate is from where the distance is measured
    public static void DrawCone(Vector2 startPosition, Direction direction, int length)
    {
        DrawCone(startPosition, direction, length, TargetLayer.REACH);
    }

    // Start is for example top left corner and indicate is from where the distance is measured
    public static void DrawCone(Vector2 startPosition, Direction direction, int length, TargetLayer targetLayer)
    {
        for (int i = 0; i <= length; i++)
        {
            Vector3Int basePosition = new Vector3Int((int)startPosition.x, (int)startPosition.y, 0);

            // west
            if (direction == Direction.WEST)
            {
                basePosition.x -= i;
                basePosition.y -= i;
            }

            for (int a = 0; a < i * 2 + 1; a++)
            {

                Vector3Int checkPosition = new Vector3Int((int)basePosition.x, (int)basePosition.y, 0);

                // west
                if (direction == Direction.WEST)
                {
                    checkPosition.y += a;
                }

                instance.targetTilemap.SetTile(checkPosition, instance.targetUnderground);

            }
        }
    }

    // We accept that there can only be one interactable at a position at one time,
    // This may change when flying later,
    // Can return null
    // This takes longer but is the correct approach
    public static GameObject GetInteractableAtPosition(Vector2 position, int layerIndex)
    {
        Collider2D[] col2d = Physics2D.OverlapCircleAll(position, 0.1f, 1 << layerIndex);

        foreach (var col in col2d)
        {

            if (col.gameObject.GetComponent<Interactable>() != null)
            {
                return col.gameObject;
            }
        }

        return null;
    }


    // Start is for example top left corner and indicate is from where the distance is measured
    public static void DrawReach(Vector2 startPosition, Vector2 indicatePosition, int reach, TargetLayer targetLayer)
    {
        for (int i = 0; i <= reach * 2; i++)
        {
            for (int a = 0; a <= reach * 2; a++)
            {
                Vector2 toCheckPosition = new Vector2(startPosition.x + i, startPosition.y - a);
                Vector3Int toCheckPosition3Int = new Vector3Int((int)toCheckPosition.x, (int)toCheckPosition.y, 0);
                if (Vector2.Distance(toCheckPosition, indicatePosition) <= reach + 1 && instance.groundTilemap.GetTile(toCheckPosition3Int) != null)
                {
                    if (TargetLayer.REACH == targetLayer)
                    {
                        instance.targetTilemap.SetTile(toCheckPosition3Int, instance.targetUnderground);
                    }
                    else
                    {
                        instance.targetTilemap.SetTile(toCheckPosition3Int, instance.targetUndergroundRed);
                    }
                }
            }
        }
    }

}
