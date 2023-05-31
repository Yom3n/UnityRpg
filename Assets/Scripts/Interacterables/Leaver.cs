using Dialog;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactor
{
    public class Leaver : MonoBehaviour, IInteractable
    {
        public GameObject SwichableGameObject;
        [SerializeField] private TextAsset dialogAsset;

        public void Toggle()
        {
            DialogManager.GetInstance().EnterDialogMode(dialogAsset);
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !spriteRenderer.flipX;
            var client = SwichableGameObject.GetComponent<ISwitchable>();

            if (client.IsActive)
            {
                client.OnDeactivate();
         
            }
            else
            {
                client.OnActivate();
            }
        }

        public void OnInteractionTriggered(GameObject gameObject)
        {
            Toggle();
        }
    }
}