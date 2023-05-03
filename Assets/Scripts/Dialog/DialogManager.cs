using System;
using Ink.Runtime;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private TextMeshProUGUI dialogText;
        private static DialogManager _instance;

        private Story _currentStory;
        private bool _dialogIsPlaying;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static DialogManager GetInstance()
        {
            return _instance;
        }

        private void Start()
        {
            _dialogIsPlaying = false;
            dialogPanel.SetActive(false);
        }

        public void EnterDialogMode(TextAsset inkJson)
        {
            _currentStory = new Story(inkJson.text);
            _dialogIsPlaying = true;
            dialogPanel.SetActive(true);
            ContinueStory();
        }

        private void ExitDialogMode()
        {
            _dialogIsPlaying = false;
            dialogPanel.SetActive(false);
            dialogText.text = "";
        }

        private void ContinueStory()
        {
            if (_currentStory.canContinue)
            {
                _currentStory.Continue();
            }
            else
            {
                ExitDialogMode();
            }
        }
    }
}