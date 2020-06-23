using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class Client
    {
        public string Nickname { get; private set; }
        public bool IsHost { get; private set; }

        public readonly bool IsLocal;
        public readonly int ActorNumber;
        public readonly Dictionary<string, object> Properties;

        public Client(bool isLocal, int actorNumber)
        {
            IsLocal = isLocal;
            ActorNumber = actorNumber;

            Properties = new Dictionary<string, object>();
        }

        /// <summary>sets the nickname for this client</summary>
        public void SetNickname(string nickname)
        {
            Nickname = nickname;
        }

        /// <summary>Sets the host status for this client</summary>
        public void SetIsHost(bool value)
        {
            IsHost = value;
        }

        /// <summary>Updates the properties of this client</summary>
        public void UpdatesProperties(Hashtable properties)
        {
            Properties.Merge(properties);
        }

        /// <summary>Casts player type to a client type</summary>
        public static explicit operator Client(Player player)
        {
            Client client = new Client(player.IsLocal, player.ActorNumber);
            client.SetNickname(player.NickName);
            client.SetIsHost(player.IsMasterClient);
            client.UpdatesProperties(player.CustomProperties);
            return client;
        }
    }
}