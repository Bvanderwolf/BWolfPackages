using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper.Main
{
    public class NicknameForm : MonoBehaviour
    {
        [SerializeField]
        private InputField inputNickname = null;

        private Action<string> finishAction;

        private bool nicknameWasFocused;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

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

        /// <summary>Actives this form by setting it active, focusing on nickname and storing the finish action</summary>
        public void Activate(Action<string> finishAction)
        {
            gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(inputNickname.gameObject);
            this.finishAction = finishAction;
        }

        /// <summary>Cancels this form by setting it inactive</summary>
        public void OnCancel()
        {
            gameObject.SetActive(false);
        }

        /// <summary>Finishes the form by firing the finish action event using the written nickname as argument if it is a valid name</summary>
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