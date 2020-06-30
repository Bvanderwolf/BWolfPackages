using BWolf.Wrappers.PhotonSDK;
using BWolf.Wrappers.PhotonSDK.Handlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class GameBoardHandler : MonoBehaviour
    {
        [Header("GameBoardSettings")]
        [SerializeField]
        private int milisecondsBeforeStart = 2500;

        [Header("Player Distinction")]
        [SerializeField]
        private Material diskMaterial = null;

        [SerializeField]
        private string nameOfDiskPrefab = "PlayerDisk";

        [SerializeField]
        private Material crossMaterial = null;

        [SerializeField]
        private string nameOfCrossPrefab = "PlayerCross";

        [Header("PlayerOne")]
        [SerializeField]
        private Transform playerOneSpawns = null;

        [SerializeField]
        private Transform playerOnePlayPositions = null;

        [SerializeField]
        private TMP_Text playerOneHead = null;

        [Header("PlayerTwo")]
        [SerializeField]
        private Transform playerTwoSpawns = null;

        [SerializeField]
        private Transform playerTwoPlayPositions = null;

        [SerializeField]
        private TMP_Text playerTwoHead = null;

        private List<GameObject> pawns = new List<GameObject>();

        private WaitForFixedUpdate waitForRenderFrame = new WaitForFixedUpdate();

        public static event Action OnSetupFinished;

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
                    playerOneHead.text = client.Nickname;
                }
                else
                {
                    crossMaterial.color = color;
                    playerTwoHead.text = client.Nickname;
                }
            }

            playerOneHead.alpha = 0f;
            playerTwoHead.alpha = 0f;

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
                bool isPlayerOne = NetworkingService.IsHost;
                CreatePlayerPawns(isPlayerOne);
                DoIntro(isPlayerOne);
            }
        }

        private void CreatePlayerPawns(bool isPlayerOne)
        {
            Transform lineup = isPlayerOne ? playerOneSpawns : playerTwoSpawns;
            string nameOfPrefab = isPlayerOne ? nameOfDiskPrefab : nameOfCrossPrefab;
            foreach (Transform child in lineup)
            {
                pawns.Add(NetworkingService.InstantiateOwnedObject(nameOfPrefab, child.position, child.rotation));
            }
        }

        private async void DoIntro(bool isPlayerOne)
        {
            await Task.Delay(milisecondsBeforeStart);
            StartCoroutine(PlayerHeadersFadeIn());
            StartCoroutine(MovePawnsTowardsStart(isPlayerOne));
        }

        private IEnumerator PlayerHeadersFadeIn()
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime;
                if (t > 1f) { t = 1f; }
                float perc = t / 1f;
                playerOneHead.alpha = Mathf.Lerp(0, 1f, Mathf.Sin(perc * Mathf.PI * 0.5f));
                playerTwoHead.alpha = Mathf.Lerp(0, 1f, Mathf.Sin(perc * Mathf.PI * 0.5f));
                yield return null;
            }
        }

        private IEnumerator MovePawnsTowardsStart(bool isPlayerOne)
        {
            float t = 0;
            Transform spawnPositions = isPlayerOne ? playerOneSpawns : playerTwoSpawns;
            Transform startPositions = isPlayerOne ? playerOnePlayPositions : playerTwoPlayPositions;
            while (t < 1f)
            {
                t += Time.deltaTime;
                if (t > 1f) { t = 1f; }
                float perc = t / 1f;
                for (int ci = 0; ci < spawnPositions.childCount; ci++)
                {
                    pawns[ci].transform.position = Vector3.Lerp(spawnPositions.GetChild(ci).position, startPositions.GetChild(ci).position, Mathf.Sin(perc * Mathf.PI * 0.5f));
                }

                yield return waitForRenderFrame;
            }

            OnSetupFinished();
        }
    }
}