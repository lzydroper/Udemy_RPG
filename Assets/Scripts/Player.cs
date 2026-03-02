using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 PlayerInput { get; private set; } 
    public PlayerInputSet InputSet { get; private set; }

    private void Awake()
    {
        InputSet = new PlayerInputSet();
    }

    private void OnEnable()
    {
        InputSet.Enable();
        
        InputSet.Player.Movement.performed += ctx => PlayerInput = ctx.ReadValue<Vector2>();
        InputSet.Player.Movement.canceled += ctx => PlayerInput = Vector2.zero;
    }

    private void OnDisable()
    {
        InputSet.Disable();
    }
}