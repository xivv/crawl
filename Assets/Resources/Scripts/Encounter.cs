using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Encounter : MonoBehaviour
{

    public static Encounter instance;

    public List<UnitOrderObject> participants = new List<UnitOrderObject>();
    private List<UnitOrderObject> monster = new List<UnitOrderObject>();
    private List<UnitOrderObject> heroes = new List<UnitOrderObject>();
    private List<UnitOrderObject> initiative = new List<UnitOrderObject>();

    // Camera Stuff
    public Camera unitCamera;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    [Range(0.5f, 2f)]
    public float smoothTime = 1f;

    public Tilemap groundTilemap;
    public Tilemap wallTilemap;

    // Target Selection
    protected Tilemap targetUndergroundTilemap;
    public TileBase targetUnderground;

    // By fleeing we can destroy the turn cycle
    [HideInInspector]
    public bool alive = false;

    // TurnOrder
    private UnitOrderObject unitToAct;
    private int initiativeCounter = 0;
    public TargetSelector targetSelector;
    public AbilityMenu abilityMenu;
    public ActionMenu actionMenu;
    public UnitInfo unitInfo;

    // UI
    public Canvas canvas;
    public GameObject actButton;
    public GameObject defendActionButton;
    public GameObject finishSelectionButton;
    public GameObject stopSelectionButton;

    // Generating Encounter
    public List<UnitOrderObject> possibleEnemies = new List<UnitOrderObject>();
    public TileBase groundTile;
    public TileBase wallTile;

    void GenerateEncounter(int challengeRating, int width, int height)
    {

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
            UnitOrderObject newObject = Instantiate(possibleEnemies[0], new Vector2(Convert.ToSingle(Random.Range(1, width - 1) + 0.5), Convert.ToSingle(Random.Range(1, height - 1) + 0.5)), Quaternion.identity);
            newObject.unit = new Skeleton();
            this.participants.Add(newObject);

        }

        for (int i = 0; i < PlayerController.instance.heroes.Count; i++)
        {
            CreateUnitOrderObject(PlayerController.instance.heroes[i], width, height);
        }
    }

    private void CreateUnitOrderObject(Unit unit, int width, int height)
    {
        GameObject newObject = new GameObject();
        newObject.transform.position = new Vector2(Convert.ToSingle(Random.Range(1, width - 1) + 0.5), Convert.ToSingle(Random.Range(1, height - 1) + 0.5));
        newObject.AddComponent<UnitOrderObject>();
        newObject.AddComponent<BoxCollider2D>();
        newObject.GetComponent<UnitOrderObject>().unit = unit;
        newObject.AddComponent<SpriteRenderer>();
        newObject.GetComponent<SpriteRenderer>().sprite = unit.sprite;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = "Units";
        newObject.layer = 9;
        this.participants.Add(newObject.GetComponent<UnitOrderObject>());
        Debug.Log("Hero " + unit.unitName + " charges into battle!");
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one encounter instance found");
        }
        instance = this;
        this.targetUndergroundTilemap = GameObject.Find("Target").GetComponent<Tilemap>();
        Debug.Log("INIT");
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateEncounter(0, 5, 5);
        StartEncounter();
    }

    void StartEncounter()
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

        // Initiate turn order
        alive = true;
        unitToAct = initiative[initiativeCounter];
        unitInfo.unitOrderObject = unitToAct;
        unitToAct.BeforeTurn();
        canvas.gameObject.SetActive(true);
        offset = unitCamera.transform.position - unitToAct.transform.position;
    }

    void ToggleActions()
    {
        bool unitCanActAndIsAbleToMove = this.unitToAct.unit.hasStandardAction && !this.unitToAct.pausedMovement;

        if (unitCanActAndIsAbleToMove)
        {
            this.actionMenu.open();
        }
        else
        {
            this.actionMenu.abilitySelection();
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
        this.participants.Remove(unitOrderObject);
        this.initiative.Remove(unitOrderObject);
        Destroy(unitOrderObject.gameObject);
    }


    void ExecuteAbility(Ability ability, UnitOrderObject source, List<UnitOrderObject> selectedTargets)
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (alive && bothPartiesLive())
        {
            RemoveDeadUnits();

            // If we selected an ability we grab it here and first activate the target selection
            if (this.abilityMenu.canAct == true && this.abilityMenu.selectedAbility != null)
            {
                this.abilityMenu.canAct = false;
                this.abilityMenu.gameObject.SetActive(false);
                Ability selectedAbility = this.abilityMenu.selectedAbility;
                SelectTargets(this.unitToAct.transform.position, selectedAbility);
            }

            // We enable - disable actions
            ToggleActions();

            // Apply Conditions
            if (!unitToAct.canAct)
            {
                initiativeCounter++;

                // If one turn ends and we need to start over again
                if (initiative.Count == initiativeCounter)
                {
                    initiativeCounter = 0;
                }

                // We give the unit all its previous state (movement etc.)
                unitToAct = initiative[initiativeCounter];
                unitInfo.unitOrderObject = unitToAct;
                CheckConditions(unitToAct.unit);
                unitToAct.BeforeTurn();
            }

            // If we are done with the selection we reactivate the units movement or resume to take action
            if (this.unitToAct.pausedMovement == true && this.targetSelector.pausedMovement == true && !this.abilityMenu.canAct)
            {

                // We use the ability and remove the standard action
                if (this.targetSelector.TargetSelectionValid())
                {

                    List<Unit> targets = new List<Unit>();
                    foreach (UnitOrderObject unitOrderObject in this.targetSelector.selectedTargets)
                    {
                        targets.Add(unitOrderObject.unit);
                    }

                    this.targetSelector.EndTargetSelection().executeAbility(this.unitToAct.unit, targets);

                    this.unitToAct.pausedMovement = false;
                    this.abilityMenu.selectedAbility = null;
                    if (this.unitToAct.remainingMovementSpeed <= 0)
                    {
                        this.unitToAct.canAct = false;
                    }
                    else
                    {
                        this.unitToAct.unit.hasStandardAction = false;
                    }
                }
                // We cancle the action
                else
                {
                    Debug.Log("No valid targets selected or the action was canceld.>");
                    this.targetSelector.EndTargetSelection();
                    this.unitToAct.pausedMovement = false;
                    this.abilityMenu.selectedAbility = null;
                }

                targetUndergroundTilemap.ClearAllTiles();
            }
        }
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
        this.actButton.SetActive(false);
        this.finishSelectionButton.SetActive(true);
    }

    public void StartAbilitySelection()
    {
        // If the character does not have abilities
        if (this.unitToAct.unit.abilities.Count <= 0) return;

        // Stop the movement of the unit
        this.unitToAct.pausedMovement = true;

        // Show the panel for the abilities
        // Add the abilities of the unit to the panel
        this.abilityMenu.gameObject.SetActive(true);
        this.abilityMenu.source = unitToAct;
        this.abilityMenu.setAbilities(this.unitToAct.unit.abilities);
        this.abilityMenu.canAct = true;
    }

    public void StopAbilitySelection()
    {
        this.actionMenu.open();

        // Hide the panel for the abilities
        // Add the abilities of the unit to the panel
        this.abilityMenu.gameObject.SetActive(false);
        this.abilityMenu.canAct = false;

        this.targetSelector.StopAbilityExecution();
    }

    public void finishSelection()
    {
        this.StopAbilitySelection();
        this.targetSelector.AllowAbilityExecution();
    }

    public void defendAction()
    {
        this.AddCondition(1, ConditionType.DEFENSE, unitToAct.unit, unitToAct.unit);
        // End the turn
        this.StopAbilitySelection();
        this.endTurnAction();
    }

    public void fleeAction()
    {
        // Ends the encounter
        this.alive = false;
        canvas.gameObject.SetActive(true);
        this.endTurnAction();
        SceneManager.LoadScene(1);
    }

    // Reset everything to starting state
    public void endTurnAction()
    {
        this.unitToAct.canAct = false;
        this.abilityMenu.gameObject.SetActive(false);
        this.abilityMenu.selectedAbility = null;
        this.targetSelector.pausedMovement = true;
        this.targetSelector.gameObject.SetActive(false);
        this.actButton.SetActive(true);
        this.finishSelectionButton.SetActive(false);
        this.targetUndergroundTilemap.ClearAllTiles();
    }

    // -----------------

    public void AddCondition(int duration, ConditionType conditionType, Unit source, Unit target)
    {
        target.conditions.Add(new Condition(conditionType, duration, source));
    }

    private void CheckConditions(Unit source)
    {
        foreach (UnitOrderObject unitOrderObject in this.participants)
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
        Vector3 offsetPosition = observingUnit.transform.position + offset;
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

    public bool bothPartiesLive()
    {
        bool living = heroesLive() && monstersLive();

        if (!living)
        {
            this.unitToAct.canAct = false;
            this.alive = false;
            canvas.gameObject.SetActive(true);

            if (heroesLive())
            {
                Debug.Log("All of the enemies died. Returning to World Map.");
                SceneManager.LoadScene(1);
            }
            else
            {
                Debug.Log("All of the heroes died. Returning to Menu. Game Over.");
                SceneManager.LoadScene(0);
            }

        }
        return living;
    }

}
