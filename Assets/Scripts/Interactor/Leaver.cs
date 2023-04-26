using UnityEngine;

namespace Interactor
{
    public class Leaver : MonoBehaviour, IInteractable
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnInteractionTriggered(GameObject gameObject)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}