using UnityEngine;

namespace Interactor
{
    public interface IInteractable
    {
        void OnInteractionTriggered(GameObject gameObject);
    }
}