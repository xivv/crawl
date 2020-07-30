using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Sunlight : MonoBehaviour
{

    private Light2D light2d;

    // Start is called before the first frame update
    void Start()
    {
        light2d = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dawn()
    {
        light2d.intensity = 0.4f;
        light2d.color = new Color(188, 101, 180);

    }

    public void Day()
    {
        light2d.intensity = 0.7f;
        light2d.color = Color.white;
    }

    public void Dusk()
    {
        light2d.intensity = 0.4f;
        light2d.color = new Color(188, 101, 180);
    }

    public void Night()
    {
        light2d.intensity = 0.05f;
        light2d.color = Color.white;
    }



}
