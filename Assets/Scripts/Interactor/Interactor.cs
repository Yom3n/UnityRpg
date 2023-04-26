using System;
using System.Collections;
using Locomotion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactor
{
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// Collider that activates on "Interact" tap
        /// </summary>
        [SerializeField] private Collider2D interactorColliderTop;

        [SerializeField] private Collider2D interactorColliderRight;
        [SerializeField] private Collider2D interactorColliderLeft;
        [SerializeField] private Collider2D interactorColliderBottom;

        private Locomotion.Locomotion _locomotion;
        private PlayerInputAction _playerInputAction;


        private void Awake()
        {
            _playerInputAction = new PlayerInputAction();
        }

        private void OnEnable()
        {
            _getInteractAction().Enable();
        }

        private void OnDisable()
        {
            _getInteractAction().Disable();
        }

        // Start is called before the first frame update
        void Start()
        {
            _locomotion = GetComponent<Locomotion.Locomotion>();
            if (_locomotion == null)
            {
                throw new MissingComponentException("Locomotion component is missing");
            }

            _disableAllColliders();
        }


        // Update is called once per frame
        void Update()
        {
            if (_getInteractAction().WasPerformedThisFrame())
            {
                var direction = _locomotion.GetDirection();
                var colliderToActivate = GetColliderForDirection(direction);
                colliderToActivate.gameObject.SetActive(true);
                StartCoroutine(DelayedDisableCollidersCoroutine());
            }
        }

        private IEnumerator DelayedDisableCollidersCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            _disableAllColliders();
        }

        private Collider2D GetColliderForDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return interactorColliderTop;
                case Direction.Left:
                    return interactorColliderLeft;
                case Direction.Right:
                    return interactorColliderRight;
                case Direction.Bottom:
                    return interactorColliderBottom;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void _disableAllColliders()
        {
            interactorColliderTop.gameObject.SetActive(false);
            interactorColliderRight.gameObject.SetActive(false);
            interactorColliderLeft.gameObject.SetActive(false);
            interactorColliderBottom.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                var interactable = other.GetComponent<IInteractable>();
                interactable?.OnInteractionTriggered(this.GameObject());
            }
        }

        private InputAction _getInteractAction()
        {
            return _playerInputAction.Player.Interact;
        }
    }
}