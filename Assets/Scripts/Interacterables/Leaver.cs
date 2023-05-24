using Dialog;
using Ink.Runtime;
using UnityEngine;

namespace Interactor
{
    public class Leaver : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextAsset dialogAsset;


        public void OnInteractionTriggered(GameObject other)
        {
            DialogManager.GetInstance().EnterDialogMode(dialogAsset);
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}