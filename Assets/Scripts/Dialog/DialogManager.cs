using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Dialog
{
    public enum DialogManagerStatus
    {
        notActive,

        // generatingText,
        canContinue,
        canEnd,
    }


    public class DialogManager : MonoBehaviour
    {
        public event Action<DialogManagerStatus> OnStateChanged;
        private Action<DialogData> OnStoryChanged;

        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI dialogText;

        [SerializeField] private GameObject[] choicesUI;

        private static DialogManager _instance;

        public DialogManagerStatus status;
        [CanBeNull] private Story _currentStory;

        private PlayerInputAction _playerInputAction;


        public void SelectChoice(int choiceIndex)
        {
            if (_currentStory == null)
            {
                throw new Exception("Story is null!");
            }
            
            _currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }

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


        private void ListenForPlayerInput()
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


        public void EnterDialogMode(TextAsset inkJson, Action<DialogData> callback = null)
        {
            if (dialogPanel.activeSelf) return;
            OnStoryChanged = callback;
            ListenForPlayerInput();
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
            if (_currentStory?.currentChoices != null && _currentStory.currentChoices.Count() > 0)
            {
                Debug.Log("Can't continue. Select choice");
                return;
            }

            if (status == DialogManagerStatus.canContinue)
            {
                if (_currentStory == null) return;
                dialogText.text = _currentStory.Continue();
                _DisplayChoices();
                OnStoryChanged?.Invoke(new DialogData(_currentStory.currentText, _currentStory.currentTags
                    ));
                if (_currentStory != null && !_currentStory.canContinue && _currentStory.currentChoices.Count() == 0)
                {
                    SetDialogStatus(DialogManagerStatus.canEnd);
                }
            }
            else
            {
                ExitDialogMode();
            }
        }

        private void _DisplayChoices()
        {
            var choices = _currentStory?.currentChoices;
            if (choicesUI.Length < choices.Count())
            {
                throw new Exception(
                    "Not enough space to show all of the choices!"); // TODO Change to DialogException
            }

            for (int i = 0; i < choices.Count; i++)
            {
                choicesUI[i].GetComponentInChildren<TextMeshProUGUI>().text = choices[i].text;
                StartCoroutine(SelectFirstChoice());
            }

            // Clear text for not used choices
            for (int i = choices.Count(); i < choicesUI.Length; i++)
            {
                choicesUI[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        private void SetDialogStatus(DialogManagerStatus status)
        {
            this.status = status;
            OnStateChanged?.Invoke(status);
        }

        /// <summary>
        /// Selects first choice in the dialog, to let user select choices with arrows
        /// </summary>
        /// <returns></returns>
        private IEnumerator SelectFirstChoice()
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choicesUI[0]);
        }
    }
}