using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class ClientHandler
    {
        public readonly Client LocalClient;

        public readonly Dictionary<int, Client> ClientsInRoom = new Dictionary<int, Client>();

        public ClientHandler(CallbackHandler callbackHandler)
        {
            LocalClient = new Client(true);
            UpdateClientPlayerData(LocalClient, PhotonNetwork.LocalPlayer);

            callbackHandler.AddListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            callbackHandler.AddListener(SimpleCallbackEvent.LeftRoom, Reset);
            callbackHandler.AddListener(SimpleCallbackEvent.Disconnected, Reset);

            callbackHandler.AddListener(InRoomCallbackEvent.ClientJoined, OnClientJoined);
            callbackHandler.AddListener(InRoomCallbackEvent.ClientLeft, OnClientLeft);

            callbackHandler.AddListener(OnClientPropertyUpdate);
        }

        private void OnJoinedRoom(string message)
        {
            UpdateClientPlayerData(LocalClient, PhotonNetwork.LocalPlayer);

            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                Client client = (Client)player;
                ClientsInRoom.Add(client.ActorNumber, client);
            }
        }

        private void Reset(string message)
        {
            UpdateClientPlayerData(LocalClient, PhotonNetwork.LocalPlayer);
            ClientsInRoom.Clear();
        }

        private void OnClientJoined(Client client)
        {
            ClientsInRoom.Add(client.ActorNumber, client);
        }

        private void OnClientLeft(Client client)
        {
            ClientsInRoom.Remove(client.ActorNumber);
        }

        private void OnClientPropertyUpdate(Client client, Dictionary<string, object> properties)
        {
            ClientsInRoom[client.ActorNumber].Properties.Merge(properties);
        }

        private void UpdateClientPlayerData(Client client, Player player)
        {
            client.SetNickname(player.NickName);
            client.SetActorNumber(player.ActorNumber);
            client.SetIsHost(player.IsMasterClient);
            client.SetProperties(player.CustomProperties);
        }
    }
}