using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTools : MonoBehaviour
{

    public static MapTools instance;

    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tilemap encounterTilemap;
    public Tilemap tavernTilemap;
    public Tilemap targetTilemap;

    public TileBase targetUnderground;
    public TileBase targetUndergroundRed;

    // Use this for initialization
    void Start()
    {
        this.groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        this.encounterTilemap = GameObject.Find("Encounter").GetComponent<Tilemap>();
        this.wallTilemap = GameObject.Find("Wall").GetComponent<Tilemap>();
        this.tavernTilemap = GameObject.Find("Tavern").GetComponent<Tilemap>();
        this.targetTilemap = GameObject.Find("Target").GetComponent<Tilemap>();

        instance = this;

        DontDestroyOnLoad(this.gameObject);
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


    public static void placeTile(TileBase tileBase, Tilemap tileMap, Vector3Int position)
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
