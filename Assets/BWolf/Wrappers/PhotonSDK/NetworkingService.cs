using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    public static class NetworkingService
    {
        private static readonly CallbackHandler callbackHandler = new CallbackHandler();
        private static readonly ConnectionHandler connectionHandler = new ConnectionHandler();

        /// <summary>Returns whether this client is connected to the server or not</summary>
        public static bool IsConnected
        {
            get { return PhotonNetwork.IsConnected; }
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

        static NetworkingService()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif

            PhotonNetwork.AddCallbackTarget(callbackHandler);
        }

        /// <summary>Adds a callback listener for events that either return no value or a string value</summary>
        public static void AddCallbackListener(SimpleCallbackEvent callbackEvent, Action<string> callback)
        {
            callbackHandler.AddListener(callbackEvent, callback);
        }

        /// <summary>Adds a callback listener to the statistics update event to be called when lobby statistics are updated</summary>
        public static void AddLobbyStatisticsListener(Action<List<LobbyInfo>> onUpdate)
        {
            callbackHandler.AddListener(onUpdate);
        }

        /// <summary>Removes callback listener for events that either return no value or a string value</summary>
        public static void RemoveCallbackListener(SimpleCallbackEvent callbackEvent, Action<string> callback)
        {
            callbackHandler.RemoveListener(callbackEvent, callback);
        }

        /// <summary>Removes callback listener from the lobby statistics update event</summary>
        public static void RemoveLobbyStatisticsListener(Action<List<LobbyInfo>> onUpdate)
        {
            callbackHandler.RemoveListener(onUpdate);
        }

        /// <summary>Connects to networking service using the default settings. Set onConnected callback if you want some function to execute when connected</summary>
        public static void ConnectWithDefaultSettings(Action onConnecteToMaster = null, Action onConnected = null)
        {
            connectionHandler.StartDefaultConnection();
            if (onConnecteToMaster != null)
            {
                callbackHandler.AddSingleCallback(SimpleCallbackEvent.ConnectedToMaster, onConnecteToMaster);
            }
            if (onConnected != null)
            {
                callbackHandler.AddSingleCallback(SimpleCallbackEvent.Connected, onConnected);
            }
        }

        /// <summary>Disconnects the client from the server. Set onDisconnect callback if you want some function to execute when disconnected</summary>
        public static void Disconnect(Action onDisconnect = null)
        {
            connectionHandler.Disconnect();
            if (onDisconnect != null)
            {
                callbackHandler.AddSingleCallback(SimpleCallbackEvent.Disconnected, onDisconnect);
            }
        }

        /// <summary>Joins lobby with given name, Set on lobby joined callback if you want to execute some function when joined</summary>
        public static void JoinLobby(string lobbyName, Action onLobbyJoined = null)
        {
            string log = string.Empty;
            if (!connectionHandler.JoinLobby(lobbyName, ref log))
            {
                Debug.LogWarningFormat("Failed Joining lobby {0} :: {1}", lobbyName, log);
            }
            else
            {
                if (onLobbyJoined != null)
                {
                    callbackHandler.AddSingleCallback(SimpleCallbackEvent.JoinedLobby, onLobbyJoined);
                }
            }
        }

        /// <summary>Leaves the current lobby the client is in. Set onLeftLobby callback if you want to execute some function when left</summary>
        public static void LeaveLobby(Action onLeftLobby = null)
        {
            string log = string.Empty;
            if (!connectionHandler.LeaveLobby(ref log))
            {
                Debug.LogWarningFormat("Failed leaving lobby :: {0}", log);
            }
            else
            {
                if (onLeftLobby != null)
                {
                    callbackHandler.AddSingleCallback(SimpleCallbackEvent.LeftLobby, onLeftLobby);
                }
            }
        }

        /// <summary>Toggles the offline mode of the networking service to given value</summary>
        public static void SetOffline(bool value)
        {
            string log = string.Empty;
            if (value)
            {
                if (!connectionHandler.StartOffline(ref log))
                {
                    Debug.LogWarningFormat("Failed starting offline mode :: {0}", log);
                }
            }
            else
            {
                if (!connectionHandler.StopOffline(ref log))
                {
                    Debug.LogWarningFormat("Failed stopping offline mode :: {0}", log);
                }
            }
        }
    }
}