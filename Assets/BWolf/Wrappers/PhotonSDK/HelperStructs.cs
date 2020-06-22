namespace BWolf.Wrappers.PhotonSDK
{
    public struct LobbyData
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        private int playerCount;

        public int PlayerCount
        {
            get { return playerCount; }
        }

        private int roomCount;

        public int RoomCount
        {
            get { return roomCount; }
        }

        /// <summary>Creates a new Lobby Data object without calling a constructor</summary>
        public static LobbyData Create(string name, int playerCount, int roomCount)
        {
            LobbyData data;
            data.name = name;
            data.playerCount = playerCount;
            data.roomCount = roomCount;
            return data;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, PlayerCount: {1}, RoomCount: {2}", Name, PlayerCount, RoomCount);
        }
    }

    public struct RoomData
    {
        private bool removedFromList;

        public bool RemovedFromList
        {
            get { return removedFromList; }
        }

        private string name;

        public string Name
        {
            get { return name; }
        }

        private int playerCount;

        public int PlayerCount
        {
            get { return playerCount; }
        }

        private int maxPlayers;

        public int MaxPlayers
        {
            get { return maxPlayers; }
        }

        private string key;

        public string Key
        {
            get { return key; }
        }

        public bool HasKey
        {
            get { return !string.IsNullOrEmpty(key); }
        }

        public bool IsFull
        {
            get { return playerCount == maxPlayers; }
        }

        public const string PasswordPropertyKey = "RoomPassword";

        /// <summary>Creates a new RoomData object without calling a constructor</summary>
        public static RoomData Create(bool removedFromList, string name, int playerCount, int maxPlayers, string key)
        {
            RoomData data;
            data.removedFromList = removedFromList;
            data.name = name;
            data.playerCount = playerCount;
            data.maxPlayers = maxPlayers;
            data.key = key;
            return data;
        }

        public override string ToString()
        {
            return string.Format("Removed: {0}, Name: {1}, PlayerCount: {2}, MaxPlayers: {3}, key: {4}", removedFromList, name, playerCount, maxPlayers, key);
        }
    }
}