using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private Action<Choice> OnChoiceSelected;

        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI dialogText;

        [SerializeField] private TextMeshProUGUI[] choicesTexts;

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

            OnChoiceSelected.Invoke(_currentStory.currentChoices[choiceIndex]);
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

        private void OnEnable()
        {
            _playerInputAction.Enable();
            StartCoroutine(DelayedActivatePlayerInputCorutine());

        }

        private IEnumerator DelayedActivatePlayerInputCorutine() //Nah, it doesnt work. Can be deleted
        {
            yield return new WaitForEndOfFrame();
            _playerInputAction.Player.Interact.performed += OnContinueTap;
            StopCoroutine(DelayedActivatePlayerInputCorutine());
            
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


        public void EnterDialogMode(TextAsset inkJson, Action<Choice> callback = null)
        {
            if (dialogPanel.activeSelf) return;
            SetDialogStatus(DialogManagerStatus.canContinue);
            _currentStory = new Story(inkJson.text);
            dialogPanel.SetActive(true);
            ContinueStory();
            OnChoiceSelected = callback;
        }

        private void ExitDialogMode()
        {
            dialogPanel.SetActive(false);
            dialogText.text = "";
            SetDialogStatus(DialogManagerStatus.notActive);
        }

        private void ContinueStory()
        {
            if (_currentStory?.currentChoices != null && _currentStory.currentChoices.Count() != 0)
            {
                Debug.Log("Can't continue. Select choice");
                return;
            }

            if (status == DialogManagerStatus.canContinue)
            {
                if (_currentStory == null) return;
                dialogText.text = _currentStory.Continue();
                var choices = _currentStory?.currentChoices;
                if (choicesTexts.Length < choices.Count())
                {
                    throw new Exception(
                        "Not enough space to show all of the choices!"); // TODO Change to DialogException
                }

                for (int i = 0; i < choices.Count; i++)
                {
                    choicesTexts[i].text = choices[i].text;
                }

                // Clear text for not used choices
                for (int i = choices.Count(); i < choicesTexts.Length; i++)
                {
                    choicesTexts[i].text = "";
                }


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
            this.status = status;
            OnStateChanged?.Invoke(status);
        }
    }
}