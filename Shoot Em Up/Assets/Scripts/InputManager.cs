using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerInput inputActions;

    public InputAction Move;
    public InputAction Special;
    public InputAction Fire;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        inputActions = GetComponent<PlayerInput>();
        
        Move = inputActions.actions.FindAction("Move");
        Special = inputActions.actions.FindAction("Special");
        Fire = inputActions.actions.FindAction("Fire");
    }
}
