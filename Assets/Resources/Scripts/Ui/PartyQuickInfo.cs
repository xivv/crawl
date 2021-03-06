﻿using UnityEngine;

public class PartyQuickInfo : MonoBehaviour
{

    public static PartyQuickInfo instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }

    public static void Show()
    {
        instance.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (PlayerController.instance.heroes.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 160 * PlayerController.instance.heroes.Count);

        foreach (Unit unit in PlayerController.instance.heroes)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Ui/HeroQuickInfo");
            GameObject newObject = Instantiate(prefab, transform);
            newObject.GetComponent<HeroQuickInfo>().unit = unit;
        }
    }

    // Update is called once per frame
    void Update()
    {



    }
}
