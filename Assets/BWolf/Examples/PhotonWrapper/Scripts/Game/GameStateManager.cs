using BWolf.Wrappers.PhotonSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class GameStateManager : MonoBehaviour
    {
        public GameState State { get; private set; }

        private void Awake()
        {
            State = GameState.WaitingForPlayers;

            GameBoardHandler.OnSetupFinished += OnGameBoardSetupFinish;
            GameBoardHandler.OnGameFinished += OnGameFinished;

            NetworkingService.AddClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void OnGameFinished(Client winningClient)
        {
            State = GameState.GameEnded;
        }

        private void OnDestroy()
        {
            GameBoardHandler.OnSetupFinished -= OnGameBoardSetupFinish;

            NetworkingService.RemoveClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void OnGameBoardSetupFinish()
        {
            State = GameState.Playing;
        }

        private void OnAllClientsLoadedScene(Scene scene)
        {
            if (scene == gameObject.scene)
            {
                State = GameState.Setup;
            }
        }
    }

    public enum GameState
    {
        WaitingForPlayers,
        Setup,
        Playing,
        GameEnded
    }
}