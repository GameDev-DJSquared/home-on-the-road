using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;
    Vector2 lookDir = Vector2.zero;
    PlayerInput playerInput;
    bool interactPressed = false;
    bool backPressed = false;
    bool submitPressed = false;
    bool runPressed = false;
    bool pausePressed = false;

    public static InputManager instance { get; private set; }



    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("MORE THAN ONE INPUT MANAGER. PANIC!");
        }
        instance = this;
        playerInput = GetComponent<PlayerInput>();
    }


    public void MovePressed(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDir = context.ReadValue<Vector2>();
        }

    }

    public void LookPressed(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDir = context.ReadValue<Vector2>();
        }

    }

    public void RunPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            runPressed = true;
        }
        else if (context.canceled)
        {
            runPressed = false;
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            interactPressed = true;
        } else if(context.canceled)
        {
            interactPressed = false;
        }
    }


    public void SwitchActions(int map)
    {
        switch (map)
        {
            case 0:
                playerInput.SwitchCurrentActionMap("Main");
                break;
            case 1:
                playerInput.SwitchCurrentActionMap("UI");
                break;
            default:
                Debug.LogWarning("Warning: SwitchActions given invalid index");
                break;
        }


    }

    public Vector2 GetMoveDir()
    {
        return moveDir;
    }

    public bool GetInteractPressed()
    {
        bool value = interactPressed;
        interactPressed = false;
        return value;
    }

    public bool GetSubmitPressed()
    {
        bool value = submitPressed;
        submitPressed = false;
        return value;
    }



    public bool GetRunPressed()
    {

        return runPressed;
    }


}
