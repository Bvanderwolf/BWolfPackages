using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class JoinRoomForm : MonoBehaviour
    {
        [SerializeField]
        private InputField inputPassword = null;

        private string requiredKey;
        private string nameOfRoom;

        private event Action<bool, string> OnFinished;

        private bool passwordWasFocused;

        private const string incorrectPasswordText = "wrong password";

        private void OnEnable()
        {
            requiredKey = string.Empty;
            nameOfRoom = string.Empty;
            inputPassword.text = string.Empty;

            EventSystem.current.SetSelectedGameObject(inputPassword.gameObject);
        }

        private void Update()
        {
            //using the return key you can also finish the join room form
            if (Input.GetKeyDown(KeyCode.Return) && passwordWasFocused)
            {
                FinishWithPasswordInput();
            }

            passwordWasFocused = inputPassword.isFocused;
        }

        /// <summary>Adds listener to on finished event</summary>
        public void AddListener(Action<bool, string> onFinished)
        {
            OnFinished += onFinished;
        }

        /// <summary>removes listener to on finished event</summary>
        public void RemoveListener(Action<bool, string> onFinished)
        {
            OnFinished -= onFinished;
        }

        /// <summary>Adds the required key to be stored and checked by this form</summary>
        public void AddRequiredKey(string key)
        {
            requiredKey = key;
        }

        /// <summary>Adds the name of the room to be send back on finish</summary>
        public void AddNameOfRoom(string name)
        {
            nameOfRoom = name;
        }

        /// <summary>Called when the finish button has been clicked to finish with password input</summary>
        public void OnFinishButtonClick()
        {
            FinishWithPasswordInput();
        }

        /// <summary>called when the cancel button is clicked to cancel this form</summary>
        public void OnCancel()
        {
            OnFinished(false, nameOfRoom);
        }

        /// <summary>Either fires on finish event to finish succesfully or resets password input based on current password input</summary>
        private void FinishWithPasswordInput()
        {
            string input = inputPassword.text;
            if (input == requiredKey)
            {
                OnFinished(true, nameOfRoom);
            }
            else
            {
                inputPassword.text = string.Empty;
                ((Text)inputPassword.placeholder).text = incorrectPasswordText;
            }
        }
    }
}