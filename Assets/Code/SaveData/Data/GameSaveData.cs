using System;

namespace Code.SaveData.Data
{
    [Serializable]
    internal sealed class GameSaveData
    {
        public PlayerSaveData Player;
        public LocationSaveData Location;
        
        public override string ToString() =>
            $"<color=green>Player</color> \n{Player} \n \n <color=red>Location</color>\n{Location}";

    }
}