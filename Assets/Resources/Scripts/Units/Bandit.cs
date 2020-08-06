﻿using UnityEngine;

public class Bandit : Npc
{
    private MovingObject movingObject;
    public MovementDirection movementDirection;
    public int fieldsOfView = 6;

    private BoxCollider2D boxCollider2D;
    private bool surprised = false;
    private bool isWatching = true;

    private GameObject target;

    void Surprise(GameObject gameObject)
    {
        surprised = true;
        isWatching = false;
        PlayerController.SetCanMove(false);
        target = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        movingObject = GetComponent<MovingObject>();
        boxCollider2D.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 rayTarget = gameObject.transform.position + (GetRayDirection() * fieldsOfView);
        Debug.DrawLine(gameObject.transform.position, rayTarget);

        if (!surprised && isWatching)
        {
            Watch();
        }
        else if (surprised && !isWatching)
        {

            Vector2 newPosition = Vector2.MoveTowards(transform.position, target.transform.position, 2f * Time.deltaTime);
            transform.position = newPosition;
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance <= 1)
            {
                target = null;
                surprised = false;
                isWatching = false;

                DialogControl.StartDialog(dialogId);
            }
        }
    }

    void WalkTo(GameObject gameObject)
    {

    }

    GameObject Watch()
    {

        RaycastHit2D raycastHit2D = Physics2D.Raycast(gameObject.transform.position + GetRayOffset(), GetRayDirection(), fieldsOfView, 1 << 9);

        if (raycastHit2D.collider != null)
        {
            Surprise(raycastHit2D.collider.gameObject);
        }


        return null;
    }

    Vector3 GetRayOffset()
    {
        if (movementDirection == MovementDirection.EAST)
        {
            return new Vector2(0.5f, 0f);
        }
        else if (movementDirection == MovementDirection.WEST)
        {
            return new Vector2(-0.5f, 0f);
        }
        else if (movementDirection == MovementDirection.NORTH)
        {
            return new Vector2(0, 0.5f);
        }
        else if (movementDirection == MovementDirection.SOUTH)
        {
            return new Vector2(0, -0.5f);
        }
        else
        {
            return new Vector2();
        }
    }

    Vector3 GetRayDirection()
    {

        if (movementDirection == MovementDirection.EAST)
        {
            return Vector2.right;
        }
        else if (movementDirection == MovementDirection.WEST)
        {
            return Vector2.left;
        }
        else if (movementDirection == MovementDirection.NORTH)
        {
            return Vector2.up;
        }
        else if (movementDirection == MovementDirection.SOUTH)
        {
            return Vector2.down;
        }
        else
        {
            return new Vector2();
        }
    }
}
