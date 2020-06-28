using BWolf.Wrappers.PhotonSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class GameBoardHandler : MonoBehaviour
    {
        [Header("Player Distinction")]
        [SerializeField]
        private Material diskMaterial = null;

        [SerializeField]
        private Material crossMaterial = null;

        [Header("Lineups")]
        [SerializeField]
        private Transform playerOne = null;

        [SerializeField]
        private Transform playerTwo = null;

        private void Awake()
        {
            var clients = NetworkingService.ClientsInRoom;
            if (clients.Count != 2)
            {
                Debug.LogError("for this game there can only be 2 clients");
                return;
            }

            foreach (var client in clients.Values)
            {
                Color color = (Color)client.Properties[ClientHandler.PlayerColorKey];
                if (client.IsHost)
                {
                    diskMaterial.color = color;
                }
                else
                {
                    crossMaterial.color = color;
                }
            }
            NetworkingService.AddClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void Start()
        {
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerSceneLoaded, gameObject.scene.name);
        }

        private void OnDestroy()
        {
            NetworkingService.RemoveClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void OnAllClientsLoadedScene(Scene scene)
        {
            if (scene == gameObject.scene)
            {
                Debug.LogError("start game");
            }
        }
    }
}