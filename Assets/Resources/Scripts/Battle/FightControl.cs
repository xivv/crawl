public class FightControl
{

    private static FightControl instance;
    private Fight fight;
    private BattleField battleField;

    public static void StartFight(Fight fight, BattleField battleField)
    {
        instance.fight = fight;
        instance.battleField = battleField;

        GenerateBattleField();
        GeneratPositions();
        GenerateTurnOrder();
        NextTurn();
    }

    public static void GenerateBattleField()
    {

    }
    public static void GeneratPositions()
    {

    }
    public static void GenerateTurnOrder()
    {

    }
    private static void NextTurn()
    {

    }

}
