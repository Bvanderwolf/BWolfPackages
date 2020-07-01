namespace BWolf.Wrappers.PhotonSDK.Serialiazation
{
    using BWolf.Examples.PhotonWrapper.Game;
    using ExitGames.Client.Photon;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>static class for registering the default custom types for this wrapper</summary>
    internal static class CustomTypes
    {
        internal static void Register()
        {
            PhotonPeer.RegisterType(typeof(Color), (byte)'C', SerializeColor, DeserializeColor);
            PhotonPeer.RegisterType(typeof(CustomSpawnInfo), (byte)'S', CustomSpawnInfo.Serialize, CustomSpawnInfo.Deserialize);
            PhotonPeer.RegisterType(typeof(TurnFinishedInfo), (byte)'T', TurnFinishedInfo.Serialize, TurnFinishedInfo.Deserialize);
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