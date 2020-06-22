using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class CreateRoomForm : MonoBehaviour
    {
        [SerializeField]
        private Button btnFinish = null;

        [SerializeField]
        private InputField inputName = null;

        [SerializeField]
        private InputField inputPassword = null;

        [SerializeField]
        private RoomItemsUI controller = null;

        private string nameEntered;
        private string passwordEntered;

        private bool nameWasFocused;
        private bool passwordWasFocused;

        private void Awake()
        {
            btnFinish.onClick.AddListener(OnFinishButtonClick);
            inputName.onEndEdit.AddListener(OnNameEntered);
            inputPassword.onEndEdit.AddListener(OnPasswordEntered);
            CheckFinishability();
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(inputName.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && nameWasFocused)
            {
                EventSystem.current.SetSelectedGameObject(inputPassword.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Return) && (nameWasFocused || passwordWasFocused))
            {
                TryReturnInputFinish();
            }

            nameWasFocused = inputName.isFocused;
            passwordWasFocused = inputPassword.isFocused;
        }

        private void OnDestroy()
        {
            btnFinish.onClick.RemoveListener(OnFinishButtonClick);
            inputName.onEndEdit.RemoveListener(OnNameEntered);
            inputPassword.onEndEdit.RemoveListener(OnPasswordEntered);
        }

        public void OnNameEntered(string edit)
        {
            nameEntered = edit;
            CheckFinishability();
        }

        public void OnPasswordEntered(string edit)
        {
            passwordEntered = edit;
        }

        public void OnFinishButtonClick()
        {
            controller.CreateRoomFinish(nameEntered, passwordEntered);
        }

        private void TryReturnInputFinish()
        {
            if (!string.IsNullOrEmpty(inputName.text))
            {
                controller.CreateRoomFinish(inputName.text, passwordEntered);
            }
        }

        private void CheckFinishability()
        {
            btnFinish.interactable = !string.IsNullOrEmpty(nameEntered) && !controller.IsListedRoom(nameEntered);
        }
    }
}