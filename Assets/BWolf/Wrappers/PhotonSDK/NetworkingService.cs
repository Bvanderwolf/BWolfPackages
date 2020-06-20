using Photon.Pun;
using System;
using UnityEditor;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    public static class NetworkingService
    {
        private static readonly CallbackHandler callbackHandler = new CallbackHandler();
        private static readonly ConnectionHandler connectionHandler = new ConnectionHandler();

        public static bool IsConnected
        {
            get { return PhotonNetwork.IsConnected; }
        }

        public static bool InOfflineMode
        {
            get { return PhotonNetwork.OfflineMode; }
        }

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

        public static void AddCallbackListener(CallbackEvent callbackEvent, Action<string> callback)
        {
            callbackHandler.AddListener(callbackEvent, callback);
        }

        public static void RemoveCallbackListener(CallbackEvent callbackEvent, Action<string> callback)
        {
            callbackHandler.RemoveListener(callbackEvent, callback);
        }

        /// <summary>Connects to networking service using the default settings</summary>
        public static void ConnectWithDefaultSettings(Action onConnecteToMaster = null, Action onConnected = null)
        {
            connectionHandler.StartDefaultConnection();
            if (onConnecteToMaster != null)
            {
                callbackHandler.AddSingleCallback(CallbackEvent.ConnectedToMaster, onConnecteToMaster);
            }
            if (onConnected != null)
            {
                callbackHandler.AddSingleCallback(CallbackEvent.Connected, onConnected);
            }
        }

        public static void Disconnect(Action onDisconnect = null)
        {
            connectionHandler.Disconnect();
            if (onDisconnect != null)
            {
                callbackHandler.AddSingleCallback(CallbackEvent.Disconnected, onDisconnect);
            }
        }

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
                    callbackHandler.AddSingleCallback(CallbackEvent.JoinedLobby, onLobbyJoined);
                }
            }
        }

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
                    callbackHandler.AddSingleCallback(CallbackEvent.LeftLobby, onLeftLobby);
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