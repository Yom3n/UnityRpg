using System;
using System.Linq;
using Dialog;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactor
{
    public class Leaver : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// Whenever this tag will be used in dialog, lever will be activated
        /// </summary>
        [SerializeField] private String toggleTagName = "action:lever_moved";

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

        public void OnInteractionTriggered(GameObject gameObject)
        {
            DialogManager.GetInstance().EnterDialogMode(dialogAsset, OnStoryChanged);
        }

        void OnStoryChanged(DialogData data)
        {
            if (data.tags.Count == 0) return;
            if (data.tags.Any(dialogTag => dialogTag == toggleTagName))
            {
                Toggle();
            }
        }

        private void Toggle()
        {
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
    }
}