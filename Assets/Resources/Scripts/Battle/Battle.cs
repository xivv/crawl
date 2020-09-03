using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Battle : MonoBehaviour
{
    public static Battle instance;
    public BattleState battleState;

    public List<UnitOrderObject> participants = new List<UnitOrderObject>();
    private List<UnitOrderObject> monster = new List<UnitOrderObject>();
    private List<UnitOrderObject> heroes = new List<UnitOrderObject>();

    // Camera Stuff
    public Camera unitCamera;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    [Range(0.5f, 2f)]
    public float smoothTime = 1f;

    // By fleeing we can destroy the turn cycle
    [HideInInspector]
    public bool alive = false;

    // Controlling encounter states like pausing
    public bool isRunning = false;

    // TurnOrder
    private List<UnitOrderObject> initiative = new List<UnitOrderObject>();
    private int initiativeCounter = 0;
    private UnitOrderObject unitToAct;

    // Menu
    public TargetSelector targetSelector;
    public UnitInfo unitInfo;

    // Generating Encounter
    public List<UnitOrderObject> possibleEnemies = new List<UnitOrderObject>();

    public Tilemap groundTilemap;
    public TileBase groundTile;

    public Tilemap wallTilemap;
    public TileBase wallTile;

    // Vision
    public bool isDay = true;

    public bool IsAlly(UnitOrderObject source, UnitOrderObject target)
    {

        if (monster.Contains(source))
        {
            return monster.Contains(target);
        }
        else if (heroes.Contains(source))
        {
            return heroes.Contains(target);
        }

        return false;
    }

    void GenerateEncounter(int challengeRating, int width, int height)
    {

        int number = Random.Range(0, 10);

        if (isDay)
        {
            Instantiate(Resources.Load("Prefabs/Vision/Sunlight"), new Vector2(0, 0), Quaternion.identity);
        }

        for (int i = 0; i < width; i++)
        {
            for (int a = 0; a < height; a++)
            {

                // Place Walls
                if (i == 0 || a == 0 || i == width - 1 || a == height - 1)
                {
                    wallTilemap.SetTile(new Vector3Int(a, i, 0), wallTile);

                }
                // Place ground
                else
                {
                    groundTilemap.SetTile(new Vector3Int(a, i, 0), groundTile);
                }
            }
        }

        int enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randomPosition = new Vector2(Convert.ToSingle(Random.Range(1, width - 1) + 0.5), Convert.ToSingle(Random.Range(1, height - 1) + 0.5));
            // Vector2 randomPosition = new Vector2(Convert.ToSingle(Random.Range(1, width - 1)), Convert.ToSingle(Random.Range(1, height - 1)));
            GameObject monster = MonsterBreeder.Breed(2, randomPosition);
            UnitOrderObject unitOrderObject = monster.GetComponent<UnitOrderObject>();
            this.participants.Add(unitOrderObject);
        }

        heroes = PlayerController.GenerateHeroes();

        // Set Random Position
        foreach (UnitOrderObject unitOrderObject in heroes)
        {
            Vector2 vector2 = new Vector2(Convert.ToSingle(Random.Range(1, width - 1) + 0.5), Convert.ToSingle(Random.Range(1, height - 1) + 0.5));
            unitOrderObject.gameObject.transform.position = vector2;
            this.participants.Add(unitOrderObject);
        }

    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one encounter instance found");
            Destroy(this);
        }
        else
        {
            instance = this;
            isRunning = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateEncounter(0, 10, 10);
        StartEncounter();
    }

    void CreateInitiative()
    {
        // Add units to initiative
        foreach (UnitOrderObject unitOrderObject in participants)
        {
            Unit unit = unitOrderObject.unit;

            if (!unit.isDead)
            {
                unitOrderObject.RollInitiative();
                initiative.Add(unitOrderObject);

                if (unit.typeClass == TypeClass.HERO)
                {
                    heroes.Add(unitOrderObject);
                }
                else if (unit.typeClass == TypeClass.MONSTER)
                {
                    monster.Add(unitOrderObject);
                }
            }
        }

        // Sort by init
        initiative.Sort((a, b) =>
        {
            if (a.rolledInit >= b.rolledInit)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });
    }

    void StartEncounter()
    {

        CreateInitiative();

        // Initiate turnorder
        alive = true;
        unitToAct = initiative[initiativeCounter];
        unitInfo.unitOrderObject = unitToAct;
        unitToAct.BeforeTurn();
        offset = unitCamera.transform.position - unitToAct.transform.position;

        // Load turnorder ui
        TurnOrder.Reload(unitToAct, initiative);
    }

    void ToggleActionMenu()
    {
        bool unitCanActAndIsAbleToMove = unitToAct.unit.hasStandardAction && !unitToAct.pausedMovement;
        bool unitIsAbleToMoveNoStandardAction = !unitToAct.unit.hasStandardAction && !unitToAct.pausedMovement;

        if (unitIsAbleToMoveNoStandardAction)
        {
            ActionMenu.NoStandardAction();
        }
        else if (unitCanActAndIsAbleToMove)
        {
            ActionMenu.Open();
        }
    }

    void RemoveDeadUnits()
    {
        List<UnitOrderObject> deadUnits = new List<UnitOrderObject>();

        foreach (UnitOrderObject unitOrderObject in this.participants)
        {
            if (unitOrderObject.unit.isDead)
            {
                deadUnits.Add(unitOrderObject);
            }
        }

        foreach (UnitOrderObject unitOrderObject in deadUnits)
        {
            RemoveDeadUnit(unitOrderObject);
        }
    }

    void RemoveDeadUnit(UnitOrderObject unitOrderObject)
    {
        participants.Remove(unitOrderObject);
        initiative.Remove(unitOrderObject);
        TurnOrder.Reload(unitToAct, initiative);
        Destroy(unitOrderObject.gameObject);
    }


    void ExecuteAbility(Ability ability, UnitOrderObject source)
    {

        unitToAct.pausedMovement = false;

        ability.Execute(unitToAct.unit, UnitOrderObjectsToUnits(targetSelector.EndTargetSelection()));

        if (unitToAct.remainingMovementSpeed <= 0)
        {
            unitToAct.canAct = false;
        }
        else
        {
            unitToAct.unit.hasStandardAction = false;
        }
    }

    public static void SetState(BattleState state)
    {

        if (state == BattleState.ABILITYSELECTION)
        {
            if (instance.battleState == BattleState.TARGETSELECTION)
            {
                // CONTINUE HERE
                AbilityMenu.Deselect();
                instance.targetSelector.EndTargetSelection();
                GridTools.ClearTargetTileMap();
            }
            else
            {

                if (instance.battleState == BattleState.ABILITYSELECTION)
                {
                    instance.StopAbilitySelection();
                }

                instance.StartAbilitySelection();
            }
        }
        else if (state == BattleState.TARGETSELECTION)
        {
            AbilityMenu.Select();
            instance.targetSelector.StartTargetSelection(instance.unitToAct, AbilityMenu.GetSelectedAbility(), instance.unitToAct.transform.position, instance.participants);
        }
        else if (state == BattleState.ABILITYEXECUTION)
        {
            instance.ExecuteAbility(AbilityMenu.GetSelectedAbility(), instance.unitToAct);
        }
        else if (state == BattleState.ACTION)
        {
            if (instance.battleState == BattleState.ABILITYSELECTION)
            {
                // Normally we would call Unload() but StopAbilitySelection Calls Unload aswell
                instance.StopAbilitySelection();
            }

            instance.unitToAct.canAct = true;
        }

        instance.battleState = state;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning && alive && bothPartiesLive())
        {
            RemoveDeadUnits();

            // If we selected an ability we grab it here and first activate the target selection
            //if (abilityMenu.HasTargetSelected())
            //{
            //   SelectTargets(unitToAct.transform.position, abilityMenu.GetSelectedAbility());
            //   abilityMenu.StopAbilitySelection();
            //}

            // We enable - disable actions
            ToggleActionMenu();

            // Apply Conditions
            if (!unitToAct.canAct)
            {
                unitToAct.AfterTurn();

                initiativeCounter++;

                // If one turn ends and we need to start over again
                if (initiative.Count == initiativeCounter)
                {
                    initiativeCounter = 0;
                }

                // We give the unit all its previous state (movement etc.)
                unitToAct = initiative[initiativeCounter];
                TurnOrder.Reload(unitToAct, initiative);
                unitInfo.unitOrderObject = unitToAct;

                CheckConditions(unitToAct.unit);
                unitToAct.BeforeTurn();
            }
        }
    }

    void CreateTorch()
    {
        // Create a vision object for the unit
        GameObject torch = Resources.Load<GameObject>("Prefabs/Torch");
        Instantiate(torch, unitToAct.gameObject.transform.position, Quaternion.identity);
    }

    void LateUpdate()
    {
        if (alive)
        {

            TurnOrderObject observingUnit = this.unitToAct;

            if (this.unitToAct.pausedMovement == true && this.targetSelector.pausedMovement == false)
            {
                observingUnit = this.targetSelector;
            }

            if (!observingUnit.canAct)
            {
                offset = unitCamera.transform.position - observingUnit.transform.position;
            }
            else
            {
                moveUnitCamera(observingUnit);
            }

        }
    }

    // Actions

    private void SelectTargets(Vector2 startingPosition, Ability ability)
    {
        this.targetSelector.StartTargetSelection(unitToAct, ability, startingPosition, participants);
    }

    public void StartAbilitySelection()
    {
        // If the character does not have abilities
        if (unitToAct.unit.abilities == null || unitToAct.unit.abilities.Count <= 0) return;

        // Stop the movement of the unit
        unitToAct.pausedMovement = true;

        // Show the panel for the abilities
        // Add the abilities of the unit to the panel
        ActionMenu.AbilitySelection();
        AbilityMenu.Load(unitToAct.unit.abilities, unitToAct.transform.position);

    }

    public void OpenAbilityMenu()
    {
        SetState(BattleState.ABILITYSELECTION);
    }

    public void CloseAbilityMenu()
    {
        SetState(BattleState.ACTION);
    }

    public void StopAbilitySelection()
    {
        ActionMenu.Open();

        // Hide the panel for the abilities
        // Add the abilities of the unit to the panel
        unitToAct.pausedMovement = false;
        GridTools.ClearTargetTileMap();
        targetSelector.EndTargetSelection();

        AbilityMenu.UnLoad();
    }

    public void finishSelection()
    {
        this.StopAbilitySelection();
        this.ExecuteAbility(this.targetSelector.ability, this.unitToAct);
    }

    public void defendAction()
    {
        unitToAct.unit.AddCondition(1, ConditionType.DEFENSE, unitToAct.unit);

        // End the turn
        this.StopAbilitySelection();
        this.endTurnAction();
    }

    public void fleeAction()
    {
        // Ends the encounter
        alive = false;
        endTurnAction();
        ExitEncounterAlive();
    }

    // Reset everything to starting state
    public void endTurnAction()
    {
        unitToAct.canAct = false;
        targetSelector.pausedMovement = true;
        targetSelector.gameObject.SetActive(false);
        GridTools.ClearTargetTileMap();
        AbilityMenu.UnLoad();
    }

    private void CheckConditions(Unit source)
    {
        foreach (UnitOrderObject unitOrderObject in participants)
        {

            Unit unit = unitOrderObject.unit;

            for (int i = unit.conditions.Count - 1; i >= 0; i--)
            {

                Condition condition = unit.conditions[i];

                // Reverse
                if (condition.source == source)
                {
                    condition.remainingTime -= 1;

                    if (condition.remainingTime <= 0)
                    {
                        unit.conditions.RemoveAt(i);

                        if (condition.conditionType == ConditionType.DEFENSE)
                        {
                            unit.encounterStats.ac -= 3;

                        }
                        else if (condition.conditionType == ConditionType.RAGE)
                        {
                            unit.encounterStats.strength -= 2;
                            unit.encounterStats.will += 1;
                        }
                        else if (condition.conditionType == ConditionType.FEARED)
                        {
                            unit.encounterStats.will += 2;
                        }
                        else if (condition.conditionType == ConditionType.POISONED)
                        {
                            unit.encounterStats.fortitude += 1;
                            unit.encounterStats.constitution += 2;
                        }
                        else if (condition.conditionType == ConditionType.BLINDED)
                        {
                            unit.encounterStats.strength += 1;
                            unit.encounterStats.dexterity += 1;
                        }
                        else if (condition.conditionType == ConditionType.ENTANGLED || condition.conditionType == ConditionType.HASTE)
                        {
                            unit.encounterStats.speed = unit.baseStats.speed;
                        }
                        else if (condition.conditionType == ConditionType.BLESSED)
                        {
                            unit.encounterStats.strength -= 1;
                            unit.encounterStats.dexterity -= 1;
                            unit.encounterStats.constitution -= 1;
                            unit.encounterStats.intelligence -= 1;
                            unit.encounterStats.wisdom -= 1;
                            unit.encounterStats.charisma -= 1;
                        }
                    }
                    // Ongoing buffs that do something every round
                    else
                    {
                        if (condition.conditionType == ConditionType.BLEEDING)
                        {
                            int damage = Convert.ToInt32(unit.encounterStats.health * 0.05) * -1;
                            unit.handleHealthChange(damage, DamageType.BLEED);
                        }
                        else if (condition.conditionType == ConditionType.POISONED)
                        {
                            int damage = Convert.ToInt32(unit.encounterStats.health * 0.02) * -1;
                            unit.handleHealthChange(damage, DamageType.POISON);
                        }
                        else if (condition.conditionType == ConditionType.REGENERATING)
                        {
                            int damage = Convert.ToInt32(unit.encounterStats.health * 0.05);
                            unit.handleHealthChange(damage, DamageType.HOLY);
                        }
                    }

                }
                // Apply
                else if (condition.remainingTime == condition.duration)
                {
                    if (condition.conditionType == ConditionType.DEFENSE)
                    {
                        unit.encounterStats.ac += 3;
                    }
                    else if (condition.conditionType == ConditionType.RAGE)
                    {
                        unit.encounterStats.strength += 2;
                        unit.encounterStats.will -= 1;
                    }
                    else if (condition.conditionType == ConditionType.FEARED)
                    {
                        unit.encounterStats.will -= 2;
                    }
                    else if (condition.conditionType == ConditionType.BLEEDING)
                    {
                        int damage = Convert.ToInt32(unit.encounterStats.health * 0.05) * -1;
                        unit.handleHealthChange(damage, DamageType.BLEED);
                    }
                    else if (condition.conditionType == ConditionType.POISONED)
                    {
                        int damage = Convert.ToInt32(unit.encounterStats.health * 0.02) * -1;
                        unit.handleHealthChange(damage, DamageType.POISON);
                        unit.encounterStats.fortitude -= 1;
                        unit.encounterStats.constitution -= 2;
                    }
                    else if (condition.conditionType == ConditionType.BLINDED)
                    {
                        unit.encounterStats.strength -= 1;
                        unit.encounterStats.dexterity -= 1;
                    }
                    else if (condition.conditionType == ConditionType.ENTANGLED)
                    {
                        unit.encounterStats.speed = 0;
                    }
                    else if (condition.conditionType == ConditionType.BLESSED)
                    {
                        unit.encounterStats.strength += 1;
                        unit.encounterStats.dexterity += 1;
                        unit.encounterStats.constitution += 1;
                        unit.encounterStats.intelligence += 1;
                        unit.encounterStats.wisdom += 1;
                        unit.encounterStats.charisma += 1;
                    }
                    else if (condition.conditionType == ConditionType.REGENERATING)
                    {
                        int damage = Convert.ToInt32(unit.encounterStats.health * 0.05);
                        unit.handleHealthChange(damage, DamageType.HOLY);
                    }
                    else if (condition.conditionType == ConditionType.HASTE)
                    {
                        unit.encounterStats.speed = unit.baseStats.speed * 2;
                    }
                }
            }
        }
    }

    private void moveUnitCamera(TurnOrderObject observingUnit)
    {
        Vector3 offsetPosition = new Vector3(observingUnit.transform.position.x, observingUnit.transform.position.y, observingUnit.transform.position.z + offset.z);
        unitCamera.transform.position = Vector3.SmoothDamp(unitCamera.transform.position, offsetPosition, ref velocity, smoothTime);
    }

    public bool monstersLive()
    {
        foreach (UnitOrderObject unitOrderObject in monster)
        {
            if (!unitOrderObject.unit.isDead)
            {
                return true;
            }
        }
        return false;
    }

    public bool heroesLive()
    {
        foreach (UnitOrderObject unitOrderObject in heroes)
        {
            if (!unitOrderObject.unit.isDead)
            {
                return true;
            }
        }
        return false;
    }

    public int CalculateExperiencePointsReward()
    {
        int totalExperiencePoints = 0;

        foreach (UnitOrderObject unitOrderObject in monster)
        {
            if (unitOrderObject.unit.isDead)
            {
                totalExperiencePoints += unitOrderObject.unit.metaInformation.exp;
            }
        }

        return totalExperiencePoints;
    }

    public void AwardLoot()
    {

        List<Item> loot = new List<Item>();

        foreach (UnitOrderObject unitOrderObject in monster)
        {
            if (unitOrderObject.unit.isDead)
            {
                foreach (Item item in unitOrderObject.unit.items)
                {
                    loot.Add(item);
                }
            }
        }

        PlayerController.AwardLoot(loot);
    }

    public void AwardHeroesExperiencePoints()
    {
        // Give the players experience points for killing even if fleeing a battle

        foreach (UnitOrderObject unitOrderObject in heroes)
        {
            // Should dead heroes also get experience points?
            // This could matter for reviving, on hard dont give
            //     unitOrderObject.unit.AwardExperience(CalculateExperiencePointsReward() / heroes.Count);
        }
    }

    public void ExitEncounterAlive()
    {
        isRunning = false;
        AwardHeroesExperiencePoints();
        AwardLoot();

        Debug.Log("All of the enemies died. Returning to World Map.");
        SceneManager.LoadScene("WorldMap");
    }

    public bool bothPartiesLive()
    {
        bool living = heroesLive() && monstersLive();

        if (!living)
        {
            unitToAct.canAct = false;
            alive = false;

            if (heroesLive())
            {
                ExitEncounterAlive();
            }
            else
            {
                Debug.Log("All of the heroes died. Returning to Menu. Game Over.");
                SceneManager.LoadScene(0);
            }

        }
        return living;
    }

    public List<Unit> UnitOrderObjectsToUnits(List<UnitOrderObject> unitOrderObjects)
    {
        List<Unit> targets = new List<Unit>();
        foreach (UnitOrderObject unitOrderObject in unitOrderObjects)
        {
            targets.Add(unitOrderObject.unit);
        }

        return targets;
    }

    public static void Pause()
    {
        instance.isRunning = false;

        // TODO: Disable all UI elements
    }

    public static void Resume()
    {
        instance.isRunning = true;

        // TODO: Disable all UI elements
    }
}


public enum BattleState
{
    ACTION,
    ABILITYSELECTION,
    ABILITYEXECUTION,
    TARGETSELECTION
}
