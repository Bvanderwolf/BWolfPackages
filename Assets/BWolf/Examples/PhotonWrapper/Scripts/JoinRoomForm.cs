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
            if (Input.GetKeyDown(KeyCode.Return) && passwordWasFocused)
            {
                CheckPasswordInput();
            }

            passwordWasFocused = inputPassword.isFocused;
        }

        public void AddListener(Action<bool, string> onFinished)
        {
            OnFinished += onFinished;
        }

        public void RemoveListener(Action<bool, string> onFinished)
        {
            OnFinished -= onFinished;
        }

        public void AddRequirement(string key)
        {
            requiredKey = key;
        }

        public void AddNameOfRoom(string name)
        {
            nameOfRoom = name;
        }

        public void OnFinishButtonClick()
        {
            CheckPasswordInput();
        }

        public void OnCancel()
        {
            OnFinished(false, nameOfRoom);
        }

        private void CheckPasswordInput()
        {
            string input = inputPassword.text;
            print(input);
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