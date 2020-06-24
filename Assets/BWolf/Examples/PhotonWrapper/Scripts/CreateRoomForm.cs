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
            //set focus on input name when enabling this form
            EventSystem.current.SetSelectedGameObject(inputName.gameObject);

            //make sure inputs are clean
            inputName.text = string.Empty;
            inputPassword.text = string.Empty;
        }

        private void Update()
        {
            //tab can be used to switch focus from name input to password input
            if (Input.GetKeyDown(KeyCode.Tab) && nameWasFocused)
            {
                EventSystem.current.SetSelectedGameObject(inputPassword.gameObject);
            }

            //return can be used to try finish the form without pressing the finish button
            if (Input.GetKeyDown(KeyCode.Return) && (nameWasFocused || passwordWasFocused))
            {
                TryReturnInputFinish();
            }

            //store states of isfocused to make sure that if a key was pressed, the next frame, it can be checked whether it was focused or not
            nameWasFocused = inputName.isFocused;
            passwordWasFocused = inputPassword.isFocused;
        }

        private void OnDestroy()
        {
            btnFinish.onClick.RemoveListener(OnFinishButtonClick);
            inputName.onEndEdit.RemoveListener(OnNameEntered);
            inputPassword.onEndEdit.RemoveListener(OnPasswordEntered);
        }

        /// <summary>Called when a name has been entered to store that new name and check if the finish button can be set to be interactable</summary>
        public void OnNameEntered(string edit)
        {
            nameEntered = edit;
            CheckFinishability();
        }

        /// <summary>Called when the password has been edited, it updates the stored value</summary>
        public void OnPasswordEntered(string edit)
        {
            passwordEntered = edit;
        }

        /// <summary>Called when the finish button has been clicked it calls upon the controller to finish creating the room</summary>
        public void OnFinishButtonClick()
        {
            controller.CreateRoomFinish(nameEntered, passwordEntered);
        }

        /// <summary>finishes up creating the room if the current room name input is valid</summary>
        private void TryReturnInputFinish()
        {
            if (!string.IsNullOrEmpty(inputName.text))
            {
                controller.CreateRoomFinish(inputName.text, passwordEntered);
            }
        }

        /// <summary>checks whether creating a room can actually be finished</summary>
        private void CheckFinishability()
        {
            btnFinish.interactable = !string.IsNullOrEmpty(nameEntered) && !controller.IsListedRoom(nameEntered);
        }
    }
}