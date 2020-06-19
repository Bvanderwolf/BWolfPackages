using System;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    public static class NetworkingService
    {
        private static readonly CallbackHandler callbackHandler = new CallbackHandler();
        private static readonly ConnectionHandler connectionHandler = new ConnectionHandler();

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
        public static void ConnectWithDefaultSettings()
        {
            connectionHandler.StartDefaultConnection();
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