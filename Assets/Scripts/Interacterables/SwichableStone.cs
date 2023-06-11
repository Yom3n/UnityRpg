using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Interactor
{
    public class SwichableStone : MonoBehaviour, ISwitchable
    {
        public bool IsActive { get; set; }

        public void OnActivate()
        {
            IsActive = true;
            gameObject.SetActive(false);
            Debug.Log("Opening doors");
        }

        public void OnDeactivate()
        {
            IsActive = false;
            Debug.Log("Closing doors");
        }
    }
}