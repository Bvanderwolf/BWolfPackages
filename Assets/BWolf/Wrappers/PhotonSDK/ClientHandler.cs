using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class ClientHandler
    {
        public readonly Client LocalClient;

        public ClientHandler(CallbackHandler callbackHandler)
        {
            LocalClient = new Client(true);
            UpdateLocalClient();

            callbackHandler.AddListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            callbackHandler.AddListener(SimpleCallbackEvent.LeftRoom, Reset);
            callbackHandler.AddListener(SimpleCallbackEvent.Disconnected, Reset);

            callbackHandler.AddListener(InRoomCallbackEvent.HostChanged, OnHostChanged);

            callbackHandler.AddListener(OnClientPropertyUpdate);
        }

        private void OnJoinedRoom(string message)
        {
            UpdateLocalClient();
        }

        private void Reset(string message)
        {
            UpdateLocalClient();
        }

        private void OnHostChanged(Client newHost)
        {
            if (newHost.IsLocal)
            {
                UpdateLocalClient();
            }
        }

        private void OnClientPropertyUpdate(Client client, Dictionary<string, object> properties)
        {
            if (client.IsLocal)
            {
                LocalClient.Properties.Merge(properties);
            }
        }

        private void UpdateLocalClient()
        {
            Player p = PhotonNetwork.LocalPlayer;
            LocalClient.SetNickname(p.NickName);
            LocalClient.SetActorNumber(p.ActorNumber);
            LocalClient.SetIsHost(p.IsMasterClient);
            LocalClient.SetProperties(p.CustomProperties);
        }
    }
}