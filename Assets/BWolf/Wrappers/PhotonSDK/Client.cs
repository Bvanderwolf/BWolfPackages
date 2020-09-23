// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>The wrapper class representing a player</summary>
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
            if (!string.IsNullOrEmpty(Nickname) && Nickname.Equals(nickname))
            {
                return;
            }

            Nickname = nickname;

            if (IsLocal)
            {
                //update photon local player aswell if this is our local client
                PhotonNetwork.LocalPlayer.NickName = nickname;
            }
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

        public static bool operator ==(Client lhs, Client rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Client lhs, Client rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(Client p)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(p, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, p))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != p.GetType())
            {
                return false;
            }

            return ActorNumber == p.ActorNumber;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Client);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ActorNumber.GetHashCode();
            }
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