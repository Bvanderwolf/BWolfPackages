using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class ClientHandler
    {
        public readonly Client LocalClient;

        public readonly Dictionary<int, Client> ClientsInRoom = new Dictionary<int, Client>();

        public ClientHandler()
        {
            LocalClient = new Client(true);

            UpdateClient(LocalClient, PhotonNetwork.LocalPlayer);
        }

        public void UpdateClientsInRoom()
        {
            Dictionary<int, Player> playerDict = PhotonNetwork.CurrentRoom.Players;
            foreach (var playerEntry in playerDict)
            {
            }
        }

        private void UpdateClient(Client client, Player player)
        {
            client.SetNickname(player.NickName);
            client.SetActorNumber(player.ActorNumber);
            client.SetIsHost(player.IsMasterClient);
            client.SetProperties(player.CustomProperties);
        }
    }
}