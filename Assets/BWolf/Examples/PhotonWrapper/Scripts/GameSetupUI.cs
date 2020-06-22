using BWolf.Wrappers.PhotonSDK;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class GameSetupUI : MonoBehaviour
    {
        [SerializeField]
        private Button btnStart = null;

        [SerializeField]
        private Text txtPlayerOne = null;

        [SerializeField]
        private Text txtPlayerTwo = null;

        private void Awake()
        {
            btnStart.onClick.AddListener(StartGame);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
        }

        private void OnDestroy()
        {
            btnStart.onClick.RemoveListener(StartGame);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
        }

        private void OnJoinedRoom(string message)
        {
            //need client list
        }

        private void StartGame()
        {
        }
    }
}