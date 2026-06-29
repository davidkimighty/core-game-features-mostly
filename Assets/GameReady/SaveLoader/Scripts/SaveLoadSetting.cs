using UnityEngine;

namespace GameReady.SaveLoad
{
    [CreateAssetMenu(fileName = "SaveLoadSetting", menuName = "GameReady/SaveLoad/SaveLoadSetting")]
    public class SaveLoadSetting : ScriptableObject
    {
        public string SaveDirectory;
        public string SaveFileName;
        public string AesKeyName;
        public bool UseCryptoStream;
    }
}
