using UnityEngine;

namespace GameReady.SaveLoad
{
    public class JsonSaveDataSerializer : ISaveLoadSerializer
    {
        public string Serialize(object data)
        {
            return JsonUtility.ToJson(data);
        }
        
        public void Deserialize(string serializedData, object target)
        {
            JsonUtility.FromJsonOverwrite(serializedData, target);
        }
    }
}