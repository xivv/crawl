using UnityEngine;

public class Weather : MonoBehaviour
{

    private ParticleSystem particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        particleSystem.transform.position = new Vector3(PlayerController.position.x, PlayerController.position.y, 8);
    }
}
