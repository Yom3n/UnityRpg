using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Interactor
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 1.2f;
        private Locomotion.Locomotion _locomotion;
        private PlayerInputAction _playerInputAction;


        private void Awake()
        {
            _playerInputAction = new PlayerInputAction();
            _getInteractAction().performed += _onPerformActionTapped;
        }

        void OnDestroy()
        {
            _getInteractAction().performed -= _onPerformActionTapped;
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
        }


        private void _onPerformActionTapped(InputAction.CallbackContext context)
        {
            Vector2 position = transform.position;
            RaycastHit2D[] hitResults = Physics2D.RaycastAll(
                position,
                _locomotion.GetDirectionVector(),
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
    }
}