using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locomotion : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;
    private InputAction move;
    [SerializeField] private float speed = 5;

    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        move = _playerInputAction.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        Vector2 moveDirection = move.ReadValue<Vector2>();
        transform.position +=
            new Vector3(speed * Time.deltaTime * moveDirection.x, speed * Time.deltaTime * moveDirection.y);
    }
}