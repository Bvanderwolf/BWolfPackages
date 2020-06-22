using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    public class Client
    {
        public string Nickname { get; private set; }
        public int ActorNumber { get; private set; }
        public bool IsHost { get; private set; }

        public readonly bool IsLocal;
        public readonly Dictionary<string, object> Properties;

        public Client(bool isLocal)
        {
            IsLocal = isLocal;
            Properties = new Dictionary<string, object>();
        }

        public void SetNickname(string nickname)
        {
            Nickname = nickname;
        }

        public void SetActorNumber(int actornumber)
        {
            ActorNumber = actornumber;
        }

        public void SetIsHost(bool value)
        {
            IsHost = value;
        }

        public void SetProperties(Hashtable properties)
        {
            Properties.Merge(properties);
        }
    }
}