using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]

public class InputReader : MonoBehaviour
{

    [SerializeField] PlayerInput pInput;
    [SerializeField] float doubleTapTime = 0.5f;
    InputAction moveAction;


    float lastMoveTime;
    float lastMoveDirection;

    public event Action LeftTap;
    public event Action RightTap;

    public Vector2 Move => moveAction.ReadValue<Vector2>();


    private void OnValidate()
    {
        //this.ValidateRefs();
    }

    private void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        moveAction = pInput.actions["Move"];
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
    }

    void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        float currentDirection = Move.x;
        if(Time.time - lastMoveTime < doubleTapTime && currentDirection == lastMoveDirection)
        {
            if(currentDirection < 0)
            {
                LeftTap?.Invoke();
            }
            else if(currentDirection > 0)
            {
                RightTap?.Invoke();
            }
        }

        lastMoveTime = Time.time;
        lastMoveDirection = currentDirection;

    }
}
