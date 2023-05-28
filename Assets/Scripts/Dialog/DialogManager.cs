using System;
using Ink.Runtime;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        public event Action<DialogManagerStatus> OnDialogStatusChanged;

        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI dialogText;
        private static DialogManager _instance;

        public DialogManagerStatus status;
        [CanBeNull] private Story _currentStory;

        private PlayerInputAction _playerInputAction;

        private void Awake()
        {
            _playerInputAction = new PlayerInputAction();
            if (_instance == null)
            {
                _instance = this;
            }

            dialogPanel.SetActive(false);
            dialogText.text = "";
            _currentStory = null;
            SetDialogStatus(DialogManagerStatus.notActive);
        }

        private void OnEnable()
        {
            _playerInputAction.Enable();
            _playerInputAction.Player.Interact.performed += OnContinueTap;
        }

        private void OnDisable()
        {
            _playerInputAction.Disable();
            _playerInputAction.Player.Interact.performed -= OnContinueTap;
        }

        private void OnContinueTap(InputAction.CallbackContext context)
        {
            if (status == DialogManagerStatus.notActive) return;
            ContinueStory();
        }


        public static DialogManager GetInstance()
        {
            return _instance;
        }


        public void EnterDialogMode(TextAsset inkJson)
        {
            if (dialogPanel.activeSelf) return;
            SetDialogStatus(DialogManagerStatus.canContinue);
            _currentStory = new Story(inkJson.text);
            dialogPanel.SetActive(true);
            ContinueStory();
        }

        private void ExitDialogMode()
        {
            dialogPanel.SetActive(false);
            dialogText.text = "";
            SetDialogStatus(DialogManagerStatus.notActive);
        }

        private void ContinueStory()
        {
            if (status == DialogManagerStatus.canContinue)
            {
                if (_currentStory == null) return;
                _currentStory.Continue();
                dialogText.text = _currentStory?.currentText ?? "";
                if (_currentStory != null && !_currentStory.canContinue)
                {
                    SetDialogStatus(DialogManagerStatus.canEnd);
                }
            }
            else
            {
                ExitDialogMode();
            }
        }

        private void SetDialogStatus(DialogManagerStatus status)
        {
            print("Changind dialog status to:");
            print(status);
            this.status = status;
            OnDialogStatusChanged?.Invoke(status);
        }
    }
}