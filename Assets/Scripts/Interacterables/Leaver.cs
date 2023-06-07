using System;
using Dialog;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactor
{
    public class Leaver : MonoBehaviour, IInteractable
    {
        public GameObject[] swichableGameObjects;

        [SerializeField] private TextAsset dialogAsset;

        private void Start()
        {
            foreach (var swichable in swichableGameObjects)
            {
                if (swichable.GetComponent<ISwitchable>() == null)
                {
                    throw new MissingComponentException(
                        $"{swichable.name} added to {this.name} does not have ISwichable interface!");
                }
            }
        }

        private void Toggle()
        {
            DialogManager.GetInstance().EnterDialogMode(dialogAsset,  OnChoiceSelected);
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = !spriteRenderer.flipX;

            foreach (var swichableGameObject in swichableGameObjects)
            {
                var client = swichableGameObject.GetComponent<ISwitchable>();
                if (client.IsActive)
                {
                    client.OnDeactivate();
                }
                else
                {
                    client.OnActivate();
                }
            }
        }

        void OnChoiceSelected(Choice dialogChoice)
        {
            // if(dialogChoice.)
        }

        public void OnInteractionTriggered(GameObject gameObject)
        {
            Toggle();
        }
    }
}