using System;
using System.Collections.Generic;

namespace BWolf.Wrappers.PhotonSDK.Editables
{
    public struct TurnFinishedInfo
    {
        public int ActorNrOfFinishedClient;
        public int GridIndex;

        public TurnFinishedInfo(int actorNr, int gridIndex)
        {
            ActorNrOfFinishedClient = actorNr;
            GridIndex = gridIndex;
        }

        /// <summary>Used for serializing this object to be send over the network</summary>
        public static byte[] Serialize(object obj)
        {
            TurnFinishedInfo info = (TurnFinishedInfo)obj;
            List<byte> bytes = new List<byte>();
            foreach (byte b in BitConverter.GetBytes(info.ActorNrOfFinishedClient)) bytes.Add(b);
            foreach (byte b in BitConverter.GetBytes(info.GridIndex)) bytes.Add(b);
            return bytes.ToArray();
        }

        /// <summary>Used for Deserializing this object to be received from the network</summary>
        public static object Deserialize(byte[] data)
        {
            TurnFinishedInfo info;
            info.ActorNrOfFinishedClient = BitConverter.ToInt32(data, 0);
            info.GridIndex = BitConverter.ToInt32(data, 4);
            return info;
        }
    }
}