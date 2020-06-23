using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>Helper class managing everything regarding local client data and client properties</summary>
    public class ClientHandler
    {
        public Client LocalClient { get; private set; }

        public ClientHandler()
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
        }

        /// <summary>Called when having joined a room, it creates a new local client</summary>
        public void OnJoinedRoom()
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
        }

        /// <summary>Called when having left a room or disconnected it creates a new local client</summary>
        public void Reset()
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
        }

        /// <summary>Called when the host in a room has been changed it updates the local client if he is the new host</summary>
        public void OnHostChanged(Client newHost)
        {
            if (newHost.IsLocal)
            {
                UpdateLocalClient(PhotonNetwork.LocalPlayer);
            }
        }

        /// <summary>Called when a clients properties in a room have been changed, it updates the local clients properties if the client is our local client</summary>
        public void OnClientPropertyUpdate(Client client, Dictionary<string, object> properties)
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