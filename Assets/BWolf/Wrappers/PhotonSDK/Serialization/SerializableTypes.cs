namespace BWolf.Wrappers.PhotonSDK.Serialization
{
    using ExitGames.Client.Photon;
    using Photon.Realtime;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>class for registering the default custom types for this wrapper</summary>
    public class SerializableTypes
    {
        private readonly List<char> usedCodes = new List<char> { 'W', 'V', 'Q', 'P' };

        public readonly List<Type> SerializedTypes = new List<Type>
        {
            //types defined by photon
            typeof(Player),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Quaternion)
        };

        /// <summary>Registers the custom types internally defined by the wrapper</summary>
        public void RegisterCustomTypesInternal()
        {
            RegisterCustomType(typeof(Color), 'C', SerializeColor, DeserializeColor);
            RegisterCustomType(typeof(CustomSpawnInfo), 'S', CustomSpawnInfo.Serialize, CustomSpawnInfo.Deserialize);
        }

        /// <summary>registers a custom serializable type using type, a character and 2 methods for serialization and deserilialization</summary>
        public void RegisterCustomType(Type t, char c, SerializeMethod s, DeserializeMethod d)
        {
            if (usedCodes.Contains(c))
            {
                Debug.LogWarningFormat("Failed registering serializable type with code {0}:: code is already used", c);
                return;
            }

            if (SerializedTypes.Contains(t))
            {
                Debug.LogWarningFormat("Failed registering serializable type {0}:: type is already registered", t);
                return;
            }

            if (!PhotonPeer.RegisterType(t, (byte)c, s, d))
            {
                Debug.LogWarningFormat("Failed registering serializable type with code {0}::  check photon requirements for serialization", c);
                return;
            }

            usedCodes.Add(c);
            SerializedTypes.Add(t);
        }

        /// <summary>Used for serializing color to be send over the network</summary>
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

        /// <summary>Used for Deserializing color to be received from the network</summary>
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