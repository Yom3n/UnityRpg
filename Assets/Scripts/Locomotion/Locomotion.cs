using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Locomotion
{
    public class Locomotion : MonoBehaviour
    {
        [SerializeField] private float speed = 5;

        private PlayerInputAction _playerInputAction;
        private Direction _direction = new Direction();

        public Direction GetDirection()
        {
            return _direction;
        }

        public Vector2 GetDirectionVector()
        {
            return _direction.GetDirectionVector();
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
            _direction.SetValue(moveInput);

            if (moveInput == new Vector2())
            {
                // No input
                return;
            }

            var moveDirectionVector = _direction.GetDirectionVector();
            transform.position +=
                new Vector3(speed * Time.deltaTime * moveDirectionVector.x,
                    speed * Time.deltaTime * moveDirectionVector.y);
        }


        private InputAction _getMoveAction()
        {
            return _playerInputAction.Player.Move;
        }
    }
}