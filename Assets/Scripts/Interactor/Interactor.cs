using UnityEngine;

namespace Interactor
{
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// Collider that activates on "Interact" tap
        /// </summary>
        [SerializeField] Collider2D InteractorColliderTop;

        [SerializeField] Collider2D InteractorColliderRight;
        [SerializeField] Collider2D InteractorColliderLeft;
        [SerializeField] Collider2D InteractorColliderBottom;

        private Locomotion.Locomotion _locomotion;

        // Start is called before the first frame update
        void Start()
        {
            _locomotion = GetComponent<Locomotion.Locomotion>();
            if (_locomotion == null)
            {
                throw new MissingComponentException("Locomotion component is missing");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}