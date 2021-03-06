﻿using UnityEngine;

public class TurnOrderObject : MovingObject
{
    /** Tilemaps **/
    protected LayerMask unitLayer;
    public int remainingMovementSpeed;
    public bool limitedMovement = true;

    // If its the turn of the unit
    public bool canAct = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.unitLayer = LayerMask.GetMask("UnitsGround");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!canAct) return;

        base.Update();
    }

    public virtual void AfterTurn()
    {
        // See inheritation
    }

    public virtual void BeforeTurn()
    {
        this.canAct = true;
        this.pausedMovement = false;
    }

    protected RaycastHit2D rayCastToUnit(Vector2 end)
    {
        boxCollider.enabled = false;
        return Physics2D.Linecast(transform.position, end, unitLayer);
    }

    protected RaycastHit2D UnitAtPosition(Vector2 position)
    {
        boxCollider.enabled = false;
        return Physics2D.Linecast(position, position, unitLayer);
    }

    protected bool IsUnitAtPosition(Vector2 position)
    {
        return UnitAtPosition(position).transform != null;
    }

    protected bool hitsUnit(Vector2 end)
    {
        bool hit = rayCastToUnit(end).transform != null;
        boxCollider.enabled = true;
        return hit;
    }

    protected override void resetMovement()
    {
        base.resetMovement();

        if (!lastMoveFailed)
        {
            remainingMovementSpeed--;
        }

        if (limitedMovement && remainingMovementSpeed <= 0)
        {
            this.pausedMovement = true;
        }
    }

}
