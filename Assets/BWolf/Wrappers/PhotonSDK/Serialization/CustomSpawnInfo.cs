using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK.Serialization
{
    /// <summary>structure containing information about the spawning of a scene owned object or static networked object</summary>
    public struct CustomSpawnInfo
    {
        public string PrefabId;
        public Vector3 Position;
        public Quaternion Rotation;

        public CustomSpawnInfo(string prefabId, Vector3 position, Quaternion rotation)
        {
            PrefabId = prefabId;
            Position = position;
            Rotation = rotation;
        }

        /// <summary>Used for serializing this object to be send over the network</summary>
        public static byte[] Serialize(object obj)
        {
            CustomSpawnInfo info = (CustomSpawnInfo)obj;
            List<byte> bytes = new List<byte>();

            //convert position
            foreach (byte b in BitConverter.GetBytes(info.Position.x)) bytes.Add(b);
            foreach (byte b in BitConverter.GetBytes(info.Position.y)) bytes.Add(b);
            foreach (byte b in BitConverter.GetBytes(info.Position.z)) bytes.Add(b);

            //convert rotation
            foreach (byte b in BitConverter.GetBytes(info.Rotation.x)) bytes.Add(b);
            foreach (byte b in BitConverter.GetBytes(info.Rotation.y)) bytes.Add(b);
            foreach (byte b in BitConverter.GetBytes(info.Rotation.z)) bytes.Add(b);
            foreach (byte b in BitConverter.GetBytes(info.Rotation.w)) bytes.Add(b);

            //convert prefabId
            foreach (char c in info.PrefabId)
            {
                foreach (byte b in BitConverter.GetBytes(c)) { bytes.Add(b); }
            }

            return bytes.ToArray();
        }

        /// <summary>Used for Deserializing this object to be received from the network</summary>
        public static object Deserialize(byte[] data)
        {
            CustomSpawnInfo info;

            //get position
            Vector3 position;
            position.x = BitConverter.ToSingle(data, 0);
            position.y = BitConverter.ToSingle(data, 4);
            position.z = BitConverter.ToSingle(data, 8);
            info.Position = position;

            //get rotation
            Quaternion rotation;
            rotation.x = BitConverter.ToSingle(data, 12);
            rotation.y = BitConverter.ToSingle(data, 16);
            rotation.z = BitConverter.ToSingle(data, 20);
            rotation.w = BitConverter.ToSingle(data, 24);
            info.Rotation = rotation;

            //reconstruct name
            List<char> chars = new List<char>();
            for (int i = 28; i < data.Length; i += 2) { chars.Add(BitConverter.ToChar(data, i)); }
            string prefabId = new string(chars.ToArray());
            info.PrefabId = prefabId;

            return info;
        }
    }
}