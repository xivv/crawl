using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightControl : MonoBehaviour
{

    private static FightControl instance;
    public static Fight fight;
    public static BattleField battleField;
    public static TurnOrder2 turnOrder;

    public static void StartFight(int fightId, int battleFieldId)
    {

        fight = FightLoader.Get(fightId);
        battleField = BattleFieldLoader.Get(battleFieldId);

        SceneManager.LoadScene("FightMap");
    }

    private void Awake()
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

    public void Start()
    {
        GenerateBattleField();
        GeneratePositions();
        GenerateTurnOrder();
        NextTurn();
    }

    public static void GenerateBattleField()
    {

    }
    public static void GeneratePositions()
    {

    }

    public static void GenerateTurnOrder()
    {

        List<UnitOrderObject> allUnitOrderObjects = new List<UnitOrderObject>();

        foreach (Unit unit in fight.GetAllUnits())
        {
            UnitOrderObject unitOrderObject = new UnitOrderObject(unit);
            allUnitOrderObjects.Add(unitOrderObject);
        }

        turnOrder.Initialise(allUnitOrderObjects);
    }

    private static void NextTurn()
    {
        turnOrder.GetActiveUnitOrderObject().AfterTurn();
        turnOrder.Next().BeforeTurn();
    }

}
