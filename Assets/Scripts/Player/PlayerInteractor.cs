using System;
using Dialog;
using Interactor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private float interactionDistance = 1.2f;
        private PlayerLocomotion _playerLocomotion;
        private PlayerInputAction _playerInputAction;


        private void Awake()
        {
            _playerInputAction = new PlayerInputAction();
            dialogManager.OnDialogStatusChanged += OnDialogStatusChanged;
        }

        void OnDestroy()
        {
            dialogManager.OnDialogStatusChanged -= OnDialogStatusChanged;
        }

        private void OnEnable()
        {
            _getInteractAction().performed += _onPerformActionTapped;
            _getInteractAction().Enable();
        }

        private void OnDisable()
        {
            _getInteractAction().Disable();
            _getInteractAction().performed -= _onPerformActionTapped;
        }


        // Start is called before the first frame update
        void Start()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            if (_playerLocomotion == null)
            {
                throw new MissingComponentException("Locomotion component is missing");
            }
        }


        private void _onPerformActionTapped(InputAction.CallbackContext context)
        {
            Vector2 position = transform.position;
            RaycastHit2D[] hitResults = Physics2D.RaycastAll(
                position,
                _playerLocomotion.GetDirectionVector(),
                interactionDistance);
            foreach (var hit in hitResults)
            {
                if (hit.collider.gameObject == transform.root.gameObject)
                {
                    //Avoid hitting object that shoot ray
                    continue;
                }

                var interactableComponent = hit.collider.gameObject.GetComponent<IInteractable>();
                interactableComponent?.OnInteractionTriggered(transform.root.gameObject);
            }
        }


        private InputAction _getInteractAction()
        {
            return _playerInputAction.Player.Interact;
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