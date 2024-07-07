using System;
using UnityEngine;

public class InputSystem
{
    private Controls controls;

    public event Action Paused;

    public event Action Acted;
    
    public Vector2 Move => controls.Player.Move.ReadValue<Vector2>();


    public InputSystem()
    {
        controls = new Controls();
        controls.Player.Enable();

        controls.Player.Act.performed += Act_performed;
        controls.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Paused?.Invoke();
    }

    private void Act_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ToString());
        Acted?.Invoke();
    }
}