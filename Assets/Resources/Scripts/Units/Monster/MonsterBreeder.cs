using UnityEngine;

public class MonsterBreeder : MonoBehaviour
{

    public static GameObject Breed(int id, Vector2 position)
    {

        Monster monster = MonsterLoader.Get(id);
        GameObject newObject = new GameObject();

        switch (monster.size)
        {
            case Size.FINE:
                break;
            case Size.SMALL:
                break;
            case Size.MEDIUM:
                newObject.transform.localScale = new Vector3(1, 1, 1);
                break;
            case Size.LARGE:
                newObject.transform.localScale = new Vector3(2, 2, 1);
                break;
            case Size.HUGE:
                newObject.transform.localScale = new Vector3(3, 3, 1);
                break;
        }

        newObject.AddComponent<UnitOrderObject>();
        newObject.AddComponent<SpriteRenderer>();

        newObject.GetComponent<UnitOrderObject>().unit = monster;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = "Units";
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Large");
        newObject.layer = 9;
        newObject.transform.position = position;
        newObject.AddComponent<BoxCollider2D>();
        newObject.name = monster.name;

        return newObject;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
