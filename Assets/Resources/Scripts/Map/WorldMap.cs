using UnityEngine;

public class WorldMap : MonoBehaviour
{

    public static GameObject player;
    private Vector3 offset;


    public static void CreatePlayer(Vector3 position)
    {
        player = Instantiate(Resources.Load<GameObject>("prefabs/Player"), new Vector3(0.5f, 0.5f), Quaternion.identity);
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
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
