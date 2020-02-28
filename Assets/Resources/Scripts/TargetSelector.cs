using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetSelector : TurnOrderObject
{
    [HideInInspector]
    public List<UnitOrderObject> selectedTargets = new List<UnitOrderObject>();

    private Ability ability;
    private UnitOrderObject source;

    private Vector2 startingPosition;

    public Tilemap selectedTargetsTileMap;
    public TileBase selectedTargetTileBase;

    // At least one target and correct number of targets
    public bool TargetSelectionValid()
    {
        return selectedTargets.Count > 0 && (selectedTargets.Count >= ability.minTargets || ability.minTargets == null || ability.maxTargets == null);
    }

    public void StartTargetSelection(UnitOrderObject source, Ability ability, Vector2 startingPosition, List<UnitOrderObject> participants)
    {
        this.source = source;
        this.source.pausedMovement = true;
        this.pausedMovement = false;
        this.ability = ability;
        this.startingPosition = startingPosition;
        this.transform.position = startingPosition;
        this.gameObject.SetActive(true);

        // If its an AOE Effect where we dont need to target anybody
        if (ability.minTargets == null && ability.maxTargets == null)
        {
            // Check the distance for everybody and set the list of the targetselector to the units we got
            foreach (UnitOrderObject unitOrderObject in participants)
            {
                float distance = Vector2.Distance(startingPosition, unitOrderObject.transform.position);
                if (distance <= ability.reach + 1)
                {
                    selectedTargets.Add(unitOrderObject);
                }
            }
        }
        // If we can only target ourself
        else if (ability.reach == 0)
        {
            selectedTargets.Add(source);
        }
        else
        {
            // Draw where targets can be selected
            Vector2 startPosition = new Vector2(source.transform.position.x - ability.reach, source.transform.position.y + ability.reach);

            if (ability.targetPolygon == TargetPolygon.RECTANGLE)
            {
                MapTools.DrawReach(startPosition, startingPosition, ability.reach);
            }
        }
    }

    public void StopAbilityExecution()
    {
        this.selectedTargets.Clear();
        this.pausedMovement = false;
    }

    public void AllowAbilityExecution()
    {
        this.pausedMovement = true;
    }

    public Ability EndTargetSelection()
    {
        this.pausedMovement = true;
        this.gameObject.SetActive(false);
        this.selectedTargets.Clear();
        this.selectedTargetsTileMap.ClearAllTiles();
        return this.ability;
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

            Vector3Int toCheckPosition3Int = new Vector3Int((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.y, 0);

            // Check if we already selected the target thus removing it
            if (this.selectedTargets.Contains(selectedUnit))
            {
                this.selectedTargets.Remove(selectedUnit);
                MapTools.placeTile(null, selectedTargetsTileMap, toCheckPosition3Int);
            }

            // If the list is not full already or we have no limit
            else if (this.selectedTargets.Count < ability.maxTargets || ability.maxTargets == null)
            {
                this.selectedTargets.Add(selectedUnit);
                MapTools.placeTile(selectedTargetTileBase, selectedTargetsTileMap, toCheckPosition3Int);
            }
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
        if (this.canAct && !this.pausedMovement)
        {
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())))
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

            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
            {
                this.StopAbilityExecution();
            }
        }
    }
}
