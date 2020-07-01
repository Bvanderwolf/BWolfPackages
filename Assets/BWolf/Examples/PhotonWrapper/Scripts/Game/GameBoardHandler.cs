using BWolf.Wrappers.PhotonSDK;
using BWolf.Wrappers.PhotonSDK.Editables;
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

        private Dictionary<int, Client> gridState = new Dictionary<int, Client>();
        private List<WinningStrip> winningStrips = new List<WinningStrip>();

        private const int gridSize = 3;

        private void Awake()
        {
            var clients = NetworkingService.ClientsInRoom;
            if (clients.Count != 2)
            {
                Debug.LogError("for this game there can only be 2 clients");
                return;
            }

            InitGridState();
            SetupPlayerDistinction(clients);

            playerOneHead.alpha = 0f;
            playerTwoHead.alpha = 0f;

            NetworkingService.AddClientsLoadedSceneListener(OnAllClientsLoadedScene);
            NetworkingService.AddGameEventListener(GameEvent.TurnFinished, OnTurnFinished);
        }

        private void Start()
        {
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerSceneLoaded, gameObject.scene.name);
        }

        private void OnDestroy()
        {
            NetworkingService.RemoveClientsLoadedSceneListener(OnAllClientsLoadedScene);
            NetworkingService.RemoveGameEventListener(GameEvent.TurnFinished, OnTurnFinished);
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

        private void OnTurnFinished(object obj)
        {
            TurnFinishedInfo info = (TurnFinishedInfo)obj;
            Client finishedClient = NetworkingService.ClientsInRoom[info.ActorNrOfFinishedClient];
            if (gridState.ContainsKey(info.GridIndex))
            {
                gridState[info.GridIndex] = finishedClient;
                CheckForEndCondition(finishedClient);
            }
        }

        private void CheckForEndCondition(Client client)
        {
            int length = 0;
            foreach (WinningStrip strip in winningStrips)
            {
                if (TryStrip(strip.StartIndex, strip.Exponent, client, ref length))
                {
                    OnCompletedWinningStrip(strip);
                    break;
                }
                length = 0;
            }
        }

        private void OnCompletedWinningStrip(WinningStrip strip)
        {
        }

        private bool TryStrip(int index, int exponent, Client client, ref int length)
        {
            if (index < 0 || index >= gridState.Count) { return false; ; }

            if (client == gridState[index])
            {
                length++;
                if (length == gridSize)
                {
                    return true;
                }
                else
                {
                    return TryStrip(index + exponent, exponent, client, ref length);
                }
            }

            return false;
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

        private void InitGridState()
        {
            const int indices = gridSize * gridSize;
            for (int i = 0; i < indices; i++)
            {
                gridState.Add(i, null);
            }

            //vertical strips top -> bottom
            winningStrips.Add(WinningStrip.Create(0, 3));
            winningStrips.Add(WinningStrip.Create(1, 3));
            winningStrips.Add(WinningStrip.Create(2, 3));

            //vertical strips bottom -> top
            winningStrips.Add(WinningStrip.Create(6, -3));
            winningStrips.Add(WinningStrip.Create(7, -3));
            winningStrips.Add(WinningStrip.Create(8, -3));

            //horizontal strips left -> right
            winningStrips.Add(WinningStrip.Create(0, 1));
            winningStrips.Add(WinningStrip.Create(3, 1));
            winningStrips.Add(WinningStrip.Create(6, 1));

            //horizontal strips left -> right
            winningStrips.Add(WinningStrip.Create(2, -1));
            winningStrips.Add(WinningStrip.Create(5, -1));
            winningStrips.Add(WinningStrip.Create(8, -1));

            //oblique strips top -> bottom
            winningStrips.Add(WinningStrip.Create(0, 4));
            winningStrips.Add(WinningStrip.Create(2, 2));

            //oblique strips bottom -> top
            winningStrips.Add(WinningStrip.Create(6, -2));
            winningStrips.Add(WinningStrip.Create(8, -4));
        }

        private void SetupPlayerDistinction(Dictionary<int, Client> clients)
        {
            foreach (var client in clients.Values)
            {
                Color color = (Color)client.Properties[ClientHandler.PlayerColorKey];
                if (client.IsHost)
                {
                    diskMaterial.color = color;
                    playerOneHead.text = string.Format("[{0}]", client.Nickname);
                }
                else
                {
                    crossMaterial.color = color;
                    playerTwoHead.text = string.Format("[{0}]", client.Nickname);
                }
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

        private struct WinningStrip
        {
            public int StartIndex;
            public int Exponent;

            public static WinningStrip Create(int startIndex, int exponent)
            {
                WinningStrip strip;
                strip.StartIndex = startIndex;
                strip.Exponent = exponent;
                return strip;
            }
        }
    }
}