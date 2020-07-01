﻿namespace BWolf.Wrappers.PhotonSDK.Serialization
{
    using BWolf.Examples.PhotonWrapper.Game;
    using ExitGames.Client.Photon;
    using Photon.Realtime;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>static class for registering the default custom types for this wrapper</summary>
    internal class SerializableTypes
    {
        private readonly List<char> usedCodes = new List<char> { 'W', 'V', 'Q', 'P' };

        internal readonly List<Type> SerializedTypes = new List<Type>
        {
            typeof(Player),
            typeof(Vector2),
            typeof(Vector3),
            typeof(Quaternion)
        };

        internal void RegisterCustomTypesInternal()
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