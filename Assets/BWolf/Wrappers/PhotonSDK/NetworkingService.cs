// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Wrappers.PhotonSDK.DataContainers;
using BWolf.Wrappers.PhotonSDK.Handlers;
using BWolf.Wrappers.PhotonSDK.Serialization;
using BWolf.Wrappers.PhotonSDK.Synchronization;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>the main class to be used for accessing all functionalities regarding networking. this class is static</summary>
    public static class NetworkingService
    {
        private static readonly CallbackHandler callbackHandler;
        private static readonly MatchmakingHandler matchmakingHandler;
        private static readonly ClientHandler clientHandler;
        private static readonly RoomHandler roomHandler;
        private static readonly ResourceHandler resourceHandler;
        private static readonly MultiplayerEventHandler eventHandler;
        private static readonly SerializableTypes serialization;

        private static NetworkingSettings settings;

        public const int MaxPlayersOnServer = 20; //value is according to photon's free account maximum

        /// <summary>Returns whether this client is connected to the server or not</summary>
        public static bool IsConnected
        {
            get { return PhotonNetwork.IsConnected; }
        }

        /// <summary>Returns whether the client is in a room at the moment</summary>
        public static bool InRoom
        {
            get { return PhotonNetwork.InRoom; }
        }

        /// <summary>Returns whether this client is in offline mode</summary>
        public static bool InOfflineMode
        {
            get { return PhotonNetwork.OfflineMode; }
        }

        /// <summary>Returns the connection state of the client formatted to a string</summary>
        public static string ConnectionState
        {
            get { return PhotonNetwork.NetworkClientState.ToString(); }
        }

        /// <summary>A dictionary containing the clients that are in the room at the moment, where the key is a client's actornumber</summary>
        public static Dictionary<int, Client> ClientsInRoom
        {
            get { return roomHandler.ClientsInRoom; }
        }

        /// <summary>The players local client instance</summary>
        public static Client LocalClient
        {
            get { return clientHandler.LocalClient; }
        }

        /// <summary>Returns the actor number of the host when inside a room, if not inside a room returns -1</summary>
        public static int ActorNumberOfHost
        {
            get { return roomHandler.ActorNumberOfHost; }
        }

        /// <summary>Returns whether this client is the host client in the current room he is in</summary>
        public static bool IsHost
        {
            get { return roomHandler.IsHost; }
        }

        /// <summary>gets or sets the nickname of the local client</summary>
        public static string Nickname
        {
            get { return clientHandler.LocalClient.Nickname; }
            set { clientHandler.LocalClient.SetNickname(value); }
        }

        static NetworkingService()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif

            settings = Resources.Load<NetworkingSettings>("NetworkingSettings");

            matchmakingHandler = new MatchmakingHandler();
            serialization = new SerializableTypes();
            eventHandler = new MultiplayerEventHandler(serialization);
            clientHandler = new ClientHandler();
            roomHandler = new RoomHandler();
            callbackHandler = new CallbackHandler(clientHandler, roomHandler);
            resourceHandler = new ResourceHandler(settings, eventHandler);

            serialization.RegisterCustomTypesInternal();

            PhotonNetwork.SerializationRate = settings.SerializationRate;
            PhotonNetwork.SendRate = settings.SendRate;
            PhotonNetwork.AutomaticallySyncScene = settings.SynchronizeClientScenes;

            PhotonNetwork.AddCallbackTarget(callbackHandler);
            PhotonNetwork.AddCallbackTarget(eventHandler);
        }

        /// <summary>Adds a callback listener for matchmaking events</summary>
        public static void AddCallbackListener(MatchmakingCallbackEvent callbackEvent, Action<string> callback)
        {
            callbackHandler.AddListener(callbackEvent, callback);
        }

        /// <summary>Adds a callback listener for events in a room that contain a client value</summary>
        public static void AddCallbackListener(InRoomCallbackEvent callbackEvent, Action<Client> callback)
        {
            callbackHandler.AddListener(callbackEvent, callback);
        }

        /// <summary>Adds a callback listener for game events in the room that contains content based on the type of game event</summary>
        public static void AddGameEventListener(string nameOfEvent, Action<object> callback)
        {
            eventHandler.AddGameEventListener(nameOfEvent, callback);
        }

        /// <summary>Adds a callback listener for game requests in the room that contains content based on the type of game request</summary>
        public static void AddGameRequestListener(string nameOfRequest, Action<object> callback)
        {
            eventHandler.AddGameRequestListener(nameOfRequest, callback);
        }

        /// <summary>Adds a callback listener to the statistics update event to be called when lobby statistics are updated</summary>
        public static void AddLobbyStatisticsListener(Action<List<LobbyData>> onUpdate)
        {
            callbackHandler.AddListener(onUpdate);
        }

        /// <summary>Adds a callback listener to the room list event to be called when room list is updated</summary>
        public static void AddRoomListListener(Action<List<RoomData>> onUpdate)
        {
            callbackHandler.AddListener(onUpdate);
        }

        /// <summary>Adds the callback listener to the client property update event</summary>
        public static void AddClientPropertyUpdateListener(Action<Client, Dictionary<string, object>> onUpdate)
        {
            callbackHandler.AddListener(onUpdate);
        }

        /// <summary>Adds listener to the clients loaded scene event which is fired when ALL clients have loaded a scene</summary>
        public static void AddClientsLoadedSceneListener(Action<Scene> onClientsLoadedScene)
        {
            callbackHandler.AddListener(onClientsLoadedScene);
        }

        /// <summary>Removes callback listener for matchmaking events</summary>
        public static void RemoveCallbackListener(MatchmakingCallbackEvent callbackEvent, Action<string> callback)
        {
            callbackHandler.RemoveListener(callbackEvent, callback);
        }

        /// <summary>Removes a callback listener for events in a room that contain a client value</summary>
        public static void RemoveCallbackListener(InRoomCallbackEvent callbackEvent, Action<Client> callback)
        {
            callbackHandler.RemoveListener(callbackEvent, callback);
        }

        /// <summary>Stops callback listener from listening to game events in the room that contains content based on the type of game event</summary>
        public static void RemoveGameEventListener(string nameOfEvent, Action<object> callback)
        {
            eventHandler.RemoveGameEventListener(nameOfEvent, callback);
        }

        /// <summary>Stops callback listener from listening to game requests in the room that contains content based on the type of game event</summary>
        public static void RemoveGameRequestListener(string nameOfRequest, Action<object> callback)
        {
            eventHandler.RemoveGameRequestListener(nameOfRequest, callback);
        }

        /// <summary>Removes callback listener from the lobby statistics update event</summary>
        public static void RemoveLobbyStatisticsListener(Action<List<LobbyData>> onUpdate)
        {
            callbackHandler.RemoveListener(onUpdate);
        }

        /// <summary>Removes callback listener from the room list update event</summary>
        public static void RemoveRoomListListener(Action<List<RoomData>> onUpdate)
        {
            callbackHandler.RemoveListener(onUpdate);
        }

        /// <summary>Adds the callback listener to the client property update event</summary>
        public static void RemoveClientPropertyUpdateListener(Action<Client, Dictionary<string, object>> onUpdate)
        {
            callbackHandler.RemoveListener(onUpdate);
        }

        /// <summary>removes listener from the clients loaded scene event which is fired when ALL clients have loaded a scene</summary>
        public static void RemoveClientsLoadedSceneListener(Action<Scene> onClientsLoadedScene)
        {
            callbackHandler.RemoveListener(onClientsLoadedScene);
        }

        /// <summary>registers a custom serializable type using type, a character and 2 methods for serialization and deserilialization</summary>
        public static void RegisterCustomSerializable(Type type, char charCode, SerializeMethod serialize, DeserializeMethod deserialize)
        {
            serialization.RegisterCustomType(type, charCode, serialize, deserialize);
        }

        /// <summary>registers game event using given name of event and the type of content it needs to use</summary>
        public static void RegisterGameEvent(string nameOfEvent, Type contentType)
        {
            if (!string.IsNullOrEmpty(nameOfEvent))
            {
                eventHandler.RegisterGameEvent(nameOfEvent, contentType);
            }
        }

        /// <summary>registers game request using given name, type of content, request delay, start handler and decision handler</summary>
        public static void RegisterGameRequest(string nameOfRequest, Type contentType, int requestDelayMiliseconds, RequestStartHandler startHandler, RequestDecisiontHandler decisionHandler)
        {
            if (!string.IsNullOrEmpty(nameOfRequest))
            {
                eventHandler.RegisterGameRequest(nameOfRequest, contentType, requestDelayMiliseconds, startHandler, decisionHandler);
            }
        }

        /// <summary>Loads a scene using given build index. Makes use of NetworkingSettings's synchronizeclientscenes flag to make other clients also load this scene if set to true</summary>
        public static void LoadScene(int sceneBuildIndex)
        {
            if (settings.SynchronizeClientScenes && !IsHost)
            {
                Debug.LogWarning("Only the host can load a scene with the synchronize client scenes flag set");
                return;
            }

            PhotonNetwork.LoadLevel(sceneBuildIndex);
        }

        /// <summary>Loads a scene using given scene name. Makes use of NetworkingSettings's synchronizeclientscenes flag to make other clients also load this scene if set to true</summary>
        public static void LoadScene(string sceneName)
        {
            if (settings.SynchronizeClientScenes && !IsHost)
            {
                Debug.LogWarning("Only the host can load a scene with the synchronize client scenes flag set");
                return;
            }

            PhotonNetwork.LoadLevel(sceneName);
        }

        /// <summary>Raises game event of given type, with given content, send at given eventreceivers.</summary>
        public static void RaiseGameEvent(string nameOfEvent, object content, EventReceivers receivers, bool sendReliable = true)
        {
            eventHandler.RaiseGameEvent(nameOfEvent, content, (ReceiverGroup)receivers, sendReliable);
        }

        /// <summary>Raises game event of given type, with given content, send with given eventOptions.</summary>
        public static void RaiseGameEvent(string nameOfEvent, object content, int[] targetActorNumbers, bool sendReliable = true)
        {
            eventHandler.RaiseGameEvent(nameOfEvent, content, targetActorNumbers, sendReliable);
        }

        /// <summary>Raises game request using the name of the request and a target view id of the object the request is about</summary>
        public static void RaiseGameRequest(string nameOfRequest, int targetViewId)
        {
            eventHandler.RaiseGameRequest(nameOfRequest, targetViewId);
        }

        /// <summary>Raises game request using given name of request. Use when request doesn't involve an object in the scene</summary>
        public static void RaiseGameRequest(string nameOfRequest)
        {
            eventHandler.RaiseGameRequest(nameOfRequest);
        }

        /// <summary>Instantiates an object that can be owned by a client accross the network using given prefabName, position and rotation. This can only be a movable networked object</summary>
        public static GameObject InstantiateOwnedObject(string prefabName, Vector3 position, Quaternion rotation)
        {
            if (!settings.IsStaticObject(prefabName))
            {
                return PhotonNetwork.Instantiate(prefabName, position, rotation);
            }
            else
            {
                Debug.LogError("Static objects are part of the scene and not owned :: use InstantiateSceneObject instead");
                return null;
            }
        }

        /// <summary>Instantiates an object owned by the scene accross the network using given prefabName, position and rotation. This can be a static or a movable networked object</summary>
        public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation)
        {
            if (settings.IsStaticObject(prefabName))
            {
                //static objects are spawned using the static objects spawn event type.
                CustomSpawnInfo info = new CustomSpawnInfo(prefabName, position, rotation);
                eventHandler.RaiseEvent((byte)InternalEvent.StaticObjectSpawn, info, ReceiverGroup.All, true);
                return null;
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                //objects spawned as scene object by a non host client will be converted into an event send to the host to spawn it instead
                CustomSpawnInfo info = new CustomSpawnInfo(prefabName, position, rotation);
                int[] targets = new int[] { PhotonNetwork.CurrentRoom.MasterClientId };
                eventHandler.RaiseEvent((byte)InternalEvent.SceneObjectSpawn, info, targets, true);
                return null;
            }

            return PhotonNetwork.InstantiateSceneObject(prefabName, position, rotation);
        }

        /// <summary>Destroys given networkedObject on all clients</summary>
        public static void Destroy(NetworkedObject gameObject)
        {
            if (settings.IsStaticObject(gameObject.name))
            {
                eventHandler.RaiseEvent((byte)InternalEvent.StaticObjectDestroy, gameObject.ViewId, ReceiverGroup.All, true);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject.gameObject);
            }
        }

        /// <summary>
        /// Updates your client property and synchronizes it with other client if you are in a room
        /// </summary>
        /// <typeparam name="T">Type of value (this needs to be a serializable type)</typeparam>
        /// <param name="key">property key</param>
        /// <param name="value">new value you want your property to have</param>
        public static void UpdateClientProperty<T>(string key, T value)
        {
            if (!string.IsNullOrEmpty(key) && PhotonNetwork.InRoom)
            {
                clientHandler.UpdateProperty(key, value);
            }
        }

        /// <summary>
        /// Returns your client property based on given key. Make sure the type matches the key
        /// </summary>
        /// <typeparam name="T">Type of value the property has</typeparam>
        /// <param name="key">Key to search for</param>
        /// <returns></returns>
        public static T GetClientProperty<T>(string key)
        {
            return !string.IsNullOrEmpty(key) ? clientHandler.GetProperty<T>(key) : default;
        }

        /// <summary>
        /// Returns a dictionary containing for each client their properties value based on given key.
        /// </summary>
        /// <typeparam name="T">Type of value the property has</typeparam>
        /// <param name="key">Key to search for</param>
        /// <returns></returns>
        public static Dictionary<int, T> GetPropertiesOfClientsInRoom<T>(string key)
        {
            return !string.IsNullOrEmpty(key) ? clientHandler.GetPropertiesOfPlayersInRoom<T>(key) : default;
        }

        /// <summary>Connects to networking service using the default settings. Set onConnected callback if you want some function to execute when connected</summary>
        public static void ConnectWithDefaultSettings(Action onConnecteToMaster = null, Action onConnected = null)
        {
            matchmakingHandler.StartDefaultConnection();
            if (onConnecteToMaster != null)
            {
                callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.ConnectedToMaster, onConnecteToMaster);
            }
            if (onConnected != null)
            {
                callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.Connected, onConnected);
            }
        }

        /// <summary>Disconnects the client from the server. Set onDisconnect callback if you want some function to execute when disconnected</summary>
        public static void Disconnect(Action onDisconnect = null)
        {
            matchmakingHandler.Disconnect();
            if (onDisconnect != null)
            {
                callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.Disconnected, onDisconnect);
            }
        }

        /// <summary>Creates rooom with given options. Set on created callback if you want to execute some function when creation has finished</summary>
        public static void CreateRoom(string name, int maxPlayers, string key, Action onCreated = null)
        {
            string log = string.Empty;
            if (!matchmakingHandler.CreateRoom(name, maxPlayers, key, ref log))
            {
                Debug.LogWarningFormat("Failed creating room {0} :: {1}", name, log);
            }
            else
            {
                if (onCreated != null)
                {
                    callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.CreatedRoom, onCreated);
                }
            }
        }

        /// <summary>Joins a room with given name, Set on joined to excecute some function when the client has joined the room</summary>
        public static void JoinRoom(string name, Action onJoined = null)
        {
            string log = string.Empty;
            if (!matchmakingHandler.JoinRoom(name, ref log))
            {
                Debug.LogWarningFormat("Failed joining room {0} :: {1}", name, log);
            }
            else
            {
                if (onJoined != null)
                {
                    callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.JoinedRoom, onJoined);
                }
            }
        }

        /// <summary>Makes client leave the current room it is in. return to lobby indicates whether the client should return to the lobby it was in before joining the room</summary>
        public static void LeaveRoom(bool returnToLobby, Action onLefRoom = null)
        {
            string log = string.Empty;
            if (!matchmakingHandler.LeaveRoom(ref log))
            {
                Debug.LogWarningFormat("Failed leaving room :: {0}", log);
            }
            else
            {
                if (onLefRoom != null)
                {
                    callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.LeftRoom, onLefRoom);
                }
                if (returnToLobby)
                {
                    callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.ConnectedToMaster, () => PhotonNetwork.JoinLobby(PhotonNetwork.CurrentLobby));
                }
            }
        }

        /// <summary>Closes the current room the client is in so no other player can join. Make sure this is only called on one client (preferably the host's client)</summary>
        public static void CloseRoom()
        {
            string log = string.Empty;
            if (!roomHandler.CloseRoom(ref log))
            {
                Debug.LogWarningFormat("Failed Closing room :: {0}", log);
            }
        }

        /// <summary>Joins lobby with given name, Set on lobby joined callback if you want to execute some function when joined</summary>
        public static void JoinLobby(string lobbyName, Action onLobbyJoined = null)
        {
            string log = string.Empty;
            if (!matchmakingHandler.JoinLobby(lobbyName, ref log))
            {
                Debug.LogWarningFormat("Failed Joining lobby {0} :: {1}", lobbyName, log);
            }
            else
            {
                if (onLobbyJoined != null)
                {
                    callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.JoinedLobby, onLobbyJoined);
                }
            }
        }

        /// <summary>Leaves the current lobby the client is in. Set onLeftLobby callback if you want to execute some function when left</summary>
        public static void LeaveLobby(Action onLeftLobby = null)
        {
            string log = string.Empty;
            if (!matchmakingHandler.LeaveLobby(ref log))
            {
                Debug.LogWarningFormat("Failed leaving lobby :: {0}", log);
            }
            else
            {
                if (onLeftLobby != null)
                {
                    callbackHandler.AddSingleCallback(MatchmakingCallbackEvent.LeftLobby, onLeftLobby);
                }
            }
        }

        /// <summary>Toggles the offline mode of the networking service to given value</summary>
        public static void SetOffline(bool value)
        {
            string log = string.Empty;
            if (value)
            {
                if (!matchmakingHandler.StartOffline(ref log))
                {
                    Debug.LogWarningFormat("Failed starting offline mode :: {0}", log);
                }
            }
            else
            {
                if (!matchmakingHandler.StopOffline(ref log))
                {
                    Debug.LogWarningFormat("Failed stopping offline mode :: {0}", log);
                }
            }
        }
    }
}