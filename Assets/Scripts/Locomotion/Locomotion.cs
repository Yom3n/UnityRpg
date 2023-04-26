using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Locomotion
{
    public class Locomotion : MonoBehaviour
    {
        [SerializeField] private float speed = 5;

        private PlayerInputAction _playerInputAction;
        private Direction _direction;

        public Direction GetDirection()
        {
            return _direction;
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
            return _direction switch
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
                _direction = Direction.Right;
            }
            else if (moveInput.x < 0)
            {
                _direction = Direction.Left;
            }
            else if (moveInput.y > 0)
            {
                _direction = Direction.Top;
            }
            else if (moveInput.y < 0)
            {
                _direction = Direction.Bottom;
            }
        }

        private InputAction _getMoveAction()
        {
            return _playerInputAction.Player.Move;
        }
    }
}