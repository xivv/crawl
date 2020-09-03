using UnityEngine;

public class KeyController : MonoBehaviour
{
    public static KeyController instance;
    private KeyState keyState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        keyState = GetKeyState();
    }

    void OnGUI()
    {

        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))
        {
            if (keyState == KeyState.WORLDMAP)
            {
                MainMenu.OpenMenu();
            }
            else if (keyState == KeyState.ABILITYMENU)
            {
                if (AbilityMenu.CanAct())
                {
                    Battle.SetState(BattleState.ACTION);
                }
                else
                {
                    Battle.SetState(BattleState.ABILITYSELECTION);
                }
            }
            else if (keyState == KeyState.MENU)
            {
                MainMenu.HideMenu();
            }
        }

        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.KeypadEnter.ToString())) || Event.current.Equals(Event.KeyboardEvent(KeyCode.Return.ToString())))
        {
            if (keyState == KeyState.WORLDMAP)
            {
                PlayerController.Interact();
            }
            else if (keyState == KeyState.ABILITYMENU || AbilityMenu.CanAct())
            {
                Battle.SetState(BattleState.TARGETSELECTION);
            }
            else if (keyState == KeyState.DIALOG)
            {
                DialogControl.Progress();
            }
            else if (keyState == KeyState.TARGETSELECTOR)
            {
                TargetSelector.CheckForTarget();
            }
        }

    }


    public static KeyState GetKeyState()
    {
        if (AbilityMenu.IsKeyBlocking())
        {
            return KeyState.ABILITYMENU;
        }
        else if (DialogControl.IsKeyBlocking())
        {
            return KeyState.DIALOG;
        }
        else if (MainMenu.IsKeyBlocking())
        {
            return KeyState.MENU;
        }
        else if (TargetSelector.IsKeyBlocking())
        {
            return KeyState.TARGETSELECTOR;
        }



        return KeyState.WORLDMAP;
    }

}


public enum KeyState
{
    WORLDMAP,
    ABILITYMENU,
    DIALOG,
    MENU,
    TARGETSELECTOR
}
