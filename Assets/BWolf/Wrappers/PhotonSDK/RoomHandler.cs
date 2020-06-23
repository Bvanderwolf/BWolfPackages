using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class RoomHandler
    {
        public readonly Dictionary<int, Client> ClientsInRoom = new Dictionary<int, Client>();

        public int ActorNumberOfHost
        {
            get { return PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.MasterClientId : -1; }
        }

        public RoomHandler(CallbackHandler callbackHandler)
        {
            callbackHandler.AddListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            callbackHandler.AddListener(SimpleCallbackEvent.LeftRoom, Reset);
            callbackHandler.AddListener(SimpleCallbackEvent.Disconnected, Reset);

            callbackHandler.AddListener(InRoomCallbackEvent.ClientJoined, OnClientJoined);
            callbackHandler.AddListener(InRoomCallbackEvent.ClientLeft, OnClientLeft);
            callbackHandler.AddListener(InRoomCallbackEvent.HostChanged, OnHostChanged);

            callbackHandler.AddListener(OnClientPropertyUpdate);
        }

        private void OnJoinedRoom(string message)
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                Client client = (Client)player;
                ClientsInRoom.Add(client.ActorNumber, client);
            }
        }

        private void OnHostChanged(Client newHost)
        {
            ClientsInRoom.Clear();
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                Client client = (Client)player;
                ClientsInRoom.Add(client.ActorNumber, client);
            }
        }

        private void Reset(string message)
        {
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
    }
}