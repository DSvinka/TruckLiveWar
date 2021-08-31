using Code.Markers;
using UnityEngine;

namespace Code.SaveData
{
    internal sealed class PlayerPrefsData: IData<SavedData>
    {
        public void Save(SavedData data, string path = null)
        {
            PlayerPrefs.SetFloat("Health", data.Health);
            PlayerPrefs.SetFloat("PosX", data.Position.X);
            PlayerPrefs.SetFloat("PosY", data.Position.Y);
            PlayerPrefs.SetFloat("PosZ", data.Position.Z);
            PlayerPrefs.SetString("IsEnable", data.IsEnabled.ToString());

            PlayerPrefs.Save();
        }

        public SavedData Load(string path = null)
        {
            var result = new SavedData();

            var key = "Health";
            if (PlayerPrefs.HasKey(key))
                result.Health = PlayerPrefs.GetFloat(key);
            
            key = "PosX";
            if (PlayerPrefs.HasKey(key))
                result.Position.X = PlayerPrefs.GetFloat(key);
            
            key = "PosY";
            if (PlayerPrefs.HasKey(key))
                result.Position.Y = PlayerPrefs.GetFloat(key);
            
            key = "PosZ";
            if (PlayerPrefs.HasKey(key))
                result.Position.Z = PlayerPrefs.GetFloat(key);
            
            key = "IsEnable";
            if (PlayerPrefs.HasKey(key))
                bool.TryParse(PlayerPrefs.GetString(key), out result.IsEnabled);

            return result;
        }
    }
}