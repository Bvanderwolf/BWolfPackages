// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BWolf.Wrappers.PhotonSDK.Handlers
{
    /// <summary>Helper class managing everything regarding local client data and client properties</summary>
    public class ClientHandler
    {
        public Client LocalClient { get; private set; }

        public const string PlayerColorKey = "PlayerColor";
        public const string PlayerSceneLoaded = "SceneLoaded";

        public ClientHandler()
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
        }

        /// <summary>Updates property with given key with given value</summary>
        public void UpdateProperty<T>(string key, T value)
        {
            Hashtable table = PhotonNetwork.LocalPlayer.CustomProperties;
            if (table.ContainsKey(key))
            {
                table[key] = value;
            }
            else
            {
                table.Add(key, value);
            }
            PhotonNetwork.SetPlayerCustomProperties(table);
        }

        /// <summary>Returns property of given given based on key</summary>
        public T GetProperty<T>(string key)
        {
            Hashtable table = PhotonNetwork.LocalPlayer.CustomProperties;
            return table.ContainsKey(key) ? (T)table[key] : default;
        }

        /// <summary>Returns a dictionary containing for each actor key, the property value</summary>
        public Dictionary<int, T> GetPropertiesOfPlayersInRoom<T>(string key)
        {
            Dictionary<int, T> properties = new Dictionary<int, T>();
            Hashtable table;
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                table = player.Value.CustomProperties;
                properties.Add(player.Key, table.ContainsKey(key) ? (T)table[key] : default);
            }
            return properties;
        }

        /// <summary>Called when having joined a room, it creates a new local client</summary>
        public void OnJoinedRoom()
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            LocalClient = new Client(true, localPlayer.ActorNumber);
            UpdateLocalClient(localPlayer);
            UpdateProperty(PlayerSceneLoaded, SceneManager.GetActiveScene().name);
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