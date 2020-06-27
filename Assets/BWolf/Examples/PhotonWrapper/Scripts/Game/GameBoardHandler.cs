using BWolf.Wrappers.PhotonSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class GameBoardHandler : MonoBehaviour
    {
        [SerializeField]
        private Material diskMaterial = null;

        [SerializeField]
        private Material crossMaterial = null;

        private void Awake()
        {
            diskMaterial.color = Color.blue;
            crossMaterial.color = Color.red;
            NetworkingService.AddClientsLoadedSceneListener(OnAllClientsLoadedScene);
        }

        private void Start()
        {
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerSceneLoaded, gameObject.scene.name);
        }

        private void OnAllClientsLoadedScene(Scene scene)
        {
            if (scene.name == "Game")
            {
                Debug.LogError("start game");
            }
        }
    }
}