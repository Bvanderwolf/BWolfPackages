using BWolf.Wrappers.PhotonSDK;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class EndGameUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text txtEndGame = null;

        [SerializeField]
        private Button btnLeave = null;

        private bool activeState = true;

        private void Awake()
        {
            ToggleActiveState();

            btnLeave.onClick.AddListener(OnLeaveButtonClick);
            GameBoardHandler.OnGameFinished += OnGameFinished;
        }

        private void OnDestroy()
        {
            btnLeave.onClick.RemoveListener(OnLeaveButtonClick);
            GameBoardHandler.OnGameFinished -= OnGameFinished;
        }

        private void OnLeaveButtonClick()
        {
            ToggleActiveState();
            NetworkingService.LeaveRoom(true, () =>
            {
                SceneManager.LoadScene("Main");
            });
        }

        private void OnGameFinished(Client winningClient)
        {
            ToggleActiveState();
            if (winningClient == null)
            {
                txtEndGame.text = "Draw!";
            }
            else
            {
                txtEndGame.text = string.Format("[{0}] Won!", winningClient.Nickname);
            }
        }

        private void ToggleActiveState()
        {
            activeState = !activeState;
            txtEndGame.gameObject.SetActive(activeState);
            btnLeave.gameObject.SetActive(activeState);
        }
    }
}