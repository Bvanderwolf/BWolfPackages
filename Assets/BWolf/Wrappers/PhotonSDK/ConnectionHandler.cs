﻿using Photon.Pun;
using Photon.Realtime;

namespace BWolf.Wrappers.PhotonSDK
{
    public class ConnectionHandler
    {
        private readonly TypedLobby alphaLobby = new TypedLobby("Alpha", LobbyType.Default);
        private readonly TypedLobby betaLobby = new TypedLobby("Beta", LobbyType.Default);

        /// <summary>Starts the connection witbh photon using the default settings</summary>
        public void StartDefaultConnection()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public bool JoinLobby(string lobbyName, ref string log)
        {
            if (PhotonNetwork.InLobby)
            {
                log += "Already in a lobby";
                return false;
            }

            switch (lobbyName)
            {
                case "Default": return PhotonNetwork.JoinLobby(TypedLobby.Default);
                case "Alpha": return PhotonNetwork.JoinLobby(alphaLobby);
                case "Beta": return PhotonNetwork.JoinLobby(betaLobby);
            }
            log += "Not a falid lobby name";
            return false;
        }

        /// <summary>Tries starting offline mode ouptutting any problems into the given log string</summary>
        public bool StartOffline(ref string log)
        {
            if (PhotonNetwork.OfflineMode)
            {
                log += "OfflineMode is already true";
                return false;
            }
            else if (PhotonNetwork.IsConnected)
            {
                log += "You can't start offline mode when you are connected";
                return false;
            }
            else
            {
                PhotonNetwork.OfflineMode = true;
                return true;
            }
        }

        /// <summary>Tries stopping offline mode ouptutting any problems into the given log string</summary>
        public bool StopOffline(ref string log)
        {
            if (!PhotonNetwork.OfflineMode)
            {
                log += "OfflineMode is already false";
                return false;
            }
            else
            {
                PhotonNetwork.OfflineMode = false;
                return true;
            }
        }
    }
}