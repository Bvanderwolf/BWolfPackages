using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class NicknameForm : MonoBehaviour
    {
        [SerializeField]
        private InputField inputNickname = null;

        private Action<string> finishAction;

        private bool nicknameWasFocused;

        private void OnDisable()
        {
            finishAction = null;
            inputNickname.text = string.Empty;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && nicknameWasFocused)
            {
                OnFinish();
            }

            nicknameWasFocused = inputNickname.isFocused;
        }

        public void Activate(Action<string> finishAction)
        {
            gameObject.SetActive(true);
            this.finishAction = finishAction;
            EventSystem.current.SetSelectedGameObject(inputNickname.gameObject);
        }

        public void OnCancel()
        {
            gameObject.SetActive(false);
        }

        public void OnFinish()
        {
            string name = inputNickname.text;

            if (!string.IsNullOrEmpty(name))
            {
                finishAction(name);
                gameObject.SetActive(false);
            }
        }
    }
}