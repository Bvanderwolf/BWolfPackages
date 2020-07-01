namespace BWolf.Wrappers.PhotonSDK.Serialiazation
{
    using BWolf.Examples.PhotonWrapper.Game;
    using ExitGames.Client.Photon;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>static class for registering the default custom types for this wrapper</summary>
    internal class SerializableTypes
    {
        private List<char> usedCodes = new List<char> { 'W', 'V', 'Q', 'P' };

        private List<Type> serialiableTyeps = new List<Type>()
        {
            typeof(byte),
            typeof(bool),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(string)
        };

        internal void RegisterInternal()
        {
            RegisterCustomType(typeof(Color), 'C', SerializeColor, DeserializeColor);
            RegisterCustomType(typeof(CustomSpawnInfo), 'S', CustomSpawnInfo.Serialize, CustomSpawnInfo.Deserialize);
        }

        internal void RegisterCustomType(Type t, char c, SerializeMethod s, DeserializeMethod d)
        {
            if (usedCodes.Contains(c))
            {
                Debug.LogWarningFormat("Failed registering serializable type with code {0}:: code is already used", c);
                return;
            }

            if (!PhotonPeer.RegisterType(t, (byte)c, s, d))
            {
                Debug.LogWarningFormat("Failed registering serializable type with code {0}::  check photon requirements for serialization", c);
                return;
            }

            usedCodes.Add(c);
        }

        internal void RegisterGameEvent(string nameOfEvent, Type contentType)
        {
        }

        public static byte[] SerializeColor(object obj)
        {
            Color color = (Color)obj;
            List<byte> bytes = new List<byte>();
            foreach (byte b in BitConverter.GetBytes(color.r)) { bytes.Add(b); }
            foreach (byte b in BitConverter.GetBytes(color.g)) { bytes.Add(b); }
            foreach (byte b in BitConverter.GetBytes(color.b)) { bytes.Add(b); }
            foreach (byte b in BitConverter.GetBytes(color.a)) { bytes.Add(b); }
            return bytes.ToArray();
        }

        public static object DeserializeColor(byte[] data)
        {
            Color color;
            color.r = BitConverter.ToSingle(data, 0);
            color.g = BitConverter.ToSingle(data, 4);
            color.b = BitConverter.ToSingle(data, 8);
            color.a = BitConverter.ToSingle(data, 12);
            return color;
        }
    }
}