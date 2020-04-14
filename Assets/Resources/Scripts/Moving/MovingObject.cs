﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MovingObject : MonoBehaviour
{
    protected bool isMoving;
    private static float moveTime = 0.1f;
    private static float movementDelay = 0.1f;

    protected bool lastMoveFailed = true;
    protected Vector2 direction;

    protected BoxCollider2D boxCollider;
    protected Tilemap groundTilemap;
    protected Tilemap wallTilemap;
    protected Tilemap encounterTilemap;
    protected Tilemap tavernTilemap;

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    protected virtual void Start()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.encounterTilemap = GameObject.Find("Encounter").GetComponent<Tilemap>();
        this.groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        this.wallTilemap = GameObject.Find("Wall").GetComponent<Tilemap>();
        this.tavernTilemap = GameObject.Find("Tavern").GetComponent<Tilemap>();
    }

    protected virtual bool allowMovement(Vector2 targetCell)
    {
        bool hasGroundTile = getCell(groundTilemap, targetCell) != null; //If target Tile has a ground
        bool hasObstacleTile = getCell(wallTilemap, targetCell) != null; //if target Tile has an obstacle
        bool hitsWall = Physics2D.OverlapCircleAll(targetCell, 0.1f, 1 << 8).Length > 0;
        return hasGroundTile && !hasObstacleTile && !hitsWall;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (isMoving) return;

        //To store move directions.
        int horizontal = 0;
        int vertical = 0;
        //To get move directionsy
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //We can't go in both directions at the same time
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //If there's a direction, we are trying to move.
        if (horizontal != 0 || vertical != 0)
        {
            direction = new Vector2(horizontal, vertical);
            StartCoroutine(Move(horizontal, vertical));
        }
    }

    protected virtual void AfterMovement()
    {
        // Used to do something after movement
    }

    private IEnumerator Move(int xDir, int yDir)
    {
        isMoving = true;
        lastMoveFailed = true;

        Vector2 startCell = transform.position;
        Vector2 targetCell = startCell + new Vector2(xDir, yDir);

        Debug.DrawLine(startCell, targetCell, Color.red, 5f);
        Debug.Log("Moving in :" + GetDirection());

        bool isOnGround = getCell(groundTilemap, startCell) != null; //If the player is on the ground
        bool hasEncounterTile = getCell(encounterTilemap, targetCell) != null; //if target Tile is an encounter
        bool hasTavernTile = getCell(tavernTilemap, targetCell) != null; //if target Tile is an encounter

        //If the player starts their movement from a ground tile.
        if (isOnGround)
        {

            //If the front tile is a walkable ground tile, the player moves here.
            if (allowMovement(targetCell))
            {

                lastMoveFailed = false;
                float sqrRemainingDistance = (transform.position - (Vector3)targetCell).sqrMagnitude;
                while (sqrRemainingDistance > float.Epsilon)
                {

                    Vector3 newPosition = Vector3.MoveTowards(transform.position, targetCell, (1 / moveTime) * Time.deltaTime);
                    transform.position = newPosition;
                    sqrRemainingDistance = (transform.position - (Vector3)targetCell).sqrMagnitude;
                    yield return null;
                }
            }

            Invoke("resetMovement", movementDelay);
            AfterMovement();

            if (hasTavernTile)
            {
                // Load the hero selection
                SceneManager.LoadScene("Tavern");
            }
            else if (hasEncounterTile)
            {
                // Load the encounter
                SceneManager.LoadScene("BattleMap");
            }
        }
    }

    protected virtual void resetMovement()
    {
        this.isMoving = false;
    }

    protected TileBase getCell(Tilemap tilemap, Vector2 cellWorldPos)
    {
        return tilemap.GetTile(tilemap.WorldToCell(cellWorldPos));
    }

    public MovementDirection GetDirection()
    {
        if (direction.x == 0)
        {
            if (direction.y == -1)
            {
                return MovementDirection.SOUTH;
            }
            else
            {
                return MovementDirection.NORTH;
            }
        }
        else
        {
            if (direction.x == -1)
            {
                return MovementDirection.WEST;
            }
            else
            {
                return MovementDirection.EAST;
            }
        }
    }

}

public enum MovementDirection
{
    SOUTH,
    NORTH,
    EAST,
    WEST
}
