using System;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerLocomotion), typeof(PlayerInteractor))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private DialogManager _dialogManager;
        private PlayerState _playerState;

        private void Awake()
        {
            SetPlayerState(PlayerState.Adventure);
            if (_dialogManager == null)
            {
                throw new MissingComponentException("Player doesnt have DialogManager attached!");
            }
        }

        private void OnEnable()
        {
            _dialogManager.OnStateChanged += OnStatusChanged;
        }

        private void OnDisable()
        {
            _dialogManager.OnStateChanged -= OnStatusChanged;
        }

        private void SetPlayerState(PlayerState newState)
        {
            _playerState = newState;
            PlayerLocomotion playerLocomotion = GetComponent<PlayerLocomotion>();
            PlayerInteractor playerInteractor = GetComponent<PlayerInteractor>();

            switch (newState)
            {
                case PlayerState.Adventure:
                    playerLocomotion.enabled = true;
                    playerInteractor.enabled = true;
                    break;
                case PlayerState.InDialog:
                    playerLocomotion.enabled = false;
                    playerInteractor.enabled = false;
                    break;
                case PlayerState.InCombat:
                    playerLocomotion.enabled = false;
                    playerInteractor.enabled = false;
                    break;
                case PlayerState.Paused:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void OnStatusChanged(DialogManagerStatus status)
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