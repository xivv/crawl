using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetSelector : TurnOrderObject
{
    [HideInInspector]
    public List<UnitOrderObject> selectedTargets = new List<UnitOrderObject>();

    [HideInInspector]
    public List<UnitOrderObject> participants = new List<UnitOrderObject>();

    public Encounter encounter;

    [HideInInspector]
    public Ability ability;
    private UnitOrderObject source;

    private Vector2 startingPosition;

    public Tilemap selectedTargetsTileMap;
    public TileBase selectedTargetTileBase;

    public bool IsValidTarget(UnitOrderObject unitOrderObject)
    {
        // We need to check if atleast one effect can target the target

        Boolean isValid = false;

        foreach (AbilityEffect abilityEffect in ability.effects)
        {
            if (abilityEffect.targetType.Equals(TargetType.ALL))
            {
                isValid = true;
            }
            else if (abilityEffect.targetType.Equals(TargetType.ALLY))
            {
                isValid = encounter.IsAlly(source, unitOrderObject) || unitOrderObject.Equals(source);
            }
            else if (abilityEffect.targetType.Equals(TargetType.ENEMY))
            {
                isValid = !encounter.IsAlly(source, unitOrderObject);
            }
            else if (abilityEffect.targetType.Equals(TargetType.SELF))
            {
                isValid = unitOrderObject.Equals(source);
            }
            else if (abilityEffect.targetType.Equals(TargetType.OTHER))
            {
                isValid = !unitOrderObject.Equals(source);
            }

            if (isValid)
            {
                return isValid;
            }
        }

        return false;
    }

    // At least one target and correct number of targets
    public bool TargetSelectionValid()
    {
        // evtl. muss hier statt == 0 noch was anderes 
        // Dies kam durch die EInführung nicht entfernung von serializable
        return selectedTargets.Count > 0 && (selectedTargets.Count >= ability.minTargets || ability.minTargets == 0 || ability.maxTargets == 0);
    }

    public void SelectAOETargets(Ability ability)
    {
        // Check the distance for everyboady and set the list of the targetselector to the units we got
        foreach (UnitOrderObject unitOrderObject in participants)
        {
            float distance = Vector2.Distance(startingPosition, unitOrderObject.transform.position);
            if (distance <= ability.reach + 1)
            {
                this.AddTarget(unitOrderObject);
            }
        }
    }

    public void StartTargetSelection(UnitOrderObject source, Ability ability, Vector2 startingPosition, List<UnitOrderObject> participants)
    {
        this.selectedTargets.Clear();
        this.participants = participants;
        this.source = source;
        this.source.pausedMovement = true;
        this.pausedMovement = false;
        this.ability = ability;
        this.startingPosition = startingPosition;
        this.transform.position = startingPosition;
        this.gameObject.SetActive(true);

        if (ability.IsAOE())
        {
            this.SelectAOETargets(ability);
        }
        // If we can only target ourself
        else if (ability.IsSelfTargeting())
        {
            this.AddTarget(source);
        }
        // Draw where targets can be selected
        else
        {
            ability.DrawReach(source, startingPosition);
        }
    }

    public void AllowAbilityExecution()
    {
        this.pausedMovement = true;
    }

    public void ClearSelectedTargets()
    {

    }

    public List<UnitOrderObject> EndTargetSelection()
    {
        this.pausedMovement = true;
        this.gameObject.SetActive(false);
        this.selectedTargetsTileMap.ClearAllTiles();
        return this.selectedTargets;
    }


    protected override void Start()
    {
        base.Start();
        selectedTargetsTileMap = GameObject.Find("SelectedTargets").GetComponent<Tilemap>();

    }

    protected override bool allowMovement(Vector2 targetCell)
    {
        float distance = Vector2.Distance(startingPosition, targetCell);
        return base.allowMovement(targetCell) && distance < ability.reach + 1;
    }

    void AddTargetAtPosition(Vector2 position)
    {
        Vector2 endPosition = position;
        Vector2 regionPosition = new Vector2(Convert.ToSingle(position.x + 0.5), Convert.ToSingle(position.y + 0.5));

        bool targetAquired = false;

        if (ability.targetStartPoint == TargetStartPoint.REGION)
        {
            // We add half
            targetAquired = this.IsUnitAtPosition(regionPosition);
        }
        else
        {
            targetAquired = this.hitsUnit(position);
        }

        if (targetAquired)
        {

            UnitOrderObject selectedUnit = null;

            if (ability.targetStartPoint == TargetStartPoint.REGION)
            {
                selectedUnit = this.UnitAtPosition(regionPosition).transform.gameObject.GetComponent<UnitOrderObject>();
            }
            else
            {
                selectedUnit = this.rayCastToUnit(position).transform.gameObject.GetComponent<UnitOrderObject>();
            }

            this.AddTarget(selectedUnit);
        }
    }

    public void AddTarget(UnitOrderObject unitOrderObject)
    {
        Vector3Int toCheckPosition3Int = new Vector3Int((int)unitOrderObject.transform.position.x, (int)unitOrderObject.transform.position.y, 0);

        if (this.selectedTargets.Contains(unitOrderObject))
        {
            this.selectedTargets.Remove(unitOrderObject);
            MapTools.placeTile(null, selectedTargetsTileMap, toCheckPosition3Int);
        }

        // If the list is not full already or we have no limit
        else if ((this.selectedTargets.Count < ability.maxTargets || ability.maxTargets == 0) && this.IsValidTarget(unitOrderObject))
        {
            this.selectedTargets.Add(unitOrderObject);
            MapTools.placeTile(selectedTargetTileBase, selectedTargetsTileMap, toCheckPosition3Int);
        }
    }


    protected override void AfterMovement()
    {
        base.AfterMovement();

        if (this.pausedMovement == false && ability.targetStartPoint == TargetStartPoint.REGION)
        {
            Vector2 startPosition = new Vector2(this.transform.position.x - ability.targetArea, this.transform.position.y + ability.targetArea);
            MapTools.ClearTargetTileMap();

            if (ability.targetPolygon == TargetPolygon.RECTANGLE)
            {
                MapTools.DrawReach(startPosition, this.transform.position, ability.targetArea, TargetLayer.AREA);
            }
            else if (ability.targetPolygon == TargetPolygon.CONE)
            {
                MapTools.DrawCone(this.transform.position, Direction.WEST, ability.targetArea, TargetLayer.AREA);
            }
        }
    }

    void CheckRectanglePosition()
    {
        selectedTargets.Clear();
        selectedTargetsTileMap.ClearAllTiles();
        Vector2 startPosition = new Vector2(this.transform.position.x - ability.targetArea, this.transform.position.y + ability.targetArea); ;
        Vector2 indicatePosition = this.transform.position;

        for (int i = 0; i <= ability.targetArea * 2; i++)
        {
            for (int a = 0; a <= ability.targetArea * 2; a++)
            {
                Vector2 toCheckPosition = new Vector2(startPosition.x + i, startPosition.y - a);
                Vector3Int toCheckPosition3Int = new Vector3Int((int)toCheckPosition.x, (int)toCheckPosition.y, 0);
                if (Vector2.Distance(toCheckPosition, indicatePosition) <= ability.targetArea + 1 && groundTilemap.GetTile(toCheckPosition3Int) != null)
                {
                    AddTargetAtPosition(new Vector2(toCheckPosition3Int.x, toCheckPosition3Int.y));
                }
            }
        }
    }

    void CheckConePosition()
    {
        selectedTargets.Clear();
        selectedTargetsTileMap.ClearAllTiles();
        Vector2 startPosition = this.gameObject.transform.position;
        Vector2 sourcePosition = this.source.gameObject.transform.position;
        Vector2 coneDirection = new Vector2(startingPosition.x - sourcePosition.x, startingPosition.y - sourcePosition.y);
        Direction direction = Direction.WEST;

        // We check the direction the cone needs to be checked at

        for (int i = 0; i <= ability.targetArea; i++)
        {
            Vector3Int basePosition = new Vector3Int((int)startPosition.x, (int)startPosition.y, 0);

            // west
            if (direction == Direction.WEST)
            {
                basePosition.x -= i;
                basePosition.y -= i;
            }

            for (int a = 0; a < i * 2 + 1; a++)
            {

                Vector3Int checkPosition = new Vector3Int((int)basePosition.x, (int)basePosition.y, 0);

                // west
                if (direction == Direction.WEST)
                {
                    checkPosition.y += a;
                }

                AddTargetAtPosition(new Vector2(checkPosition.x, checkPosition.y));

            }
        }
    }

    void OnGUI()
    {
        if (canAct && !this.pausedMovement)
        {
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())))
            {

                if (ability.IsAOE())
                {
                    this.SelectAOETargets(ability);
                }
                else if (ability.IsSelfTargeting())
                {
                    this.AddTarget(source);
                }
                else
                {

                    if (ability.targetStartPoint == TargetStartPoint.REGION)
                    {

                        if (ability.targetPolygon == TargetPolygon.RECTANGLE)
                        {
                            CheckRectanglePosition();
                        }
                        else if (ability.targetPolygon == TargetPolygon.CONE)
                        {
                            CheckConePosition();
                        }
                    }
                    else if (ability.targetStartPoint == TargetStartPoint.SELF)
                    {
                        AddTargetAtPosition(this.transform.position);
                    }
                }

            }
        }
    }
}
