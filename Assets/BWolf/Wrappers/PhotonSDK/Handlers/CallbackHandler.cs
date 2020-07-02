using BWolf.Wrappers.PhotonSDK.DataContainers;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace BWolf.Wrappers.PhotonSDK.Handlers
{
    /// <summary>Helper class to handle callbacks and manage callback events</summary>
    public class CallbackHandler : IConnectionCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
    {
        private Dictionary<MatchmakingCallbackEvent, Action<string>> matchmakingCallbackEvents = new Dictionary<MatchmakingCallbackEvent, Action<string>>();
        private Dictionary<InRoomCallbackEvent, Action<Client>> inRoomCallbackEvents = new Dictionary<InRoomCallbackEvent, Action<Client>>();
        private Dictionary<MatchmakingCallbackEvent, Action> singleMatchmakingCallbackEvents = new Dictionary<MatchmakingCallbackEvent, Action>();

        private event Action<List<LobbyData>> lobbyStatisticsUpdate;

        private event Action<List<RoomData>> roomListUpdate;

        private event Action<Client, Dictionary<string, object>> clientPropertyUpdate;

        private event Action<Scene> onAllClientsLoadedScene;

        private ClientHandler clientHandler;
        private RoomHandler roomHandler;

        public CallbackHandler(ClientHandler clientHandler, RoomHandler roomHandler)
        {
            this.clientHandler = clientHandler;
            this.roomHandler = roomHandler;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        ~CallbackHandler()
        {
            matchmakingCallbackEvents.Clear();
            singleMatchmakingCallbackEvents.Clear();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>Adds single callback event to singlecallback events dictionary</summary>
        public void AddSingleCallback(MatchmakingCallbackEvent callbackEvent, Action callback)
        {
            if (singleMatchmakingCallbackEvents.ContainsKey(callbackEvent))
            {
                singleMatchmakingCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                singleMatchmakingCallbackEvents.Add(callbackEvent, null);
                singleMatchmakingCallbackEvents[callbackEvent] += callback;
            }
        }

        /// <summary>Adds callback event to callback events dictionary</summary>
        public void AddListener(MatchmakingCallbackEvent callbackEvent, Action<string> callback)
        {
            if (matchmakingCallbackEvents.ContainsKey(callbackEvent))
            {
                matchmakingCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                matchmakingCallbackEvents.Add(callbackEvent, null);
                matchmakingCallbackEvents[callbackEvent] += callback;
            }
        }

        /// <summary>Adds callback event to in room callback events dictionary</summary>
        public void AddListener(InRoomCallbackEvent callbackEvent, Action<Client> callback)
        {
            if (inRoomCallbackEvents.ContainsKey(callbackEvent))
            {
                inRoomCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                inRoomCallbackEvents.Add(callbackEvent, null);
                inRoomCallbackEvents[callbackEvent] += callback;
            }
        }

        /// <summary>Adds lobby info statistics update  callback event to lobby statistics update event invocation list</summary>
        public void AddListener(Action<List<LobbyData>> action)
        {
            lobbyStatisticsUpdate += action;
        }

        /// <summary>Adds room list update  callback event to room list update event invocation list</summary>
        public void AddListener(Action<List<RoomData>> action)
        {
            roomListUpdate += action;
        }

        /// <summary>Adds client property update callback to client property update event invocation list</summary>
        public void AddListener(Action<Client, Dictionary<string, object>> action)
        {
            clientPropertyUpdate += action;
        }

        /// <summary>Adds listener to the all clients loaded scene event</summary>
        public void AddListener(Action<Scene> callback)
        {
            onAllClientsLoadedScene += callback;
        }

        /// <summary>removes callback event from callback events dictionary</summary>
        public void RemoveListener(MatchmakingCallbackEvent callbackEvent, Action<string> callback)
        {
            if (matchmakingCallbackEvents.ContainsKey(callbackEvent))
            {
                matchmakingCallbackEvents[callbackEvent] -= callback;
            }
        }

        /// <summary>removes callback event from in room callback events dictionary</summary>
        public void RemoveListener(InRoomCallbackEvent callbackEvent, Action<Client> callback)
        {
            if (inRoomCallbackEvents.ContainsKey(callbackEvent))
            {
                inRoomCallbackEvents[callbackEvent] -= callback;
            }
        }

        /// <summary>Removes lobby statistics update callback event from invocation list</summary>
        public void RemoveListener(Action<List<LobbyData>> action)
        {
            lobbyStatisticsUpdate -= action;
        }

        /// <summary>Removes room list update callback event from invocation list</summary>
        public void RemoveListener(Action<List<RoomData>> action)
        {
            roomListUpdate -= action;
        }

        /// <summary>Removes client property update callback from client property update event invocation list</summary>
        public void RemoveListener(Action<Client, Dictionary<string, object>> action)
        {
            clientPropertyUpdate -= action;
        }

        /// <summary>removes listener to the all clients loaded scene event</summary>
        public void RemoveListener(Action<Scene> callback)
        {
            onAllClientsLoadedScene -= callback;
        }

        /// <summary>Called when a scene has been loaded, it when in a room start waiting for other clients to also load this scene</summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (NetworkingService.InRoom)
            {
                WaitForAllClientsToLoadScene(scene);
            }
        }

        /// <summary>Starts a waiting task going through clients in the room their properties to see if everyone has loaded</summary>
        private async void WaitForAllClientsToLoadScene(Scene scene)
        {
            var values = NetworkingService.ClientsInRoom.Values;
            await TaskHelper.WaitUntil(() => values.All(c => (string)c.Properties[ClientHandler.PlayerSceneLoaded] == scene.name), 20);
            onAllClientsLoadedScene?.Invoke(scene);
        }

        /// <summary>Called when connected initialily with the server it fires events if there are subscribers</summary>
        public void OnConnected()
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.Connected))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.Connected](null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.Connected))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.Connected]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.Connected);
            }
        }

        /// <summary>Called when connected with the master server it fires events if there are subscribers</summary>
        public void OnConnectedToMaster()
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.ConnectedToMaster))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.ConnectedToMaster](null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.ConnectedToMaster))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.ConnectedToMaster]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.ConnectedToMaster);
            }
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        /// <summary>Called when disconnected from the server it fires events if there are subscribers</summary>
        public void OnDisconnected(DisconnectCause cause)
        {
            clientHandler.Reset();
            roomHandler.Reset();

            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.Disconnected))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.Disconnected]?.Invoke(cause.ToString());
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.Disconnected))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.Disconnected]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.Disconnected);
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        /// <summary>Called when having joined a lobby it fires events if there are subscribers</summary>
        public void OnJoinedLobby()
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.JoinedLobby))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.JoinedLobby](null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.JoinedLobby))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.JoinedLobby]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.JoinedLobby);
            }
        }

        /// <summary>Called when having left a lobby it fires events if there are subscribers</summary>
        public void OnLeftLobby()
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.LeftLobby))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.LeftLobby](null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.LeftLobby))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.LeftLobby]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.LeftLobby);
            }
        }

        /// <summary>Called rooms have been created or deleted, it fires an event with room info if there are subscribers</summary>
        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (roomListUpdate != null)
            {
                List<RoomData> data = new List<RoomData>();
                foreach (RoomInfo info in roomList)
                {
                    string roomKey = (string)info.CustomProperties[RoomData.PasswordPropertyKey];
                    data.Add(RoomData.Create(info.RemovedFromList, info.Name, info.PlayerCount, info.MaxPlayers, info.IsOpen, info.IsVisible, roomKey));
                }
                roomListUpdate(data);
            }
        }

        /// <summary>Called when lobby statistics have been updated it fires an event with lobby info if there are subscribers</summary>
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            if (lobbyStatisticsUpdate != null)
            {
                List<LobbyData> data = new List<LobbyData>();
                foreach (TypedLobbyInfo info in lobbyStatistics)
                {
                    data.Add(LobbyData.Create(info.Name, info.PlayerCount, info.RoomCount));
                }
                lobbyStatisticsUpdate(data);
            }
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        /// <summary>Called when having created a room it fires events if there are subscribers</summary>
        public void OnCreatedRoom()
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.CreatedRoom))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.CreatedRoom](null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.CreatedRoom))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.CreatedRoom]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.CreatedRoom);
            }
        }

        /// <summary>Called when having failed creating a room it fires events if there are subscribers</summary>
        public void OnCreateRoomFailed(short returnCode, string message)
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.CreateRoomFailed))
            {
                string info = string.Format("ReturnCode: {0}, message: {1}", returnCode, message);
                matchmakingCallbackEvents[MatchmakingCallbackEvent.CreateRoomFailed](info);
            }
        }

        /// <summary>Called when having joined a room it fires events if there are subscribers</summary>
        public void OnJoinedRoom()
        {
            clientHandler.OnJoinedRoom();
            roomHandler.OnJoinedRoom();

            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.JoinedRoom))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.JoinedRoom](null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.JoinedRoom))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.JoinedRoom]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.JoinedRoom);
            }
        }

        /// <summary>Called when having failed joining a room it fires events if there are subscribers</summary>
        public void OnJoinRoomFailed(short returnCode, string message)
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.JoinRoomFailed))
            {
                string info = string.Format("ReturnCode: {0}, message: {1}", returnCode, message);
                matchmakingCallbackEvents[MatchmakingCallbackEvent.JoinRoomFailed](info);
            }
        }

        /// <summary>Called when having failed joining a random room it fires events if there are subscribers</summary>
        public void OnJoinRandomFailed(short returnCode, string message)
        {
            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.JoinRandomRoomFailed))
            {
                string info = string.Format("ReturnCode: {0}, message: {1}", returnCode, message);
                matchmakingCallbackEvents[MatchmakingCallbackEvent.JoinRandomRoomFailed](info);
            }
        }

        /// <summary>Called when having left a room it fires events if there are subscribers</summary>
        public void OnLeftRoom()
        {
            clientHandler.Reset();
            roomHandler.Reset();

            if (matchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.LeftRoom))
            {
                matchmakingCallbackEvents[MatchmakingCallbackEvent.LeftRoom]?.Invoke(null);
            }
            if (singleMatchmakingCallbackEvents.ContainsKey(MatchmakingCallbackEvent.LeftRoom))
            {
                singleMatchmakingCallbackEvents[MatchmakingCallbackEvent.LeftRoom]();
                singleMatchmakingCallbackEvents.Remove(MatchmakingCallbackEvent.LeftRoom);
            }
        }

        /// <summary>Called when a player has entered a room it fires events if there are subscribers</summary>
        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Client client = (Client)newPlayer;
            roomHandler.OnClientJoined(client);
            if (inRoomCallbackEvents.ContainsKey(InRoomCallbackEvent.ClientJoined))
            {
                inRoomCallbackEvents[InRoomCallbackEvent.ClientJoined]?.Invoke(client);
            }
        }

        /// <summary>Called when a player has left a room it fires events if there are subscribers</summary>
        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Client client = (Client)otherPlayer;
            roomHandler.OnClientLeft(client);
            if (inRoomCallbackEvents.ContainsKey(InRoomCallbackEvent.ClientJoined))
            {
                inRoomCallbackEvents[InRoomCallbackEvent.ClientLeft]?.Invoke(client);
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        /// <summary>Called when a players properties have been changed and callbacks need to be handled</summary>
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            Client client = (Client)targetPlayer;
            Dictionary<string, object> props = new Dictionary<string, object>();
            foreach (var prop in changedProps)
            {
                props.Add((string)prop.Key, prop.Value);
            }

            clientHandler.OnClientPropertyUpdate(client, props);
            roomHandler.OnClientPropertyUpdate(client, props);
            clientPropertyUpdate?.Invoke(client, props);
        }

        /// <summary>Called when the host in the room has been switched it fires events if there are subscribers</summary>
        public void OnMasterClientSwitched(Player newMasterClient)
        {
            Client client = (Client)newMasterClient;
            clientHandler.OnHostChanged(client);
            roomHandler.OnHostChanged(client);

            if (inRoomCallbackEvents.ContainsKey(InRoomCallbackEvent.ClientJoined))
            {
                inRoomCallbackEvents[InRoomCallbackEvent.HostChanged]?.Invoke(client);
            }
        }
    }
}