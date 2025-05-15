using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;
    Vector2 lookDir = Vector2.zero;
    Vector2 scrollDir = Vector2.zero;
    PlayerInput playerInput;
    bool interactPressed = false;
    bool backPressed = false;
    bool submitPressed = false;
    bool runPressed = false;
    bool pausePressed = false;
    bool dropPressed = false;
    bool slot1Pressed = false;
    bool slot2Pressed = false;
    bool slot3Pressed = false;
    bool slot4Pressed = false;
    bool slot5Pressed = false;
    bool restartPressed = false;
    bool firePressed = false;

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

    public void DropPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dropPressed = true;
        }
        else if (context.canceled)
        {
            dropPressed = false;
        }
    }

    public void Slot1Pressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            slot1Pressed = true;
        }
        else if (context.canceled)
        {
            slot1Pressed = false;
        }
    }

    public void Slot2Pressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            slot2Pressed = true;
        }
        else if (context.canceled)
        {
            slot2Pressed = false;
        }
    }

    public void Slot3Pressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            slot3Pressed = true;
        }
        else if (context.canceled)
        {
            slot3Pressed = false;
        }
    }

    public void Slot4Pressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            slot4Pressed = true;
        }
        else if (context.canceled)
        {
            slot4Pressed = false;
        }
    }

    public void Slot5Pressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            slot5Pressed = true;
        }
        else if (context.canceled)
        {
            slot5Pressed = false;
        }
    }


    public void Scrolled(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            scrollDir = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            scrollDir = Vector2.zero;
        }
    }

    public void RestartPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            restartPressed = true;
        }
        else if (context.canceled)
        {
            restartPressed = false;
        }
    }


    public void PausePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pausePressed = true;
        }
        else if (context.canceled)
        {
            pausePressed = false;
        }
    }

    public void FirePressed(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            firePressed = true;
        } else if(context.canceled)
        {
            firePressed = false;
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

    public bool GetDropPressed()
    {
        bool value = dropPressed;
        dropPressed = false;
        return value;
    }

    public bool GetSlot1Pressed()
    {
        bool value = slot1Pressed;
        slot1Pressed = false;
        return value;
    }

    public bool GetSlot2Pressed()
    {
        bool value = slot2Pressed;
        slot2Pressed = false;
        return value;
    }
    public bool GetSlot3Pressed()
    {
        bool value = slot3Pressed;
        slot3Pressed = false;
        return value;
    }

    public bool GetSlot4Pressed()
    {
        bool value = slot4Pressed;
        slot4Pressed = false;
        return value;
    }

    public bool GetSlot5Pressed()
    {
        bool value = slot5Pressed;
        slot5Pressed = false;
        return value;
    }

    public Vector2 GetScrollDir()
    {
        Vector2 value = scrollDir;
        scrollDir = Vector2.zero;
        return value;
    }

    public bool GetRunPressed()
    {

        return runPressed;
    }

    public bool GetFirePressed()
    {
        bool value = firePressed;
        firePressed = false;
        return value;
    }

    public bool GetRestartPressed()
    {
        return restartPressed;
    }

    public bool GetPausePressed()
    {
        bool value = pausePressed;
        pausePressed = false;
        return value;
    }

}
