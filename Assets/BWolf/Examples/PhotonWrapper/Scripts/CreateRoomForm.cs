using UnityEngine;
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

        private void Awake()
        {
            btnFinish.onClick.AddListener(OnFinishButtonClick);
            CheckFinishability();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
            }
        }

        private void OnDestroy()
        {
            btnFinish.onClick.RemoveListener(OnFinishButtonClick);
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

        private void CheckFinishability()
        {
            btnFinish.interactable = !string.IsNullOrEmpty(nameEntered) && !controller.IsListedRoom(nameEntered);
        }
    }
}