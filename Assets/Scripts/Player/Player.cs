using System;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using UnityEngine;

// Player zarządzający komponentami na podsatwie zmian w dialogu? 
namespace Player
{
    [RequireComponent(typeof(PlayerLocomotion), typeof(PlayerInteractor))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private DialogManager _dialogManager;
        private PlayerState _playerState;

        private void Start()
        {
            SetPlayerState(PlayerState.Adventure);
        }

        private void OnEnable()
        {
            _dialogManager.OnDialogStatusChanged += OnDialogStatusChanged;
        }

        private void OnDisable()
        {
            _dialogManager.OnDialogStatusChanged -= OnDialogStatusChanged;
        }

        private void SetPlayerState(PlayerState newState)
        {
            _playerState = newState;
            PlayerLocomotion playerLocomotion = GetComponent<PlayerLocomotion>();
            if (playerLocomotion == null)
            {
                throw new MissingComponentException("Locomotion is null!");
            }

            switch (newState)
            {
                case PlayerState.Adventure:
                    playerLocomotion.enabled = true;
                    break;
                case PlayerState.InDialog:
                    playerLocomotion.enabled = false;
                    break;
                case PlayerState.InCombat:
                    playerLocomotion.enabled = false;
                    break;
                case PlayerState.Paused:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void OnDialogStatusChanged(DialogManagerStatus status)
        {
            switch (status)
            {
                case DialogManagerStatus.notActive:
                    SetPlayerState(PlayerState.Adventure);
                    break;
                case DialogManagerStatus.canContinue:
                    SetPlayerState(PlayerState.InDialog);
                    break;
                case DialogManagerStatus.canEnd:
                    SetPlayerState(PlayerState.InDialog);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}