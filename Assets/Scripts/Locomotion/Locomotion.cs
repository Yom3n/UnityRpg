using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Locomotion
{
    enum Direction
    {
        Top,
        Left,
        Right,
        Bottom,
    }

    public class Locomotion : MonoBehaviour
    {
        private PlayerInputAction _playerInputAction;
        private Direction _playerDirection;
        [SerializeField] private float speed = 5;

        private InputAction _getMoveAction()
        {
            return _playerInputAction.Player.Move;
        }

        private void Awake()
        {
            _playerInputAction = new PlayerInputAction();
        }

        private void OnEnable()
        {
            _getMoveAction().Enable();
        }

        private void OnDisable()
        {
            _getMoveAction().Disable();
        }


        private void FixedUpdate()
        {
            var moveInput = _getMoveAction().ReadValue<Vector2>();
            UpdatePlayerDirection(moveInput);


            if (moveInput == new Vector2())
            {
                // No input
                return;
            }

            var moveDirection = GetPlayerDirectionVector();
            transform.position +=
                new Vector3(speed * Time.deltaTime * moveDirection.x, speed * Time.deltaTime * moveDirection.y);
        }

        // Player should be able to move only vertical or only horizontal. No diagonal move is allowed
        private Vector2 GetPlayerDirectionVector()
        {
            return _playerDirection switch
            {
                Direction.Top => new Vector2(0, 1),
                Direction.Left => new Vector2(-1, 0),
                Direction.Right => new Vector2(1, 0),
                Direction.Bottom => new Vector2(0, -1),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void UpdatePlayerDirection(Vector2 moveInput)
        {
            if (moveInput.x > 0)
            {
                _playerDirection = Direction.Right;
            }
            else if (moveInput.x < 0)
            {
                _playerDirection = Direction.Left;
            }
            else if (moveInput.y > 0)
            {
                _playerDirection = Direction.Top;
            }
            else if (moveInput.y < 0)
            {
                _playerDirection = Direction.Bottom;
            }
        }
    }
}