using BWolf.Wrappers.PhotonSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState State { get; private set; }

        private void Awake()
        {
            Instance = this;
            State = GameState.WaitingForPlayers;

            NetworkingService.AddClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void OnDestroy()
        {
            NetworkingService.RemoveClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void OnAllClientsLoadedScene(Scene scene)
        {
            if (scene == gameObject.scene)
            {
                State = GameState.Playing;
            }
        }
    }

    public enum GameState
    {
        WaitingForPlayers,
        Playing,
        GameEnded
    }
}