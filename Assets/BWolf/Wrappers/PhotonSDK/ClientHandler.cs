using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class ClientHandler
    {
        public Client LocalClient { get; private set; }

        public ClientHandler(CallbackHandler callbackHandler)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);

            callbackHandler.AddListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            callbackHandler.AddListener(SimpleCallbackEvent.LeftRoom, Reset);
            callbackHandler.AddListener(SimpleCallbackEvent.Disconnected, Reset);

            callbackHandler.AddListener(InRoomCallbackEvent.HostChanged, OnHostChanged);

            callbackHandler.AddListener(OnClientPropertyUpdate);
        }

        /// <summary>Called when having joined a room, it updates the local client</summary>
        private void OnJoinedRoom(string message)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
        }

        /// <summary>Called when having left a room or disconnected it updates the local client</summary>
        private void Reset(string message)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
        }

        /// <summary>Called when the host in a room has been changed it updates the local client if he is the new host</summary>
        private void OnHostChanged(Client newHost)
        {
            if (newHost.IsLocal)
            {
                UpdateLocalClient(PhotonNetwork.LocalPlayer);
            }
        }

        /// <summary>Called when a clients properties in a room have been changed, it updates the local clients properties if the client is our local client</summary>
        private void OnClientPropertyUpdate(Client client, Dictionary<string, object> properties)
        {
            if (client.IsLocal)
            {
                LocalClient.Properties.Merge(properties);
            }
        }

        /// <summary>Updates the local client with photons local player data</summary>
        private void UpdateLocalClient(Player p)
        {
            LocalClient.SetNickname(p.NickName);
            LocalClient.SetIsHost(p.IsMasterClient);
            LocalClient.UpdatesProperties(p.CustomProperties);
        }
    }
}