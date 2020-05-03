using System.Collections;
using UnityEngine;
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

    protected virtual void Start()
    {
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.encounterTilemap = GameObject.Find("Encounter").GetComponent<Tilemap>();
        this.groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        this.wallTilemap = GameObject.Find("Wall").GetComponent<Tilemap>();
    }

    protected virtual bool canMove(Vector2 targetCell)
    {
        bool hasGroundTile = GetCell(groundTilemap, targetCell) != null; //If target Tile has a ground
        bool hasObstacleTile = GetCell(wallTilemap, targetCell) != null; //if target Tile has an obstacle
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

    // Used to do something after movement
    protected virtual void AfterMovement(Vector2 startCell, Vector2 targetCell)
    {

        if (!lastMoveFailed)
        {
            TileBase tileBase = GetCell(encounterTilemap, targetCell);

            //if target Tile is an encounter

            Collider2D collider2D = Physics2D.OverlapCircle(targetCell, 0.1f, 1 << 10);
            bool hasEncounterTile = collider2D != null;

            if (hasEncounterTile)
            {
                // Get the component of the object
                Encounter encounter = collider2D.gameObject.GetComponent<Encounter>();
                encounter.OnEnter();
            }
        }
    }

    private IEnumerator Move(int xDir, int yDir)
    {
        isMoving = true;
        lastMoveFailed = true;

        Vector2 startCell = transform.position;
        Vector2 targetCell = startCell + new Vector2(xDir, yDir);

        bool isOnGround = GetCell(groundTilemap, startCell) != null; //If the player is on the ground

        //If the player starts their movement from a ground tile.
        if (isOnGround)
        {

            //If the front tile is a walkable ground tile, the player moves here.
            if (canMove(targetCell))
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
            AfterMovement(startCell, targetCell);
        }
    }


    protected virtual void resetMovement()
    {
        this.isMoving = false;
    }

    protected TileBase GetCell(Tilemap tilemap, Vector2 cellWorldPos)
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
