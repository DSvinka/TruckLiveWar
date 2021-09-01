using System;

namespace Code.SaveData
{
    [Serializable]
    internal sealed class GameSaveData
    {
        public PlayerSaveData Player;
        public string LocationNameID;
        
        public override string ToString() =>
            $"<color=red>Player</color> {Player} \n <color=red>LocationNameID</color>{LocationNameID}";

    }
}