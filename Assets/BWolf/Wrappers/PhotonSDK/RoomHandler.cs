using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class RoomHandler
    {
        public readonly Dictionary<int, Client> ClientsInRoom = new Dictionary<int, Client>();

        /// <summary>Returns the actor number of the host when inside a room, if not inside a room returns -1</summary>
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

        /// <summary>Called when having joined a room it adds all players in the room as clients to clients in room</summary>
        private void OnJoinedRoom(string message)
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                Client client = (Client)player;
                ClientsInRoom.Add(client.ActorNumber, client);
            }
        }

        /// <summary>Called when the host changes, it refreshes the clients in room dictionary</summary>
        private void OnHostChanged(Client newHost)
        {
            ClientsInRoom.Clear();
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                Client client = (Client)player;
                ClientsInRoom.Add(client.ActorNumber, client);
            }
        }

        /// <summary>Called when having left a room or when disconnect it clears the clients in room dictionary</summary>
        private void Reset(string message)
        {
            ClientsInRoom.Clear();
        }

        /// <summary>Called when a client has joined the room, it adds it to the clients in room dictionary</summary>
        private void OnClientJoined(Client client)
        {
            ClientsInRoom.Add(client.ActorNumber, client);
        }

        /// <summary>Called when a client has left the room, it removes it from the clients in room dictionary</summary>
        private void OnClientLeft(Client client)
        {
            ClientsInRoom.Remove(client.ActorNumber);
        }

        /// <summary>Called when a client's properties have been changed, it merges them with the client's stored properties in the clients in room dictionary</summary>
        private void OnClientPropertyUpdate(Client client, Dictionary<string, object> properties)
        {
            ClientsInRoom[client.ActorNumber].Properties.Merge(properties);
        }
    }
}