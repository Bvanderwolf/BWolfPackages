using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>Helper class to handle callbacks and manage callback events</summary>
    public class CallbackHandler : IConnectionCallbacks, ILobbyCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
    {
        private Dictionary<SimpleCallbackEvent, Action<string>> simpleCallbackEvents = new Dictionary<SimpleCallbackEvent, Action<string>>();
        private Dictionary<InRoomCallbackEvent, Action<Client>> inRoomCallbackEvents = new Dictionary<InRoomCallbackEvent, Action<Client>>();
        private Dictionary<SimpleCallbackEvent, Action> singleSimpleCallbackEvents = new Dictionary<SimpleCallbackEvent, Action>();

        private event Action<List<LobbyData>> lobbyStatisticsUpdate;

        private event Action<List<RoomData>> roomListUpdate;

        private event Action<Client, Dictionary<string, object>> clientPropertyUpdate;

        private ClientHandler clientHandler;
        private RoomHandler roomHandler;

        public CallbackHandler(ClientHandler clientHandler, RoomHandler roomHandler)
        {
            this.clientHandler = clientHandler;
            this.roomHandler = roomHandler;
        }

        ~CallbackHandler()
        {
            simpleCallbackEvents.Clear();
            singleSimpleCallbackEvents.Clear();
        }

        /// <summary>Adds single callback event to singlecallback events dictionary</summary>
        public void AddSingleCallback(SimpleCallbackEvent callbackEvent, Action callback)
        {
            if (singleSimpleCallbackEvents.ContainsKey(callbackEvent))
            {
                singleSimpleCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                singleSimpleCallbackEvents.Add(callbackEvent, null);
                singleSimpleCallbackEvents[callbackEvent] += callback;
            }
        }

        /// <summary>Adds callback event to callback events dictionary</summary>
        public void AddListener(SimpleCallbackEvent callbackEvent, Action<string> callback)
        {
            if (simpleCallbackEvents.ContainsKey(callbackEvent))
            {
                simpleCallbackEvents[callbackEvent] += callback;
            }
            else
            {
                simpleCallbackEvents.Add(callbackEvent, null);
                simpleCallbackEvents[callbackEvent] += callback;
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

        /// <summary>removes callback event from callback events dictionary</summary>
        public void RemoveListener(SimpleCallbackEvent callbackEvent, Action<string> callback)
        {
            if (simpleCallbackEvents.ContainsKey(callbackEvent))
            {
                simpleCallbackEvents[callbackEvent] -= callback;
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

        /// <summary>Called when connected initialily with the server it fires events if there are subscribers</summary>
        public void OnConnected()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Connected))
            {
                simpleCallbackEvents[SimpleCallbackEvent.Connected](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Connected))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.Connected]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.Connected);
            }
        }

        /// <summary>Called when connected with the master server it fires events if there are subscribers</summary>
        public void OnConnectedToMaster()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.ConnectedToMaster))
            {
                simpleCallbackEvents[SimpleCallbackEvent.ConnectedToMaster](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.ConnectedToMaster))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.ConnectedToMaster]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.ConnectedToMaster);
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

            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Disconnected))
            {
                simpleCallbackEvents[SimpleCallbackEvent.Disconnected]?.Invoke(cause.ToString());
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.Disconnected))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.Disconnected]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.Disconnected);
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        /// <summary>Called when having joined a lobby it fires events if there are subscribers</summary>
        public void OnJoinedLobby()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinedLobby))
            {
                simpleCallbackEvents[SimpleCallbackEvent.JoinedLobby](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinedLobby))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.JoinedLobby]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.JoinedLobby);
            }
        }

        /// <summary>Called when having left a lobby it fires events if there are subscribers</summary>
        public void OnLeftLobby()
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.LeftLobby))
            {
                simpleCallbackEvents[SimpleCallbackEvent.LeftLobby](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.LeftLobby))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.LeftLobby]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.LeftLobby);
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
                    data.Add(RoomData.Create(info.RemovedFromList, info.Name, info.PlayerCount, info.MaxPlayers, roomKey));
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
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.CreatedRoom))
            {
                simpleCallbackEvents[SimpleCallbackEvent.CreatedRoom](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.CreatedRoom))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.CreatedRoom]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.CreatedRoom);
            }
        }

        /// <summary>Called when having failed creating a room it fires events if there are subscribers</summary>
        public void OnCreateRoomFailed(short returnCode, string message)
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.CreateRoomFailed))
            {
                string info = string.Format("ReturnCode: {0}, message: {1}", returnCode, message);
                simpleCallbackEvents[SimpleCallbackEvent.CreateRoomFailed](info);
            }
        }

        /// <summary>Called when having joined a room it fires events if there are subscribers</summary>
        public void OnJoinedRoom()
        {
            clientHandler.OnJoinedRoom();
            roomHandler.OnJoinedRoom();

            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinedRoom))
            {
                simpleCallbackEvents[SimpleCallbackEvent.JoinedRoom](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinedRoom))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.JoinedRoom]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.JoinedRoom);
            }
        }

        /// <summary>Called when having failed joining a room it fires events if there are subscribers</summary>
        public void OnJoinRoomFailed(short returnCode, string message)
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinRoomFailed))
            {
                string info = string.Format("ReturnCode: {0}, message: {1}", returnCode, message);
                simpleCallbackEvents[SimpleCallbackEvent.JoinRoomFailed](info);
            }
        }

        /// <summary>Called when having failed joining a random room it fires events if there are subscribers</summary>
        public void OnJoinRandomFailed(short returnCode, string message)
        {
            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.JoinRandomRoomFailed))
            {
                string info = string.Format("ReturnCode: {0}, message: {1}", returnCode, message);
                simpleCallbackEvents[SimpleCallbackEvent.JoinRandomRoomFailed](info);
            }
        }

        /// <summary>Called when having left a room it fires events if there are subscribers</summary>
        public void OnLeftRoom()
        {
            clientHandler.Reset();
            roomHandler.Reset();

            if (simpleCallbackEvents.ContainsKey(SimpleCallbackEvent.LeftRoom))
            {
                simpleCallbackEvents[SimpleCallbackEvent.LeftRoom](null);
            }
            if (singleSimpleCallbackEvents.ContainsKey(SimpleCallbackEvent.LeftRoom))
            {
                singleSimpleCallbackEvents[SimpleCallbackEvent.LeftRoom]();
                singleSimpleCallbackEvents.Remove(SimpleCallbackEvent.LeftRoom);
            }
        }

        /// <summary>Called when a player has entered a room it fires events if there are subscribers</summary>
        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            Client client = (Client)newPlayer;
            roomHandler.OnClientJoined(client);
            if (inRoomCallbackEvents.ContainsKey(InRoomCallbackEvent.ClientJoined))
            {
                inRoomCallbackEvents[InRoomCallbackEvent.ClientJoined](client);
            }
        }

        /// <summary>Called when a player has left a room it fires events if there are subscribers</summary>
        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            Client client = (Client)otherPlayer;
            roomHandler.OnClientLeft(client);
            if (inRoomCallbackEvents.ContainsKey(InRoomCallbackEvent.ClientJoined))
            {
                inRoomCallbackEvents[InRoomCallbackEvent.ClientLeft](client);
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

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
                inRoomCallbackEvents[InRoomCallbackEvent.HostChanged](client);
            }
        }
    }
}