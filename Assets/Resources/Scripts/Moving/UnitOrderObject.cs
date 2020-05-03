using UnityEngine;

public class UnitOrderObject : TurnOrderObject
{

    public Unit unit;
    public GameObject vision;

    public int rolledInit;

    protected override bool canMove(Vector2 targetCell)
    {
        return base.canMove(targetCell) && !hitsUnit(targetCell);
    }

    public override void AfterTurn()
    {
        base.AfterTurn();
        Destroy(vision);
    }

    public override void BeforeTurn()
    {
        base.BeforeTurn();
        // if the unit is stunned we skip our turn
        if (unit.IsStunned())
        {
            canAct = false;
        }
        this.remainingMovementSpeed = this.unit.encounterStats.speed;
        this.unit.hasStandardAction = true;

        CreateVision();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (vision != null)
        {
            vision.transform.position = gameObject.transform.position;
        }
    }

    public void RollInitiative()
    {
        this.rolledInit = (int)Random.Range(1, 20 + this.unit.encounterStats.init);
    }


    public void CreateVision()
    {
        if (unit.visions.Count > 0)
        {

            GameObject visionObject = null;

            if (unit.visions.Contains(Vision.DARKVISION))
            {
                visionObject = Resources.Load<GameObject>("Prefabs/Vision/Darkvision");
            }

            if (visionObject != null)
            {
                vision = Instantiate(visionObject, gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}
