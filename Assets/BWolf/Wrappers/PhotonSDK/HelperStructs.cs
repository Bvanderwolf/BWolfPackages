namespace BWolf.Wrappers.PhotonSDK
{
    public struct LobbyInfo
    {
        public string Name;
        public int PlayerCount;
        public int RoomCount;

        public static LobbyInfo Create(string name, int playerCount, int roomCount)
        {
            LobbyInfo info;
            info.Name = name;
            info.PlayerCount = playerCount;
            info.RoomCount = roomCount;
            return info;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, PlayerCount: {1}, RoomCount: {2}", Name, PlayerCount, RoomCount);
        }
    }
}