using System.Collections.Generic;
using UnityEngine;

public class AbilityMenu : MonoBehaviour
{

    public static AbilityMenu instance;
    private List<AbilitySlot> abilities = new List<AbilitySlot>();
    private Vector2 sourcePosition;
    private int index = 0;

    private bool isMoving = false;
    private bool canAct = false;
    private bool initialized = false;

    private float movementDelay = 0.2f;

    // Start is called before the first frame update
    void Start()
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

    public static void UnLoad()
    {

        foreach (Transform child in instance.transform)
        {
            Destroy(child.gameObject);
        }

        instance.index = 0;
        instance.abilities.Clear();
        instance.canAct = false;
        instance.initialized = false;
        instance.gameObject.SetActive(false);
    }

    public static void Load(List<Ability> abilities, Vector2 sourcePosition)
    {

        instance.sourcePosition = sourcePosition;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Ui/AbilitySlot");

        foreach (Ability abilty in abilities)
        {
            GameObject newObject = Instantiate(prefab, instance.transform);

            AbilitySlot slot = newObject.GetComponent<AbilitySlot>();
            slot.ability = abilty;
            instance.abilities.Add(slot);
        }

        instance.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 55 * abilities.Count);
        instance.canAct = true;
        instance.gameObject.SetActive(true);
        instance.DrawSelectedAbilityReach();
        instance.abilities[instance.index].Hover();
    }

    // Update is called once per frame
    void Update()
    {

        if (abilities.Count > 0)
        {

            if (isMoving || !canAct) return;

            int vertical = 0;
            vertical = (int)(Input.GetAxisRaw("Vertical")) * -1;

            if (vertical != 0)
            {
                isMoving = true;

                abilities[index].Unhover();
                index += vertical;

                if (index >= abilities.Count)
                {
                    index = 0;
                }
                else if (index < 0)
                {
                    index = this.abilities.Count - 1;
                }

                abilities[index].Hover();
                DrawSelectedAbilityReach();

                Invoke("ResetMovement", movementDelay);
            }
        }
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())))
        {
            // We choose the ability and need to now select targets
            canAct = !canAct;
            abilities[index].Select();
            Battle.SetState(BattleState.TARGETSELECTION);
        }

        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
        {
            if (!canAct)
            {
                abilities[index].Deselect();
            }
            else
            {
                AbilityMenu.UnLoad();
            }
        }
    }

    private void ResetMovement()
    {
        isMoving = false;
    }

    private void DrawSelectedAbilityReach()
    {

        Ability ability = GetSelectedAbility();

        if (ability != null)
        {

            GridTools.ClearTargetTileMap();

            if (ability.targetPolygon == TargetPolygon.RECTANGLE)
            {
                Vector2 startPosition = new Vector2(sourcePosition.x - ability.reach, sourcePosition.y + ability.reach);
                GridTools.DrawReach(startPosition, sourcePosition, ability.reach);
            }
            else if (ability.targetPolygon == TargetPolygon.CONE)
            {
                GridTools.DrawCone(sourcePosition, Direction.WEST, ability.targetArea);
            }
        }
    }

    public static Ability GetSelectedAbility()
    {

        if (instance.abilities[instance.index].selected)
        {
            return instance.abilities[instance.index].ability;
        }
        else
        {
            return null;
        }
    }
}
