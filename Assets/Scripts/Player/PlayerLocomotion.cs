using System;
using Dialog;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerLocomotion : MonoBehaviour
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private float speed = 5;

        private PlayerInputAction _playerInputAction;
        private Direction _direction = new();


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
            dialogManager.OnDialogStatusChanged += OnDialogStatusChanged;
        }

        private void OnEnable()
        {
            _getMoveAction().Enable();
        }

        private void OnDisable()
        {
            _getMoveAction().Disable();
        }

        public void OnDestroy()
        {
            dialogManager.OnDialogStatusChanged -= OnDialogStatusChanged;
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

        private void OnDialogStatusChanged(DialogManagerStatus status)
        {
            switch (status)
            {
                case DialogManagerStatus.notActive:
                    enabled = true;
                    break;
                case DialogManagerStatus.canContinue:
                    enabled = false;
                    break;
                case DialogManagerStatus.canEnd:
                    enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}