using UnityEngine;

public class WorldMap : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerController.instance != null)
        {
            player = Instantiate(player, PlayerController.instance.playerPosition, Quaternion.identity);
        }

        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.instance.playerPosition = player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
}
